using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint.Utilities.Common;
using System.Data;
using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using System.Collections;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.PaymentRequest
{
    public partial class Installment : System.Web.UI.UserControl
    {
        #region update by YG

        public Repeater InstallmentRepeaterControl
        {
            get { return ReapterInstallment; }
        }

        public bool IsFromPO
        {
            set;
            get;
        }
        private string requestId;
        public string RequestId
        {
            get
            {
                return requestId;
            }
            set
            {
                requestId = value;
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        public string PONO { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrency();
                if (string.IsNullOrEmpty(PONO))
                {
                    BindReapeaterData(2);
                }
                else
                {
                    BindReapeaterData();
                }
                BindCostCenterAndExpenseType();
                if (requestId != null && requestId != "" && IsFromPO == false)
                {
                    DataTable itemDetails = PaymentRequestComm.GetDataTable(requestId);
                    double totalAmount = 0;
                    foreach (DataRow dRow in itemDetails.Rows)
                    {
                        if (dRow["ItemAmount"] != null)
                        {
                            string itemAmount = dRow["ItemAmount"].ToString();
                            totalAmount += (itemAmount != string.Empty) ? double.Parse(itemAmount) : 0;
                        }
                    }
                    this.txtTotalAmount2.Text = totalAmount.ToString();
                    this.rptItem.DataSource = itemDetails;
                    this.rptItem.DataBind();
                }
                else
                {
                    DataBindDataTable();
                }
                GetGLAccountDataTable();
            }
            //GetGLAccountDataTable();
        }

        /// <summary>
        /// 绑定分期付款的数据行
        /// </summary>
        /// <param name="iCount"></param>
        void BindReapeaterData()
        {
            DataTable dt = PaymentRequestComm.GetPaymentInstallmentInfo(PONO).GetDataTable();
            if (dt != null && dt.Rows.Count >= 2)
            {
                dt.Columns.Add("PaymentAmount");
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TotalAmount"].ToString() != "")
                    {
                        decimal amount = decimal.Parse(dt.Rows[i]["TotalAmount"].ToString());
                        if (CopyStatus.Equals("Copy", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (dt.Rows[i]["PaidThisTimeAmount"].AsString() == "")
                            {
                                dt.Rows[i]["PaymentAmount"] = Math.Round(amount * decimal.Parse(dt.Rows[i]["Paid"].ToString()) / 100, 2);
                            }
                            else
                            {
                                dt.Rows[i]["PaymentAmount"] = Math.Round(decimal.Parse(dt.Rows[i]["PaidThisTimeAmount"].ToString()), 2);
                            }
                        }
                        else
                        {
                            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == true)
                            {
                                dt.Rows[i]["PaymentAmount"] = Math.Round(amount * decimal.Parse(dt.Rows[i]["Paid"].ToString()) / 100, 2);
                            }
                            else
                            {
                                dt.Rows[i]["PaymentAmount"] = Math.Round(decimal.Parse(dt.Rows[i]["PaidThisTimeAmount"].ToString()), 2);
                            }
                        }
                        ++count;
                    }
                }
                this.DDLPaymentCount.SelectedIndex = count - 2;
                txtTotalAmount2.Text = dt.Rows[0]["TotalAmount"].ToString();
                ReapterInstallment.DataSource = dt;
                ReapterInstallment.DataBind();
            }
            else{
               BindReapeaterData(2); 
            }
        }

        /// <summary>
        /// 绑定分期付款的数据行
        /// </summary>
        /// <param name="iCount"></param>
        void BindReapeaterData(int iCount)
        {
            DataTable dt = CreateInstallData();
            for (int i = 0; i < iCount; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            ReapterInstallment.DataSource = dt;
            ReapterInstallment.DataBind();
        }

        /// <summary>
        /// 创建分期付款的数据表的列 
        /// </summary>
        /// <returns></returns>
        DataTable CreateInstallData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONo");
            dt.Columns.Add("Index"); 
            dt.Columns.Add("Paid");
            dt.Columns.Add("IsNeedGR");
            dt.Columns.Add("Comments");
            dt.Columns.Add("TotalAmount");
            dt.Columns.Add("PaymentAmount");
            return dt;
        }

        protected void DDLPaymentCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCount = int.Parse(DDLPaymentCount.SelectedValue);
            BindReapeaterData(iCount);
            ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "UpdatePanel2", "BindEvent()", true);
        }

        #endregion

        #region cost center related method -- update by LJ

        public string TaxPrice 
        {
            get 
            {
                return this.hfTaxPrice.Value;
            }
        }

        internal DataTable ItemTable
        {
            get
            {
                return (this.ViewState["ItemTable1"] as DataTable) ?? CreateItemTable();
            }
            set
            {
                this.ViewState["ItemTable1"] = value;
            }
        }

        internal DataTable ExpenseTypes
        {
            get
            {
                return this.ViewState["ExpenseTypes1"] as DataTable;
            }
            set
            {
                this.ViewState["ExpenseTypes1"] = value;
            }
        }

        internal DataTable CostCenters
        {
            get
            {
                return this.ViewState["CostCenters1"] as DataTable;
            }
            set
            {
                this.ViewState["CostCenters1"] = value;
            }
        }

        public string SummaryAmount
        {
            set { this.hfSummaryAmount.Value = value; }
            get { return this.hfSummaryAmount.Value.Trim(); }
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

        private void BindCostCenterAndExpenseType()
        {
            //Payment Request Expense Types
            ExpenseTypes = PaymentRequestComm.GetPRExpenseTypeDataTable(Installment_Opex_Capex_Status);
            CostCenters =WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
            OriginalExpenseType = this.GetOriginalExpenseType();
        }

        private Hashtable GetOriginalExpenseType()
        {
            DataTable dt = ExpenseTypes;
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["OriginalExpenseType"].ToString(), dr["ExpenseType"].ToString());
            }
            return ht;
        }

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("ItemAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");
            ItemTable.Columns.Add("FANO");
            ItemTable.Rows.Add();
            return ItemTable;
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
                if (this.hfTax.Value == "1")
                {
                    //double tax = double.Parse(hfTaxPrice.Value.Trim()) * double.Parse(hfNoTaxTotalAmount.Value.Trim());
                    row["ExpenseType"] = "Tax payable - VAT input";
                    row["CostCenter"] = "";
                    //row["ItemAmount"] = tax;
                    row["ItemAmount"] = "";
                    row["GLAccount"] = this.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
                    row["FANO"] = "";
                }
                else
                {
                    row["ExpenseType"] = ddlExpenseType.SelectedValue;
                    row["CostCenter"] = ddlCostCenter.SelectedValue;
                    row["ItemAmount"] = txtAmount.Text;
                    row["GLAccount"] = lblGLAccount.Text;
                    row["FANO"] = txtFANO.Text;
                }
              
            }
        }

        protected void rptItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateItem();
                if (e.Item.ItemIndex > 0)
                {
                    ItemTable.Rows.Remove(ItemTable.Rows[e.Item.ItemIndex]);
                }
                this.rptItem.DataSource = ItemTable;
                this.rptItem.DataBind();
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
                    var txtAmount = (TextBox)item.FindControl("txtAmount");
                    var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                    var txtFANO = (TextBox)item.FindControl("txtFANO");
                    DataBindExpenseType(ddlExpenseType, ExpenseTypes);
                    DataBindCostCenter(ddlCostCenter, CostCenters);
                    ddlExpenseType.SelectedValue = OriginalExpenseType[row["ExpenseType"].ToString()].AsString();
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["ItemAmount"].ToString();
                    if (row["ExpenseType"].ToString() != "")
                    {
                        lblGLAccount.Text = this.GetGLAccountByExpenseType(OriginalExpenseType[row["ExpenseType"].ToString()].AsString());
                    }
                    else
                    {
                        lblGLAccount.Text = "";
                    }
                    txtFANO.Text = row["FANO"].AsString();
                }
            }
        }

        private void GetGLAccountDataTable()
        {
            System.Text.StringBuilder strGLAccount = new System.Text.StringBuilder();
            strGLAccount.Append("[");
            DataTable dt = PaymentRequestComm.GetPRExpenseTypeDataTable(Installment_Opex_Capex_Status);
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
            this.hfGLAccount1.Value = strGLAccount.ToString();

            switch (Installment_Opex_Capex_Status)
            {
                case OpexCapexStatus.Opex:
                    this.lblTDStatus1.Text = "GL Account";
                    this.lblExpenseType1.Text = "Expense Type";
                    this.FAStatus2.Value = "0";
                    break;
                case OpexCapexStatus.Capex_AssetNo:
                    this.lblTDStatus1.Text = "Asset Class";
                    this.lblExpenseType1.Text = "Asset Type";
                    this.FAStatus2.Value = "1";
                    break;
                case OpexCapexStatus.Capex_NoAssetNo:
                    this.lblTDStatus1.Text = "Asset Class";
                    this.lblExpenseType1.Text = "Asset Type";
                    this.FAStatus2.Value = "0";
                    break;
            }
        }

        private string GetGLAccountByExpenseType(string key)
        {
            Hashtable ht = GetExpenseTypeAndGLAccountHashTable();
            string gLAccount = "";
            if (key != "")
            {
                gLAccount = ht[key] != null ? ht[key].ToString() : "";
            }
            return gLAccount;
        }

        private Hashtable GetExpenseTypeAndGLAccountHashTable()
        {
            Hashtable ht = new Hashtable();
            DataTable dt = PaymentRequestComm.GetPRExpenseTypeDataTable(Installment_Opex_Capex_Status);
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
            }
            return ht;
        }

        private void UpdateItem()
        {
            this.ItemTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                var txtFANO = (TextBox)item.FindControl("txtFANO");
                DataRow row = this.ItemTable.Rows.Add();
                row["ExpenseType"] = GetNewExpenseTypeByOriginalExpenseType(ddlExpenseType.SelectedValue);
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["ItemAmount"] = txtAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;
                row["FANO"] = txtFANO.Text;
                //row["ItemAmount"] = Math.Round(double.Parse(txtAmount.Text) * 
                //    double.Parse(((TextBox)ReapterInstallment.Items[0].FindControl("txtPaid")).Text) / 100, 2);
            }
        }

        private string GetNewExpenseTypeByOriginalExpenseType(string et)
        {
            DataTable dt = ExpenseTypes; //if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
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

        private void DataBindExpenseType(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["ExpenseType"].ToString(), dr["ExpenseType"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void DataBindCostCenter(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["Display"].ToString(), dr["Title"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void DataBindDataTable()
        {
            DataTable data = this.ItemTable;
            this.rptItem.DataSource = data;
            this.rptItem.DataBind();
        }

        public void Update()
        {
            UpdateItem();
        }

        #endregion

        #region Opex Capex

        public string HF_I_RequestType
        {
            get { return this.hf_I_RequestType.Value.Trim(); }
            set { this.hf_I_RequestType.Value = value; }
        }

        public string Installment_Opex_Capex_Status
        {
            get
            {
                return this.ViewState["Installment_Opex_Capex_Status"].ToString();
            }
            set
            {
                this.ViewState["Installment_Opex_Capex_Status"] = value;
            }
        }

        #endregion

        private void LoadCurrency()
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
            if (WorkflowContext.Current.DataFields.Count == 0)
            {
                this.ddlCurrency.SelectedValue = "RMB";
                this.txtExchangeRate.Text = "1";
            }
            else
            {
                this.ddlCurrency.SelectedValue = WorkflowContext.Current.DataFields["Currency"].AsString();
                this.txtExchangeRate.Text = WorkflowContext.Current.DataFields["ExchRate"].AsString();
            }
        }

        public string SummaryExpenseType
        {
            get { return this.hidSummaryExpenseType.Value.Trim(); }
            
        }

        #region Copy Module
        private string copyStatus = "0";
        public string CopyStatus
        {
            get { return copyStatus; }
            set { copyStatus = value; }
        }
        private string workFlowNumber = "";
        public string WorkFlowNumber
        {
            get { return workFlowNumber; }
            set { workFlowNumber = value; }
        }

        #endregion

    }
}