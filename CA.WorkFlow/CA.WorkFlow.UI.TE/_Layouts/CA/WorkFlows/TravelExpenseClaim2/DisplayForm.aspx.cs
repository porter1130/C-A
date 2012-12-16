
namespace CA.WorkFlow.UI.TE
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;

    public partial class DisplayForm : CAWorkFlowPage
    {
        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string _step;

        public string Step
        {
            get { return _step; }
            set { _step = value; }
        }

        public string FapiaoStatus
        {
            get { return this.DataForm1.FapiaoStatus; }
        }
        public string InformationStatus
        {
            get { return this.DataForm1.InformationStatus; }
        }
        public string ClaimedAmtStatus
        {
            get { return this.DataForm1.ClaimedAmtStatus; }
        }
        public string OtherReasonsStatus
        {
            get { return this.DataForm1.OtherReasonsStatus; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
            //this.DataForm1.Mode = "Display";
            this.TaskTrace1.Applicant = fields["Applicant"].AsString();
            this.Status = fields["Status"].AsString();
            this.Step = fields["WorkflowStep"].AsString();
        }
    }
}