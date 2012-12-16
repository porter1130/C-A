using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table 
{
    public class POHEADER
    {
        public string PO_NUMBER { get; set; }

        private string mCOMP_CODE = "CA10";
        public string COMP_CODE { get { return mCOMP_CODE; } set { mCOMP_CODE = value; } }

        private string mDOC_TYPE = "ZNT";
        public string DOC_TYPE { get { return mDOC_TYPE; } set { mDOC_TYPE = value; } }

        private string mVENDOR = string.Empty;
        public string VENDOR { get { return mVENDOR; } set { mVENDOR = value; } }

        private string mPURCH_ORG = "TALL";
        public string PURCH_ORG { get { return mPURCH_ORG; } set { mPURCH_ORG = value; } }

        private string mPUR_GROUP = "AST";
        public string PUR_GROUP { get { return mPUR_GROUP; } set { mPUR_GROUP = value; } }

        private string mDOC_DATE = string.Empty;
        public string DOC_DATE { get { return mDOC_DATE; } set { mDOC_DATE = value; } }

        private string mPMNTTRMS = "Z002";
        public string PMNTTRMS { get { return mPMNTTRMS; } set { mPMNTTRMS = value; } }

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

        private string mCREATED_BY = "acnotes";
        public string CREATED_BY
        {
            get
            {
                return mCREATED_BY;
            }
            set 
            {
                mCREATED_BY = value;
            }
        }
    }
}
