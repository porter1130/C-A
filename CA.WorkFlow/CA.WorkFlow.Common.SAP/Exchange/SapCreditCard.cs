using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 上传EXCEL时，POST数据到SAP
    /// </summary>
    internal class SapCreditCard : SapExchange
    {
        public SapCreditCard()
        {
            CurrentDestinationStr = SapDestinationNames.NCO_CREDITCARD;
        }

        /// <summary>
        /// 插入数据到 AccountPayable 表
        /// </summary>
        protected void SetAccountPayable()
        {
            int i = 1;               
            for (; i <= CurrentSapParameter.ExpenceDetails.Count; i++)
            {
                ExpenceDetail expen = CurrentSapParameter.ExpenceDetails[i - 1];
                SetAccountPayable(i, "Y", "", expen.RefKey, expen.ItemText, expen.Amount.ToString() + @"/" + expen.Currency, expen.EmpID);
            }

            SetAccountPayable(i, "", "", "", CurrentSapParameter.BankName, "", CurrentSapParameter.BankId);
        }

        private void SetAccountPayable(int INDEX, string SPGLIND, string ACCOUNTGL, string REFKEY, string ITEMTEXT, string ALLOCNMBR, string VENDORNO)
        {
            ACCOUNTPAYABLE accountPay = new ACCOUNTPAYABLE()
              {
                  PMNTTRMS = (string.IsNullOrEmpty(SPGLIND) ? "*" : ""),  
                  ITEMNO_ACC = INDEX,
                  SP_GL_IND = SPGLIND,
                  REF_KEY_1 = REFKEY,
                  GL_ACCOUNT = ACCOUNTGL,
                  ALLOC_NMBR = ALLOCNMBR,
                  ITEM_TEXT = ITEMTEXT,
                  VENDOR_NO = VENDORNO,
                  BUS_AREA = CurrentSapParameter.BusArea
              };

            InsertDataAccountPlayble(accountPay);
        }

        /// <summary>
        /// 插入数据到 CurrencyAmount 表
        /// </summary>
        protected void SetCurrencyAmount()
        {
            for (int i = 0, count = CurrentSapParameter.ExpenceDetails.Count; i <= count; )
            {
                decimal AMTDOCCUR = 0;
                if (i == count){
                    AMTDOCCUR = -(CurrentSapParameter.ExpenceDetails.Select(p => p.Amount)).Sum();
                }
                else{
                    AMTDOCCUR = CurrentSapParameter.ExpenceDetails[i].Amount;
                }

                CURRENCYAMOUNT currAmount = new CURRENCYAMOUNT()
                {
                    ITEMNO_ACC = ++i,
                    AMT_DOCCUR = AMTDOCCUR,
                    CURRENCY = CurrentSapParameter.Currency,
                    EXCH_RATE = CurrentSapParameter.ExchRate
                };

                InsertDataCurrencyAmount(currAmount, 0);
            }
        }

        /// <summary>
        /// 插入头数据
        /// </summary>
        protected void SetDocumentHeader()
        {
            DOCUMENTHEADER header = new DOCUMENTHEADER()
            {
                USERNAME = CurrentSapParameter.UserName,
                COMP_CODE = CurrentSapParameter.CompCode,
                DOC_TYPE = CurrentSapParameter.DocType,
                REF_DOC_NO = CurrentSapParameter.RefDocNo,
                HEADER_TXT = CurrentSapParameter.Header,
                BUS_ACT = CurrentSapParameter.BusAct,
                DOC_DATE = Convert.ToDateTime(CurrentSapParameter.DocDate),
                PSTNG_DATE = DateTime.Now
            };

            InsertDataDocumentHeader(header);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override void OperationForSap()
        {
            SetDocumentHeader();
            SetAccountPayable();
            SetCurrencyAmount();
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
            if (SapReturnObjKey != "$" && string.IsNullOrEmpty(SapReturnObjKey) == false)
            {
                sapResult.OBJ_KEY = SapReturnObjKey;
                sapResult.OBJ_SYS = SapReturnObjSys;
                sapResult.OBJ_TYPE = SapReturnObjType;
                return true;
            }
            return false;
        }
    }
}
