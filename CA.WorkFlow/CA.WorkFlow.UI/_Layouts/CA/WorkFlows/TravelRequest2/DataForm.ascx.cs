namespace CA.WorkFlow.UI.TravelRequest2
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
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public partial class DataForm : TravelRequest2Control
    {
        private decimal Total;

        private string requestId;

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        private string mode;

        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }


        public Employee Applicant
        {
            get
            {
                return (this.ViewState["Applicant"] as Employee);
            }
            set
            {
                this.ViewState["Applicant"] = value;
            }
        }

        private string userAccount;
        public string UserAccount
        {
            get
            {
                return userAccount;
            }
            set
            {
                this.userAccount = value;
            }
        }

        private string totalBudget;
        public string TotalBudget
        {
            set
            {
                totalBudget = value;
            }
        }

        public string ChineseName
        {
            get { return ffChineseName.Value.AsString(); }
        }


        public string EnglishName
        {
            get { return ffEnglishName.Value.AsString(); }
        }


        public string Department
        {
            get { return lblDepartment.Text; }
        }



        public string OtherPurpose
        {
            get { return txtOtherPurpose.Text; }
            set
            {
                txtOtherPurpose.Text = value;
            }
        }



        public bool ChosenFlight
        {
            get { return cbChosenFlight.Checked; }
            set
            {
                cbChosenFlight.Checked = value;
            }
        }


        public bool NextFlight
        {
            get { return cbNextFlight.Checked; }
            set
            {
                cbNextFlight.Checked = value;
            }
        }

        public bool IsBookHotel
        {
            get { return cbIsBookHotel.Checked; }
            set
            {
                cbIsBookHotel.Checked = value;
            }
        }

        internal DataTable VehicleTable
        {
            get
            {
                return (this.ViewState["VehicleTable"] as DataTable) ?? CreateVehicleTable();
            }
            set
            {
                this.ViewState["VehicleTable"] = value;
            }
        }

        internal DataTable TravelTable
        {
            get
            {
                return (this.ViewState["TravelTable"] as DataTable) ?? CreateTravelTable();
            }
            set
            {
                this.ViewState["TravelTable"] = value;
            }
        }

        internal DataTable HotelTable
        {
            get
            {
                return (this.ViewState["HotelTable"] as DataTable) ?? CreateHotelTable();
            }
            set
            {
                this.ViewState["HotelTable"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.mode == "Edit")
                {
                    this.cpfUser.CommaSeparatedAccounts = this.UserAccount;
                    this.lblTotalBudget.Text = this.totalBudget;

                    DataTable travelDetails2 = GetDataTable(requestId, "Travel Details2"),
                        travelVehicleInfo2 = GetDataTable(requestId, "Travel Vehicle Info2"),
                        travelHotelInfo2 = GetDataTable(requestId, "Travel Hotel Info2");

                    if (travelDetails2 == null)
                    {
                        this.TravelTable.Rows.Clear();
                    }
                    if (travelVehicleInfo2 == null)
                    {
                        this.VehicleTable.Rows.Clear();
                    }
                    if (travelHotelInfo2 == null)
                    {
                        this.HotelTable.Rows.Clear();
                    }

                    this.rptTravel.DataSource = travelDetails2;
                    this.rptTravel.DataBind();

                    this.rptVehicle.DataSource = travelVehicleInfo2;
                    this.rptVehicle.DataBind();

                    this.rptHotel.DataSource = travelHotelInfo2;
                    this.rptHotel.DataBind();

                    DataBindCostCenter();
                }
                else
                {
                    this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();

                    this.rptVehicle.DataSource = this.VehicleTable;
                    this.rptVehicle.DataBind();

                    this.rptTravel.DataSource = this.TravelTable;
                    this.rptTravel.DataBind();

                    this.rptHotel.DataSource = this.HotelTable;
                    this.rptHotel.DataBind();

                    DataBindCostCenter();

                }
            }
            this.cpfUser.Load += new EventHandler(cpfUser_Load);

        }

        protected void DataBindCostCenter()
        {
            DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();

            if (this.rptTravel.Items.Count > 0)
            {
                DropDownList ddlCostCenter = this.rptTravel.Items[0].FindControl("ddlCostCenter") as DropDownList;
                ddlCostCenter.DataSource = dtCostCenter;
                ddlCostCenter.DataTextField = "Display";
                ddlCostCenter.DataValueField = "Title";
                ddlCostCenter.DataBind();
            }
        }

        protected void btnAddHotel_Click(object sender, ImageClickEventArgs e)
        {
            if (rptHotel.Items.Count > 0)
            {
                UpdateHotel();
            }
            //this.HotelTable.Rows.Add();
            FillDefaultHotelTable();

            this.rptHotel.DataSource = this.HotelTable;
            this.rptHotel.DataBind();
        }

        protected void btnAddVehicle_Click(object sender, ImageClickEventArgs e)
        {
            if (rptVehicle.Items.Count > 0)
            {
                UpdateVehicle();
            }
            //this.VehicleTable.Rows.Add();
            FillDefaultVehicleTable();

            this.VehicleTable.AcceptChanges();
            this.rptVehicle.DataSource = this.VehicleTable;
            this.rptVehicle.DataBind();
        }

        protected void btnAddTravel_Click(object sender, ImageClickEventArgs e)
        {
            if (rptTravel.Items.Count > 0)
            {
                UpdateTravel();
            }

            //this.TravelTable.Rows.Add();
            FillDefaultTravelTable();

            this.rptTravel.DataSource = this.TravelTable;
            this.rptTravel.DataBind();
        }

        protected void rptVehicle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;

                if (row == null)
                {
                    return;
                }

                var caDateTime = (CADateTimeControl)item.FindControl("CADateTimeVehicleDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["Date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Date"].ToString());
                var txtBox = (TextBox)item.FindControl("txtTime");
                txtBox.Text = row["Time"].ToString();
                txtBox = (TextBox)item.FindControl("txtVehicleNum");
                txtBox.Text = row["VehicleNumber"].ToString();
                txtBox = (TextBox)item.FindControl("txtFrom");
                txtBox.Text = row["VehicleFrom"].ToString();
                txtBox = (TextBox)item.FindControl("txtTo");
                txtBox.Text = row["VehicleTo"].ToString();

                DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");
                ddlVehicle.SelectedValue = row["VehicleCostItem"].ToString();
            }
        }

        protected void rptVehicle_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                this.UpdateVehicle();

                this.VehicleTable.Rows.RemoveAt(e.Item.ItemIndex);
                this.rptVehicle.DataSource = this.VehicleTable;
                this.rptVehicle.DataBind();
            }
        }

        protected void rptTravel_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateTravel();
                TravelTable.Rows.Remove(TravelTable.Rows[e.Item.ItemIndex]);
                this.rptTravel.DataSource = TravelTable;
                this.rptTravel.DataBind();
            }
        }

        protected void rptTravel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;

            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;

                if (row == null)
                {
                    return;
                }

                var caDateTime = (CADateTimeControl)item.FindControl("dtTravelDateFrom");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["TravelDateFrom"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["TravelDateFrom"].ToString());
                caDateTime = (CADateTimeControl)item.FindControl("dtTravelDateTo");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["TravelDateTo"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["TravelDateTo"].ToString());

                TextBox txtBox = (TextBox)item.FindControl("txtTravelFrom");
                txtBox.Text = row["TravelLocationFrom"].ToString();
                txtBox = (TextBox)item.FindControl("txtTravelTo");
                txtBox.Text = row["TravelLocationTo"].ToString();
                txtBox = (TextBox)item.FindControl("txtOthersCostItem");
                txtBox.Text = row["OthersCostItem"].ToString();
                txtBox = (TextBox)item.FindControl("txtVehicleCost");
                txtBox.Text = row["VehicleEstimatedCost"].ToString();
                txtBox = (TextBox)item.FindControl("txtHotelCost");
                txtBox.Text = row["HotelEstimatedCost"].ToString();
                txtBox = (TextBox)item.FindControl("txtMealCost");
                txtBox.Text = row["MealEstimatedCost"].ToString();
                txtBox = (TextBox)item.FindControl("txtTransportationCost");
                txtBox.Text = row["LocalTransportationEstimatedCost"].ToString();
                txtBox = (TextBox)item.FindControl("txtSample");
                txtBox.Text = row["SamplePurchaseCost"].ToString();
                txtBox = (TextBox)item.FindControl("txtOtherCost");
                txtBox.Text = row["OtherEstimatedCost"].ToString();

                Label lbl = (Label)item.FindControl("lblTotalCost");
                lbl.Text = row["TotalEstimatedCost"].ToString();

                DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
                ddlCostCenter.DataSource = dtCostCenter;
                ddlCostCenter.DataTextField = "Display";
                ddlCostCenter.DataValueField = "Title";
                ddlCostCenter.DataBind();

                if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
                {
                    ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
                }
                DropDownList ddlArea = (DropDownList)item.FindControl("ddlArea");
                ddlArea.SelectedValue = row["Area"].ToString();
                DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");
                ddlVehicle.SelectedValue = row["VehicleCostItem"].ToString();

                CheckBox cbReturnVehicle = (CheckBox)item.FindControl("returnVehicle");
                //cbReturnVehicle.Checked =string.IsNullOrEmpty(row["ReturnVehicleCostItem"].ToString()) ? false : row["ReturnVehicleCostItem"];
                cbReturnVehicle.Checked = row["ReturnVehicleCostItem"].AsString().IsNotNullOrWhitespace() &&
                    (row["ReturnVehicleCostItem"].AsString() == "1" || row["ReturnVehicleCostItem"].AsString().Equals("true", StringComparison.CurrentCultureIgnoreCase)) ? true : false;

                HiddenField hidCost = (HiddenField)item.FindControl("hidCost");
                hidCost.Value = row["hidCost"].AsString();
            }
        }

        protected void rptHotel_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateHotel();
                HotelTable.Rows.Remove(HotelTable.Rows[e.Item.ItemIndex]);
                this.rptHotel.DataSource = HotelTable;
                this.rptHotel.DataBind();
            }
        }

        protected void rptHotel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var caDateTime = (CADateTimeControl)item.FindControl("dtCheckInDate");
                    caDateTime.SelectedDate = string.IsNullOrEmpty(row["CheckInDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["CheckInDate"].ToString());
                    caDateTime = (CADateTimeControl)item.FindControl("dtCheckOutDate");
                    caDateTime.SelectedDate = string.IsNullOrEmpty(row["CheckOutDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["CheckOutDate"].ToString());

                    TextBox txtBox = (TextBox)item.FindControl("txtCity");
                    txtBox.Text = row["City"].ToString();
                    txtBox = (TextBox)item.FindControl("txtHotelName");
                    txtBox.Text = row["HotelName"].ToString();
                    txtBox = (TextBox)item.FindControl("txtTotalNights");
                    txtBox.Text = row["TotalNights"].ToString();
                }
            }
        }

        private void UpdateHotel()
        {
            this.HotelTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptHotel.Items)
            {
                var txtCity = (TextBox)item.FindControl("txtCity");
                var dtCheckInDate = (CADateTimeControl)item.FindControl("dtCheckInDate");
                var dtCheckOutDate = (CADateTimeControl)item.FindControl("dtCheckOutDate");
                var txtTotalNights = (TextBox)item.FindControl("txtTotalNights");
                var txtHotelName = (TextBox)item.FindControl("txtHotelName");

                DataRow row = this.HotelTable.Rows.Add();
                row["City"] = txtCity.Text;
                row["HotelName"] = txtHotelName.Text;
                row["CheckInDate"] = dtCheckInDate.IsDateEmpty ? string.Empty : dtCheckInDate.SelectedDate.ToShortDateString();
                row["CheckOutDate"] = dtCheckOutDate.IsDateEmpty ? string.Empty : dtCheckOutDate.SelectedDate.ToShortDateString();
                row["TotalNights"] = txtTotalNights.Text;
            }
        }

        private void UpdateVehicle()
        {
            this.VehicleTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptVehicle.Items)
            {
                var caVehicleDate = (CADateTimeControl)item.FindControl("CADateTimeVehicleDate");
                var txtTime = (TextBox)item.FindControl("txtTime");
                var txtVehicleNum = (TextBox)item.FindControl("txtVehicleNum");
                var txtFrom = (TextBox)item.FindControl("txtFrom");
                var txtTo = (TextBox)item.FindControl("txtTo");
                var ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");

                DataRow row = this.VehicleTable.Rows.Add();
                row["Date"] = caVehicleDate.IsDateEmpty ? string.Empty : caVehicleDate.SelectedDate.ToShortDateString();
                row["Time"] = txtTime.Text;
                row["VehicleNumber"] = txtVehicleNum.Text;
                row["VehicleFrom"] = txtFrom.Text;
                row["VehicleTo"] = txtTo.Text;
                row["VehicleCostItem"] = ddlVehicle.SelectedValue;
            }
        }

        private void UpdateTravel()
        {
            TravelTable.Rows.Clear();
            foreach (RepeaterItem item in rptTravel.Items)
            {
                CADateTimeControl travelDateFrom = (CADateTimeControl)item.FindControl("dtTravelDateFrom");
                CADateTimeControl travelDateTo = (CADateTimeControl)item.FindControl("dtTravelDateTo");
                TextBox txtDeparture = (TextBox)item.FindControl("txtTravelFrom");
                TextBox txtDestination = (TextBox)item.FindControl("txtTravelTo");
                DropDownList ddlArea = (DropDownList)item.FindControl("ddlArea");
                DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");
                CheckBox cbReturnVehicle = (CheckBox)item.FindControl("returnVehicle");
                TextBox txtOthersCostItem = (TextBox)item.FindControl("txtOthersCostItem");
                TextBox txtVehicleCost = (TextBox)item.FindControl("txtVehicleCost");
                TextBox txtHotelCost = (TextBox)item.FindControl("txtHotelCost");
                TextBox txtMealCost = (TextBox)item.FindControl("txtMealCost");
                TextBox txtTransportationCost = (TextBox)item.FindControl("txtTransportationCost");
                TextBox txtSampleCost = (TextBox)item.FindControl("txtSample");
                TextBox txtOtherCost = (TextBox)item.FindControl("txtOtherCost");
                HiddenField hidCost = (HiddenField)item.FindControl("hidCost");
                //Label txtTotalCost = (Label)item.FindControl("lblTotalCost");

                DataRow row = TravelTable.Rows.Add();
                row["TravelDateFrom"] = travelDateFrom.IsDateEmpty ? string.Empty : travelDateFrom.SelectedDate.ToShortDateString();
                row["TravelDateTo"] = travelDateTo.IsDateEmpty ? string.Empty : travelDateTo.SelectedDate.ToShortDateString();
                row["TravelLocationFrom"] = txtDeparture.Text;
                row["TravelLocationTo"] = txtDestination.Text;
                row["Area"] = ddlArea.SelectedValue;
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["VehicleCostItem"] = ddlVehicle.SelectedValue;
                row["ReturnVehicleCostItem"] = cbReturnVehicle.Checked;
                row["OthersCostItem"] = txtOthersCostItem.Text;
                row["VehicleEstimatedCost"] = txtVehicleCost.Text;
                row["HotelEstimatedCost"] = txtHotelCost.Text;
                row["MealEstimatedCost"] = txtMealCost.Text;
                row["LocalTransportationEstimatedCost"] = txtTransportationCost.Text;
                row["SamplePurchaseCost"] = txtSampleCost.Text;
                row["OtherEstimatedCost"] = txtOtherCost.Text;
                row["TotalEstimatedCost"] = float.Parse(txtVehicleCost.Text) + float.Parse(txtHotelCost.Text)
                    + float.Parse(txtMealCost.Text) + float.Parse(txtTransportationCost.Text) + float.Parse(txtSampleCost.Text) + float.Parse(txtOtherCost.Text);

                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                TravelCostItem travelCostItem = oSerializer.Deserialize<TravelCostItem>(hidCost.Value) ?? new TravelCostItem();

                travelCostItem.ID = item.ItemIndex.ToString();
                travelCostItem.DateFrom = row["TravelDateFrom"].AsString();
                travelCostItem.DateTo = row["TravelDateTo"].AsString();
                travelCostItem.LocationFrom = row["TravelLocationFrom"].AsString().Replace("'", "");
                travelCostItem.LocationTo = row["TravelLocationTo"].AsString().Replace("'", "");
                //travelCostItem.Flight = row["VehicleEstimatedCost"].AsString();

                row["HidCost"] = oSerializer.Serialize(travelCostItem);

            }
        }



        private DataTable CreateVehicleTable()
        {
            VehicleTable = new DataTable();
            VehicleTable.Columns.Add("RequestID");
            VehicleTable.Columns.Add("VehicleCostItem");
            VehicleTable.Columns.Add("Date");
            VehicleTable.Columns.Add("Time");
            VehicleTable.Columns.Add("VehicleNumber");
            VehicleTable.Columns.Add("VehicleFrom");
            VehicleTable.Columns.Add("VehicleTo");

            FillDefaultVehicleTable();

            return VehicleTable;
        }

        private void FillDefaultVehicleTable()
        {
            DataRow row = this.VehicleTable.Rows.Add();
            row["VehicleNumber"] = @"N/A";
        }



        private DataTable CreateTravelTable()
        {
            TravelTable = new DataTable();
            TravelTable.Columns.Add("RequestID");
            TravelTable.Columns.Add("TravelDateFrom");
            TravelTable.Columns.Add("TravelDateTo");
            TravelTable.Columns.Add("TravelLocationFrom");
            TravelTable.Columns.Add("TravelLocationTo");
            TravelTable.Columns.Add("Area");
            TravelTable.Columns.Add("VehicleCostItem");
            TravelTable.Columns.Add("ReturnVehicleCostItem");
            TravelTable.Columns.Add("OthersCostItem");
            TravelTable.Columns.Add("VehicleEstimatedCost");
            TravelTable.Columns.Add("HotelEstimatedCost");
            TravelTable.Columns.Add("MealEstimatedCost");
            TravelTable.Columns.Add("LocalTransportationEstimatedCost");
            TravelTable.Columns.Add("SamplePurchaseCost");
            TravelTable.Columns.Add("OtherEstimatedCost");
            TravelTable.Columns.Add("TotalEstimatedCost");
            TravelTable.Columns.Add("CostCenter");
            TravelTable.Columns.Add("HidCost");

            FillDefaultTravelTable();

            return TravelTable;
        }

        private void FillDefaultTravelTable()
        {
            DataRow row = this.TravelTable.Rows.Add();
            //row["OthersCostItem"] = @"N/A";
            row["VehicleEstimatedCost"] = 0;
            row["HotelEstimatedCost"] = 0;
            row["MealEstimatedCost"] = 0;
            row["LocalTransportationEstimatedCost"] = 0;
            row["SamplePurchaseCost"] = 0;
            row["OtherEstimatedCost"] = 0;
            row["TotalEstimatedCost"] = 0;

        }

        private DataTable CreateHotelTable()
        {
            HotelTable = new DataTable();
            HotelTable.Columns.Add("RequestID");
            HotelTable.Columns.Add("City");
            HotelTable.Columns.Add("HotelName");
            HotelTable.Columns.Add("CheckInDate");
            HotelTable.Columns.Add("CheckOutDate");
            HotelTable.Columns.Add("TotalNights");


            FillDefaultHotelTable();
            //HotelTable.Rows.Add();
            return HotelTable;
        }

        private void FillDefaultHotelTable()
        {
            DataRow row = this.HotelTable.Rows.Add();
            row["HotelName"] = @"N/A";
            row["TotalNights"] = 0;
        }

        /// <summary>
        /// Get the Travel budget total Estimated Cost.
        /// </summary>
        /// <returns>total Estimated Cost</returns>
        public decimal  GetTotal()
        {
            foreach (DataRow row in TravelTable.Rows)
            {

                Total += decimal.Parse(row["TotalEstimatedCost"].ToString());
            }
            return Total;
        }

        public void Update()
        {
            UpdateTravel();
            UpdateVehicle();
            UpdateHotel();
            if (this.cpfUser.Accounts.Count == 0)
            {
                this.Applicant = null;
            }
            else
            {
                this.cpfUser.Validate();
            }
        }

        void cpfUser_Load(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            FillEmployeeData(this.Applicant, false); //Dont overwrite the personal fields when loading if the fields have content
        }

        protected void btnPeopleInfo_Click(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            FillEmployeeData(this.Applicant, true); //Overwrite the all personal fields when clicking
        }

        //
        private void FillEmployeeData(Employee employee, bool isOverwrite)
        {
            if (isOverwrite)
            {
                this.ffChineseName.Value = this.Applicant.DisplayName;
                this.ffEnglishName.Value = this.Applicant.PreferredName;
                this.ffMobile.Value = this.Applicant.Mobile;
                this.ffOfficeExt.Value = this.Applicant.Phone;
            }
            else
            {
                if (this.ffChineseName.Value.AsString().IsNullOrWhitespace())
                {
                    this.ffChineseName.Value = this.Applicant.DisplayName;
                }
                if (this.ffEnglishName.Value.AsString().IsNullOrWhitespace())
                {
                    this.ffEnglishName.Value = this.Applicant.PreferredName;
                }
                if (this.ffMobile.Value.AsString().IsNullOrWhitespace())
                {
                    this.ffMobile.Value = this.Applicant.Mobile;
                }
                if (this.ffOfficeExt.Value.AsString().IsNullOrWhitespace())
                {
                    this.ffOfficeExt.Value = this.Applicant.Phone;
                }
            }
            
            this.lblDepartment.Text = this.Applicant.Department;
        }

        protected void btnReloadCostPolicy_Click(object sender, EventArgs e)
        {
            string mixedValues = this.hidMixedLocation.Value;
            if (mixedValues.IsNotNullOrWhitespace())
            {
                var hidCost = new StringBuilder();
                TravelCostItem travelCostItem = new TravelCostItem();
                travelCostItem.Flight = "0";
                var values = mixedValues.Split('|');
                char[] split = { ';', '#' };
                string totalMeal = "170";
                string hotelLimit = "400";
                string currency = "RMB";

                SPListItem item = values[7].Equals("China", StringComparison.CurrentCultureIgnoreCase) ? GetTravelPolicyByCity(values[1].ToLower()) : GetTravelPolicyByArea(values[7].ToLower());
                if (item != null)
                {
                    totalMeal = float.Parse(item["TotalMealAllowance"].AsString().Split(split)[2]).ToString("#0");
                    hotelLimit = item["HotelLimit"].AsString();
                    currency = item["Currency"].ToString();
                }
                hotelLimit = hotelLimit.IsNullOrWhitespace() ? "0" : hotelLimit;

                if (!currency.Equals("RMB", StringComparison.CurrentCultureIgnoreCase))
                {
                    var rateItem = ConvertToRMB(currency);
                    totalMeal = rateItem != null ? (Convert.ToDouble(rateItem["Rate"].AsString()) * Convert.ToDouble(totalMeal)).ToString() : totalMeal;
                }
                totalMeal = Math.Round(Convert.ToDouble(totalMeal), 0).ToString();

                //var days = values[7];

                foreach (RepeaterItem travelItem in rptTravel.Items)
                {
                    if (values[2].Substring(0, values[2].Length - 1) == travelItem.ClientID)
                    {
                        travelCostItem.ID = travelItem.ItemIndex.ToString();
                    }
                }
                travelCostItem.DateFrom = GetCADateTimeControlByClientId(values[2] + "dtTravelDateFrom", "dtTravelDateFrom").SelectedDate.ToShortDateString();
                travelCostItem.DateTo = GetCADateTimeControlByClientId(values[2] + "dtTravelDateTo", "dtTravelDateTo").SelectedDate.ToShortDateString();
                travelCostItem.LocationFrom = GetTextBoxByClientId(values[2] + "txtTravelFrom", "txtTravelFrom").Text.Replace("'","");
                travelCostItem.LocationTo = GetTextBoxByClientId(values[2] + "txtTravelTo", "txtTravelTo").Text.Replace("'","");
                if (this.rptTravel.Items.Count > 0)
                {
                    TextBox txtSourceLocation = this.rptTravel.Items[0].FindControl("txtTravelFrom") as TextBox;
                    if (txtSourceLocation.Text == travelCostItem.LocationTo)
                    {
                        hotelLimit = "0";
                        totalMeal = "0";
                    }
                }

                travelCostItem.HotelLimit = hotelLimit;
                travelCostItem.MealLimit = totalMeal;

                TextBox txtHotel = GetTextBoxByClientId(values[2] + values[3], values[3]);
                TextBox txtMeal = GetTextBoxByClientId(values[2] + values[4], values[4]);
                //txtHotel.Text = !IsNotNumberic(txtHotel.Text) && float.Parse(txtHotel.Text) > 0 ? txtHotel.Text : hotelLimit;
                //txtMeal.Text = !IsNotNumberic(txtMeal.Text) && float.Parse(txtMeal.Text) > 0 ? txtMeal.Text : totalMeal;
                txtHotel.Text = (float.Parse(hotelLimit) * float.Parse(values[9])).ToString();
                txtMeal.Text = (float.Parse(totalMeal) * float.Parse(values[8])).ToString();

                var price =values[7].Equals("China", StringComparison.CurrentCultureIgnoreCase) ? GetFlightPrice(values[0], values[1]):"0";
                //if (price != "0")
                //{
                    travelCostItem.Flight = Convert.ToBoolean(values[6]) ? Convert.ToString(float.Parse(price) * 2) : price;

                    if (GetDropDownListByClientId(values[2] + "ddlVehicle", "ddlVehicle").SelectedValue == "Flight")
                    {
                        GetTextBoxByClientId(values[2] + values[5], values[5]).Text = travelCostItem.Flight;
                    }
                //}
                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                GetHiddenFieldByClientId(values[2] + "hidCost", "hidCost").Value = oSerializer.Serialize(travelCostItem);

            }
        }

        //protected void btnReloadTravelPolicy_Click(object sender, EventArgs e)
        //{
        //    string mixedValues = this.hidMixedLocation.Value;
        //    if (mixedValues.IsNotNullOrWhitespace())
        //    {
        //        var values = mixedValues.Split('|');
        //        SPListItem item = GetTravelPolicy(values[1]);
        //        if (item != null)
        //        {
        //            var totalMeal = item["TotalMealAllowance"].AsString();
        //            char[] split = { ';', '#' };

        //            var days = values[7];
        //            GetTextBoxByClientId(values[2] + values[3], values[3]).Text = days.IsNotNullOrWhitespace() ? (Convert.ToInt32(item["HotelLimit"].AsString()) * Convert.ToInt32(days)).ToString() : item["HotelLimit"].AsString();
        //            GetTextBoxByClientId(values[2] + values[4], values[4]).Text = days.IsNotNullOrWhitespace() ? (float.Parse(totalMeal.Split(split)[2]) * (Convert.ToInt32(days) + 1)).ToString("#0") : float.Parse(totalMeal.Split(split)[2]).ToString("#0");
        //            //GetTextBoxByClientId(values[2] + values[3], values[3]).Text = item["HotelLimit"].AsString();
        //            //GetTextBoxByClientId(values[2] + values[4], values[4]).Text = float.Parse(totalMeal.Split(split)[2]).ToString("#0");
        //        }
        //    }
        //}

        protected void btnReloadFlightPrice_Click(object sender, EventArgs e)
        {
            string mixedValues = this.hidMixedLocation.Value;
            if (mixedValues.IsNotNullOrWhitespace())
            {
                var values = mixedValues.Split('|');
                var price = GetFlightPrice(values[0], values[1]);
                if (price.IsNotNullOrWhitespace())
                {
                    GetTextBoxByClientId(values[2] + values[5], values[5]).Text = Convert.ToBoolean(values[6]) ? Convert.ToString(float.Parse(price) * 2) : price;
                }
            }
        }

        protected void btnClearHotelInfo_Click(object sender, EventArgs e)
        {
            this.HotelTable.Rows.Clear();
            this.rptHotel.DataBind();
        }

        private DropDownList GetDropDownListByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptTravel.Items select row.FindControl(controlId) as DropDownList).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private CADateTimeControl GetCADateTimeControlByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptTravel.Items select row.FindControl(controlId) as CADateTimeControl).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private HiddenField GetHiddenFieldByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptTravel.Items select row.FindControl(controlId) as HiddenField).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private TextBox GetTextBoxByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptTravel.Items select row.FindControl(controlId) as TextBox).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private CheckBox GetCheckBoxByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptTravel.Items select row.FindControl(controlId) as CheckBox).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private string validateGeneralInfo()
        {
            string status = string.Empty;

            if (string.IsNullOrEmpty(ffIDNumber.Value.ToString()))
                status += "Please supply a ID number.\\n";
            if (string.IsNullOrEmpty(ffMobile.Value.ToString()))
                status += "Please supply a mobile number.\\n";
            if (string.IsNullOrEmpty(ffOfficeExt.Value.ToString()))
                status += "Please supply an office Ext.No.\\n";
            //if (string.IsNullOrEmpty(ffNote.Value.ToString()))
            //    status += "Please supply a Remark.\\n";
            //if (string.IsNullOrEmpty(ffHotelRemark.Value.ToString()))
            //    status += "Please supply a hotel remark Ext.No.\\n";

            //if (cbChosenFlight.Checked == false && cbNextFlight.Checked == false)
            //{
            //    status += "Please choose Business class or Others.\\n";
            //}

            return status;
        }

        private string validateTravelDetails()
        {
            string status = string.Empty;

            //float tmp;

            if (rptTravel.Items.Count == 0)
            {
                status = "Please supply valid travel details.\\n";
            }

            for (int i = 0; i < rptTravel.Items.Count; i++)
            {
                RepeaterItem item = rptTravel.Items[i];

                CADateTimeControl travelDateFrom = (CADateTimeControl)item.FindControl("dtTravelDateFrom");
                CADateTimeControl travelDateTo = (CADateTimeControl)item.FindControl("dtTravelDateTo");
                TextBox txtDeparture = (TextBox)item.FindControl("txtTravelFrom");
                TextBox txtDestination = (TextBox)item.FindControl("txtTravelTo");
                DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");
                TextBox txtOthersCostItem = (TextBox)item.FindControl("txtOthersCostItem");
                TextBox txtVehicleCost = (TextBox)item.FindControl("txtVehicleCost");
                TextBox txtHotelCost = (TextBox)item.FindControl("txtHotelCost");
                TextBox txtMealCost = (TextBox)item.FindControl("txtMealCost");
                TextBox txtTransportationCost = (TextBox)item.FindControl("txtTransportationCost");
                TextBox txtSampleCost = (TextBox)item.FindControl("txtSample");
                TextBox txtOtherCost = (TextBox)item.FindControl("txtOtherCost");

                if ((travelDateFrom.IsDateEmpty)
                      || (travelDateTo.IsDateEmpty)
                      || (string.IsNullOrEmpty(txtDeparture.Text))
                      || (string.IsNullOrEmpty(txtDestination.Text))
                    //|| (string.IsNullOrEmpty(txtOthersCostItem.Text))
                    || (IsNotNumberic(txtVehicleCost.Text))
                    || (IsNotNumberic(txtHotelCost.Text))
                    || (IsNotNumberic(txtMealCost.Text))
                    || (IsNotNumberic(txtTransportationCost.Text))
                    || (IsNotNumberic(txtSampleCost.Text))
                    || (IsNotNumberic(txtOtherCost.Text))
                    )
                {
                    status = "Please supply valid travel details.\\n";
                    break;
                }

                //validate vehicle cost that must be more than 0 
                //if (float.TryParse(txtVehicleCost.Text, out tmp))
                //{
                //    if (tmp <= 0)
                //    {
                //        status += "Please supply the Vehicle Cost which is more than 0";
                //        break;
                //    }
                //}
                //if (float.TryParse(txtMealCost.Text, out tmp))
                //{
                //    if (tmp <= 0)
                //    {
                //        status += "Please supply the Meal Cost which is more than 0";
                //        break;
                //    }
                //}

            }
            return status;
        }

        private string validateHotelInfo()
        {
            string status = string.Empty;

            if (!this.cbIsBookHotel.Checked)
            {

                if (rptHotel.Items.Count == 0)
                {
                    status = "Please supply valid hotel info.\\n";
                }

                for (int i = 0; i < rptHotel.Items.Count; i++)
                {
                    RepeaterItem item = rptHotel.Items[i];

                    var txtCity = (TextBox)item.FindControl("txtCity");
                    var dtCheckInDate = (CADateTimeControl)item.FindControl("dtCheckInDate");
                    var dtCheckOutDate = (CADateTimeControl)item.FindControl("dtCheckOutDate");
                    var txtTotalNights = (TextBox)item.FindControl("txtTotalNights");

                    if ((dtCheckInDate.IsDateEmpty)
                       || (dtCheckOutDate.IsDateEmpty)
                       || (string.IsNullOrEmpty(txtCity.Text))
                        || (IsNotNumberic(txtTotalNights.Text))
                        )
                    {
                        status = "Please supply valid hotel info.\\n";
                        break;
                    }

                }
            }
            return status;
        }

        private string validateVehicleInfo()
        {
            string status = string.Empty;

            if (rptVehicle.Items.Count == 0)
            {
                status = "Please supply valid travel details.\\n";
            }

            for (int i = 0; i < rptVehicle.Items.Count; i++)
            {
                RepeaterItem item = rptVehicle.Items[i];

                var caVehicleDate = (CADateTimeControl)item.FindControl("CADateTimeVehicleDate");
                var txtTime = (TextBox)item.FindControl("txtTime");
                var txtVehicleNum = (TextBox)item.FindControl("txtVehicleNum");
                var txtFrom = (TextBox)item.FindControl("txtFrom");
                var txtTo = (TextBox)item.FindControl("txtTo");
                if ((caVehicleDate.IsDateEmpty)
                        || (string.IsNullOrEmpty(txtTime.Text))
                        || (string.IsNullOrEmpty(txtVehicleNum.Text))
                        || (string.IsNullOrEmpty(txtFrom.Text))
                        || (string.IsNullOrEmpty(txtTo.Text))
                    )
                {
                    status = "Please supply valid travel details.\\n";
                    break;
                }
            }


            return status;
        }

        private string validateApprover()
        {
            Employee approver = WorkFlowUtil.GetEmployeeApprover(Applicant);
            if (approver == null)
            {
                return "Unable to find an approver for the applicant. Please contact IT for further help.";
            }
            else
            {
                return string.Empty;
            }
        }
        private string validateApplicant()
        {
            if (this.cpfUser.Accounts.Count == 0)
            {
                return "Please fill in the applicant field.";
            }
            //Employee emp = UserProfileUtil.GetEmployeeEx(this.cpfUser.Accounts[0].ToString());
            //if (emp == null || emp.Department.AsString().IsNullOrWhitespace())
            //{
            //    return "The department manager is not set in the system.";
            //}
            //else
            //{
            //    return string.Empty;
            //}
            return string.Empty;
        }
        private bool IsNotNumberic(string oText)
        {
            if (oText == "NaN") {
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
        public string ValidateForSave()
        {
            string statusTravel = string.Empty;
            string statusHotel = string.Empty;

            for (int i = 0; i < rptTravel.Items.Count; i++)
            {
                RepeaterItem item = rptTravel.Items[i];

                TextBox txtVehicleCost = (TextBox)item.FindControl("txtVehicleCost");
                TextBox txtHotelCost = (TextBox)item.FindControl("txtHotelCost");
                TextBox txtMealCost = (TextBox)item.FindControl("txtMealCost");
                TextBox txtTransportationCost = (TextBox)item.FindControl("txtTransportationCost");
                TextBox txtSample = (TextBox)item.FindControl("txtSample");
                TextBox txtOtherCost = (TextBox)item.FindControl("txtOtherCost");

                if ((IsNotNumberic(txtVehicleCost.Text))
                    || (IsNotNumberic(txtHotelCost.Text))
                    || (IsNotNumberic(txtMealCost.Text))
                    || (IsNotNumberic(txtTransportationCost.Text))
                    || (IsNotNumberic(txtSample.Text))
                    || (IsNotNumberic(txtOtherCost.Text))
                    )
                {
                    statusTravel = "Please supply valid travel details.\\n";
                    break;
                }
            }
            for (int i = 0; i < rptHotel.Items.Count; i++)
            {
                RepeaterItem item = rptHotel.Items[i];

                var txtTotalNights = (TextBox)item.FindControl("txtTotalNights");

                if ((IsNotNumberic(txtTotalNights.Text)))
                {
                    statusHotel = "Please supply valid hotel No.of nights info.\\n";
                    break;
                }

            }

            return statusTravel + statusHotel;
        }

        public string ValidateForSubmit()
        {
            string status = string.Empty;

            status += validateGeneralInfo();
            status += validateTravelDetails();
            status += validateVehicleInfo();
            status += validateHotelInfo();
            //status += validateApprover();
            status += validateApplicant();

            return status;
        }


    }
    public class TravelCostItem
    {
        public string ID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string HotelLimit { get; set; }
        public string MealLimit { get; set; }
        public string Flight { get; set; }
        public string HotelValue { get; set; }
        public string MealValue { get; set; }
    }
}
