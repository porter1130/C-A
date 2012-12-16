using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// PO采购订单
    /// </summary>
    internal class SapPurchaseOrder : SapExchange
    {
        public SapPurchaseOrder()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_PO_CREATE;
            CurrentDestinationStr = SapDestinationNames.NCO_PURCHASEORDER;
        }

        /// <summary>
        ///  插入头数据
        /// </summary>
        protected void SetPOHeader()
        {
            InsertDataPOHeader(new POHEADER()
            {
                PUR_GROUP = CurrentSapParameter.PurGroup,
                DOC_DATE = CurrentSapParameter.DocDate,
                PMNTTRMS = CurrentSapParameter.Pmnttrms,
                VENDOR = CurrentSapParameter.Vendor,
                Currency = CurrentSapParameter.Currency,
                CREATED_BY = CurrentSapParameter.UserName
            });

            InsertDataPOTextHeader(new POTEXTHEADER()
            {
                TEXT_ID = "F21",
                TEXT_FORM = "EN",
                TEXT_LINE = CurrentSapParameter.RefDocNo
            });

            InsertDataPOTextHeader(new POTEXTHEADER()
            {
                TEXT_ID = "F22",
                TEXT_FORM = "EN",
                TEXT_LINE = CurrentSapParameter.PaymentCond
            });
        }

        /// <summary>
        /// 插入PO Itemn
        /// </summary>
        protected void SetPOItem()
        {
            foreach (var item in CurrentSapParameter.PurchaseOrderItems)
            {
               // int itemNo = SapCommFunctions.GetPurchaseOrderItemCode(item.ItemNo);
                bool isPriceZero = false;
                decimal dPrice = item.CondValue;
                if (dPrice == 0)
                {
                    isPriceZero = true;
                }
                InsertDataPOItem(new POITEM()
                {
                    PO_ITEM = item.ItemNo,
                    SHORT_TEXT = item.Description,
                    MATL_GROUP = item.MatlGroup,
                    QUANTITY = item.Quantity,
                    TAX_CODE = item.TaxCode,
                    ACCTASSCAT = item.Acctasscat,
                    IsPriceZero=isPriceZero
                });

                InsertDataPOAccount(new POACCOUNT()
                {
                    PO_ITEM = item.ItemNo,
                    COSTCENTER = item.CostCenter,
                    ASSET_NO = item.AssetNo
                });

                InsertDataPOCond(new POCOND()
                {
                    ITM_NUMBER = item.ItemNo,
                    COND_VALUE =dPrice,// item.CondValue,
                    CURRENCY = item.Currency
                });
            }      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override void OperationForSap()
        {
            SetPOHeader();
            SetPOItem();
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
            if (SapReturnExpPurchaseOrder != "$" && string.IsNullOrEmpty(SapReturnExpPurchaseOrder) == false)
            {
                sapResult.OBJ_KEY = SapReturnExpPurchaseOrder;
                return true;
            }
            return false;
        }
    }
}
