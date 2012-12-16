using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal class VENDOR
    {
        private string mNAME = string.Empty;
        /// <summary>
        /// 供应商名字，最长34位
        /// </summary>
        public string NAME { get { return mNAME; } set { mNAME = value; } }

         private string mNAME_2 = string.Empty;
        /// <summary>
        /// 多余的供应商名字
        /// </summary>
        public string NAME_2 { get { return mNAME_2; } set { mNAME_2 = value; } }

         private string mCITY = string.Empty;
        /// <summary>
        /// 供应商所在地
        /// </summary>
        public string CITY { get { return mCITY; } set { mCITY = value; } }

         private string mCOUNTRY = string.Empty;
        /// <summary>
        /// 应商所在国家
        /// </summary>
        public string COUNTRY { get { return mCOUNTRY; } set { mCOUNTRY = value; } }

         private string mBANK_ACCT = string.Empty;
        /// <summary>
        /// 银行账号，最长18位
        /// </summary>
        public string BANK_ACCT { get { return mBANK_ACCT; } set { mBANK_ACCT = value; } }

        private string mBKREF = string.Empty;
        /// <summary>
        /// 多余的银行账号
        /// </summary>
         public string BKREF
         {
             get
             {
                 return mBKREF;
             }
             set
             {
                 mBKREF = value;
             }
         }

         private string mBANK_NO = string.Empty;
        /// <summary>
        /// 银行KEY,银行代码，必须在SAP存在
        /// </summary>
        public string BANK_NO { get { return mBANK_NO; } set { mBANK_NO = value; } }

         private string mBANK_CTRY = string.Empty;
        /// <summary>
        /// 银行所在国，默认CN
        /// </summary>
        public string BANK_CTRY { get { return mBANK_CTRY; } set { mBANK_CTRY = value; } }
    }
}
