using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    public partial class Review : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.TaskTrace1.Applicant = TravelExpenseClaimForSAPCommon.ReturnApplicant(WorkflowContext.Current.DataFields["TCWorkflowNumber"].AsString());
            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {

            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            switch (e.Action)
            {
                case "Confirm":
                    fields["Status"] = CAWorkflowStatus.Completed;
                    break;
                case "Reject":
                    fields["Status"] = CAWorkflowStatus.Rejected;

                    context.UpdateWorkflowVariable("ConfirmTaskUsers", TravelExpenseClaimForSAPCommon.GetTaskUsersWithoutDeleman(WorkflowGroupName.WF_Accountants));
                    context.UpdateWorkflowVariable("ConfirmTaskTitle", "Please resubmit Travel Expense Claim for SAP");

                    break;
                default:
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

    }
}