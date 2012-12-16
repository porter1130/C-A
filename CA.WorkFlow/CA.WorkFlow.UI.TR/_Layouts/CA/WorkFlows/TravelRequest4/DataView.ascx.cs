namespace CA.WorkFlow.UI.TR
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;

    public partial class DataView : TravelRequest3Control
    {
        private string requestId;

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        private string _isSAPNoVisible;

        public string IsSAPNoVisible
        {
            get { return _isSAPNoVisible; }
            set { _isSAPNoVisible = value; }
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

            //SetSAPVisible();
        }

        private void SetSAPVisible()
        {
            IsSAPNoVisible = "false";
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (fields["SAPNumber"].AsString().IsNotNullOrWhitespace())
            {
                IsSAPNoVisible = "true";
            }
        }

        private string ReturnTargetFieldValue(string targetField, string queryField, string queryFieldValue, string listName)
        {
            string targetFieldValue = string.Empty;
            SPQuery query = new SPQuery();
            query.Query = WorkFlowUtil.GetQuery(queryField, queryFieldValue);

            SPListItemCollection items = SPContext.Current.Web.Lists[listName].GetItems(query);
            if (items.Count > 0)
            {
                targetFieldValue = items[0][targetField].AsString();
            }

            return targetFieldValue;
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