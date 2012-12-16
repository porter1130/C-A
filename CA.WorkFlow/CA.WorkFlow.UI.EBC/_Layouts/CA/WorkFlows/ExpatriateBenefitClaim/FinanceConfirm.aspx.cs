using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Collections.Generic;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;

namespace CA.WorkFlow.UI.EBC
{
    public partial class FinanceConfirm : CAWorkFlowPage
    {
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

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                fields["Status"] = CAWorkflowStatus.Completed;
                //更新状态
                ExpatriateBenefitClaimCommon.BatchUpdateSAPItems(this.DataForm1.ItemTable);
            }
            else
            {
                fields["Status"] = CAWorkflowStatus.Rejected;
                NameCollection acAccounts = WorkFlowUtil.GetUsersInGroup(ExpatriateBenefitClaimConstants.wf_EBC_Accountants);
                context.UpdateWorkflowVariable("ACReviewUsers", GetDelemanNameCollection(acAccounts, ExpatriateBenefitClaimConstants.ExpatriateBenefitClaimSAP));
            }

            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

    }
}