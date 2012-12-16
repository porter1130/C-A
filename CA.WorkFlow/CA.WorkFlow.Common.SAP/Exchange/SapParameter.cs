using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 客户端需要提供的参数
    /// </summary>
    public class SapParameter
    {
        private string mPymtMeth=string.Empty;
        /// <summary>
        /// 信用卡支付时需要赋值
        /// 当是现金时，赋值：E
        /// 若是转账，赋值：空
        /// </summary>
        public string PymtMeth
        {
            get
            {
                return mPymtMeth;
            }
            set
            {
                mPymtMeth = value;
            }
        }

        private string mBankName = string.Empty;
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName
        {
            get
            {
                return mBankName;
            }
            set
            {
                mBankName = value;
            }
        }
        private string mBankId = "0000008277";
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankId
        {
            get
            {
                return mBankId;
            }
            set
            {
                mBankId = value;
            }
        }
        /// <summary>
        /// SAP返回的NUMBER
        /// </summary>
        public string SapNumber
        {
            get; set;
        }
        private string mPaymentCond = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string PaymentCond
        {
            get { return mPaymentCond; }
            set { mPaymentCond = value; }
        }
        private string mVendor = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Vendor
        {
            get { return mVendor; }
            set { mVendor = value; }
        }
        private string mPurGroup = string.Empty;
        /// <summary>
        /// 
        /// </summary>                                              
        public string PurGroup
        {
            get { return mPurGroup; }
            set { mPurGroup = value; }
        }
     
        private string mDocDate = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 
        /// </summary>
        public string DocDate
        {
            get { return mDocDate; }
            set { mDocDate = value; }
        }

        private string mPmnttrms = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Pmnttrms
        {
            get { return mPmnttrms; }
            set { mPmnttrms = value; }
        }
        private string mUserName = "ewf";
        /// <summary>
        /// 连接SAP时使用的用户名，如：ewf
        /// </summary>
        public string UserName
        {
            get
            {
                return mUserName;
            }
            set
            {
                mUserName = value;
            }
        }
        private string mCompCode = "CA10";
        /// <summary>
        /// Company Code
        /// </summary>
        public string CompCode { 
            get { return mCompCode; } 
            set { mCompCode = value; } 
        }
        private string mDocType = "SA";
        /// <summary>
        /// Document Type
        /// </summary>
        public string DocType { 
            get { return mDocType; } 
            set { mDocType = value; }
        }
        private string mRefDocNo = string.Empty;
        /// <summary>
        /// Reference Document Number(E-WF claim No)
        /// </summary>
        public string RefDocNo { 
            get { return mRefDocNo; } 
            set { mRefDocNo = value; } 
        }

        private string mRefDocNo1 = string.Empty;
        /// <summary>
        /// Reference Document Number(E-WF claim No)
        /// </summary>
        public string RefDocNo1
        {
            get { return mRefDocNo1; }
            set { mRefDocNo1 = value; }
        }

        private string mHeader = string.Empty;
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Header { 
            get { return mHeader; } 
            set { mHeader = value; } 
        }
        private string mBusAct = "RFBU";
        /// <summary>
        /// Business Transaction
        /// </summary>
        public string BusAct { 
            get { return mBusAct; } 
            set { mBusAct = value; } 
        }
        private string mBusArea = "0001";
        /// <summary>
        /// 业务范围(Business Area)
        /// </summary>
        public string BusArea { 
            get { return mBusArea; } 
            set { mBusArea = value; } 
        }
        /// <summary>
        /// 用户ID (Employee ID)
        /// </summary>
        private string mEmployeeID = string.Empty;
        /// <summary>
        /// 信用卡持有人ID
        /// </summary>
        public string EmployeeID
        {
            get
            {
                return mEmployeeID;
            }
            set
            {
                mEmployeeID = new string('0', 10 - value.Length) + value;
            }
        }


        /// <summary>
        /// 用户名 (Employee Name)
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 信用卡支付
        /// </summary>
        public decimal PaidByCC { get; set; }
        /// <summary>
        /// 现金支付
        /// </summary>
        public decimal CashAmount { get; set; }
        private decimal mExchRate = 1;
        /// <summary>
        /// 汇率(Exchange rate),默认值为:1
        /// </summary>
        public decimal ExchRate
        {
            get
            {
                return mExchRate;
            }
            set
            {
                mExchRate = value;
            }
        }
        private string mCurrency = "RMB";
        /// <summary>
        /// 货币类型(Currency Key),默认值为：RMB
        /// </summary>
        public string Currency
        {
            get
            {
                return mCurrency;
            }
            set
            {
                mCurrency = value;
            }
        }

        private string mTaxRate = "0.17";
        /// <summary>
        /// 税率,默认值为：0.17
        /// </summary>
        public string TaxRate
        {
            get
            {
                return mTaxRate;
            }
            set
            {
                mTaxRate = value;
            }
        }

        /// <summary>
        /// 如果供应商ID为空
        /// </summary>
        public Vendor VendorInfo
        {
            set;
            get;
        }

        /// <summary>
        /// 信用卡支付，员工费用报销专用。员工报销中可能会有多项记录
        /// </summary>
        public CashAdvance[] CashAdvances { set; get; }
        /// <summary>
        /// 费用详细信息
        /// </summary>
        public List<ExpenceDetail> ExpenceDetails { get; set; }
        /// <summary>
        /// 聚合后的费用信息
        /// </summary>
        public List<ExpenceDetail> GroupExpenceDetails { get; set; }
        /// <summary>
        /// 采购订单详细信息项
        /// </summary>
        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<StoresReceiveItem> StoresReceiveItems { get; set;  } 
    }

    /// <summary>
    /// 员工费用报销 信用卡支付
    /// </summary>
    public class CashAdvance
    {
        public string ID { set; get; }
        public decimal CashAmount { set; get; }
    }

    /// <summary>
    /// 模型对象，只是用来传递数据
    /// </summary>
    public class ExpenceDetail
    {
        private string mBusArea = "0001";
        /// <summary>
        ///
        /// </summary>
        public string BusArea
        {
            get
            {
                return mBusArea;
            }
            set
            {
                mBusArea = value;
            }
        }

        private string mEmpID = string.Empty;
        /// <summary>
        /// 信用卡持有人ID
        /// </summary>
        public string EmpID
        {
            get
            {
                return mEmpID;
            }
            set
            {
                mEmpID = new string('0', 10 - value.Length) + value;
            }
        }
        /// <summary>
        /// 信用卡项的唯一标识，从LIST中获取
        /// </summary>
        public string RefKey { get; set; }

        /// <summary>
        /// 费用类型对应账号，如：Travel_hotel 对应 15511004，那么Account值为：15511004
        /// </summary>
        private string mAccountGL = string.Empty;
        /// <summary>
        /// 信用卡持有人ID
        /// </summary>
        public string AccountGL
        {
            get
            {
                return mAccountGL;
            }
            set
            {
                mAccountGL = new string('0', 10 - value.Length) + value;
            }
        }

        private string mItemText = string.Empty;
        /// <summary>
        /// //人名+费用类别,如：EricZheng + Travel_hotel
        /// </summary>
        public string ItemText 
        {
            get
            {
                return mItemText;
            }
            set
            {
                mItemText = value;
            }
        }
        
        /// <summary>
        /// Cost Center
        /// </summary>
        public string CostCenter { get; set; }
        
        /// <summary>
        /// 金额(Amount in document currency)
        /// </summary>
        public decimal Amount { get; set; }

        private decimal mExchRate = 1;
        /// <summary>
        /// 汇率(Exchange rate),默认值为:1
        /// </summary>
        public decimal ExchRate
        {
            get
            {
                return mExchRate;
            }
            set
            {
                mExchRate = value;
            }
        }

        private string mCurrency = "RMB";
        /// <summary>
        /// 货币类型(Currency Key),默认值为：RMB
        /// </summary>
        public string Currency
        {
            get
            {
                return mCurrency;
            }
            set
            {
                mCurrency = value;
            }
        }

        private decimal mCompanyStd = 10;
         /// <summary>
        /// 公司标准,默认值为100
        /// </summary>
        public decimal CompanyStd
        {
            get { return mCompanyStd; }
            set { mCompanyStd = value; }
        }

        private bool MIsPaidByCC = false;
        /// <summary>
        /// 是否信用卡支付,默认值为false
        /// </summary>
        public bool IsPaidByCC
        {
            get { return MIsPaidByCC; }
            set { MIsPaidByCC = value; }
        }

        private bool mIsApproved = true;
        /// <summary>
        /// 是否审批通过,默认值为true
        /// </summary>
        public bool IsApproved
        {
            get { return mIsApproved; }
            set { mIsApproved = value; }
        }

        private bool mIsNeedApproved = true;
        /// <summary>
        /// 是否需要审批,默认值为true
        /// </summary>
        public bool IsNeedApproved
        {
            get { return mIsNeedApproved; }
            set { mIsNeedApproved = value; }
        }
    }

    public class PurchaseOrderItem
    {
        private int mItemNo = 0;
        public int ItemNo
        {
            get { return mItemNo; }
            set { mItemNo = value * 10; }
        }

        private string mDescrption = string.Empty;
        public string Description
        {
            get { return mDescrption; }
            set { mDescrption = value; }
        }

        private string mMatlGroup = string.Empty;
        public string MatlGroup
        {
            get { return mMatlGroup; }
            set { mMatlGroup = value; }
        }

        private decimal mQuantity = 1m;
        public decimal Quantity
        {
            get { return mQuantity; }
            set { mQuantity = value; }
        }

        private string mTaxCode = string.Empty;
        public string TaxCode
        {
            get { return mTaxCode; }
            set { mTaxCode = value; }
        }

        private string mAcctasscat = string.Empty;
        public string Acctasscat
        {
            get { return mAcctasscat; }
            set { mAcctasscat = value; }
        }


        private string mCostCenter = string.Empty;
        public string CostCenter
        {
            get { return mCostCenter; }
            set { mCostCenter = value; }
        }

        private string mAssetNo = string.Empty;
        public string AssetNo
        {
            get { return mAssetNo; }
            set { mAssetNo = value; }
        }

        private decimal mCondValue;
        public decimal CondValue
        {
            get { return mCondValue; }
            set { mCondValue = value; }
        }

        private string mCurrency;
        public string Currency
        {
            get { return mCurrency; }
            set { mCurrency = value; }
        }
    }

    public class StoresReceiveItem
    {
        private decimal mQuantity = 0m;

        public decimal Quantity
        {
            get { return mQuantity; }
            set { mQuantity = value; }
        }

        private string mSapNumber = string.Empty;

        public string SapNumber
        {
            get { return mSapNumber; }
            set { mSapNumber = value; }
        }


        private string mItemText = string.Empty;

        public string ItemText
        {
            get
            {
                return mItemText;
            }
            set
            {
                mItemText = value;
            }
        }

        private int mItemNo = 0;

        public int ItemNo
        {
            get { return mItemNo; }
            set { mItemNo = value * 10; }
        }
    }

    public class Vendor
    {
        private string mName;
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mCity;
        public string City
        {
            get
            {
                return mCity;
            }
            set
            {
                mCity = value;
            }
        }

        private string mCountry;
        public string Country
        {
            get
            {
                return mCountry;
            }
            set
            {
                mCountry = value;
            }
        }

        private string mBankAcct;
        public string BankAcct
        {
            get
            {
                return mBankAcct;
            }
            set
            {
                mBankAcct = value;
            }
        }

        private string mBankNo;
        public string BankNo
        {
            get
            {
                return mBankNo;
            }
            set
            {
                mBankNo = value;
            }
        }

        private string mBankCity;
        public string BankCity
        {
            get
            {
                return mBankCity;
            }
            set
            {
                mBankCity = value;
            }
        }
    }
}
