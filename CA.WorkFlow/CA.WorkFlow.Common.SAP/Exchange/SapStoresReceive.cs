using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 收货
    /// </summary>
    internal class SapStoresReceive : SapExchange
    {
        public SapStoresReceive()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_GR_CREATE;
            CurrentDestinationStr = SapDestinationNames.NCO_STORESRECEIVE;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetGRHeader()
        {
            InsertDataGRHeader(new GRHEADER()
            {
                DOC_DATE = CurrentSapParameter.DocDate,
                PSTNG_DATE = CurrentSapParameter.DocDate,
                HEADER_TXT = CurrentSapParameter.Header,
                REF_DOC_NO = CurrentSapParameter.RefDocNo,
                PR_UNAME = CurrentSapParameter.UserName
            });
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetGRItem()
        {
            foreach (var item in CurrentSapParameter.StoresReceiveItems)
            {
                InsertDataGRItem(new GRITEM()
                {
                    PO_ITEM = item.ItemNo,
                    ENTRY_QNT = item.Quantity,
                    PO_NUMBER = item.SapNumber,
                    ITEM_TEXT = item.ItemText 
                });
            }
        }

        protected void SetGRCode()
        {
            InsertDataGRCode(new GRCODE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override void OperationForSap()
        {
            SetGRHeader();
            SetGRCode();
            SetGRItem();
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
            if (string.IsNullOrEmpty(SapReturnMatErialDocument) == false)
            {
                sapResult.OBJ_KEY = SapReturnMatDocumentYear + "_" + SapReturnMatErialDocument;
                return true;
            }
            return false;
        }
    }
}

