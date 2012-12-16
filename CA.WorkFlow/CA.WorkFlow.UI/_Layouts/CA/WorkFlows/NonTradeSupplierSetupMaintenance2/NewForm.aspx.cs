namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.SharePoint;
    using QuickFlow;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using CA.SharePoint;
    using SharePoint.Utilities.Common;

    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;

            this.dfSelectVendor.Department = CurrentEmployee.Department;
            this.dfSelectVendor.ApplicantAccount = CurrentEmployee.UserAccount;
            this.DataForm1.DepartmentVal = CurrentEmployee.Department;
            this.DataForm1.ApplicantAccount = CurrentEmployee.UserAccount;

            this.StartWorkflowButton2.OnClientClick = "return dispatchAction(this);";
        }

        //Save or Submit
        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            var dpTaskUsers = new NameCollection();
            var btn = sender as StartWorkflowButton;
            var isSave = string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase);
            if (isSave == false)
            {
                if (!this.DataForm1.Validate())
                {
                    DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ?
                                   this.DataForm1.msg : "Please fill in all the necessary fields.");
                    e.Cancel = true;
                    return;
                }

                dpTaskUsers = GetNextApproveTaskUsers();
                if (dpTaskUsers == null)
                {
                    DisplayMessage("The manager is not set in the system.");
                    e.Cancel = true;
                    return;
                }
            }

            //更新数据
            DataForm1.UpdateValues();
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
            WorkflowContext.Current.DataFields["Status"] = isSave ? CAWorkflowStatus.Pending :
                                                                     CAWorkflowStatus.InProgress;
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = CreateWorkflowNumber();
            WorkflowContext.Current.DataFields["Applicant"] = CurrentEmployee.DisplayName +
                                                              "(" + CurrentEmployee.UserAccount + ")";
            WorkflowContext.Current.DataFields["DepartmentVal"] = CurrentEmployee.Department;
            WorkflowContext.Current.DataFields["Manager"] = dpTaskUsers.Count > 0 ? dpTaskUsers[0] : "";
            //Update varivable.(Payment term less than 30 days required CFO's special approval)
            WorkflowContext.Current.DataFields["PaymentTerm"] = DataForm1.PaymentTerm;



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

            string taskTitle = CurrentEmployee.DisplayName + "'s Non-Trade Supplier Setup & Maintenance ";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskTitle", "please complete Supplier Setup & Maintenance");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskTitle", taskTitle);
            WorkflowContext.Current.UpdateWorkflowVariable("CfoTaskTitle", taskTitle);
            WorkflowContext.Current.UpdateWorkflowVariable("MdmTaskTitle", taskTitle);
            //更新各步骤URL
            var editURL = "/_Layouts/CA/WorkFlows/NonTradeSupplierSetupMaintenance2/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/NonTradeSupplierSetupMaintenance2/ApproveForm.aspx";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskFormUrl", editURL);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskFormUrl", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("MdmTaskFormUrl", approveURL);
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
                //Set default depart manager
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

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private static string CreateWorkflowNumber()
        {
            return "NTV_" + WorkFlowUtil.CreateWorkFlowNumber("NonTradeSupplierSetupMaintenance").ToString("000000");
        }

    }
}