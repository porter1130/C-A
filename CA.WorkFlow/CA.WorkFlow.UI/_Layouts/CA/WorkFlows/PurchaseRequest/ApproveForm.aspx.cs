namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using Microsoft.SharePoint;
    using System.Collections.Generic;
    using CA.SharePoint;

    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            CheckSecurity();

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            if (!IsPostBack)
            {
                this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
                DataForm1.DisplayMode = "Display";
            }

            this.Actions.OnClientClick = "return dispatchAction(this);";
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

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
            {
                fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", fields["CurrManager"].ToString());
                if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", SPContext.Current.Web.CurrentUser.LoginName);
                }
                
                var originManager = UserProfileUtil.GetEmployeeEx(fields["CurrManager"].ToString());
                var start = originManager.JobLevel.IndexOf("-") + 1; //L-3
                var length = originManager.JobLevel.Length - start;
                fields["LevelHistory"] = fields["LevelHistory"].AsString() + ";" + originManager.JobLevel.Substring(start, length);

                var isStop = false;
                var approvalTotalRmb = Convert.ToDouble(fields["ApprovalTotalRMB"].ToString());
                var quotaStr = WorkFlowUtil.GetQuotaByType("Contract Approval Limits");
                char[] split = { ';' };
                var quotas = quotaStr.Split(split);
                var countReady = IsCountReady(fields["LevelHistory"].AsString(), fields["LowLevel"].ToString()); //是否满足4-eyes规则
                //var levelReady = Convert.ToInt32(originManager.JobLevel) <= Convert.ToInt32(fields["HighLevel"]); //是否满足循环停止条件
                var applicantAccount = WorkFlowUtil.GetApplicantAccount(fields["Applicant"].ToString()); //取得申请人帐号


                if (IsSpecialCompleted(applicantAccount, fields["Approvers"].AsString())) //判断流程中CEO与CFO均已涉及过，需结束
                {
                    isStop = true;
                }
                else if (approvalTotalRmb <= Convert.ToDouble(quotas[0])) //小于5000只需审批一次，且manager的level至少为7
                {
                    string sLevel = originManager.JobLevel;
                    string[] sArray = sLevel.Split('-');
                    int iLevel = 0;
                    if (sArray.Length == 2)
                    {
                        iLevel = int.Parse(sArray[1].Trim());
                        if (iLevel <= 7)
                        {
                            isStop = true;
                        }
                    }
                }
                else if (approvalTotalRmb <= Convert.ToDouble(quotas[2]))
                {
                    isStop = countReady; //循环停止条件与4-eyes规则同时满足
                }
                

                if (isStop) //结束审批循环，指定下一任务的人
                {
                    var checkPerson = fields["CheckPerson"].AsString();
                    var confirmManager = new NameCollection();
                    confirmManager.Add(checkPerson);
                    var deleman = PurchaseRequestCommon.GetDeleman(checkPerson); //查找代理人
                    if (deleman != null)
                    {
                        confirmManager.Add(deleman);
                    }

                    context.UpdateWorkflowVariable("ConfirmTaskUsers", confirmManager);
                    WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", false); //结束循环
                }
                else//继续循环审批
                {
                    #region Set users for workflow
                    //Modify task users
                    NameCollection manager = new NameCollection();

                    //if (approvalTotalRmb > Convert.ToDouble(quotas[2]))//审批金额大于10W,则先由CFO,再由CEO
                    //{
                    manager = GetCFOCEOApprover(originManager.UserAccount);//先由 CFO再由CEO批
                    //}
                    //else
                    //{
                    //    manager = GetCEOCFOApprover( originManager.UserAccount);
                    //}
                    if (manager == null)
                    {
                        e.Cancel = true;
                        return;  
                    }
                    CommonUtil.logInfo("PR的CurrentManager:"+manager[0]);
                    fields["CurrManager"] = manager[0];
                    WorkflowContext.Current.UpdateWorkflowVariable("ApproveTaskUsers", manager);
                    WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", true);
                    #endregion
                }
            }
            else//Reject
            {
                if (!Validate(e.Action, e))
                {
                    return;
                }
                fields["Status"] = CAWorkflowStatus.Rejected;
                fields["LevelHistory"] = string.Empty;//拒绝时清空审批历史
                context.UpdateWorkflowVariable("CompleteTaskTitle", fields["WorkflowNumber"].ToString() + " :Please complete the rejected PR");
                SendRejectMail();
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);

        }
        /// <summary>
        /// 得到CFO,CEO顺序的审批用户
        /// </summary>
        /// <param name="originManager"></param>
        /// <returns></returns>
        NameCollection GetCFOCEOApprover(string originManager)
        {
            NameCollection manager = new NameCollection();

            Employee managerEmp = WorkFlowUtil.GetNextConstructionApprover(originManager);///下一步审批人
            if (managerEmp == null)///找不到Manager
            {
                DisplayMessage("The manager is not set in the system.");
                return null;
            }

            string sManagerAccount = managerEmp.UserAccount;///下一步审批人帐号
            if (WorkflowPerson.IsCEO(sManagerAccount)) //如果下一步审批人为CEO,且PR价为大于10万,则转换为CFO
            {
                List<string> cfos = WorkflowPerson.GetCFO();
                if (cfos.Count == 0)
                {
                    DisplayMessage("The init error about WorkflowPerson in the system.");
                    return null; 
                }
                sManagerAccount = UserProfileUtil.GetEmployeeEx(cfos[0]).UserAccount;///将"下一步审批人账号"改为CFO
            }

            if (WorkflowPerson.IsCFO(originManager)) //如果当前原审批人为CFO，则下一步需特殊指定给CEO，以保证4-eyes规则
            {
                List<string> ceos = WorkflowPerson.GetCEO();
                if (ceos.Count == 0)
                {
                    DisplayMessage("The init error about WorkflowPerson in the system.");
                    return null;
                }
                sManagerAccount = UserProfileUtil.GetEmployeeEx(ceos[0]).UserAccount;
            }

            manager.Add(sManagerAccount);
            var deleman = PurchaseRequestCommon.GetDeleman(sManagerAccount); //查找代理人
            if (deleman != null)
            {
                manager.Add(deleman);
            }
            return manager;
        }

        /// <summary>
        /// 得到CEO,CFO顺序的审批用户
        /// </summary>
        /// <param name="sCurrManager"></param>
        /// <returns></returns>
        NameCollection GetCEOCFOApprover(string originManager)
        {
            NameCollection managerColl = new NameCollection();
            Employee managerEmp = WorkFlowUtil.GetNextConstructionApprover(originManager);
            string sManagerAccount=string.Empty;
            if (WorkflowPerson.IsCEO(originManager)) //如果当前原审批人为CEO，则下一步需特殊指定给CFO，以保证4-eyes规则
            {
                List<string> cfos = WorkflowPerson.GetCFO();
                if (cfos.Count == 0)
                {
                    DisplayMessage("The init error about WorkflowPerson in the system.");
                    return null;
                }
                sManagerAccount = UserProfileUtil.GetEmployeeEx(cfos[0]).UserAccount;
            }
            else if (managerEmp == null) //若当前原审批人不为CEO，则需找下一级manager进行审批。
            {
                DisplayMessage("The manager is not set in the system.");
                return null;
            }
            else
            {
                sManagerAccount = managerEmp.UserAccount;
            }

            managerColl.Add(sManagerAccount);
            var deleman = PurchaseRequestCommon.GetDeleman(sManagerAccount); //查找代理人
            if (deleman != null)
            {
                managerColl.Add(deleman);
            }
            return managerColl;
        }

        //判断审批中是否满足4-eyes规则
        private bool IsCountReady(string levelHistory, string lowLevel)
        {
            char[] split = { ';' };
            var levels = levelHistory.Split(split);
            int count = 0;
            foreach (var level in levels)
            {
                if (string.IsNullOrEmpty(level))
                {
                    continue;
                }
                if (Convert.ToInt32(level) <= Convert.ToInt32(lowLevel))
                {
                    count++;
                }
            }

            return count >= 2;
        }

        //判断是否存在CEO与CFO都已在流程中
        private bool IsSpecialCompleted(string applicant, string approvers)
        {
            //applicant: CA\test1
            //approvers: CA\test2;CA\test3;
            var persons = approvers + applicant;
            char[] split = { ';' };
            var personArr = persons.Split(split);
            int countCEO = 0;
            int countCFO = 0;
            foreach (var person in personArr)
            {
                if (string.IsNullOrEmpty(person))
                {
                    continue;
                }
                countCEO += WorkflowPerson.IsCEO(person) ? 1 : 0;
                countCFO += WorkflowPerson.IsCFO(person) ? 1 : 0;
            }
            return countCEO > 0 && countCFO > 0;
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

        private void SendRejectMail()
        {
            var templateTitle = "PurchaseRequestReject";

//To Applicant
//Dear {0},<br /><br /> 
//Your Purchase Request has been rejected by {1}
//<br />
//<br /> 
//Please view the detailed information by <a href='{2}' target="_blank">clicking here</a>! 
//<br/>
//<br/> 
//-EWF System Account
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            List<string> to = new List<string>();
            to.Add(applicantAccount);

            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/PurchaseRequest/DisplayForm.aspx?List="
                                + Request.QueryString["List"]
                                + "&ID=" + Request.QueryString["ID"];

            var length = applicantStr.IndexOf("(");
            parameters.Add(length > 0 ? applicantStr.Substring(0, length) : applicantAccount);
            parameters.Add(CurrentEmployee.DisplayName);
            parameters.Add(detailLink);

            SendNotificationMail(templateTitle, parameters, to, true);
        }
    }
}