using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Collections.Generic;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Data;

namespace CA.WorkFlow.UI.PaymentRequestSAP
{
    public partial class ACReview : CAWorkFlowPage
    {
        #region Load Method

        //AC Review页面加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                this.TaskTrace1.Applicant = fields["Applicant"].ToString();
            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;
        }

        #endregion

        #region Save SAP Items

        //保存SAP Items
        private void btnSave_Click(object sender, EventArgs e) 
        {
            SPListItem curItem = SPContext.Current.ListItem;
            curItem["PaymentDesc"] = this.DataForm1.ExpenseDescription;
            if (curItem["FromPOStatus"].ToString() == "0")
            {
                curItem["ExchRate"] = this.DataForm1.ExchangeRate;
                curItem["Currency"] = this.DataForm1.Currency;
            }
            curItem.Web.AllowUnsafeUpdates = true;
            curItem.Update();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.Update();
            PaymentRequestSAPCommon.AddItemTable(this.DataForm1);
            PaymentRequestSAPCommon.DeleteAllDraftSAPItems(fields["WorkflowNumber"].AsString());
            PaymentRequestSAPCommon.SaveSAPItemsDetails(this.DataForm1, fields["WorkflowNumber"].AsString());
            RedirectToTask();
        }

        #endregion

        #region WorkFlow Action Execute Event

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                this.DataForm1.Update();
                PaymentRequestSAPCommon.AddItemTable(this.DataForm1);
                PaymentRequestSAPCommon.DeleteAllDraftSAPItems(fields["WorkflowNumber"].AsString());
                PaymentRequestSAPCommon.SaveSAPItemsDetails(this.DataForm1, fields["WorkflowNumber"].AsString());
                fields["PaymentDesc"] = this.DataForm1.ExpenseDescription;
                fields["Status"] = "Confirm";

                if (fields["FromPOStatus"].ToString() == "0")
                {
                    fields["ExchRate"] = this.DataForm1.ExchangeRate;
                    fields["Currency"] = this.DataForm1.Currency;
                }

                NameCollection financeConfirmAccounts = null;
                if (fields["FromPOStatus"].ToString() == "1")
                {
                    if (fields["RequestType"].AsString().ToLower() == "opex")
                    {
                        financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(CA.WorkFlow.UI.PaymentRequest.PaymentRequestGroupNames.Opex_ConstructionPO_SAPConfirm);
                    }
                    else 
                    {
                        financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(CA.WorkFlow.UI.PaymentRequest.PaymentRequestGroupNames.Capex_ConstructionPO_SAPConfirm);
                    }
                }
                else 
                {
                    financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(CA.WorkFlow.UI.PaymentRequest.PaymentRequestGroupNames.Opex_GeneralPO_SAPConfirm);
                }
                 

                context.UpdateWorkflowVariable("FinanceConfirmUsers", GetDelemanNameCollection(financeConfirmAccounts, Constants.CAModules.EmployeeExpenseClaimSAP));
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
            }
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        #endregion

    }
}