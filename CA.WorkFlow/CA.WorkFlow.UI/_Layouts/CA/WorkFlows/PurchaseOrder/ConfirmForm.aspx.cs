namespace CA.WorkFlow.UI.PurchaseOrder
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using QuickFlow;

    public partial class ConfirmForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            if (!IsPostBack)
            {
                this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();

                DataForm1.IsHideFinanceNum = WorkflowContext.Current.Step.Equals("ChopTask", StringComparison.CurrentCultureIgnoreCase);
                DataForm1.IsCreateSapStep = WorkflowContext.Current.Step.Equals("ConfirmTask", StringComparison.CurrentCultureIgnoreCase);
            }

            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Chop", StringComparison.CurrentCultureIgnoreCase)
                || e.Action.Equals("Create SAP PO", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                switch (WorkflowContext.Current.Step)
                {
                    case "ChopTask":
                        NameCollection financeManager = GetFinanceuser();// PurchaseOrderCommon.GetTaskUsers("wf_Finance_PO");
                        if (financeManager == null || financeManager.Count == 0)
                        {
                            DisplayMessage("Can not find people from finance po group, please contact IT for help.");
                            e.Cancel = true;
                            return;
                        }
                        context.UpdateWorkflowVariable("ConfirmTaskUsers", financeManager);

                        var legals = UserProfileUtil.UserListInGroup("wf_Legal").ToArray();
                        fields["Approvers"] = ReturnAllApprovers(CurrentEmployee.UserAccount);
                        fields["Approvers"] = ReturnAllApprovers(legals);
                        fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", CurrentEmployee.UserAccount);
                        fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", legals);
                        break;
                    case "ConfirmTask":

                        string sPoNO = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
                        bool isCapex = PurchaseOrderCommon.IsComPex(sPoNO);
                        string sUserGroup = string.Empty;
                        if (isCapex)
                        {
                            sUserGroup="wf_Finance_PO_Capex";
                        }
                        else
                        {
                            sUserGroup="wf_Finance_PO";
                        }

                        context.DataFields["Status"] = CAWorkflowStatus.Completed;
                        var finances = UserProfileUtil.UserListInGroup(sUserGroup).ToArray();
                        fields["Approvers"] = ReturnAllApprovers(CurrentEmployee.UserAccount);
                        fields["Approvers"] = ReturnAllApprovers(finances);
                        fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", CurrentEmployee.UserAccount);
                        fields["ApproversSP"] = ReturnAllApproversSP("ApproversSP", finances);
                        DataForm1.AddFinanceComments();
                        if (e.Action.Equals("Create SAP PO", StringComparison.CurrentCultureIgnoreCase))
                        {
                            AddPaymentLink(fields);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (!Validate(e.Action, e))
                {
                    return;
                }
                context.DataFields["Status"] = CAWorkflowStatus.Rejected;
                context.UpdateWorkflowVariable("CompleteTaskTitle", fields["WorkflowNumber"].ToString() + ": Please complete the rejected PO");
            }


            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
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

        /// <summary>
        /// 添加支付相关链接
        /// </summary>
        /// <param name="fields"></param>
        void AddPaymentLink(WorkflowDataFields fields)
        {
            string sPONO = fields["PONumber"].ToString();
            if (sPONO.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            SPFieldUrlValue linkInstall = new SPFieldUrlValue();
            linkInstall.Description = "Payment";
            linkInstall.Url = string.Concat("/WorkFlowCenter/lists/PaymentRequestItems/NewForm.aspx?PONO=", sPONO, "&IsFromPO=ture");

            SPFieldUrlValue linkHistory = new SPFieldUrlValue();
            linkHistory.Description = "Payment History";
            linkHistory.Url = string.Concat("/WorkFlowCenter/_layouts/ca/workflows/PaymentRequest/HistoryForm.aspx?PONO=" , sPONO, "&IsFromPO=ture");

            fields["Installment"] = linkInstall;
            fields["InstallmentHistory"] = linkHistory;
        }

        /// <summary>
        /// 得到财务审批人
        /// </summary>
        /// <returns></returns>
        NameCollection GetFinanceuser()
        {
            string sPoNO = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
            bool isCapex = PurchaseOrderCommon.IsComPex(sPoNO);
            if (isCapex)
            {
                return PurchaseOrderCommon.GetTaskUsers("wf_Finance_PO_Capex");
            }
            else
            {
                return PurchaseOrderCommon.GetTaskUsers("wf_Finance_PO"); 
            }
        }

    }
}