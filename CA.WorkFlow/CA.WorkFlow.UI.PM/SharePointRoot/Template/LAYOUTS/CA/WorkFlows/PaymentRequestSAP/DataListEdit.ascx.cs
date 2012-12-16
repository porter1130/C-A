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
    public partial class DataListEdit : BaseWorkflowUserControl
    {
        #region Fields
        public string Currency
        {
            get
            {
                return this.ddlCurrency.SelectedValue;
            }
        }

        public string ExchangeRate
        {
            get
            {
                return this.txtExchangeRate.Text.Trim() == "" ? "1" : this.txtExchangeRate.Text.Trim();
            }
        }

        public string ExpenseDescription
        {
            get { return this.txtExpenseDescription.Text; }
        }
        internal DataTable CRTable
        {
            get
            {
                return GetCRTable();
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

        internal DataTable CostCenters
        {
            get
            {
                return this.ViewState["CostCenters"] as DataTable;
            }
            set
            {
                this.ViewState["CostCenters"] = value;
            }
        }

        internal DataTable ExpenseType
        {
            get
            {
                return this.ViewState["ExpenseType"] as DataTable;
            }
            set
            {
                this.ViewState["ExpenseType"] = value;
            }
        }

        internal DataTable BusinessArea
        {
            get
            {
                return this.ViewState["BusinessArea"] as DataTable;
            }
            set
            {
                this.ViewState["BusinessArea"] = value;
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

        private string requestId;

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }
        internal DataTable GLAccount
        {
            get
            {
                return this.ViewState["GLAccount"] as DataTable;
            }
            set
            {
                this.ViewState["GLAccount"] = value;
            }
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
        #endregion

        #region Load Method

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
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
                CostCenters = PaymentRequestSAPCommon.GetCostCenterDT();
                DataTable table = WorkFlowUtil.GetCollectionByList("Payment Request Expense Type").GetDataTable()
                                                 .AsEnumerable()
                                                 .Where(dr => dr.Field<string>("OpexCapexType").ToString().ToLower() == et)
                                                 .CopyToDataTable();
                DataTable expenseTypeAndGLAccount = PaymentRequestSAPCommon.GetExpenseTypeAndGLAccount(requestId, table);
                ExpenseType = expenseTypeAndGLAccount;
                OriginalExpenseType = this.GetOriginalExpenseType();
                GLAccount = expenseTypeAndGLAccount;
                BusinessArea = WorkFlowUtil.GetCollectionByList("CostCenterBAMap").GetDataTable();
                ExpenseTypeMapping = WorkFlowUtil.GetCollectionByList("PaymentRequestExpenseTypeMapping").GetDataTable();
                DataTable itemDetails = PaymentRequestSAPCommon.GetDataTableToSAP(requestId);
                if (itemDetails == null)
                {
                    this.ItemTable.Rows.Clear();
                }
                DataTable dt = GetItemDetailsTable(itemDetails);
                this.rptItem.DataSource = dt;
                this.rptItem.DataBind();
                
                //this.lblCRText.Text = "OP-Non-trade vendor";
                //设置CRText
                SetCRText(fields);
                txtExpenseDescription.Text = fields["PaymentDesc"].AsString();
                lbTotalAmount.Text = fields["PreTotalAmount"].AsString();
                //lblCashAdvanceAmount.Text = fields["CashAdvanceAmount"].ToString() == "" ? "0" : fields["CashAdvanceAmount"].ToString();
                //lblPreTotalAmount.Text = fields["PreTotalAmount"].ToString();
                lblWorkFlowNumber.Text = fields["PRWorkflowNumber"].AsString();
                string name = fields["Applicant"].AsString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                lblRequestedID.Text = employee.EmployeeID;
                lblRequestedBy.Text = employee.DisplayName;
                GetGLAccountDataTable();
                this.lblEmployeeID1.Text = fields["VendorNo"].AsString();
                this.txtEmployeeVendor.Text = (0 - double.Parse(fields["PreTotalAmount"].AsString())).ToString();
                this.txtEmployeeVendor.ReadOnly = true;
                //this.hfCashAdvanceWorkFlowNumber.Value = fields["CashAdvanceWorkFlowNumber"] == null ? "" : fields["CashAdvanceWorkFlowNumber"].ToString().Trim();
                LoadCurrency();
            }
        }

        private Hashtable GetOriginalExpenseType()
        {
            DataTable dt = ExpenseType;
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["OriginalExpenseType"].ToString(), dr["ExpenseType"].ToString());
            }
            return ht;
        }

        private void LoadCurrency()
        {
            if (WorkflowContext.Current.DataFields["FromPOStatus"].ToString() == "0")
            {
                DataTable dt = WorkFlowUtil.GetCollectionByList("Payment Request Country Currency").GetDataTable();
                this.ddlCurrency.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem liCurrency = new ListItem()
                    {
                        Text = dr["CurrencyCode"].ToString(),
                        Value = dr["CurrencyCode"].ToString()
                    };
                    this.ddlCurrency.Items.Add(liCurrency);
                }
                this.ddlCurrency.SelectedValue = WorkflowContext.Current.DataFields["Currency"].AsString();
                this.txtExchangeRate.Text = WorkflowContext.Current.DataFields["ExchRate"].AsString();
                this.lblCurrency.Text = WorkflowContext.Current.DataFields["Currency"].AsString();
                this.lblAmountCurrency.Text = WorkflowContext.Current.DataFields["Currency"].AsString();
            }
        }

        private void SetCRText(WorkflowDataFields fields)
        {
            //bool isSystemGR = false;
            //DataTable dt = PaymentRequestSAPCommon.GetEmployeeExpenseClaimSAPItemsByPONumber("Purchase Order Workflow", pONo).GetDataTable();
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    string systemGR = dt.Rows[0]["IsSystemGR"].ToString();
            //    if (systemGR == "1")
            //    {
            //        isSystemGR = true;
            //    }
            //}

            if (fields["FromPOStatus"].ToString() == "1")
            {
                this.lblCRText.Text = "GRIR vendor code";
            }
            else
            {
                this.lblCRText.Text = "OP-Non-trade vendor";
            }
        }

        private DataTable GetItemDetailsTable(DataTable itemDetails)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ExpenseType");
            dt.Columns.Add("ItemAmount");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("GLAccount");
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("PRWorkflowNumber");
            dt.Columns.Add("AssetNo");
            dt.Columns.Add("BusinessArea");
            foreach (DataRow dr in itemDetails.Rows)
            {
                if (dr["ExpenseType"].ToString().IndexOf("OP-Non-trade vendor") < 0
                    && dr["ExpenseType"].ToString().IndexOf("GRIR vendor code") < 0)
                {
                    DataRow dataRow = dt.Rows.Add();
                    dataRow["ExpenseType"] = dr["ExpenseType"].ToString();
                    dataRow["ItemAmount"] = dr["ItemAmount"].ToString();
                    dataRow["CostCenter"] = dr["CostCenter"].ToString();
                    dataRow["GLAccount"] = dr["GLAccount"].ToString();
                    dataRow["EmployeeID"] = dr["EmployeeID"].ToString();
                    dataRow["EmployeeName"] = dr["EmployeeName"].ToString();
                    dataRow["PRWorkflowNumber"] = dr["PRWorkflowNumber"].ToString();
                    dataRow["AssetNo"] = dr["AssetNo"].AsString();
                    dataRow["BusinessArea"] = dr["BusinessArea"].AsString();
                }
            }
            return dt;
        }

        private void DataBindDDL(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["Display"].ToString(), dr["Title"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void DataBindDDLExpense(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["ExpenseType"].ToString(), dr["ExpenseType"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void DataBindDDLBusinessArea(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["BusinessArea"].ToString(), dr["BusinessArea"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void GetGLAccountDataTable()
        {
            System.Text.StringBuilder strGLAccount = new System.Text.StringBuilder();
            strGLAccount.Append("[");
            //DataTable dt = WorkFlowUtil.GetCollectionByList("Payment Request Expense Type").GetDataTable()
            //                                     .AsEnumerable()
            //                                     .Where(dr => dr.Field<string>("OpexCapexType").ToString().ToLower() == "opex")
            //                                     .CopyToDataTable();
            DataTable dt = ExpenseType;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    strGLAccount.Append("{");
                    strGLAccount.AppendFormat("name:'{0}',val:'{1}'", dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
                    strGLAccount.Append("},");
                }
            }
            strGLAccount.Append("]");
            this.hfGLAccount.Value = strGLAccount.ToString();


            System.Text.StringBuilder strCostCenterBAMap = new System.Text.StringBuilder();
            strCostCenterBAMap.Append("[");
            DataTable data = BusinessArea;
            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow dr in data.Rows)
                {
                    strCostCenterBAMap.Append("{");
                    strCostCenterBAMap.AppendFormat("name:'{0}',val:'{1}'", dr["CostCenter"].ToString(), dr["BusinessArea"].ToString());
                    strCostCenterBAMap.Append("},");
                }
            }
            strCostCenterBAMap.Append("]");
            this.hfCostCenterBAMap.Value = strCostCenterBAMap.ToString();
        }

        #endregion

        #region Payment Request SAP Items 

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("ItemAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");
            ItemTable.Columns.Add("EmployeeID");
            ItemTable.Columns.Add("EmployeeName");
            ItemTable.Columns.Add("PRWorkflowNumber");
            ItemTable.Columns.Add("AssetNo");
            ItemTable.Columns.Add("BusinessArea");
            ItemTable.Rows.Add();
            return ItemTable;
        }

        private string GetNewExpenseTypeByOriginalExpenseType(string et)
        {
            DataTable dt = ExpenseType; //if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
            string expenseType = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ExpenseType"].ToString().IndexOf(et) != -1)
                {
                    expenseType = dr["OriginalExpenseType"].ToString();
                    break;
                }
            }
            return expenseType;
        }

        private void UpdateItem()
        {
            this.ItemTable.Rows.Clear();
            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var ddlBusinessArea = (DropDownList)item.FindControl("ddlBusinessArea");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                var txtFANO = (TextBox)item.FindControl("txtFANO");
                DataRow row = this.ItemTable.Rows.Add();
                row["ExpenseType"] = GetNewExpenseTypeByOriginalExpenseType(ddlExpenseType.SelectedValue);
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["ItemAmount"] = txtAmount.Text;

                row["GLAccount"] = lblGLAccount.Text;

                //row["GLAccount"] = PaymentRequestSAPCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
                row["EmployeeID"] = lblRequestedID.Text.Trim();
                row["EmployeeName"] = lblRequestedBy.Text.Trim();
                row["PRWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                row["AssetNo"] = txtFANO.Text.Trim();
                //row["CashAmount"] = lblCashAdvanceAmount.Text.Trim();
                row["BusinessArea"] = ddlBusinessArea.SelectedValue;
            }
        }

        protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            UpdateItem();
            DataRow row = ItemTable.Rows.Add();
            AddRow(row);
            this.rptItem.DataSource = this.ItemTable;
            this.rptItem.DataBind();
        }

        private void AddRow(DataRow row)
        {
            if (this.rptItem.Items.Count > 0)
            {
                RepeaterItem item = this.rptItem.Items[this.rptItem.Items.Count - 1];
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                var txtFANO = (TextBox)item.FindControl("txtFANO");
                var ddlBusinessArea = (DropDownList)item.FindControl("ddlBusinessArea");
                row["ExpenseType"] = ddlExpenseType.SelectedValue;
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["ItemAmount"] = txtAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;
                if (row["ExpenseType"].ToString() == "Tax payable - VAT input")
                {
                    row["CostCenter"] = "";
                }
                else
                {
                    row["CostCenter"] = ddlCostCenter.SelectedValue;
                }
                row["AssetNo"] = txtFANO.Text.Trim();
                row["BusinessArea"] = ddlBusinessArea.SelectedValue;
            }
        }

        protected void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                    var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    var ddlBusinessArea = (DropDownList)item.FindControl("ddlBusinessArea");
                    var txtAmount = (TextBox)item.FindControl("txtAmount");
                    var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                    var txtFANO = (TextBox)item.FindControl("txtFANO");
                    DataBindDDL(ddlCostCenter, CostCenters);
                    DataBindDDLExpense(ddlExpenseType, ExpenseType);
                    DataBindDDLBusinessArea(ddlBusinessArea, BusinessArea);
                    //ddlExpenseType.SelectedValue = row["ExpenseType"].ToString();
                    ddlExpenseType.SelectedValue = PaymentRequestSAPCommon.GetExpenseTypeByExpenseType(OriginalExpenseType[row["ExpenseType"].ToString()].AsString(), ExpenseTypeMapping);
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["ItemAmount"].ToString();
                    if (row["ExpenseType"].ToString() != "0")
                    {
                        lblGLAccount.Text = PaymentRequestSAPCommon.GetGLAccountByExpenseType(ddlExpenseType.SelectedValue, GLAccount);
                    }
                    else
                    {
                        lblGLAccount.Text = "";
                    }
                    txtFANO.Text = row["AssetNo"].AsString();
                    ddlBusinessArea.SelectedValue = row["BusinessArea"].AsString();
                }
            }
        }

        protected void rptItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateItem();
                ItemTable.Rows.Remove(ItemTable.Rows[e.Item.ItemIndex]);
                this.rptItem.DataSource = ItemTable;
                this.rptItem.DataBind();
            }
        }

        #endregion

        #region WorkFlow Page Method

        private DataTable GetCRTable()
        {
            DataTable CRTable = new DataTable();
            CRTable.Columns.Add("ExpenseType");
            CRTable.Columns.Add("ItemAmount");
            CRTable.Columns.Add("CostCenter");
            CRTable.Columns.Add("GLAccount");
            CRTable.Columns.Add("EmployeeID");
            CRTable.Columns.Add("EmployeeName");
            CRTable.Columns.Add("PRWorkflowNumber");
            CRTable.Columns.Add("AssetNo");
            CRTable.Columns.Add("BusinessArea");
            if (txtEmployeeVendor.Text != "0")
            {
                DataRow row = CRTable.Rows.Add();
                row["ExpenseType"] = this.lblCRText.Text;
                row["CostCenter"] = "";
                row["ItemAmount"] = txtEmployeeVendor.Text;
                row["GLAccount"] = lblEmployeeID1.Text;
                row["EmployeeID"] = lblRequestedID.Text.Trim();
                row["EmployeeName"] = lblRequestedBy.Text.Trim();
                row["PRWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                row["AssetNo"] = "";
                row["BusinessArea"] = "";
            }
            //if (hfCashAdvanceWorkFlowNumber.Value != "")
            //{
            //    List<string> strlist = hfCashAdvanceWorkFlowNumber.Value.Split(';').ToList<string>();
            //    foreach (string str in strlist)
            //    {
            //        if (str != "")
            //        {
            //            string canumber = str.Substring(str.IndexOf('-') + 1);
            //            DataRow row1 = CRTable.Rows.Add();
            //            row1["ExpenseType"] = "OR - cash advance";
            //            row1["CostCenter"] = "";
            //            row1["ItemAmount"] = "-" + canumber;
            //            row1["GLAccount"] = lblEmployeeID1.Text;
            //            row1["EmployeeID"] = lblRequestedID.Text.Trim();
            //            row1["EmployeeName"] = lblRequestedBy.Text.Trim();
            //            row1["PRWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
            //            //row1["CashAmount"] = lblCashAdvanceAmount.Text.Trim();
            //        }
            //    }
            //}
            return CRTable;
        }

        public void Update()
        {
            UpdateItem();
        }

        #endregion

    }
}