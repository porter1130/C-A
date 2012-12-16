using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 存储ABAP固定数据值
    /// </summary>
    internal class SapDestinationNames
    {
        public static readonly string NCO_TESTS_WITHOUT_POOL = "NCO_TESTS_WITHOUT_POOL";
        public static readonly string NCO_TRAVELCLAIM = ConfigurationManager.AppSettings["NCO_TRAVELCLAIM"].ToString();
        public static readonly string NCO_CASHADVANCE = ConfigurationManager.AppSettings["NCO_CASHADVANCE"].ToString();
        public static readonly string NCO_CREDITCARD = ConfigurationManager.AppSettings["NCO_CREDITCARD"].ToString();
        public static readonly string NCO_EMPLOYEECCCLAIM = ConfigurationManager.AppSettings["NCO_EMPLOYEECCCLAIM"].ToString();
        public static readonly string NCO_EMPLOYEECLAIM = ConfigurationManager.AppSettings["NCO_EMPLOYEECLAIM"].ToString();
        public static readonly string NCO_PURCHASEORDER = ConfigurationManager.AppSettings["NCO_PURCHASEORDER"].ToString();
        public static readonly string NCO_PURCHASEORDERMOD = ConfigurationManager.AppSettings["NCO_PURCHASEORDERMOD"].ToString();
        public static readonly string NCO_PURCHASEORDERQUERY = ConfigurationManager.AppSettings["NCO_PURCHASEORDERQUERY"].ToString();
        public static readonly string NCO_PURCHASEORDERRET = ConfigurationManager.AppSettings["NCO_PURCHASEORDERRET"].ToString();
        public static readonly string NCO_STORESRECEIVE = ConfigurationManager.AppSettings["NCO_STORESRECEIVE"].ToString();
        public static readonly string NCO_OSP = ConfigurationManager.AppSettings["NCO_OSP"].ToString();
        public static readonly string NCO_POTYPE = ConfigurationManager.AppSettings["NCO_POTYPE"].ToString();
    }
}
