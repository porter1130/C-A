using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    public class GRITEM
    {
        private string mITEM_TEXT = "HQAT";
        /// <summary>
        /// 
        /// </summary>
        public string ITEM_TEXT
        {
            get
            {
                return mITEM_TEXT;
            }
            set
            {
                mITEM_TEXT = value;
            }
        }

        private string mPLANT = "HQAT";
        /// <summary>
        /// Hard code : HQAT
        /// </summary>
        public string PLANT
        {
            get { return mPLANT; }
            set { mPLANT = value; }
        }

        private string mMOVE_TYPE = "101";
        /// <summary>
        /// Hard code : 101
        /// </summary>
        public string MOVE_TYPE
        {
            get { return mMOVE_TYPE; }
            set { mMOVE_TYPE = value; }
        }

        private decimal mENTRY_QNT = 0m;
        /// <summary>
        /// 
        /// </summary>
        public decimal ENTRY_QNT
        {
            get { return mENTRY_QNT; }
            set { mENTRY_QNT = value; }
        }

        private string mENTRY_UOM = "PCS";
        /// <summary>
        /// Hard code : PCS
        /// </summary>
        public string ENTRY_UOM
        {
            get { return mENTRY_UOM; }
            set { mENTRY_UOM = value; }
        }
        private string mPO_NUMBER = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string PO_NUMBER
        {
            get { return mPO_NUMBER; }
            set { mPO_NUMBER = value; }
        }
        private int mPO_ITEM;
        /// <summary>
        /// 
        /// </summary>
        public int PO_ITEM
        {
            get { return mPO_ITEM; }
            set { mPO_ITEM = value; }
        }
        private string mMVT_IND = "B";
        /// <summary>
        /// Hard code : B
        /// </summary>
        public string MVT_IND
        {
            get { return mMVT_IND; }
            set { mMVT_IND = value; }
        }

    }
}
