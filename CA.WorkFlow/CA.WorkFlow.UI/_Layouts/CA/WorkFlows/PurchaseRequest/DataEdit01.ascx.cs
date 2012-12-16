namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Linq;
    using System.Data;
    using System.Web.UI;
    using CA.SharePoint.Utilities.Common;
    using System.Globalization;
    using System.Collections.Generic;
    using CA.WorkFlow.UI.Code;
    using Microsoft.SharePoint;
    using System.IO;
    using System.Text;
    using System.Web.UI.WebControls;

    public partial class DataEdit01 : BaseWorkflowUserControl
    {
        private int index = 1; //Genernate the number automatically.
        private string msg = string.Empty;
        internal string MSG { get { return msg; } }

        private string requestId;
        internal string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        private string mode;
        internal string Mode
        {
            set
            {
                this.mode = value;
            }
        }
        private string applicant;
        internal string Applicant { get { return applicant; } set { applicant = value; } }

        private string capexType;
        internal string CapexType { get { return GetCapexType(); } set { capexType = value; } }

        private string requestType;
        internal string RequestType { get { return GetRequestType(); } set { requestType = value; } }

        private string formType;
        internal string FormType { get { return GetFormType(); } set { formType = value; } }

        private string hoPurposeType;
        internal string HOPurposeType { get { return GetHOPurposeType(); } set { hoPurposeType = value; } }

        private string storePurposeType;
        internal string StorePurposeType { get { return GetStorePurposeType(); } set { storePurposeType = value; } }

        internal string ItemIdentity { get { return GetItemIdentity(); } }

        private string displayMode;
        internal string DisplayMode { get { return this.displayMode; } set { this.displayMode = value; } }

        internal bool IsReturn { get { return Convert.ToBoolean(this.ffIsReturn.Value); } }

        private bool isHO = false;
        internal bool IsHO { set { isHO = value; } }


        internal string GetTaskAssignType(DataTable dtItemTable)
        {
            string taskAssignType = string.Empty;

            string curItemType = string.Empty;
            int count = 0;
            foreach (DataRow dr in dtItemTable.Rows)
            {
                if (dr["ItemType"].ToString().ToLower().Contains("maintenance"))
                    count++;
            }
            if (count == 0)
            {
                taskAssignType = GetRequestType();
            }
            else
            {
                taskAssignType = "Maintenance";
            }
            return taskAssignType;
        }

        internal string UploadMsg
        {
            get
            {
                return (this.ViewState["UploadMsg"] as string) ?? string.Empty;
            }
            set
            {
                this.ViewState["UploadMsg"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {

                BindCostCenter();
                #region Recovery Data For Edit and Check pages
                if (this.mode != null && (this.mode.Equals("Edit", StringComparison.CurrentCultureIgnoreCase) || this.mode.Equals("Check", StringComparison.CurrentCultureIgnoreCase)))
                {
                    DataTable itemDetails = PurchaseRequestCommon.GetDataTable(requestId);
                    if (itemDetails == null)
                    {
                        ///this.ItemTable.Rows.Clear();
                        itemDetails = CreateItemTable();/// SetRowLimit(itemDetails);
                    }
                    this.rptItem.DataSource = itemDetails;// == null ? SetRowLimit(this.ItemTable) : SetRowLimit(itemDetails)//SetRowLimit(this.ItemTable) : SetRowLimit(itemDetails);
                    this.rptItem.DataBind();

                    SetRequestType(this.requestType);
                    SetCapexType(this.capexType);
                    SetFormType(this.formType);
                    SetHOPurposeType(this.hoPurposeType);
                    SetStorePurposeType(this.storePurposeType);
                    this.lbTotal.Text = GetTotalRMB(itemDetails).ToString();//this.totalRMB;
                    this.lbApproveTotal.Text = GetApprovalTotalRMB(itemDetails);//this.approvalTotalRMB;
                }
                else//New
                {
                    this.rptItem.DataSource = SetRowLimit(CreateItemTable());//this.ItemTable);
                    this.rptItem.DataBind();
                }
                #endregion
                SetPaperBagEnable();//设置是否可选纸袋

                this.lbApplicant.Text = this.applicant;
                this.hidDisplayMode.Value = displayMode;



                #region Set Form Type Value and Effect
                //Hide controls which needn't for current user
                //当新建和编辑页面时控件控件的显示效果
                switch (this.mode)
                {
                    case "Edit"://Edit页面
                        if (!isHO)
                        {
                            this.rbFormType.Items[1].Enabled = false;
                        }
                        else
                        {
                            this.rbFormType.Items[0].Enabled = false;
                        }
                        break;
                    case "Check"://Check页面
                        if (!isHO)
                        {
                            this.rbFormType.Items[1].Enabled = false;
                        }
                        else
                        {
                            this.rbFormType.Items[0].Enabled = false;
                        }
                        break;
                    default://New页面
                        if (!isHO)
                        {
                            this.rbFormType.Items[1].Enabled = false;
                        }
                        else
                        {
                            this.rbFormType.Items[0].Enabled = false;
                            this.rbFormType.Items[1].Selected = true;
                        }
                        break;
                }
                #endregion
                // SetVendorJson();
            }
        }
        /// <summary>
        /// 得到 CostCenter
        /// </summary>
        /// <returns></returns>
        DataTable dtCostCenter()
        {
            DataTable dtCostCenters = new DataTable();
            if (isHO)
            {
                dtCostCenters = PurchaseRequestCommon.GetAllCostCenterDT();
            }
            else
            {
                dtCostCenters = PurchaseRequestCommon.GetCostCenterForStore(WorkFlowUtil.GetApplicantAccount(applicant));
            }
            return dtCostCenters;
        }
        
        internal void RebindItems(DataTable dt)
        {
            this.rptItem.DataSource = dt;//this.ItemTable.Rows[0]["ItemCode"].ToString().IsNullOrWhitespace() ? PurchaseRequestCommon.GetDataTable(requestId) : this.ItemTable;
            this.rptItem.DataBind();
        }

        internal string GetRequestType()
        {
            return this.rbRequestType.SelectedValue;
        }

        /// <summary>
        /// 得到申请类型:新店，改造店，旧店,易耗品
        /// </summary>
        /// <returns></returns>
        private string GetCapexType()
        {
            var type = string.Empty;

            var requestType = GetRequestType();//Capex, opex
            if (requestType.Equals(this.rbRequestType.Items[1].Value, StringComparison.CurrentCultureIgnoreCase))//Capex
            {
                type = this.rbCapexType.SelectedValue;//新店，改造店，旧店
            }
            else if (requestType.Equals(this.rbRequestType.Items[0].Value, StringComparison.CurrentCultureIgnoreCase))//Opex
            {
                type = this.rbRequestType.Items[0].Value;//易耗品
            }

            return type;
        }

        internal string GetFormType()
        {
            return this.rbFormType.SelectedValue;
        }

        private string GetHOPurposeType()
        {
            return this.rbPRHOPurpose.SelectedValue;
        }


        internal string GetStorePurposeType()
        {
            return this.rbPRStorePurpose.SelectedValue;
        }
        //若FormType为Store，则识别号为FormType+PurposeType+RequestType+CapexType+"R"/"N"
        //若FormType不为Store，则识别号为FormType+PurposeType+RequestType+CapexType+"R"/"N"
        private string GetItemIdentity()
        {
            var formType = GetFormType();//Shore, HO
            var requestType = GetRequestType();//Capex,opex
            var capexType = GetCapexType();//NewProject，Refurbishment，Exist,Opex
            var purposeType = string.Empty;
            if (formType.Equals(this.rbFormType.Items[0].Value, StringComparison.CurrentCultureIgnoreCase))//Store
            {
                purposeType = GetStorePurposeType();//Daily,QuarterlyOrder//,PaperBag
            }
            else if (formType.Equals(this.rbFormType.Items[1].Value, StringComparison.CurrentCultureIgnoreCase))//HO
            {
                purposeType = GetHOPurposeType();//Construction,Store,Department
            }

            var identity = string.Empty;
            var checkReturn = (Convert.ToBoolean(this.ffIsReturn.Value) ? "R" : "N");
            var checkHO = isHO ? "Y" : "N";

            switch (purposeType)
            {
                case "Daily":
                    purposeType = "DA";
                    break;
                case "QuarterlyOrder":
                    purposeType = "QO";
                    break;
                case "Construction":
                    purposeType = "COP";
                    break;
                case "Store":
                    purposeType = "SOP";
                    break;
                case "Department":
                    purposeType = "DTP";
                    break;
                case "PaperBag":  // add by xu 
                    purposeType = "PB"; //add by xu 
                    break; //add by xu 
            }
            switch (capexType)
            {
                case "NewProject":
                    capexType = "NP";
                    break;
                case "Refurbishment":
                    capexType = "REF";
                    break;
                case "Maintenance":
                    capexType = "MT";
                    break;
                case "Exist":
                    capexType = "EX";
                    break;
            }

            if (purposeType == "PB")// by xu
            {
                identity = "PB" + purposeType + requestType + capexType + checkReturn;
            }
            else // by xu 
            {
                switch (formType)
                {
                    case "Store":
                        formType = "ST";
                        identity = formType + purposeType + requestType + capexType + checkReturn;
                        break;
                    case "HO":
                        formType = "HO";
                        identity = formType + purposeType + requestType + capexType + checkReturn;
                        break;
                    default:
                        identity = "Invalid";
                        break;
                }
            }

            return identity;
        }
        private void SetFormType(string type)
        {
            this.rbFormType.SelectedValue = type;
        }

        private void SetHOPurposeType(string type)
        {
            this.rbPRHOPurpose.SelectedValue = type;
        }

        private void SetStorePurposeType(string type)
        {
            this.rbPRStorePurpose.SelectedValue = type;
        }

        private void SetRequestType(string type)
        {
            this.rbRequestType.SelectedValue = type;
        }

        private void SetCapexType(string type)
        {
            this.rbCapexType.SelectedValue = type;
        }

        protected void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var lbItem = (Label)item.FindControl("lbItem");
                    var lbDesc = (Label)item.FindControl("lbDesc");
                    var txtTransQuantity = (TextBox)item.FindControl("txtTransQuantity");
                    var lbRequestQuantity = (Label)item.FindControl("lbRequestQuantity");
                    var hidRequestQuantity = (HiddenField)item.FindControl("hidRequestQuantity");
                    var lbUnit = (Label)item.FindControl("lbUnit");
                    var lbVendor = (Label)item.FindControl("lbVendor");
                    var txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                    var lbTotalPrice = (Label)item.FindControl("lbTotalPrice");
                    var lbDeliveryPeriod = (Label)item.FindControl("lbDeliveryPeriod");
                    var lbTaxValue = (Label)item.FindControl("lbTaxValue");
                    HiddenField HiddenFieldCostCenterID = item.FindControl("HiddenFieldCostCenterID") as HiddenField;
                    HiddenField HiddenFieldCostCenter = item.FindControl("HiddenFieldCostCenter") as HiddenField;
                    //var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter"); old
                    var lbAssetClass = (Label)item.FindControl("lbAssetClass");
                    var hlPhoto = (HyperLink)item.FindControl("hlPhoto");
                    var lbUnitPrice = (Label)item.FindControl("lbUnitPrice");
                    var lbItemCode = (Label)item.FindControl("lbItemCode");
                    var hidItemCode = (HiddenField)item.FindControl("hidItemCode");
                    var hidDesc = (HiddenField)item.FindControl("hidDesc");
                    var hidUnit = (HiddenField)item.FindControl("hidUnit");
                    var hidVendor = (HiddenField)item.FindControl("hidVendor");
                    var hidDeliveryPeriod = (HiddenField)item.FindControl("hidDeliveryPeriod");
                    var hidTaxValue = (HiddenField)item.FindControl("hidTaxValue");
                    var hidAssetClass = (HiddenField)item.FindControl("hidAssetClass");
                    var hidVendorId = (HiddenField)item.FindControl("hidVendorId");
                    var txtTotalQuantity = (TextBox)item.FindControl("txtTotalQuantity");
                    var lbCurrency = (Label)item.FindControl("lbCurrency");
                    var hidCurrency = (HiddenField)item.FindControl("hidCurrency");
                    var txtExchangeRate = (TextBox)item.FindControl("txtExchangeRate");
                    var lbItemType = (Label)item.FindControl("lbItemType");
                    var hidItemType = (HiddenField)item.FindControl("hidItemType"); 
                    HiddenField HFPackagedRegulation = item.FindControl("HFPackagedRegulation") as HiddenField; 


                   // DataBindDDL(ddlCostCenter, CostCenters); old

                    lbItem.Text = Convert.ToString(index++);
                    if (row["ItemCode"].ToString().IsNotNullOrWhitespace())
                    {
                        var currencyItem = new ListItem(row["Currency"].AsString(), row["Currency"].AsString());
                        lbItemCode.Text = row["ItemCode"].ToString();
                        hidItemCode.Value = row["ItemCode"].ToString();
                        hidDesc.Value = row["Description"].ToString();
                        hidUnit.Value = row["Unit"].ToString();
                        hidVendor.Value = row["VendorName"].ToString();
                        hidDeliveryPeriod.Value = row["DeliveryPeriod"].ToString();
                        hidTaxValue.Value = row["TaxRate"].ToString();
                        hidAssetClass.Value = row["AssetClass"].ToString();
                        hidVendorId.Value = row["VendorID"].ToString();
                        HiddenFieldCostCenterID.Value = row["CostCenterID"].ToString();
                        HiddenFieldCostCenter.Value = row["CostCenter"].ToString() + " - " + row["CostCenterName"].ToString();
                        // ddlCostCenter.SelectedValue = row["CostCenterID"].ToString();"CostCenterName" old
                        //SetCostCenterValue(ddlCostCenter, row["CostCenter"].ToString(), row["CostCenterName"].ToString(), row["CostCenterID"].ToString());old
                        lbAssetClass.Text = row["AssetClass"].ToString();
                        lbDesc.Text = row["Description"].ToString();
                        hidRequestQuantity.Value = row["RequestQuantity"].ToString();
                        lbRequestQuantity.Text = row["RequestQuantity"].ToString();
                        txtTransQuantity.Text = row["TransQuantity"].ToString();
                        txtTotalQuantity.Text = row["TotalQuantity"].ToString();
                        hidRequestQuantity.Value = row["RequestQuantity"].ToString();
                        lbUnit.Text = row["Unit"].ToString();
                        lbVendor.Text = row["VendorName"].ToString();
                        txtUnitPrice.Text = row["UnitPrice"].ToString();
                        lbUnitPrice.Text = row["UnitPrice"].ToString();
                        lbTotalPrice.Text = row["TotalPrice"].ToString();
                        lbTaxValue.Text = row["TaxRate"].ToString();
                        lbDeliveryPeriod.Text = row["DeliveryPeriod"].ToString();
                        lbCurrency.Text = row["Currency"].AsString();
                        hidCurrency.Value = row["Currency"].AsString();
                        txtExchangeRate.Text = row["ExchangeRate"].ToString();
                        HFPackagedRegulation.Value = row["PackagedRegulation"].ToString();
                       
                        hlPhoto.NavigateUrl = "/WorkFlowCenter/ItemPic/" + row["ItemCode"].AsString() + ".jpg";
                        lbItemType.Text = row["ItemType"].ToString();
                        hidItemType.Value = row["ItemType"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 得到Repeater里的datatable数据集
        /// </summary>
        /// <returns></returns>
       internal DataTable dtGetPRData()
        {
            DataTable dtResul = CreateItemTable();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                HiddenField hidItem = (HiddenField)item.FindControl("hidItemCode");
                if (string.IsNullOrEmpty(hidItem.Value.Trim()))
                {
                    continue;
                }
                HiddenField hidDesc = (HiddenField)item.FindControl("hidDesc");
                HiddenField hidRequestQuantity = (HiddenField)item.FindControl("hidRequestQuantity");
                TextBox txtTransQuantity = (TextBox)item.FindControl("txtTransQuantity");
                HiddenField hidUnit = (HiddenField)item.FindControl("hidUnit");
                HiddenField hidVendor = (HiddenField)item.FindControl("hidVendor");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                Label lbTotalPrice = (Label)item.FindControl("lbTotalPrice");
                HiddenField hidDeliveryPeriod = (HiddenField)item.FindControl("hidDeliveryPeriod");
                HiddenField hidTaxValue = (HiddenField)item.FindControl("hidTaxValue");
                HiddenField HiddenFieldCostCenter = (HiddenField)item.FindControl("HiddenFieldCostCenter");
                HiddenField HiddenFieldCostCenterID = (HiddenField)item.FindControl("HiddenFieldCostCenterID");
                HiddenField hidAssetClass = (HiddenField)item.FindControl("hidAssetClass");
                HiddenField hidVendorId = (HiddenField)item.FindControl("hidVendorId");
                TextBox txtTotalQuantity = (TextBox)item.FindControl("txtTotalQuantity");
                HiddenField hidCurrency = (HiddenField)item.FindControl("hidCurrency");
                TextBox txtExchangeRate = (TextBox)item.FindControl("txtExchangeRate");
                HiddenField hidItemType = (HiddenField)item.FindControl("hidItemType");
                HiddenField HFPackagedRegulation = item.FindControl("HFPackagedRegulation") as HiddenField;

                if (IsNotNumberic(txtUnitPrice.Text))
                {
                    txtUnitPrice.Text = "";
                }
                if (IsNotNumberic(txtTotalQuantity.Text))
                {
                    txtTotalQuantity.Text = "";
                }
                if (IsNotNumberic(hidTaxValue.Value))
                {
                    hidTaxValue.Value = "";
                }
                DataRow row = dtResul.Rows.Add();
                row["Item"] = hidItem.Value.Trim();
                row["ItemCode"] = hidItem.Value.Trim();
                row["Description"] = hidDesc.Value.Trim();
                row["RequestQuantity"] = hidRequestQuantity.Value.Trim();
                row["TransQuantity"] = txtTransQuantity.Text.Trim();
                row["TotalQuantity"] = txtTotalQuantity.Text.Trim();
                row["Unit"] = hidUnit.Value.Trim();
                row["VendorID"] = hidVendorId.Value.Trim();
                row["VendorName"] = hidVendor.Value.Trim();
                row["UnitPrice"] = txtUnitPrice.Text.Trim();
                row["TotalPrice"] = txtTotalQuantity.Text.IsNullOrWhitespace() || txtUnitPrice.Text.IsNullOrWhitespace() ? 0 : Convert.ToDouble(txtUnitPrice.Text.Trim()) * Convert.ToDouble(txtTotalQuantity.Text.Trim());
                row["TaxRate"] = hidTaxValue.Value.Trim();
                row["DeliveryPeriod"] = hidDeliveryPeriod.Value.Trim();
                row["AssetClass"] = hidAssetClass.Value.Trim();
                row["CostCenterID"] = HiddenFieldCostCenterID.Value.Trim();
                string sCostCenter = HiddenFieldCostCenter.Value.Trim();
                string[] arrayCostCenter = sCostCenter.Split('-');
                if (arrayCostCenter.Length == 2)
                {
                    row["CostCenter"] = arrayCostCenter.Length == 2 ? arrayCostCenter[0].Trim() : sCostCenter;
                    row["CostCenterName"] = arrayCostCenter[1].Trim(); // by xu 
                }
                string sPackagedRegulation = HFPackagedRegulation.Value.Trim();
                if (sPackagedRegulation.Length > 0)
                {
                    row["PackagedRegulation"] = sPackagedRegulation;
                }
                else
                {
                    row["PackagedRegulation"] = null;
                }
                row["Currency"] = hidCurrency.Value.Trim();
                row["ExchangeRate"] = txtExchangeRate.Text.Trim();
                row["TaxValue"] = hidTaxValue.Value.IsNullOrWhitespace() ? 0 : Convert.ToDouble(row["TotalPrice"]) - (Convert.ToDouble(row["TotalPrice"]) / (Convert.ToDouble(row["TaxRate"]) + 1));
                row["ItemType"] = hidItemType.Value.Trim();
            }
            return dtResul;
        }

        internal void UpdateItemForQuartOrder()
        {
            foreach (RepeaterItem item in this.rptItem.Items)
            {
                HiddenField hidItem = (HiddenField)item.FindControl("hidItemCode");
                if (string.IsNullOrEmpty(hidItem.Value.Trim()))
                {
                    continue;
                }
                HiddenField hidDesc = (HiddenField)item.FindControl("hidDesc");
                HiddenField hidAssetClass = (HiddenField)item.FindControl("hidAssetClass");
                Label lbItemCode = (Label)item.FindControl("lbItemCode");
                Label lbDesc = (Label)item.FindControl("lbDesc");
                Label lbAssetClass = (Label)item.FindControl("lbAssetClass");

                if (lbItemCode.Text.Length == 0)
                {
                    lbItemCode.Text = hidItem.Value;
                }
                if (lbDesc.Text.Length == 0)
                {
                    lbDesc.Text = hidDesc.Value;
                }
                if (lbAssetClass.Text.Length == 0)
                {
                    lbAssetClass.Text = hidAssetClass.Value;
                }
            }
        }

        private DataTable CreateItemTable()
        {
            DataTable ItemTable = new DataTable();
            ItemTable.Columns.Add("Item");
            ItemTable.Columns.Add("ItemCode");
            ItemTable.Columns.Add("Description");
            ItemTable.Columns.Add("RequestQuantity");
            ItemTable.Columns.Add("TransQuantity");
            ItemTable.Columns.Add("TotalQuantity");
            ItemTable.Columns.Add("Unit");
            ItemTable.Columns.Add("VendorID");
            ItemTable.Columns.Add("VendorName");
            ItemTable.Columns.Add("UnitPrice");
            ItemTable.Columns.Add("TotalPrice");
            ItemTable.Columns.Add("DeliveryPeriod");
            ItemTable.Columns.Add("TaxRate");
            ItemTable.Columns.Add("TaxValue");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("CostCenterID");
            ItemTable.Columns.Add("AssetClass");
            ItemTable.Columns.Add("Currency");
            ItemTable.Columns.Add("ExchangeRate");
            ItemTable.Columns.Add("CostCenterName"); // by xu 
            ItemTable.Columns.Add("ItemType"); //Add 20111221 by toddy
            ItemTable.Columns.Add("PackagedRegulation"); // by xu 

            //ItemTable.Rows.Add();
            return ItemTable;
        }

        //获取申请单总价。正常情况下，商品价格为正，折扣价格为负，其他服务费为正。退货情况下，商品价格为负，折扣价格为正，其他服务费为正
        internal double GetTotalRMB(DataTable dtItemTable)
        {
            double tmpTotal = 0;
            double tmpCurrTotal = 0;
            //string discountItemCode = WorkflowItemCode.DISCOUNT;
            bool isReturn = (bool) this.ffIsReturn.Value;
            string currItemCode = string.Empty;
            string currDesc = string.Empty;

            foreach (DataRow row in dtItemTable.Rows)
            {
                tmpCurrTotal = Convert.ToDouble(row["TotalPrice"]) * Convert.ToDouble(row["ExchangeRate"]);

                currItemCode = row["ItemCode"].ToString().ToLower();
                currDesc = row["Description"].ToString().ToLower();
                if (currItemCode.StartsWith("x"))
                {
                    if (currDesc.Contains(WorkflowItemCode.DISCOUNT))
                    {
                        tmpTotal += isReturn ? tmpCurrTotal : -tmpCurrTotal;
                    }
                    else
                    {
                        tmpTotal += tmpCurrTotal;
                    }
                }
                else
                {
                    tmpTotal += isReturn ? -tmpCurrTotal : tmpCurrTotal;
                }
            }

            return tmpTotal;
        }

        //获取申请单审批价格。任何情况下都是绝对值相加。
        internal string GetApprovalTotalRMB(DataTable dtItemTable)
        {
            double tmpAbsTotal = 0;
            bool isReturn = (bool)this.ffIsReturn.Value;
            string currItemCode = string.Empty;
            string currDesc = string.Empty;
            double tmpCurrTotal = 0;

            foreach (DataRow row in dtItemTable.Rows)
            {
                currItemCode = row["ItemCode"].ToString().ToLower();
                currDesc = row["Description"].ToString().ToLower();
                tmpCurrTotal = Convert.ToDouble(row["TotalPrice"]) * Convert.ToDouble(row["ExchangeRate"]);
                if (currItemCode.StartsWith("x") && currDesc.Contains(WorkflowItemCode.DISCOUNT))
                {
                    tmpCurrTotal = -tmpCurrTotal;
                }

                tmpAbsTotal += tmpCurrTotal;
            }

            return tmpAbsTotal.ToString();
        }

        private void DataBindDDL(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "Display";
            ddl.DataValueField = "ID";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }

        public override bool Validate()
        {
            ValidateForSave();

            //门店不用输入退货PO编号，如果是HO必须输入
            if (isHO && Convert.ToBoolean(this.ffIsReturn.Value) && string.IsNullOrEmpty(this.ffPRNumber.Value.AsString()))
            {
                msg += "Please input PO numbers of goods to be returned.\\n";
            }

            if (string.IsNullOrEmpty(this.ffReason.Value.AsString()))
            {
                msg += "Please supply reason for request.\\n";
            }

            return msg.Length == 0;
        }

        internal bool ValidateForSave()
        {
            var currency = string.Empty;
            string itemType = string.Empty;
            int maintanceCount = 0;
            //Check the number type data & check the item type

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("CostCenter");
            int iItemCount = 0;
            for (int i = 0; i < rptItem.Items.Count; i++)
            {
                RepeaterItem item = rptItem.Items[i];

                HiddenField hidItemCode = (HiddenField)item.FindControl("hidItemCode");
                if (string.IsNullOrEmpty(hidItemCode.Value.Trim()))
                {
                    continue;
                }

                iItemCount++;
                HiddenField hidRequestQuantity = (HiddenField)item.FindControl("hidRequestQuantity");
                TextBox txtTransQuantity = (TextBox)item.FindControl("txtTransQuantity");
                TextBox txtTotalQuantity = (TextBox)item.FindControl("txtTotalQuantity");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                HiddenField hidVendorId = (HiddenField)item.FindControl("hidVendorId");//客户方出现过vendor为空的情况，导致产生PO时分组不正确
                string curItemType = ((HiddenField)item.FindControl("hidItemType")).Value.ToLower();
                //string scostCenter = (item.FindControl("ddlCostCenter") as DropDownList).SelectedItem.Text; old
                string scostCenter = (item.FindControl("HiddenFieldCostCenter") as HiddenField).Value;
                string sCostCenterID = (item.FindControl("HiddenFieldCostCenterID") as HiddenField).Value;
                if (IsNotNumberic(txtTotalQuantity.Text)
                    || IsNotNumberic(txtTransQuantity.Text)
                    || IsNotNumberic(hidRequestQuantity.Value)
                    || IsNotNumberic(txtUnitPrice.Text) 
                    || hidRequestQuantity.Value.Equals("0"))
                {
                    msg += "Please supply valid number.\\n";
                    break;
                }

                if (scostCenter.Trim().Length == 0 || sCostCenterID.Trim().Length == 0)
                {
                    msg += "Please select cost center for" + hidItemCode.Value + ".\\n";
                    break;
                }
                    
                if (hidItemCode.Value.IsNullOrWhitespace())
                {
                    msg += "Please select item code first.\\n";
                    break;
                }

                if (hidVendorId.Value.IsNullOrWhitespace())
                {
                    msg += "No available vendor. Please contact IT for further help.\\n";
                    break;
                }
                
                #region 验证Itemcode是否有重复数据
                DataRow dr = dt.NewRow();
                string sItemCode=hidItemCode.Value;
                string sCondition = string.Format("ItemCode='{0}' and CostCenter='{1}'",sItemCode, scostCenter);
                DataRow[] drArray= dt.Select(sCondition);
                if(drArray.Length>0)
                {

                    msg += string.Format("{0} has already selected {1},Please check the data.\\n", scostCenter, sItemCode);///"No available vendor. Please contact IT for further help.\\n";
                    break;
                }
                dr["ItemCode"] = sItemCode;
                dr["CostCenter"] =scostCenter;
                dt.Rows.Add(dr);
                #endregion

                maintanceCount += curItemType.Contains("maintenance") ? 1 : 0;
            }
            if (!isHO && maintanceCount != 0 && maintanceCount != iItemCount)
            {
                msg += "The request contains the items with maintenance and non-maintenance type.\\n";
            }

            return msg.Length == 0;
        }

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

        internal void Update()
        {
           // UpdateItem();xu
            dtGetPRData();
        }

        private Label GetLabelByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptItem.Items select row.FindControl(controlId) as Label).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private TextBox GetTextBoxByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptItem.Items select row.FindControl(controlId) as TextBox).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        private HyperLink GetHyperLinkByClientId(string clientId, string controlId)
        {
            return (from RepeaterItem row in rptItem.Items select row.FindControl(controlId) as HyperLink).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }

        /**
         * 上传Excel文件，利用Excel Service解析上传的文件并将文件内容显示到页面中
         */
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            bool isPaperBag = rbPRStorePurpose.Items[2].Selected;//by xu
            bool isHO = this.rbFormType.Items[1].Selected;
            string libraryName = WorkflowListName.WorkflowDocumentLibrary;
            string subFolder = WorkflowFolderName.PurchaseRequest;
            string colsKeyValue = string.Empty;
            string primaryKeyValue = string.Empty;
            string positionKeyValue = string.Empty;

            #region Set Configuration
       
            colsKeyValue = ExcelService.GetExcelConfigInfo(WorkflowConfigName.PRHOCol);
            primaryKeyValue = ExcelService.GetExcelConfigInfo(WorkflowConfigName.PRHOPK);
            positionKeyValue = ExcelService.GetExcelConfigInfo(WorkflowConfigName.PRHOPosition);
            if (string.IsNullOrEmpty(colsKeyValue))
            {
                DisplayMessage("The system can not find some neccessary configuration. Please contact IT for help.");
                return;
            }
            #endregion

            if (uploadAttachment.HasFile)
            {
                //get file extension
                string fileExt = Path.GetExtension(uploadAttachment.FileName).ToLower();

                if (fileExt == ".xlsx")
                {
                    DataTable excelData = null;
                    try
                    {

                        SPList list = SPContext.Current.Web.Lists[libraryName];
                        string filePath = uploadAttachment.PostedFile.FileName;

                        SPFile file = list.RootFolder.SubFolders[subFolder].Files.Add(uploadAttachment.FileName, uploadAttachment.PostedFile.InputStream, true);

                        var fileName = file.Name;
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                        char[] split = new char[] { '_' };
                        var sheetNames = fileName.Split(split);
                        if (sheetNames.Length > 1)
                        {
                            excelData = ExcelService.ParseExcel(file, positionKeyValue, primaryKeyValue, colsKeyValue, sheetNames[1]);
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtil.logError("Upload or read file fail, error:" + ex.Message);
                        throw ex;
                    }

                    DateTime dtBeginCheck = DateTime.Now;

                    //如果解析出来的Excel文件正确，则将excel中内容显示到页面中
                    if (excelData != null && excelData.Rows.Count > 0)
                    {
                        excelData.Columns[0].ColumnName = "ItemCode";
                        excelData.Columns[1].ColumnName = "TotalQuantity";
                        excelData.Columns[2].ColumnName = "CostCenter";

                       // this.ItemTable.Rows.Clear(); by xu
                        DataTable dtItemTable = CreateItemTable();
                       /* List<string> itemTitles = new List<string>();
                        foreach (DataRow dr in excelData.Rows)
                        {
                            itemTitles.Add(dr["ItemCode"].ToString());
                        }*/
                        DataTable dtItemCodes = PurchaseRequestCommon.GetActiveItemCode();// GetItemCodeByTitle(itemTitles.ToArray());
                        DataTable dtVendors = PurchaseRequestCommon.GetVendorFromCache();//.GetVendorDT();
                        DataRow[] tmpItems = null;
                        DataRow currDataRow = null;
                        DataRow[] tmpVendors = null;
                        DataRow currVendor = null;
                        DataRow[] tmpCostCenters = null;
                        DataRow currCostCenter = null;
                        StringBuilder missItems = new StringBuilder();
                        StringBuilder missVendors = new StringBuilder();
                        StringBuilder missCostCenters = new StringBuilder();
                        StringBuilder missQuantities = new StringBuilder();
                        int count = 0;

                        string _itemCode = string.Empty;
                        string _costCenter = string.Empty;
                        string _totalQuantity = string.Empty;

                        DataTable CostCentdt = dtCostCenter();
                        foreach (DataRow dr in excelData.Rows)
                        {
                            _itemCode = dr["ItemCode"].AsString().Trim();
                            _costCenter = dr["CostCenter"].AsString().Trim();
                            _totalQuantity = dr["TotalQuantity"].AsString().Trim();

                            if (!isHO)//是Store用户  验证costcenter
                            {
                                bool bCheckCostcenter = CheckCostCenter(_costCenter, CostCentdt);
                                if (!bCheckCostcenter)//  CostCenter不属于当前的门店
                                {
                                    missCostCenters.Append(string.Format("{0} inavaliable !Please check data\\n", _costCenter));
                                    continue;
                                }
                            }

                            string sItemType = string.Empty;
                            bool isCheck = CheckItemCode(_itemCode, out sItemType);//验证ItemCode是否合法
                            if (!isCheck)
                            {
                                missItems.Append(string.Format("{0} inavaliable !Please check data\\n", _itemCode));
                                continue;
                            }

                            if (IsNotNumberic(_totalQuantity))
                            {
                                missQuantities.Append(string.Format("{0}:Error quantity {1}\\n",_itemCode, _totalQuantity));
                                continue;
                            }

                            tmpItems = dtItemCodes.Select("Title='" + _itemCode + "'");
                            if (tmpItems == null || tmpItems.Length == 0)
                            {
                                missItems.Append(string.Format("Itemcode {0} does not exist\\n", _itemCode));
                                continue;
                            }
                            currDataRow = tmpItems[0];

                            tmpVendors = dtVendors.Select("VendorId='" + currDataRow["VendorId"].AsString() + "'");
                            if (tmpVendors == null || tmpVendors.Length == 0)
                            {
                                missVendors.Append(string.Format("VendorID {0} does not exist for {1}\\n", currDataRow["VendorId"].AsString(), _itemCode));
                                continue;
                            }
                            currVendor = tmpVendors[0];

                            tmpCostCenters = CostCentdt.Select("Title='" + _costCenter + "'");
                            if (tmpCostCenters == null || tmpCostCenters.Length == 0)
                            {
                                missCostCenters.Append(string.Format("CostCenter {0} does not exist\\n", _costCenter));
                                continue;
                            }
                            currCostCenter = tmpCostCenters[0];

                            string sItemCode = currDataRow["Title"].AsString();

                            DataRow row = dtItemTable.Rows.Add();


                            row["Item"] = sItemCode;// currDataRow["Title"].AsString();
                            row["ItemCode"] = sItemCode;// currDataRow["Title"].AsString();
                            row["Description"] = currDataRow["Description"].AsString();
                            row["RequestQuantity"] = dr["TotalQuantity"].AsString().Trim();
                            row["TransQuantity"] = 0;
                            row["TotalQuantity"] = dr["TotalQuantity"].AsString().Trim();
                            row["Unit"] = currDataRow["Unit"].AsString();
                            row["VendorID"] = currDataRow["VendorID"].AsString();
                            row["VendorName"] = currVendor["Title"].AsString();
                            row["UnitPrice"] = currDataRow["UnitPrice"].AsString();

                            double total = Convert.ToDouble(row["UnitPrice"].ToString()) * Convert.ToDouble(row["TotalQuantity"].ToString());
                            row["TotalPrice"] = Math.Round(total, 2);
                            row["TaxRate"] = currDataRow["TaxValue"].AsString();
                            row["DeliveryPeriod"] = currDataRow["DeliveryPeriod"].AsString();
                            row["CostCenter"] = _costCenter;
                            row["AssetClass"] = currDataRow["AssetClass"].AsString();
                            row["CostCenterID"] = currCostCenter["ID"].AsString();
                            row["Currency"] = currDataRow["Currency"].AsString();
                            row["ExchangeRate"] = 1;
                            row["TaxValue"] = total - (total / (Convert.ToDouble(row["TaxRate"]) + 1));
                            row["ItemType"] = sItemType;
                            row["PackagedRegulation"] = currDataRow["PackagedRegulation"].AsString();
                            
                            count++;
                        }

                        UploadMsg = string.Format("Upload Finished.\\nTotal: {0}, Success: {1}, Error: {2}", excelData.Rows.Count, count, excelData.Rows.Count - count);
                        PrintErrorMsg(missItems.ToString(), missVendors.ToString(), missCostCenters.ToString(), missQuantities.ToString());

                        this.rptItem.DataSource = dtItemTable;
                        this.rptItem.DataBind();


                    }
                }
                else
                {
                    DisplayMessage("Please upload excel 2007 format document(.xlsx).");
                }
            }
        }

        /**
         * Record the error msg to system log file
         */
        private void PrintErrorMsg(string forItems, string forVendors, string forCostCenters, string forQuantities)
        {
            StringBuilder error = new StringBuilder();
            if (forItems.Length > 0)
            {
                error.Append(string.Format("Error when selecting the item table by given item code\\n{0}\\n\\n", forItems));
            }
            if (forVendors.Length > 0)
            {
                error.Append(string.Format("Error when selecting the vendor table by given item code\\n{0}\\n\\n", forVendors));
            }
            if (forCostCenters.Length > 0)
            {
                error.Append(string.Format("Error when selecting the cost center table by given costcenter\\n{0}\\n\\n", forCostCenters));
            }
            if (forQuantities.Length > 0)
            {
                error.Append(string.Format("Error for the quantity format \\n{0}\\n\\n", forQuantities));
            }

            if (error.Length > 0)
            {
                DisplayMessage(error.ToString().Replace("\'", ","));
            }
        }

        private void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

       /// <summary>
        /// 找不到CostCenter,就将其加入其中，并设为默认值
       /// </summary>
       /// <param name="ddl"></param>
       /// <param name="sCostName"></param>
       /// <param name="sCostCenterName"></param>
       /// <param name="sID"></param>
        void SetCostCenterValue(DropDownList ddl, string sCostName, string sCostCenterName, string sID)
        {
            ListItem li = new ListItem(string.Concat(sCostName, " - ", sCostCenterName), sID);
            if( ddl.Items.FindByValue(sID)==null)
            {
                ddl.Items.Add(li);
                ddl.SelectedValue = sID;
            }
            else
            {
                ddl.SelectedValue = sID;
            }
        }

        /// <summary>
        /// 验证Item类型是否合法。
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns></returns>
        bool CheckItemCode(string sItemCode,out string sItemCodeType)
        {
            bool isOK = false;
            string sReturnItemCodeType = string.Empty;
            string sFormType = rbFormType.SelectedValue;//Store,HO
            string sRequestType = rbRequestType.SelectedValue;//Opex,Capex
            if (sRequestType.Equals("Opex", StringComparison.InvariantCultureIgnoreCase))
            {
                isOK = sItemCode.StartsWith("E", StringComparison.InvariantCultureIgnoreCase) || sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase);
            }
            else if (sRequestType.Equals("Capex", StringComparison.InvariantCultureIgnoreCase))
            {
                isOK = sItemCode.StartsWith("C", StringComparison.InvariantCultureIgnoreCase) || sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase);
            }
            sItemCodeType = string.Empty;
            if (!isOK)//验证不通过。
            {
                return isOK;
            }
            ///Store用户就在验证其ItemScope的类型 
            string sItemType = rbPRStorePurpose.SelectedValue;//Daily,QuarterlyOrder,PaperBag
            string sItemScope = string.Empty;
            sItemScope = sGetItemScope(sItemCode, out sReturnItemCodeType);
            if (sFormType.Equals("Store"))
            {
                switch (sItemType)
                {
                    case "Daily": isOK = (!sItemScope.Equals("QO", StringComparison.InvariantCultureIgnoreCase) && !sItemScope.Equals("PB", StringComparison.InvariantCultureIgnoreCase));
                        break;
                    case "QuarterlyOrder": isOK = sItemScope.Equals("QO", StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "PaperBag": isOK = sItemScope.Equals("PB", StringComparison.InvariantCultureIgnoreCase);
                        break;
                }
            }
            sItemCodeType = sReturnItemCodeType;
            return isOK;
        }

        /// <summary>
        /// 得到Item的ItemScope
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="sItemType"></param>
        /// <returns></returns>
        string sGetItemScope(string sItemCode,out string sItemType)
        {
            string sIemScope = string.Empty;
            string sResultItemType = string.Empty;
            DataTable dt = PurchaseRequestCommon.GetActiveItemCode();
            DataRow[] drArray= dt.Select("Title='" + sItemCode + "'");
            if (null != drArray && drArray.Count() > 0)
            {
                sResultItemType = drArray[0]["ItemType"] == null ? string.Empty : drArray[0]["ItemType"].ToString().Trim();
                sIemScope = drArray[0]["ItemScope"] == null ? string.Empty : drArray[0]["ItemScope"].ToString().Trim();
            }
             
            sItemType = sResultItemType;
            return sIemScope;
        }


        /// <summary>
        /// 验证CostCenter
        /// </summary>
        /// <param name="sCostCenter"></param>
        /// <returns></returns>
        bool CheckCostCenter(string sCostCenter,DataTable dtCostCenter)
        {
            bool isOK = false;
            string sCondition = string.Format("Title='{0}'",sCostCenter);
            DataRow[] drArray = dtCostCenter.Select(sCondition);
            if (null != drArray || drArray.Count() > 0)
            {
                isOK = true;
            }
            return isOK;
        }

        /// <summary>
        /// /默认在页面上一次加载多条数据。默认在页面上一次加载多条数据。
        /// </summary>
        /// <returns></returns>
        DataTable SetRowLimit(DataTable dt)
        {
           // DataTable dt = CreateItemTable();
            int iRowCount = PurchaseRequestCommon.GetPRItemLoadCount();
            for (int i = 0; i < iRowCount; i++)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn item in dt.Columns)
                {
                    dr[item] = DBNull.Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        //加载Costcenter
        void BindCostCenter()
        {
            DropDownListCostCenter.DataSource = dtCostCenter();/// CostCenters;
            DropDownListCostCenter.DataBind();

            DropDownListCostCenter.DataTextField = "Display";
            DropDownListCostCenter.DataValueField = "ID";
            DropDownListCostCenter.DataBind();
            DropDownListCostCenter.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }

        /// <summary>
        /// 设置是否可选纸袋
        /// </summary>
        void SetPaperBagEnable()
        {
            rbPRStorePurpose.Items[2].Enabled = false;
            string sAllowPaperBagDate = PurchaseRequestCommon.GetPaperBagConfig();
            if (sAllowPaperBagDate.Length > 0)
            {
                string[] sArrayDate = sAllowPaperBagDate.Split(',');
                foreach (string SDate in sArrayDate)
                {
                    if (isAllowPaperBag(SDate))
                    {
                        rbPRStorePurpose.Items[2].Enabled = true;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 当前日期是否能可选纸袋订单
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        bool isAllowPaperBag(string sDate)//6-10
        {
            bool isOK=false;
            string[] sArrayDate= sDate.Split('-');
            if (sArrayDate.Length != 2)
            {
                return isOK;
            }
            int iBegin = 0;
            int iEnd = 0;
            if (int.TryParse(sArrayDate[0], out iBegin) && int.TryParse(sArrayDate[1], out iEnd) && iBegin <= iEnd)
            {
                int iDay=DateTime.Now.Day;
                if (iBegin <= iDay && iDay <= iEnd)
                {
                    isOK = true;
                }
            }
            return isOK;
        }
        /// <summary>
        /// 添加行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonAddItem_Click(object sender, EventArgs e)
        {
            DataTable dtPRData= dtGetPRData();//页面上的Repeater里的数据集
            DataTable dt= SetRowLimit(dtPRData);//加上空行数的数据集
            rptItem.DataSource = dt;
            rptItem.DataBind();
        }

    }
}
