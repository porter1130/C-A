
namespace CA.WorkFlow.UI.TravelExpenseClaim
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using Microsoft.SharePoint;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;
    using CA.SharePoint;
    using System.Globalization;
    using System.Data;

    public partial class ApproveForm : CAWorkFlowPage
    {
        private string _step;

        public string Step
        {
            get { return _step; }
            set { _step = value; }
        }

        private bool _isPending;

        public bool IsPending
        {
            get { return _isPending; }
            set { _isPending = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSecurity();

            this.DataForm1.Mode = "Approve";

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
            this.TaskTrace1.Applicant = fields["Applicant"].AsString();
            this.DataForm1.Step = WorkflowContext.Current.Step;
            this.Step = WorkflowContext.Current.Step;

            SetViewStyle();

            this.Actions.OnClientClick = "return dispatchAction(this);";

        }

        private void SetViewStyle()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            switch (WorkflowContext.Current.Step)
            {
                case "ConfirmTask":
                    if (fields["IsNotice"].AsString().Equals("True", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.btnNotice.Enabled = false;
                    }
                    else
                    {
                        this.btnNotice.Enabled = true;
                    }

                    if (fields["IsPending"].AsString().Equals("True", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.IsPending = true;
                    }
                    else
                    {
                        this.IsPending = false;
                    }
                    break;
                default:
                    this.btnNotice.Visible = false;
                    break;
            }
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            switch (WorkflowContext.Current.Step)
            {
                case "NextApproveTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                        fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                        if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                        }

                        //Save SpecialApprove Info
                        this.DataForm1.SetSpecialApprove(fields["WorkflowNumber"].AsString());

                        var levelType = "Payment Approval Limits";
                        double total = Convert.ToDouble(fields["TotalCost"].ToString());
                        var quota = WorkFlowUtil.GetQuota(fields["CurrManager"].ToString(), levelType);
                        if (total > quota)
                        {
                            #region Set users for workflow
                            //Modify task users
                            var manager = new NameCollection();

                            var managerEmp = WorkFlowUtil.GetNextApprover(fields["CurrManager"].ToString());

                            //if it's CEO
                            if (managerEmp == null)
                            {
                                List<string> ceos = WorkflowPerson.GetCEO();
                                if (ceos.Count == 0)
                                {
                                    DisplayMessage("The init error about WorkflowPerson in the system.");
                                    e.Cancel = true;
                                    return;
                                }
                                managerEmp = UserProfileUtil.GetEmployeeEx(ceos[0]);
                            }

                            //Get Task users include deleman
                            TravelExpenseClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);

                            fields["CurrManager"] = managerEmp.UserAccount;

                            WorkflowContext.Current.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                            WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", true);
                            #endregion
                        }
                        else
                        {
                            //set last approve date
                            fields["SubmissionDate"] = DateTime.Now.ToLocalTime();

                            WorkflowContext.Current.UpdateWorkflowVariable("ConfirmTaskUsers", TravelExpenseClaimCommon.GetTaskUsersWithoutDeleman("wf_FinanceConfirm"));
                            WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", false);
                        }
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.Rejected;


                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Travel Expense Claim");
                        WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", false);

                        this.SendEmail("Reject");
                    }
                    break;
                case "ConfirmTask":
                    string errorMessage = string.Empty;

                    switch (e.Action)
                    {
                        case "Confirm":

                            this.DataForm1.SaveCommonListData();

                            //if (TravelExpenseClaimCommon.IsLastTask(Request.QueryString["List"], Request.QueryString["ID"]))
                            //{
                            fields["Status"] = CAWorkflowStatus.Completed;
                            //WorkflowContext.Current.UpdateWorkflowVariable("IsConfirm", true);
                            //}
                            //fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", UserProfileUtil.UserListInGroup("wf_FinanceConfirm").ToArray());
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                            fields["Approvers"] = ReturnAllApprovers(SPContext.Current.Web.CurrentUser.LoginName);

                            StartSAPWorkflow(fields);
                            break;
                        case "Reject":
                            errorMessage = this.DataForm1.ValidatePendingForm();
                            if (errorMessage.IsNotNullOrWhitespace())
                            {
                                DisplayMessage(errorMessage);
                                return;
                            }

                            if (!Validate(e.Action, e))
                            {
                                return;
                            }
                            fields["Status"] = CAWorkflowStatus.TE_Finance_Reject;

                            this.DataForm1.SaveCommonListData();
                            this.DataForm1.SavePendingForm();

                            TravelExpenseClaimCommon.SwitchControl("Pending and Notice", true);
                            context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Travel Expense Claim");
                            this.SendEmail("Reject");

                            break;

                        case "Pending":

                            errorMessage = this.DataForm1.ValidatePendingForm();
                            if (errorMessage.IsNotNullOrWhitespace())
                            {
                                DisplayMessage(errorMessage);
                                e.Cancel = true;
                                return;
                            }

                            fields["Status"] = CAWorkflowStatus.TE_Finance_Pending;

                            this.DataForm1.SaveCommonListData();
                            this.DataForm1.SavePendingForm();

                            WorkflowContext.Current.UpdateWorkflowVariable("ConfirmTaskUsers", TravelExpenseClaimCommon.GetTaskUsersWithoutDeleman("wf_FinanceConfirm"));
                            this.SendEmail("Pending");

                            TravelExpenseClaimCommon.SwitchControl("Pending", false);
                            WorkflowContext.Current.SaveListItem();
                            break;

                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private void StartSAPWorkflow(WorkflowDataFields fields)
        {
            SPList list = SPContext.Current.Web.Lists[WorkflowListName.TravelExpenseClaimForSAP];
            SPListItem item = list.Items.Add();

            List<string> fieldsList = new List<string>() { "ApplicantSPUser",
                                                           "EnglishName"};

            string SAPWorkflowNumber = TravelExpenseClaimCommon.SaveListFields(fields, item, fieldsList);

            fieldsList.Clear();
            fieldsList = new List<string>(){"ExpenseType",
                                            "CostCenter",
                                            "RmbAmt",
                                            "ApprovedRmbAmt"
                                            };

            DataTable detailsDataTable = WorkFlowUtil.GetDataTableSource(fields["WorkflowNumber"].AsString(), WorkflowListName.TravelExpenseClaimDetails, fieldsList);
            WorkFlowUtil.BatchInsertDataTable(WorkflowListName.TravelExpenseClaimDetailsForSAP, SAPWorkflowNumber, detailsDataTable, null);
            item["Status"] = CAWorkflowStatus.InProgress;

            item.Update();

            WorkflowVariableValues vs = new WorkflowVariableValues();
            vs["ConfirmTaskFormURL"] = "/_Layouts/CA/Workflows/TravelExpenseClaimForSAP/NewForm.aspx";
            vs["ConfirmTaskTitle"] = fields["WorkflowNumber"].AsString() + ":Please confirm the Travel Expense Claim Info";
            vs["ConfirmTaskUsers"] = TravelExpenseClaimCommon.GetTaskUsersWithoutDeleman(WorkflowGroupName.WF_Accountants);
            vs["IsConfirm"] = true;

            var eventData = SerializeUtil.Serialize(vs);

            var wfAss = list.WorkflowAssociations.GetAssociationByName(WorkflowConfigName.TravelExpenseClaimForSAP,
                                                            CultureInfo.CurrentCulture);
            SPContext.Current.Site.WorkflowManager.StartWorkflow(item, wfAss, eventData);
            WorkFlowUtil.UpdateWorkflowPath(item, eventData);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        protected void btnNotice_Click(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (IsLimitApprove(fields["WorkflowNumber"].AsString()))
            {
                this.SendEmail("LimitApprove");
            }
            else
            {
                this.SendEmail("Approve");
            }
            this.btnNotice.Enabled = false;
            this.DataForm1.SaveCommonListData();
            this.DataForm1.SavePendingForm();
            TravelExpenseClaimCommon.SwitchControl("Notice", false);
            WorkflowContext.Current.SaveListItem();
        }


        private bool Validate(string action, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {
                DisplayMessage(this.DataForm1.MSG);
                e.Cancel = true;
                flag = false;
            }
            return flag;
        }

        private void CheckSecurity()
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true))
            {
                RedirectToTask();
            }
        }

        private bool IsLimitApprove(string workflowNumber)
        {
            SPListItemCollection tcDetailsItems = TravelExpenseClaimCommon.GetDataCollection(workflowNumber, "Travel Expense Claim Details");
            foreach (SPListItem item in tcDetailsItems)
            {
                if (Convert.ToBoolean(item["SpecialApprove"]))
                {
                    if (!Convert.ToBoolean(item["SpecialApproved"]))
                    {
                        return true;
                    }
                }
            }
            return false;
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

                    detailLink = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/MyApply.aspx";
                    parameters[3] = detailLink;

                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "Approve":

                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "Reject":
                    to.Clear();
                    to.Add(applicantAccount);
                    parameters.Clear();
                    parameters.Add("");
                    parameters.Add(fields["WorkflowNumber"].AsString());
                    parameters.Add(CurrentEmployee.DisplayName);
                    parameters.Add(detailLink);
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "LimitApprove":
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "Pending":
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;

                case "Notice":
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;
                default:
                    break;
            }
        }

    }
}