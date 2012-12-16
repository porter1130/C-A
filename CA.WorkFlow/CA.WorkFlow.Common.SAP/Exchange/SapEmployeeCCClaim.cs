using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 员工报销信用卡
    /// </summary>
    internal class SapEmployeeCCClaim : SapExchangeClaim
    {
        public SapEmployeeCCClaim()
        {
            CurrentDestinationStr = SapDestinationNames.NCO_EMPLOYEECCCLAIM;
        }

        /// <summary>
        /// 返回所有需要插入到SAP中的CashAdvance项
        /// </summary>
        protected override List<object[]> GetCashAdvance()
        {
            return null;
        }

        /// <summary>
        /// 返回CashAmount值,当员工报销信用卡时，所有项都为信用卡
        /// </summary>
        /// <returns></returns>
        protected override decimal GetCashAmount()
        {
            return 0;
        }

        /// <summary>
        /// 返回PaidByCC值
        /// </summary>
        /// <returns></returns>
        protected override decimal GetPaidByCCAmount()
        {
            decimal totalAmount = 0;
            foreach (var expen in CurrentSapParameter.ExpenceDetails)
            {
                totalAmount += expen.Amount * (expen.ExchRate == 0 ? 1 : expen.ExchRate);
            }

            return totalAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<ExpenceDetail> GetExpenceDetails()
        {
            return CurrentSapParameter.ExpenceDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<object[]> GetPaidByCC()
        {
            List<object[]> cc = new List<object[]>();
            foreach (var expen in CurrentSapParameter.ExpenceDetails)
            {
                cc.Add(new object[] { expen.RefKey, expen.Amount.ToString() + @"/" + expen.Currency, - expen.Amount });
            }
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
                totalAmount += expen.Amount * (expen.ExchRate == 0 ? 1 : expen.ExchRate);
            }
            return totalAmount;
        }
    }
}
