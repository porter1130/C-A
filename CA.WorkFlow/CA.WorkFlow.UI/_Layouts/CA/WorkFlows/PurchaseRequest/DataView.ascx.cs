namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Web.UI;
    using SharePoint.Utilities.Common;
    using QuickFlow.Core;

    public partial class DataView : BaseWorkflowUserControl
    {
        private string msg;
        public string MSG { get { return msg; } }

        private string requestId;
        public string RequestId { set { this.requestId = value; } }

        private string displayMode;
        public string DisplayMode { get { return this.displayMode; } set { this.displayMode = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.rptItem.DataSource = PurchaseRequestCommon.GetDataTable(requestId);
                this.rptItem.DataBind();
            }
            this.hidDisplayMode.Value = displayMode;
        }

        public override bool Validate(string action)
        {
            bool isValid = false;
            if (action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                isValid = WorkflowContext.Current.TaskFields["Body"].AsString().IsNotNullOrWhitespace();
                if (!isValid)
                {
                    msg = "Please fill in the Reject Comments.";
                    return isValid;
                }
            }
            return true;
        }
    }
}