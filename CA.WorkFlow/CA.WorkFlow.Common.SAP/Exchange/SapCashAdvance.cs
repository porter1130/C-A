using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// Cash Advance
    /// </summary>
    internal class SapCashAdvance : SapExchange
    {
        public SapCashAdvance()
        {
            CurrentDestinationStr = SapDestinationNames.NCO_CASHADVANCE;
        }

        /// <summary>
        /// 插入数据到 AccountPayable 表
        /// </summary>
        protected void SetAccountPayable()
        {
            //Cash Advance 只需插入固定的两行记录到SAP中的两个表
            for (int i = 1; i < 3; i++)   
            {
                string SPGLIND = i == 1 ? "" : "V";
                string ITEMTEXT = CurrentSapParameter.Header;

                ACCOUNTPAYABLE accountPay = new ACCOUNTPAYABLE()
                {
                    PMNTTRMS = (string.IsNullOrEmpty(SPGLIND) ? "*" : ""),
                    ITEMNO_ACC = i,
                    SP_GL_IND = SPGLIND,
                    ITEM_TEXT = ITEMTEXT,
                    PYMT_METH = CurrentSapParameter.PymtMeth,
                    REF_KEY_1 = CurrentSapParameter.RefDocNo,
                    VENDOR_NO = CurrentSapParameter.EmployeeID,
                    BUS_AREA = CurrentSapParameter.BusArea
                };

                InsertDataAccountPlayble(accountPay);
            }
        }

        /// <summary>
        /// 插入数据到 CurrencyAmount 表
        /// </summary>
        protected void SetCurrencyAmount()
        {
            //Cash Advance 只需插入固定的两行记录到SAP中的两个表
            for (int i = 1; i < 3; i++)
            {
                //当 i==1 时，AMTDOCCUR为负数，当i==2 时，AMTDOCCUR 值为整数
                decimal AMTDOCCUR = (decimal)(CurrentSapParameter.CashAmount * (i == 1 ? -1 : 1));
                CURRENCYAMOUNT currAmount = new CURRENCYAMOUNT()
                {
                    ITEMNO_ACC = i,
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

        protected override void OperationForSap()
        {
            SetDocumentHeader();
            SetAccountPayable();
            SetCurrencyAmount();
            Completed();
        }

        /// <summary>
        /// POST SAP完成后需要处理的操作，获取处理结果
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
