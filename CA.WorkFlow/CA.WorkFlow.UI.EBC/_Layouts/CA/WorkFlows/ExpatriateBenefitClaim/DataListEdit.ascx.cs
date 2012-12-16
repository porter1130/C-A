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

namespace CA.WorkFlow.UI.EBC
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
        public string RequestId { set { this.requestId = value; } }

        public string ExpenseDescription
        {
            get { return this.txtExpenseDescription.Text; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                CostCenters = ExpatriateBenefitClaimCommon.GetCostCenterDT();

                DataTable itemDetails = ExpatriateBenefitClaimCommon.GetDataTableToSAP(requestId);
                if (itemDetails == null)
                {
                    this.ItemTable.Rows.Clear();
                }
                DataTable dt = GetItemDetailsTable(itemDetails);
                this.rptItem.DataSource = dt;
                this.rptItem.DataBind();

                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                txtExpenseDescription.Text = fields["ExpenseDescription"].ToString();
                lbTotalAmount.Text = fields["TotalAmount"].ToString();
                lblCashAdvanceAmount.Text = fields["CashAdvanceAmount"].ToString() == "" ? "0" : fields["CashAdvanceAmount"].ToString();
                lblPreTotalAmount.Text = fields["PreTotalAmount"].ToString();
                lblWorkFlowNumber.Text = fields["EBCWorkflowNumber"].ToString();
                string name = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                lblRequestedID.Text = employee.EmployeeID;
                lblRequestedBy.Text = employee.DisplayName;

                GetGLAccountDataTable();

                this.lblEmployeeID1.Text = employee.EmployeeID;
                this.txtEmployeeVendor.Text = (0 - double.Parse(fields["PreTotalAmount"].ToString())).ToString();
                this.txtEmployeeVendor.ReadOnly = true;
                this.hfCashAdvanceWorkFlowNumber.Value = fields["CashAdvanceWorkFlowNumber"] == null ? "" : fields["CashAdvanceWorkFlowNumber"].ToString().Trim();
            }
        }

        private DataTable GetCRTable()
        {
            DataTable CRTable = new DataTable();
            CRTable.Columns.Add("ExpenseType");
            CRTable.Columns.Add("ItemAmount");
            CRTable.Columns.Add("CostCenter");
            CRTable.Columns.Add("GLAccount");
            CRTable.Columns.Add("EmployeeID");
            CRTable.Columns.Add("EmployeeName");
            CRTable.Columns.Add("EBCWorkflowNumber");
            if (txtEmployeeVendor.Text != "0")
            {
                DataRow row = CRTable.Rows.Add();
                row["ExpenseType"] = "OR - employee vendor";
                row["CostCenter"] = "";
                row["ItemAmount"] = txtEmployeeVendor.Text;
                row["GLAccount"] = lblEmployeeID1.Text;
                row["EmployeeID"] = lblRequestedID.Text.Trim();
                row["EmployeeName"] = lblRequestedBy.Text.Trim();
                row["EBCWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
            }
            if (hfCashAdvanceWorkFlowNumber.Value != "")
            {
                List<string> strlist = hfCashAdvanceWorkFlowNumber.Value.Split(';').ToList<string>();
                foreach (string str in strlist)
                {
                    if (str != "")
                    {
                        string canumber = str.Substring(str.IndexOf('-') + 1);
                        DataRow row1 = CRTable.Rows.Add();
                        row1["ExpenseType"] = "OR - cash advance";
                        row1["CostCenter"] = "";
                        row1["ItemAmount"] = "-" + canumber;
                        row1["GLAccount"] = lblEmployeeID1.Text;
                        row1["EmployeeID"] = lblRequestedID.Text.Trim();
                        row1["EmployeeName"] = lblRequestedBy.Text.Trim();
                        row1["EBCWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
                    }
                }

            }
            return CRTable;
        }

        private void GetGLAccountDataTable()
        {
            System.Text.StringBuilder strGLAccount = new System.Text.StringBuilder();
            strGLAccount.Append("[");
            DataTable dt = WorkFlowUtil.GetCollectionByList("Expatriate BenefitType GLAccount").GetDataTable();
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

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("ItemAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");

            ItemTable.Columns.Add("EmployeeID");
            ItemTable.Columns.Add("EmployeeName");
            ItemTable.Columns.Add("EBCWorkflowNumber");
           
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
            RepeaterItem item = this.rptItem.Items[this.rptItem.Items.Count - 1];
            var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
            var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
            var txtAmount = (TextBox)item.FindControl("txtAmount");
            var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
            row["ExpenseType"] = ddlExpenseType.SelectedValue;
            row["CostCenter"] = ddlCostCenter.SelectedValue;
            row["ItemAmount"] = txtAmount.Text;
            row["GLAccount"] = lblGLAccount.Text;
            row["EmployeeID"] = lblRequestedID.Text.Trim();
            row["EmployeeName"] = lblRequestedBy.Text.Trim();
            row["EBCWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
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

        private void UpdateItem()
        {
            this.ItemTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                DataRow row = this.ItemTable.Rows.Add();
                row["ExpenseType"] = ddlExpenseType.SelectedValue;
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["ItemAmount"] = txtAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;

                //row["GLAccount"] = ExpatriateBenefitClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());



                row["EmployeeID"] = lblRequestedID.Text.Trim();
                row["EmployeeName"] = lblRequestedBy.Text.Trim();
                row["EBCWorkflowNumber"] = lblWorkFlowNumber.Text.Trim();
               
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
                    DataBindDDL(ddlCostCenter, CostCenters);
                    ddlExpenseType.SelectedValue = row["ExpenseType"].ToString();
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["ItemAmount"].ToString();
                    //lblGLAccount.Text = row["GLAccount"].ToString();
                    if (row["ExpenseType"].ToString() != "0")
                    {
                        if (row["GLAccount"].ToString() != "")
                        {
                            lblGLAccount.Text = row["GLAccount"].ToString();
                        }
                        else
                        {
                            lblGLAccount.Text = ExpatriateBenefitClaimCommon.GetGLAccountByExpenseType(row["ExpenseType"].ToString());
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

        private DataTable GetItemDetailsTable(DataTable itemDetails)
        {
            //List<int> index = new List<int>();
            DataTable dt = new DataTable();
            dt.Columns.Add("ExpenseType");
            dt.Columns.Add("ItemAmount");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("GLAccount");
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("EBCWorkflowNumber");
            
           foreach (DataRow dr in itemDetails.Rows)
            {
                if (dr["ExpenseType"].ToString().IndexOf("OR - employee vendor") == -1 && dr["ExpenseType"].ToString().IndexOf("OR - cash advance") == -1)
                {
                    DataRow dataRow = dt.Rows.Add();
                    dataRow["ExpenseType"] = dr["ExpenseType"].ToString();
                    dataRow["ItemAmount"] = dr["ItemAmount"].ToString();
                    dataRow["CostCenter"] = dr["CostCenter"].ToString();
                    dataRow["GLAccount"] = dr["GLAccount"].ToString();
                    dataRow["EmployeeID"] = dr["EmployeeID"].ToString();
                    dataRow["EmployeeName"] = dr["EmployeeName"].ToString();
                    dataRow["EBCWorkflowNumber"] = dr["EBCWorkflowNumber"].ToString();
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

        public void Update()
        {
            UpdateItem();
        }

    }
}