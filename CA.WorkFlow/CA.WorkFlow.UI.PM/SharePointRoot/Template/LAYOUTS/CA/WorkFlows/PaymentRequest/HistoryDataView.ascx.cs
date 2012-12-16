namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using System.Web.UI;
    using System.Data;
    using CA.SharePoint;
    using Microsoft.SharePoint;

    public partial class HistoryDataView : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string subPRNo = Request.QueryString["SUBPRNO"].ToString();
                DataTable prTable = PaymentRequestComm.GetPaymentRequestItemsInfoBySUBPRNO(subPRNo).GetDataTable();

                FillVendorInfo(prTable);
                FillEmployeeInfo(prTable);
                FillPaymentRequestInfo(prTable);
                FillPeymentInstallmentInfo(prTable);
            }
        }

        /// <summary>
        /// 填充供应商信息
        /// </summary>
        private void FillVendorInfo(DataTable dTable)
        {
            if (dTable != null)
            {
                txtVenderCode.Text = dTable.Rows[0]["VendorNo"].ToString();
                txtVenderName.Text = dTable.Rows[0]["VendorName"].ToString();
                txtBankName.Text = dTable.Rows[0]["BankName"].ToString();
                txtBankAC.Text = dTable.Rows[0]["BankAccount"].ToString();
                txtSwiftCode.Text = dTable.Rows[0]["SwiftCode"].ToString();
            }
        }

        /// <summary>
        /// 填充用户信息
        /// </summary>
        private void FillEmployeeInfo(DataTable dTable)
        {
            txtApplicant.Text = dTable.Rows[0]["Applicant"].ToString();
            txtDept.Text = dTable.Rows[0]["Dept"].ToString();
        }

        private void FillPeymentInstallmentInfo(DataTable dTable)
        {
            if (dTable != null)
            {
                txtTotalAmount.Text = dTable.Rows[0]["TotalAmount"].ToString();
                txtPaidBefore.Text = dTable.Rows[0]["PaidBefore"].ToString();
                txtPaidThisTime.Text = dTable.Rows[0]["PaidThisTime"].ToString();
                txtBalance.Text = dTable.Rows[0]["Balance"].ToString();
            }
        }

        private void FillPaymentRequestInfo(DataTable dTable)
        {
            if (dTable != null)
            {
                txtCostCenter.Text = dTable.Rows[0]["CostCenter"].ToString();
                txtRemark.Text = dTable.Rows[0]["InvoiceRemark"].ToString();
                txtPaymentDesc.Text = dTable.Rows[0]["PaymentDesc"].ToString();
                txtContractPO.Text = dTable.Rows[0]["ContractPONo"].ToString();
                txtSystemPO.Text = dTable.Rows[0]["SystemPONo"].ToString();
                txtPaymentReason.Text = dTable.Rows[0]["PaymentReason"].ToString();

                radioExpenceType.SelectedIndex = dTable.Rows[0]["PaymentType"].ToString() == "1" ? 0 : 1;
                radioInstallment.SelectedIndex = dTable.Rows[0]["IsInstallment"].ToString() == "1" ? 0 : 1;
                radioContractPO.SelectedIndex = dTable.Rows[0]["IsContractPO"].ToString() == "1" ? 0 : 1;
                radioContractGR.SelectedIndex = dTable.Rows[0]["IsContractGR"].ToString() == "1" ? 0 : 1;
                radioSystemGR.SelectedIndex = dTable.Rows[0]["IsSystemGR"].ToString() == "1" ? 0 : 1;
                radioSystemPO.SelectedIndex = dTable.Rows[0]["IsSystemPO"].ToString() == "1" ? 0 : 1;
                radioInvoice.SelectedIndex = dTable.Rows[0]["IsAttachedInvoice"].ToString() == "1" ? 0 : 1;
            }
        }
    }
}