using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Collections.Generic;
using QuickFlow.Core;
using System.Collections;

namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    public partial class DataListView : BaseWorkflowUserControl
    {
        #region Fields
        private string requestId;
        public string RequestId { set { this.requestId = value; } }
        internal DataTable ItemTable
        {
            get
            {
                return (this.ViewState["ItemTable"] as DataTable) ?? CreateItemTable();
            }
            set
            {
                this.ViewState["ItemTable"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                LoadExpenseType();
                DataTable itemDetails = EmployeeExpenseClaimCommon.GetDataTableToSAP(requestId);

                this.rptItem.DataSource = itemDetails;
                this.rptItem.DataBind();

                 this.ItemTable.Rows.Clear();
                 foreach (DataRow dr in itemDetails.Rows)
                 {
                     DataRow row = this.ItemTable.Rows.Add();
                     row["ID"] = dr["ID"].ToString();
                     row["ExpenseType"] = dr["ExpenseType"].ToString();
                     row["CostCenter"] = dr["CostCenter"].ToString();
                     row["ItemAmount"] = dr["ItemAmount"].ToString();
                     row["GLAccount"] = dr["GLAccount"].ToString();
                 }

                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                txtExpenseDescription.Text = fields["ExpenseDescription"].ToString();
                lbTotalAmount.Text = fields["TotalAmount"].AsString();
                lblCashAdvanceAmount.Text = fields["CashAdvanceAmount"].AsString();
                lblPreTotalAmount.Text = fields["PreTotalAmount"].AsString();
                lblWorkFlowNumber.Text = fields["EECWWorkflowNumber"].AsString();
                lblSAPNo.Text = fields["SAPNumber"].AsString();
                string name = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                lblRequestedID.Text = employee.EmployeeID;
                lblRequestedBy.Text = employee.DisplayName;
            }
        }

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ID");
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("ItemAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");
            ItemTable.Rows.Add();
            return ItemTable;
        }

        internal Hashtable OriginalExpenseType
        {
            get
            {
                return this.ViewState["OriginalExpenseType"] as Hashtable;
            }
            set
            {
                this.ViewState["OriginalExpenseType"] = value;
            }
        }

        private void LoadExpenseType()
        {
            DataTable dt = WorkFlowUtil.GetCollectionByList("Expense Claim SAP GLAccount").GetDataTable();
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["ExpenseType"].ToString(), dr["NewExpenseType"].ToString());
            }
            OriginalExpenseType = ht;
        }

        protected void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var lblExpenseType = (Label)item.FindControl("lblExpenseType");
                    lblExpenseType.Text = OriginalExpenseType[row["ExpenseType"].ToString()].AsString();
                }
            }
        }
    }
}