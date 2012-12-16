using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal class ACCOUNTPAYABLE
    {
        private int mITEMNO_ACC = 0;
        /// <summary>
        /// Accounting Document Line Item Number
        /// </summary>
        public int ITEMNO_ACC { get { return mITEMNO_ACC; } set { mITEMNO_ACC = value; } }

        private string mVENDOR_NO = string.Empty;
        /// <summary>
        /// Account Number of Vendor or Creditor
        /// </summary>
        public string VENDOR_NO { get { return mVENDOR_NO; } set { mVENDOR_NO = value; } }

        private string mGL_ACCOUNT = string.Empty;
        /// <summary>
        /// General Ledger Account
        /// </summary>
        public string GL_ACCOUNT { get { return mGL_ACCOUNT; } set { mGL_ACCOUNT = value; } }

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

        private string mPMNTTRMS = string.Empty;
        /// <summary>
        /// Terms of Payment Key
        /// </summary>
        public string PMNTTRMS { get { return mPMNTTRMS; } set { mPMNTTRMS = value; } }

        private DateTime mBLINE_DATE = DateTime.Now;
        /// <summary>
        /// Baseline Date For Due Date Calculation
        /// </summary>
        public DateTime BLINE_DATE { get { return mBLINE_DATE; } set { mBLINE_DATE = value; } }

        private int mDSCT_DAYS1 = 0;
        /// <summary>
        /// Days for first cash discount
        /// </summary>
        public int DSCT_DAYS1 { get { return mDSCT_DAYS1; } set { mDSCT_DAYS1 = value; } }

        private int mDSCT_DAYS2 = 0;
        /// <summary>
        /// Days for second cash discount
        /// </summary>
        public int DSCT_DAYS2 { get { return mDSCT_DAYS2; } set { mDSCT_DAYS2 = value; } }

        private int mNETTERMS = 0;
        /// <summary>
        /// Deadline for net conditions
        /// </summary>
        public int NETTERMS { get { return mNETTERMS; } set { mNETTERMS = value; } }

        private int mDSCT_PCT1 = 0;
        /// <summary>
        /// Percentage for First Cash Discount
        /// </summary>
        public int DSCT_PCT1 { get { return mDSCT_PCT1; } set { mDSCT_PCT1 = value; } }

        private int mDSCT_PCT2 = 0;
        /// <summary>
        /// Percentage for Second Cash Discount
        /// </summary>
        public int DSCT_PCT2 { get { return mDSCT_PCT2; } set { mDSCT_PCT2 = value; } }

        private string mPYMT_METH = string.Empty;
        /// <summary>
        /// Payment method
        /// </summary>
        public string PYMT_METH { get { return mPYMT_METH; } set { mPYMT_METH = value; } }

        private string mPMTMTHSUPL = string.Empty;
        /// <summary>
        /// Payment Method Supplement
        /// </summary>
        public string PMTMTHSUPL { get { return mPMTMTHSUPL; } set { mPMTMTHSUPL = value; } }

        private string mPMNT_BLOCK = string.Empty;
        /// <summary>
        /// Payment block key
        /// </summary>
        public string PMNT_BLOCK { get { return mPMNT_BLOCK; } set { mPMNT_BLOCK = value; } }

        private string mSCBANK_IND = string.Empty;
        /// <summary>
        /// State Central Bank Indicator
        /// </summary>
        public string SCBANK_IND { get { return mSCBANK_IND; } set { mSCBANK_IND = value; } }

        private string mSUPCOUNTRY = string.Empty;
        /// <summary>
        /// Supplying Country
        /// </summary>
        public string SUPCOUNTRY { get { return mSUPCOUNTRY; } set { mSUPCOUNTRY = value; } }

        private string mSUPCOUNTRY_ISO = string.Empty;
        /// <summary>
        /// Supplier country ISO code
        /// </summary>
        public string SUPCOUNTRY_ISO { get { return mSUPCOUNTRY_ISO; } set { mSUPCOUNTRY_ISO = value; } }

        private string mBLLSRV_IND = string.Empty;
        /// <summary>
        /// Service Indicator (Foreign Payment)
        /// </summary>
        public string BLLSRV_IND { get { return mBLLSRV_IND; } set { mBLLSRV_IND = value; } }

        private string mALLOC_NMBR = string.Empty;
        /// <summary>
        /// Assignment Number
        /// </summary>
        public string ALLOC_NMBR { get { return mALLOC_NMBR; } set { mALLOC_NMBR = value; } }

        private string mITEM_TEXT = string.Empty;
        /// <summary>
        /// Item Text
        /// </summary>
        public string ITEM_TEXT { get { return mITEM_TEXT; } set { mITEM_TEXT = value; } }

        private string mPO_SUB_NO = string.Empty;
        /// <summary>
        /// ISR Subscriber Number
        /// </summary>
        public string PO_SUB_NO { get { return mPO_SUB_NO; } set { mPO_SUB_NO = value; } }

        private string mPO_CHECKDG = string.Empty;
        /// <summary>
        /// ISR Check Digit
        /// </summary>
        public string PO_CHECKDG { get { return mPO_CHECKDG; } set { mPO_CHECKDG = value; } }

        private string mPO_REF_NO = string.Empty;
        /// <summary>
        /// ISR Reference Number
        /// </summary>
        public string PO_REF_NO { get { return mPO_REF_NO; } set { mPO_REF_NO = value; } }

        private string mW_TAX_CODE = string.Empty;
        /// <summary>
        /// Withholding tax code
        /// </summary>
        public string W_TAX_CODE { get { return mW_TAX_CODE; } set { mW_TAX_CODE = value; } }

        private string mBUSINESSPLACE = string.Empty;
        /// <summary>
        /// Stores
        /// </summary>
        public string BUSINESSPLACE { get { return mBUSINESSPLACE; } set { mBUSINESSPLACE = value; } }

        private string mSECTIONCODE = string.Empty;
        /// <summary>
        /// Section Code
        /// </summary>
        public string SECTIONCODE { get { return mSECTIONCODE; } set { mSECTIONCODE = value; } }

        private int mINSTR1 = 0;
        /// <summary>
        /// Instruction Key 1
        /// </summary>
        public int INSTR1 { get { return mINSTR1; } set { mINSTR1 = value; } }

        private int mINSTR2 = 0;
        /// <summary>
        /// Instruction Key 2
        /// </summary>
        public int INSTR2 { get { return mINSTR2; } set { mINSTR2 = value; } }

        private int mINSTR3 = 0;
        /// <summary>
        /// Instruction Key 3
        /// </summary>
        public int INSTR3 { get { return mINSTR3; } set { mINSTR3 = value; } }

        private int mINSTR4 = 0;
        /// <summary>
        /// Instruction Key 4
        /// </summary>
        public int INSTR4 { get { return mINSTR4; } set { mINSTR4 = value; } }

        private string mBRANCH = string.Empty;
        /// <summary>
        /// Account number of the branch
        /// </summary>
        public string BRANCH { get { return mBRANCH; } set { mBRANCH = value; } }

        private string mPYMT_CUR = string.Empty;
        /// <summary>
        /// Currency for automatic payment
        /// </summary>
        public string PYMT_CUR { get { return mPYMT_CUR; } set { mPYMT_CUR = value; } }

        private int mPYMT_AMT = 0;
        /// <summary>
        /// Amount in Payment Currency
        /// </summary>
        public int PYMT_AMT { get { return mPYMT_AMT; } set { mPYMT_AMT = value; } }

        private string mPYMT_CUR_ISO = string.Empty;
        /// <summary>
        /// ISO code currency
        /// </summary>
        public string PYMT_CUR_ISO { get { return mPYMT_CUR_ISO; } set { mPYMT_CUR_ISO = value; } }

        private string mSP_GL_IND = string.Empty;
        /// <summary>
        /// Special G/L Indicator
        /// </summary>
        public string SP_GL_IND { get { return mSP_GL_IND; } set { mSP_GL_IND = value; } }

        private string mTAX_CODE = string.Empty;
        /// <summary>
        /// Sales Tax Code
        /// </summary>
        public string TAX_CODE { get { return mTAX_CODE; } set { mTAX_CODE = value; } }

        private DateTime mTAX_DATE = DateTime.Now;
        /// <summary>
        /// Date Relevant for Determining the Tax Rate
        /// </summary>
        public DateTime TAX_DATE { get { return mTAX_DATE; } set { mTAX_DATE = value; } }

        private string mTAXJURCODE = string.Empty;
        /// <summary>
        /// Tax Jurisdiction
        /// </summary>
        public string TAXJURCODE { get { return mTAXJURCODE; } set { mTAXJURCODE = value; } }

        private string mALT_PAYEE = string.Empty;
        /// <summary>
        /// Alternative payee
        /// </summary>
        public string ALT_PAYEE { get { return mALT_PAYEE; } set { mALT_PAYEE = value; } }

        private string mALT_PAYEE_BANK = string.Empty;
        /// <summary>
        /// Bank type of alternative payer
        /// </summary>
        public string ALT_PAYEE_BANK { get { return mALT_PAYEE_BANK; } set { mALT_PAYEE_BANK = value; } }

        private string mPARTNER_BK = string.Empty;
        /// <summary>
        /// Partner Bank Type
        /// </summary>
        public string PARTNER_BK { get { return mPARTNER_BK; } set { mPARTNER_BK = value; } }

        private string mBANK_ID = string.Empty;
        /// <summary>
        /// Short Key for a House Bank
        /// </summary>
        public string BANK_ID { get { return mBANK_ID; } set { mBANK_ID = value; } }

        private string mPARTNER_GUID = string.Empty;
        /// <summary>
        /// Com. Interface: Business Partner GUID
        /// </summary>
        public string PARTNER_GUID { get { return mPARTNER_GUID; } set { mPARTNER_GUID = value; } }

        private string mPROFIT_CTR = string.Empty;
        /// <summary>
        /// Profit Center
        /// </summary>
        public string PROFIT_CTR { get { return mPROFIT_CTR; } set { mPROFIT_CTR = value; } }

        private string mFUND = string.Empty;
        /// <summary>
        /// Fund
        /// </summary>
        public string FUND { get { return mFUND; } set { mFUND = value; } }

        private string mGRANT_NBR = string.Empty;
        /// <summary>
        ///  Grant
        /// </summary>
        public string GRANT_NBR { get { return mGRANT_NBR; } set { mGRANT_NBR = value; } }

        private string mMEASURE = string.Empty;
        /// <summary>
        /// Funded Program
        /// </summary>
        public string MEASURE { get { return mMEASURE; } set { mMEASURE = value; } }

        private string mHOUSEBANKACCTID = string.Empty;
        /// <summary>
        /// ID for Account Details
        /// </summary>
        public string HOUSEBANKACCTID { get { return mHOUSEBANKACCTID; } set { mHOUSEBANKACCTID = value; } }
    }
}
