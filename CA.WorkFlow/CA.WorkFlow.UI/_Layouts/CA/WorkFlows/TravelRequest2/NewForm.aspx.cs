namespace CA.WorkFlow.UI.TravelRequest2
{
    using System;
    using System.ComponentModel;
    using QuickFlow.UI.Controls;
    using QuickFlow.Core;
    using QuickFlow;
    using CA.SharePoint;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using System.Configuration;
    using System.Collections.Generic;

    public partial class NewForm : CAWorkFlowPage
    {
        string workflowNumber = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string taskTitle = string.Empty;
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            //Check which button has been clicked
            var btn = sender as StartWorkflowButton;
            var departmentManagerTaskUsers = new NameCollection();
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                var msg = this.DataForm1.ValidateForSave();
                if (msg.IsNotNullOrWhitespace())
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                this.DataForm1.Update(); //Save the inputed data to datatable
                context.UpdateWorkflowVariable("IsSave", true);
                fields["Status"] = CAWorkflowStatus.Pending;
                decimal  totalCost = this.DataForm1.GetTotal();
                fields["TravelTotalCost"] = totalCost;
                context.UpdateWorkflowVariable("CompleteTaskTitle", "please complete Travel Request");
            }
            else
            {
                var msg = this.DataForm1.ValidateForSubmit();
                bool isCEO = false;

                if (msg.IsNotNullOrWhitespace())
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                this.DataForm1.Update(); //Save the inputed data to datatable
                context.UpdateWorkflowVariable("IsSave", false);
                fields["Status"] = CAWorkflowStatus.InProgress;

                #region Set users for workflow
                //Modify task users

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
                    //fields["Delegates"] = deleman;
                }
                context.UpdateWorkflowVariable("DepartmentHeadTaskUsers", departmentManagerTaskUsers);
                #endregion
                decimal totalCost = this.DataForm1.GetTotal();
                if (isCEO)
                {
                    context.UpdateWorkflowVariable("IsBusiness", false);
                    context.UpdateWorkflowVariable("Total", 0);
                }
                else
                {
                    context.UpdateWorkflowVariable("IsBusiness", this.DataForm1.ChosenFlight);
                    //context.UpdateWorkflowVariable("Total", totalCost);
                    context.UpdateWorkflowVariable("Total", 0);

                }
                fields["TravelTotalCost"] = totalCost;

                taskTitle = DataForm1.EnglishName + "'s Travel Request ";

                #region Set title for workflow
                //Modify task title                
                context.UpdateWorkflowVariable("DepartmentHeadTaskTitle", taskTitle + "needs approval");
                context.UpdateWorkflowVariable("MTMTaskTitle", taskTitle + "needs approval");
                context.UpdateWorkflowVariable("CFOTaskTitle", taskTitle + "needs approval");
                context.UpdateWorkflowVariable("CEOTaskTitle", taskTitle + "needs approval");
                context.UpdateWorkflowVariable("ReceptionistTaskTitle", taskTitle + "needs confirm");
                #endregion
            }
            workflowNumber = this.CreateWorkFlowNumber();
            fields["WorkflowNumber"] = workflowNumber;

            Employee applicant = this.DataForm1.Applicant;
            fields["Applicant"] = applicant != null ? applicant.DisplayName + "(" + applicant.UserAccount + ")" : string.Empty;
            fields["ApplicantSPUser"] = SPContext.Current.Web.EnsureUser(applicant.UserAccount);
            fields["Department"] = DataForm1.Department;

            fields["Department"] = this.DataForm1.Department;
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


            TravelRequest2Common.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString()); //Save request details to lists

            #region Set constants for workflow
            string authorityDepartment = ConfigurationManager.AppSettings["AuthorityDepartment"];
            string authorityMTM = ConfigurationManager.AppSettings["AuthorityMTM"];
            string authorityCFO = ConfigurationManager.AppSettings["AuthorityCFO"];
            context.UpdateWorkflowVariable("L1", Int32.Parse(authorityDepartment));
            context.UpdateWorkflowVariable("L2", Int32.Parse(authorityMTM));
            context.UpdateWorkflowVariable("L3", Int32.Parse(authorityCFO));
            #endregion

            #region Set page URL for workflow
            //Set page url
            var editURL = "/_Layouts/CA/WorkFlows/TravelRequest2/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/TravelRequest2/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("DepartmentHeadTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ReceptionistTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("CFOTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("CEOTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("MTMTaskFormURL", approveURL);
            #endregion

            if (!string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                SendMailForSubmit(departmentManagerTaskUsers);
            }
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
                            if (user.LoginName.Equals(userAccount, StringComparison.CurrentCultureIgnoreCase))
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

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private string CreateWorkFlowNumber()
        {
            var department = DataForm1.Department;
            return "TR" + department + WorkFlowUtil.CreateWorkFlowNumber("TravelRequestWorkflow2" + department).ToString("0000");
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
            string detailLink = rootweburl + "WorkFlowCenter/Lists/TravelRequestWorkflow2/TRPending.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(workflowNumber);
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