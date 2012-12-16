using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    public class GRHEADER
    {
        private string mPSTNG_DATE = DateTime.Now.ToString("yyyyMMdd");
        public string PSTNG_DATE { get { return mPSTNG_DATE; } set { mPSTNG_DATE = value; } }

        private string mREF_DOC_NO = string.Empty;
        /// <summary>
        /// SAP中返回的PO number
        /// </summary>
        public string REF_DOC_NO { get { return mREF_DOC_NO; } set { mREF_DOC_NO = value; } }

        private string mDOC_DATE = DateTime.Now.ToString("yyyyMMdd");
        public string DOC_DATE { get { return mDOC_DATE; } set { mDOC_DATE = value; } }

        private string mHEADER_TXT = string.Empty;
        /// <summary>
        /// 工作流ID
        /// </summary>
        public string HEADER_TXT { get { return mHEADER_TXT; } set { mHEADER_TXT = value; } }

        private string mPR_UNAME = "acnotes";
        public string PR_UNAME
        {
            get
            {
                return mPR_UNAME;
            }
            set
            {
                mPR_UNAME = value;
            }
        }
    }
}
