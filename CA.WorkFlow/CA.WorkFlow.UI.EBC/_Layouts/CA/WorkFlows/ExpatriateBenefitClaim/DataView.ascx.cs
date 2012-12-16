using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using QuickFlow.Core;
using CA.SharePoint;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.EBC
{
    public partial class DataView : BaseWorkflowUserControl
    {
        #region Fields

        public string SummaryExpenseType
        {
            set { this.hidSummaryExpenseType.Value = value; }
        }

        public string ExpatriateBenefitForm
        {
            set { this.hidExpatriateBenefitForm.Value = value; }
        }

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (WorkflowContext.Current.Step == "ConfirmTask" || this.Step == "DisplayStep")
                {
                    FillFiancePendingForm();
                }
            }
        }

        private bool IsRadioListItem(RadioButtonList radioButtonList, string fieldValue)
        {
            return radioButtonList.Items.Contains(new ListItem(fieldValue, fieldValue));
        }

        private void FillFiancePendingForm()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
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