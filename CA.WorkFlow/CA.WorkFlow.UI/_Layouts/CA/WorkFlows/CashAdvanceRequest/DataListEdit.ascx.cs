using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class DataListEdit : QFUserControl
    {
        private string amount;

        public string Amount
        {
            get { return this.txtAmount.Text.Trim(); }
        }

        public string Term 
        {
            get { return this.dplTerm.SelectedValue; }
        }

        public string AdvanceRemark
        {
            get { return this.txtAdvanceRemark.Text.Trim(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!Page.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.dplTerm.SelectedValue = fields["AdvanceType"].ToString();
                lblWorkFlowNumber.Text = fields["CAWorkflowNumber"].ToString();
                lblDept.Text = fields["Department"].ToString();
                lblRequestedID.Text = fields["EmployeeID"].ToString();
                lblRequestedBy.Text = fields["EmployeeName"].ToString();
                txtAmount.Text = fields["Amount"].ToString();
                hfAmount.Value = fields["Amount"].ToString();
                txtEmployeeVendor.Text = "-" + fields["Amount"].ToString();
                lbTotalAmount.Text = fields["Amount"].ToString();
                txtAdvanceRemark.Text = fields["AdvanceRemark"].ToString().Split(';').ToList<string>()[0];
            }
        }
    }
}
