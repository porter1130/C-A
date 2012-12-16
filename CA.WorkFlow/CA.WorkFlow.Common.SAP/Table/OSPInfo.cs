using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    public class OSPInfo
    {
        string sUB_DIV;

        public string SUB_DIV
        {
            get { return sUB_DIV; }
            set { sUB_DIV = value; }
        }
        string cLASS;

        public string CLASS
        {
            get { return cLASS; }
            set { cLASS = value; }
        }
        string pO;

        public string PO
        {
            get { return pO; }
            set { pO = value; }
        }
        string qTY;

        public string QTY
        {
            get { return qTY; }
            set { qTY = value; }
        }
        string oRIGINAL_OSP;

        public string ORIGINAL_OSP
        {
            get { return oRIGINAL_OSP; }
            set { oRIGINAL_OSP = value; }
        }
        string cURRENT_OMU;

        public string CURRENT_OMU
        {
            get { return cURRENT_OMU; }
            set { cURRENT_OMU = value; }
        }
        string cREATED_BY;

        public string CREATED_BY
        {
            get { return cREATED_BY; }
            set { cREATED_BY = value; }
        }
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
        string gR;

        public string GR
        {
            get { return gR; }
            set { gR = value; }
        }
        string aLLOCATED_DATE;

        public string ALLOCATED_DATE
        {
            get { return aLLOCATED_DATE; }
            set { aLLOCATED_DATE = value; }
        }
        string mESSAGE;

        public string MESSAGE
        {
            get { return mESSAGE; }
            set { mESSAGE = value; }
        }

        string cOST = string.Empty;

        public string COST
        {
            get { return cOST; }
            set { cOST = value; }
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
    }
}
