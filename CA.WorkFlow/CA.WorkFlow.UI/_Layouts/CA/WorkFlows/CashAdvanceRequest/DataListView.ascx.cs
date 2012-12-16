using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;
namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class DataListView : QFUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                lblWorkFlowNumber.Text = fields["CAWorkflowNumber"].ToString();
                lblDept.Text = fields["Department"].ToString();
                lblRequestedID.Text = fields["EmployeeID"].ToString();
                lblRequestedBy.Text = fields["EmployeeName"].ToString();
                lblAmount.Text = fields["Amount"].ToString();
                lblEmployeeVendor.Text = "-" + fields["Amount"].ToString();
                lblSAPNo.Text = fields["SAPNumber"].AsString();
                lbTotalAmount.Text = fields["Amount"].ToString();
                lblAdvanceRemark.Text = fields["AdvanceRemark"].ToString();
                lblTerm.Text = fields["AdvanceType"].ToString();
             }
        }
    }
}