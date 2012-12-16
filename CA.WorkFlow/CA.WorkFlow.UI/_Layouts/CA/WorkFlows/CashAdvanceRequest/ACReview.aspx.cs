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
    public partial class ACReview : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem curItem = SPContext.Current.ListItem;
            curItem["Amount"] = this.DataForm1.Amount;
            curItem["AdvanceRemark"] = this.DataForm1.AdvanceRemark;
            curItem.Web.AllowUnsafeUpdates = true;
            curItem.Update();
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                fields["Status"] = "Confirm";
                fields["Amount"] = this.DataForm1.Amount;
                fields["AdvanceRemark"] = this.DataForm1.AdvanceRemark;
                fields["AdvanceType"] = this.DataForm1.Term;

                NameCollection financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_FinanceManager);
                context.UpdateWorkflowVariable("FinanceConfirmUsers", GetDelemanNameCollection(financeConfirmAccounts, Constants.CAModules.CashAdvanceRequestSAP));
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "Approvers", "ApproversLoginName");
            }

            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

    }
}