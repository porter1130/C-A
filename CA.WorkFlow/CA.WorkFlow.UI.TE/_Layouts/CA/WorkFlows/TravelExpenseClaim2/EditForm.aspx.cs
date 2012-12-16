namespace CA.WorkFlow.UI.TE
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
            if (!Page.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.DataForm.RequestId = fields["WorkflowNumber"].AsString();
                this.DataForm.TrWorkflowNumber = fields["TRWorkflowNumber"].AsString();
                this.DataForm.Mode = "Edit";
                this.DataForm.Purpose = fields["Purpose"].AsString();
                this.DataForm.TotalCost = fields["TotalCost"].AsString();
                this.DataForm.CashAdvanced = fields["CashAdvanced"].AsString() == "" ? "0" : fields["CashAdvanced"].AsString();
                this.DataForm.PaidByCreditCard = fields["PaidByCreditCard"].AsString();
                this.DataForm.NetPayable = fields["NetPayable"].AsString();
                this.DataForm.TotalExceptFlight = fields["TotalExceptFlight"].AsString();
                this.DataForm.ComparedToApproved = fields["ComparedToApproved"].AsString();
                this.DataForm.Reasons = fields["Reasons"].AsString();
                this.DataForm.SupportingSubmitted = fields["SupportingSubmitted"].AsString();
                this.DataForm.SubmissionDate = fields["SubmissionDate"].AsString();
                this.DataForm.FinanceRemark = fields["FinanceRemark"].AsString();

                this.DataForm.HotelForm = fields["HotelForm"].AsString();
                this.DataForm.MealAllowanceForm = fields["MealAllowanceForm"].AsString();
                this.DataForm.TransportationForm = fields["TransportationForm"].AsString();
                this.DataForm.SamplePurchaseForm = fields["SamplePurchaseForm"].AsString();
                this.DataForm.OtherForm = fields["OtherForm"].AsString();
            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            #region Save the data
            TravelExpenseClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            TravelExpenseClaimCommon.SaveDetails(this.DataForm, fields["WorkflowNumber"].AsString()); //Save request details to lists
            #endregion
            #region Save Form
            fields["Purpose"] = this.DataForm.Purpose;
            fields["TotalCost"] = this.DataForm.TotalCost;
            fields["CashAdvanced"] = this.DataForm.CashAdvanced;
            fields["PaidByCreditCard"] = this.DataForm.PaidByCreditCard;
            fields["NetPayable"] = this.DataForm.NetPayable;
            fields["TotalExceptFlight"] = this.DataForm.TotalExceptFlight;
            fields["ComparedToApproved"] = this.DataForm.ComparedToApproved;
            fields["Reasons"] = this.DataForm.Reasons;
            fields["SupportingSubmitted"] = this.DataForm.SupportingSubmitted;
            fields["SubmissionDate"] = this.DataForm.SubmissionDate;
            fields["FinanceRemark"] = this.DataForm.FinanceRemark;

            fields["HotelForm"] = this.DataForm.HotelForm;
            fields["MealAllowanceForm"] = this.DataForm.MealAllowanceForm;
            fields["TransportationForm"] = this.DataForm.TransportationForm;
            fields["SamplePurchaseForm"] = this.DataForm.SamplePurchaseForm;
            fields["OtherForm"] = this.DataForm.OtherForm;

            fields["HotelSubTotal"] = this.DataForm.HotelSubTotal;
            fields["MealSubTotal"] = this.DataForm.MealSubTotal;
            fields["TransSubTotal"] = this.DataForm.TransSubTotal;
            fields["SampleSubTotal"] = this.DataForm.SampleSubTotal;
            fields["OthersSubTotal"] = this.DataForm.OthersSubTotal;
            #endregion
            context.SaveTask();
            RedirectToSaveTask();
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
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
            //string taskTitle = fields["EnglishName"].AsString() + "'s Travel Expense ";
            string taskTitle = string.Format("{0} {1} {2}'s Travel Expense ", fields["WorkflowNumber"].AsString(), this.DataForm.TotalCost, fields["EnglishName"].AsString());
            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle );
            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle );
            this.SendEmail("SubmitToApplicant");
            #region Save the data
            TravelExpenseClaimCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
            TravelExpenseClaimCommon.SaveDetails(this.DataForm, fields["WorkflowNumber"].AsString()); //Save request details to lists
            #endregion
            #region Save Form
            fields["Purpose"] = this.DataForm.Purpose;
            fields["TotalCost"] = this.DataForm.TotalCost;
            fields["CashAdvanced"] = this.DataForm.CashAdvanced;
            fields["PaidByCreditCard"] = this.DataForm.PaidByCreditCard;
            fields["NetPayable"] = this.DataForm.NetPayable;
            fields["TotalExceptFlight"] = this.DataForm.TotalExceptFlight;
            fields["ComparedToApproved"] = this.DataForm.ComparedToApproved;
            fields["Reasons"] = this.DataForm.Reasons;
            fields["SupportingSubmitted"] = this.DataForm.SupportingSubmitted;
            fields["SubmissionDate"] = this.DataForm.SubmissionDate;
            fields["FinanceRemark"] = this.DataForm.FinanceRemark;

            fields["HotelForm"] = this.DataForm.HotelForm;
            fields["MealAllowanceForm"] = this.DataForm.MealAllowanceForm;
            fields["TransportationForm"] = this.DataForm.TransportationForm;
            fields["SamplePurchaseForm"] = this.DataForm.SamplePurchaseForm;
            fields["OtherForm"] = this.DataForm.OtherForm;

            fields["HotelSubTotal"] = this.DataForm.HotelSubTotal;
            fields["MealSubTotal"] = this.DataForm.MealSubTotal;
            fields["TransSubTotal"] = this.DataForm.TransSubTotal;
            fields["SampleSubTotal"] = this.DataForm.SampleSubTotal;
            fields["OthersSubTotal"] = this.DataForm.OthersSubTotal;
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

        protected new void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);
        }

    }
}