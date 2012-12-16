namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance2
{
    using System;
    using Microsoft.SharePoint;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using System.Collections.Generic;
    using QuickFlow;

    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, true) && !SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                RedirectToTask();
            }

            if (!this.Page.IsPostBack)
            {
                this.DataForm1.CurrentStep = WorkflowContext.Current.Task.Step;
                this.DataForm1.RecordType = WorkflowContext.Current.DataFields["Record Type"].AsString();
                this.DataForm1.DepartmentVal = WorkflowContext.Current.DataFields["DepartmentVal"].AsString();
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
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
            string vendorID = WorkflowContext.Current.DataFields["Vendor ID"].AsString();
            int paymentTerm = Convert.ToInt32(WorkflowContext.Current.DataFields["PaymentTerm"].AsString());
            bool isReject = false, isToCfo = false;

            NameCollection cfoTaskUsers = new NameCollection();
            NameCollection faTaskUsers = new NameCollection();
            CheckNextApprove(paymentTerm, ref cfoTaskUsers, ref faTaskUsers, e, manager, ref currentStatus, ref vendorID,
                             ref isReject, ref isToCfo);
            UpdateWorkflowVariable(cfoTaskUsers, faTaskUsers, isReject, isToCfo);
            UpdateWorkFlowDataFields((isToCfo && cfoTaskUsers.Count > 0) ? cfoTaskUsers[0] : manager, currentStatus, vendorID);
            SendMailAndSaveApprovers(e, isReject);

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        /// <summary>
        /// 检查下一步审批
        /// </summary>
        /// <param name="cfoTaskUsers">CFO</param>
        /// <param name="faTaskUsers">财务</param>
        /// <param name="e">事件对象</param>
        /// <param name="manager">管理者</param>
        /// <param name="status">状态</param>
        /// <param name="orderNum"></param>
        /// <param name="isReject">是否拒绝</param>
        /// <param name="isToCfo">是否转向CFO审批</param>
        private void CheckNextApprove(int paymentTerm, ref NameCollection cfoTaskUsers, ref NameCollection faTaskUsers, ActionEventArgs e, string manager, ref string status, ref string vendorID, ref bool isReject, ref bool isToCfo)
        {
            if (WorkflowContext.Current.Step == "DepartmentManagerTask")
            {
                faTaskUsers = GetTaskUsers("wf_Finance_MDM");
                cfoTaskUsers = GetTaskUsers("wf_CFO");

                if (e.Action == "Approve")
                {
                    isToCfo = (paymentTerm >= 30) ? false : true;
                    status = CAWorkflowStatus.IODepartmentManagerApprove;
                }
                else if (e.Action == "Reject")
                {
                    isReject = true;
                    status = CAWorkflowStatus.IODepartmentManagerReject;
                }
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApproverLoginName);
            }
            else if (WorkflowContext.Current.Step == "CfoTask")
            {
                if (e.Action == "Approve")
                {
                    faTaskUsers = GetTaskUsers("wf_Finance_MDM");
                    status = CAWorkflowStatus.IOCFOApprove;
                }
                else if (e.Action == "Reject")
                {
                    isReject = true;
                    status = CAWorkflowStatus.IOCFOReject;
                }
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApproverLoginName);
            }
            else if (WorkflowContext.Current.Step == "MdmTask")
            {
                //不论财务是拒绝还是确认，工作流都结束
                if (e.Action == "Confirm")
                {
                    status = CAWorkflowStatus.Completed;
                }
                else if (e.Action == "Reject")
                {
                    vendorID = string.Empty;
                    status = CAWorkflowStatus.IOFinanceReject;
                }
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApproverLoginName);
            }

            bool isCfo = CurrentManagerIsCfo(cfoTaskUsers, manager);
            //如果部门领导是CFO，当点击 “To CFO”，直接跳转到财务
            if (isCfo && isToCfo)
            {
                isToCfo = false;
                status = CAWorkflowStatus.IOCFOApprove;
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceApproverLoginName);
            }
        }

        /// <summary>
        /// 判断当前审批人是否下一个审批者
        /// </summary>
        /// <param name="faTaskUsers"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        private bool CurrentManagerIsFinance(NameCollection faTaskUsers, string manager)
        {
            foreach (string user in faTaskUsers)
            {
                if (manager.Equals(user, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
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
                if (manager.Equals(user, StringComparison.CurrentCultureIgnoreCase))
                {
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
            //当拒绝时，验证事件和发送邮件
            if (e.Action == "Reject")
            {
                if (!Validate(e.Action, e))
                {
                    return;
                }

                SendMail(isReject);
            }

            //当条件满足，保存审批信息
            if (e.Action == "Approve" || e.Action == "Confirm")
            {
                SaveToApprovers();

                if (e.Action == "Confirm")
                {
                    SendMail(isReject);
                }
            }
        }

        /// <summary>
        /// 更新工作流相关list中字段数据
        /// </summary>
        /// <param name="cfoTaskUsers"></param>
        /// <param name="status"></param>
        /// <param name="orderNum"></param>
        private void UpdateWorkFlowDataFields(string manager, string status, string vendorID)
        {
            WorkflowContext.Current.DataFields["Manager"] = manager;
            WorkflowContext.Current.DataFields["Status"] = status;
            WorkflowContext.Current.DataFields["Vendor ID"] = vendorID;
        }

        /// <summary>
        /// 更新WorkFlow工作流的相关变量值
        /// </summary>
        /// <param name="cfoTaskUsers"></param>
        /// <param name="faTaskUsers"></param>
        private void UpdateWorkflowVariable(NameCollection cfoTaskUsers, NameCollection faTaskUsers, bool isReject, bool isToCfo)
        {
            WorkflowContext.Current.UpdateWorkflowVariable("IsReject", isReject);
            WorkflowContext.Current.UpdateWorkflowVariable("IsToCfo", isToCfo);

            WorkflowContext.Current.UpdateWorkflowVariable("CfoTaskUsers", cfoTaskUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("MdmTaskUsers", faTaskUsers);
        }

        //Return task users object according to special group
        private NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, Constants.CAModules.NonTradeSupplierSetupMaintenance);
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
                this.lblError.Text = this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : string.Empty;
                e.Cancel = true;
                flag = false;
            }
            return flag;
        }

        //title - identity for the feature
        //isReject
        private void SendMail(bool isReject)
        {
            string title = "NonTradeSupplierSetupMaintenance2";
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
                string recordType = WorkflowContext.Current.DataFields["Record Type"].ToString(); //Record Type
                string vendId = WorkflowContext.Current.DataFields["Vendor ID"].AsString(); //Vend ID
                string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString(); //Approvers
                string applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();  //Applicant
                string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/NonTradeSupplierSetupMaintenance2/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/NonTradeSupplierSetupMaintenance2Workflow/MyApply.aspx";
                //string comment = WorkflowContext.Current.TaskFields["Body"].AsString();

                List<Employee> employees = WorkFlowUtil.GetEmployees(approvers);
                string approversNames = WorkFlowUtil.GetDisplayNames(employees);
                Employee applicantUser = WorkFlowUtil.GetEmployee(applicant);
                List<string> parameters = new List<string> { 
                    string.Empty, 
                    isReject ? "rejected" : "approved", 
                    recordType, 
                    vendId.IsNotNullOrWhitespace() ? vendId : "N/A", 
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
