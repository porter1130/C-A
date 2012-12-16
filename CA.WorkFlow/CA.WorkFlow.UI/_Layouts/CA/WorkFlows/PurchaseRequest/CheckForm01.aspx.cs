namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using QuickFlow.UI.Controls;
    using System.Collections;
    using CA.SharePoint;
    using System.Data;

    public partial class CheckForm01 : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CheckSecurity();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (!IsPostBack)
            {
                
                DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                //DataForm1.TotalRMB = fields["TotalRMB"].AsString();
                //DataForm1.ApprovalTotalRMB = fields["ApprovalTotalRMB"].AsString();
                DataForm1.RequestType = fields["RequestType"].AsString();
                DataForm1.CapexType = fields["CapexType"].AsString();
                DataForm1.FormType = fields["FormType"].AsString();
                DataForm1.HOPurposeType = fields["PRHOPurpose"].AsString();
                DataForm1.StorePurposeType = fields["PRStorePurpose"].AsString();
                DataForm1.DisplayMode = "Display";
                DataForm1.Applicant = fields["Applicant"].AsString();
                this.DataForm1.Mode = "Check";
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();

                
            }

            bool isHO = (bool)fields["IsHO"];
            DataForm1.IsHO = isHO;
           // dfSelectItem.IsHO = isHO;

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
            if (!PurchaseRequestCommon.isAdmin() && !SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                fields["Approvers"] = ReturnAllApprovers(CurrentEmployee.UserAccount);
                fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", CurrentEmployee.UserAccount);

                DataTable dtPRData = DataForm1.dtGetPRData();
                if (!this.DataForm1.Validate()) 
                {
                    DisplayMessage(DataForm1.MSG);
                    e.Cancel = true;
                    DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                    DataForm1.RebindItems(dtPRData);
                    return;
                }

                #region Set users for workflow
                //Modify task users
                var manager = new NameCollection();
                manager.Add(fields["CurrManager"].ToString());
                var deleman = PurchaseRequestCommon.GetDeleman(fields["CurrManager"].ToString()); //查找代理人
                if (deleman != null)
                {
                    manager.Add(deleman);
                }
                context.UpdateWorkflowVariable("ApproveTaskUsers", manager);
                #endregion

                #region Save the data
                this.DataForm1.Update(); //Save the inputed data to datatable
                bool isHO = Convert.ToBoolean(fields["IsHO"]);
                var formType = DataForm1.FormType;
                var itemIdentity = DataForm1.ItemIdentity;

                //Delete all draft items before saving
                PurchaseRequestCommon.DeleteAllDraftItems(fields["WorkflowNumber"].AsString());
                //Save request details to lists
                PurchaseRequestCommon.SaveDetails(dtPRData, fields["WorkflowNumber"].AsString(), DataForm1.CapexType, DataForm1.RequestType, isHO, itemIdentity); 

                //float total = float.Parse(DataForm1.Total);
               /// float totalRMB = float.Parse(DataForm1.GetTotalRMB(dtPRData));
                float approvalTotalRMB = float.Parse(DataForm1.GetApprovalTotalRMB(dtPRData));
                //fields["Total"] = total;
                fields["TotalRMB"] = DataForm1.GetTotalRMB(dtPRData);// totalRMB;
                fields["ApprovalTotalRMB"] = approvalTotalRMB;
                fields["RequestType"] = DataForm1.RequestType;
                fields["CapexType"] = DataForm1.CapexType;
                fields["FormType"] = formType;
                fields["PRHOPurpose"] = DataForm1.HOPurposeType;
                fields["PRStorePurpose"] = DataForm1.StorePurposeType;
                fields["CheckPerson"] = CurrentEmployee.UserAccount;

                Hashtable ht = WorkFlowUtil.GetApproveLevel(approvalTotalRMB, "PR");
                if (ht.Count == 0)
                {
                    DisplayMessage("The system didn't set the approve level info.");
                    e.Cancel = true;
                    return;
                }
                //fields["HighLevel"] = ht["HighLevel"].ToString();
                fields["LowLevel"] = ht["LowLevel"].ToString();
                #endregion
            }
            else
            {
                if (!Validate(e.Action, e))
                {
                    return;
                }
                fields["Status"] = CAWorkflowStatus.Rejected;
                context.UpdateWorkflowVariable("CompleteTaskTitle", fields["WorkflowNumber"].ToString() + " :Please complete the rejected PR");
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);

        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private bool Validate(string action, ActionEventArgs e)
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
    }
}