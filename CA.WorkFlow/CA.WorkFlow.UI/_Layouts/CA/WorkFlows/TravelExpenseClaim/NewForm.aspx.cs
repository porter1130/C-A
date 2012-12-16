namespace CA.WorkFlow.UI.TravelExpenseClaim
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

            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;

            if (!this.IsPostBack)
            {
                this.DataForm1.RequestId = requestId;


            }
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string taskTitle = string.Empty;
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            //Check which button has been clicked
            var btn = sender as StartWorkflowButton;

            #region Save Common List Data
            List<string> fieldsList =
                new List<string>() {"Applicant",
                                    "ApplicantSPUser",
                                    "ChineseName",
                                    "Department",
                                    "EnglishName",
                                    "Mobile",
                                    "OfficeExt",
                                    "IDNumber"
                                     };

            workflowNumber = TravelExpenseClaimCommon.SaveListFields(requestId, "Travel Request Workflow2", fields, fieldsList);

            #endregion

           

            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                //var msg = this.DataForm1.ValidateForSave();
                //if (msg.IsNotNullOrWhitespace())
                //{
                //    DisplayMessage(msg);
                //    e.Cancel = true;
                //    return;
                //}

                context.UpdateWorkflowVariable("IsSave", true);

                fields["Status"] = CAWorkflowStatus.Pending;

                //Set CompleteTask title for workflow
                context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Travel Expense Claim");
            }
            else
            {
                string msg = this.DataForm1.ValidateForSubmit();
                if (msg.IsNotNullOrWhitespace())
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }

                #region Set users for workflow
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

                //Get Task users include deleman
                TravelExpenseClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);

                fields["CurrManager"] = managerEmp.UserAccount;

                WorkflowContext.Current.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                #endregion

                //Set NextApproveTask title for workflow    
                taskTitle = string.Format("{0} {1} {2}'sTravel Expense", fields["WorkflowNumber"].AsString(), this.DataForm1.TotalCost, fields["EnglishName"].AsString());
                context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "needs approval");
                context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle + "needs confirm");

                context.UpdateWorkflowVariable("IsSave", false);
                fields["Status"] = CAWorkflowStatus.InProgress;

                this.SendEmail("SubmitToApplicant");
            }

            #region update travel request 'claim' link

            SPFieldUrlValue link = new SPFieldUrlValue();
            link.Description = "Closed";
            var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            link.Url = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/MyApply.aspx";

            //SPListItemCollection trItems = TravelExpenseClaimCommon.GetDataCollection(fields["TRWorkflowNumber"].ToString(), "Travel Request Workflow2");
            //trItems[0]["Claim"] = link;
            //SPContext.Current.Web.AllowUnsafeUpdates = true;
            //trItems[0].Update();
            //SPContext.Current.Web.AllowUnsafeUpdates = false;

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

            #region Save Details
            this.DataForm1.Update(); //Save the inputed data to datatable

            TravelExpenseClaimCommon.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString());
            #endregion

            #region Set page URL for workflow
            //Set page url
            var editURL = "/_Layouts/CA/WorkFlows/TravelExpenseClaim/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/TravelExpenseClaim/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormURL", approveURL);
            #endregion

            WorkFlowUtil.UpdateWorkflowPath(context);
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



        //private bool IsCEO(string userAccount)
        //{
        //    bool isCEO = false;
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
        //            {
        //                SPGroup group = web.Groups["wf_CEO"];
        //                foreach (SPUser user in group.Users)
        //                {
        //                    if (user.LoginName.Equals(userAccount, StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        isCEO = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    });
        //    return isCEO;
        //}

    }
}