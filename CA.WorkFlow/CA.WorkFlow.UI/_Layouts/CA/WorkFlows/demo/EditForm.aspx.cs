using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.demo
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);

            SPListItem curItem = SPContext.Current.ListItem;
            lblEmployeeName.Text = curItem["EmployeeName"] + "";
            lblOnBoardDate.Text = curItem["OnBoardDate"] + "";
            lblSn.Text = curItem["Title"] + "";
            lblManagementStatus.Text = curItem["ManagementStatus"] + "";
            lblHrStatus.Text = curItem["HrStatus"] + "";
            lblItStatus.Text = curItem["ItStatus"] + "";
            var flowStatus = curItem["FlowStatus"] + "";
            if(!string.IsNullOrEmpty(flowStatus))
            {
                lblFlowStatus.Text = string.Format("({0})", flowStatus);
            }
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            
            WorkflowContext curContext = WorkflowContext.Current;
            WorkflowDataFields fields = curContext.DataFields;

            SPWeb web = SPContext.Current.Web;
            SPUser curuser = web.CurrentUser;

            var now = DateTime.Now;

            switch (WorkflowContext.Current.Task.Step)
            {
                case "ManagementTask":
                    if (e.Action == "Approve")
                    {
                        fields["ManagementStatus"] = "approved";
                        fields["HrStatus"] = "pending";
                    }
                    else if (e.Action == "Reject")
                    {
                        fields["ManagementStatus"] = "rejected";
                        fields["FlowStatus"] = "completed";
                    }

                    fields["ManagementActedBy"] = curuser.LoginName;
                    fields["ManagementActedAt"] = now;
                    break;
                case "HrTask":
                    if (e.Action == "Confirm")
                    {
                        fields["HrStatus"] = "confirmed";
                        fields["ItStatus"] = "pending";
                    }

                    fields["HrActedBy"] = curuser.LoginName;
                    fields["HrActedAt"] = now;
                    break;
                case "ItTask":
                    if (e.Action == "Confirm")
                    {
                        fields["ItStatus"] = "confirmed";
                        fields["FlowStatus"] = "completed";
                    }

                    fields["ItActedBy"] = curuser.LoginName;
                    fields["ItActedAt"] = now;
                    break;
                    
            }

                
            
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }
    }
}