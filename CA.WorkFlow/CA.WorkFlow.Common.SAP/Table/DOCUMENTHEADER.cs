using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal class DOCUMENTHEADER
    {
        private string mObj_TYPE = string.Empty;
        /// <summary>
        /// Reference Transaction
        /// </summary>
        public string OBJ_TYPE { get { return mObj_TYPE; } set { mObj_TYPE = value; } }

        private string mOBJ_KEY = string.Empty;
        /// <summary>
        /// Reference Key
        /// </summary>
        public string OBJ_KEY { get { return mOBJ_KEY; } set { mOBJ_KEY = value; } }

        private string mOBJ_SYS = string.Empty;
        /// <summary>
        /// Logical system of source document
        /// </summary>
        public string OBJ_SYS { get { return mOBJ_SYS; } set { mOBJ_SYS = value; } }

        private string mBUS_ACT = string.Empty;
        /// <summary>
        /// Business Transaction
        /// </summary>
        public string BUS_ACT { get { return mBUS_ACT; } set { mBUS_ACT = value; } }

        private string mUSERNAME = string.Empty;
        /// <summary>
        /// User Name
        /// </summary>
        public string USERNAME { get { return mUSERNAME; } set { mUSERNAME = value; } }

        private string mHEADER_TXT = string.Empty;
        /// <summary>
        /// Document Header Text
        /// </summary>
        public string HEADER_TXT { get { return mHEADER_TXT; } set { mHEADER_TXT = value; } }

        private string mCOMP_CODE = string.Empty;
        /// <summary>
        /// Company Code
        /// </summary>
        public string COMP_CODE { get { return mCOMP_CODE; } set { mCOMP_CODE = value; } }

        private DateTime mDOC_DATE = DateTime.Now;
        /// <summary>
        /// Document Date in Document
        /// </summary>
        public DateTime DOC_DATE { get { return mDOC_DATE; } set { mDOC_DATE = value; } }

        private DateTime mPSTNG_DATE = DateTime.Now;
        /// <summary>
        /// Posting Date in the Document
        /// </summary>
        public DateTime PSTNG_DATE { get { return mPSTNG_DATE; } set { mPSTNG_DATE = value; } }

        private DateTime mTRANS_DATE = DateTime.Now;
        /// <summary>
        /// Translation Date
        /// </summary>
        public DateTime TRANS_DATE { get { return mTRANS_DATE; } set { mTRANS_DATE = value; } }

        private int mFISC_YEAR = 0;
        /// <summary>
        /// Fiscal Year
        /// </summary>
        public int FISC_YEAR { get { return mFISC_YEAR; } set { mFISC_YEAR = value; } }

        private int mFIS_PERIOD = 0;
        /// <summary>
        /// Fiscal Period
        /// </summary>
        public int FIS_PERIOD { get { return mFIS_PERIOD; } set { mFIS_PERIOD = value; } }

        private string mDOC_TYPE = string.Empty;
        /// <summary>
        /// Document Type
        /// </summary>
        public string DOC_TYPE { get { return mDOC_TYPE; } set { mDOC_TYPE = value; } }

        private string mREF_DOC_NO = string.Empty;
        /// <summary>
        /// Reference Document Number
        /// </summary>
        public string REF_DOC_NO { get { return mREF_DOC_NO; } set { mREF_DOC_NO = value; } }

        private string mAC_DOC_NO = string.Empty;
        /// <summary>
        /// Accounting Document Number
        /// </summary>
        public string AC_DOC_NO { get { return mAC_DOC_NO; } set { mAC_DOC_NO = value; } }

        private string mOBJ_KEY_R = string.Empty;
        /// <summary>
        /// Cancel: object key (AWREF_REV and AWORG_REV)
        /// </summary>
        public string OBJ_KEY_R { get { return mOBJ_KEY_R; } set { mOBJ_KEY_R = value; } }

        private string mREASON_REV = string.Empty;
        /// <summary>
        /// Reason for reversal
        /// </summary>
        public string REASON_REV { get { return mREASON_REV; } set { mREASON_REV = value; } }

        private string mCOMPO_ACC = string.Empty;
        /// <summary>
        /// Component in ACC Interface
        /// </summary>
        public string COMPO_ACC { get { return mCOMPO_ACC; } set { mCOMPO_ACC = value; } }

        private string mREF_DOC_NO_LONG = string.Empty;
        /// <summary>
        /// Reference Document Number (for Dependencies see Long Text)
        /// </summary>
        public string REF_DOC_NO_LONG { get { return mREF_DOC_NO_LONG; } set { mREF_DOC_NO_LONG = value; } }

        private string mACC_PRINCIPLE = string.Empty;
        /// <summary>
        /// Accounting Principle
        /// </summary>
        public string ACC_PRINCIPLE { get { return mACC_PRINCIPLE; } set { mACC_PRINCIPLE = value; } }

        private string mNEG_POSTNG = string.Empty;
        /// <summary>
        /// Indicator: Negative posting
        /// </summary>
        public string NEG_POSTNG { get { return mNEG_POSTNG; } set { mNEG_POSTNG = value; } }

        private string mOBJ_KEY_INV = string.Empty;
        /// <summary>
        /// Invoice Ref: Object Key (AWREF_REB and AWORG_REB)
        /// </summary>
        public string OBJ_KEY_INV { get { return mOBJ_KEY_INV; } set { mOBJ_KEY_INV = value; } }

        private string mBILL_CATEGORY = string.Empty;
        /// <summary>
        /// Billing category
        /// </summary>
        public string BILL_CATEGORY { get { return mBILL_CATEGORY; } set { mBILL_CATEGORY = value; } }

        private DateTime mVATDATE = DateTime.Now;
        /// <summary>
        /// Tax Reporting Date
        /// </summary>
        public DateTime VATDATE { get { return mVATDATE; } set { mVATDATE = value; } }
    }
}
