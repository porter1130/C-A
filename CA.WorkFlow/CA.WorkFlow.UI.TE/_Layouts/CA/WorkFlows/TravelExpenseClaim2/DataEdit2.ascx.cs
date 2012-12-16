namespace CA.WorkFlow.UI.TE
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CA.SharePoint.Utilities.Common;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Globalization;
    using System.Text;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public partial class DataEdit2 : BaseWorkflowUserControl
    {
        #region Field

        private string requestId;

        public string RequestId
        {
            set { requestId = value; }
        }

        private string trWorkflowNumber;

        public string TrWorkflowNumber
        {
            set { trWorkflowNumber = value; }
            get { return trWorkflowNumber; }
        }

        private string mode;

        public string Mode
        {
            set { mode = value; }
        }

        public string HotelForm
        {
            set { this.hfHotelForm.Value = value; }
            get { return this.hfHotelForm.Value; }
        }

        public string MealAllowanceForm
        {
            set { this.hfMealAllowanceForm.Value = value; }
            get { return this.hfMealAllowanceForm.Value; }
        }

        public string TransportationForm
        {
            set { this.hfTransportationForm.Value = value; }
            get { return this.hfTransportationForm.Value; }
        }

        public string SamplePurchaseForm
        {
            set { this.hfSamplePurchaseForm.Value = value; }
            get { return this.hfSamplePurchaseForm.Value; }
        }

        public string OtherForm
        {
            set { this.hfOtherForm.Value = value; }
            get { return this.hfOtherForm.Value; }
        }
        //

        public string Purpose
        {
            set { this.txtTravelPurpose.Value = value; }
            get { return this.txtTravelPurpose.Value; }
        }

        public string TotalCost
        {
            set { this.txtTotalCost.Value = value; }
            get { return this.txtTotalCost.Value; }
        }

        public string CashAdvanced
        {
            set { this.txtCashAdvanced.Value = value; }
            get { return this.txtCashAdvanced.Value; }
        }

        public string PaidByCreditCard
        {
            set { this.txtPaidByCreditCard.Value = value; }
            get { return this.txtPaidByCreditCard.Value; }
        }

        public string NetPayable
        {
            set { this.txtNetPayable.Value = value; }
            get { return this.txtNetPayable.Value; }
        }

        public string TotalExceptFlight
        {
            set { this.txtTotalExceptFlight.Value = value; }
            get { return this.txtTotalExceptFlight.Value; }
        }

        public string ComparedToApproved
        {
            set { this.txtComparedToApproved.Value = value; }
            get { return this.txtComparedToApproved.Value; }
        }

        public string Reasons
        {
            set { this.txtReasons.Value = value; }
            get { return this.txtReasons.Value; }
        }

        public string SupportingSubmitted
        {
            set { this.txtSupportingSubmitted.Value = value; }
            get { return this.txtSupportingSubmitted.Value; }
        }

        public string SubmissionDate
        {
            set { this.txtSubmissionDate.Value = value; }
            get { return this.txtSubmissionDate.Value; }
        }

        public string FinanceRemark
        {
            set { this.txtFinanceRemark.Value = value; }
            get { return this.txtFinanceRemark.Value; }
        }

        public string HotelSubTotal
        {
            get { return this.hfHotelSubTotal.Value; }
        }

        public string MealSubTotal
        {
            get { return this.hfMealSubTotal.Value; }
        }

        public string TransSubTotal
        {
            get { return this.hfTransSubTotal.Value; }
        }

        public string SampleSubTotal
        {
            get { return this.hfSampleSubTotal.Value; }
        }

        public string OthersSubTotal
        {
            get { return this.hfOthersSubTotal.Value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.mode == "New") 
                {
                    LoadSourceData(requestId);
                }
                if (this.mode == "Edit")
                {
                    LoadSourceData(trWorkflowNumber);
                }
                LoadCostCenterAndExchangeRate();
            }
        }

        private void LoadCostCenterAndExchangeRate()
        {
            StringBuilder strCostCenter = new StringBuilder();
            strCostCenter.Append("[");
            DataTable dtCostCenter = WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
            if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCostCenter.Rows)
                {
                    strCostCenter.Append("{");
                    strCostCenter.AppendFormat("name:'{0}',val:'{1}'", dr["Display"].ToString(), dr["Title"].ToString());
                    strCostCenter.Append("},");
                }
            }
            strCostCenter.Append("]");
            hfCostCenter.Value = strCostCenter.ToString();

            StringBuilder strExchangeRate = new StringBuilder();
            strExchangeRate.Append("[");
            DataTable dtExchangeRate = WorkFlowUtil.GetCollectionByList("ExchangeRates").GetDataTable();
            if (dtExchangeRate != null && dtExchangeRate.Rows.Count > 0)
            {
                foreach (DataRow dr in dtExchangeRate.Rows)
                {
                    strExchangeRate.Append("{");
                    strExchangeRate.AppendFormat("name:'{0}',val:'{1}'", dr["From"].ToString(), dr["Rate"].ToString());
                    strExchangeRate.Append("},");
                }
            }
            strExchangeRate.Append("]");
            hfExchangeRate.Value = strExchangeRate.ToString();

            StringBuilder strTravelPolicy = new StringBuilder();
            strTravelPolicy.Append("[");
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Travel Policy");
            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='Country' />" +
                               "<FieldRef Name='Currency' />" +
                               "<FieldRef Name='Location' />" +
                               "<FieldRef Name='HotelLimit' />" +
                               "<FieldRef Name='BreakfastLimit' />" +
                               "<FieldRef Name='LunchLimit' />" +
                               "<FieldRef Name='DinnerLimit' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();
                if (data != null && data.Rows.Count > 0)
                {
                    foreach (DataRow dr in data.Rows)
                    {
                        strTravelPolicy.Append("{");
                        strTravelPolicy.AppendFormat("Country:'{0}',Currency:'{1}',Location:'{2}',HotelLimit:'{3}',BreakfastLimit:'{4}',LunchLimit:'{5}',DinnerLimit:'{6}'"
                                                                , dr["Country"].ToString()
                                                                , dr["Currency"].ToString()
                                                                , dr["Location"].ToString()
                                                                , dr["HotelLimit"].AsString()
                                                                , dr["BreakfastLimit"].ToString()
                                                                , dr["LunchLimit"].ToString()
                                                                , dr["DinnerLimit"].ToString());
                        strTravelPolicy.Append("},");
                    }
                }
                strTravelPolicy.Append("]");
                hfTravelPolicy.Value = strTravelPolicy.ToString();
            }
        }

        private void LoadSourceData(string id)
        {
            if (id.IsNotNullOrWhitespace())
            {
                SPListItemCollection travelDetailItems = TravelExpenseClaimCommon.GetDataCollection(id, "Travel Details2");
                SPListItemCollection travelRequestItems = TravelExpenseClaimCommon.GetDataCollection(id, "Travel Request Workflow2");
                DataTable dt = travelDetailItems.GetDataTable();
                SPListItem tri = travelRequestItems[0];
                this.txtApplicant.Value = tri["Applicant"].AsString();
                this.txtChineseName.Value = tri["ChineseName"].AsString();
                this.txtEnglishName.Value = tri["EnglishName"].AsString();
                this.txtIDPassportNo.Value = tri["IDNumber"].AsString();
                this.txtDepartment.Value = tri["Department"].AsString();
                this.txtMobile.Value = tri["Mobile"].AsString();
                this.txtOffice.Value = tri["OfficeExt"].AsString();
                if (this.mode == "New")
                {
                    this.txtCashAdvanced.Value = tri["CashAdvanced"].AsString() == "" ? "0" : tri["CashAdvanced"].AsString();
                    this.txtTravelPurpose.Value = tri["TravelOtherPurpose"].AsString() == "" ? tri["TravelPurpose"].AsString() : tri["TravelOtherPurpose"].AsString();
                    double travelTotalBudget = tri["TravelTotalCost"].AsString() == "" ? 0 : double.Parse(tri["TravelTotalCost"].AsString());
                    double vehicleCost = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        vehicleCost += row["VehicleEstimatedCost"].AsString() == "" ? 0 : double.Parse(row["VehicleEstimatedCost"].AsString());
                    }
                    this.txtTotalExceptFlight.Value = (travelTotalBudget - vehicleCost).ToString();
                }
            }
        }

    }
}