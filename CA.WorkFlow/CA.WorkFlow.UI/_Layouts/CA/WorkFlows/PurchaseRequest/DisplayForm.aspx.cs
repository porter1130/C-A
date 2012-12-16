namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;

    public partial class DisplayForm : CAWorkFlowPage
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();

            //if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    //spsadmin will ignore the security check
            //}
            //else if (!SecurityValidateForView())
            //{
            //    RedirectToTask();
            //}

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
            this.TaskTrace1.Applicant = fields["Applicant"].AsString();
        }

        private void CheckAccount()
        {
            //Legal,门店和HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (PurchaseRequestCommon.IsStore(current) || PurchaseRequestCommon.isAdmin())
            {
                DataForm1.DisplayMode = string.Empty;
            }
            else if (PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_HO", "wf_Legal", "wf_Finance_PO" }) || PurchaseRequestCommon.isAdmin())
            {
                DataForm1.DisplayMode = "Display";
            }
            else if (WorkflowContext.Current.DataFields["Approvers"].AsString().Contains(current))
            {
                DataForm1.DisplayMode = "Display";
            }
            else
            {
                RedirectToTask();
            }
        }
    }
}