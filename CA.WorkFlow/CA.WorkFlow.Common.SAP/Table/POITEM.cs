using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    public class POITEM
    {
        private int mPO_ITEM = 0;
        public int PO_ITEM { get { return mPO_ITEM; } set { mPO_ITEM = value; } }

        private string mSHORT_TEXT = string.Empty;
        public string SHORT_TEXT { get { return mSHORT_TEXT; } set { mSHORT_TEXT = value; } }

        private string mPLANT = "HQAT";
        public string PLANT { get { return mPLANT; } set { mPLANT = value; } }

        private string mMATL_GROUP = string.Empty;
        public string MATL_GROUP { get { return mMATL_GROUP; } set { mMATL_GROUP = value; } }

        private decimal mQUANTITY = 0m;
        public decimal QUANTITY { get { return mQUANTITY; } set { mQUANTITY = value; } }

        private string mPO_UNIT = "PCS";
        public string PO_UNIT { get { return mPO_UNIT; } set { mPO_UNIT = value; } }

        private string mACCTASSCAT = "A";
        public string ACCTASSCAT { get { return mACCTASSCAT; } set { mACCTASSCAT = value; } }

        private string mTAX_CODE = "J0";
        public string TAX_CODE { get { return mTAX_CODE; } set { mTAX_CODE = value; } }

        private bool isPriceZero = false;
        /// <summary>
        /// 单价是否为零
        /// </summary>
        public bool IsPriceZero
        {
            get { return isPriceZero; }
            set { isPriceZero = value; }
        }
    }
}
