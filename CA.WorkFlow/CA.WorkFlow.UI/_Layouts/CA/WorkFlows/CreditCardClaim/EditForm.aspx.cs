using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using QuickFlow;
using QuickFlow.Core;
using System.ComponentModel;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint;
namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (!IsPostBack)
            {
                this.DataForm1.Mode = "Edit";

            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;
            this.DataForm1.RequestId = fields["WorkflowNumber"].ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.DataForm1.CheckInfo()))
            {
                DisplayMessage(this.DataForm1.CheckInfo());
                return;
            }
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            //Save the inputed data to datatable
            //this.DataForm1.Update(); 
            //Save request details to lists
            this.DataForm1.SaveCommonData();
            CreditCardClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            this.DataForm1.SaveDetails(fields["WorkflowNumber"].AsString());
            fields["Applicant"] = this.DataForm1.ApplicantEmployee.DisplayName + "(" + this.DataForm1.ApplicantEmployee.UserAccount + ")";
            fields["ApplicantSPUser"] = this.EnsureUser(this.DataForm1.ApplicantEmployee.UserAccount);
            fields["SaveStatus"] = "SaveStatus";

            //context.SaveTask();
            RedirectToSaveTask();
        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "CreditCardClaim" + emailType;

                var applicant = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(applicant.Substring(applicant.IndexOf('(') + 1, applicant.IndexOf(')') - applicant.IndexOf('(') - 1));
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                //string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/CreditCardClaim/DisplayForm.aspx?List="
                //                                 + Request.QueryString["List"]
                //                                 + "&ID=" + Request.QueryString["ID"];
                string detailLink = rootweburl + "WorkFlowCenter/Lists/CreditCardClaimWorkflow/MyApply.aspx";
                List<string> parameters = new List<string>();
                List<string> to = new List<string>();
                to.Add(applicantAccount);

                switch (emailType)
                {
                    case "Approve":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Reject":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(CurrentEmployee.DisplayName);
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Pending":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(CurrentEmployee.DisplayName);
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Notice":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Submit":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].AsString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string taskTitle = fields["WorkflowNumber"].AsString() + " " + fields["ApproveAmount"].AsString() + " " + this.DataForm1.ApplicantEmployee.DisplayName + "'s Credit Card Claim ";
            #region Set users for workflow
            var managerEmp = WorkFlowUtil.GetNextApprover(this.DataForm1.ApplicantEmployee);
            if (managerEmp == null)
            {
                if (!WorkflowPerson.IsCEO(this.DataForm1.ApplicantEmployee.UserAccount))
                {
                    DisplayMessage("The manager is not set in the system.");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    List<string> cfos = WorkflowPerson.GetCFO();
                    if (cfos.Count == 0)
                    {
                        DisplayMessage("The init error about WorkflowPerson in the system.");
                        e.Cancel = true;
                        return;
                    }
                    managerEmp = UserProfileUtil.GetEmployeeEx(cfos[0]);
                }
            }
            NameCollection manager = new NameCollection();
            manager.Add(managerEmp.UserAccount);
            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimNextApproveTaskUsers, GetDelemanNameCollection(manager, Constants.CAModules.CreditCardClaim));
            //Modify task users
            fields["CurrManager"] = managerEmp.UserAccount;
            //Save the inputed data to datatable
            //this.DataForm1.Update(); 
            //Save request details to lists
            this.DataForm1.SaveCommonData();
            CreditCardClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            this.DataForm1.SaveDetails(fields["WorkflowNumber"].AsString());

            NameCollection wfCFO = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm_CreditCard");
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimNextApproveTask, managerEmp.UserAccount);
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimConfirmTask, wfCFO.JoinString(","));
            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();
            #endregion
            context.UpdateWorkflowVariable("IsSave", false);
            context.DataFields["Status"] = CAWorkflowStatus.InProgress;

            if (this.DataForm1.TravelRequestStatus == "1")
            {
                context.UpdateWorkflowVariable("TravelRequestStatus", true);
                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimConfirmTaskUsers, GetDelemanNameCollection(wfCFO, Constants.CAModules.CreditCardClaim));
            }
            else
            {
                context.UpdateWorkflowVariable("TravelRequestStatus", false);
            }

            //fields["FapiaoReason"] = "";
            //fields["InformationReason"] = "";
            //fields["ClaimedAmtReason"] = "";
            //fields["OtherReasons"] = "";
            fields["ReasonsResult"] = "0";
            fields["NoticeResult"] = "0";

            fields["Applicant"] = this.DataForm1.ApplicantEmployee.DisplayName + "(" + this.DataForm1.ApplicantEmployee.UserAccount + ")";
            fields["ApplicantSPUser"] = this.EnsureUser(this.DataForm1.ApplicantEmployee.UserAccount);

            fields["RMBSummaryExpenseType"] = this.DataForm1.RMBSummaryExpenseType.Trim();
            fields["USDSummaryExpenseType"] = this.DataForm1.USDSummaryExpenseType.Trim();

            this.DataForm1.TerminateWorkflow("EditMode", fields["Title"].AsString());

            this.DataForm1.SaveCommonData();
            CreditCardClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            this.DataForm1.SaveDetails(fields["WorkflowNumber"].AsString());

            #region Set title for workflow
            //Modify task title
            context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Credit Card Claim");
            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "");
            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle + "");
            #endregion
            #region Set page URL for workflow
            //Set page url
            var editURL = "/_Layouts/CA/WorkFlows/CreditCardClaim/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/CreditCardClaim/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormURL", approveURL);
            #endregion

            SendEmail("Submit");

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        public SPUser EnsureUser(string strUser)
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

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}