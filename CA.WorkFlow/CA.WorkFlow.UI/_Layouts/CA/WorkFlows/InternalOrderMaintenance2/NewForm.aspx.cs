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

    public partial class NewForm : CAWorkFlowPage        
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
            this.DataForm1.Department = CurrentEmployee.Department;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            var btn = sender as StartWorkflowButton;
            var dpTaskUsers = new NameCollection();
            var isSave = string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase);
            if (isSave == false)
            {
                if (!this.DataForm1.Validate()){
                    DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ?
                                   this.DataForm1.msg : "Please fill in all the necessary fields.");
                    e.Cancel = true;
                    return;
                }

                //获取审批人
                dpTaskUsers = GetNextApproveTaskUsers();
                if (dpTaskUsers == null){
                    DisplayMessage("The manager is not set in the system.");
                    e.Cancel = true;
                    return;
                }
            }

            //更新工作流变量
            UpdateWorkflowVariable(dpTaskUsers, isSave);
            //更新list相关变量
            UpdateWorkFlowDataFields(dpTaskUsers, isSave);

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        /// <summary>
        /// 更新工作流相关list中字段数据
        /// </summary>
        /// <param name="dpTaskUsers">部门领导</param>
        /// <param name="isSave">是否为保存数据状态</param>
        private void UpdateWorkFlowDataFields(NameCollection dpTaskUsers, bool isSave)
        {
            double lastValue = DataForm1.GetLastValue(WorkflowContext.Current.DataFields["Order Number"].ToString());
            WorkflowContext.Current.DataFields["Last Value"] = lastValue;
            WorkflowContext.Current.DataFields["Status"] = isSave ? CAWorkflowStatus.Pending :
                                                                     CAWorkflowStatus.InProgress;
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = CreateWorkflowNumber();
            WorkflowContext.Current.DataFields["Applicant"] = CurrentEmployee.DisplayName +
                                                              "(" + CurrentEmployee.UserAccount + ")";
            WorkflowContext.Current.DataFields["Department"] = CurrentEmployee.Department;
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

            string taskTitle = CurrentEmployee.DisplayName + "'s Project Control Maintenance ";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskTitle", "please complete Project Control creation");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskTitle", taskTitle + "'s Project Control Maintenance");
            WorkflowContext.Current.UpdateWorkflowVariable("CfoTaskTitle", taskTitle + "'s Project Control Creation needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnlystTaskTitle", taskTitle + "'s Project Control Creation needs confirm");
            //更新各步骤URL
            var editURL = "/_Layouts/CA/WorkFlows/InternalOrderMaintenance2/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/InternalOrderMaintenance2/ApproveForm.aspx";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskFormUrl", editURL);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskFormUrl", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnlystTaskFormUrl", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("CfoTaskFormUrl", approveURL);
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

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        /// <summary>
        /// 获取工作流编号
        /// </summary>
        /// <returns></returns>
        private static string CreateWorkflowNumber()
        {
            return "IOM_" + WorkFlowUtil.CreateWorkFlowNumber("InternalOrderMaintenance").ToString("000000");
        }

    }
}