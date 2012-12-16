using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal class ACCOUNTTAX
    {
        private int mVENDOR_NO = 0;
        /// <summary>
        /// Accounting Document Line Item Number
        /// </summary>
        public int ITEMNO_ACC
        {
            get
            {
                return mVENDOR_NO;
            }
            set
            {
                mVENDOR_NO = value;
            }
        }

        private int mTAX_CODE = 0;
        /// <summary>
        /// 
        /// </summary>
        public int TAX_CODE
        {
            get
            {
                return mTAX_CODE;
            }
            set
            {
                mTAX_CODE = value;
            }
        }
    }
       
}
