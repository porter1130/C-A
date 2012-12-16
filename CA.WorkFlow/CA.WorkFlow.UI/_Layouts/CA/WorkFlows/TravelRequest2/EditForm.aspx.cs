namespace CA.WorkFlow.UI.TravelRequest2
{
    using System;
    using System.ComponentModel;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Collections.Generic;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataForm1.Mode = "Edit";

            //Check security
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;

            if (!IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                if (fields["Applicant"].AsString() != null)
                {
                    this.DataForm1.UserAccount = GetUserAccount(fields["Applicant"].AsString());
                }
                this.DataForm1.TotalBudget = fields["TravelTotalCost"].AsString();
                this.DataForm1.OtherPurpose = fields["TravelOtherPurpose"].AsString();
                this.DataForm1.ChosenFlight = Convert.ToBoolean(fields["IsBusiness"].AsString());
                this.DataForm1.NextFlight = Convert.ToBoolean(fields["IsNextFlight"].AsString());
                this.DataForm1.IsBookHotel = !Convert.ToBoolean(fields["IsBookHotel"].AsString());
            }

        }

        private string GetUserAccount(string applicant)
        {
            if (applicant.IsNotNullOrWhitespace())
            {
                char[] split = { '(', ')' };
                return applicant.Split(split)[1].ToString();
            }
            return applicant;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var msg = this.DataForm1.ValidateForSave();
            if (msg.IsNotNullOrWhitespace())
            {
                DisplayMessage(msg);
                return;
            }
            else
            {
                this.DataForm1.Update(); //Save the inputed data to datatable

                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                Employee applicant = this.DataForm1.Applicant;

                fields["Applicant"] = applicant != null ? applicant.DisplayName + "(" + applicant.UserAccount + ")" : string.Empty;
                fields["ApplicantSPUser"] = SPContext.Current.Web.EnsureUser(applicant.UserAccount);
                fields["Department"] = DataForm1.Department;

                decimal totalCost = this.DataForm1.GetTotal();
                fields["TravelTotalCost"] = totalCost;
                fields["TravelOtherPurpose"] = this.DataForm1.OtherPurpose;
                var isBusiness = this.DataForm1.ChosenFlight;
                var isNextFlight = this.DataForm1.NextFlight;
                var isBookHotel = this.DataForm1.IsBookHotel;
                fields["IsBusiness"] = isBusiness;
                fields["IsNextFlight"] = isNextFlight;
                fields["IsBookHotel"] = !isBookHotel;
                var flightClass = string.Empty;
                if (isBusiness)
                {
                    flightClass = "Business";
                }
                else if (isNextFlight)
                {
                    flightClass = "Other available flight";
                }
                else
                {
                    flightClass = "Economy";
                }
                fields["FlightClass"] = flightClass;

                //Delete all draft items before saving
                TravelRequest2Common.DeleteAllDraftItems(fields["WorkflowNumber"].AsString());

                TravelRequest2Common.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString()); //Save request details to lists

                WorkflowContext.Current.SaveTask();
                RedirectToTask();
            }
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            var msg = this.DataForm1.ValidateForSubmit();
            var isCEO = false;

            if (msg.IsNotNullOrWhitespace())
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            fields["Status"] = CAWorkflowStatus.InProgress;

            this.DataForm1.Update(); //Save the inputed data to datatable
            context.UpdateWorkflowVariable("IsSave", false);
            fields["Status"] = CAWorkflowStatus.InProgress;

            var taskTitle = DataForm1.EnglishName + "'s Travel Request ";

            #region Set title for workflow
            //Modify task title                
            context.UpdateWorkflowVariable("DepartmentHeadTaskTitle", taskTitle + "needs approval");
            context.UpdateWorkflowVariable("MTMTaskTitle", taskTitle + "needs approval");
            context.UpdateWorkflowVariable("CFOTaskTitle", taskTitle + "needs approval");
            context.UpdateWorkflowVariable("CEOTaskTitle", taskTitle + "needs approval");
            context.UpdateWorkflowVariable("ReceptionistTaskTitle", taskTitle + "needs confirm");
            #endregion

            #region Set users for workflow
            //Modify task users
            var departmentManagerTaskUsers = new NameCollection();

            //If system can't find the manager, return null;
            var managerEmp = WorkFlowUtil.GetEmployeeApprover(this.DataForm1.Applicant);
            string manager = string.Empty;
            if (managerEmp == null)
            {
                if (!IsCEO(this.DataForm1.Applicant.UserAccount))
                {
                    DisplayMessage("The manager is not set in the system.");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    isCEO = true;
                    manager = this.DataForm1.Applicant.UserAccount;
                }
            }
            else
            {
                manager = managerEmp.UserAccount;
            }

            departmentManagerTaskUsers.Add(manager);

            //0830
            fields["Managers"] = manager;

            var deleman = WorkFlowUtil.GetDeleman(manager, CA.WorkFlow.UI.Constants.CAModules.TravelRequest);
            if (deleman != null)
            {
                departmentManagerTaskUsers.Add(deleman);
            }
            context.UpdateWorkflowVariable("DepartmentHeadTaskUsers", departmentManagerTaskUsers);
            #endregion

            Employee applicant = this.DataForm1.Applicant;
            fields["Applicant"] = applicant != null ? applicant.DisplayName + "(" + applicant.UserAccount + ")" : string.Empty;
            fields["ApplicantSPUser"] = SPContext.Current.Web.EnsureUser(applicant.UserAccount);
            fields["Department"] = DataForm1.Department;

            decimal totalCost = this.DataForm1.GetTotal();
            if (isCEO)
            {
                context.UpdateWorkflowVariable("IsBusiness", false);
                context.UpdateWorkflowVariable("Total", 0);
            }
            else
            {
                context.UpdateWorkflowVariable("IsBusiness", DataForm1.ChosenFlight);
                //context.UpdateWorkflowVariable("Total", totalCost);
                context.UpdateWorkflowVariable("Total", 0);
            }

            fields["TravelTotalCost"] = totalCost;
            fields["TravelOtherPurpose"] = this.DataForm1.OtherPurpose;
            var isBusiness = this.DataForm1.ChosenFlight ? "Yes" : "No";
            var isNextFlight = this.DataForm1.NextFlight ? "Yes" : "No";
            fields["IsBusiness"] = isBusiness;
            fields["IsNextFlight"] = isNextFlight;
            var flightClass = string.Empty;
            if (isBusiness.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
            {
                flightClass = "Business";
            }
            else if (isNextFlight.Equals("No", StringComparison.CurrentCultureIgnoreCase))
            {
                flightClass = "Other available flight";
            }
            else
            {
                flightClass = "Economy";
            }
            fields["FlightClass"] = flightClass;

            //Delete all draft items before saving
            TravelRequest2Common.DeleteAllDraftItems(fields["WorkflowNumber"].AsString());

            TravelRequest2Common.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString()); //Save request details to lists
            
            SendMailForSubmit(departmentManagerTaskUsers);

        }

        private bool IsCEO(string userAccount)
        {
            bool isCEO = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPGroup group = web.Groups["wf_CEO"];
                        foreach (SPUser user in group.Users)
                        {
                            if (user.LoginName.Equals( userAccount,StringComparison.CurrentCultureIgnoreCase))
                            {
                                isCEO = true;
                                break;
                            }
                        }
                    }
                }
            });
            return isCEO;
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void SendMailForSubmit(NameCollection departmentManagerTaskUsers)
        {
            //Send mail to Onsite and Receptionist
            var templateTitle = "TravelRequest2Submit2";
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            List<string> to = TravelRequest2Common.GetMailMembers("Receptionist", "C-Trip");
            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/TravelRequest2/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/TravelRequest2/MyApply.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(WorkflowContext.Current.DataFields["WorkflowNumber"].AsString());
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);

            //Send mail to Applicant
            to = new List<string>();
            parameters = new List<string>();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            var approverNames = WorkFlowUtil.GetDisplayNames(TravelRequest2Common.ConvertToList(departmentManagerTaskUsers));
            templateTitle = "TravelRequest2Submit1";
            //detailLink = rootweburl + "WorkFlowCenter/Lists/TravelRequestWorkflow2/MyApply.aspx";
            to.Add(applicantAccount);
            parameters.Add("");
            parameters.Add(approverNames);
            //parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, true);

            //Send mail to Department Manager
            parameters = new List<string>();
            to = TravelRequest2Common.ConvertToList(departmentManagerTaskUsers);
            templateTitle = "TravelRequest2Submit3";
            detailLink = rootweburl + "CA/MyTasks.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);
        }
    }
}