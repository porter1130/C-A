namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.ComponentModel;
    using QuickFlow.UI.Controls;
    using QuickFlow.Core;
    using Microsoft.SharePoint;
    using QuickFlow;
    using CA.SharePoint;
    using System.Collections;
    using CA.SharePoint.Utilities.Common;

    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();

            if (!this.IsPostBack)
            {
                DataForm1.Applicant = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
                
            }

            bool isHO = PurchaseRequestCommon.IsHO(CurrentEmployee.UserAccount);
            DataForm1.IsHO = isHO;
            dfSelectItem.IsHO = isHO;

            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton1.OnClientClick = "return beforeSubmit(this)";
            this.StartWorkflowButton2.OnClientClick = "return beforeSubmit(this)";
        }

        private void CheckAccount()
        {
            //门店和HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (PurchaseRequestCommon.IsStore(current) || PurchaseRequestCommon.isAdmin())
            {
                DataForm1.DisplayMode = string.Empty;
            }
            else if (PurchaseRequestCommon.IsHO(current))
            {
                DataForm1.DisplayMode = "Display";
            }
            else
            {
                RedirectToTask();
            }
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string taskTitle = CurrentEmployee.DisplayName + "'s Purchase Request ";
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            bool isHO = PurchaseRequestCommon.IsHO(CurrentEmployee.UserAccount);
            var formType = DataForm1.FormType;
            var storePurpose = DataForm1.StorePurposeType;

            //Check which button has been clicked
            var btn = sender as StartWorkflowButton;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!this.DataForm1.ValidateForSave())
                {
                    DisplayMessage(DataForm1.MSG);
                    e.Cancel = true;
                    DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                    DataForm1.RebindItems();
                    return;
                }

                this.DataForm1.Update(); //验证通过更新DataTable

                #region Check QuarterlyOrder Count
                if (!isHO && !PurchaseRequestCommon.IsLegalStoreRequest(storePurpose, CurrentEmployee.UserAccount))//是门店并且不合法的季度订单
                {
                    DataForm1.UpdateItemForQuartOrder();
                    DisplayMessage("Can not request quarterly order in current month or you have requested it.");
                    e.Cancel = true;
                    return;
                }
                #endregion

                context.UpdateWorkflowVariable("IsSave", true);
                context.DataFields["Status"] = CAWorkflowStatus.Pending;
            }
            else
            {
                if (!this.DataForm1.Validate())
                {
                    DisplayMessage(DataForm1.MSG);
                    e.Cancel = true;
                    DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                    DataForm1.RebindItems();
                    return;
                }

                this.DataForm1.Update(); //验证通过更新DataTable

                #region Check QuarterlyOrder Count
                if (!isHO && !PurchaseRequestCommon.IsLegalStoreRequest(storePurpose, CurrentEmployee.UserAccount))//是门店并且不合法的季度订单
                {
                    DataForm1.UpdateItemForQuartOrder();
                    DisplayMessage("Can not request quarterly order in current month or you have requested it.");
                    e.Cancel = true;
                    return;
                }
                #endregion

                #region Set users for workflow
                var managerEmp = WorkFlowUtil.GetNextConstructionApprover(CurrentEmployee);
                if (managerEmp == null)
                {
                    DisplayMessage("The manager is not set in the system.");
                    e.Cancel = true;
                    return;
                }
                if (isHO)
                {
                    var manager = new NameCollection();
                    manager.Add(managerEmp.UserAccount);
                    var deleman = PurchaseRequestCommon.GetDeleman(managerEmp.UserAccount); //查找代理人
                    if (deleman != null)
                    {
                        manager.Add(deleman);
                    }
                    context.UpdateWorkflowVariable("ApproveTaskUsers", manager);
                }
                else 
                {
                    string taskAssignType = DataForm1.TaskAssignType;
                    SPListItemCollection lc = null;
                    lc = WorkFlowUtil.GetCollectionByColumn("AssignType", taskAssignType, "PurchaseRequestTaskAssign");

                    NameCollection checkManager = new NameCollection();
                    Employee emp = null;
                    foreach (SPListItem item in lc)
                    {
                        emp = UserProfileUtil.GetEmployeeEx(item["TaskChecker"].ToString());
                        if (emp != null)
                        {
                            checkManager.Add(emp.UserAccount);
                        }
                    }

                    if (checkManager.Count == 0)
                    {
                        DisplayMessage("The HO checkers are not set in the system. Please contact IT for further help.");
                        e.Cancel = true;
                        return;
                    }
                    
                    //Modify task users
                    context.UpdateWorkflowVariable("CheckTaskUsers", checkManager);
                }

                fields["CurrManager"] = managerEmp.UserAccount;
                #endregion

                context.UpdateWorkflowVariable("IsSave", false);
                context.DataFields["Status"] = CAWorkflowStatus.InProgress;

                #region SkipApproveCondition
                bool isSkipApprove = false;
                //若门店申请，则只能选择StoreRequest;若HO申请，则可以选择除StoreRequest外的其他三项
                //即PaperBagRequest和HORequest肯定是HO用户的申请
                if (DataForm1.FormType == "HO")
                {
                    isSkipApprove = DataForm1.HOPurposeType.Equals("Department");//当HO代其他部门申请时，则无需PR工作流审批
                }

                /*switch (DataForm1.FormType)
                {
                    case "PaperBag":
                        isSkipApprove = true;//纸袋已线下审批，无需PR工作流再次审批
                        break;
                    case "HO":
                        isSkipApprove = DataForm1.HOPurposeType.Equals("Department");//当HO代其他部门申请时，则无需PR工作流审批
                        break;
                    default:
                        break;
                }*/
                context.UpdateWorkflowVariable("IsSkipApprove", isSkipApprove);//Paper bag和HO代其他部门填写是不需要审批的，直接可以生成PO了
                #endregion

                #region SkipApproveTaskAssign
                //若SkipApprove，则需直接指定最后Confirm的任务人
                if (isSkipApprove)
                {
                    var confirmManager = new NameCollection();
                    confirmManager.Add(CurrentEmployee.UserAccount);
                    fields["CheckPerson"] = CurrentEmployee.UserAccount;
                    context.UpdateWorkflowVariable("ConfirmTaskUsers", confirmManager);
                }
                #endregion
            }
            #region Save the data
            //Save data to master table and sub table
            var workflowNumber = CreateWorkflowNumber();
            workflowNumber = DataForm1.IsReturn ? workflowNumber + "R" : workflowNumber;
            fields["WorkflowNumber"] = workflowNumber;

            var requestType = DataForm1.RequestType;
            var capexType = DataForm1.CapexType;
            
            var itemIdentity = DataForm1.ItemIdentity;

            PurchaseRequestCommon.SaveDetails(this.DataForm1, workflowNumber, capexType, requestType, isHO, itemIdentity); //Save request details to lists

            //float total = float.Parse(DataForm1.Total);
            float totalRMB = float.Parse(DataForm1.TotalRMB);
            float approvalTotalRMB = float.Parse(DataForm1.ApprovalTotalRMB);
            fields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
            //fields["Total"] = total;
            fields["TotalRMB"] = totalRMB;
            fields["ApprovalTotalRMB"] = approvalTotalRMB;
            fields["RequestType"] = requestType;
            fields["CapexType"] = capexType;
            fields["FormType"] = formType;
            fields["PRHOPurpose"] = DataForm1.HOPurposeType;
            fields["PRStorePurpose"] = DataForm1.StorePurposeType;
            #endregion

            if (isHO)
            {
                var ht = WorkFlowUtil.GetApproveLevel(approvalTotalRMB, "PR");
                if (ht.Count == 0)
                {
                    DisplayMessage("The system didn't set the approve level info.");
                    e.Cancel = true;
                    return;
                }
                //fields["HighLevel"] = ht["HighLevel"].ToString();
                fields["LowLevel"] = ht["LowLevel"].ToString();
                fields["CheckPerson"] = CurrentEmployee.UserAccount;//HO发起人自己做最后一步的Confirm
            }

            fields["IsHO"] = isHO;

            #region Set title for workflow
            //Modify task title
            context.UpdateWorkflowVariable("CompleteTaskTitle", workflowNumber + " :Please complete the PR");
            context.UpdateWorkflowVariable("CheckTaskTitle", string.Format("{0} \"{1}\" needs check", taskTitle, workflowNumber));
            context.UpdateWorkflowVariable("ApproveTaskTitle", string.Format("{0} \"{1}\" needs approval", taskTitle, workflowNumber));
            context.UpdateWorkflowVariable("ConfirmTaskTitle", string.Format("{0} \"{1}\" needs generate PO", taskTitle, workflowNumber));
            #endregion

            #region Set page URL for workflow
            //Set task url
            var editURL = "/_Layouts/CA/WorkFlows/PurchaseRequest/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/PurchaseRequest/ApproveForm.aspx";
            var checkURL = "/_Layouts/CA/WorkFlows/PurchaseRequest/CheckForm.aspx";
            var confirmURL = "/_Layouts/CA/WorkFlows/PurchaseRequest/ConfirmForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("CheckTaskFormURL", checkURL);
            context.UpdateWorkflowVariable("ApproveTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormURL", confirmURL);
            #endregion

            //Update IsHO vari in wf. Different level person have different path
            context.UpdateWorkflowVariable("IsHO", isHO);

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private static string CreateWorkflowNumber()
        {
            return "PR" + WorkFlowUtil.CreateWorkFlowNumber("PurchaseRequest").ToString("000000");
        }
    }
}
