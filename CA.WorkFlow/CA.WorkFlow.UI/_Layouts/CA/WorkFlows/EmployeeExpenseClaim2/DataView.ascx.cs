namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Data;
    using System.Globalization;
    using CA.SharePoint.Utilities.Common;
    using QuickFlow.Core;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Collections;

    public partial class DataView : BaseWorkflowUserControl
    {
        private string msg;
        public string MSG { get { return msg; } }

        private string dept;
        public string Department { set { dept = value; } }

        private string requestId;
        public string RequestId { set { this.requestId = value; } }

        private string applicant;
        public string Applicant { set { applicant = value; } }

        private string mode;
        public string Mode { set { mode = value; } }

        private DataTable itemTable;
        public DataTable ItemTable { get { return itemTable; } }

        private double totalAmount;
        public double TotalAmount { get { return totalAmount; } }

        private string displayMode;
        public string DisplayMode { set { displayMode = value; } }

        private string workFlowNumber;
        public string WorkFlowNumber { get { return GetWorkFlowNumber(); } }

        private double hidtas;
        public double Hidtas { get { return Convert.ToDouble(this.hidta.Value.ToString()); } }
        private double hidcas;
        public double Hidcas { get { return Convert.ToDouble(this.hidca.Value.ToString()); } }

        private string step;

        public string Step
        {
            get { return step; }
            set { step = value; }
        }
        private string summaryExpenseType;

        public string SummaryExpenseType
        {
            get { return this.hidSummaryExpenseType.Value.Trim(); }
            set { this.hidSummaryExpenseType.Value = value; }
        }
        private string pending;

        public string Pending
        {
            get { return pending; }
            set { pending = value; }
        }

        private string satus;

        public string Satus
        {
            get { return satus; }
            set { satus = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                LoadExpenseType();
                this.rptItem.DataSource = EmployeeExpenseClaimCommon.GetDataTable(requestId);
                this.rptItem.DataBind();
                bool isConfirmTask = WorkflowContext.Current.Step == "ConfirmTask" ? true : false;
                if (isConfirmTask)
                {
                    FillFiancePendingForm();
                }
                if (this.Step == "DisplayStep") 
                {
                    FillFiancePendingForm();
                }

                if (WorkflowContext.Current.DataFields["Status"] != null)
                {
                    if (WorkflowContext.Current.DataFields["Status"].ToString() == "Completed")
                    {
                        Satus = "Confirm";
                    }
                }

            }
            if (displayMode != null)
            {
                this.hidDisplayMode.Value = displayMode;
            }
            
            
        }

        public void SetSpecialApproveResult(string result) 
        {
            this.hidspecialApproveResult.Value = result;
        }

        public void SetSpecialApprove()
        {
            UpdateItem();
            if (itemTable.Rows.Count > 0)
            {
                EmployeeExpenseClaimCommon.BatchUpdateItems(itemTable);
            }
        }

        

        public void SetApprovedSpecialApprove()
        {
            itemTable = CreateItemTable();
            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var hidItemId = (HiddenField)item.FindControl("hidItemId");
                var hidApprovedAmount = (HiddenField)item.FindControl("hidApprovedAmount");

                var hidPreAmount = (HiddenField)item.FindControl("hidPreAmount"); ;


                var lbCompanyStd = (Label)item.FindControl("lbCompanyStd");
                //var isSpecialApprove = (CheckBox)item.FindControl("IsSpecialApprove");
                var ddlSpecialApprove = (DropDownList)item.FindControl("ddlSpecialApprove");
                if (IsNotNumberic(lbCompanyStd.Text))
                {
                    lbCompanyStd.Text = "0";
                }
                // if (isSpecialApprove.Checked && Convert.ToDouble(hidApprovedAmount.Value) > Convert.ToDouble(lbCompanyStd.Text))
                // if (isSpecialApprove.Checked)
                if (ddlSpecialApprove.SelectedValue == "1" )
                {
                    DataRow row = itemTable.Rows.Add();
                    row["ID"] = hidItemId.Value;
                    row["ApprovedAmount"] = hidApprovedAmount.Value;
                    row["IsSpecialApprove"] = "11";
                    row["OriginalAmount"] = hidPreAmount.Value.Trim();
                }
                if (ddlSpecialApprove.SelectedValue == "2")
                {
                    DataRow row1 = itemTable.Rows.Add();
                    row1["ID"] = hidItemId.Value;
                    row1["ApprovedAmount"] = hidApprovedAmount.Value;
                    row1["IsSpecialApprove"] = "00";
                    row1["OriginalAmount"] = hidPreAmount.Value.Trim();
                }
                if (ddlSpecialApprove.SelectedValue == "0")
                {
                    DataRow row1 = itemTable.Rows.Add();
                    row1["ID"] = hidItemId.Value;
                    row1["ApprovedAmount"] = hidApprovedAmount.Value;
                    row1["IsSpecialApprove"] = "22";
                    row1["OriginalAmount"] = hidPreAmount.Value.Trim();
                }

            }
            if (itemTable.Rows.Count > 0)
            {
                EmployeeExpenseClaimCommon.BatchUpdateItems(itemTable, "SpecialApprove");
            }
        }
        private bool IsNotNumberic(string oText)
        {
            if (oText == "NaN" || oText == "")
            {
                return true;
            }
            float fnum = 0;
            if (float.TryParse(oText, NumberStyles.Any, CultureInfo.InvariantCulture, out fnum))
            {
                return false;
            }
            else
                return true;
        }
        private void UpdateItem()
        {
            itemTable = CreateItemTable();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var hidItemId = (HiddenField)item.FindControl("hidItemId");
                var hidApprovedAmount = (HiddenField)item.FindControl("hidApprovedAmount");
                //var isSpecialApprove = (CheckBox)item.FindControl("IsSpecialApprove");
                var ddlSpecialApprove = (DropDownList)item.FindControl("ddlSpecialApprove");
                //if (!isSpecialApprove.Checked)
                //{
                //    DataRow row = itemTable.Rows.Add();
                //    row["ID"] = hidItemId.Value;
                //    row["ApprovedAmount"] = hidApprovedAmount.Value;
                //    row["IsSpecialApprove"] = 0;
                //}
                if (ddlSpecialApprove.SelectedValue == "1")
                {
                    DataRow row = itemTable.Rows.Add();
                    row["ID"] = hidItemId.Value;
                    row["ApprovedAmount"] = hidApprovedAmount.Value;
                    row["IsSpecialApprove"] = "1";
                }
                if (ddlSpecialApprove.SelectedValue == "2")
                {
                    DataRow row1 = itemTable.Rows.Add();
                    row1["ID"] = hidItemId.Value;
                    row1["ApprovedAmount"] = hidApprovedAmount.Value;
                    row1["IsSpecialApprove"] = "00";
                }
                if (ddlSpecialApprove.SelectedValue == "0")
                {
                    DataRow row1 = itemTable.Rows.Add();
                    row1["ID"] = hidItemId.Value;
                    row1["ApprovedAmount"] = hidApprovedAmount.Value;
                    row1["IsSpecialApprove"] = "22";
                }



                totalAmount += Convert.ToDouble(hidApprovedAmount.Value);
            }
        }

        private DataTable CreateItemTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("ApprovedAmount");
            dt.Columns.Add("IsSpecialApprove");


            dt.Columns.Add("OriginalAmount");
            return dt;
        }

        public override bool Validate(string action)
        {
            bool isValid = false;
            if (action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                isValid = WorkflowContext.Current.TaskFields["Body"].AsString().IsNotNullOrWhitespace();
                if (!isValid)
                {
                    msg = "Please fill in the Reject Comments.";
                    return isValid;
                }
            }
            return true;
        }

        public string GetWorkFlowNumber() 
        {
            RepeaterItem rItem = this.rptItem.Items[0];
            var hidTotalAmount = (HiddenField)rItem.FindControl("hidTotalAmount");
            string workFlowNumber = hidTotalAmount.Value.Substring(0, hidTotalAmount.Value.IndexOf('-'));
            return workFlowNumber;
        }
        private bool IsRadioListItem(RadioButtonList radioButtonList, string fieldValue)
        {
            return radioButtonList.Items.Contains(new ListItem(fieldValue, fieldValue));
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

                        this.Step = "ConfirmTask1";
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
            DataTable dt =WorkFlowUtil.GetCollectionByList("Employee Expense Claim Type").GetDataTable();
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