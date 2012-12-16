using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;
using System.Web.Script.Serialization;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    public partial class DataView : System.Web.UI.UserControl
    {
        private string _isSAPNoVisible;

        public string IsSAPNoVisible
        {
            get { return _isSAPNoVisible; }
            set { _isSAPNoVisible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                SourceDataBind(fields["WorkflowNumber"].AsString(), WorkflowListName.TravelExpenseClaimDetailsForSAP);
                LoadSourceData(fields["TCWorkflowNumber"].AsString());
            }
            SetSAPVisible();
        }

        private void LoadSourceData(string id)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();

            if (id.IsNotNullOrWhitespace())
            {
                SPListItemCollection travelExpenseClaimItems = TravelExpenseClaimForSAPCommon.GetDataCollection(id, WorkflowListName.TravelExpenseClaim);

                List<object> travelExpenseClaimInfo = TravelExpenseClaimForSAPCommon.GetSerializingList(travelExpenseClaimItems, new TravelExpenseClaimItem());

                hidTravelDetails.Value = oSerializer.Serialize(travelExpenseClaimInfo);

            }
        }

        private void SetSAPVisible()
        {
            if (WorkflowContext.Current.DataFields["SAPNo"].AsString().IsNotNullOrWhitespace())
            {
                this.lblSapNumber.Text = WorkflowContext.Current.DataFields["SAPNo"].AsString();
                IsSAPNoVisible = "true";
            }
            else
            {
                IsSAPNoVisible = "false";
            }
        }

        private void SourceDataBind(string workflowNumber, string listName)
        {
            DataTable dt = TravelExpenseClaimForSAPCommon.GetDataTable(workflowNumber, listName);
            if (dt != null)
            {
                rptExpense.DataSource = dt;
                rptExpense.DataBind();
            }
        }
    }
}