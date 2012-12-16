using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Collections.Generic;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;

namespace CA.WorkFlow.UI.PaymentRequestSAP
{
    public partial class FinanceConfirm : CAWorkFlowPage
    {
        #region Load Method

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                this.TaskTrace1.Applicant = fields["Applicant"].ToString();
            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
        }

        #endregion

        #region WorkFlow Action Execute Event

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                fields["Status"] = CAWorkflowStatus.Completed;
                //更新状态
                PaymentRequestSAPCommon.BatchUpdateSAPItems(this.DataForm1.ItemTable);
            }
            else
            {
                fields["Status"] = CAWorkflowStatus.Rejected;
                NameCollection acAccounts = null;
                if (fields["FromPOStatus"].ToString() == "1")
                {
                    if (fields["RequestType"].AsString().ToLower() == "opex")
                    {
                        acAccounts = WorkFlowUtil.GetUsersInGroup(CA.WorkFlow.UI.PaymentRequest.PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview);
                    }
                    else 
                    {
                        acAccounts = WorkFlowUtil.GetUsersInGroup(CA.WorkFlow.UI.PaymentRequest.PaymentRequestGroupNames.Capex_ConstructionPO_SAPReview);
                    }
                }
                else
                {
                    acAccounts = WorkFlowUtil.GetUsersInGroup(CA.WorkFlow.UI.PaymentRequest.PaymentRequestGroupNames.Opex_GeneralPO_SAPReview);
                }

                context.UpdateWorkflowVariable("ACReviewUsers", GetDelemanNameCollection(acAccounts, Constants.CAModules.EmployeeExpenseClaimSAP));
            }

            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        #endregion

    }
}