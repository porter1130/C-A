using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Text;
using CA.SharePoint;

namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class FinanceConfirm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            }
            else
            {
                fields["Status"] = CAWorkflowStatus.Rejected;
                NameCollection acAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_Accountants);
                context.UpdateWorkflowVariable("ACReviewUsers", GetDelemanNameCollection(acAccounts, Constants.CAModules.CashAdvanceRequestSAP));
            }

            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "Approvers", "ApproversLoginName");
            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}