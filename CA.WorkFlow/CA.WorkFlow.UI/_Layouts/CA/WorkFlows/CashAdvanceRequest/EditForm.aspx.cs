using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using QuickFlow.Core;
using Microsoft.SharePoint;
using QuickFlow;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckSecurity();
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                Employee employee = UserProfileUtil.GetEmployee(fields["Applicant"].ToString());
                this.DataForm1.Applicant = employee;
            }
            this.btnSave.Click += this.btnSave_Click;
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "CashAdvanceRequest" + emailType;

                var applicantAccount = fields["Applicant"].AsString();

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                //string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/CashAdvanceRequest/DisplayForm.aspx?List="
                //                                 + Request.QueryString["List"]
                //                                 + "&ID=" + Request.QueryString["ID"];
                string detailLink = rootweburl + "WorkFlowCenter/Lists/CashAdvanceRequest/MyApply.aspx";
                List<string> parameters = new List<string>();
                List<string> to = new List<string>();
                to.Add(applicantAccount);

                switch (emailType)
                {
                    case "Approve":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].AsString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Reject":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].AsString());
                        parameters.Add(CurrentEmployee.DisplayName);
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

        private void CheckSecurity()
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }
        }
        private void DataBindFields(SPListItem curItem)
        {
            curItem["Company"] = this.DataForm1.Company;
            curItem["Department"] = this.DataForm1.Department;
            curItem["Applicant"] = this.DataForm1.Applicant.UserAccount;
            curItem["ApplicantSPUser"] = this.EnsureUser(this.DataForm1.Applicant.UserAccount);
            curItem["Purpose"] = this.DataForm1.Purpose;
            curItem["Amount"] = this.DataForm1.Amount;
            curItem["Term"] = this.DataForm1.Term;
            curItem["CashAdvanceType"] = this.DataForm1.CashAdvanceType;
            curItem["Remark"] = this.DataForm1.Remark;
            curItem["UrgentRemark"] = this.DataForm1.UrgentRemark;
            curItem.Update();
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem curItem = SPContext.Current.ListItem;
            DataBindFields(curItem);
            RedirectToSaveTask();
        }
        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            //get next approver
            //get next approver
            string errorMsg = this.DataForm1.CheckInfo();
            if (errorMsg.IsNotNullOrWhitespace())
            {
                DisplayMessage(errorMsg);
                e.Cancel = true;
                return;
            }
            var managerEmp = WorkFlowUtil.GetNextApprover(this.DataForm1.Applicant);
            if (managerEmp == null)
            {
                if (!WorkflowPerson.IsCEO(this.DataForm1.Applicant.UserAccount))
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
                    managerEmp = CA.SharePoint.UserProfileUtil.GetEmployeeEx(cfos[0]);








































                }
            }
            NameCollection manager = new NameCollection();
            manager.Add(managerEmp.UserAccount);
            fields["CurrManager"] = managerEmp.UserAccount;
            NameCollection wfCFO = WorkFlowUtil.GetUsersInGroup("wf_Finace_FC");
            NameCollection wfFinaceConfirm = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            if (wfCFO.Contains(CurrentEmployee.UserAccount) && this.DataForm1.CashAdvanceType == "Urgent")
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsFinance", true);

            }
            else
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsFinance", false);
            }
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTask, manager.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceFinaceApprove, wfCFO.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceFinaceConfirmEnd, wfFinaceConfirm.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceFinaceConfirm, wfFinaceConfirm.JoinString(","));
            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();
            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(manager, Constants.CAModules.CashAdvanceRequest));
            context.UpdateWorkflowVariable("IsContinue", true);
            string strNextTaskTitle = string.Format("{0} {1} {2}'s Cash Advance Request", fields["WorkflowNumber"].AsString(), this.DataForm1.Amount, this.DataForm1.Applicant.DisplayName);
            context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle);
            context.UpdateWorkflowVariable("IsSave", false);
            context.UpdateWorkflowVariable("IsUrgent", false);
            if (this.DataForm1.CashAdvanceType == "Urgent")
            {
                context.UpdateWorkflowVariable("IsUrgent", true);
            }
            fields["Status"] = CAWorkflowStatus.InProgress;

            var editURL = "/_Layouts/CA/WorkFlows/CashAdvanceRequest/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/CashAdvanceRequest/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            SPListItem curItem = SPContext.Current.ListItem;
            DataBindFields(curItem);
            SendEmail("Submit");

            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}

