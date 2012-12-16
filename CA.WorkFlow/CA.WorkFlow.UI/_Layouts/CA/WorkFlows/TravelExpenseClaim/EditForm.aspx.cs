
namespace CA.WorkFlow.UI.TravelExpenseClaim
{
    using System;
    using System.ComponentModel;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Collections.Generic;
    using System.Web.UI;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSecurity();

            this.DataForm1.Mode = "Edit";

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
            this.DataForm1.TrWorkflowNumber = fields["TRWorkflowNumber"].AsString();


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

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (!this.DataForm1.ValidateForSave())
            //{
            //    DisplayMessage(this.DataForm1.MSG);
            //    return;
            //}

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            #region Save the data
            this.DataForm1.Update(); //Save the inputed data to datatable
            TravelExpenseClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            TravelExpenseClaimCommon.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString()); //Save request details to lists
            #endregion

            context.SaveTask();
            RedirectToSaveTask();

        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            var msg = this.DataForm1.ValidateForSubmit();
            if (msg.IsNotNullOrWhitespace())
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            #region Set users for workflow
            var manager = new NameCollection();
            SPUser applicantUser = SPContext.Current.Web.AllUsers.GetByID(GetApplicantSPUserID(fields["ApplicantSPUser"].AsString()));
            var managerEmp = WorkFlowUtil.GetNextApprover(applicantUser.LoginName);
            if (managerEmp == null)
            {
                if (!WorkflowPerson.IsCEO(applicantUser.LoginName))
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

            //Get Task users include deleman
            TravelExpenseClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);

            fields["CurrManager"] = managerEmp.UserAccount;
            fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());

            context.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
            #endregion


            //Set NextApproveTask title for workflow    
            string taskTitle = string.Format("{0} {1} {2}'sTravel Expense", fields["WorkflowNumber"].AsString(), this.DataForm1.TotalCost, fields["EnglishName"].AsString());
            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "needs approval");
            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle + "needs confirm");

            this.SendEmail("SubmitToApplicant");

            #region Save the data
            this.DataForm1.Update(); //Save the inputed data to datatable
            TravelExpenseClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            TravelExpenseClaimCommon.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString()); //Save request details to lists
            #endregion

            context.UpdateWorkflowVariable("IsSave", false);
            if (!fields["Status"].AsString().Equals(CAWorkflowStatus.TE_Finance_Reject, StringComparison.CurrentCultureIgnoreCase))
            {
                fields["Status"] = CAWorkflowStatus.InProgress;
            }

            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private int GetApplicantSPUserID(string applicantSPUser)
        {
            CommonUtil.logInfo(applicantSPUser);
            string userId = applicantSPUser.Split(new string[] { ";#" }, StringSplitOptions.None)[0];
            return int.Parse(userId);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        protected void btnTRNumber_Click(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string url = TravelExpenseClaimCommon.GetRedirectTRListItemUrl(fields["TRWorkflowNumber"].AsString());
            Response.Redirect(url);
        }

        private void SendEmail(string type)
        {

            var fields = WorkflowContext.Current.DataFields;
            var templateTitle = "TravelExpenseClaim" + type;

            var applicantStr = fields["Applicant"].AsString();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            var applicantName = fields["EnglishName"].AsString();

            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/TravelExpenseClaim/DisplayForm.aspx?List="
         + Request.QueryString["List"]
         + "&ID=" + Request.QueryString["ID"];

            List<string> parameters = new List<string>();
            parameters.Add("");
            parameters.Add(fields["WorkflowNumber"].AsString());
            parameters.Add(detailLink);
            List<string> to = new List<string>();
            to.Add(applicantAccount);

            switch (type)
            {
                case "SubmitToApplicant":

                    detailLink = rootweburl + "CA/MyTasks.aspx";
                    parameters[2] = detailLink;

                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "Approve":

                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "Reject":
                    to.Clear();
                    to.Add(applicantAccount);
                    parameters.Add("");
                    parameters.Add(fields["WorkflowNumber"].AsString());
                    parameters.Add(CurrentEmployee.DisplayName);
                    parameters.Add(detailLink);
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "LimitApprove":
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                default:
                    break;
            }
        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }
    }
}