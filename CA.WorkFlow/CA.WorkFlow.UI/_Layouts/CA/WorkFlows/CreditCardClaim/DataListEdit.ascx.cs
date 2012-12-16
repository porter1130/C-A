using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint.WebControls;
using System.Collections;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class DataListEdit : BaseWorkflowUserControl
    {
        #region Fields
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
        internal DataTable USDItemTable
        {
            get
            {
                return (this.ViewState["USDItemTable"] as DataTable) ?? CreateUSDItemTable();
            }
            set
            {
                this.ViewState["USDItemTable"] = value;
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
        private string requestId;
        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }
        public string ExpenseDescription
        {
            get { return this.txtExpenseDescription.Text; }
        }
        #endregion

        #region Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                txtExpenseDescription.Text = fields["ExpenseDescription"].ToString();
                lbTotalAmount.Text = fields["TotalAmount"].ToString();
                lblWorkFlowNumber.Text = fields["CCCWWorkflowNumber"].ToString();
                string name = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                lblRequestedID.Text = employee.EmployeeID;
                lblRequestedBy.Text = employee.DisplayName;

                //lblEmployeeID1.Text = employee.EmployeeID;
                //lblEmployeeID2.Text = employee.EmployeeID;

                CostCenters = CreditCardClaimCommon.GetCostCenterDT();

                GetGLAccountDataTable();

                DataTable itemDetails = CreditCardClaimCommon.GetDataTableToSAP(requestId);
                DataBindRMBAndUSDItem(itemDetails);
            }
        }

        private void DataBindRMBAndUSDItem(DataTable itemDetails)
        {
            bool rmbStatus = false;
            bool usdStatus = false;

            double rmbTotalAmount = 0;
            double usdTotalAmount = 0;

            DataTable dtRMB = new DataTable();
            dtRMB.Columns.Add("ExpenseType");
            dtRMB.Columns.Add("DealAmount");
            dtRMB.Columns.Add("DepositAmount");
            dtRMB.Columns.Add("PayAmount");
            dtRMB.Columns.Add("CostCenter");
            dtRMB.Columns.Add("AmountType");
            dtRMB.Columns.Add("GLAccount");
            dtRMB.Columns.Add("TransactionDescription");
            dtRMB.Columns.Add("CreditCardBillID");

            DataTable dtUSD = new DataTable();
            dtUSD.Columns.Add("ExpenseType");
            dtUSD.Columns.Add("DealAmount");
            dtUSD.Columns.Add("DepositAmount");
            dtUSD.Columns.Add("PayAmount");
            dtUSD.Columns.Add("CostCenter");
            dtUSD.Columns.Add("AmountType");
            dtUSD.Columns.Add("GLAccount");
            dtUSD.Columns.Add("TransactionDescription");
            dtUSD.Columns.Add("CreditCardBillID");

            foreach (DataRow dr in itemDetails.Rows)
            {
                if (dr["AmountType"].ToString() == "RMB" && dr["ExpenseType"].ToString() != "OR - employee vendor")
                {
                    DataRow drRMB = dtRMB.Rows.Add();
                    drRMB["ExpenseType"] = dr["ExpenseType"].ToString();
                    drRMB["CostCenter"] = dr["CostCenter"].ToString();
                    drRMB["DealAmount"] = dr["DealAmount"].ToString();
                    drRMB["DepositAmount"] = dr["DepositAmount"].ToString();
                    drRMB["PayAmount"] = dr["PayAmount"].ToString();
                    drRMB["AmountType"] = "RMB";
                    drRMB["GLAccount"] = dr["GLAccount"].ToString();
                    drRMB["TransactionDescription"] = dr["TransactionDescription"].ToString();
                    drRMB["CreditCardBillID"] = dr["CreditCardBillID"].ToString();
                    //rmbTotalAmount += float.Parse(drRMB["PayAmount"].ToString()) - float.Parse(drRMB["DepositAmount"].ToString());
                    rmbTotalAmount += double.Parse(drRMB["DealAmount"].ToString());
                    rmbStatus = true;
                }

                if (dr["AmountType"].ToString() == "USD" && dr["ExpenseType"].ToString() != "OR - employee vendor")
                {
                    DataRow drUSD = dtUSD.Rows.Add();
                    drUSD["ExpenseType"] = dr["ExpenseType"].ToString();
                    drUSD["CostCenter"] = dr["CostCenter"].ToString();
                    drUSD["DealAmount"] = dr["DealAmount"].ToString() ;
                    drUSD["DepositAmount"] = dr["DepositAmount"].ToString();
                    drUSD["PayAmount"] = dr["PayAmount"].ToString();
                    drUSD["TransactionDescription"] = dr["TransactionDescription"].ToString();
                    drUSD["AmountType"] = "USD";
                    drUSD["GLAccount"] = dr["GLAccount"].ToString();
                    drUSD["CreditCardBillID"] = dr["CreditCardBillID"].ToString();

                    //usdTotalAmount += float.Parse(drUSD["PayAmount"].ToString()) - float.Parse(drUSD["DepositAmount"].ToString());
                    usdTotalAmount += double.Parse(drUSD["DealAmount"].ToString());
                    usdStatus = true;
                }
            }

            if (rmbStatus)
            {
                if (dtRMB == null)
                {
                    this.ItemTable.Rows.Clear();
                }
                this.rptItem.DataSource = dtRMB == null ? ItemTable : dtRMB;
                this.rptItem.DataBind();
                //if (rmbTotalAmount < 0)
                //{
                //    this.txtEmployeeVendor1.Text = rmbTotalAmount.ToString(); 
                //}
                //if (rmbTotalAmount == 0)
                //{
                //    this.txtEmployeeVendor1.Text = "0";
                //}
                //if (rmbTotalAmount >0) 
                //{
                //    this.txtEmployeeVendor1.Text = "-" + rmbTotalAmount.ToString();
                //}
                rmbTotalAmount = Math.Round(rmbTotalAmount, 2);
                this.lblRMBTotalAmount.Text = rmbTotalAmount.ToString();
            }
            else
            {
                hfTableStatus.Value = "RMB";
            }

            if (usdStatus)
            {
                if (dtUSD == null)
                {
                    this.USDItemTable.Rows.Clear();
                }
                this.rptUSDItem.DataSource = dtUSD == null ? USDItemTable : dtUSD;
                this.rptUSDItem.DataBind();
                //if (usdTotalAmount < 0)
                //{
                //    this.txtEmployeeVendor2.Text = usdTotalAmount.ToString();
                //}
                //if (usdTotalAmount == 0)
                //{
                //    this.txtEmployeeVendor2.Text = "0";
                //}
                //if (usdTotalAmount > 0)
                //{
                //    this.txtEmployeeVendor2.Text = "-" + usdTotalAmount.ToString();
                //}
                usdTotalAmount = Math.Round(usdTotalAmount, 2);
                this.lblUSDTotalAmount.Text = usdTotalAmount.ToString();
            }
            else
            {
                hfTableStatus.Value = "USD";
            }
        }

        private DataTable GetCRTable()
        {
            DataTable CRTable = new DataTable();
            CRTable.Columns.Add("ExpenseType");
            CRTable.Columns.Add("DealAmount");
            CRTable.Columns.Add("DepositAmount");
            CRTable.Columns.Add("PayAmount");
            CRTable.Columns.Add("CostCenter");
            CRTable.Columns.Add("GLAccount");
            CRTable.Columns.Add("AmountType");
            CRTable.Columns.Add("TransactionDescription");
            CRTable.Columns.Add("EmployeeID");
            CRTable.Columns.Add("EmployeeName");
            CRTable.Columns.Add("CCCWWorkflowNumber");
            CRTable.Columns.Add("CreditCardBillID");
            if (hfRMBEmployeeVendor.Value!="") 
            {
                List<string> list = hfRMBEmployeeVendor.Value.Trim().Split(';').ToList<string>();
                list.Remove("");
                foreach(string str in list)
                {
                    DataRow row = CRTable.Rows.Add();
                    row["ExpenseType"] = "OR - employee vendor";
                    row["CostCenter"] = "";
                    row["DealAmount"] = str;
                    row["DepositAmount"] = "0";
                    row["PayAmount"] = "0";
                    row["GLAccount"] = lblRequestedID.Text;
                    row["EmployeeID"] = lblRequestedID.Text.Trim();
                    row["EmployeeName"] = lblRequestedBy.Text.Trim();
                    row["AmountType"] = "RMB";
                    row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                    row["CreditCardBillID"] = "";
                    row["TransactionDescription"] = "";
                }
            }
            if (hfUSDEmployeeVendor.Value != "")
            {
                List<string> list = hfUSDEmployeeVendor.Value.Trim().Split(';').ToList<string>();
                list.Remove("");
                foreach (string str in list)
                {
                    DataRow row = CRTable.Rows.Add();
                    row["ExpenseType"] = "OR - employee vendor";
                    row["CostCenter"] = "";
                    row["DealAmount"] = str;
                    row["DepositAmount"] = "0";
                    row["PayAmount"] = "0";
                    row["GLAccount"] = lblRequestedID.Text;
                    row["EmployeeID"] = lblRequestedID.Text.Trim();
                    row["EmployeeName"] = lblRequestedBy.Text.Trim();
                    row["AmountType"] = "USD";
                    row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                    row["CreditCardBillID"] = "";
                    row["TransactionDescription"] = "";
                }
            }
            //if (txtEmployeeVendor1.Text != "0")
            //{
            //    DataRow row = CRTable.Rows.Add();
            //    row["ExpenseType"] = "OR - employee vendor";
            //    row["CostCenter"] = "";
            //    row["DealAmount"] = txtEmployeeVendor1.Text;
            //    row["DepositAmount"] = "0";
            //    row["PayAmount"] = "0";
            //    row["GLAccount"] = lblEmployeeID1.Text;
            //    row["EmployeeID"] = lblRequestedID.Text.Trim();
            //    row["EmployeeName"] = lblRequestedBy.Text.Trim();
            //    row["AmountType"] = "RMB1";
            //    row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
            //}
            //if (txtEmployeeVendor2.Text != "0")
            //{
            //    DataRow row = CRTable.Rows.Add();
            //    row["ExpenseType"] = "OR - employee vendor";
            //    row["CostCenter"] = "";
            //    row["DealAmount"] = txtEmployeeVendor2.Text;
            //    row["DepositAmount"] = "0";
            //    row["PayAmount"] = "0";
            //    row["GLAccount"] = lblEmployeeID2.Text;
            //    row["EmployeeID"] = lblRequestedID.Text.Trim();
            //    row["EmployeeName"] = lblRequestedBy.Text.Trim();
            //    row["AmountType"] = "USD2";
            //    row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
            //}
            return CRTable;
        }

        #endregion

        #region RMB Method

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("DealAmount");
            ItemTable.Columns.Add("DepositAmount");
            ItemTable.Columns.Add("PayAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");
            ItemTable.Columns.Add("AmountType");

            ItemTable.Columns.Add("TransactionDescription");

            ItemTable.Columns.Add("EmployeeID");
            ItemTable.Columns.Add("EmployeeName");
            ItemTable.Columns.Add("CCCWWorkflowNumber");

            ItemTable.Columns.Add("CreditCardBillID");
            return ItemTable;
        }

        protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            UpdateItem();
            DataRow row = ItemTable.Rows.Add();
            AddRMBRow(row);

            this.rptItem.DataSource = this.ItemTable;
            this.rptItem.DataBind();
        }

        private void AddRMBRow(DataRow row)
        {
            RepeaterItem item = this.rptItem.Items[this.rptItem.Items.Count - 1];
            var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
            var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
            var txtAmount = (TextBox)item.FindControl("txtAmount");
            //var txtDepositAmount = (TextBox)item.FindControl("txtDepositAmount");
            //var txtPayAmount = (TextBox)item.FindControl("txtPayAmount");
            var lblTransactionDescription = (Label)item.FindControl("lblTransactionDescription");

            var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
            var hfRMBCreditCardBillID = (HiddenField)item.FindControl("hfRMBCreditCardBillID");
            
            row["ExpenseType"] = ddlExpenseType.SelectedValue;
            row["CostCenter"] = ddlCostCenter.SelectedValue;
            row["DealAmount"] = txtAmount.Text;
            //row["DepositAmount"] = txtDepositAmount.Text;
            //row["PayAmount"] = txtPayAmount.Text;
            row["GLAccount"] = lblGLAccount.Text;
            //row["GLAccount"] = CreditCardClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
            row["AmountType"] = "RMB";
            row["EmployeeID"] = lblRequestedID.Text.Trim();
            row["EmployeeName"] = lblRequestedBy.Text.Trim();
            row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
            row["CreditCardBillID"] = hfRMBCreditCardBillID.Value;
            row["TransactionDescription"] = lblTransactionDescription.Text;
        }

        private void UpdateItem()
        {
            this.ItemTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                //var txtDepositAmount = (TextBox)item.FindControl("txtDepositAmount");
                //var txtPayAmount = (TextBox)item.FindControl("txtPayAmount");
                var lblTransactionDescription = (Label)item.FindControl("lblTransactionDescription");

                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                var hfRMBCreditCardBillID = (HiddenField)item.FindControl("hfRMBCreditCardBillID");
                DataRow row = this.ItemTable.Rows.Add();
                row["ExpenseType"] = ddlExpenseType.SelectedValue;
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["DealAmount"] = txtAmount.Text;
                //row["DepositAmount"] = txtDepositAmount.Text;
                //row["PayAmount"] = txtPayAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;
                //row["GLAccount"] = CreditCardClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
                row["AmountType"] = "RMB";
                row["EmployeeID"] = lblRequestedID.Text.Trim();
                row["EmployeeName"] = lblRequestedBy.Text.Trim();
                row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                row["CreditCardBillID"] = hfRMBCreditCardBillID.Value;
                row["TransactionDescription"] = lblTransactionDescription.Text;
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
                    //var txtDepositAmount = (TextBox)item.FindControl("txtDepositAmount");
                    //var txtPayAmount = (TextBox)item.FindControl("txtPayAmount");

                    var lblTransactionDescription = (Label)item.FindControl("lblTransactionDescription");
                    var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");

                    var hfRMBCreditCardBillID = (HiddenField)item.FindControl("hfRMBCreditCardBillID");

                    DataBindDDL(ddlCostCenter, CostCenters);
                    ddlExpenseType.SelectedValue = row["ExpenseType"].ToString();
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["DealAmount"].ToString();
                    //txtDepositAmount.Text = row["DepositAmount"].ToString();
                    //txtPayAmount.Text = row["PayAmount"].ToString();
                    lblTransactionDescription.Text = row["TransactionDescription"].ToString();

                    hfRMBCreditCardBillID.Value = row["CreditCardBillID"].ToString();

                    if (row["ExpenseType"].ToString() != "0")
                    {
                        if (row["GLAccount"].ToString() != "")
                        {
                            lblGLAccount.Text = row["GLAccount"].ToString();
                        }
                        else
                        {
                            lblGLAccount.Text = CreditCardClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
                        }
                    }
                    else
                    {
                        if (row["GLAccount"].ToString() != "")
                        {
                            lblGLAccount.Text = row["GLAccount"].ToString();
                        }
                        else
                        {
                            lblGLAccount.Text = "";
                        }
                    }
                }
            }
        }

        #endregion

        #region USD Method

        private DataTable CreateUSDItemTable()
        {
            USDItemTable = new DataTable();
            USDItemTable.Columns.Add("ExpenseType");
            USDItemTable.Columns.Add("DealAmount");
            USDItemTable.Columns.Add("DepositAmount");
            USDItemTable.Columns.Add("PayAmount");
            USDItemTable.Columns.Add("CostCenter");
            USDItemTable.Columns.Add("GLAccount");
            USDItemTable.Columns.Add("AmountType");

            USDItemTable.Columns.Add("TransactionDescription");

            USDItemTable.Columns.Add("EmployeeID");
            USDItemTable.Columns.Add("EmployeeName");
            USDItemTable.Columns.Add("CCCWWorkflowNumber");

            USDItemTable.Columns.Add("CreditCardBillID");

            USDItemTable.Rows.Add();
            return USDItemTable;
        }

        protected void btnAddUSDItem_Click(object sender, ImageClickEventArgs e)
        {
            UpdateUSDItem();
            DataRow row = USDItemTable.Rows.Add();
            AddUSDRow(row);
            this.rptUSDItem.DataSource = this.USDItemTable;
            this.rptUSDItem.DataBind();
        }

        private void AddUSDRow(DataRow row) 
        {
            RepeaterItem item = this.rptUSDItem.Items[this.rptUSDItem.Items.Count - 1];
            var ddlExpenseType = (DropDownList)item.FindControl("ddlUSDExpenseType");
            var ddlCostCenter = (DropDownList)item.FindControl("ddlUSDCostCenter");
            var txtAmount = (TextBox)item.FindControl("txtUSDAmount");
            //var txtDepositAmount = (TextBox)item.FindControl("txtUSDDepositAmount");
            //var txtPayAmount = (TextBox)item.FindControl("txtUSDPayAmount");
            var lblUSDTransactionDescription = (Label)item.FindControl("lblUSDTransactionDescription");

            var lblGLAccount = (TextBox)item.FindControl("lblUSDGLAccount");
            var hfUSDCreditCardBillID = (HiddenField)item.FindControl("hfUSDCreditCardBillID");
            
            row["ExpenseType"] = ddlExpenseType.SelectedValue;
            row["CostCenter"] = ddlCostCenter.SelectedValue;
            row["DealAmount"] = txtAmount.Text;
            //row["DepositAmount"] = txtDepositAmount.Text;
            //row["PayAmount"] = txtPayAmount.Text;
            row["GLAccount"] = lblGLAccount.Text;
            // row["GLAccount"] = CreditCardClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
            row["AmountType"] = "USD";
            row["EmployeeID"] = lblRequestedID.Text.Trim();
            row["EmployeeName"] = lblRequestedBy.Text.Trim();
            row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
            row["CreditCardBillID"] = hfUSDCreditCardBillID.Value;
            row["TransactionDescription"] = lblUSDTransactionDescription.Text;
        }

        private void UpdateUSDItem()
        {
            this.USDItemTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptUSDItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlUSDExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlUSDCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtUSDAmount");
                //var txtDepositAmount = (TextBox)item.FindControl("txtUSDDepositAmount");
                //var txtPayAmount = (TextBox)item.FindControl("txtUSDPayAmount");
                var lblUSDTransactionDescription = (Label)item.FindControl("lblUSDTransactionDescription");

                var lblGLAccount = (TextBox)item.FindControl("lblUSDGLAccount");
                var hfUSDCreditCardBillID = (HiddenField)item.FindControl("hfUSDCreditCardBillID");
                DataRow row = this.USDItemTable.Rows.Add();
                row["ExpenseType"] = ddlExpenseType.SelectedValue;
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["DealAmount"] = txtAmount.Text;
                //row["DepositAmount"] = txtDepositAmount.Text;
                //row["PayAmount"] = txtPayAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;
               // row["GLAccount"] = CreditCardClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
                row["AmountType"] = "USD";
                row["EmployeeID"] = lblRequestedID.Text.Trim();
                row["EmployeeName"] = lblRequestedBy.Text.Trim();
                row["CCCWWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                row["CreditCardBillID"] = hfUSDCreditCardBillID.Value;
                row["TransactionDescription"] = lblUSDTransactionDescription.Text;

            }
        }

        protected void rptUSDItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateUSDItem();
                USDItemTable.Rows.Remove(USDItemTable.Rows[e.Item.ItemIndex]);
                this.rptUSDItem.DataSource = USDItemTable;
                this.rptUSDItem.DataBind();
            }
        }

        protected void rptUSDItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var ddlExpenseType = (DropDownList)item.FindControl("ddlUSDExpenseType");
                    var ddlCostCenter = (DropDownList)item.FindControl("ddlUSDCostCenter");
                    var txtAmount = (TextBox)item.FindControl("txtUSDAmount");
                    //var txtDepositAmount = (TextBox)item.FindControl("txtUSDDepositAmount");
                    //var txtPayAmount = (TextBox)item.FindControl("txtUSDPayAmount");
                    var lblUSDTransactionDescription = (Label)item.FindControl("lblUSDTransactionDescription");
                    var lblGLAccount = (TextBox)item.FindControl("lblUSDGLAccount");

                    var hfUSDCreditCardBillID = (HiddenField)item.FindControl("hfUSDCreditCardBillID");

                    DataBindDDL(ddlCostCenter, CostCenters);
                    ddlExpenseType.SelectedValue = row["ExpenseType"].ToString();
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["DealAmount"].ToString();
                    //txtDepositAmount.Text = row["DepositAmount"].ToString();
                    //txtPayAmount.Text = row["PayAmount"].ToString();
                    lblUSDTransactionDescription.Text = row["TransactionDescription"].ToString();

                    hfUSDCreditCardBillID.Value = row["CreditCardBillID"].ToString();

                    if (row["ExpenseType"].ToString() != "0")
                    {
                        if (row["GLAccount"].ToString() != "")
                        {
                            lblGLAccount.Text = row["GLAccount"].ToString();
                        }
                        else
                        {
                            lblGLAccount.Text = CreditCardClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
                        }
                    }
                    else
                    {
                        if (row["GLAccount"].ToString() != "")
                        {
                            lblGLAccount.Text = row["GLAccount"].ToString();
                        }
                        else
                        {
                            lblGLAccount.Text = "";
                        }
                    }
                }
            }
        }

        #endregion

        #region Common Method

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

        private void GetGLAccountDataTable()
        {
            System.Text.StringBuilder strGLAccount = new System.Text.StringBuilder();
            strGLAccount.Append("[");
            DataTable dt = WorkFlowUtil.GetCollectionByList("Expense Claim SAP GLAccount").GetDataTable();
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
        }

        public void Update()
        {
            UpdateItem();
            UpdateUSDItem();
        }

        #endregion

    }
}