using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal class ACCOUNTGL
    {
        private int mITEMNO_ACC = 0;
        /// <summary>
        ///  Accounting Document Line Item Number
        /// </summary>
        public int ITEMNO_ACC { get { return mITEMNO_ACC; } set { mITEMNO_ACC = value; } }

        private string mGL_ACCOUNT = string.Empty;
        /// <summary>
        /// General Ledger Account
        /// </summary>
        public string GL_ACCOUNT { get { return mGL_ACCOUNT; } set { mGL_ACCOUNT = value; } }

        private string mITEM_TEXT = string.Empty;
        /// <summary>
        /// Item Text
        /// </summary>
        public string ITEM_TEXT { get { return mITEM_TEXT; } set { mITEM_TEXT = value; } }

        private string mSTAT_CON = string.Empty;
        /// <summary>
        /// Indicator for statistical line items
        /// </summary>
        public string STAT_CON { get { return mSTAT_CON; } set { mSTAT_CON = value; } }

        private string mLOG_PROC = string.Empty;
        /// <summary>
        /// Logical Transaction
        /// </summary>
        public string LOG_PROC { get { return mLOG_PROC; } set { mLOG_PROC = value; } }

        private string mAC_DOC_NO = string.Empty;
        /// <summary>
        /// Accounting Document Number
        /// </summary>
        public string AC_DOC_NO { get { return mAC_DOC_NO; } set { mAC_DOC_NO = value; } }

        private string mREF_KEY_1 = string.Empty;
        /// <summary>
        /// Business Partner Reference Key
        /// </summary>
        public string REF_KEY_1 { get { return mREF_KEY_1; } set { mREF_KEY_1 = value; } }

        private string mREF_KEY_2 = string.Empty;
        /// <summary>
        /// Business Partner Reference Key
        /// </summary>
        public string REF_KEY_2 { get { return mREF_KEY_2; } set { mREF_KEY_2 = value; } }

        private string mREF_KEY_3 = string.Empty;
        /// <summary>
        /// Reference Key for Line Item
        /// </summary>
        public string REF_KEY_3 { get { return mREF_KEY_3; } set { mREF_KEY_3 = value; } }

        private string mACCT_KEY = string.Empty;
        /// <summary>
        /// Transaction Key
        /// </summary>
        public string ACCT_KEY { get { return mACCT_KEY; } set { mACCT_KEY = value; } }

        private string mACCT_TYPE = string.Empty;
        /// <summary>
        /// Account Type
        /// </summary>
        public string ACCT_TYPE { get { return mACCT_TYPE; } set { mACCT_TYPE = value; } }

        private string mDOC_TYPE = string.Empty;
        /// <summary>
        /// Document Type
        /// </summary>
        public string DOC_TYPE { get { return mDOC_TYPE; } set { mDOC_TYPE = value; } }

        private string mCOMP_CODE = string.Empty;
        /// <summary>
        /// Company Code
        /// </summary>
        public string COMP_CODE { get { return mCOMP_CODE; } set { mCOMP_CODE = value; } }

        private string mBUS_AREA = string.Empty;
        /// <summary>
        /// Business Area
        /// </summary>
        public string BUS_AREA { get { return mBUS_AREA; } set { mBUS_AREA = value; } }

        private string mFUNC_AREA = string.Empty;
        /// <summary>
        /// Functional Area
        /// </summary>
        public string FUNC_AREA { get { return mFUNC_AREA; } set { mFUNC_AREA = value; } }

        private string mPLANT = string.Empty;
        /// <summary>
        /// Site
        /// </summary>
        public string PLANT { get { return mPLANT; } set { mPLANT = value; } }

        private int mFIS_PERIOD = 0;
        /// <summary>
        /// Fiscal Period
        /// </summary>
        public int FIS_PERIOD { get { return mFIS_PERIOD; } set { mFIS_PERIOD = value; } }

        private int mFISC_YEAR = 0;
        /// <summary>
        /// Fiscal Year
        /// </summary>
        public int FISC_YEAR { get { return mFISC_YEAR; } set { mFISC_YEAR = value; } }

        private DateTime mPSTNG_DATE = DateTime.Now;
        /// <summary>
        /// Posting Date in the Document
        /// </summary>
        public DateTime PSTNG_DATE { get { return mPSTNG_DATE; } set { mPSTNG_DATE = value; } }

        private DateTime mVALUE_DATE = DateTime.Now;
        /// <summary>
        /// Value Date
        /// </summary>
        public DateTime VALUE_DATE { get { return mVALUE_DATE; } set { mVALUE_DATE = value; } }

        private string mFM_AREA = string.Empty;
        /// <summary>
        /// Financial Management Area
        /// </summary>
        public string FM_AREA { get { return mFM_AREA; } set { mFM_AREA = value; } }

        private string mCUSTOMER = string.Empty;
        /// <summary>
        /// Customer Number 1
        /// </summary>
        public string CUSTOMER { get { return mCUSTOMER; } set { mCUSTOMER = value; } }

        private string mCSHDIS_IND = string.Empty;
        /// <summary>
        /// Indicator: Line Item Not Liable to Cash Discount
        /// </summary>
        public string CSHDIS_IND { get { return mCSHDIS_IND; } set { mCSHDIS_IND = value; } }

        private string mVENDOR_NO = string.Empty;
        /// <summary>
        /// Account Number of Vendor or Creditor
        /// </summary>
        public string VENDOR_NO { get { return mVENDOR_NO; } set { mVENDOR_NO = value; } }

        private string mALLOC_NMBR = string.Empty;
        /// <summary>
        /// Assignment Number
        /// </summary>
        public string ALLOC_NMBR { get { return mALLOC_NMBR; } set { mALLOC_NMBR = value; } }

        private string mTAX_CODE = string.Empty;
        /// <summary>
        /// Sales Tax Code
        /// </summary>
        public string TAX_CODE { get { return mTAX_CODE; } set { mTAX_CODE = value; } }

        private string mTAXJURCODE = string.Empty;
        /// <summary>
        /// Tax Jurisdiction
        /// </summary>
        public string TAXJURCODE { get { return mTAXJURCODE; } set { mTAXJURCODE = value; } }

        private string mEXT_OBJECT_ID = string.Empty;
        /// <summary>
        /// Technical Key of External Object
        /// </summary>
        public string EXT_OBJECT_ID { get { return mEXT_OBJECT_ID; } set { mEXT_OBJECT_ID = value; } }

        private string mBUS_SCENARIO = string.Empty;
        /// <summary>
        /// Business Scenario in Controlling for Logistical Objects
        /// </summary>
        public string BUS_SCENARIO { get { return mBUS_SCENARIO; } set { mBUS_SCENARIO = value; } }

        private string mCOSTOBJECT = string.Empty;
        /// <summary>
        /// Cost Object
        /// </summary>
        public string COSTOBJECT { get { return mCOSTOBJECT; } set { mCOSTOBJECT = value; } }

        private string mCOSTCENTER = string.Empty;
        /// <summary>
        /// Cost Center
        /// </summary>
        public string COSTCENTER { get { return mCOSTCENTER; } set { mCOSTCENTER = value; } }

        private string mACTTYPE = string.Empty;
        /// <summary>
        /// Activity Type
        /// </summary>
        public string ACTTYPE { get { return mACTTYPE; } set { mACTTYPE = value; } }

        private string mPROFIT_CTR = string.Empty;
        /// <summary>
        /// Profit Center
        /// </summary>
        public string PROFIT_CTR { get { return mPROFIT_CTR; } set { mPROFIT_CTR = value; } }

        private string mPART_PRCTR = string.Empty;
        /// <summary>
        /// Partner Profit Center
        /// </summary>
        public string PART_PRCTR { get { return mPART_PRCTR; } set { mPART_PRCTR = value; } }

        private string mNETWORK = string.Empty;
        /// <summary>
        /// Network Number for Account Assignment
        /// </summary>
        public string NETWORK { get { return mNETWORK; } set { mNETWORK = value; } }

        private string mWBS_ELEMENT = string.Empty;
        /// <summary>
        /// Work Breakdown Structure Element (WBS Element)
        /// </summary>
        public string WBS_ELEMENT { get { return mWBS_ELEMENT; } set { mWBS_ELEMENT = value; } }

        private string mORDERID = string.Empty;
        /// <summary>
        /// Order Number
        /// </summary>
        public string ORDERID { get { return mORDERID; } set { mORDERID = value; } }

        private int mORDER_ITNO = 0;
        /// <summary>
        /// Order Item Number
        /// </summary>
        public int ORDER_ITNO { get { return mORDER_ITNO; } set { mORDER_ITNO = value; } }

        private int mROUTING_NO = 0;
        /// <summary>
        /// Routing number of operations in the order
        /// </summary>
        public int ROUTING_NO { get { return mROUTING_NO; } set { mROUTING_NO = value; } }

        private string mACTIVITY = string.Empty;
        /// <summary>
        /// Operation/Activity Number
        /// </summary>
        public string ACTIVITY { get { return mACTIVITY; } set { mACTIVITY = value; } }

        private string mCOND_TYPE = string.Empty;
        /// <summary>
        /// Condition type
        /// </summary>
        public string COND_TYPE { get { return mCOND_TYPE; } set { mCOND_TYPE = value; } }

        private int mCOND_COUNT = 0;
        /// <summary>
        /// Condition Counter
        /// </summary>
        public int COND_COUNT { get { return mCOND_COUNT; } set { mCOND_COUNT = value; } }

        private int mCOND_ST_NO = 0;
        /// <summary>
        /// Level Number
        /// </summary>
        public int COND_ST_NO { get { return mCOND_ST_NO; } set { mCOND_ST_NO = value; } }

        private string mFUND = string.Empty;
        /// <summary>
        /// Fund
        /// </summary>
        public string FUND { get { return mFUND; } set { mFUND = value; } }

        private string mFUNDS_CTR = string.Empty;
        /// <summary>
        /// Funds Center
        /// </summary>
        public string FUNDS_CTR { get { return mFUNDS_CTR; } set { mFUNDS_CTR = value; } }

        private string mCMMT_ITEM = string.Empty;
        /// <summary>
        /// Commitment Item
        /// </summary>
        public string CMMT_ITEM { get { return mCMMT_ITEM; } set { mCMMT_ITEM = value; } }

        private string mCO_BUSPROC = string.Empty;
        /// <summary>
        /// Business Process
        /// </summary>
        public string CO_BUSPROC { get { return mCO_BUSPROC; } set { mCO_BUSPROC = value; } }

        private string mASSET_NO = string.Empty;
        /// <summary>
        /// Main Asset Number
        /// </summary>
        public string ASSET_NO { get { return mASSET_NO; } set { mASSET_NO = value; } }

        private string mSUB_NUMBER = string.Empty;
        /// <summary>
        /// Asset Subnumber
        /// </summary>
        public string SUB_NUMBER { get { return mSUB_NUMBER; } set { mSUB_NUMBER = value; } }

        private string mBILL_TYPE = string.Empty;
        /// <summary>
        /// Billing Type
        /// </summary>
        public string BILL_TYPE { get { return mBILL_TYPE; } set { mBILL_TYPE = value; } }

        private string mSALES_ORD = string.Empty;
        /// <summary>
        /// Sales Order Number
        /// </summary>
        public string SALES_ORD { get { return mSALES_ORD; } set { mSALES_ORD = value; } }

        private int mS_ORD_ITEM = 0;
        /// <summary>
        /// Item Number in Sales Order
        /// </summary>
        public int S_ORD_ITEM { get { return mS_ORD_ITEM; } set { mS_ORD_ITEM = value; } }

        private string mDISTR_CHAN = string.Empty;
        /// <summary>
        /// Distribution Channel
        /// </summary>
        public string DISTR_CHAN { get { return mDISTR_CHAN; } set { mDISTR_CHAN = value; } }

        private string mDIVISION = string.Empty;
        /// <summary>
        /// Division
        /// </summary>
        public string DIVISION { get { return mDIVISION; } set { mDIVISION = value; } }

        private string mSALESORG = string.Empty;
        /// <summary>
        /// Sales Organization
        /// </summary>
        public string SALESORG { get { return mSALESORG; } set { mSALESORG = value; } }

        private string mSALES_GRP = string.Empty;
        /// <summary>
        /// Sales Group
        /// </summary>
        public string SALES_GRP { get { return mSALES_GRP; } set { mSALES_GRP = value; } }

        private string mSALES_OFF = string.Empty;
        /// <summary>
        /// Sales Office
        /// </summary>
        public string SALES_OFF { get { return mSALES_OFF; } set { mSALES_OFF = value; } }

        private string mSOLD_TO = string.Empty;
        /// <summary>
        /// Sold-to party
        /// </summary>
        public string SOLD_TO { get { return mSOLD_TO; } set { mSOLD_TO = value; } }

        private string mDE_CRE_IND = string.Empty;
        /// <summary>
        /// Indicator: subsequent debit/credit
        /// </summary>
        public string DE_CRE_IND { get { return mDE_CRE_IND; } set { mDE_CRE_IND = value; } }

        private string mP_EL_PRCTR = string.Empty;
        /// <summary>
        /// Partner profit center for elimination of internal business
        /// </summary>
        public string P_EL_PRCTR { get { return mP_EL_PRCTR; } set { mP_EL_PRCTR = value; } }

        private string mXMFRW = string.Empty;
        /// <summary>
        /// Indicator: Update quantity in RW
        /// </summary>
        public string XMFRW { get { return mXMFRW; } set { mXMFRW = value; } }

        private int mQUANTITY = 0;
        /// <summary>
        /// Quantity
        /// </summary>
        public int QUANTITY { get { return mQUANTITY; } set { mQUANTITY = value; } }

        private string mBASE_UOM = string.Empty;
        /// <summary>
        /// Base Unit of Measure
        /// </summary>
        public string BASE_UOM { get { return mBASE_UOM; } set { mBASE_UOM = value; } }

        private string mBASE_UOM_ISO = string.Empty;
        /// <summary>
        /// Base unit of measure in ISO code
        /// </summary>
        public string BASE_UOM_ISO { get { return mBASE_UOM_ISO; } set { mBASE_UOM_ISO = value; } }

        private int mINV_QTY = 0;
        /// <summary>
        /// Actual Invoiced Quantity
        /// </summary>
        public int INV_QTY { get { return mINV_QTY; } set { mINV_QTY = value; } }

        private int mINV_QTY_SU = 0;
        /// <summary>
        /// Billing quantity in stockkeeping unit
        /// </summary>
        public int INV_QTY_SU { get { return mINV_QTY_SU; } set { mINV_QTY_SU = value; } }

        private string mSALES_UNIT = string.Empty;
        /// <summary>
        /// Sales unit
        /// </summary>
        public string SALES_UNIT { get { return mSALES_UNIT; } set { mSALES_UNIT = value; } }

        private string mSALES_UNIT_ISO = string.Empty;
        /// <summary>
        /// Sales unit in ISO code
        /// </summary>
        public string SALES_UNIT_ISO { get { return mSALES_UNIT_ISO; } set { mSALES_UNIT_ISO = value; } }

        private int mPO_PR_QNT = 0;
        /// <summary>
        /// Quantity in order price quantity unit
        /// </summary>
        public int PO_PR_QNT { get { return mPO_PR_QNT; } set { mPO_PR_QNT = value; } }

        private string mPO_PR_UOM = string.Empty;
        /// <summary>
        /// Order price unit (purchasing)
        /// </summary>
        public string PO_PR_UOM { get { return mPO_PR_UOM; } set { mPO_PR_UOM = value; } }

        private string mPO_PR_UOM_ISO = string.Empty;
        /// <summary>
        /// Purchase order price unit in ISO code
        /// </summary>
        public string PO_PR_UOM_ISO { get { return mPO_PR_UOM_ISO; } set { mPO_PR_UOM_ISO = value; } }

        private int mENTRY_QNT = 0;
        /// <summary>
        /// Quantity in Unit of Entry
        /// </summary>
        public int ENTRY_QNT { get { return mENTRY_QNT; } set { mENTRY_QNT = value; } }

        private string mENTRY_UOM = string.Empty;
        /// <summary>
        /// Unit of Entry
        /// </summary>
        public string ENTRY_UOM { get { return mENTRY_UOM; } set { mENTRY_UOM = value; } }

        private string mENTRY_UOM_ISO = string.Empty;
        /// <summary>
        /// Unit of entry in ISO code
        /// </summary>
        public string ENTRY_UOM_ISO { get { return mENTRY_UOM_ISO; } set { mENTRY_UOM_ISO = value; } }

        private int mVOLUME = 0;
        /// <summary>
        /// Volume
        /// </summary>
        public int VOLUME { get { return mVOLUME; } set { mVOLUME = value; } }

        private string mVOLUMEUNIT = string.Empty;
        /// <summary>
        /// Volume unit
        /// </summary>
        public string VOLUMEUNIT { get { return mVOLUMEUNIT; } set { mVOLUMEUNIT = value; } }

        private string mVOLUMEUNIT_ISO = string.Empty;
        /// <summary>
        /// Volume unit in ISO code
        /// </summary>
        public string VOLUMEUNIT_ISO { get { return mVOLUMEUNIT_ISO; } set { mVOLUMEUNIT_ISO = value; } }

        private int mGROSS_WT = 0;
        /// <summary>
        /// Gross Weight
        /// </summary>
        public int GROSS_WT { get { return mGROSS_WT; } set { mGROSS_WT = value; } }

        private int mNET_WEIGHT = 0;
        /// <summary>
        /// Net weight
        /// </summary>
        public int NET_WEIGHT { get { return mNET_WEIGHT; } set { mNET_WEIGHT = value; } }

        private string mUNIT_OF_WT = string.Empty;
        /// <summary>
        /// Weight unit
        /// </summary>
        public string UNIT_OF_WT { get { return mUNIT_OF_WT; } set { mUNIT_OF_WT = value; } }

        private string mUNIT_OF_WT_ISO = string.Empty;
        /// <summary>
        /// Unit of weight in ISO code
        /// </summary>
        public string UNIT_OF_WT_ISO { get { return mUNIT_OF_WT_ISO; } set { mUNIT_OF_WT_ISO = value; } }

        private string mITEM_CAT = string.Empty;
        /// <summary>
        /// Item category in purchasing document
        /// </summary>
        public string ITEM_CAT { get { return mITEM_CAT; } set { mITEM_CAT = value; } }

        private string mMATERIAL = string.Empty;
        /// <summary>
        /// Article Number
        /// </summary>
        public string MATERIAL { get { return mMATERIAL; } set { mMATERIAL = value; } }

        private string mMATL_TYPE = string.Empty;
        /// <summary>
        /// Article Type
        /// </summary>
        public string MATL_TYPE { get { return mMATL_TYPE; } set { mMATL_TYPE = value; } }

        private string mMVT_IND = string.Empty;
        /// <summary>
        /// Movement Indicator
        /// </summary>
        public string MVT_IND { get { return mMVT_IND; } set { mMVT_IND = value; } }

        private string mREVAL_IND = string.Empty;
        /// <summary>
        /// Revaluation
        /// </summary>
        public string REVAL_IND { get { return mREVAL_IND; } set { mREVAL_IND = value; } }

        private string mORIG_GROUP = string.Empty;
        /// <summary>
        /// Origin Group as Subdivision of Cost Element
        /// </summary>
        public string ORIG_GROUP { get { return mORIG_GROUP; } set { mORIG_GROUP = value; } }

        private string mORIG_MAT = string.Empty;
        /// <summary>
        /// Article-related origin
        /// </summary>
        public string ORIG_MAT { get { return mORIG_MAT; } set { mORIG_MAT = value; } }

        private int mSERIAL_NO = 0;
        /// <summary>
        /// Sequential number of account assignment
        /// </summary>
        public int SERIAL_NO { get { return mSERIAL_NO; } set { mSERIAL_NO = value; } }

        private string mPART_ACCT = string.Empty;
        /// <summary>
        /// Partner Account Number
        /// </summary>
        public string PART_ACCT { get { return mPART_ACCT; } set { mPART_ACCT = value; } }

        private string mTR_PART_BA = string.Empty;
        /// <summary>
        /// Trading Partner's Business Area
        /// </summary>
        public string TR_PART_BA { get { return mTR_PART_BA; } set { mTR_PART_BA = value; } }

        private string mTRADE_ID = string.Empty;
        /// <summary>
        /// Company ID of Trading Partner
        /// </summary>
        public string TRADE_ID { get { return mTRADE_ID; } set { mTRADE_ID = value; } }

        private string mVAL_AREA = string.Empty;
        /// <summary>
        /// Valuation Area
        /// </summary>
        public string VAL_AREA { get { return mVAL_AREA; } set { mVAL_AREA = value; } }

        private string mVAL_TYPE = string.Empty;
        /// <summary>
        /// Valuation Type
        /// </summary>
        public string VAL_TYPE { get { return mVAL_TYPE; } set { mVAL_TYPE = value; } }

        private DateTime mASVAL_DATE = DateTime.Now;
        /// <summary>
        ///  Reference Date
        /// </summary>
        public DateTime ASVAL_DATE { get { return mASVAL_DATE; } set { mASVAL_DATE = value; } }

        private string mPO_NUMBER = string.Empty;
        /// <summary>
        /// Purchasing Document Number
        /// </summary>
        public string PO_NUMBER { get { return mPO_NUMBER; } set { mPO_NUMBER = value; } }

        private int mPO_ITEM = 0;
        /// <summary>
        /// Item Number of Purchasing Document
        /// </summary>
        public int PO_ITEM { get { return mPO_ITEM; } set { mPO_ITEM = value; } }

        private int mITM_NUMBER = 0;
        /// <summary>
        /// Item number of the SD document
        /// </summary>
        public int ITM_NUMBER { get { return mITM_NUMBER; } set { mITM_NUMBER = value; } }

        private string mCOND_CATEGORY = string.Empty;
        /// <summary>
        /// Condition Category (Examples: Tax, Freight, Price, Cost)
        /// </summary>
        public string COND_CATEGORY { get { return mCOND_CATEGORY; } set { mCOND_CATEGORY = value; } }

        private string mFUNC_AREA_LONG = string.Empty;
        /// <summary>
        /// Functional Area
        /// </summary>
        public string FUNC_AREA_LONG { get { return mFUNC_AREA_LONG; } set { mFUNC_AREA_LONG = value; } }

        private string mCMMT_ITEM_LONG = string.Empty;
        /// <summary>
        /// Commitment Item
        /// </summary>
        public string CMMT_ITEM_LONG { get { return mCMMT_ITEM_LONG; } set { mCMMT_ITEM_LONG = value; } }

        private string mGRANT_NBR = string.Empty;
        /// <summary>
        /// Grant
        /// </summary>
        public string GRANT_NBR { get { return mGRANT_NBR; } set { mGRANT_NBR = value; } }

        private string mCS_TRANS_T = string.Empty;
        /// <summary>
        /// Transaction Type
        /// </summary>
        public string CS_TRANS_T { get { return mCS_TRANS_T; } set { mCS_TRANS_T = value; } }

        private string mMEASURE = string.Empty;
        /// <summary>
        /// Funded Program
        /// </summary>
        public string MEASURE { get { return mMEASURE; } set { mMEASURE = value; } }

        private string mSEGMENT = string.Empty;
        /// <summary>
        /// Segment for Segmental Reporting
        /// </summary>
        public string SEGMENT { get { return mSEGMENT; } set { mSEGMENT = value; } }

        private string mPARTNER_SEGMENT = string.Empty;
        /// <summary>
        /// Partner Segment for Segmental Reporting
        /// </summary>
        public string PARTNER_SEGMENT { get { return mPARTNER_SEGMENT; } set { mPARTNER_SEGMENT = value; } }

        private string mRES_DOC = string.Empty;
        /// <summary>
        /// Document Number for Earmarked Funds
        /// </summary>
        public string RES_DOC { get { return mRES_DOC; } set { mRES_DOC = value; } }

        private int mRES_ITEM = 0;
        /// <summary>
        /// Earmarked Funds: Document Item
        /// </summary>
        public int RES_ITEM { get { return mRES_ITEM; } set { mRES_ITEM = value; } }
    }
}
