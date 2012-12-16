namespace CA.WorkFlow.UI.InternalOrderMaintenance2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.SharePoint;
    using QuickFlow;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using CA.SharePoint;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {                                                                             
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
            this.Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            this.btnSave.Click += new EventHandler(btnSave_Click);

            this.DataForm1.Department = WorkflowContext.Current.DataFields["Department"].AsString();
            this.DataForm1.OrderNumber = WorkflowContext.Current.DataFields["Order Number"].AsString();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate(new CancelEventArgs()))
            {
                return;
            }
            else
            {
                WorkflowContext.Current.SaveTask();
            }
            
            RedirectToSaveTask();
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            if (!this.DataForm1.Validate())
            {
                DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ?
                               this.DataForm1.msg : "Please fill in all the necessary fields.");
                e.Cancel = true;
                return;
            }

            var dpTaskUsers = new NameCollection();
            dpTaskUsers = GetNextApproveTaskUsers();
            if (dpTaskUsers == null)
            {
                DisplayMessage("The manager is not set in the system.");
                e.Cancel = true;
                return;
            }

            //更新数据到数据库
            DataForm1.UpdateValues();
            //更新工作流变量
            UpdateWorkflowVariable(dpTaskUsers, false);
            //更新list相关变量
            UpdateWorkFlowDataFields(dpTaskUsers);

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        /// <summary>
        /// 更新工作流相关list中字段数据
        /// </summary>
        /// <param name="dpTaskUsers">部门领导</param>
        private void UpdateWorkFlowDataFields(NameCollection dpTaskUsers)
        {
            WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;
            WorkflowContext.Current.DataFields["Manager"] = dpTaskUsers.Count > 0 ? dpTaskUsers[0] : "";

            var DepartmentManagerTaskUsers = new NameCollection();
            var FinanceAnalystTaskUsers = new NameCollection();
            var CFOTaskUsers = new NameCollection();

            DepartmentManagerTaskUsers.Add(UserProfileUtil.GetDepartmentManager(CurrentEmployee.Department));

            List<string> lst = WorkFlowUtil.UserListInGroup("wf_CFO");
            CFOTaskUsers.AddRange(lst.ToArray());

            lst = WorkFlowUtil.UserListInGroup("wf_FinanceAnalyst_IO");
            FinanceAnalystTaskUsers.AddRange(lst.ToArray());
            //WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskUsers", FinanceAnalystTaskUsers);
            //WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskUsers", CFOTaskUsers);
            //WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskUsers", DepartmentManagerTaskUsers);

            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.InternalOrderMaintenanceDepartmentManagerTask, DepartmentManagerTaskUsers.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.InternalOrderMaintenanceCFOTask, CFOTaskUsers.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.InternalOrderMaintenanceFinanceAnalystTask, FinanceAnalystTaskUsers.JoinString(","));
            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

        }

        /// <summary>
        /// 更新WorkFlow工作流的相关变量值
        /// </summary>
        /// <param name="dpTaskUsers">部门领导</param>
        /// <param name="isSave">是否为保存数据状态</param>
        private void UpdateWorkflowVariable(NameCollection dpTaskUsers, bool isSave)
        {
            WorkflowContext.Current.UpdateWorkflowVariable("IsSave", isSave);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskUsers", dpTaskUsers);
        }

        /// <summary>
        /// 获取下一步审批人
        /// </summary>
        /// <returns></returns>
        private NameCollection GetNextApproveTaskUsers()
        {
            var nextApproveTaskUsers = new NameCollection();
            var manager = UserProfileUtil.GetDepartmentManager(CurrentEmployee.Department);
            if (manager.IsNullOrWhitespace())
            {
                return null;
            }
            else if (manager.Equals(CurrentEmployee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
            {
                var managerEmp = WorkFlowUtil.GetEmployeeApprover(CurrentEmployee);
                if (managerEmp == null)
                {
                    return null;
                }
                manager = managerEmp.UserAccount;
            }

            nextApproveTaskUsers.Add(manager);
            //获取代理人员
            var deleman = WorkFlowUtil.GetDeleman(manager, Constants.CAModules.InternalOrderMaintenance);
            if (deleman != null)
            {
                nextApproveTaskUsers.Add(deleman);
            }

            return nextApproveTaskUsers;
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private bool Validate(CancelEventArgs e)
        {
            bool flag = false;
            if (!this.DataForm1.Validate())
            {
                this.lblError.Text = this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : "Please fill in all the necessary fields.";
                e.Cancel = true;
                flag = false;
            }
            else
            {
                flag = true;
            }
            return flag;
        }

    }
}