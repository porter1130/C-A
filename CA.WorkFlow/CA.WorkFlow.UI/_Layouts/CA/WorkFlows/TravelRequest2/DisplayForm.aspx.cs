namespace CA.WorkFlow.UI.TravelRequest2
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;

    public partial class DisplayForm : CAWorkFlowPage
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidateForView())
            {
                RedirectToTask();
            }

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();

            this.TaskTrace1.Applicant = fields["Applicant"].AsString();
        }
    }
}