using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 员工报销
    /// </summary>
    internal class SapEmployeeClaim : SapExchangeClaim
    {
        public SapEmployeeClaim()
        {
            CurrentDestinationStr = SapDestinationNames.NCO_EMPLOYEECLAIM;
        }

        /// <summary>
        /// 实现父类的抽象方法, 返回所有需要插入到SAP中的CashAdvance项
        /// </summary>
        protected override List<object[]> GetCashAdvance()
        {
            List<object[]> cashList = new List<object[]>();
            CashAdvance[] cashAdvances = CurrentSapParameter.CashAdvances;
            if (cashAdvances != null)
            {
                for (int i = 0; i < cashAdvances.Length; i++)
                {
                    cashList.Add(new object[] { cashAdvances[i].ID, cashAdvances[i].CashAmount });
                }
            }

            return cashList;
        }

        /// <summary>
        /// 实现父类的抽象方法，返回CashAmount值
        /// </summary>
        /// <returns></returns>
        protected override decimal GetCashAmount()
        {
            decimal cashAmount = 0;
            if (CurrentSapParameter.CashAdvances != null)
            {
                for (int i = 0; i < CurrentSapParameter.CashAdvances.Length; i++)
                {
                    cashAmount += CurrentSapParameter.CashAdvances[i].CashAmount;
                }
            }
            return cashAmount;
        }

        /// <summary>
        /// 实现父类的抽象方法，返回PaidByCC值
        /// </summary>
        /// <returns></returns>
        protected override decimal GetPaidByCCAmount()
        {
            return CurrentSapParameter.PaidByCC;
        }

        /// <summary>
        /// 实现父类的抽象方法
        /// </summary>
        /// <returns></returns>
        protected override List<ExpenceDetail> GetExpenceDetails()
        {
            return CurrentSapParameter.ExpenceDetails;
        }

        /// <summary>
        /// 员工报销中，会存在信用卡支付
        /// </summary>
        /// <returns></returns>
        protected override List<object[]> GetPaidByCC()
        {
            List<object[]> cc = new List<object[]>();
            cc.Add(new object[] { "", CurrentSapParameter.PaidByCC.ToString() + @"/" + CurrentSapParameter.Currency, -CurrentSapParameter.PaidByCC });
            return cc;
        }

        /// <summary>
        /// 计算总金额
        /// </summary>
        /// <returns></returns>
        protected override decimal GetTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var expen in CurrentSapParameter.ExpenceDetails){
                totalAmount += expen.Amount * expen.ExchRate;
            }
            return totalAmount;
        }

        /// <summary>
        /// 插入数据到第二个表：AccountPayable
        /// </summary>
        /// <param name="GlobalCount"></param>
        /// <param name="SPGLIND"></param>
        /// <param name="ITEMTEXT"></param>
        protected override void SetAccountPayable(int globalCount, string SPGLIND, string REFKEY, string ALLOCNMBR, string ITEMTEXT)
        {
            ACCOUNTPAYABLE accountPay = new ACCOUNTPAYABLE()
            {
                PMNTTRMS = (string.IsNullOrEmpty(SPGLIND) ? "*" : ""),
                ALLOC_NMBR = ALLOCNMBR,
                SP_GL_IND = SPGLIND,
                ITEM_TEXT = (string.IsNullOrEmpty(SPGLIND) ? "*" : "") + ITEMTEXT,
                REF_KEY_1 = REFKEY,
                ITEMNO_ACC = globalCount,
                VENDOR_NO = CurrentSapParameter.EmployeeID,
                BUS_AREA = CurrentSapParameter.BusArea,
                PYMT_METH = (CurrentSapParameter.EmployeeID == "0099999999" ? "X" : "")
            };

            InsertDataAccountPlayble(accountPay);
        }
    }
}


