using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 方法扩展
    /// </summary>
    internal static class SapExtenMethods
    {
        /// <summary>
        /// 扩展 List<decimal> 类型对象的方法 ToDecimal（），返回 decimal[] 类型的数组对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static decimal[] ToDecimal<T>(this List<T> list)
        {
            decimal[] decs = new decimal[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                decs[i] = (decimal)(list[i] as object);
            }
            return decs;
        }
    }
}
