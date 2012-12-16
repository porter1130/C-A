namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using System.Web.UI;
    using System.Data;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Web.UI.WebControls;
    using System.Collections;

    public partial class DataView : UserControl
    {
        #region Field
        private string step;
        public string Step
        {
            get { return step; }
            set { step = value; }
        }
        private string pending;
        public string Pending
        {
            get { return pending; }
            set { pending = value; }
        }
        private string requestId;
        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }
        private string wFStep;
        public string Wfstep
        {
            get { return wFStep; }
            set { wFStep = value; }
        }
        public string FANO
        {
            get
            {
                return this.hfFAList.Value.Trim();
            }
        }
        public string Opex_Capex_Status
        {
            get
            {
                return this.FAStatus.Value.Trim();
            }
        }
        public string VendorNameText
        {
            get
            {
                return this.txtVenderName.Text.Trim().Length > 5
                      ? this.txtVenderName.Text.Trim().Substring(0, 5) + "..."
                      : this.txtVenderName.Text.Trim();
            }
        }
        public string ApproveAmount
        {
            get
            {
                if (this.radioInstallment.SelectedIndex == 1)
                {
                    return this.txtTotalAmount1.Text.Trim();
                }
                else
                {
                    return this.txtPaidThisTime.Text.Trim();
                }
            }
        }
        #endregion

        #region

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadExpenseType();
                bool isConfirmTask = WorkflowContext.Current.Step == "ConfirmTask" ? true : false;
                if (isConfirmTask)
                {
                    FillFiancePendingForm();
                }
                if (this.Step == "DisplayStep")
                {
                    FillFiancePendingForm();
                }
                FillPaymentInfo();
                if (requestId != "" && requestId != null)
                {
                    DataTable itemDetails = PaymentRequestComm.GetDataTable(requestId);
                    WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                    this.FAStatus.Value = Wfstep;//ConfirmTask  ApproveTask  DisplayStep
                    var rows = itemDetails.AsEnumerable();
                    foreach (DataRow dr in rows)
                    {
                        if (dr["ExpenseType"].ToString().ToLower().IndexOf("tax") == -1)
                        {
                            if (dr["GLAccount"].ToString().StartsWith("A"))
                            {
                                lblTDStatus.Text = "Asset Class";
                                this.lblExpenseType.Text = "Asset Type";
                                FAStatus.Value += "_Capex_FANO";
                            }
                            else
                            {
                                lblTDStatus.Text = "GL Account";
                                this.lblExpenseType.Text = "Expense Type";
                                lblTDStatus.Text = "GL Account";
                                FAStatus.Value += "_Opex_FANO";
                            }
                            break;
                        }

                    }
                    this.rptItem.DataSource = itemDetails;
                    this.rptItem.DataBind();

                    this.lblCurrency.Text = fields["Currency"].AsString();
                    this.lblExchangeRate.Text = fields["ExchRate"].AsString();
                }
            }
        }

        private DataTable CreateInstalmentDT()
        {
            DataTable dTable = new DataTable();
            dTable.Columns.Add("PaidBefore");
            dTable.Columns.Add("PaidThisTime");
            dTable.Columns.Add("Balance");
            dTable.Columns.Add("PaidInd");
            dTable.Columns.Add("TotalAmount");
            dTable.Columns.Add("PaidThisTimeAmount");
            return dTable;
        }

        private void FillPeymentInstallmentInfo(DataTable dTable)
        {
            if (dTable != null)
            {
                if (string.IsNullOrEmpty(dTable.Rows[0]["TotalAmount"].ToString()) == false)
                {
                    txtTotalAmount.Text = dTable.Rows[0]["TotalAmount"].ToString();
                    txtTotalAmount1.Text = dTable.Rows[0]["TotalAmount"].ToString();
                    if (string.IsNullOrEmpty(dTable.Rows[0]["PaidThisTime"].ToString()) == false)
                    {
                        if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == true)
                        {
                            txtPaidBefore.Text = Math.Round(decimal.Parse(dTable.Rows[0]["TotalAmount"].ToString()) *
                                             decimal.Parse(dTable.Rows[0]["PaidBefore"].ToString()) / 100, 2).ToString();
                            txtPaidThisTime.Text = Math.Round(decimal.Parse(dTable.Rows[0]["TotalAmount"].ToString()) *
                                             decimal.Parse(dTable.Rows[0]["PaidThisTime"].ToString()) / 100, 2).ToString();
                        }
                        else
                        {
                            txtPaidBefore.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidBefore"].ToString()), 2).ToString();
                            txtPaidThisTime.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidThisTime"].ToString()), 2).ToString();
                        }
                        txtBlance.Text = Math.Round(decimal.Parse(txtTotalAmount.Text) - decimal.Parse(txtPaidBefore.Text) -
                                            decimal.Parse(txtPaidThisTime.Text), 2).ToString();
                    }
                }
            }
        }

        private void FillPaymentInfo()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            txtVenderCode.Text = GetString(fields["VendorNo"]);
            txtVenderName.Text = GetString(fields["VendorName"]);
            txtBankName.Text = GetString(fields["BankName"]);
            txtBankAC.Text = GetString(fields["BankAccount"]);
            txtSwiftCode.Text = GetString(fields["SwiftCode"]);

            txtVendorCity.Text = GetString(fields["VendorCity"]);
            txtVendorCountry.Text = GetString(fields["VendorCountry"]);
            txtBankCity.Text = GetString(fields["BankCity"]);

            txtApplicant.Text = GetString(fields["Applicant"]);
            txtDept.Text = GetString(fields["Dept"]);
            if (fields["TotalAmount"] != null)
            {
                string subPRNo = fields["SubPRNo"].AsString();
                string index = subPRNo.Substring(subPRNo.IndexOf('_') + 1, subPRNo.Length - subPRNo.IndexOf('_') - 1);
                DataTable dt = GetInstallmentInfo(fields["PONo"].AsString(), index);
                FillPeymentInstallmentInfo(dt);
            }
            txtCostCenter.Text = GetString(fields["CostCenter"]);
            txtRemark.Text = GetString(fields["InvoiceRemark"]);
            txtPaymentDesc.Text = GetString(fields["PaymentDesc"]);
            txtContractPO.Text = GetString(fields["ContractPONo"]);
            txtSystemPO.Text = GetString(fields["SystemPONo"]);
            txtPaymentReason.Text = GetString(fields["PaymentReason"]);
            radioInstallment.SelectedIndex = GetString(fields["IsInstallment"]).Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            radioContractPO.SelectedIndex = GetString(fields["IsContractPO"]).Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            radioNeedGR.SelectedIndex = GetString(fields["IsNeedGR"]).Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            radioSystemGR.SelectedIndex = GetString(fields["IsAllSystemGR"]).Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            radioSystemPO.SelectedIndex = GetString(fields["IsSystemPO"]).Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            radioInvoice.SelectedIndex = GetString(fields["IsAttachedInvoice"]).Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
        }

        private string GetString(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        public void SavePendingForm()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (this.rblFapiao.SelectedValue == "other reasons, please state")
            {
                fields["FapiaoReason"] = this.txtFapiaoOtherReason.Text;
            }
            else
            {
                fields["FapiaoReason"] = this.rblFapiao.SelectedValue;
            }
            if (this.rblInformation.SelectedValue == "other reasons, please state")
            {
                fields["InformationReason"] = this.txtInformationOtherReason.Text;
            }
            else
            {
                fields["InformationReason"] = this.rblInformation.SelectedValue;
            }
            if (this.rblClaimedAmt.SelectedValue == "other reasons, please state")
            {
                fields["ClaimedAmtReason"] = this.txtClaimedOtherReason.Text;
            }
            else
            {
                fields["ClaimedAmtReason"] = this.rblClaimedAmt.SelectedValue;
            }
            fields["OtherReasons"] = this.txtOtherReasons.Text;
            fields["ReasonsResult"] = "1";
        }

        public void SaveRejectPendingForm()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (this.rblFapiao.SelectedValue == "other reasons, please state")
            {
                fields["FapiaoReason"] = this.txtFapiaoOtherReason.Text;
            }
            else
            {
                fields["FapiaoReason"] = this.rblFapiao.SelectedValue;
            }
            if (this.rblInformation.SelectedValue == "other reasons, please state")
            {
                fields["InformationReason"] = this.txtInformationOtherReason.Text;
            }
            else
            {
                fields["InformationReason"] = this.rblInformation.SelectedValue;
            }
            if (this.rblClaimedAmt.SelectedValue == "other reasons, please state")
            {
                fields["ClaimedAmtReason"] = this.txtClaimedOtherReason.Text;
            }
            else
            {
                fields["ClaimedAmtReason"] = this.rblClaimedAmt.SelectedValue;
            }
            fields["OtherReasons"] = this.txtOtherReasons.Text;
            fields["ReasonsResult"] = "0";
        }

        private void FillFiancePendingForm()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (fields["ReasonsResult"] != null)
            {
                if (fields["ReasonsResult"].ToString() == "1")
                {
                    if (this.Step == "DisplayStep")
                    {
                        this.Step = "Display";
                    }
                }
            }
            if (!IsRadioListItem(this.rblFapiao, fields["FapiaoReason"].AsString()))
            {
                if (fields["FapiaoReason"].AsString() != "")
                {
                    this.rblFapiao.SelectedValue = "other reasons, please state";
                    this.txtFapiaoOtherReason.Text = fields["FapiaoReason"].AsString();
                }
            }
            else
            {
                this.rblFapiao.SelectedValue = fields["FapiaoReason"].AsString();
            }
            if (!IsRadioListItem(this.rblInformation, fields["InformationReason"].AsString()))
            {
                if (fields["InformationReason"].AsString() != "")
                {
                    this.rblInformation.SelectedValue = "other reasons, please state";
                    this.txtInformationOtherReason.Text = fields["InformationReason"].AsString();
                }
            }
            else
            {
                this.rblInformation.SelectedValue = fields["InformationReason"].AsString();
            }
            if (!IsRadioListItem(this.rblClaimedAmt, fields["ClaimedAmtReason"].AsString()))
            {
                if (fields["ClaimedAmtReason"].AsString() != "")
                {
                    this.rblClaimedAmt.SelectedValue = "other reasons, please state";
                    this.txtClaimedOtherReason.Text = fields["ClaimedAmtReason"].AsString();
                }
            }
            else
            {
                this.rblClaimedAmt.SelectedValue = fields["ClaimedAmtReason"].AsString();
            }
            this.txtOtherReasons.Text = fields["OtherReasons"].AsString();
        }

        private bool IsRadioListItem(RadioButtonList radioButtonList, string fieldValue)
        {
            return radioButtonList.Items.Contains(new ListItem(fieldValue, fieldValue));
        }

        #endregion

        #region

        private DataTable GetInstallmentInfo(string poID)
        {
            decimal mPaidBefore = 0; ;
            DataTable dTable = PaymentRequestComm.GetPaymentInstallmentInfo(poID).GetDataTable();
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow row in dTable.Rows)
                {
                    if (row["IsPaid"].ToString() == "0" || row["IsPaid"].ToString().IsNullOrWhitespace())
                    {
                        DataTable dTable1 = CreateInstalmentDT();
                        DataRow dRow = dTable1.NewRow();
                        dRow["PaidBefore"] = mPaidBefore;
                        if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == false)
                        {
                            dRow["Balance"] = (100 - mPaidBefore - (row["PaidThisTimeAmount"].ToString().IsNullOrWhitespace()
                                           ? 0 : decimal.Parse(row["PaidThisTimeAmount"].ToString())));
                            dRow["PaidThisTime"] = row["PaidThisTimeAmount"];
                        }
                        else
                        {
                            dRow["Balance"] = (100 - mPaidBefore - (row["Paid"].ToString().IsNullOrWhitespace()
                                           ? 0 : decimal.Parse(row["Paid"].ToString())));
                            dRow["PaidThisTime"] = row["Paid"];
                        }
                        dRow["PaidThisTimeAmount"] = row["PaidThisTimeAmount"];
                        dRow["PaidInd"] = row["Index"];
                        dRow["TotalAmount"] = row["TotalAmount"];
                        dTable1.Rows.Add(dRow);
                        ViewState["Installment"] = dTable1;
                        return dTable1;
                    }
                    else
                    {
                        if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == false)
                        {
                            mPaidBefore += decimal.Parse(row["PaidThisTimeAmount"].ToString());
                        }
                        else
                        {
                            mPaidBefore += decimal.Parse(row["Paid"].ToString());
                        }
                    }
                }
            }
            return null;
        }

        private DataTable GetInstallmentInfo(string poID, string index)
        {
            decimal mPaidBefore = 0; ;
            DataTable dTable = PaymentRequestComm.GetPaymentInstallmentInfo(poID).GetDataTable();
            if (dTable != null)
            {
                if (dTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dTable.Rows)
                    {
                        if (row["IsPaid"].ToString() == "0" ||
                            (row["IsPaid"].ToString() == "1" && row["Index"].AsString() == index))
                        {
                            DataTable data = CreateInstalmentDT();
                            DataRow dRow = data.NewRow();
                            dRow["PaidBefore"] = mPaidBefore;
                            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == false)
                            {
                                dRow["PaidThisTime"] = row["PaidThisTimeAmount"];
                            }
                            else
                            {
                                dRow["PaidThisTime"] = row["Paid"];
                            }
                            dRow["PaidThisTimeAmount"] = row["PaidThisTimeAmount"];
                            dRow["PaidInd"] = row["Index"];
                            dRow["TotalAmount"] = row["TotalAmount"];
                            data.Rows.Add(dRow);
                            ViewState["Installment"] = data;
                            return data;
                        }
                        if (row["IsPaid"].ToString() == "1" && row["Index"].AsString() != index)
                        {
                            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == false)
                            {
                                mPaidBefore += decimal.Parse(row["PaidThisTimeAmount"].ToString());
                            }
                            else
                            {
                                mPaidBefore += decimal.Parse(row["Paid"].ToString());
                            }
                        }
                    }
                }
            }
            return null;
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
            DataTable dt= PaymentRequestComm.GetPRExpenseTypeDataTable(WorkflowContext.Current.DataFields["RequestType"].ToString());
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
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
                    var lblExpenseType = (Label)item.FindControl("lblExpenseType");
                    lblExpenseType.Text = OriginalExpenseType[row["ExpenseType"].ToString()].AsString();
                }
            }
        }

    }
}