namespace CA.WorkFlow.UI.TravelRequest2
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;

    public partial class DataView : TravelRequest2Control
    {
        private string requestId;

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        public string msg { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(requestId))
                {
                    return;
                }
                this.rptVehicle.DataSource = GetDataTable(requestId, "Travel Vehicle Info2");
                this.rptVehicle.DataBind();

                this.rptTravel.DataSource = GetDataTable(requestId, "Travel Details2");
                this.rptTravel.DataBind();

                this.rptHotel.DataSource = GetDataTable(requestId, "Travel Hotel Info2");
                this.rptHotel.DataBind();
            }
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