using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// PO 退货
    /// </summary>
    internal class SapPurchaseOrderRet : SapExchange
    {
        public SapPurchaseOrderRet()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_PO_CHANGE;
            CurrentDestinationStr = SapDestinationNames.NCO_PURCHASEORDERRET;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetPOReturnHeader()
        {
            InsertDataPOReturnHeader(new POHEADER() { PO_NUMBER = CurrentSapParameter.SapNumber });
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetPOReturnItem()
        {
            foreach (var item in CurrentSapParameter.PurchaseOrderItems)
            {
               // int itemNo = SapCommFunctions.GetPurchaseOrderItemCode(item.ItemNo);
                InsertDataPOReturnItem(new POITEM()
                {
                    PO_ITEM = item.ItemNo,
                    QUANTITY = item.Quantity,
                });
            }      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override void OperationForSap()
        {
            SetPOReturnHeader();
            SetPOReturnItem();
            Completed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {   
            sapResult = SapReturnResult;
            if (sapResult.RETURN_LIST[0].TYPE == "W" || sapResult.RETURN_LIST[0].TYPE == "S")
            {
                return true;
            }
            return false;
        }
    }
}
