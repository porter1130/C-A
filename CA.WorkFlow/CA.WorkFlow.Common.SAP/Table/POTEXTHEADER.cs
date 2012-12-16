using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    public class POTEXTHEADER
    {
        private string mTEXT_ID = string.Empty;
        public string TEXT_ID { get { return mTEXT_ID; } set { mTEXT_ID = value; } }

        private string mTEXT_FORM = "EN";
        public string TEXT_FORM { get { return mTEXT_FORM; } set { mTEXT_FORM = value; } }

        private string mTEXT_LINE = string.Empty;
        public string TEXT_LINE { get { return mTEXT_LINE; } set { mTEXT_LINE = value; } }
    }
}
