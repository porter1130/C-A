using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using QuickFlow.UI.Controls;
using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.EBC
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //string url = string.Empty;
                //if (Request.UrlReferrer.AsString() == "")
                //{
                //    url = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]) + "WorkFlowCenter/default.aspx";
                //}
                //else
                //{
                //    url = Request.UrlReferrer.AsString();
                //}
               
                //if(!CurrentEmployee.EmployeeID.StartsWith("2"))
                //{
                //    Response.Write("<script type=\"text/javascript\">alert('The Expatriate Benefit Claim EWF only cover expat employees benefit');window.location = '" + url + "';</script>");
                //    Response.End();
                //    return;
                //}
                DataForm.Applicant = CurrentEmployee;
                DataForm.DataFormMode = "New";
            }
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string url = Request.UrlReferrer.ToString();
            string specialEmployeelist = ExpatriateBenefitClaimCommon.GetSpecialEmployeeForEBC();
            if (!this.DataForm.Applicant.EmployeeID.StartsWith("2") && !specialEmployeelist.Contains(this.DataForm.Applicant.EmployeeID))
            {
                Response.Write("<script type=\"text/javascript\">alert('The Expatriate Benefit Claim EWF only cover expat employees benefit');window.location = '" + url + "';</script>");
                Response.End();
                e.Cancel = true;
                return;
            }

            if (this.DataForm.ExpatriateBenefitForm.Length <= 2)
            {
                Response.Write("<script type=\"text/javascript\">alert('Please fill in the Expatriate Benefit Claim Details.');window.location = '" + url + "';</script>");
                Response.End();
                e.Cancel = true;
                return;
            }

            string taskTitle = this.DataForm.Applicant.DisplayName + "'s Expatriate Benefit Claim ";
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            string workflowNumber = CreateWorkflowNumber();
            fields["WorkflowNumber"] = workflowNumber;
            fields["Applicant"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";

            var btn = sender as StartWorkflowButton;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                context.UpdateWorkflowVariable("IsSave", true);
                context.DataFields["Status"] = CAWorkflowStatus.Pending;
            }
            else
            {
                #region Set users for workflow

                NameCollection wf_EBC_HRDirector = WorkFlowUtil.GetUsersInGroup(ExpatriateBenefitClaimConstants.wf_EBC_HRDirector);
                List<string> cfo = WorkflowPerson.GetCFO();
                if (wf_EBC_HRDirector.Count == 0 || cfo.Count == 0)
                {
                    Response.Write("<script type=\"text/javascript\">alert('The init error about WorkflowPerson in the system.');window.location = '" + url + "';</script>");
                    Response.End();
                    e.Cancel = true;
                }
                List<string> listHRDirector = UserProfileUtil.UserListInGroup(ExpatriateBenefitClaimConstants.wf_EBC_HRDirector);
                var managerEmp = CA.SharePoint.UserProfileUtil.GetEmployeeEx(listHRDirector[0]);
                fields["CurrManager"] = managerEmp.UserAccount;
                context.UpdateWorkflowVariable("NextApproveTaskUsers", GetDelemanNameCollection(wf_EBC_HRDirector, WorkFlowUtil.GetModuleIdByListName("Expatriate Benefit Claim Workflow")));

                NameCollection wf_FinanceConfirm = WorkFlowUtil.GetUsersInGroup(ExpatriateBenefitClaimConstants.wf_EBC_FinanceConfirm);
                System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
                strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimNextApproveTask, wf_EBC_HRDirector);
                strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimConfirmTask, wf_FinanceConfirm.JoinString(","));
                WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

                #endregion

                context.UpdateWorkflowVariable("IsSave", false);
                context.DataFields["Status"] = CAWorkflowStatus.InProgress;

                SendEmail("Submit");
            }

            #region Save the data
            
            ExpatriateBenefitClaimCommon.SaveDetails(this.DataForm.ExpatriateBenefitForm, workflowNumber);

            fields["ApplicantSPUser"] = this.EnsureUser(this.DataForm.Applicant.UserAccount);
            fields["Department"] = this.DataForm.Applicant.Department;

            fields["CashAdvanceAmount"] = this.DataForm.CashAdvanceAmount;
            fields["CashAdvanceWorkFlowNumber"] = this.DataForm.CashAdvanceID;
            fields["CashAdvanceIDAndAmount"] = this.DataForm.CashAdvanceIDAndAmount;
            fields["IsAttachInvoice"] = this.DataForm.IsAttachInvoice;
            fields["SummaryExpenseType"] = this.DataForm.SummaryExpenseType;
            fields["ExpatriateBenefitForm"] = this.DataForm.ExpatriateBenefitForm;

            var totalAmount = decimal.Parse(this.DataForm.TotalAmount);
            var cashAdvanceAmount = decimal.Parse(this.DataForm.CashAdvanceAmount);
            fields["TotalAmount"] = Math.Round(totalAmount, 2).ToString();

            var amountDue = Math.Round(totalAmount - cashAdvanceAmount, 2);
            fields["AmountDue"] = amountDue.ToString();
            #endregion

            #region Set title for workflow
            context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Expatriate Benefit Claim");
            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle );
            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle );
            #endregion

            #region Set page URL for workflow
            var editURL = "/_Layouts/CA/WorkFlows/ExpatriateBenefitClaim/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/ExpatriateBenefitClaim/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormURL", approveURL);
            #endregion
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            });
            return user;
        }

        private static string CreateWorkflowNumber()
        {
            return "EBC" + WorkFlowUtil.CreateWorkFlowNumber("ExpatriateBenefitClaim").ToString("000000");
        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "ExpatriateBenefitClaim" + emailType;

                var applicant = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(applicant.Substring(applicant.IndexOf('(') + 1, applicant.IndexOf(')') - applicant.IndexOf('(') - 1));
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                //string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/ExpatriateBenefitClaim/DisplayForm.aspx?List="
                //                                 + Request.QueryString["List"]
                //                                 + "&ID=" + Request.QueryString["ID"];
                string detailLink = rootweburl + "WorkFlowCenter/Lists/ExpatriateBenefitClaimWorkflow/MyApply.aspx";
                List<string> parameters = new List<string>();
                List<string> to = new List<string>();
                to.Add(applicantAccount);

                switch (emailType)
                {
                    case "Submit":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.logError(string.Format("Expatriate Benefit Claim Form：{0}\nError：{1}", WorkflowContext.Current.DataFields["WorkflowNumber"].ToString(), ex.Message));
            }
        }

    }
}