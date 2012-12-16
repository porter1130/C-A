using CA.SharePoint.Utilities.Common;
using System;
using QuickFlow.Core;
using System.ComponentModel;
using System.Collections.Generic;
using CA.SharePoint;
using QuickFlow;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.EBC
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                

                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                DataForm.IsAttachInvoice = Convert.ToBoolean(fields["IsAttachInvoice"]);
                DataForm.SummaryExpenseType = fields["SummaryExpenseType"].AsString();
                DataForm.ExpatriateBenefitForm = fields["ExpatriateBenefitForm"].AsString();
                DataForm.CashAdvanceAmount = fields["CashAdvanceAmount"].AsString();
                DataForm.CashAdvanceID = fields["CashAdvanceWorkFlowNumber"].AsString();
                DataForm.CashAdvanceIDAndAmount = fields["CashAdvanceIDAndAmount"].AsString();
                DataForm.TotalAmount = fields["TotalAmount"].AsString();

                string userAccount = fields["Applicant"].AsString().Substring(fields["Applicant"].AsString().IndexOf("(") + 1, fields["Applicant"].AsString().IndexOf(")") - fields["Applicant"].AsString().IndexOf("(") - 1);
                Employee employee = UserProfileUtil.GetEmployee(userAccount);
                DataForm.Applicant = employee;
            }
            this.DataForm.DataFormMode = "Edit";

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            this.btnSave.Click += this.btnSave_Click;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string url = Request.UrlReferrer.ToString();
            string specialEmployeelist = ExpatriateBenefitClaimCommon.GetSpecialEmployeeForEBC();
            if (!this.DataForm.Applicant.EmployeeID.StartsWith("2") && !specialEmployeelist.Contains(this.DataForm.Applicant.EmployeeID))
            {
                Response.Write("<script type=\"text/javascript\">alert('The Expatriate Benefit Claim EWF only cover expat employees benefit');window.location = '" + url + "';</script>");
                Response.End();
                return;
            }
            if (this.DataForm.ExpatriateBenefitForm.Length <= 2)
            {
                Response.Write("<script type=\"text/javascript\">alert('Please fill in the Expatriate Benefit Claim Details.');window.location = '" + url + "';</script>");
                Response.End();
                return;
            }
            else
            {
                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                fields["Applicant"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";
                string workflowNumber = fields["WorkflowNumber"].AsString();
               
                #region Save the data
                ExpatriateBenefitClaimCommon.DeleteAllDraftItems(workflowNumber);
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

                context.SaveTask();
                RedirectToTask();
            }
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            string url = Request.UrlReferrer.ToString();
            if (!this.DataForm.Applicant.EmployeeID.StartsWith("2"))
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

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string taskTitle = this.DataForm.Applicant.DisplayName + "'s Expatriate Benefit Claim ";
            fields["Applicant"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";
            string workflowNumber = fields["WorkflowNumber"].AsString();

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

            #region Save the data
            ExpatriateBenefitClaimCommon.DeleteAllDraftItems(workflowNumber);
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

            fields["ReasonsResult"] = "0";
            fields["NoticeResult"] = "0";
            #endregion

            context.UpdateWorkflowVariable("IsSave", false);
            context.DataFields["Status"] = CAWorkflowStatus.InProgress;

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
            SendEmail("Submit");
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
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