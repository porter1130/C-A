using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using Microsoft.SharePoint;
using QuickFlow.UI.Controls;
using System.ComponentModel;
using CA.SharePoint;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;
            this.TaskTrace1.Applicant = TravelExpenseClaimForSAPCommon.ReturnApplicant(WorkflowContext.Current.DataFields["TCWorkflowNumber"].AsString());
            this.Actions.OnClientClick = "return beforeSubmit(this)";
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            string taskTitle = string.Empty;
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            var msg = this.DataForm1.ValidateForSubmit();
            if (msg.IsNotNullOrWhitespace())
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            #region Set users for workflow
            WorkflowContext.Current.UpdateWorkflowVariable("ReviewTaskUsers", TravelExpenseClaimForSAPCommon.GetTaskUsersWithoutDeleman(WorkflowGroupName.WF_FinanceManager));
            WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
            #endregion

            //Set ReviewTask title for workflow    
            taskTitle = fields["EnglishName"].AsString() + "'s Travel Expense Claim ";
            context.UpdateWorkflowVariable("ReviewTaskTitle", taskTitle + "needs review");

            context.UpdateWorkflowVariable("IsSave", false);
            fields["Status"] = CAWorkflowStatus.InProgress;

            //this.SendEmail("SubmitToApplicant");


            #region Save Details
            WorkFlowUtil.BatchDeleteItems(WorkflowListName.TravelExpenseClaimDetailsForSAP, fields["WorkflowNumber"].AsString());
            this.DataForm1.Update(); //Save the inputed data to datatable

            TravelExpenseClaimForSAPCommon.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString());
            #endregion

            #region Set page URL for workflow
            //Set page url         
            var reviewURL = WorkflowConfigName.TravelExpenseClaimForSAPUrl + "Review.aspx";
            context.UpdateWorkflowVariable("ReviewTaskFormURL", reviewURL);
            #endregion

        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            //var msg = this.DataForm1.ValidateForSave();
            //if (msg.IsNotNullOrWhitespace())
            //{
            //    DisplayMessage(msg);
            //    e.Cancel = true;
            //    return;
            //}
            fields["Status"] = CAWorkflowStatus.Pending;

            //Set ConfirmTask title for workflow
            context.UpdateWorkflowVariable("ConfirmTaskTitle", "Please complete Travel Expense Claim for SAP");
            // context.UpdateWorkflowVariable("IsSave", true);

            WorkFlowUtil.BatchDeleteItems(WorkflowListName.TravelExpenseClaimDetailsForSAP, fields["WorkflowNumber"].AsString());
            this.DataForm1.Update(); //Save the inputed data to datatable
            TravelExpenseClaimForSAPCommon.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString());

            context.SaveListItem();

            RedirectToTask();
        }
    }
}