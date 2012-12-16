using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using Microsoft.SharePoint;
using QuickFlow;
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint;

using CA.SharePoint.Utilities.Common;
namespace CA.WorkFlow.UI.demo
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;

           

        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string taskTitle = string.Empty;
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            //Check which button has been clicked
            var btn = sender as StartWorkflowButton;

            context.UpdateWorkflowVariable("IsSubmit", true);

            #region Set users for workflow
            var manager = new NameCollection();


            List<string> groupUsers = WorkFlowUtil.UserListInGroup("wf_IT");
            manager.AddRange(groupUsers.ToArray());

            WorkflowContext.Current.UpdateWorkflowVariable("ApproveTaskUsers", manager);
            #endregion

            //Set NextApproveTask title for workflow    
            taskTitle = "DEMO Workflow";
            context.UpdateWorkflowVariable("ApproveTaskTitle", taskTitle + "needs approval");

            SPQuery query = new SPQuery();
            query.Query = @"<OrderBy>
                             <FieldRef Name='ID' Ascending='False' />
                          </OrderBy>";

            fields["Title"] = WorkFlowUtil.CreateWorkFlowNumber("DEMO_");
            #region Set page URL for workflow
            //Set page url
            var URL = "/_Layouts/CA/WorkFlows/demo/ApproveForm.aspx";

            context.UpdateWorkflowVariable("ApproveTaskFormURL", URL);

            #endregion
            //"DEMO_" +(int.Parse( SPContext.Current.List.GetItems(query)[0]["ID"].AsString())+1);
        }


        

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        

    }
}