using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    public class POACCOUNT
    {
        private int mPO_ITEM = 0;
        public int PO_ITEM { get { return mPO_ITEM; } set { mPO_ITEM = value; } }

        private string mCOSTCENTER = string.Empty;
        public string COSTCENTER { get { return mCOSTCENTER; } set { mCOSTCENTER = value; } }

        private string mASSET_NO = string.Empty;
        public string ASSET_NO { get { return mASSET_NO; } set { mASSET_NO = value; } }
    }
}
