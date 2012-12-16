namespace CA.WorkFlow.UI.TR
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
    using SAP.Middleware.Exchange;

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
            if (WorkflowContext.Current.Step.Equals("FinanceConfirmTask", StringComparison.CurrentCultureIgnoreCase))
            {
                PostCashAdvancedToSAP(WorkflowContext.Current.DataFields);
            }
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
                        //保存审批人信息
                        fields["Approvers"] = ReturnAllApprovers(fields["Managers"].ToString());
                        fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["Managers"].ToString());
                        if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["Managers"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                        }

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
                                SendMail(false);
                                context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle + "needs confirm");
                                context.UpdateWorkflowVariable("ConfirmTaskUsers", TravelRequest3Common.GetTaskUsersByModuleWithoutDeleman("wf_ReceptionCtrip_TR", "TravelRequestWorkFlow"));
                                context.UpdateWorkflowVariable("IsContinue", false);
                            }
                        }



                        var receptionists = WorkFlowUtil.UserListInGroup("wf_ReceptionCtrip_TR");
                        fields["Receptionists"] = string.Join(";", receptionists.ToArray());

                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "please resubmit Travel Request");
                        context.UpdateWorkflowVariable("IsContinue", false);
                        SendMail(true);
                    }
                    break;
                case "ConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        SendFinanceMail();

                        fields["Status"] = CAWorkflowStatus.InProgress;
                        SaveToApprovers("Receptionists", "Approvers", "ApproversSPUser");

                        if (fields["IsCashAdvanced"].AsString().Equals("Yes"))
                        {
                            context.UpdateWorkflowVariable("IsNeedFinance", true);
                            context.UpdateWorkflowVariable("FinanceConfirmTaskTitle", taskTitle + "needs confirm");
                            context.UpdateWorkflowVariable("FinanceConfirmTaskFormURL", "/_Layouts/CA/WorkFlows/TravelRequest3/ApproveForm.aspx");
                            context.UpdateWorkflowVariable("FinanceConfirmTaskUsers", TravelRequest3Common.GetTaskUsersByModuleWithoutDeleman(WorkflowGroupName.WF_FinanceConfirm, "TravelRequestWorkFlow"));
                        }
                        else
                        {
                            context.UpdateWorkflowVariable("IsNeedFinance", false);

                            //if (TravelRequest3Common.IsLastTask(Request.QueryString["List"], Request.QueryString["ID"]))
                            //{
                            fields["Status"] = CAWorkflowStatus.Completed;
                            SPFieldUrlValue link = new SPFieldUrlValue();
                            link.Description = "Claim";
                            var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                            link.Url = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/NewForm.aspx?TRNumber=" + Server.UrlEncode(fields["WorkflowNumber"].ToString());
                            fields["Claim"] = link;
                            //}
                        }
                    }
                    break;
                case "FinanceConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        //if (TravelRequest3Common.IsLastTask(Request.QueryString["List"], Request.QueryString["ID"]))
                        //{
                        fields["Approvers"] = ReturnAllApprovers(fields["Managers"].ToString());
                        fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["Managers"].ToString());
                        if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["Managers"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                        }
                        fields["Status"] = CAWorkflowStatus.Completed;
                        SPFieldUrlValue link = new SPFieldUrlValue();
                        link.Description = "Claim";
                        var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                        link.Url = rootweburl + "WorkFlowCenter/Lists/TravelExpenseClaim/NewForm.aspx?TRNumber=" + Server.UrlEncode(fields["WorkflowNumber"].ToString());
                        fields["Claim"] = link;


                        //}
                    }
                    break;
                default:
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private void PostCashAdvancedToSAP(WorkflowDataFields fields)
        {
            string accountName = string.Empty;
            SPFieldUser fieldUser = SPContext.Current.ListItem.Fields["ApplicantSPUser"] as SPFieldUser;
            if (fieldUser != null)
            {
                SPFieldUserValue fieldUserValue = fieldUser.GetFieldValue(fields["ApplicantSPUser"].AsString()) as SPFieldUserValue;
                accountName = fieldUserValue.User.LoginName;
            }
            Employee emp = UserProfileUtil.GetEmployee(accountName);

            if (emp != null)
            {
                List<SapParameter> mSapParametersCD = new List<SapParameter>();
                SapParameter mSapParameters = new SapParameter()
                {
                    BusAct = "RFBU",
                    CompCode = "CA10",
                    DocType = "KR",
                    BusArea = "0001",
                    Currency = "RMB",
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.DisplayName,
                    ExchRate = 1,
                    Header = emp.EmployeeID + "CashAdvanced - OR",
                    RefDocNo = fields["Title"].AsString(),
                    CashAmount = decimal.Parse(fields["CashAdvanced"].AsString())

                };
                if (mSapParameters.CashAmount <= 2000)
                {
                    mSapParameters.PymtMeth = "E";
                }

                mSapParametersCD.Add(mSapParameters);
                string sAPNumber = "";
                string errorMsg = "";
                bool postResult = false;
                ISapExchange sapExchange = SapExchangeFactory.GetCashAdvance();
                List<object[]> result = sapExchange.ImportDataToSap(mSapParametersCD);
                //if (null == result)
                //{
                //    errorMsg += hfEmployeeName + "-" + DateTime.Now.ToShortDateString() + "：" + "Connection failed.";
                //}
                if (null == result)
                {
                    this.Page.ClientScript.RegisterStartupScript(typeof(DataForm), "alert", "<script type=\"text/javascript\">alert('Connection failed !'); window.location = window.location;</script>");
                    return;
                }
                else
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        SapParameter sp = (SapParameter)result[i][0];
                        bool bl = (bool)result[i][2];
                        if (bl)
                        {
                            SapResult sr = (SapResult)result[i][1];
                            sAPNumber = sr.OBJ_KEY;
                            postResult = true;
                        }
                        else
                        {
                            if (result[i][1] is string)
                            {
                                errorMsg += emp.DisplayName + "-" + DateTime.Now.ToShortDateString() + "：" + result[i][1].ToString() + " \n ";
                            }
                            else
                            {
                                string wfID = sp.RefDocNo;
                                SapResult sr = (SapResult)result[i][1];
                                foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
                                {
                                    errorMsg += emp.DisplayName + "-" + DateTime.Now.ToShortDateString() + "：" + ret.MESSAGE + " \n ";
                                }
                            }
                        }
                    }
                }
                var delegationList = CA.SharePoint.SharePointUtil.GetList(WorkflowListName.TravelRequestWorkflow2);
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", fields["Title"].AsString());
                SPListItemCollection trListItem = delegationList.GetItems(query);
                SPListItem trItem = trListItem[0];

                string emsg = trItem["ErrorMsg"].AsString();
                emsg += errorMsg;
                if (postResult)
                {
                    trItem["SAPNumber"] = sAPNumber;
                }
                else
                {
                    trItem["ErrorMsg"] = emsg;
                }


                trItem.Web.AllowUnsafeUpdates = true;
                trItem.Update();
                trItem.Web.AllowUnsafeUpdates = false;
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
            //if (!isReject 
            //    && WorkflowContext.Current.DataFields["IsCashAdvanced"].AsString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
            //    && WorkflowContext.Current.Step.Equals("ConfirmTask",StringComparison.CurrentCultureIgnoreCase)
            //    && !WorkflowContext.Current.DataFields["FinanceMail"].AsString().Equals("Done", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    templateTitle = "TravelRequest2Finance";
            //    to = TravelRequest3Common.GetMailMembers("Finance");
            //    parameters = new List<string>();
            //    parameters.Add("");
            //    parameters.Add(applicantName);
            //    parameters.Add(detailLink);
            //    SendNotificationMail(templateTitle, parameters, to, false);
            //    WorkflowContext.Current.DataFields["FinanceMail"] = "Done";
            //}

        }

        private void SendFinanceMail()
        {
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/TravelRequest3/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"];

            if (WorkflowContext.Current.DataFields["IsCashAdvanced"].AsString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                && WorkflowContext.Current.Step.Equals("ConfirmTask", StringComparison.CurrentCultureIgnoreCase)
                && !WorkflowContext.Current.DataFields["FinanceMail"].AsString().Equals("Done", StringComparison.CurrentCultureIgnoreCase))
            {
                string templateTitle = "TravelRequest2Finance";
                List<string> to = TravelRequest3Common.GetMailMembers("Finance");
                List<string> parameters = new List<string>();
                parameters.Add("");
                parameters.Add(applicantName);
                parameters.Add(detailLink);
                SendNotificationMail(templateTitle, parameters, to, false);
                WorkflowContext.Current.DataFields["FinanceMail"] = "Done";
            }

        }
    }
}
