using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 公用方法集合
    /// </summary>
    internal class SapCommFunctions
    {
        /// <summary>
        /// 补齐10位
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public static int GetPurchaseOrderItemCode(int itemCode)
        {
            int len = itemCode.ToString().Length > 4 ? 4 : itemCode.ToString().Length;
            return Convert.ToInt32(itemCode.ToString().Substring(0, len)) * 10;
        }
    }
}
