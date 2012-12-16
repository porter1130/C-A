using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 查询某个PO单是否已经收货
    /// </summary>
    internal class SapPurchaseOrderQuery : SapExchange
    {
        public SapPurchaseOrderQuery()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_PO_QUERY_DATE;
            CurrentDestinationStr = SapDestinationNames.NCO_PURCHASEORDERQUERY;
        }

        /// <summary>
        /// 插入数据到 AccountPayable 表
        /// </summary>
        protected void GetPurchaseOrderInfo()
        {
            SelectDataPODate(new PO() { PONo = CurrentSapParameter.SapNumber });
        }

        protected override void OperationForSap()
        {
            GetPurchaseOrderInfo();
            Completed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {
            sapResult.OBJ_POINFO = GetPoInfo();
            return true;
        }
    }
}
