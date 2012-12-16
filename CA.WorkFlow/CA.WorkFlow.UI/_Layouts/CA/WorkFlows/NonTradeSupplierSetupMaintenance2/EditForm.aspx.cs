namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance2
{
    using System;
    using System.ComponentModel;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using CA.SharePoint;
    using System.Collections.Generic;
    using Microsoft.SharePoint;
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
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;

            this.dfSelectVendor.Department = CurrentEmployee.Department;
            this.dfSelectVendor.ApplicantAccount = CurrentEmployee.UserAccount;
            this.DataForm1.DepartmentVal = CurrentEmployee.Department;
            this.DataForm1.ApplicantAccount = CurrentEmployee.UserAccount;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WorkflowContext.Current.SaveTask();
            RedirectToSaveTask();
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            var dpTaskUsers = new NameCollection();
            var paymentTerm = this.DataForm1.PaymentTerm;
            if (!this.DataForm1.Validate()){
                DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ?
                               this.DataForm1.msg : "Please fill in all the necessary fields.");
                e.Cancel = true;
                return;
            }

            dpTaskUsers = GetNextApproveTaskUsers();
            if (dpTaskUsers == null){
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
        /// <param name="isSave">是否为保存数据状态</param>
        private void UpdateWorkFlowDataFields(NameCollection dpTaskUsers)
        {
            WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;
            WorkflowContext.Current.DataFields["Manager"] = dpTaskUsers.Count > 0 ? dpTaskUsers[0] : "";
            //Update varivable.(Payment term less than 30 days required CFO's special approval)
            WorkflowContext.Current.DataFields["Payment Term"] = DataForm1.PaymentTerm;

            var DepartmentHeadTaskUsers = new NameCollection();
            var MDMTaskUsers = new NameCollection();
            var CFOTaskUsers = new NameCollection();

            string department = CurrentEmployee.Department;
            DepartmentHeadTaskUsers.Add(UserProfileUtil.GetDepartmentManager(department));

            List<string> lst = WorkFlowUtil.UserListInGroup("wf_CFO");
            CFOTaskUsers.AddRange(lst.ToArray());

            lst = WorkFlowUtil.UserListInGroup("wf_Finance_MDM");
            MDMTaskUsers.AddRange(lst.ToArray());

            //WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskUsers", DepartmentHeadTaskUsers);
            //WorkflowContext.Current.UpdateWorkflowVariable("MDMTaskUsers", MDMTaskUsers);
            //WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskUsers", CFOTaskUsers);

            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceDepartmentHeadTask, DepartmentHeadTaskUsers.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceMDMTask, MDMTaskUsers.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierSetupMaintenanceCFOTask, CFOTaskUsers.JoinString(","));
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
                manager = SetDefaultDepartManager();
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
            var deleman = WorkFlowUtil.GetDeleman(manager, Constants.CAModules.NonTradeSupplierSetupMaintenance);
            if (deleman != null)
            {
                nextApproveTaskUsers.Add(deleman);
            }

            return nextApproveTaskUsers;
        }

        private string SetDefaultDepartManager()
        {
            string manager = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList list = web.Lists["Department"];

                        SPQuery query = new SPQuery();
                        query.Query = string.Format(@"<Where>
                                    <Eq>
                                        <FieldRef Name='Title' />
                                        <Value Type='Text'>{0}</Value>
                                    </Eq>
                                </Where>", "Store Operations");
                        SPListItemCollection items = list.GetItems(query);

                        if (items.Count > 0)
                        {
                            manager = (new SPFieldLookupValue(items[0]["ManagerAccount"].AsString())).LookupValue;
                        }
                    }
                }
            });

            return manager;
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}