namespace CA.WorkFlow.UI.TravelRequest2
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using System.Collections.Generic;
    using QuickFlow;
    using System.Configuration;
    using Microsoft.SharePoint;
    using System.Collections;
    using CA.SharePoint;

    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
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

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();

            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            //double total = Convert.ToDouble(fields["TravelTotalCost"].ToString());
            double total = 0;
            bool isBusiness = fields["FlightClass"].AsString().Equals("Business", StringComparison.CurrentCultureIgnoreCase);
            bool isApprove = e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase);

            double L1Authority = Convert.ToDouble(ConfigurationManager.AppSettings["AuthorityDepartment"]);
            double L2Authority = Convert.ToDouble(ConfigurationManager.AppSettings["AuthorityMTM"]);
            double L3Authority = Convert.ToDouble(ConfigurationManager.AppSettings["AuthorityCFO"]);

            //When the flight is Business class, the order should be checked by CEO.
            if (isApprove && isBusiness)
            {
                context.UpdateWorkflowVariable("CEOTaskUsers", GetTaskUsers("wf_CEO"));

                //0830
                var ceos = WorkFlowUtil.UserListInGroup("wf_CEO");
                fields["Ceos"] = string.Join(";", ceos.ToArray());

            }

            string step = WorkflowContext.Current.Step;
            switch (step)
            {
                case "DepartmentHeadTask":
                    if (isApprove)
                    {
                        if (total <= L1Authority)
                        {
                            context.UpdateWorkflowVariable("ReceptionistTaskUsers", GetTaskUsersWithoutDeleman("wf_ReceptionCtrip_TR"));

                            //0830
                            var receptionists = WorkFlowUtil.UserListInGroup("wf_ReceptionCtrip_TR");
                            fields["Receptionists"] = string.Join(";", receptionists.ToArray());

                            //SendMail(false);
                        }
                        else
                        {
                            context.UpdateWorkflowVariable("MTMTaskUsers", GetTaskUsers("wf_Finance_MTM"));

                            //0830
                            var mtms = WorkFlowUtil.UserListInGroup("wf_Finance_MTM");
                            fields["Mtms"] = string.Join(";", mtms.ToArray());
                        }

                        //0830
                        SaveToApprovers("Managers", "Approvers", "ApproversSPUser");
                        

                        fields["Status"] = CAWorkflowStatus.TRDepartmentManagerApprove;

                        //0908 If CEO is the first manager, needn't to check whether it is business flights.
                        var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].AsString());
                        var approver = WorkFlowUtil.GetEmployeeApprover(UserProfileUtil.GetEmployeeEx(applicant));
                        if (approver != null && TravelRequest2Common.IsCEO(approver.UserAccount))
                        {
                            context.UpdateWorkflowVariable("IsBusiness", false);
                            isBusiness = false;
                        }
                        //else
                        //{
                        //    //When approver is null, it indicates the applicant is CEO, needn't more approve process and the workflow will forward to receptionist directly.
                        //    context.UpdateWorkflowVariable("ReceptionistTaskUsers", GetTaskUsersWithoutDeleman("wf_ReceptionCtrip_TR"));

                        //    //0830
                        //    var receptionists = WorkFlowUtil.UserListInGroup("wf_ReceptionCtrip_TR");
                        //    fields["Receptionists"] = string.Join(";", receptionists.ToArray());

                        //    SendMail(false);
                        //}
                        

                        //For special person, the business flight needn't checked by CEO
                        if (TravelRequest2Common.isSpecialLevel(fields["Applicant"].AsString()))
                        {
                            context.UpdateWorkflowVariable("IsBusiness", false);
                            isBusiness = false;
                        }
                        if (!isBusiness)
                        {
                            SendMail(false);
                        }
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.TRDepartmentManagerReject;
                        SendMail(true);
                    }
                    break;
                case "MTMTask":
                    if (isApprove)
                    {
                        if (total <= L2Authority)
                        {
                            context.UpdateWorkflowVariable("ReceptionistTaskUsers", GetTaskUsersWithoutDeleman("wf_ReceptionCtrip_TR"));

                            //0830
                            var receptionists = WorkFlowUtil.UserListInGroup("wf_ReceptionCtrip_TR");
                            fields["Receptionists"] = string.Join(";", receptionists.ToArray());

                            if (!isBusiness)
                            {
                                SendMail(false);
                            }
                        }
                        else
                        {
                            context.UpdateWorkflowVariable("CFOTaskUsers", GetTaskUsers("wf_CFO"));

                            //0830
                            var cfos = WorkFlowUtil.UserListInGroup("wf_CFO");
                            fields["Cfos"] = string.Join(";", cfos.ToArray());

                        }

                        //SaveToApprovers();
                        //fields["ApproversSPUser"] = WorkFlowUtil.GetApproversValue();

                        //0830
                        SaveToApprovers("Mtms", "Approvers", "ApproversSPUser");

                        fields["Status"] = CAWorkflowStatus.TRMTMApprove;
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.TRMTMReject;
                        SendMail(true);
                    }
                    break;
                case "CFOTask":
                    if (isApprove)
                    {
                        if (total <= L3Authority)
                        {
                            context.UpdateWorkflowVariable("ReceptionistTaskUsers", GetTaskUsersWithoutDeleman("wf_ReceptionCtrip_TR"));

                            //0830
                            var receptionists = WorkFlowUtil.UserListInGroup("wf_ReceptionCtrip_TR");
                            fields["Receptionists"] = string.Join(";", receptionists.ToArray());

                            if (!isBusiness)
                            {
                                SendMail(false);
                            }
                        }
                        else
                        {
                            context.UpdateWorkflowVariable("CEOTaskUsers", GetTaskUsers("wf_CEO"));

                            //0830
                            var ceos = WorkFlowUtil.UserListInGroup("wf_CEO");
                            fields["Ceos"] = string.Join(";", ceos.ToArray());
                        }

                        //SaveToApprovers();
                        //fields["ApproversSPUser"] = WorkFlowUtil.GetApproversValue();

                        //0830
                        SaveToApprovers("Cfos", "Approvers", "ApproversSPUser");
                        
                        fields["Status"] = CAWorkflowStatus.TRCFOApprove;
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.TRCFOReject;
                        SendMail(true);
                    }
                    break;
                case "CEOTask":
                    if (isApprove)
                    {                     
                        //SaveToApprovers();
                        //fields["ApproversSPUser"] = WorkFlowUtil.GetApproversValue();

                        //0830
                        SaveToApprovers("Ceos", "Approvers", "ApproversSPUser");
                        
                        fields["Status"] = CAWorkflowStatus.TRCEOApprove;

                        context.UpdateWorkflowVariable("ReceptionistTaskUsers", GetTaskUsersWithoutDeleman("wf_ReceptionCtrip_TR"));

                        //0830
                        var receptionists = WorkFlowUtil.UserListInGroup("wf_ReceptionCtrip_TR");
                        fields["Receptionists"] = string.Join(";", receptionists.ToArray());

                        SendMail(false);
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.TRCEOReject;
                        SendMail(true);
                    }
                    break;
                case "ReceptionistTask":
                    if (e.Action == "Confirm")
                    {                    
                        //SaveToApprovers();
                        //fields["ApproversSPUser"] = WorkFlowUtil.GetApproversValue();

                        //0830
                        SaveToApprovers("Receptionists", "Approvers", "ApproversSPUser");
                       
                        fields["Status"] = CAWorkflowStatus.InProgress;
                        if (TravelRequest2Common.IsLastTask(Request.QueryString["List"], Request.QueryString["ID"]))
                        {
                            fields["Status"] = CAWorkflowStatus.Completed;

                            SPFieldUrlValue link = new SPFieldUrlValue();
                            link.Description = "Claim";
                            var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                            link.Url = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/NewForm.aspx?TRNumber=" + fields["WorkflowNumber"].ToString();
                            fields["Claim"] = link;
                            //SendMail(false);
                        }
                    }
                    break;
            }
        }

        //Return task users object according to special group
        private NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, CA.WorkFlow.UI.Constants.CAModules.TravelRequest);
        }

        private NameCollection GetTaskUsersWithoutDeleman(string group)
        {
            return WorkFlowUtil.GetTaskUsersWithoutDeleman(group, CA.WorkFlow.UI.Constants.CAModules.TravelRequest);
        }

        private bool Validate(string action, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {
                DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : string.Empty);
                e.Cancel = true;
                flag = false;
            }
            return flag;
        }

        

        //title - identity for the feature
        //isReject
        private void SendMail(bool isReject)
        {
            //Send mail to Onsite and Receptionist
            var type = isReject ? "Reject" : "Approve";
            var templateTitle = "TravelRequest2Action" + type;
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            List<string> to = TravelRequest2Common.GetMailMembers("Receptionist", "C-Trip");
            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = isReject ? rootweburl + "WorkFlowCenter/Lists/TravelRequestWorkflow2/TRReject.aspx" : rootweburl + "CA/MyTasks.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            if (isReject)
            {
                parameters.Add(CurrentEmployee.DisplayName);
                parameters.Add(WorkflowContext.Current.DataFields["WorkflowNumber"].AsString());
            }
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);

            //Send mail to Applicant
            to = new List<string>();
            parameters = new List<string>();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            templateTitle = "TravelRequest2Notify" + type;
            detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/TravelRequest2/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"];
            to.Add(applicantAccount);
            parameters.Add("");
            if (isReject)
            {
                parameters.Add(CurrentEmployee.DisplayName);
            }
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, true);

            //Send mail to Finance if need
            //If the request has been approved and it needs cash advance, the finance will receive notify mail.
            if (!isReject && WorkflowContext.Current.DataFields["IsCashAdvanced"].AsString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
            {
                templateTitle = "TravelRequest2Finance";
                to = TravelRequest2Common.GetMailMembers("Finance");
                parameters = new List<string>();
                parameters.Add("");
                parameters.Add(applicantName);
                parameters.Add(detailLink);
                SendNotificationMail(templateTitle, parameters, to, false);
            }
            
        }
    }
}