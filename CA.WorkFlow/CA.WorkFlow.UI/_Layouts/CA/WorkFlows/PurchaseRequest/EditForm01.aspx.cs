namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.ComponentModel;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using QuickFlow;
    using QuickFlow.UI.Controls;
    using CA.SharePoint;
    using System.Data;

    public partial class EditForm01 : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();

            CheckSecurity();

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (!IsPostBack)
            {                
                this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
               // DataForm1.TotalRMB = fields["TotalRMB"].AsString();
               // DataForm1.ApprovalTotalRMB = fields["ApprovalTotalRMB"].AsString();
                DataForm1.RequestType = fields["RequestType"].AsString();
                DataForm1.CapexType = fields["CapexType"].AsString();
                DataForm1.FormType = fields["FormType"].AsString();
                DataForm1.HOPurposeType = fields["PRHOPurpose"].AsString();
                DataForm1.StorePurposeType = fields["PRStorePurpose"].AsString();
                DataForm1.Applicant = fields["Applicant"].AsString();
                
            }

            bool isHO = (bool)fields["IsHO"];
            DataForm1.IsHO = isHO;
           // dfSelectItem.IsHO = isHO;

            this.DataForm1.Mode = "Edit";

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            this.Actions.OnClientClick = "return beforeSubmit(this)";
        }

        private void CheckSecurity()
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!PurchaseRequestCommon.isAdmin()&&!SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }
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

        private void SaveAction(object sender, ActionEventArgs e, DataTable dtPRData)
        {
            
                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                bool isHO = Convert.ToBoolean(fields["IsHO"]);
                var formType = DataForm1.FormType;
                var storePurpose = DataForm1.StorePurposeType;

                this.DataForm1.Update(); //验证通过更新DataTable

                #region Check QuarterlyOrder Count
                bool isQuartOrderOK = CheckQuartOrder(isHO, storePurpose, fields["Created"].AsString());
                if (!isQuartOrderOK) //if (!isHO && !PurchaseRequestCommon.IsLegalStoreRequest(storePurpose, CurrentEmployee.UserAccount))//是门店并且不合法的季度订单
                {
                    DataForm1.UpdateItemForQuartOrder();
                    DisplayMessage("Can not request quarterly order in current month or you have requested it.");
                    return;
                }
                #endregion

                #region Save the data
                var requestType = DataForm1.RequestType;
                var capexType = DataForm1.CapexType;
                var itemIdentity = DataForm1.ItemIdentity;

                PurchaseRequestCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
                PurchaseRequestCommon.SaveDetails(dtPRData, fields["WorkflowNumber"].AsString(), capexType, requestType, isHO, itemIdentity); //Save request details to lists

                //fields["Total"] = float.Parse(DataForm1.Total);

                fields["TotalRMB"] = DataForm1.GetTotalRMB(dtPRData);
                fields["ApprovalTotalRMB"] = DataForm1.GetApprovalTotalRMB(dtPRData);
                fields["RequestType"] = requestType;
                fields["CapexType"] = capexType;
                fields["FormType"] = formType;
                fields["PRHOPurpose"] = DataForm1.HOPurposeType;
                fields["PRStorePurpose"] = DataForm1.StorePurposeType;
                #endregion
             
                //context.SaveTask();
                //RedirectToTask();
            
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;


            DataTable dtPRData = DataForm1.dtGetPRData();
            if (e.Action.Equals("Save", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!this.DataForm1.ValidateForSave())
                {
                    DisplayMessage(this.DataForm1.MSG);
                    e.Cancel = true;
                    DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                    DataForm1.RebindItems(dtPRData);
                    return;
                }
                SaveAction(sender, e, dtPRData);
            } 
            else if (e.Action.Equals("Submit", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!this.DataForm1.Validate())
                {
                    DisplayMessage(DataForm1.MSG);
                    e.Cancel = true;
                    DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                    DataForm1.RebindItems(dtPRData);
                    return;
                }

                
                bool isHO = (bool)fields["IsHO"];
                var formType = DataForm1.FormType;
                var storePurpose = DataForm1.StorePurposeType;

                this.DataForm1.Update(); //验证通过更新DataTable

                #region Check QuarterlyOrder Count
                bool isQuartOrderOK = CheckQuartOrder(isHO, storePurpose, fields["Created"].AsString());
                if (!isQuartOrderOK) //if (!isHO && !PurchaseRequestCommon.IsLegalStoreRequest(storePurpose, CurrentEmployee.UserAccount))//是门店并且不合法的季度订单
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
                    string taskAssignType = DataForm1.GetTaskAssignType(dtPRData);
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

                #region Save the data
                var requestType = DataForm1.RequestType;
                var capexType = DataForm1.CapexType;
                var itemIdentity = DataForm1.ItemIdentity;

                PurchaseRequestCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString()); //Delete all draft items before saving
                PurchaseRequestCommon.SaveDetails(dtPRData, fields["WorkflowNumber"].AsString(), capexType, requestType, isHO, itemIdentity); //Save request details to lists

                //float total = float.Parse(DataForm1.Total);
                double totalRMB = DataForm1.GetTotalRMB(dtPRData);
                float approvalTotalRMB = float.Parse(DataForm1.GetApprovalTotalRMB(dtPRData));
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

                #region SkipApproveCondition
                bool isSkipApprove = false;
                //若门店申请，则只能选择StoreRequest;若HO申请，则可以选择除StoreRequest外的其他三项
                //即PaperBagRequest和HORequest肯定是HO用户的申请
                switch (DataForm1.FormType)
                {
                    case "PaperBag":
                        isSkipApprove = true;//纸袋已线下审批，无需PR工作流再次审批
                        break;
                    case "HO":
                        isSkipApprove = DataForm1.HOPurposeType.Equals("Department");//当HO代其他部门申请时，则无需PR工作流审批
                        break;
                    default:
                        break;
                }
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

                context.UpdateWorkflowVariable("IsSave", false);
                context.DataFields["Status"] = CAWorkflowStatus.InProgress;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }
        
        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
        /// <summary>
        /// 验证是否是合法的季度订单
        /// </summary>
        /// <param name="isHO"></param>
        /// <param name="sStorePurpose"></param>
        /// <returns></returns>
        bool CheckQuartOrder(bool isHO, string sStorePurpose, string sDate)
        {
            DateTime dtPRCreated = DateTime.Now;
            if (!DateTime.TryParse(sDate, out dtPRCreated))
            {
                DisplayMessage("PR Created date error.");
                return false;
            }

            bool isOK = true;
            if (!isHO && !PurchaseRequestCommon.IsLegalStoreRequest(sStorePurpose, dtPRCreated))//是门店并且不合法的季度订单
            {
                DataForm1.UpdateItemForQuartOrder();
                DisplayMessage("Can not request quarterly order in current month or you have requested it.");
                isOK = false;
            }
            return isOK;
        }
    }
}