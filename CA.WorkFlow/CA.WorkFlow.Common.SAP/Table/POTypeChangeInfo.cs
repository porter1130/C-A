using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
   public class POTypeChangeInfo
    {
        string pAD;

        public string PAD
        {
            get { return pAD; }
            set { pAD = value; }
        }
        string sAD;

        public string SAD
        {
            get { return sAD; }
            set { sAD = value; }
        }
        string oMU;

        public string OMU
        {
            get { return oMU; }
            set { oMU = value; }
        }
        string qty;

        public string Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        bool isSuccess;
        /// <summary>
        /// 标识是否更新成功
        /// </summary>
        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        string isAllocated;
        
        public string IsAllocated
        {
            get { return isAllocated; }
            set { isAllocated = value; }
        }

        string sMessage;

        public string SMessage
        {
            get { return sMessage; }
            set { sMessage = value; }
        }
    }
}
