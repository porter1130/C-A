namespace CA.WorkFlow.UI.PurchaseOrder
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using Microsoft.SharePoint;
    using System.Collections.Generic;
    using CA.SharePoint;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true))
            {
                RedirectToTask();
            }

            if (!IsPostBack)
            {
                this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();

            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;

            if (e.Action.Equals("Submit", StringComparison.CurrentCultureIgnoreCase))
            {
                context.DataFields["Status"] = CAWorkflowStatus.InProgress;

                #region Set Workflow variable
                var chopTaskTitle = string.Format("Purchase Order \"{0}\" needs chop online.", context.DataFields["WorkflowNumber"].ToString());
                var confirmTaskTitle = string.Format("Purchase Order \"{0}\" needs create system PO.", context.DataFields["WorkflowNumber"].ToString());
                context.UpdateWorkflowVariable("ChopTaskTitle", chopTaskTitle);
                context.UpdateWorkflowVariable("ConfirmTaskTitle", confirmTaskTitle);

                var confirmURL = "/_Layouts/CA/WorkFlows/PurchaseOrder/ConfirmForm.aspx";
                context.UpdateWorkflowVariable("ChopTaskFormURL", confirmURL);
                context.UpdateWorkflowVariable("ConfirmTaskFormURL", confirmURL);
                #endregion

                #region Set Next Step Task Assigner
                bool isSkipChop = (bool)context.DataFields["IsSkipChop"];
                if (isSkipChop)
                {
                    var financeManager = PurchaseOrderCommon.GetTaskUsers("wf_Finance_PO");
                    if (financeManager == null || financeManager.Count == 0)
                    {
                        DisplayMessage("Can not find people from finance po group, please contact IT for help.");
                        e.Cancel = true;
                        return;
                    }
                    context.UpdateWorkflowVariable("ConfirmTaskUsers", financeManager);
                }
                else
                {
                    var chopManager = PurchaseOrderCommon.GetTaskUsers("wf_Legal");
                    if (chopManager == null || chopManager.Count == 0)
                    {
                        DisplayMessage("Can not find people from legal group, please contact IT for help.");
                        e.Cancel = true;
                        return;
                    }
                    context.UpdateWorkflowVariable("ChopTaskUsers", chopManager);
                }
                #endregion
            }
            else
            {
                context.DataFields["Status"] = CAWorkflowStatus.Pending;
            }
                this.DataForm1.SavePaymentData();// 保存分期付款数据。

                WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }
    }
}