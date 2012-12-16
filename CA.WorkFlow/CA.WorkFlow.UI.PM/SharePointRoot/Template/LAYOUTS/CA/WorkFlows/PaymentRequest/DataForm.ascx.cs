namespace CA.WorkFlow.UI.PaymentRequest
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
    using System.ComponentModel;
    using QuickFlow.Core;
    using QuickFlow;
    using CodeArt.SharePoint.CamlQuery;
    using System.Configuration;
    using System.Collections;

    public partial class DataForm : PaymentRequestControl
    {
        #region update by YG

        public string ChooseEmployeeStatus
        {
            set
            {
                this.ChooseEmployee.Style.Add("display", value); 
            }
        }

        public SPUser ApplicantSPUser
        {
            get;
            set;
        }
        public SPFieldUserValueCollection ApproversSPUser
        {
            get;
            set;
        }
        public PaymentRequestMode PRMode
        {
            get;
            set;
        }
        /// <summary>
        /// 记录已经付款的百分比
        /// </summary>
        private decimal mPaidBefore = 0;

        private bool mIsNeedSystemGR = false;
        private string mPRNO = string.Empty;
        private string mPONO = string.Empty;
        private string mSubPRNO = string.Empty;
        /// <summary>
        /// 是否需要系统收货后才能付款
        /// </summary>
        public bool IsNeedSystemGR
        {
            get
            {
                return mIsNeedSystemGR;
            }
            set
            {
                mIsNeedSystemGR = value;
            }
        }
        public string ErrorMessage
        {
            get;
            set;
        }
        private string fromPO;
        public string FromPO
        {
            get { return fromPO; }
            set { fromPO = value; }
        }
        private bool? mIsFromPO;
        /// <summary>
        /// 当前付款申请是否来自PO单进入付款
        /// </summary>
        public bool? IsFromPO
        {
            get
            {
                if (mIsFromPO != null)
                {
                    return mIsFromPO;
                }

                bool isFromPO = false;
                if (ViewState["PrDict"] != null)
                {
                    Dictionary<string, string> dict = (Dictionary<string, string>)ViewState["PrDict"];
                    isFromPO = dict["IsFromPO"].ToString() == "1" ? true : false;
                }
                else
                {
                    //当IsFromPO不为空，来自PO ,否则当前申请是"图标进入"
                    if (Request.QueryString["IsFromPO"] != null)
                    {
                        isFromPO = true;
                    }
                }
                return isFromPO;
            }
            set
            {
                mIsFromPO = value;
            }
        }
        /// <summary>
        /// 是否已做收货
        /// </summary>
        public bool IsSystemGR
        {
            get
            {
                return radioSystemGR.SelectedIndex == 0;
            }
        }
        /// <summary>
        /// 是否签署合同
        /// </summary>
        public bool IsContractPO
        {
            get { return radioContractPO.SelectedIndex == 0; }
        }
        /// <summary>
        /// 是否让按钮失效
        /// </summary>
        public bool? IsDisableControl
        {
            get
            {
                return IsFromPO;
            }
        }
        public string Manager
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        public string SubPRNO
        {
            get
            {
                return mSubPRNO;
            }
            set
            {
                mSubPRNO = value;
            }
        }
        public string PRNO
        {
            get { return mPRNO; }
            set
            {
                mPRNO = value;
            }
        }
        /// <summary>
        /// PO 编号
        /// </summary>
        public string PONO
        {
            get { return mPONO; }
            set
            {
                mPONO = value;
                if (ViewState["PrDict"] == null)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("PONO", value);
                    dict.Add("PRNO", PRNO);
                    dict.Add("IsFromPO", IsFromPO == true ? "1" : "0");
                    ViewState["PrDict"] = dict;
                }
            }
        }
        /// <summary>
        /// 第几次付款
        /// </summary>
        public string PaidIndex
        {
            get
            {
                if (ViewState["PrDict"] != null)
                {
                    Dictionary<string, string> dict = ViewState["PrDict"] != null ? (
                    Dictionary<string, string>)ViewState["PrDict"] : new Dictionary<string, string>();

                    if (dict.ContainsKey("PIIndex") == true)
                    {
                        return dict["PIIndex"];
                    }
                }

                return "1";
            }
        }
        public string Applicant
        {
            get;
            set;
        }
        public string Approvers
        {
            get;
            set;
        }
        private string requestId;
        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }
        /// <summary>
        /// 上一次支付百分比
        /// </summary>
        private double PaidBefore
        {
            get;
            set;
        }
        /// <summary>
        /// 本次支付百分比
        /// </summary>
        private double PaidThisTime
        {
            get;
            set;
        }
        private double PaidTotalAmountThisTime
        {
            get;
            set;
        }
        /// <summary>
        /// 剩余百分比
        /// </summary>
        private double Balance
        {
            get;
            set;
        }
        public string RootURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["rootweburl"];
            }
        }

        public string GRStatus
        {
            set
            {
                this.hfgr.Value = value;
            }
            get
            {
                return this.hfgr.Value;
            }
        }
        public Employee ApplicantEmployee
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

        public string VendorNameText 
        {
            get
            {
                //return this.txtVenderName.Text.Trim().Length > 5
                //      ? this.txtVenderName.Text.Trim().Substring(0, 5) + "..." 
                //      : this.txtVenderName.Text.Trim();
                return this.txtVenderName.Text.Trim();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //定义字典对象用来存储PR申请需要的数据，如：PONO、PRNO、SubPRNO
                Dictionary<string, string> dict = new Dictionary<string, string>();
                DataTable vendTable = null, piTable = null, prTable = null;
                DataTable poTable = new DataTable();
                if (GetPurchaseOrder(ref poTable) == false)
                {
                    TransferPage(" 本次付款必须做完System PO后才能继续进行 ");
                    return;
                }

                if (Request.QueryString["PONO"] != null)
                {
                    string poId = Request.QueryString["PONO"].ToString();
                    piTable = GetInstallmentInfo(poId);
                    //如果piTable为空，则说明此次付款申请已经完成
                    if (piTable == null)
                    {
                        TransferPage(" 这张订单的付款已经全部完成 ");
                        return;
                    }
                    prTable = GetLastPaymentRequestInfo(poId);
                    if (prTable != null && prTable.Rows.Count > 0)
                    {
                        if (piTable != null)
                        {
                            //判断上一次付款申请是否已经完成
                            if (prTable.Rows[0]["PaidInd"].ToString() == piTable.Rows[0]["PaidInd"].ToString())
                            {
                                TransferPage("上一次付款还未完成，请先完成上次付款再进行本次付款申请 ");
                                return;
                            }
                        }

                        vendTable = prTable;
                        poTable = IsFromPO == false ? prTable : poTable;
                        PaidBefore = double.Parse(prTable.Rows[0]["PaidThisTime"].ToString());
                        requestId = prTable.Rows[0]["SubPRNo"].ToString();
                        dict.Add("IsFromPO", prTable.Rows[0]["IsFromPO"].ToString());
                        dict.Add("PONO", prTable.Rows[0]["PONo"].ToString());
                        dict.Add("PRNO", prTable.Rows[0]["PRNo"].ToString());
                        dict.Add("PIIndex", (int.Parse(prTable.Rows[0]["PaidInd"].ToString()) + 1).ToString());
                        ViewState["PrDict"] = dict;
                    }
                    else
                    {
                        CreateWorkFlowNumber();
                    }

                    if (vendTable == null)
                    {
                        vendTable = PaymentRequestComm.GetVendorInfo(poTable.Rows[0]["VendorNo"].ToString()).GetDataTable();
                    }

                    this.cpfUser.CommaSeparatedAccounts = this.ApplicantEmployee.UserAccount;
                }
                else
                {
                    //当SUBPRNO不为空的时候，处在编辑状态
                    if (string.IsNullOrEmpty(SubPRNO) == false)
                    {
                        this.lblCurrency.Text = WorkflowContext.Current.DataFields["Currency"].AsString() == "" ? "RMB" : WorkflowContext.Current.DataFields["Currency"].AsString();
                        this.cpfUser.CommaSeparatedAccounts = this.ApplicantEmployee.UserAccount;

                        string subPRId = SubPRNO != null ? SubPRNO : Request.QueryString["SUBPRNO"].ToString();
                        prTable = PaymentRequestComm.GetPaymentRequestItemsInfoBySUBPRNO(subPRId).GetDataTable();
                        piTable = GetInstallmentInfo(prTable.Rows[0]["PONo"].ToString());
                        //vendTable = piTable = prTable;
                        vendTable = prTable;
                        dict = ViewState["PrDict"] != null ? (Dictionary<string, string>)ViewState["PrDict"] : new Dictionary<string, string>();
                        if (dict.ContainsKey("PIIndex") == false)
                            dict.Add("PIIndex", prTable.Rows[0]["PaidInd"].ToString());
                        ViewState["PrDict"] = dict;

                        if (prTable.Rows[0]["PaidInd"].ToString() == "1" && (mIsFromPO == false || mIsFromPO == null))
                        {
                            BindClientEvent();
                        }
                    }
                    else
                    {
                        if (mIsFromPO == false || mIsFromPO == null)
                        {
                            BindClientEvent();
                            this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();
                        }
                    }
                }

                if (CopyStatus.Equals("Copy", StringComparison.CurrentCultureIgnoreCase))
                {
                    BindPaymentRequestDataByWorkFlowNumber(WorkFlowNumber);
                }
                else
                {
                    FillEmployeeInfo();
                    FillPurchaseOrderInfo(poTable);
                    LoadCountryCurrency();
                    FillVendorInfo(vendTable);
                    FillPaymentRequestInfo(prTable);
                    FillPeymentInstallmentInfo(piTable);
                    FillCostCenterInfo(prTable);
                    SetControlStatus();
                    GetGLAccountDataTable();
                }
                this.cpfUser.Load += new EventHandler(cpfUser_Load);
            }
        }

        private void FillCostCenterInfo(DataTable prTable)
        {
            FromPO = "";
            if (PaidIndex.IsNotNullOrWhitespace() == true && int.Parse(PaidIndex) > 1)
            {
                requestId = prTable.Rows[0]["SubPRNo"].ToString();
                FromPO = "FromPO";
                this.hfFromPO.Value = "FromPO";
            }
            else
            {
                if (IsFromPO == true)
                {
                    FromPO = "FromPO";
                    this.hfFromPO.Value = "FromPO";
                }
            }
            //add by lj 2012.2.23
            BindCostCenterAndExpenseType();

            //if (IsFromPO == true && (PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.Edit)
            if ((IsFromPO == false && PaidIndex.IsNotNullOrWhitespace() == true && int.Parse(PaidIndex) > 1)
               || (IsFromPO == true && (PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.Edit))

            {
                DataTable itemDetails = PaymentRequestComm.GetDataTable(requestId);
                //如果图标第二次过来的话，需要替换金额数据，引用第一次数据
                if ((PaymentRequestMode)Session["PRMode"] != PaymentRequestMode.Edit)
                {
                    double d = PaidThisTime / PaidBefore;
                    if (int.Parse(PaidIndex) == 2)
                        d = PaidThisTime / 100;

                    //PaidTotalAmountThisTime
                    double amount = 0;
                    for (int i = 0; i < itemDetails.Rows.Count; i++)
                    {
                        if (i + 1 == itemDetails.Rows.Count)
                        {
                            itemDetails.Rows[i]["ItemAmount"] = Math.Round(PaidTotalAmountThisTime - amount, 2);
                        }
                        else
                        {
                            itemDetails.Rows[i]["ItemAmount"] = Math.Round(double.Parse(itemDetails.Rows[i]["ItemAmount"].ToString()) * d, 2);
                            amount += double.Parse(itemDetails.Rows[i]["ItemAmount"].ToString());
                        }
                    }
                }

                this.rptItem.DataSource = itemDetails;
                this.rptItem.DataBind();
            }
            else
            {
                DataBindDataTable();
            }
            if (FromPO != null && FromPO != "")
            {
                this.hfpostatus.Value = "1";
            }
        }

        private void BindClientEvent()
        {
            txtPaidThisTime.Attributes.Add("onclick", "OpenInstallmentDialog();");
            txtTotalAmount.Attributes.Add("onclick", "OpenInstallmentDialog();");
            txtTotalAmount1.Attributes.Add("onclick", "OpenInstallmentDialog();");
        }

        /// <summary>
        /// 获取PO单详细信息
        /// </summary>
        /// <param name="poTable"></param>
        /// <returns>是否已经做完System PO</returns>
        private bool GetPurchaseOrder(ref DataTable poTable)
        {
            if (IsFromPO == true)
            {
                string poNO = (string.IsNullOrEmpty(PONO) == false ? PONO : Request.QueryString["PONO"].ToString());
                poTable = PaymentRequestComm.GetPruchaseOrderInfo(poNO).GetDataTable();

                if (poTable != null && poTable.Rows.Count > 0)
                {
                    if (poTable.Rows[0]["SapNO"].ToString().IsNullOrWhitespace() == true)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetControlStatus()
        {
            radioInstallment.Enabled = !(IsFromPO == null ? false : (bool)IsFromPO);
            radioContractPO.Enabled = !(IsFromPO == null ? false : (bool)IsFromPO);
            radioSystemPO.Enabled = !(IsFromPO == null ? false : (bool)IsFromPO);
            radioNeedGR.Enabled = !(IsFromPO == null ? false : (bool)IsFromPO);
            radioSystemGR.Enabled = !(IsFromPO == null ? false : (bool)IsFromPO);
            txtContractPO.ReadOnly = (IsFromPO == null ? false : (bool)IsFromPO);
            txtSystemPO.ReadOnly = (IsFromPO == null ? false : (bool)IsFromPO);

            int paidIndex = 1;
            if (ViewState["PrDict"] != null)
            {
                Dictionary<string, string> dict = ViewState["PrDict"] != null ? (
                Dictionary<string, string>)ViewState["PrDict"] : new Dictionary<string, string>();
                if (dict.ContainsKey("PIIndex") == true)
                {
                    paidIndex = int.Parse(dict["PIIndex"]);
                }
            }

            if (IsFromPO == true || (paidIndex > 1))
            {
                txtTotalAmount.ReadOnly = true;
                txtPaidThisTime.ReadOnly = true;
            }

            if (ViewState["PrDict"] != null)
            {
                Dictionary<string, string> dict = (Dictionary<string, string>)ViewState["PrDict"];
                if (int.Parse(dict["PIIndex"]) > 1)
                {
                    txtVenderCode.ReadOnly = true;
                    txtVenderName.ReadOnly = true;
                    radioInstallment.Enabled = false;
                    if (txtContractPO.Text.IsNullOrWhitespace() == false)
                    {
                        radioContractPO.Enabled = false;
                        txtContractPO.ReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dTable"></param>
        private void FillPurchaseOrderInfo(DataTable dTable)
        {
            if (dTable != null && dTable.Rows.Count > 0)
            {
                if (IsFromPO == true)
                {
                    txtContractPO.Text = dTable.Rows[0]["PONumber"].ToString();
                    txtSystemPO.Text = dTable.Rows[0]["SapNO"].ToString();
                    radioSystemPO.SelectedIndex = dTable.Rows[0]["SapNO"].ToString().IsNullOrWhitespace() ? 1 : 0;
                    radioContractPO.SelectedIndex = dTable.Rows[0]["PONumber"].ToString().IsNullOrWhitespace() ? 1 : 0;
                    radioNeedGR.SelectedIndex = 0;

                    DataTable dt = PaymentRequestComm.GetPurchaseOrderWorkflowInfo(dTable.Rows[0]["PONumber"].ToString()).GetDataTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string s = dt.Rows[0]["IsSystemGR"].ToString();
                        if (s == "0" || s.IsNullOrWhitespace())
                            radioSystemGR.SelectedIndex = 1;
                        else
                            radioSystemGR.SelectedIndex = 0;
                    }
                }
                else
                {
                    txtContractPO.Text = dTable.Rows[0]["PONo"].ToString();
                    txtSystemPO.Text = dTable.Rows[0]["SystemPONo"].ToString();
                    radioSystemPO.SelectedIndex = dTable.Rows[0]["SystemPONo"].ToString().IsNullOrWhitespace() ? 1 : 0;
                    radioContractPO.SelectedIndex = dTable.Rows[0]["PONo"].ToString().IsNullOrWhitespace() ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// 填充供应商信息
        /// </summary>
        private void FillVendorInfo(DataTable dTable)
        {
            if (dTable != null)
            {
                txtVenderCode.Text = dTable.Columns.Contains("VendorNo") ? dTable.Rows[0]["VendorNo"].ToString() :
                    dTable.Rows[0]["VendorId"].ToString();
                txtVenderName.Text = dTable.Columns.Contains("VendorName") ? dTable.Rows[0]["VendorName"].ToString() :
                        dTable.Rows[0]["Title"].ToString();
                txtBankName.Text = dTable.Rows[0]["BankName"].ToString();
                txtBankAC.Text = dTable.Rows[0]["BankAccount"].ToString();
                txtSwiftCode.Text = dTable.Rows[0]["SwiftCode"].ToString();
                if ((PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.Edit)
                {
                    WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                    this.txtVendorCity.Text = fields["VendorCity"].AsString();
                    ddlVendorCountry.SelectedValue = fields["VendorCountry"].AsString();
                    ddlBankCountry.SelectedValue = fields["BankCity"].AsString();
                }
            }
        }

        /// <summary>
        /// 填充用户信息
        /// </summary>
        private void FillEmployeeInfo()
        {
            Employee emp = UserProfileUtil.GetEmployeeEx(SPContext.Current.Web.CurrentUser.LoginName);
            txtApplicant.Text = emp.DisplayName;
            txtDept.Text = emp.Department;
        }

        /// <summary>
        /// 根据PO号获取 Cost Center 数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetCostCenter()
        {
            //DataTable poItemsTable = PaymentRequestComm.GetPurchaseOrderItemsInfo(Request.QueryString["PONO"].ToString());
            DataTable poItemsTable = PaymentRequestComm.GetPurchaseOrderItemsInfo(Request.QueryString["PONO"].ToString()).GetDataTable();
            var itemsTable = from poItems in poItemsTable.AsEnumerable()
                             where poItems.Field<string>("ItemCode").Substring(0, 1) != "X"
                             group poItems by new
                             {
                                 CostCenter = poItems.Field<string>("CostCenter"),
                                 GLAccount = poItems.Field<string>("AssetClass"),
                                 ACNumber = poItems.Field<string>("ACNumber")
                             } into items
                             select new
                             {
                                 GLAccount = items.Key.GLAccount,
                                 CostCenter = items.Key.CostCenter,
                                 ACNumber = items.Key.ACNumber,
                                 //  OpexTotalAmount = items.Sum(poItems => poItems.Field<double>("TotalPrice")),
                                 TotalAmount = items.Sum(poItems => poItems.Field<double>("AllocatedValue")),
                                 TaxTotalAmount = items.Sum(poItems => poItems.Field<double>("TaxValue"))
                             };

            var itemsTable1 = from poItems in poItemsTable.AsEnumerable()
                              where poItems.Field<string>("ItemCode").Substring(0, 1) == "X"
                              select new
                              {
                                  CostCenter = poItems.Field<string>("CostCenter"),
                                  GLAccount = poItems.Field<string>("AssetClass"),
                                  ACNumber = poItems.Field<string>("ACNumber"),
                                  TaxAmount = poItems.Field<double>("TaxValue") *
                                  (poItems.Field<string>("Description").ToString().Contains("discount") ? -1 : 1),
                                  OpexTotalAmount = poItems.Field<double>("TotalPrice") *
                                  (poItems.Field<string>("Description").ToString().Contains("discount") ? -1 : 1)
                              };

            this.ItemTable.Rows.Clear();
            Hashtable ht = GetExpenseTypeHashtable();
            foreach (var item in itemsTable)
            {
                DataRow row = this.ItemTable.Rows.Add();
                row["CostCenter"] = item.CostCenter;
                row["GLAccount"] = item.GLAccount;
                row["FANO"] = item.ACNumber;
                row["ExpenseType"] = ht[item.GLAccount] != null ? ht[item.GLAccount].ToString() : "";
                row["ItemAmount"] = Math.Round(item.TotalAmount * PaidThisTime / 100, 2);
                TaxTotalAmount += Math.Round(item.TaxTotalAmount * PaidThisTime / 100, 2);
                //  TotalAmount += Math.Round((Opex_Capex_Status == OpexCapexStatus.Opex ? item.OpexTotalAmount - item.TaxTotalAmount : item.TotalAmount) * PaidThisTime / 100, 2);

                TotalAmount += Math.Round(item.TotalAmount * PaidThisTime / 100, 2);
            }
            foreach (var item in itemsTable1)
            {
                //DataRow row = this.ItemTable.Rows.Add();
                //row["CostCenter"] = item.CostCenter;
                //row["GLAccount"] = item.GLAccount;
                //row["FANO"] = item.ACNumber;
                //row["ExpenseType"] = ht[item.GLAccount] != null ? ht[item.GLAccount].ToString() : "";
                //row["ItemAmount"] = Math.Round((item.OpexTotalAmount - item.TaxAmount) * PaidThisTime / 100, 2);

                TaxTotalAmount += Math.Round(item.TaxAmount * PaidThisTime / 100, 2);
                //if (Opex_Capex_Status == OpexCapexStatus.Opex)
                //    TotalAmount += Math.Round((item.OpexTotalAmount - item.TaxAmount) * PaidThisTime / 100, 2);
            }

            DataRow taxRow = this.ItemTable.Rows.Add();
            taxRow["ExpenseType"] = "Tax payable - VAT input";
            taxRow["CostCenter"] = "";
            taxRow["GLAccount"] = this.GetGLAccountByExpenseType(taxRow["ExpenseType"].ToString());
            taxRow["ItemAmount"] = TaxTotalAmount.ToString();
            taxRow["FANO"] = "";
            return this.ItemTable;
        }

        private Hashtable GetExpenseTypeHashtable()
        {
            DataTable expenseType = ExpenseTypes;
            Hashtable ht = new Hashtable();
            foreach (DataRow row in expenseType.Rows)
            {
                if (ht[row["GLAccount"].ToString()].AsString() == "")
                {
                    ht.Add(row["GLAccount"].ToString(), row["ExpenseType"].ToString());
                }
            }
            return ht;
        }

        /// <summary>
        /// 填充分期付款信息
        /// </summary>
        /// <param name="dTable"></param>
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
                        PaidThisTime = double.Parse(dTable.Rows[0]["PaidThisTime"].ToString());
                        if (IsFromPO == true)
                        {
                            txtPaidBefore.Text = Math.Round(decimal.Parse(dTable.Rows[0]["TotalAmount"].ToString()) *
                                             decimal.Parse(dTable.Rows[0]["PaidBefore"].ToString()) / 100, 2).ToString();
                            txtPaidThisTime.Text = Math.Round(decimal.Parse(dTable.Rows[0]["TotalAmount"].ToString()) *
                                             decimal.Parse(dTable.Rows[0]["PaidThisTime"].ToString()) / 100, 2).ToString();
                        }
                        else
                        {
                            txtPaidBefore.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidBefore"].ToString()), 2).ToString();
                            txtPaidThisTime.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidThisTimeAmount"].ToString()), 2).ToString();
                        }
                        txtBlance.Text = Math.Round(decimal.Parse(txtTotalAmount.Text) - decimal.Parse(txtPaidBefore.Text) -
                                            decimal.Parse(txtPaidThisTime.Text), 2).ToString();
                    }
                }
                string poID = Request.QueryString["PONO"] == null ? PONO : Request.QueryString["PONO"].ToString();
                DataTable dTable1 = PaymentRequestComm.GetPaymentInstallmentInfo(poID).GetDataTable();
                radioInstallment.SelectedIndex = (dTable1 != null && dTable1.Rows.Count > 1) ? 0 : 1;

            }
        }

        /// <summary>
        /// 填充付款申请详细信息
        /// </summary>
        /// <param name="dTable"></param>
        private void FillPaymentRequestInfo(DataTable dTable)
        {
            if (dTable != null)
            {
                Session["ContractPONo"] = dTable.Rows[0]["ContractPONo"].ToString();
                txtContractPO.Text = dTable.Rows[0]["ContractPONo"].ToString();
                txtSystemPO.Text = dTable.Rows[0]["SystemPONo"].ToString();
                txtPaymentDesc.Text = dTable.Rows[0]["PaymentDesc"].ToString();
                txtRemark.Text = dTable.Rows[0]["InvoiceRemark"].ToString();
                txtPaymentDesc.Text = dTable.Rows[0]["PaymentDesc"].ToString();
                txtPaymentReason.Text = dTable.Rows[0]["PaymentReason"].ToString();
                dropCostCenter.SelectedValue = dTable.Rows[0]["CostCenter"].ToString();
                radioInvoice.SelectedIndex = (int.Parse(dTable.Rows[0]["IsAttachedInvoice"].ToString()) + 1) % 2;
                radioInstallment.SelectedIndex = (int.Parse(dTable.Rows[0]["IsInstallment"].ToString()) + 1) % 2;
                radioContractPO.SelectedIndex = (int.Parse(dTable.Rows[0]["IsContractPO"].ToString()) + 1) % 2;

                if ((PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.Edit && IsFromPO == false)
                {
                    radioNeedGR.SelectedIndex = (int.Parse(dTable.Rows[0]["IsNeedGR"].ToString()) + 1) % 2;
                    radioSystemGR.SelectedIndex = (int.Parse(dTable.Rows[0]["IsAllSystemGR"].ToString()) + 1) % 2;
                    radioSystemPO.SelectedIndex = (int.Parse(dTable.Rows[0]["IsSystemPO"].ToString()) + 1) % 2;
                }
            }
        }

        /// <summary>
        /// 返回最后一次付款的信息
        /// </summary>
        /// <param name="poID"></param>
        /// <returns></returns>
        private DataTable GetLastPaymentRequestInfo(string poID)
        {
            DataTable dTable = PaymentRequestComm.GetPaymentRequestItemsInfoByPONO(poID).GetDataTable();
            if (dTable != null && dTable.Rows.Count > 0)
            {
                System.Data.DataView dView = dTable.DefaultView;
                dView.Sort = " SubPRNo desc";

                return dView.ToTable().Rows[0].Table;
            }
            return null;
        }

        /// <summary>
        /// 返回分期付款的信息
        /// </summary>
        /// <param name="poID"></param>
        /// <returns></returns>
        private DataTable GetInstallmentInfo(string poID)
        {
            DataTable dTable = PaymentRequestComm.GetPaymentInstallmentInfo(poID).GetDataTable();
            decimal newPaidBefore = 0;
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow row in dTable.Rows)
                {
                    if (row["IsPaid"].ToString() == "0" || row["IsPaid"].ToString().IsNullOrWhitespace())
                    {
                        mIsNeedSystemGR = row["IsNeedGR"].ToString().IsNullOrWhitespace()
                                       || row["IsNeedGR"].ToString() == "0" ? false : true;
                        DataTable dTable1 = CreateInstalmentDT();
                        DataRow dRow = dTable1.NewRow();
                        dRow["PaidBefore"] = mPaidBefore;
                        dRow["Balance"] = (100 - newPaidBefore - (row["Paid"].ToString().IsNullOrWhitespace()
                                           ? 0 : decimal.Parse(row["Paid"].ToString())));
                        dRow["PaidThisTime"] = row["Paid"];
                        dRow["PaidThisTimeAmount"] = row["PaidThisTimeAmount"].AsString();
                        //NewPaidBefore
                        dRow["NewPaidBefore"] = newPaidBefore;
                        dRow["PaidInd"] = row["Index"];
                        dRow["TotalAmount"] = row["TotalAmount"];
                        dTable1.Rows.Add(dRow);
                        ViewState["Installment"] = dTable1;
                        PaidTotalAmountThisTime = double.Parse(row["PaidThisTimeAmount"].AsString() == "" ? "0" : row["PaidThisTimeAmount"].AsString());
                        return dTable1;
                    }
                    else
                    {
                        if (IsFromPO == false)
                        {
                            mPaidBefore += decimal.Parse(row["PaidThisTimeAmount"].AsString() == "" ? "0" : row["PaidThisTimeAmount"].AsString());
                        }
                        else
                        {
                            mPaidBefore += decimal.Parse(row["Paid"].ToString());
                        }
                        newPaidBefore += decimal.Parse(row["Paid"].ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable CreateInstalmentDT()
        {
            DataTable dTable = new DataTable();
            dTable.Columns.Add("PaidBefore");
            dTable.Columns.Add("NewPaidBefore");
            dTable.Columns.Add("PaidThisTime");
            dTable.Columns.Add("Balance");
            dTable.Columns.Add("PaidInd");
            dTable.Columns.Add("TotalAmount"); 
            dTable.Columns.Add("PaidThisTimeAmount");
            return dTable;
        }

        public bool SavePeymentRequestData(Installment installmentForm, WorkflowDataFields fields)
        {
            if (Session["ContractPONo"] == null || (Session["ContractPONo"] != null && Session["ContractPONo"].ToString() != txtContractPO.Text))
            {
                DataTable dTable = PaymentRequestComm.GetPaymentRequestInfoByContractPONo(txtContractPO.Text.ToUpper()).GetDataTable();
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    ErrorMessage = " 您输入的合同编号已经存在，请重新输入 ";
                    return false;
                }
            }

            if ((PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.New)
            {
                if (ViewState["PrDict"] == null)
                {
                    CreateWorkFlowNumber();
                }

                if (((Dictionary<string, string>)ViewState["PrDict"])["PIIndex"].ToString() == "1")
                {
                    SetPaymentRequestInfo();
                }
                else
                {
                    PaymentRequestComm.SetPaymentRequestInfo(PONO, new List<object[]>() { 
                        new object[] { "Status", PaymentRequestStatus.InProcess },
                        new object[] { "ContractPO", txtContractPO.Text }
                        });
                }

                SetPaymentRequestUser(fields);
            }

            string paidInd = string.Empty;
            if (ViewState["Installment"] != null)
            {
                paidInd = (ViewState["Installment"] as DataTable).Rows[0]["PaidInd"].ToString();
            }

            if ((ViewState["Installment"] == null || paidInd == "1") && IsFromPO == false)
            {
                SetInstallmentInfo(installmentForm);
            }

            SetPaymentRequestItemsInfo(fields);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetInstallmentInfo(Installment installmentForm)
        {
            SetInstallmentViewState(installmentForm);

            if ((PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.Edit && IsFromPO == false)
            {
                PaymentRequestComm.DelInstallmentInfo(((Dictionary<string, string>)ViewState["PrDict"])["PONO"]);
            }

            List<List<object[]>> prInfos = new List<List<object[]>>();
            if (radioInstallment.SelectedValue == "Yes")
            {
                int i = 0;
                foreach (RepeaterItem rItem in installmentForm.InstallmentRepeaterControl.Items)
                {
                    AddInstallmentToList(prInfos, ++i, 0, ((TextBox)rItem.FindControl("txtPaid")).Text, ((TextBox)rItem.FindControl("txtPaidAmount")).Text, ((CheckBox)rItem.FindControl("txtIsNeedGR")).Checked ? "1" : "0",
                    ((TextBox)rItem.FindControl("txtComments")).Text, txtTotalAmount.Text, ((Dictionary<string, string>)ViewState["PrDict"])["PONO"], "分期第 " + i.ToString() + " 次付款");
                }
            }
            else
            {
                //一次性付款
                AddInstallmentToList(prInfos, 1, 0, 100, txtTotalAmount.Text, 0, "", txtTotalAmount.Text,
                ((Dictionary<string, string>)ViewState["PrDict"])["PONO"], "1 次性付款");
            }

            PaymentRequestComm.SetInstallmentInfo(prInfos);
        }

        private void AddInstallmentToList(List<List<object[]>> prInfos, object index, object isPaid, object paid, object paidAmount,
            object isNeedGR, object comments, object totalAmount, object poNo, object title)
        {
            //一次性付款
            List<object[]> pr = new List<object[]>();
            pr.Add(new object[] { "Index", index });
            pr.Add(new object[] { "IsPaid", isPaid });
            pr.Add(new object[] { "Paid", paid });
            pr.Add(new object[] { "PaidThisTimeAmount", (object)Math.Round(decimal.Parse(paidAmount.ToString()),2) });
            pr.Add(new object[] { "IsNeedGR", isNeedGR });
            pr.Add(new object[] { "Comments", comments });
            pr.Add(new object[] { "TotalAmount", totalAmount });
            pr.Add(new object[] { "PONo", poNo });
            pr.Add(new object[] { "Title", title });
            prInfos.Add(pr);
        }

        private void SetInstallmentViewState(Installment installmentForm)
        {
            DataTable dTable = CreateInstalmentDT();
            DataRow dRow = dTable.NewRow();
            dRow["PaidBefore"] = 0;
            dRow["NewPaidBefore"] = 0;
            dRow["PaidInd"] = 1;
            dRow["Balance"] = 0;
            dRow["PaidThisTime"] = 100;
            if (radioInstallment.SelectedValue == "Yes" && string.IsNullOrEmpty(txtTotalAmount.Text) == false)
            {
                if (string.IsNullOrEmpty(((TextBox)installmentForm.InstallmentRepeaterControl.Items[0].FindControl("txtPaid")).Text) == false)
                {
                    dRow["Balance"] = radioInstallment.SelectedValue == "Yes" ? (100 - decimal.Parse(
                        ((TextBox)installmentForm.InstallmentRepeaterControl.Items[0].FindControl("txtPaid")).Text)).ToString() : "0";
                    dRow["PaidThisTime"] = radioInstallment.SelectedValue == "Yes" ?
                        ((TextBox)installmentForm.InstallmentRepeaterControl.Items[0].FindControl("txtPaid")).Text : "100";

                }
            }
            dTable.Rows.Add(dRow);
            ViewState["Installment"] = dTable;
        }

        public string HF_D_RequestType
        {
            get { return this.hf_D_RequestType.Value.Trim(); }
            set { this.hf_D_RequestType.Value = value; }
        }

        public string RequestType
        {
            get
            {
                return this.ViewState["RequestTypeStatus"].ToString();
            }
            set
            {
                this.ViewState["RequestTypeStatus"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private void SetPaymentRequestInfo()
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)ViewState["PrDict"];
            string poNO = dict["PONO"];

            var rootweburl = ConfigurationManager.AppSettings["rootweburl"];
            SPFieldUrlValue payLink = new SPFieldUrlValue();
            payLink.Description = "Pay";
            payLink.Url = rootweburl + "/WorkFlowCenter/lists/PaymentRequestItems/NewForm.aspx?PONO=" + poNO +
                (IsFromPO == true ? "&IsFromPO=true" : "");
            payLink.Url += "&RequestType=" + RequestType;

            SPFieldUrlValue historyLink = new SPFieldUrlValue();
            historyLink.Description = "PaymentHistory";
            historyLink.Url = rootweburl + "/WorkFlowCenter/_layouts/ca/workflows/PaymentRequest/HistoryForm.aspx?PONO=" + poNO;

            var web = SPContext.Current.Web;
            SPUser user = null;
            SPFieldUserValue spUser = null;
            user = web.AllUsers[SPContext.Current.Web.CurrentUser.LoginName];
            spUser = new SPFieldUserValue(web, user.ID, user.Name);

            List<object[]> list = new List<object[]>();
            list.Add(new object[] { "PONo", poNO });
            list.Add(new object[] { "WorkFlowNumber", dict["PRNO"] });
            list.Add(new object[] { "Payment", payLink });
            list.Add(new object[] { "History", historyLink });
            list.Add(new object[] { "IsFromPO", IsFromPO });
            list.Add(new object[] { "ContractPO", txtContractPO.Text });
            list.Add(new object[] { "Status", PaymentRequestStatus.InProcess });
            list.Add(new object[] { "RequestType", Opex_Capex_Status });
            list.Add(new object[] { "Approvers", spUser });
            PaymentRequestComm.SetPaymentRequestInfo(list);
        }

        public SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            });
            return user;
        }

        protected SPFieldUserValueCollection ReturnAllApproversSPByPR(string approverCol,string pono, params string[] accounts)
        {
            var web = SPContext.Current.Web;
            SPUser user = null;
            SPFieldUserValue spUser = null;
            SPListItemCollection spListItems = PaymentRequestComm.GetPaymentRequestInfo(pono);
            foreach (SPListItem spListItem in spListItems)
            {
                SPFieldUser userField = spListItem.Fields[approverCol] as SPFieldUser;

                SPFieldUserValueCollection approvers = userField.GetFieldValue(spListItem[approverCol].AsString()) as SPFieldUserValueCollection;
                foreach (var account in accounts)
                {
                    user = web.AllUsers[account];
                    spUser = new SPFieldUserValue(web, user.ID, user.Name);

                    if (approvers == null)
                    {
                        approvers = new SPFieldUserValueCollection();
                    }
                    if (!approvers.Contains(spUser))
                    {
                        approvers.Add(spUser);
                    }
                }

                return approvers;
            }
            return null;
        }
        public string SubPRWorkFlowNumber 
        {
            get 
            {
                Dictionary<string, string> dict = (Dictionary<string, string>)ViewState["PrDict"];
                return dict["PRNO"] +"_" + dict["PIIndex"];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        private void SetPaymentRequestItemsInfo(WorkflowDataFields fields)
        {
            if ((PaymentRequestMode)Session["PRMode"] == PaymentRequestMode.New)
            {
                Dictionary<string, string> dict = (Dictionary<string, string>)ViewState["PrDict"];
                fields["Company"] = "CA";
                fields["Title"] = dict["PRNO"];
                fields["PRNo"] = dict["PRNO"];
                fields["SubPRNo"] = dict["PRNO"] +
                           "_" + dict["PIIndex"];
                fields["PONo"] = dict["PONO"];
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItemCollection spListItems = PaymentRequestComm.GetPaymentRequestInfo(fields["PONo"].AsString());
                foreach (SPListItem spListItem in spListItems)
                {
                    spListItem["TotalAmount"] = (radioInstallment.SelectedIndex == 0 ?
                                                 txtTotalAmount.Text : txtTotalAmount1.Text);
                    spListItem["PaymentReason"] = txtPaymentReason.Text;
                    spListItem["VendorName"] = txtVenderName.Text;
                    spListItem["ApplicantSPUser"] = this.EnsureUser(ApplicantEmployee.UserAccount);
                    spListItem["Approvers"] = ReturnAllApproversSPByPR("Approvers", fields["PONo"].AsString(), new string[] { ApplicantEmployee.UserAccount, SPContext.Current.Web.CurrentUser.LoginName });
                    spListItem.Web.AllowUnsafeUpdates = true;
                    spListItem.Update();
                }
            });

            fields["IsFromPO"] = IsFromPO;
            fields["Dept"] = txtDept.Text;
            fields["VendorNo"] = txtVenderCode.Text;
            fields["VendorName"] = txtVenderName.Text;
            fields["BankName"] = txtBankName.Text;
            fields["BankAccount"] = txtBankAC.Text;
            fields["SwiftCode"] = txtSwiftCode.Text;
            fields["PaymentDesc"] = txtPaymentDesc.Text;
            fields["CostCenter"] = dropCostCenter.SelectedValue;
            fields["TotalAmount"] = (radioInstallment.SelectedIndex == 0 ?
                                     txtTotalAmount.Text : txtTotalAmount1.Text);
            fields["IsContractPO"] = (radioContractPO.SelectedIndex + 1) % 2;
            fields["IsSystemPO"] = (radioSystemPO.SelectedIndex + 1) % 2;
            fields["IsNeedGR"] = (radioNeedGR.SelectedIndex + 1) % 2;
            fields["IsAllSystemGR"] = (radioSystemGR.SelectedIndex + 1) % 2;
            fields["IsInstallment"] = (radioInstallment.SelectedIndex + 1) % 2;
            fields["IsAttachedInvoice"] = (radioInvoice.SelectedIndex + 1) % 2;
            fields["InvoiceRemark"] = txtRemark.Text;
            fields["PaymentReason"] = txtPaymentReason.Text;
            fields["SubmitDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            fields["ContractPONo"] = txtContractPO.Text.ToUpper();
            fields["SystemPONo"] = txtSystemPO.Text;
            fields["Status"] = CAWorkflowStatus.InProgress;
            if (ViewState["Installment"] != null)
            {
                //fields["PaidBefore"] = ((ViewState["Installment"] as DataTable).Rows[0]["PaidBefore"].ToString());
                fields["PaidBefore"] = ((ViewState["Installment"] as DataTable).Rows[0]["NewPaidBefore"].ToString());
                fields["PaidThisTime"] = ((ViewState["Installment"] as DataTable).Rows[0]["PaidThisTime"].ToString());
                fields["Balance"] = ((ViewState["Installment"] as DataTable).Rows[0]["Balance"].ToString());
                fields["PaidInd"] = ((ViewState["Installment"] as DataTable).Rows[0]["PaidInd"].ToString());
            }

            fields["VendorCity"] = this.txtVendorCity.Text.Trim();
            //fields["VendorCountry"] = this.txtVendorCountry.Text.Trim();
            fields["VendorCountry"] = this.ddlVendorCountry.SelectedValue;
            //fields["BankCity"] = this.txtBankCity.Text.Trim();
            fields["BankCity"] = this.ddlBankCountry.SelectedValue;
        }

        private void SetPaymentRequestUser(WorkflowDataFields fields)
        {
            fields["Applicant"] = Applicant;
            fields["Manager"] = Manager;
            fields["Approvers"] = Approvers; ;
            fields["ApproversSPUser"] = ApproversSPUser;

        }

        public string SummaryExpenseType1
        {
            set { this.hidSummaryExpenseType1.Value = value; }
        }

        public void CreateWorkFlowNumber()
        {
            //string poNumber = "PRPO000001";
            //if (IsFromPO == true)
            //{
            //    poNumber = Request.QueryString["PONO"].ToString();
            //}

            //mPRNO = "PR00000001";
            //DataTable dTable = PaymentRequestComm.GetLastPaymentRequestInfo();
            //if (dTable != null && dTable.Rows.Count > 0)
            //{
            //    string wfNumber = dTable.Rows[0]["WorkFlowNumber"].ToString();
            //    wfNumber = wfNumber.Substring(2, wfNumber.Length - 2);
            //    wfNumber = (int.Parse(wfNumber) + 1).ToString();
            //    mPRNO = "PR" + new string('0', 8 - wfNumber.Length) + wfNumber;

            //    if (IsFromPO == false)
            //    {
            //        foreach (DataRow dRow in dTable.Rows)
            //        {
            //            if (dRow["PONo"].ToString() != "" && dRow["PONo"].ToString().Contains("PRPO"))
            //            {
            //                if (string.Equals(dRow["PONo"].ToString().Substring(0, 4), "PRPO", StringComparison.CurrentCultureIgnoreCase))
            //                {
            //                    string po = dRow["PONo"].ToString();
            //                    po = po.Substring(4, po.Length - 4);
            //                    po = (int.Parse(po) + 1).ToString();
            //                    poNumber = "PRPO" + new string('0', 6 - po.Length) + po;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            string poNumber = string.Empty;
            if (IsFromPO == true)
            {
                poNumber = Request.QueryString["PONO"].ToString();
            }
            else
            {
                poNumber = GetPRPOWorkflowNumber();
            }
            mPRNO = GetPRWorkflowNumber();
            //创建分期付款的对象
            CreatePiDictViewState(poNumber);
        }

        private string GetPRWorkflowNumber()
        {
            return "PR" + WorkFlowUtil.CreateWorkFlowNumber("PRPaymentRequest").ToString("0000000000");
        }

        private string GetPRPOWorkflowNumber()
        {
            return "PRPO" + WorkFlowUtil.CreateWorkFlowNumber("PRPOPaymentRequest").ToString("0000000000");
        }

        private void CreatePiDictViewState(string poNumber)
        {
            Dictionary<string, string> dict = ViewState["PrDict"] != null ? (
                    Dictionary<string, string>)ViewState["PrDict"] :
                    new Dictionary<string, string>();
            if (dict.ContainsKey("IsFromPO") == false)
                dict.Add("IsFromPO", IsFromPO == true ? "1" : "0");
            if (dict.ContainsKey("PRNO") == false)
                dict.Add("PRNO", mPRNO);
            if (dict.ContainsKey("PONO") == false)
                dict.Add("PONO", poNumber);
            if (dict.ContainsKey("PIIndex") == false)
                dict.Add("PIIndex", "1");

            ViewState["PrDict"] = dict;
        }

        /// <summary>
        /// 跳转到 My History  
        /// </summary>
        private void TransferPage(string msg)
        {
            string url = Request.UrlReferrer.ToString();
            Response.Write("<script type='text/javascript' language='javascript'>" +
                "alert('" + msg + " ');" +
                "window.location = '" + url + "';" +
                "</script>");
        }

        #endregion

        #region cost center related method -- update by LJ

        internal DataTable ItemTable
        {
            get
            {
                return (this.ViewState["ItemTable"] as DataTable) ?? CreateItemTable();
            }
            set
            {
                this.ViewState["ItemTable"] = value;
            }
        }

        internal DataTable ExpenseTypes
        {
            get
            {
                return this.ViewState["ExpenseTypes"] as DataTable;
            }
            set
            {
                this.ViewState["ExpenseTypes"] = value;
            }
        }

        internal DataTable CostCenters
        {
            get
            {
                return this.ViewState["CostCenters"] as DataTable;
            }
            set
            {
                this.ViewState["CostCenters"] = value;
            }
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

        private double TaxTotalAmount = 0;
        private double TotalAmount = 0;

        private void BindCostCenterAndExpenseType()
        {
            //Payment Request Expense Types
            ExpenseTypes = PaymentRequestComm.GetPRExpenseTypeDataTable(Opex_Capex_Status);
            CostCenters =WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
            OriginalExpenseType=this.GetOriginalExpenseType();
        }

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("ItemAmount");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("GLAccount");
            ItemTable.Columns.Add("FANO");
            ItemTable.Rows.Add();
            return ItemTable;
        }

        protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            UpdateItem();
            DataRow row = ItemTable.Rows.Add();
            AddRow(row);
            this.rptItem.DataSource = this.ItemTable;
            this.rptItem.DataBind();
        }

        private void AddRow(DataRow row)
        {
            if (this.rptItem.Items.Count > 0)
            {
                RepeaterItem item = this.rptItem.Items[this.rptItem.Items.Count - 1];
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                var txtFANO = (TextBox)item.FindControl("txtFANO");
                row["ExpenseType"] = ddlExpenseType.SelectedValue;
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["ItemAmount"] = txtAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;
                row["FANO"] = txtFANO.Text;
            }
        }

        protected void rptItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateItem();
                if (e.Item.ItemIndex > 0)
                {
                    ItemTable.Rows.Remove(ItemTable.Rows[e.Item.ItemIndex]);
                }
                this.rptItem.DataSource = ItemTable;
                this.rptItem.DataBind();
            }
        }

        protected void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                    var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    var txtAmount = (TextBox)item.FindControl("txtAmount");
                    var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                    var txtFANO = (TextBox)item.FindControl("txtFANO");
                    DataBindExpenseType(ddlExpenseType, ExpenseTypes);
                    DataBindCostCenter(ddlCostCenter, CostCenters);
                    ddlExpenseType.SelectedValue =OriginalExpenseType[row["ExpenseType"].ToString()].AsString();
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["ItemAmount"].ToString();
                    if (row["ExpenseType"].ToString() != "")
                    {
                        lblGLAccount.Text = this.GetGLAccountByExpenseType(OriginalExpenseType[row["ExpenseType"].ToString()].AsString());
                    }
                    else
                    {
                        lblGLAccount.Text = "";
                    }
                    txtFANO.Text = row["FANO"].AsString();
                }
            }
        }

        private void GetGLAccountDataTable()
        {
            System.Text.StringBuilder strGLAccount = new System.Text.StringBuilder();
            strGLAccount.Append("[");
            DataTable dt = PaymentRequestComm.GetPRExpenseTypeDataTable(Opex_Capex_Status);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    strGLAccount.Append("{");
                    strGLAccount.AppendFormat("name:'{0}',val:'{1}'", dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
                    strGLAccount.Append("},");
                }
            }
            strGLAccount.Append("]");
            this.hfGLAccount.Value = strGLAccount.ToString();

            switch (Opex_Capex_Status)
            {
                case OpexCapexStatus.Opex:
                    this.lblTDStatus.Text = "GL Account";
                    this.lblExpenseType.Text = "Expense Type";
                    this.lblSummaryType.Text = "Expense Type";
                    this.FAStatus1.Value = "0";
                    break;
                case OpexCapexStatus.Capex_AssetNo:
                    this.lblTDStatus.Text = "Asset Class";
                    this.lblExpenseType.Text = "Asset Type";
                    this.lblSummaryType.Text = "Asset Type";
                    this.FAStatus1.Value = "1";
                    break;
                case OpexCapexStatus.Capex_NoAssetNo:
                    this.lblTDStatus.Text = "Asset Class";
                    this.lblExpenseType.Text = "Asset Type";
                    this.lblSummaryType.Text = "Asset Type";
                    this.FAStatus1.Value = "0";
                    break;
            }
        }

        private Hashtable GetOriginalExpenseType()
        {
            DataTable dt = ExpenseTypes;
            Hashtable ht = new Hashtable();
            foreach(DataRow dr in dt.Rows)
            {
                ht.Add(dr["OriginalExpenseType"].ToString(), dr["ExpenseType"].ToString());
            }
            return ht;
        }

       private string GetGLAccountByExpenseType(string key)
        {
            Hashtable ht = GetExpenseTypeAndGLAccountHashTable();
            string gLAccount = "";
            if (key != "")
            {
                gLAccount = ht[key] != null ? ht[key].ToString() : "";
            }
            return gLAccount;
        }

        private Hashtable GetExpenseTypeAndGLAccountHashTable()
        {
            Hashtable ht = new Hashtable();
            DataTable dt = PaymentRequestComm.GetPRExpenseTypeDataTable(Opex_Capex_Status);
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
            }
            return ht;
        }

        private void UpdateItem()
        {
            this.ItemTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var lblGLAccount = (TextBox)item.FindControl("lblGLAccount");
                var txtFANO = (TextBox)item.FindControl("txtFANO");
                DataRow row = this.ItemTable.Rows.Add();
                row["ExpenseType"] = GetNewExpenseTypeByOriginalExpenseType(ddlExpenseType.SelectedValue);
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["ItemAmount"] = txtAmount.Text;
                row["GLAccount"] = lblGLAccount.Text;
                row["FANO"] = txtFANO.Text;
            }
        }

        private string GetNewExpenseTypeByOriginalExpenseType(string et)
        {
            DataTable dt = ExpenseTypes; //if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
            string expenseType = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ExpenseType"].ToString().IndexOf(et) != -1)
                {
                    expenseType = dr["OriginalExpenseType"].ToString();
                    break;
                }
            }
            return expenseType;
        }

        private void DataBindExpenseType(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["ExpenseType"].ToString(), dr["ExpenseType"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void DataBindCostCenter(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["Display"].ToString(), dr["Title"].ToString());
                ddl.Items.Add(li);
            }
        }

        private void DataBindDataTable()
        {
            if (IsFromPO == false)
            {
                DataTable data = this.ItemTable;
                this.rptItem.DataSource = data;
                this.rptItem.DataBind();
            }
            else
            {
                DataTable data = GetCostCenter();
                this.rptItem.DataSource = data;
                this.rptItem.DataBind();
            }
        }

        public void Update()
        {
            UpdateItem();
        }

        #endregion

        #region Opex Capex

        public string Opex_Capex_Status
        {
            get
            {
                return this.ViewState["opex_Capex_Status"].ToString();
            }
            set
            {
                this.ViewState["opex_Capex_Status"] = value;
            }
        }

        
        #endregion

        #region 新需求

        private void LoadCountryCurrency()
        {
            DataTable dt = WorkFlowUtil.GetCollectionByList("Payment Request Country Currency").GetDataTable();
            this.ddlVendorCountry.Items.Clear();
            this.ddlBankCountry.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ListItem liVendorCountry = new ListItem()
                {
                    Text = dr["CountryCode"].ToString(),
                    Value = dr["CountryCode"].ToString()
                };
                ListItem liBankCountry = new ListItem()
                {
                    Text = dr["CountryCode"].ToString(),
                    Value = dr["CountryCode"].ToString()
                };
                this.ddlVendorCountry.Items.Add(liVendorCountry);
                this.ddlBankCountry.Items.Add(liBankCountry);
            }
        }

        void cpfUser_Load(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.ApplicantEmployee = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            this.txtApplicant.Text = this.ApplicantEmployee.DisplayName;
            this.txtDept.Text = this.ApplicantEmployee.Department;
        }

        protected void btnPeopleInfo_Click(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.ApplicantEmployee = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            this.txtApplicant.Text = this.ApplicantEmployee.DisplayName;
            this.txtDept.Text = this.ApplicantEmployee.Department;
        }

        #endregion

        #region Copy Module
        private string copyStatus = "0";
        public string CopyStatus
        {
            get { return copyStatus; }
            set { copyStatus = value; }
        }
        private string workFlowNumber = "";
        public string WorkFlowNumber
        {
            get { return workFlowNumber; }
            set { workFlowNumber = value; }
        }
        private void BindPaymentRequestDataByWorkFlowNumber(string workFlowNumber)
        {
            LoadCountryCurrency();
            SetControlStatus();
            GetGLAccountDataTable();
            DataTable data = PaymentRequestComm.GetPaymentRequestItemsInfoBySUBPRNO(workFlowNumber).GetDataTable();
            if (data != null && data.Rows.Count > 0)
            {
                FillPaymentInfo(data);
            }
        }

        #endregion

        #region Bind Payment Request Data

        private DataTable GetInstallmentInfo1(string poID)
        {
            decimal mPaidBefore = 0; ;
            DataTable dTable = PaymentRequestComm.GetPaymentInstallmentInfo(poID).GetDataTable();
            if (dTable != null && dTable.Rows.Count > 0)
            {
                DataRow row = dTable.Rows[0];
                DataTable dTable1 = CreateInstalmentDT1();
                DataRow dRow = dTable1.NewRow();
                dRow["PaidBefore"] = mPaidBefore;
                dRow["Balance"] = (100 - mPaidBefore - (row["PaidThisTimeAmount"].ToString().IsNullOrWhitespace()
                                                           ? 0 : decimal.Parse(row["PaidThisTimeAmount"].ToString())));
                dRow["PaidThisTime"] = row["Paid"];
                dRow["PaidThisTimeAmount"] = row["PaidThisTimeAmount"];
                dRow["PaidInd"] = row["Index"];
                dRow["TotalAmount"] = row["TotalAmount"];
                dTable1.Rows.Add(dRow);
                ViewState["Installment"] = dTable1;
                return dTable1;
            }
            return null;
        }

        private DataTable CreateInstalmentDT1()
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

        private void FillPeymentInstallmentInfo1(DataTable dTable)
        {
            if (dTable != null)
            {
                if (string.IsNullOrEmpty(dTable.Rows[0]["TotalAmount"].ToString()) == false)
                {
                    txtTotalAmount.Text = dTable.Rows[0]["TotalAmount"].ToString();
                    txtTotalAmount1.Text = dTable.Rows[0]["TotalAmount"].ToString();
                    if (string.IsNullOrEmpty(dTable.Rows[0]["PaidThisTimeAmount"].ToString()) == false)
                    {
                        txtPaidBefore.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidBefore"].ToString()), 2).ToString();
                        txtPaidThisTime.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidThisTimeAmount"].ToString()), 2).ToString();
                        txtBlance.Text = Math.Round(decimal.Parse(txtTotalAmount.Text) - decimal.Parse(txtPaidBefore.Text) -
                                             decimal.Parse(txtPaidThisTime.Text), 2).ToString();
                    }
                    else
                    {
                        txtPaidBefore.Text = Math.Round(decimal.Parse(dTable.Rows[0]["PaidBefore"].ToString()), 2).ToString();
                        txtPaidThisTime.Text = Math.Round(decimal.Parse(dTable.Rows[0]["TotalAmount"].ToString()) *
                                              decimal.Parse(dTable.Rows[0]["PaidThisTime"].ToString()) / 100, 2).ToString();
                        txtBlance.Text = Math.Round(decimal.Parse(txtTotalAmount.Text) - decimal.Parse(txtPaidBefore.Text) -
                                             decimal.Parse(txtPaidThisTime.Text), 2).ToString();
                    }
                }
            }
        }

        private void FillPaymentInfo(DataTable data)
        {
            DataRow dr = data.Rows[0];
            //Vendor Info
            txtVenderCode.Text = GetString(dr["VendorNo"]);
            txtVenderName.Text = GetString(dr["VendorName"]);
            //Vendor Info
            txtVendorCity.Text = GetString(dr["VendorCity"]);
            ddlVendorCountry.SelectedValue = GetString(dr["VendorCountry"]);
            txtBankName.Text = GetString(dr["BankName"]);
            txtBankAC.Text = GetString(dr["BankAccount"]);
            ddlBankCountry.SelectedValue = GetString(dr["BankCity"]);
            txtSwiftCode.Text = GetString(dr["SwiftCode"]);
            //Applicant Info
            txtApplicant.Text = GetString(dr["Applicant"]);
            txtDept.Text = GetString(dr["Dept"]);
            //Payment Descriptions
            txtPaymentDesc.Text = GetString(dr["PaymentDesc"]);
            //radioInstallment
            radioInstallment.SelectedIndex = GetString(dr["IsInstallment"]).Equals("1", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            //Summary Expense
            this.lblCurrency.Text = dr["Currency"].AsString();
            this.SummaryExpenseType1 = dr["SummaryExpenseType"].AsString();
            //Installment Info
            if (dr["TotalAmount"] != null)
            {
                DataTable dt = GetInstallmentInfo1(dr["PONo"].AsString());
                FillPeymentInstallmentInfo1(dt);
            }
            //ContractPO
            radioContractPO.SelectedIndex = GetString(dr["IsContractPO"]).Equals("1", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            txtContractPO.Text = GetString(dr["ContractPONo"]);
            //SystemPO
            radioSystemPO.SelectedIndex = GetString(dr["IsSystemPO"]).Equals("1", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            txtSystemPO.Text = GetString(dr["SystemPONo"]);
            //NeedGR
            radioNeedGR.SelectedIndex = GetString(dr["IsNeedGR"]).Equals("1", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            radioSystemGR.SelectedIndex = GetString(dr["IsAllSystemGR"]).Equals("1", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            txtPaymentReason.Text = GetString(dr["PaymentReason"]);
            //radioInvoice             
            radioInvoice.SelectedIndex = GetString(dr["IsAttachedInvoice"]).Equals("1", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            //txtRemark
            txtRemark.Text = GetString(dr["InvoiceRemark"]);
        }

        private string GetString(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        #endregion

    }
   
    public static class OpexCapexStatus
    {
        public const string Opex = "Opex";
        //public const string Capex_AssetNo = "Capex_AssetNo";
        //public const string Capex_NoAssetNo = "Capex_NoAssetNo";
        public const string Capex_AssetNo = "Capex";
        public const string Capex_NoAssetNo = "Capex_NoAssetNo";
    }

}
