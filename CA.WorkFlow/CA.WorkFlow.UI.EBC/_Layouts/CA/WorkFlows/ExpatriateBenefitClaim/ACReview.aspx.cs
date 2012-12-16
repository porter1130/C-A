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

namespace CA.WorkFlow.UI.EBC
{
    public partial class ACReview : CAWorkFlowPage
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
            this.btnSave.Click += this.btnSave_Click;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem curItem = SPContext.Current.ListItem;
            curItem["ExpenseDescription"] = this.DataForm1.ExpenseDescription;
            curItem.Web.AllowUnsafeUpdates = true;
            curItem.Update();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            this.DataForm1.Update();

            ExpatriateBenefitClaimCommon.AddItemTable(this.DataForm1);
            ExpatriateBenefitClaimCommon.DeleteAllDraftSAPItems(fields["WorkflowNumber"].AsString());
            ExpatriateBenefitClaimCommon.SaveSAPItemsDetails(this.DataForm1, fields["WorkflowNumber"].AsString());
            RedirectToTask();
        }



        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                fields["ExpenseDescription"] = this.DataForm1.ExpenseDescription;
                this.DataForm1.Update();
                ExpatriateBenefitClaimCommon.AddItemTable(this.DataForm1);
                ExpatriateBenefitClaimCommon.DeleteAllDraftSAPItems(fields["WorkflowNumber"].AsString());
                ExpatriateBenefitClaimCommon.SaveSAPItemsDetails(this.DataForm1, fields["WorkflowNumber"].AsString());

                fields["Status"] = "Confirm";
                NameCollection financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(ExpatriateBenefitClaimConstants.wf_EBC_FinanceManager);
                context.UpdateWorkflowVariable("FinanceConfirmUsers", GetDelemanNameCollection(financeConfirmAccounts, ExpatriateBenefitClaimConstants.ExpatriateBenefitClaimSAP));
                AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
            }
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

    }
}