using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 发票验证
    /// </summary>
    internal class SapPurchaseIV : SapExchange
    {
        public SapPurchaseIV()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_IV_CREATE;
            CurrentDestinationStr = SapDestinationNames.NCO_PURCHASEORDERQUERY;
        }
        protected override void OperationForSap()
        {
            InsertDataHeaderData(null);
            InsertDataIVItemData(null);
            Completed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {
            GetVionceInfo();
            sapResult = GetSapResult();
            return true;
        }
    }
}
