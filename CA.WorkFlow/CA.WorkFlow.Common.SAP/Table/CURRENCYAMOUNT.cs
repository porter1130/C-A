using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal class CURRENCYAMOUNT
    {
        private int mVENDOR_NO = 0;
        /// <summary>
        /// Accounting Document Line Item Number
        /// </summary>
        public int ITEMNO_ACC { get { return mVENDOR_NO; } set { mVENDOR_NO = value; } }

        private string mCURR_TYPE = string.Empty;
        /// <summary>
        /// Currency Type and Valuation View
        /// </summary>
        public string CURR_TYPE { get { return mCURR_TYPE; } set { mCURR_TYPE = value; } }

        private string mCURRENCY = string.Empty;
        /// <summary>
        /// Currency Key
        /// </summary>
        public string CURRENCY { get { return mCURRENCY; } set { mCURRENCY = value; } }

        private string mCURRENCY_ISO = string.Empty;
        /// <summary>
        /// ISO code currency
        /// </summary>
        public string CURRENCY_ISO { get { return mCURRENCY_ISO; } set { mCURRENCY_ISO = value; } }

        private decimal mAMT_DOCCUR = 0;
        /// <summary>
        /// Amount in document currency
        /// </summary>
        public decimal AMT_DOCCUR { get { return mAMT_DOCCUR; } set { mAMT_DOCCUR = value; } }

        private decimal mEXCH_RATE = 1;
        /// <summary>
        /// Exchange rate
        /// </summary>
        public decimal EXCH_RATE { get { return mEXCH_RATE; } set { mEXCH_RATE = value; } }

        private int mEXCH_RATE_V = 0;
        /// <summary>
        /// Indirect quoted exchange rate
        /// </summary>
        public int EXCH_RATE_V { get { return mEXCH_RATE_V; } set { mEXCH_RATE_V = value; } }

        private int mAMT_BASE = 0;
        /// <summary>
        /// Tax Base Amount in Document Currency
        /// </summary>
        public int AMT_BASE { get { return mAMT_BASE; } set { mAMT_BASE = value; } }

        private int mDISC_BASE = 0;
        /// <summary>
        /// Amount eligible for cash discount in document currency
        /// </summary>
        public int DISC_BASE { get { return mDISC_BASE; } set { mDISC_BASE = value; } }

        private int mDISC_AMT = 0;
        /// <summary>
        /// Cash discount amount in the currency of the currency types
        /// </summary>
        public int DISC_AMT { get { return mDISC_AMT; } set { mDISC_AMT = value; } }

        private int mTAX_AMT = 0;
        /// <summary>
        /// Amount in document currency
        /// </summary>
        public int TAX_AMT { get { return mTAX_AMT; } set { mTAX_AMT = value; } }
    }
}
