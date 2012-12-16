using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.ComponentModel;
using QuickFlow.Core;
using QuickFlow;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class UploadExcelBill : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;

        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = context.DataFields;

            context.UpdateWorkflowVariable("UploadBillTaskTitle", "Please upload credit card e-statement");
            context.UpdateWorkflowVariable("UploadBillTaskFormURL","/_Layouts/CA/WorkFlows/CreditCardClaim/UploadExcelBill.aspx");

             var taskUsers = new NameCollection();
            List<string> groupUsers = null;

            groupUsers = WorkFlowUtil.UserListInGroup(WorkflowGroupName.WF_FinanceConfirm);
            taskUsers.AddRange(groupUsers.ToArray());

            ReturnAllApproversSP("TaskUsers", groupUsers.ToArray());

            context.UpdateWorkflowVariable("UploadBillTaskUsers", taskUsers);

        }

    }
}