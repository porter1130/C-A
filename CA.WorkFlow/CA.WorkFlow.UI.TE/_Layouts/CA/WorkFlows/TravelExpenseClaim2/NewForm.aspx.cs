namespace CA.WorkFlow.UI.TE
{
    using System;
    using System.ComponentModel;
    using QuickFlow.UI.Controls;
    using QuickFlow.Core;
    using QuickFlow;
    using CA.SharePoint;
    using System.Web.UI;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using System.Data;
    using System.Configuration;
    using System.Collections.Generic;

    public partial class NewForm : CAWorkFlowPage
    {
        string workflowNumber = string.Empty;
        string requestId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            requestId = Server.UrlDecode(Request.QueryString["TRNumber"]);
            if (!this.IsPostBack)
            {
                this.DataForm.RequestId = requestId;
                this.DataForm.Mode = "New";
            }
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                string taskTitle = string.Empty;
                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                var btn = sender as StartWorkflowButton;
                #region Save Common List Data
                List<string> fieldsList = new List<string>() 
                                               {"Applicant",
                                                "ApplicantSPUser",
                                                "ChineseName",
                                                "Department",
                                                "EnglishName",
                                                "Mobile",
                                                "OfficeExt",
                                                "IDNumber"};
                workflowNumber = TravelExpenseClaimCommon.SaveListFields(requestId, "Travel Request Workflow2", fields, fieldsList);
                #endregion
                if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
                {
                    context.UpdateWorkflowVariable("IsSave", true);
                    fields["Status"] = CAWorkflowStatus.Pending;
                    context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Travel Expense Claim");
                }
                else
                {
                    #region Set Users for Workflow
                    var manager = new NameCollection();
                    SPUser applicantUser = SPContext.Current.Web.AllUsers.GetByID(Convert.ToInt32(fields["ApplicantSPUser"].AsString()));
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
                    TravelExpenseClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);
                    fields["CurrManager"] = managerEmp.UserAccount;
                    WorkflowContext.Current.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                    WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                    #endregion
                    //Set NextApproveTask title for workflow    
                    //taskTitle = fields["EnglishName"].AsString() + "'s Travel Expense ";
                    taskTitle = string.Format("{0} {1} {2}'s Travel Expense ", fields["WorkflowNumber"].AsString(), this.DataForm.TotalCost, fields["EnglishName"].AsString());
                    context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle );
                    context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle );
                    context.UpdateWorkflowVariable("IsSave", false);
                    fields["Status"] = CAWorkflowStatus.InProgress;
                    this.SendEmail("SubmitToApplicant");
                }
                #region Save Details
                //Save the inputed data to datatable
                TravelExpenseClaimCommon.SaveDetails(this.DataForm, fields["WorkflowNumber"].AsString());
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
                #region Set page URL for workflow
                //Set page url
                var editURL = "/_Layouts/CA/WorkFlows/TravelExpenseClaim2/EditForm.aspx";
                var approveURL = "/_Layouts/CA/WorkFlows/TravelExpenseClaim2/ApproveForm.aspx";
                context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
                context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
                context.UpdateWorkflowVariable("ConfirmTaskFormURL", approveURL);
                #endregion
                WorkFlowUtil.UpdateWorkflowPath(context);
                #region Update Travel Request Claim Link
                SPFieldUrlValue link = new SPFieldUrlValue();
                link.Description = "Closed";
                var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                link.Url = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/MyApply.aspx";
                SPList list = SPContext.Current.Web.Lists["Travel Request Workflow2"];
                foreach (SPListItem item in list.Items)
                {
                    if (item["WorkflowNumber"].ToString() == fields["TRWorkflowNumber"].ToString())
                    {
                        item["Claim"] = link;
                        item.Update();
                    }
                }
                #endregion
                e.Cancel = false; 
            }
            catch (Exception exception)
            {
                e.Cancel = true;
                CommonUtil.logError(string.Format("Travel Expense Claim :: {0}", exception.Message));
                throw new Exception(exception.Message);
            }
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
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
                    parameters[2] = detailLink;
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
                default:
                    break;
            }
        }

        protected void btnTRNumber_Click(object sender, EventArgs e)
        {
            string url = TravelExpenseClaimCommon.GetRedirectTRListItemUrl(requestId);
            Response.Redirect(url);
        }

        protected new void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);
            //this.Script.Alert(msg); 用这个就可以
        }

    }
}