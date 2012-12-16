using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class DataView : BaseWorkflowUserControl
    {
        private string requestId;
        public string RequestId { set { this.requestId = value; } }
        private string step;
        private string pending;

        public string Pending
        {
            get { return pending; }
            set { pending = value; }
        }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadMonthData();
                System.Data.DataTable dt = WorkFlowUtil.GetCollection(requestId, "Credit Card Claim Detail").GetDataTable();
                rptTradeInfo.DataSource = dt;
                rptTradeInfo.DataBind();
                bool isConfirmTask = WorkflowContext.Current.Step == "ConfirmTask" ? true : false;
                if (isConfirmTask)
                {
                    FillFiancePendingForm();
                }
                if (this.Step == "DisplayStep")
                {
                    FillFiancePendingForm();
                } 
            }
        }


        protected void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as System.Data.DataRowView;
                if (row != null)
                {
                    var cbClaim = (CheckBox)item.FindControl("cbClaim");
                    if (row["IsClaim"].ToString() == "1")
                    {
                        cbClaim.Checked = true;
                    }
                    else
                    {
                        cbClaim.Checked = false;
                    }

                    var cbTravelRequest = (CheckBox)item.FindControl("cbTravelRequest");
                    if (row["IsTravelRequest"].ToString() == "1")
                    {
                        cbTravelRequest.Checked = true;
                    }
                    else
                    {
                        cbTravelRequest.Checked = false;
                    }

                    var cbPersonal = (CheckBox)item.FindControl("cbPersonal");
                    if (row["IsPersonal"].ToString() == "1")
                    {
                        cbPersonal.Checked = true;
                    }
                    else
                    {
                        cbPersonal.Checked = false;
                    }
                }
            }
        }

        public void DataDataFields(WorkflowDataFields fields)
        {
            lblMonth.Text = fields["Month"].AsString();
            this.txtDepartment.Text = fields["Department"].AsString();
            this.txtRequestedBy.Text = fields["Applicant"].AsString();
            rbtAttachInvoice.SelectedValue = fields["InvoiceStatus"].AsString();

           // this.SummaryExpenseType = fields["SummaryExpenseType"].ToString();
        }
        //private void LoadMonthData()
        //{
        //    this.dplMonth.Items.Clear();
        //    int month = 1;
        //    while (month < 13)
        //    {
        //        ListItem li = new ListItem(DateTime.Now.Year.ToString() + "-" + month);
        //        if (DateTime.Now.Month == month)
        //        {
        //            li.Selected = true;
        //        }
        //        this.dplMonth.Items.Add(li);
        //        month++;
        //    }
        //}

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
    }
}