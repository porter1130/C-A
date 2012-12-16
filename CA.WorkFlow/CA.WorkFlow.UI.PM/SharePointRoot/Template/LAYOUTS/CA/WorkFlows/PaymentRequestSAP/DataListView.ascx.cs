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

namespace CA.WorkFlow.UI.PaymentRequestSAP
{
    public partial class DataListView : BaseWorkflowUserControl
    {
        #region Fields

        private string requestId;

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

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
        internal DataTable ExpenseTypeMapping
        {
            get
            {
                return this.ViewState["ExpenseTypeMapping"] as DataTable;
            }
            set
            {
                this.ViewState["ExpenseTypeMapping"] = value;
            }
        }
        #endregion

        #region Method

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                LoadExpenseType();
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                //string et = "";
                if (fields["RequestType"].AsString().ToLower() == "opex")
                {
                    //et = "opex";
                    this.lblGLAccount.Text = "GL Account";
                    this.lblExpenseType.Text = "Expense Type";
                    this.hfStatus.Value = "opex";
                }
                else
                {
                    //et = "capex";
                    this.lblGLAccount.Text = "Asset Class";
                    this.lblExpenseType.Text = "Asset Type";
                    this.hfStatus.Value = "capex";
                }
                ExpenseTypeMapping = WorkFlowUtil.GetCollectionByList("PaymentRequestExpenseTypeMapping").GetDataTable();
                DataTable itemDetails = PaymentRequestSAPCommon.GetDataTableToSAP(requestId);
                //this.rptItem.DataSource = itemDetails;
                this.rptItem.DataSource = GetItemDetailsTable(itemDetails);
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
               
                txtExpenseDescription.Text = fields["PaymentDesc"].ToString();
                lbTotalAmount.Text = fields["PreTotalAmount"].AsString();
                //lblCashAdvanceAmount.Text = fields["CashAdvanceAmount"].AsString();
                //lblPreTotalAmount.Text = fields["PreTotalAmount"].AsString();
                lblWorkFlowNumber.Text = fields["PRWorkflowNumber"].AsString();
                lblSAPNo.Text = fields["SAPNumber"].AsString();
                string name = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                lblRequestedID.Text = employee.EmployeeID;
                lblRequestedBy.Text = employee.DisplayName;
                this.lblCurrency.Text = fields["Currency"].AsString();
                this.lblExchangeRate.Text = fields["ExchRate"].AsString();
                this.lblAmountCurrency.Text = WorkflowContext.Current.DataFields["Currency"].AsString();
            }
        }

        private DataTable GetItemDetailsTable(DataTable itemDetails)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ExpenseType");
            dt.Columns.Add("ItemAmount");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("GLAccount");
            dt.Columns.Add("AssetNo");
            dt.Columns.Add("BusinessArea");
            foreach (DataRow dr in itemDetails.Rows)
            {
                DataRow dataRow = dt.Rows.Add();
                //dataRow["ExpenseType"] = dr["ExpenseType"].ToString();
                dataRow["ExpenseType"] = PaymentRequestSAPCommon.GetExpenseTypeByExpenseType(dr["ExpenseType"].ToString(), ExpenseTypeMapping);
                dataRow["ItemAmount"] = dr["ItemAmount"].ToString();
                dataRow["CostCenter"] = dr["CostCenter"].ToString();
                dataRow["GLAccount"] = dr["GLAccount"].ToString();
                dataRow["AssetNo"] = dr["AssetNo"].AsString();
                dataRow["BusinessArea"] = dr["BusinessArea"].AsString();
            }
            return dt;
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

        #endregion

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
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string et = "";
            if (fields["RequestType"].AsString().ToLower() == "opex")
            {
                et = "opex";
                this.lblGLAccount.Text = "GL Account";
                this.lblExpenseType.Text = "Expense Type";
                this.hfStatus.Value = "opex";
            }
            else
            {
                et = "capex";
                this.lblGLAccount.Text = "Asset Class";
                this.lblExpenseType.Text = "Asset Type";
                this.hfStatus.Value = "capex";
            }
            DataTable table = WorkFlowUtil.GetCollectionByList("Payment Request Expense Type").GetDataTable()
                                             .AsEnumerable()
                                             .Where(dr => dr.Field<string>("OpexCapexType").ToString().ToLower() == et)
                                             .CopyToDataTable();
            DataTable expenseTypeAndGLAccount = PaymentRequestSAPCommon.GetExpenseTypeAndGLAccount(requestId, table);
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in expenseTypeAndGLAccount.Rows)
            {
                ht.Add(dr["OriginalExpenseType"].ToString(), dr["ExpenseType"].ToString());
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
                    var lblExpenseTypeName = (Label)item.FindControl("lblExpenseTypeName");
                    lblExpenseTypeName.Text = OriginalExpenseType[row["ExpenseType"].ToString()].AsString();
                }
            }
        }
    }
}