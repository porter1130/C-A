using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// PO 单收货，POST收货日期到SAP
    /// </summary>
    internal class SapPurchaseOrderMod : SapExchange
    {
        public SapPurchaseOrderMod()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_PO_CHANGE_DATE;
            CurrentDestinationStr = SapDestinationNames.NCO_PURCHASEORDERMOD;
        }

        /// <summary>
        /// 插入数据到 AccountPayable 表
        /// </summary>
        protected void SetPurchaseOrderDate()
        {
            UpdateDataPODate(new PO() { PONo = CurrentSapParameter.SapNumber, Date = CurrentSapParameter.DocDate });
        }

        protected override void OperationForSap()
        {
            SetPurchaseOrderDate();
            Completed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {
            sapResult = GetSapResult();
            if (SapReturnStatus == "Y" )
            {
                //Y 标示更新收货日期成功
                sapResult.OBJ_POINFO = new POINFO() { STATUS = "Y" };
                return true;
            }
            return false;
        }
    }
}
