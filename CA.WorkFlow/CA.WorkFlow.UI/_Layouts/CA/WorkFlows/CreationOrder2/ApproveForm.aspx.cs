namespace CA.WorkFlow.UI.CreationOrder2
{
    using System;
    using Microsoft.SharePoint;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using System.Collections.Generic;
    using System.Collections;
    using System.Diagnostics;
    using QuickFlow;

    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            string uListGUID = this.Request.QueryString["List"];
            string uID = this.Request.QueryString["ID"];
            string uTaskId = this.Request.QueryString["TaskId"];
            if (!this.SecurityValidate(uTaskId, uListGUID, uID, true)){
                RedirectToTask();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            
            //Set the currenty step, the user control will set the control mode of Order Number by this value
            this.DataForm1.CurrentStep = WorkflowContext.Current.Step;
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            this.DataForm1.Department = WorkflowContext.Current.DataFields["Department"].AsString();
            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            string currentStatus = string.Empty;
            string manager = WorkflowContext.Current.DataFields["Manager"].AsString();
            string orderNum = WorkflowContext.Current.DataFields["Order Number"].AsString();
            bool isReject = false, isToCfo = false;
            
            NameCollection cfoTaskUsers = new NameCollection();
            NameCollection faTaskUsers = new NameCollection();
            CheckNextApprove(ref cfoTaskUsers, ref faTaskUsers, e, manager, ref currentStatus, ref orderNum, 
                             ref isReject, ref isToCfo);
            UpdateWorkflowVariable(cfoTaskUsers, faTaskUsers, isReject, isToCfo);
            UpdateWorkFlowDataFields((isToCfo && cfoTaskUsers.Count > 0) ? cfoTaskUsers[0] : manager, currentStatus, orderNum);
            SendMailAndSaveApprovers(e, isReject);

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        /// <summary>
        /// 检查下一步审批
        /// </summary>
        /// <param name="cfoTaskUsers"> CFO</param>
        /// <param name="faTaskUsers">财务</param>
        /// <param name="e">事件对象</param>
        /// <param name="manager">管理者</param>
        /// <param name="status">状态</param>
        /// <param name="orderNum">订单号</param>
        /// <param name="isReject">是否拒绝</param>
        /// <param name="isToCfo">是否转向CFO审批</param>
        private void CheckNextApprove(ref NameCollection cfoTaskUsers, ref NameCollection faTaskUsers, ActionEventArgs e, string manager, ref string status, ref string orderNum, ref bool isReject, ref bool isToCfo)
        {
            if (WorkflowContext.Current.Step == "DepartmentManagerTask")
            {
                faTaskUsers = GetTaskUsers("wf_FinanceAnalyst_IO");
                cfoTaskUsers = GetTaskUsers("wf_CFO");

                if (e.Action == "Approve"){
                    status = CAWorkflowStatus.IODepartmentManagerApprove;
                }
                else if (e.Action == "To CFO")
                {
                    isToCfo = true;
                    status = CAWorkflowStatus.IOToCFO;
                }
                else if (e.Action == "Reject")
                {
                    isReject = true;
                    status = CAWorkflowStatus.IODepartmentManagerReject;
                }
            }
            else if (WorkflowContext.Current.Step == "CfoTask")
            {
                if (e.Action == "Approve")
                {
                    faTaskUsers = GetTaskUsers("wf_FinanceAnalyst_IO");
                    status = CAWorkflowStatus.IOCFOApprove;
                }
                else if (e.Action == "Reject")
                {
                    isReject = true;
                    status = CAWorkflowStatus.IOCFOReject;
                }
            }
            else if (WorkflowContext.Current.Step == "FinanceAnalystTask")
            {
                //不论财务是拒绝还是确认，工作流都结束
                if (e.Action == "Confirm"){
                    status = CAWorkflowStatus.Completed;
                }
                else if (e.Action == "Reject")
                {
                    orderNum = string.Empty;
                    status = CAWorkflowStatus.IOFinanceReject;
                }
            }

            bool isCfo = CurrentManagerIsCfo(cfoTaskUsers, manager);
            //如果部门领导是CFO或者CEO，当点击 “To CFO”，直接跳转到财务
            if (isCfo && isToCfo)
            {
                isToCfo = false;
                status = CAWorkflowStatus.IOCFOApprove;
            }
        }

        /// <summary>
        /// 判断当前审批人是否CFO
        /// </summary>
        /// <param name="cfoTaskUsers"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        private bool CurrentManagerIsCfo(NameCollection cfoTaskUsers, string manager)
        {
            foreach (string user in cfoTaskUsers)
            {
                if (manager.Equals(user, StringComparison.CurrentCultureIgnoreCase)){
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 发送邮件和保存数据
        /// </summary>
        /// <param name="e"></param>
        private void SendMailAndSaveApprovers(ActionEventArgs e, bool isReject)
        {
            //当拒绝和确认时，验证事件和发送邮件
            if (e.Action == "Reject" || e.Action == "Confirm")
            {
                if (!Validate(e.Action, e)){
                    return;
                }

                SendMail(isReject);
            }

            //当条件满足，保存审批信息
            if (e.Action == "Approve" || e.Action == "Confirm" || e.Action == "To CFO"){
                SaveToApprovers();
            }
        }

        /// <summary>
        /// 更新工作流相关list中字段数据
        /// </summary>
        /// <param name="cfoTaskUsers"></param>
        /// <param name="status"></param>
        /// <param name="orderNum"></param>
        private void UpdateWorkFlowDataFields(string manager, string status, string orderNum)
        {    
            WorkflowContext.Current.DataFields["Manager"] = manager;
            WorkflowContext.Current.DataFields["Status"] = status;
            WorkflowContext.Current.DataFields["Order Number"] = orderNum;
        }

        /// <summary>
        /// 更新WorkFlow工作流的相关变量值
        /// </summary>
        /// <param name="cfoTaskUsers"></param>
        /// <param name="faTaskUsers"></param>
        private void UpdateWorkflowVariable(NameCollection cfoTaskUsers, NameCollection faTaskUsers,  bool isReject, bool isToCfo)
        {
            WorkflowContext.Current.UpdateWorkflowVariable("IsReject", isReject);
            WorkflowContext.Current.UpdateWorkflowVariable("IsToCfo", isToCfo);

            WorkflowContext.Current.UpdateWorkflowVariable("CfoTaskUsers", cfoTaskUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnlystTaskUsers", faTaskUsers);
        }

        //Return task users object according to special group
        private NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, CA.WorkFlow.UI.Constants.CAModules.InternalOrderCreation);
        }

        /// <summary>
        /// 验证事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool Validate(string action, ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {                
                DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : "Please fill in Order Number field.");
                e.Cancel = true;
                flag = true;
            }
            return flag;
        }

        //title - identity for the feature
        //isReject
        private void SendMail(bool isReject)
        {
            string title = "InternalOrderCreation";
            var emailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
            if (emailTemplate == null)
            {
                return;
            }
            string bodyTemplate = emailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string subject = emailTemplate["Subject"].AsString();

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string orderType = WorkflowContext.Current.DataFields["Internal Order Type"].ToString(); //Internal Order Type
                string orderNumber = WorkflowContext.Current.DataFields["Order Number"].ToString(); //Order Number
                string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString(); //Approvers
                string applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();  //Applicant
                string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/CreationOrder2/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/CreationOrder2/MyApply.aspx";
                //string comment = WorkflowContext.Current.TaskFields["Body"].AsString();

                List<Employee> employees = WorkFlowUtil.GetEmployees(approvers);
                string approversNames = WorkFlowUtil.GetDisplayNames(employees);
                Employee applicantUser = WorkFlowUtil.GetEmployee(applicant);
                List<string> parameters =  new List<string> { 
                    string.Empty, 
                    isReject ? "rejected" : "approved", 
                    orderType, 
                    isReject ? "N/A" : orderNumber, 
                    applicantUser.DisplayName, 
                    approversNames.IsNotNullOrWhitespace() ? approversNames: "N/A", 
                    isReject ? CurrentEmployee.DisplayName : "N/A", 
                    detailLink
                };
                if (applicantUser != null)
                {
                    //Avoid the same user get the serveral mail
                    AddToEmployees(employees, applicantUser);
                }
                if (isReject)
                {
                    //Rejecter needs to get the notify mail
                    employees.Add(CurrentEmployee);
                }

                WorkFlowUtil.SendMail(subject, bodyTemplate, parameters, employees);
            }
        }
    }
}