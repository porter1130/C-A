namespace CA.WorkFlow.UI.TravelRequest3
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
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase)){
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true)){
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
            //定义标题
            string taskTitle = fields["Applicant"] + "'s Travel Request ";
            
            switch (WorkflowContext.Current.Step)
            {
                case "NextApproveTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {   
                        string totalCostStr = fields["TravelTotalCost"].ToString();
                        string managerStr = fields["Managers"].ToString();
                        double total = Convert.ToDouble(totalCostStr);
                        //获取配额
                        long quota = GetQuota(managerStr);
                        //当前登录用户是否CEO
                        bool isCEO = IsCEO(managerStr);

                        if (total > quota & isCEO == false)
                        {
                            //获取下一个审批者
                            Employee managerEmp = GetNextApproverEmp(managerStr);
                            if (managerEmp == null)
                            {
                                string errorMsg = @"The applicant\'s budget is greater than your approved amount limits, and your manager is not set in system";
                                DisplayMessage(errorMsg);
                                e.Cancel = true;
                                return;
                            }

                            //Get Task users include deleman 
                            var manager = new NameCollection();
                            TravelRequest3Common.GetTaskUsersByModule(manager, managerEmp.UserAccount, "TravelRequestWorkFlow");
                            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "needs approval");
                            context.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                            context.UpdateWorkflowVariable("IsContinue", true);
                            fields["Status"] = CAWorkflowStatus.InProgress;
                            fields["Managers"] = managerEmp.UserAccount;
                        }
                        else
                        {
                            bool isBusiness = fields["FlightClass"].AsString().Equals("Business", StringComparison.CurrentCultureIgnoreCase);
                            bool isApprove = e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase);

                            //When the flight is Business class, the order should be checked by CEO.
                            if (isCEO == false && isApprove && isBusiness)
                            {
                                var ceos = WorkFlowUtil.UserListInGroup("wf_CEO");
                                NameCollection nCollection = TravelRequest3Common.GetTaskUsersByModuleAndGroup("wf_CEO", "TravelRequestWorkFlow");
                                context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "needs approval");
                                context.UpdateWorkflowVariable("NextApproveTaskUsers", nCollection);
                                context.UpdateWorkflowVariable("IsContinue", true);
                                fields["Ceos"] = string.Join(";", ceos.ToArray());
                                fields["Managers"] = nCollection[0];
                            }
                            else
                            {
                                context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle + "needs confirm");
                                context.UpdateWorkflowVariable("ConfirmTaskUsers", TravelRequest3Common.GetTaskUsersByModuleWithoutDeleman("wf_ReceptionCtrip_TR", "TravelRequestWorkFlow"));
                                context.UpdateWorkflowVariable("IsContinue", false);
                            }
                        }

                        //保存审批人信息
                        fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", CurrentEmployee.UserAccount);
                        fields["Approvers"] = ReturnAllApprovers(CurrentEmployee.UserAccount);
                    }
                    else
                    {
                        if (!Validate(e.Action, e)){
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "please resubmit Travel Request");
                        context.UpdateWorkflowVariable("IsContinue", false);
                        SendMail(false);
                    }
                    break;
                case "ConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        fields["Status"] = CAWorkflowStatus.InProgress;
                        if (TravelRequest3Common.IsLastTask(Request.QueryString["List"], Request.QueryString["ID"]))
                        {
                            fields["Status"] = CAWorkflowStatus.Completed;
                            SPFieldUrlValue link = new SPFieldUrlValue();
                            link.Description = "Claim";
                            var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                            link.Url = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/NewForm.aspx?TRNumber=" + fields["WorkflowNumber"].ToString();
                            fields["Claim"] = link;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取当前登录用户的配额，参数：账户
        /// </summary>
        /// <returns>配额</returns>
        private long GetQuota(string manager)
        {
            string levelType = "Contract Approval Limits";
            return WorkFlowUtil.GetQuota(manager, levelType);
        }

        /// <summary>
        /// 获取下一个审批者信息
        /// </summary>
        /// <param name="mStr">当前审批人</param>
        private Employee GetNextApproverEmp(string mStr)
        {
            var managerEmp = WorkFlowUtil.GetNextApprover(mStr);
            //if (managerEmp == null)
            //{
            //    List<string> ceos = WorkflowPerson.GetCEO();
            //    if (ceos.Count == 0){
                  //  DisplayMessage("The init error about WorkflowPerson in the system.");
            //        return null;
            //    }
            //    managerEmp = UserProfileUtil.GetEmployeeEx(ceos[0]);
            //}
            return managerEmp;
        }

        /// <summary>
        /// 是否CEO
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="action"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="isReject">是否被拒绝</param>
        private void SendMail(bool isReject)
        {
            //Send mail to Onsite and Receptionist
            var type = isReject ? "Reject" : "Approve";
            var templateTitle = "TravelRequest2Action" + type;
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            List<string> to = TravelRequest3Common.GetMailMembers("Receptionist", "C-Trip");
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
            detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/TravelRequest3/DisplayForm.aspx?List="
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
                to = TravelRequest3Common.GetMailMembers("Finance");
                parameters = new List<string>();
                parameters.Add("");
                parameters.Add(applicantName);
                parameters.Add(detailLink);
                SendNotificationMail(templateTitle, parameters, to, false);
            }
            
        }
    }
}