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
    public partial class DataListView : QFUserControl
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
                DataTable itemDetails = CreditCardClaimCommon.GetDataTableToSAP(requestId);
                DataTable rmbDT= CreditCardClaimCommon.GetDataSource(itemDetails, "AmountType='RMB'");
                DataTable USDDT = CreditCardClaimCommon.GetDataSource(itemDetails, "AmountType='USD' ");
                if (rmbDT != null && rmbDT.Rows.Count > 0)
                {
                    this.rptItem.DataSource = rmbDT;
                    this.rptItem.DataBind();
                }
                else 
                {
                    this.hfTableStatus.Value = "RMB";
                }
                if (USDDT != null && USDDT.Rows.Count > 0)
                {
                    this.rptUSDItem.DataSource = USDDT;
                    this.rptUSDItem.DataBind();
                }
                else
                {
                    this.hfTableStatus.Value = "USD";
                }
                
                this.ItemTable.Rows.Clear();
                foreach (DataRow dr in itemDetails.Rows)
                {
                    DataRow row = this.ItemTable.Rows.Add();
                    row["ID"] = dr["ID"].ToString();
                    row["ExpenseType"] = dr["ExpenseType"].ToString();
                    row["CostCenter"] = dr["CostCenter"].ToString();
                    row["DealAmount"] = dr["DealAmount"].ToString();
                    row["DepositAmount"] = dr["DepositAmount"].ToString();
                    row["PayAmount"] = dr["PayAmount"].ToString();
                    row["GLAccount"] = dr["GLAccount"].ToString();
                    row["TransactionDescription"] = dr["TransactionDescription"].ToString();
                    row["AmountType"] = dr["AmountType"].ToString();
                }

                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                txtExpenseDescription.Text = fields["ExpenseDescription"].ToString();
                lbTotalAmount.Text = fields["TotalAmount"].AsString();
                lblSAPNo.Text = fields["SAPNo"].AsString() + ";" + fields["SAPUSDNo"].AsString();
                lblWorkFlowNumber.Text = fields["CCCWWorkflowNumber"].AsString();
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
            ItemTable.Columns.Add("DealAmount");
            ItemTable.Columns.Add("DepositAmount");
            ItemTable.Columns.Add("PayAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");
            ItemTable.Columns.Add("AmountType");
            ItemTable.Columns.Add("TransactionDescription");

            ItemTable.Rows.Add();
            return ItemTable;
        }
    }
}