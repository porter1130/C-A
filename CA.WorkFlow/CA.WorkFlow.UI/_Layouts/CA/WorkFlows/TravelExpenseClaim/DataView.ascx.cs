

using System.Collections.Generic;
namespace CA.WorkFlow.UI.TravelExpenseClaim
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using System.Data;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint.WebControls;

    public partial class DataView : BaseWorkflowUserControl
    {
        private string requestId;

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        private string msg;
        public string MSG { get { return msg; } }

        private string mode;

        public string Mode
        {
            set { mode = value; }
            get { return mode; }
        }

        private string _step;

        public string Step
        {
            get { return _step; }
            set { _step = value; }
        }

        private string _fapiaoStatus;

        public string FapiaoStatus
        {
            get { return _fapiaoStatus; }
            set { _fapiaoStatus = value; }
        }
        private string _informationStatus;

        public string InformationStatus
        {
            get { return _informationStatus; }
            set { _informationStatus = value; }
        }
        private string _claimedAmtStatus;

        public string ClaimedAmtStatus
        {
            get { return _claimedAmtStatus; }
            set { _claimedAmtStatus = value; }
        }

        private string _otherReasonsStatus;

        public string OtherReasonsStatus
        {
            get { return _otherReasonsStatus; }
            set { _otherReasonsStatus = value; }
        }

        internal DataTable HotelTable
        {
            get
            {
                return this.ViewState["HotelTable"] as DataTable;
            }
            set
            {
                this.ViewState["HotelTable"] = value;
            }
        }
        internal DataTable MealTable
        {
            get
            {
                return this.ViewState["MealTable"] as DataTable;
            }
            set
            {
                this.ViewState["MealTable"] = value;
            }
        }
        internal DataTable TransTable
        {
            get
            {
                return this.ViewState["TransTable"] as DataTable;
            }
            set
            {
                this.ViewState["TransTable"] = value;
            }
        }
        internal DataTable SampleTable
        {
            get
            {
                return this.ViewState["SampleTable"] as DataTable;
            }
            set
            {
                this.ViewState["SampleTable"] = value;
            }
        }
        internal DataTable OthersTable
        {
            get
            {
                return this.ViewState["OthersTable"] as DataTable;
            }
            set
            {
                this.ViewState["OthersTable"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(requestId))
                {
                    DataTable dt = TravelExpenseClaimCommon.GetDataTable(requestId, "Travel Expense Claim Details");
                    if (dt != null)
                    {
                        HotelTable = dt.Clone();
                        MealTable = dt.Clone();
                        TransTable = dt.Clone();
                        SampleTable = dt.Clone();
                        OthersTable = dt.Clone();

                        HotelTable = TravelExpenseClaimCommon.GetDataSource(HotelTable, dt, "ExpenseType='Hotel'");
                        MealTable = TravelExpenseClaimCommon.GetDataSource(MealTable, dt, "ExpenseType='Meal Allowance'");
                        TransTable = TravelExpenseClaimCommon.GetDataSource(TransTable, dt, "ExpenseType='Local Transportation'");
                        SampleTable = TravelExpenseClaimCommon.GetDataSource(SampleTable, dt, "ExpenseType='Sample Purchase'");
                        OthersTable = TravelExpenseClaimCommon.GetDataSource(OthersTable, dt, "ExpenseType='Others'");


                        this.rptHotel.DataSource = HotelTable;
                        this.rptHotel.DataBind();
                        this.rptMeal.DataSource = MealTable;
                        this.rptMeal.DataBind();
                        this.rptTrans.DataSource = TransTable;
                        this.rptTrans.DataBind();
                        this.rptSample.DataSource = SampleTable;
                        this.rptSample.DataBind();
                        this.rptOthers.DataSource = OthersTable;
                        this.rptOthers.DataBind();
                    }
                    SetControlMode();
                }
            }
        }

        private void SetControlMode()
        {
            List<Repeater> repeaters = new List<Repeater> { rptHotel, rptMeal, rptTrans, rptSample, rptOthers };
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            switch (this.mode)
            {
                case "Display":
                    foreach (Repeater repeater in repeaters)
                    {
                        foreach (RepeaterItem item in repeater.Items)
                        {
                            var lblApprovedRmbAmt = (Label)item.FindControl("lblApprovedRmbAmt");
                            var cbSpecialApproved = (CheckBox)item.FindControl("cbSpecialApproved");
                            var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                            var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");

                            lblApprovedRmbAmt.Visible = false;
                            cbSpecialApproved.Visible = false;
                            cbPaidByCredit.Enabled = false;
                            cbSpecialApprove.Enabled = false;
                        }
                    }

                    this.ffTotalCost.ControlMode = SPControlMode.Display;
                    this.ffCashAdvanced.ControlMode = SPControlMode.Display;
                    this.ffNetPayable.ControlMode = SPControlMode.Display;
                    this.ffPaidByCreditCard.ControlMode = SPControlMode.Display;
                    this.ffComparedToApproved.ControlMode = SPControlMode.Display;
                    this.ffFinanceRemark.ControlMode = SPControlMode.Display;

                    break;
                case "Approve":
                    bool isConfirmTask = WorkflowContext.Current.Step == "ConfirmTask" ? true : false;
                    foreach (Repeater repeater in repeaters)
                    {
                        foreach (RepeaterItem item in repeater.Items)
                        {
                            var lblRmbAmt = (Label)item.FindControl("lblRmbAmt");
                            var cbSpecialApproved = (CheckBox)item.FindControl("cbSpecialApproved");
                            var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                            var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");

                            lblRmbAmt.Visible = false;
                            cbSpecialApprove.Visible = false;
                            cbPaidByCredit.Enabled = false;

                            if (isConfirmTask)
                            {
                                cbSpecialApproved.Enabled = false;
                            }

                        }
                    }
                    if (isConfirmTask)
                    {
                        this.ffTotalCost.ControlMode = SPControlMode.Display;
                        this.ffCashAdvanced.ControlMode = SPControlMode.Display;
                        this.ffNetPayable.ControlMode = SPControlMode.Display;
                        this.ffPaidByCreditCard.ControlMode = SPControlMode.Display;
                        this.ffTotalExceptFlight.ControlMode = SPControlMode.Display;
                        this.ffComparedToApproved.ControlMode = SPControlMode.Display;

                        if (fields["Status"].AsString().Equals(CAWorkflowStatus.TE_Finance_Pending, StringComparison.CurrentCultureIgnoreCase)
                            || fields["Status"].AsString().Equals(CAWorkflowStatus.TE_Finance_Reject, StringComparison.CurrentCultureIgnoreCase))
                        {
                            FillFiancePendingForm();
                        }

                    }
                    else
                    {
                        this.ffFinanceRemark.ControlMode = SPControlMode.Display;
                    }
                    break;
                default:
                    foreach (Repeater repeater in repeaters)
                    {
                        foreach (RepeaterItem item in repeater.Items)
                        {
                            var lblRmbAmt = (Label)item.FindControl("lblRmbAmt");
                            var cbSpecialApproved = (CheckBox)item.FindControl("cbSpecialApproved");
                            var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                            var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");

                            lblRmbAmt.Visible = false;
                            cbSpecialApprove.Visible = false;
                            cbPaidByCredit.Enabled = false;

                            cbSpecialApproved.Enabled = false;
                            //if (cbSpecialApprove.Checked)
                            //{
                            //    cbSpecialApprove.Enabled = false;
                            //}
                        }
                    }

                    this.ffTotalCost.ControlMode = SPControlMode.Display;
                    this.ffCashAdvanced.ControlMode = SPControlMode.Display;
                    this.ffNetPayable.ControlMode = SPControlMode.Display;
                    this.ffPaidByCreditCard.ControlMode = SPControlMode.Display;
                    this.ffTotalExceptFlight.ControlMode = SPControlMode.Display;
                    this.ffComparedToApproved.ControlMode = SPControlMode.Display;

                    FillFiancePendingForm();
                    break;
            }
        }

        private void FillFiancePendingForm()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (!IsRadioListItem(this.rblFapiao, fields["FapiaoReason"].AsString()))
            {
                if (fields["FapiaoReason"].AsString() != "-1")
                {
                    this.rblFapiao.SelectedValue = "other reasons, please state";
                    this.txtFapiaoOtherReason.CssClass = "";
                    this.txtFapiaoOtherReason.Text = fields["FapiaoReason"].AsString();
                }
                else
                {
                    this.FapiaoStatus = "hidden";
                }
            }
            else
            {

                this.rblFapiao.SelectedValue = fields["FapiaoReason"].AsString();

            }

            if (!IsRadioListItem(this.rblInformation, fields["InformationReason"].AsString()))
            {
                if (fields["InformationReason"].AsString() != "-1")
                {
                    this.rblInformation.SelectedValue = "other reasons, please state";
                    this.txtFapiaoOtherReason.CssClass = "";
                    this.txtInformationOtherReason.Text = fields["InformationReason"].AsString();
                }
                else
                {
                    this.InformationStatus = "hidden";
                }
            }
            else
            {

                this.rblInformation.SelectedValue = fields["InformationReason"].AsString();

            }

            if (!IsRadioListItem(this.rblClaimedAmt, fields["ClaimedAmtReason"].AsString()))
            {
                if (fields["ClaimedAmtReason"].AsString() != "-1")
                {
                    this.rblClaimedAmt.SelectedValue = "other reasons, please state";
                    this.txtFapiaoOtherReason.CssClass = "";
                    this.txtClaimedOtherReason.Text = fields["ClaimedAmtReason"].AsString();
                }
                else
                {
                    this.ClaimedAmtStatus = "hidden";
                }
            }
            else
            {

                this.rblClaimedAmt.SelectedValue = fields["ClaimedAmtReason"].AsString();

            }
            if (fields["OtherReasons"].AsString().IsNotNullOrWhitespace())
            {
                this.txtOtherReasons.Text = fields["OtherReasons"].AsString();
            }
            else
            {
                this.OtherReasonsStatus = "hidden";
            }
        }

        private bool IsRadioListItem(RadioButtonList radioButtonList, string fieldValue)
        {
            return radioButtonList.Items.Contains(new ListItem(fieldValue, fieldValue));
        }

        public override bool Validate(string action)
        {
            bool isValid = false;
            if (action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                if (WorkflowContext.Current.Step.Equals("NextApproveTask", StringComparison.CurrentCultureIgnoreCase))
                {
                    isValid = WorkflowContext.Current.TaskFields["Body"].AsString().IsNotNullOrWhitespace();
                    if (!isValid)
                    {
                        msg = "Please fill in the Reject Comments.";
                        return isValid;
                    }
                }
            }
            return true;
        }

        internal void SetSpecialApprove(string workflowNumber)
        {
            //        JavaScriptSerializer oSerializer = new JavaScriptSerializer();

            //        string hidHotelItemValue = this.hidHotelItemValue.Value,
            //hidMealItemValue = this.hidMealItemValue.Value,
            //hidTransItemValue = this.hidTransItemValue.Value,
            //hidSampleItemValue = this.hidSampleItemValue.Value,
            //hidOthersItemValue = this.hidOthersItemValue.Value;

            //        DataItem dataItem = oSerializer.Deserialize<DataItem>(this.hidHotelItemValue.Value);

            TravelExpenseClaimCommon.BatchUpdateItems("Travel Expense Claim Details", workflowNumber, this.rptHotel);
            TravelExpenseClaimCommon.BatchUpdateItems("Travel Expense Claim Details", workflowNumber, this.rptMeal);

        }



        internal void SaveCommonListData()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            fields[this.ffTotalCost.FieldName] = this.ffTotalCost.Value;
            fields[this.ffCashAdvanced.FieldName] = this.ffCashAdvanced.Value;
            fields[this.ffNetPayable.FieldName] = this.ffNetPayable.Value;
            fields[this.ffComparedToApproved.FieldName] = this.ffComparedToApproved.Value;
            fields[this.ffPaidByCreditCard.FieldName] = this.ffPaidByCreditCard.Value;
            fields[this.ffTotalExceptFlight.FieldName] = this.ffTotalExceptFlight.Value;
        }

        internal void SavePendingForm()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (this.rblFapiao.SelectedItem != null)
            {
                if (this.rblFapiao.SelectedValue == "other reasons, please state")
                {
                    fields["FapiaoReason"] = this.txtFapiaoOtherReason.Text;
                }
                else
                {
                    fields["FapiaoReason"] = this.rblFapiao.SelectedValue;
                }
            }
            else
            {
                fields["FapiaoReason"] = "-1";
            }

            if (this.rblInformation.SelectedItem != null)
            {
                if (this.rblInformation.SelectedValue == "other reasons, please state")
                {
                    fields["InformationReason"] = this.txtInformationOtherReason.Text;
                }
                else
                {
                    fields["InformationReason"] = this.rblInformation.SelectedValue;
                }
            }
            else
            {
                fields["InformationReason"] = "-1";
            }

            if (this.rblClaimedAmt.SelectedItem != null)
            {
                if (this.rblClaimedAmt.SelectedValue == "other reasons, please state")
                {
                    fields["ClaimedAmtReason"] = this.txtClaimedOtherReason.Text;
                }
                else
                {
                    fields["ClaimedAmtReason"] = this.rblClaimedAmt.SelectedValue;
                }
            }
            else
            {
                fields["ClaimedAmtReason"] = "-1";
            }

            fields["OtherReasons"] = this.txtOtherReasons.Text;

        }


        internal string ValidatePendingForm()
        {
            string errorMessage = "";
            if (this.rblFapiao.SelectedIndex == -1
                && this.rblClaimedAmt.SelectedIndex == -1
                && this.rblInformation.SelectedIndex == -1
                && this.txtOtherReasons.Text.IsNullOrWhitespace())
            {
                errorMessage = "Please fill in or select pending reasons!";
            }
            return errorMessage;

        }
    }

    public class DataItem
    {
        public List<string> ItemID { get; set; }
    }
}