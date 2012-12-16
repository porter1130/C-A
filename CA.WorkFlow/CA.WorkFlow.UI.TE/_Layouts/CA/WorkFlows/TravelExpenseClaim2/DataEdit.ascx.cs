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

    public partial class DataEdit : BaseWorkflowUserControl
    {
        private string trWorkflowNumber;

        public string TrWorkflowNumber
        {
            set { trWorkflowNumber = value; }
            get { return trWorkflowNumber; }
        }

        private string requestId;

        public string RequestId
        {
            set { requestId = value; }
        }

        private string userAccount;

        public string UserAccount
        {
            get { return userAccount; }
            set { userAccount = value; }
        }

        private string mode;

        public string Mode
        {
            set { mode = value; }
        }

        public string EnglishName
        {
            get { return this.lblEnglishName.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.mode == "Edit")
                {
                    DataTable dt = TravelExpenseClaimCommon.GetDataTable(requestId, "Travel Expense Claim Details");
                    if (dt != null)
                    {
                        DataTable dtHotel = dt.Clone(),
                                    dtMeal = dt.Clone(),
                                    dtTrans = dt.Clone(),
                                    dtSample = dt.Clone(),
                                    dtOthers = dt.Clone();

                        dtHotel = TravelExpenseClaimCommon.GetDataSource(dtHotel, dt, "ExpenseType='Hotel'");
                        dtMeal = TravelExpenseClaimCommon.GetDataSource(dtMeal, dt, "ExpenseType='Meal Allowance'");
                        dtTrans = TravelExpenseClaimCommon.GetDataSource(dtTrans, dt, "ExpenseType='Local Transportation'");
                        dtSample = TravelExpenseClaimCommon.GetDataSource(dtSample, dt, "ExpenseType='Sample Purchase'");
                        dtOthers = TravelExpenseClaimCommon.GetDataSource(dtOthers, dt, "ExpenseType='Others'");

                        this.rptHotel.DataSource = dtHotel;
                        rptHotel.DataBind();
                        this.rptMeal.DataSource = dtMeal;
                        rptMeal.DataBind();
                        this.rptTrans.DataSource = dtTrans;
                        rptTrans.DataBind();
                        this.rptSample.DataSource = dtSample;
                        rptSample.DataBind();
                        this.rptOthers.DataSource = dtOthers;
                        rptOthers.DataBind();
                    }

                    LoadSourceData(trWorkflowNumber);
                }
                else
                {
                    LoadSourceData(requestId);

                    this.rptHotel.DataSource = this.HotelTable;
                    this.rptHotel.DataBind();

                    this.rptMeal.DataSource = this.MealTable;
                    this.rptMeal.DataBind();

                    this.rptTrans.DataSource = this.TransTable;
                    this.rptTrans.DataBind();

                    this.rptSample.DataSource = this.SampleTable;
                    this.rptSample.DataBind();

                    this.rptOthers.DataSource = this.OthersTable;
                    this.rptOthers.DataBind();
                }

            }
            DataBindCostCenter();


        }

        private void LoadSourceData(string id)
        {
            //SPSecurity.RunWithElevatedPrivileges(delegate() {
            //    using (SPSite site = new SPSite(SPContext.Current.Site.ID)) {
            //        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID)) { 
            //            }
            //    }
            //});


            JavaScriptSerializer oSerializer = new JavaScriptSerializer();

            List<object> travelPolicyInfo = TravelExpenseClaimCommon.GetSerializingList(SPContext.Current.Web.Lists["Travel Policy"].Items,
                                                                    new TravelPolicyItem());
            hidTravelPolicy.Value = oSerializer.Serialize(travelPolicyInfo);

            if (id.IsNotNullOrWhitespace())
            {
                SPListItemCollection travelDetailItems = TravelExpenseClaimCommon.GetDataCollection(id, "Travel Details2");
                SPListItemCollection travelRequestItems = TravelExpenseClaimCommon.GetDataCollection(id, "Travel Request Workflow2");

                List<object> travelDetailInfo = TravelExpenseClaimCommon.GetSerializingList(travelDetailItems, new TravelDetailItem());
                List<object> travelRequestInfo = TravelExpenseClaimCommon.GetSerializingList(travelRequestItems, new TravelRequestItem());

                hidTravelDetails.Value = oSerializer.Serialize(travelDetailInfo);
                hidTravelRequest.Value = oSerializer.Serialize(travelRequestInfo);

            }


        }


        protected void DataBindCostCenter()
        {
            DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
            ddlCostCenter.DataSource = WorkFlowUtil.GetDataSourceBySort(dtCostCenter);
            ddlCostCenter.DataTextField = "Display";
            ddlCostCenter.DataValueField = "Title";
            ddlCostCenter.DataBind();
            ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
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
        internal DataTable MealTable
        {
            get
            {
                return (this.ViewState["MealTable"] as DataTable) ?? CreateMealTable();
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
                return (this.ViewState["TransTable"] as DataTable) ?? CreateTransTable();
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
                return (this.ViewState["SampleTable"] as DataTable) ?? CreateSampleTable();
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
                return (this.ViewState["OthersTable"] as DataTable) ?? CreateOthersTable();
            }
            set
            {
                this.ViewState["OthersTable"] = value;
            }
        }

        private DataTable CreateHotelTable()
        {
            HotelTable = new DataTable();
            HotelTable.TableName = "Hotel";
            HotelTable.Columns.Add("ExpenseDetail");
            HotelTable.Columns.Add("TravelDateFrom");
            HotelTable.Columns.Add("TravelDateTo");
            HotelTable.Columns.Add("CostCenter");
            HotelTable.Columns.Add("OriginalAmt");
            HotelTable.Columns.Add("Currency");
            HotelTable.Columns.Add("ExchRate");
            HotelTable.Columns.Add("RmbAmt");
            HotelTable.Columns.Add("CompanyStandards");
            HotelTable.Columns.Add("SpecialApprove");
            HotelTable.Columns.Add("IsPaidByCredit");
            HotelTable.Columns.Add("Remark");
            HotelTable.Columns.Add("OtherCurrency");

            //HotelTable.Rows.Add();
            return HotelTable;
        }
        private DataTable CreateMealTable()
        {
            MealTable = new DataTable();
            MealTable.TableName = "Meal Allowance";
            MealTable.Columns.Add("ExpenseDetail");
            MealTable.Columns.Add("Date");
            MealTable.Columns.Add("CostCenter");
            MealTable.Columns.Add("OriginalAmt");
            MealTable.Columns.Add("Currency");
            MealTable.Columns.Add("ExchRate");
            MealTable.Columns.Add("RmbAmt");
            MealTable.Columns.Add("CompanyStandards");
            MealTable.Columns.Add("SpecialApprove");
            MealTable.Columns.Add("IsPaidByCredit");
            MealTable.Columns.Add("Remark");
            MealTable.Columns.Add("HidMealItem");
            MealTable.Columns.Add("OtherCurrency");

            return MealTable;
        }
        private DataTable CreateTransTable()
        {
            TransTable = new DataTable();
            TransTable.TableName = "Local Transportation";
            TransTable.Columns.Add("ExpenseDetail");
            TransTable.Columns.Add("Date");
            TransTable.Columns.Add("CostCenter");
            TransTable.Columns.Add("OriginalAmt");
            TransTable.Columns.Add("Currency");
            TransTable.Columns.Add("ExchRate");
            TransTable.Columns.Add("RmbAmt");
            TransTable.Columns.Add("CompanyStandards");
            TransTable.Columns.Add("SpecialApprove");
            TransTable.Columns.Add("IsPaidByCredit");
            TransTable.Columns.Add("Remark");
            TransTable.Columns.Add("OtherCurrency");

            //TransTable.Rows.Add();
            return TransTable;
        }
        private DataTable CreateSampleTable()
        {
            SampleTable = new DataTable();
            SampleTable.TableName = "Sample Purchase";
            SampleTable.Columns.Add("ExpenseDetail");
            SampleTable.Columns.Add("Date");
            SampleTable.Columns.Add("CostCenter");
            SampleTable.Columns.Add("OriginalAmt");
            SampleTable.Columns.Add("Currency");
            SampleTable.Columns.Add("ExchRate");
            SampleTable.Columns.Add("RmbAmt");
            SampleTable.Columns.Add("CompanyStandards");
            SampleTable.Columns.Add("SpecialApprove");
            SampleTable.Columns.Add("IsPaidByCredit");
            SampleTable.Columns.Add("Remark");
            SampleTable.Columns.Add("OtherCurrency");

            //SampleTable.Rows.Add();
            return SampleTable;
        }
        private DataTable CreateOthersTable()
        {
            OthersTable = new DataTable();
            OthersTable.TableName = "Others";
            OthersTable.Columns.Add("ExpenseDetail");
            OthersTable.Columns.Add("Date");
            OthersTable.Columns.Add("CostCenter");
            OthersTable.Columns.Add("OriginalAmt");
            OthersTable.Columns.Add("Currency");
            OthersTable.Columns.Add("ExchRate");
            OthersTable.Columns.Add("RmbAmt");
            OthersTable.Columns.Add("CompanyStandards");
            OthersTable.Columns.Add("SpecialApprove");
            OthersTable.Columns.Add("IsPaidByCredit");
            OthersTable.Columns.Add("Remark");
            OthersTable.Columns.Add("OtherCurrency");

            //OthersTable.Rows.Add();
            return OthersTable;
        }

        protected void rptHotel_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateDataTable(this.HotelTable, this.rptHotel);
                HotelTable.Rows.Remove(HotelTable.Rows[e.Item.ItemIndex]);
                this.rptHotel.DataSource = HotelTable;
                this.rptHotel.DataBind();
            }
        }
        protected void rptMeal_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateDataTable(this.MealTable, this.rptMeal);
                MealTable.Rows.Remove(MealTable.Rows[e.Item.ItemIndex]);
                this.rptMeal.DataSource = MealTable;
                this.rptMeal.DataBind();
            }
        }
        protected void rptTrans_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateDataTable(this.TransTable, this.rptTrans);
                TransTable.Rows.Remove(TransTable.Rows[e.Item.ItemIndex]);
                this.rptTrans.DataSource = TransTable;
                this.rptTrans.DataBind();
            }
        }
        protected void rptSample_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateDataTable(this.SampleTable, this.rptSample);
                SampleTable.Rows.Remove(SampleTable.Rows[e.Item.ItemIndex]);
                this.rptSample.DataSource = SampleTable;
                this.rptSample.DataBind();
            }
        }
        protected void rptOthers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateDataTable(this.OthersTable, this.rptOthers);
                OthersTable.Rows.Remove(OthersTable.Rows[e.Item.ItemIndex]);
                this.rptOthers.DataSource = OthersTable;
                this.rptOthers.DataBind();
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
                    var ddlLocation = (DropDownList)item.FindControl("ddlLocation");
                    var dtFrom = (CADateTimeControl)item.FindControl("dtFrom");
                    var dtTo = (CADateTimeControl)item.FindControl("dtTo");
                    var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                    var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                    var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                    var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                    var lblCompanyStandards = (Label)item.FindControl("lblCompanyStandards");
                    var hidCompanyStandards = (HiddenField)item.FindControl("hidCompanyStandards");
                    var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                    var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                    var txtRemark = (TextBox)item.FindControl("txtRemark");
                    var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                    ddlLocation.SelectedValue = row["ExpenseDetail"].ToString();
                    dtFrom.SelectedDate = string.IsNullOrEmpty(row["TravelDateFrom"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["TravelDateFrom"].ToString());
                    dtTo.SelectedDate = string.IsNullOrEmpty(row["TravelDateTo"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["TravelDateTo"].ToString());
                    txtOriginalAmt.Text = row["OriginalAmt"].ToString();
                    ddlCurrency.SelectedValue = row["Currency"].ToString();
                    txtOtherCurrency.Text = row["OtherCurrency"].ToString();
                    txtExchRate.Text = row["ExchRate"].ToString();
                    txtRmbAmt.Text = row["RmbAmt"].ToString();
                    lblCompanyStandards.Text = row["CompanyStandards"].ToString();
                    hidCompanyStandards.Value = row["CompanyStandards"].ToString();

                    cbSpecialApprove.Checked = ReturnCheckBoxValue(row["SpecialApprove"].ToString());
                    cbPaidByCredit.Checked = ReturnCheckBoxValue(row["IsPaidByCredit"].ToString());

                    txtRemark.Text = Server.HtmlDecode(row["Remark"].ToString());

                    DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
                    ddlCostCenter.DataSource = dtCostCenter;
                    ddlCostCenter.DataTextField = "Display";
                    ddlCostCenter.DataValueField = "Title";
                    ddlCostCenter.DataBind();
                    ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
                    if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
                    {
                        ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
                    }
                }
            }
        }

        protected void rptMeal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var ddlMealType = (DropDownList)item.FindControl("ddlMealType");
                    var dtDate = (CADateTimeControl)item.FindControl("dtDate");
                    var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                    var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                    var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                    var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                    var lblCompanyStandards = (Label)item.FindControl("lblCompanyStandards");
                    var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                    var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                    var txtRemark = (TextBox)item.FindControl("txtRemark");
                    var hidCompanyStandards = (HiddenField)item.FindControl("hidCompanyStandards");
                    var hidMealItem = (HiddenField)item.FindControl("hidMealItem");
                    var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                    ddlMealType.SelectedValue = row["ExpenseDetail"].ToString();
                    dtDate.SelectedDate = string.IsNullOrEmpty(row["Date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Date"].ToString());
                    txtOriginalAmt.Text = row["OriginalAmt"].ToString();
                    ddlCurrency.SelectedValue = row["Currency"].ToString();
                    txtOtherCurrency.Text = row["OtherCurrency"].ToString();
                    txtExchRate.Text = row["ExchRate"].ToString();
                    txtRmbAmt.Text = row["RmbAmt"].ToString();
                    lblCompanyStandards.Text = row["CompanyStandards"].ToString();
                    hidCompanyStandards.Value = row["CompanyStandards"].ToString();
                    cbSpecialApprove.Checked = ReturnCheckBoxValue(row["SpecialApprove"].ToString());
                    cbPaidByCredit.Checked = ReturnCheckBoxValue(row["IsPaidByCredit"].ToString());
                    txtRemark.Text = Server.HtmlDecode(row["Remark"].ToString());
                    hidMealItem.Value = row["HidMealItem"].ToString();

                    DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
                    ddlCostCenter.DataSource = dtCostCenter;
                    ddlCostCenter.DataTextField = "Display";
                    ddlCostCenter.DataValueField = "Title";
                    ddlCostCenter.DataBind();
                    ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
                    if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
                    {
                        ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
                    }
                }
            }
        }
        protected void rptTrans_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var txtExpenseDetail = (TextBox)item.FindControl("txtExpenseDetail");
                    var dtDate = (CADateTimeControl)item.FindControl("dtDate");
                    var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                    var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                    var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                    var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                    var lblCompanyStandards = (Label)item.FindControl("lblCompanyStandards");
                    var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                    var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                    var txtRemark = (TextBox)item.FindControl("txtRemark");
                    var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                    txtExpenseDetail.Text = row["ExpenseDetail"].ToString();
                    dtDate.SelectedDate = string.IsNullOrEmpty(row["Date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Date"].ToString());
                    txtOriginalAmt.Text = row["OriginalAmt"].ToString();
                    txtExchRate.Text = row["ExchRate"].ToString();
                    txtRmbAmt.Text = row["RmbAmt"].ToString();
                    lblCompanyStandards.Text = row["CompanyStandards"].ToString();
                    cbSpecialApprove.Checked = ReturnCheckBoxValue(row["SpecialApprove"].ToString());
                    cbPaidByCredit.Checked = ReturnCheckBoxValue(row["IsPaidByCredit"].ToString());
                    txtRemark.Text = Server.HtmlDecode(row["Remark"].ToString());
                    ddlCurrency.SelectedValue = row["Currency"].ToString();
                    txtOtherCurrency.Text = row["OtherCurrency"].ToString();

                    DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
                    ddlCostCenter.DataSource = dtCostCenter;
                    ddlCostCenter.DataTextField = "Display";
                    ddlCostCenter.DataValueField = "Title";
                    ddlCostCenter.DataBind();
                    ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
                    if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
                    {
                        ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
                    }
                }
            }
        }
        protected void rptSample_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var txtExpenseDetail = (TextBox)item.FindControl("txtExpenseDetail");
                    var dtDate = (CADateTimeControl)item.FindControl("dtDate");
                    var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                    var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                    var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                    var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                    var lblCompanyStandards = (Label)item.FindControl("lblCompanyStandards");
                    var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                    var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                    var txtRemark = (TextBox)item.FindControl("txtRemark");
                    var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                    txtExpenseDetail.Text = row["ExpenseDetail"].ToString();
                    dtDate.SelectedDate = string.IsNullOrEmpty(row["Date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Date"].ToString());
                    txtOriginalAmt.Text = row["OriginalAmt"].ToString();
                    ddlCurrency.SelectedValue = row["Currency"].ToString();
                    txtOtherCurrency.Text = row["OtherCurrency"].ToString();
                    txtExchRate.Text = row["ExchRate"].ToString();
                    txtRmbAmt.Text = row["RmbAmt"].ToString();
                    lblCompanyStandards.Text = row["CompanyStandards"].ToString();
                    cbSpecialApprove.Checked = ReturnCheckBoxValue(row["SpecialApprove"].ToString());
                    cbPaidByCredit.Checked = ReturnCheckBoxValue(row["IsPaidByCredit"].ToString());
                    txtRemark.Text = Server.HtmlDecode(row["Remark"].ToString());


                    DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
                    ddlCostCenter.DataSource = dtCostCenter;
                    ddlCostCenter.DataTextField = "Display";
                    ddlCostCenter.DataValueField = "Title";
                    ddlCostCenter.DataBind();
                    ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
                    if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
                    {
                        ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
                    }
                }
            }
        }
        protected void rptOthers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var txtExpenseDetail = (TextBox)item.FindControl("txtExpenseDetail");
                    var dtDate = (CADateTimeControl)item.FindControl("dtDate");
                    var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                    var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                    var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                    var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                    var lblCompanyStandards = (Label)item.FindControl("lblCompanyStandards");
                    var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                    var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                    var txtRemark = (TextBox)item.FindControl("txtRemark");
                    var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                    txtExpenseDetail.Text = row["ExpenseDetail"].ToString();
                    dtDate.SelectedDate = string.IsNullOrEmpty(row["Date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Date"].ToString());
                    txtOriginalAmt.Text = row["OriginalAmt"].ToString();
                    ddlCurrency.SelectedValue = row["Currency"].ToString();
                    txtOtherCurrency.Text = row["OtherCurrency"].ToString();
                    txtExchRate.Text = row["ExchRate"].ToString();
                    txtRmbAmt.Text = row["RmbAmt"].ToString();
                    lblCompanyStandards.Text = row["CompanyStandards"].ToString();
                    cbSpecialApprove.Checked = ReturnCheckBoxValue(row["SpecialApprove"].ToString());
                    cbPaidByCredit.Checked = ReturnCheckBoxValue(row["IsPaidByCredit"].ToString());
                    txtRemark.Text = Server.HtmlDecode(row["Remark"].ToString());

                    DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
                    ddlCostCenter.DataSource = dtCostCenter;
                    ddlCostCenter.DataTextField = "Display";
                    ddlCostCenter.DataValueField = "Title";
                    ddlCostCenter.DataBind();
                    ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
                    if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
                    {
                        ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
                    }
                }
            }
        }

        protected void btnAddHotel_Click(object sender, ImageClickEventArgs e)
        {

            UpdateDataTable(this.HotelTable, this.rptHotel);

            if (this.rptHotel.Items.Count > 0)
            {
                int rowIndex = HotelTable.Rows.Count - 1;
                HotelTable.Rows.Add(HotelTable.Rows[rowIndex].ItemArray);
            }
            else
            {
                HotelTable.Rows.Add();
            }

            this.rptHotel.DataSource = this.HotelTable;
            this.rptHotel.DataBind();
        }

        //protected void btnAddMeal_Click(object sender, ImageClickEventArgs e)
        //{

        //}
        private bool IsNotNumberic(string oText)
        {
            if (oText == "NaN")
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

        protected void btnAddMealItems_Click(object sender, EventArgs e)
        {
            UpdateDataTable(this.MealTable, this.rptMeal);

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();

            MealItem mealItem = oSerializer.Deserialize<MealItem>(hidMealItemValue.Value);
            string rate = this.txtExchRate.Text;
            string areaRate = this.hidExchRate.Value;
            string currency = this.txtCurrency.Text;

            List<string> currencyList = new List<string>() { "RMB", "USD", "GBP", "EUR", "AUD", "CHF", "CAD", "JPY", "HKD" };

            if (!currencyList.Contains(currency))
            {
                currency = "Others";
            }

            if (rate.IsNullOrWhitespace()
                || IsNotNumberic(rate))
            {
                rate = "1.0000";
            }

            if (areaRate.IsNullOrWhitespace()
                || IsNotNumberic(areaRate))
            {
                areaRate = "1.0000";
            }

            var breakfastLimit = Math.Round(Convert.ToDouble(areaRate) * Convert.ToDouble(mealItem.BreakfastLimit), 2).ToString();
            var lunchLimit = Math.Round(Convert.ToDouble(areaRate) * Convert.ToDouble(mealItem.LunchLimit), 2).ToString();
            var dinnerLimit = Math.Round(Convert.ToDouble(areaRate) * Convert.ToDouble(mealItem.DinnerLimit), 2).ToString();

            //if (mealItem.Currency != "RMB")
            //{
            //    var rateItem = TravelExpenseClaimCommon.ConvertToRMB(mealItem.Currency);
            //    if (rateItem != null)
            //    {
            //        rate = rateItem["Rate"].AsString();
            //        mealItem.BreakfastLimit = Math.Round(Convert.ToDouble(rateItem["Rate"].AsString()) * Convert.ToDouble(mealItem.BreakfastLimit), 0).ToString();
            //        mealItem.LunchLimit = Math.Round(Convert.ToDouble(rateItem["Rate"].AsString()) * Convert.ToDouble(mealItem.LunchLimit), 0).ToString();
            //        mealItem.DinnerLimit = Math.Round(Convert.ToDouble(rateItem["Rate"].AsString()) * Convert.ToDouble(mealItem.DinnerLimit), 0).ToString();
            //    }
            //}


            int days = Convert.ToInt32(mealItem.Days);
            DateTime date;
            if (string.IsNullOrEmpty(mealItem.DateFrom)
                || string.IsNullOrEmpty(mealItem.DateTo))
            {
                date = DateTime.Now;
            }
            else
            {
                date = DateTime.Parse(mealItem.DateFrom);
            }

            for (int i = 0; i < days; i++)
            {
                string mealDate = date.AddDays(Convert.ToDouble(i)).ToShortDateString();
                switch (this.rblFillAllowance.SelectedValue)
                {
                    case "1":
                        MealTable.Rows.Add(new object[] { "Breakfast", mealDate, mealItem.CostCenter, mealItem.BreakfastLimit, currency, rate, breakfastLimit, breakfastLimit, null, null, null, hidMealItemValue.Value });
                        MealTable.Rows.Add(new object[] { "Lunch", mealDate, mealItem.CostCenter, mealItem.LunchLimit, currency, rate, lunchLimit, lunchLimit, null, null, null, hidMealItemValue.Value });
                        MealTable.Rows.Add(new object[] { "Dinner", mealDate, mealItem.CostCenter, mealItem.DinnerLimit, currency, rate, dinnerLimit, dinnerLimit, null, null, null, hidMealItemValue.Value });
                        break;
                    case "0":
                        MealTable.Rows.Add(new object[] { "Breakfast", mealDate, mealItem.CostCenter, null, currency, rate, null, breakfastLimit, null, null, null, hidMealItemValue.Value });
                        MealTable.Rows.Add(new object[] { "Lunch", mealDate, mealItem.CostCenter, null, currency, rate, null, lunchLimit, null, null, null, hidMealItemValue.Value });
                        MealTable.Rows.Add(new object[] { "Dinner", mealDate, mealItem.CostCenter, null, currency, rate, null, dinnerLimit, null, null, null, hidMealItemValue.Value });
                        break;
                    default:
                        break;
                }

            }

            this.rptMeal.DataSource = this.MealTable;
            this.rptMeal.DataBind();
        }
        protected void btnAddTrans_Click(object sender, ImageClickEventArgs e)
        {
            UpdateDataTable(this.TransTable, this.rptTrans);

            if (this.rptTrans.Items.Count > 0)
            {
                int rowIndex = TransTable.Rows.Count - 1;
                TransTable.Rows.Add(TransTable.Rows[rowIndex].ItemArray);
            }
            else
            {
                TransTable.Rows.Add();
            }

            this.rptTrans.DataSource = this.TransTable;
            this.rptTrans.DataBind();
        }
        protected void btnAddSample_Click(object sender, ImageClickEventArgs e)
        {
            UpdateDataTable(this.SampleTable, this.rptSample);

            if (this.rptSample.Items.Count > 0)
            {
                int rowIndex = SampleTable.Rows.Count - 1;
                SampleTable.Rows.Add(SampleTable.Rows[rowIndex].ItemArray);
            }
            else
            {
                SampleTable.Rows.Add();
            }

            this.rptSample.DataSource = this.SampleTable;
            this.rptSample.DataBind();
        }
        protected void btnAddOthers_Click(object sender, ImageClickEventArgs e)
        {
            UpdateDataTable(this.OthersTable, this.rptOthers);

            if (this.rptOthers.Items.Count > 0)
            {
                int rowIndex = OthersTable.Rows.Count - 1;
                OthersTable.Rows.Add(OthersTable.Rows[rowIndex].ItemArray);
            }
            else
            {
                OthersTable.Rows.Add();
            }

            this.rptOthers.DataSource = this.OthersTable;
            this.rptOthers.DataBind();
        }


        private void UpdateDataTable(DataTable dataTable, Repeater repeater)
        {
            dataTable.Rows.Clear();
            if (repeater.Items.Count > 0)
            {

                switch (dataTable.TableName)
                {
                    case "Hotel":
                        foreach (RepeaterItem item in repeater.Items)
                        {
                            var ddlLocation = (DropDownList)item.FindControl("ddlLocation");
                            var dtFrom = (CADateTimeControl)item.FindControl("dtFrom");
                            var dtTo = (CADateTimeControl)item.FindControl("dtTo");
                            var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                            var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                            var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                            var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                            var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                            var hidCompanyStandards = (HiddenField)item.FindControl("hidCompanyStandards");
                            var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                            var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                            var txtRemark = (TextBox)item.FindControl("txtRemark");
                            var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                            DataRow row = dataTable.Rows.Add();
                            row["ExpenseDetail"] = ddlLocation.Text;
                            row["TravelDateFrom"] = dtFrom.IsDateEmpty ? string.Empty : dtFrom.SelectedDate.ToShortDateString();
                            row["TravelDateTo"] = dtTo.IsDateEmpty ? string.Empty : dtTo.SelectedDate.ToShortDateString();
                            row["CostCenter"] = ddlCostCenter.SelectedValue;
                            row["OriginalAmt"] = txtOriginalAmt.Text;
                            row["Currency"] = ddlCurrency.SelectedValue;
                            row["ExchRate"] = txtExchRate.Text;
                            row["RmbAmt"] = txtRmbAmt.Text;
                            row["CompanyStandards"] = hidCompanyStandards.Value;
                            row["SpecialApprove"] = cbSpecialApprove.Checked;
                            row["IsPaidByCredit"] = cbPaidByCredit.Checked;
                            row["Remark"] = txtRemark.Text;
                            row["OtherCurrency"] = txtOtherCurrency.Text;
                        }
                        break;
                    case "Meal Allowance":
                        foreach (RepeaterItem item in repeater.Items)
                        {
                            var ddlMealType = (DropDownList)item.FindControl("ddlMealType");
                            var dtDate = (CADateTimeControl)item.FindControl("dtDate");
                            var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                            var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                            var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                            var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                            var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                            var hidCompanyStandards = (HiddenField)item.FindControl("hidCompanyStandards");
                            var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                            var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                            var txtRemark = (TextBox)item.FindControl("txtRemark");
                            var hidMealItem = (HiddenField)item.FindControl("hidMealItem");
                            var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                            DataRow row = dataTable.Rows.Add();
                            row["ExpenseDetail"] = ddlMealType.SelectedValue;
                            row["Date"] = dtDate.IsDateEmpty ? string.Empty : dtDate.SelectedDate.ToShortDateString();
                            row["CostCenter"] = ddlCostCenter.SelectedValue;
                            row["OriginalAmt"] = txtOriginalAmt.Text;
                            row["Currency"] = ddlCurrency.SelectedValue;
                            row["OtherCurrency"] = txtOtherCurrency.Text;
                            row["ExchRate"] = txtExchRate.Text;
                            row["RmbAmt"] = txtRmbAmt.Text;
                            row["CompanyStandards"] = hidCompanyStandards.Value;
                            row["SpecialApprove"] = cbSpecialApprove.Checked;
                            row["IsPaidByCredit"] = cbPaidByCredit.Checked;
                            row["Remark"] = txtRemark.Text;
                            row["HidMealItem"] = hidMealItem.Value;
                        }
                        break;
                    default:
                        foreach (RepeaterItem item in repeater.Items)
                        {
                            var txtExpenseDetail = (TextBox)item.FindControl("txtExpenseDetail");
                            var dtDate = (CADateTimeControl)item.FindControl("dtDate");
                            var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                            var txtOriginalAmt = (TextBox)item.FindControl("txtOriginalAmt");
                            var ddlCurrency = (DropDownList)item.FindControl("ddlCurrency");
                            var txtExchRate = (TextBox)item.FindControl("txtExchRate");
                            var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                            var hidCompanyStandards = (HiddenField)item.FindControl("hidCompanyStandards");
                            var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApprove");
                            var cbPaidByCredit = (CheckBox)item.FindControl("cbPaidByCredit");
                            var txtRemark = (TextBox)item.FindControl("txtRemark");
                            var txtOtherCurrency = (TextBox)item.FindControl("txtOtherCurrency");

                            DataRow row = dataTable.Rows.Add();
                            row["ExpenseDetail"] = txtExpenseDetail.Text;
                            row["Date"] = dtDate.IsDateEmpty ? string.Empty : dtDate.SelectedDate.ToShortDateString();
                            row["CostCenter"] = ddlCostCenter.SelectedValue;
                            row["OriginalAmt"] = txtOriginalAmt.Text;
                            row["Currency"] = ddlCurrency.SelectedValue;
                            row["ExchRate"] = txtExchRate.Text;
                            row["RmbAmt"] = txtRmbAmt.Text;
                            row["CompanyStandards"] = hidCompanyStandards.Value;
                            row["SpecialApprove"] = cbSpecialApprove.Checked;
                            row["IsPaidByCredit"] = cbPaidByCredit.Checked;
                            row["Remark"] = txtRemark.Text;
                            row["OtherCurrency"] = txtOtherCurrency.Text;
                        }
                        break;
                }
            }
        }

        protected string GetExchRate(string currency)
        {
            var exchRate = "";
            var rateItem = TravelExpenseClaimCommon.ConvertToRMB(currency);
            if (rateItem != null)
            {
                exchRate = (Convert.ToDouble(rateItem["Rate"].AsString())).ToString();

            }
            return exchRate;
        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

        internal void Update()
        {
            UpdateDataTable(this.HotelTable, this.rptHotel);
            UpdateDataTable(this.MealTable, this.rptMeal);
            UpdateDataTable(this.TransTable, this.rptTrans);
            UpdateDataTable(this.SampleTable, this.rptSample);
            UpdateDataTable(this.OthersTable, this.rptOthers);
        }

        internal object ValidateForSave()
        {
            throw new NotImplementedException();
        }

        private bool ReturnCheckBoxValue(string s)
        {
            bool restult = false;
            int temp;
            if (!string.IsNullOrEmpty(s))
            {
                if (Int32.TryParse(s, out temp))
                {
                    restult = Convert.ToBoolean(temp);
                }
                else
                {
                    restult = Convert.ToBoolean(s);
                }
            }
            return restult;

        }

        internal string ValidateForSubmit()
        {
            string errorMessage = string.Empty;

            if (rptHotel.Items.Count == 0
                && rptMeal.Items.Count == 0
                && rptOthers.Items.Count == 0
                && rptSample.Items.Count == 0
                && rptTrans.Items.Count == 0)
            {
                errorMessage = "You have unclaimed items. Please make sure they are related to trips applied through Travel Request. If not, please claim them here.";
            }

            if (ffCashAdvanced.Value.AsString().IsNotNullOrWhitespace()
                && ffComparedToApproved.AsString().IsNotNullOrWhitespace()
                && ffHotelSubTotal.AsString().IsNotNullOrWhitespace()
                && ffMealSubTotal.AsString().IsNotNullOrWhitespace()
                && ffNetPayable.AsString().IsNotNullOrWhitespace()
                && ffOthersSubTotal.AsString().IsNotNullOrWhitespace()
                && ffPaidByCreditCard.AsString().IsNotNullOrWhitespace()
                && ffSampleSubTotal.AsString().IsNotNullOrWhitespace()
                && ffTotalCost.AsString().IsNotNullOrWhitespace()
                && ffTotalExceptFlight.AsString().IsNotNullOrWhitespace()
                && ffTransSubTotal.AsString().IsNotNullOrWhitespace())
            {
                errorMessage = "Claim Summary can't be null, please check it again.";
            }
            return errorMessage;
        }
    }

    public class TravelPolicyItem
    {
        public string Title { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public string Location { get; set; }
        public string BreakfastLimit { get; set; }
        public string LunchLimit { get; set; }
        public string DinnerLimit { get; set; }
        public string HotelLimit { get; set; }
    }

    public class TravelDetailItem
    {
        public string TravelDateFrom { get; set; }
        public string TravelDateTo { get; set; }
        public string VehicleEstimatedCost { get; set; }
    }

    public class TravelRequestItem
    {
        public string WorkflowNumber { get; set; }
        public string Applicant { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Department { get; set; }
        public string IDNumber { get; set; }
        public string Mobile { get; set; }
        public string OfficeExt { get; set; }
        public string CashAdvanced { get; set; }
        public string TravelTotalCost { get; set; }
        public string TravelPurpose { get; set; }
        public string TravelOtherPurpose { get; set; }
        //public string Modified { get; set; }
    }
    public class MealItem
    {
        public string Currency { get; set; }
        public string BreakfastLimit { get; set; }
        public string LunchLimit { get; set; }
        public string DinnerLimit { get; set; }
        public string Days { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CostCenter { get; set; }
    }
}