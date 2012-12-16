using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    /// <summary>
    /// 
    /// </summary>
    public class POCOND
    {
        private int mITM_NUMBER = 0;
        public int ITM_NUMBER { get { return mITM_NUMBER; } set { mITM_NUMBER = value; } }

        private string mCOND_TYPE = "PBXX";
        public string COND_TYPE { get { return mCOND_TYPE; } set { mCOND_TYPE = value; } }

        private decimal mCOND_VALUE = 0m;
        public decimal COND_VALUE { get { return mCOND_VALUE; } set { mCOND_VALUE = value; } }

        private string mCHANGE_ID = "U";
        public string CHANGE_ID { get { return mCHANGE_ID; } set { mCHANGE_ID = value; } }

        private string mCURRENCY = "RMB";
        public string CURRENCY { get { return mCURRENCY; } set { mCURRENCY = value; } }

        private string sFROM_PO = string.Empty;

        public string SFROM_PO
        {
            get { return sFROM_PO; }
            set { sFROM_PO = value; }
        }

    }
}
