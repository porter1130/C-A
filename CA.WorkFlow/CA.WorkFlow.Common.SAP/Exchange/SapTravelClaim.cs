using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;
using SAP.Middleware.Connector;
using System.Threading;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 出差报销
    /// </summary>
    internal class SapTravelClaim : SapExchangeClaim
    {
        public SapTravelClaim()
        {
            CurrentDestinationStr = SapDestinationNames.NCO_TRAVELCLAIM;
        }

        /// <summary>
        /// 返回所有需要插入到SAP中的CashAdvance项
        /// </summary>
        protected override List<object[]> GetCashAdvance()
        {
            List<object[]> cashList = new List<object[]>();
            cashList.Add(new object[] { CurrentSapParameter.RefDocNo1, CurrentSapParameter.CashAmount });
            return cashList;
        }

        /// <summary>
        /// 返回CashAmount值
        /// </summary>
        /// <returns></returns>
        protected override decimal GetCashAmount()
        {
            return CurrentSapParameter.CashAmount;
        }

        /// <summary>
        /// 返回PaidByCC值
        /// </summary>
        /// <returns></returns>
        protected override decimal GetPaidByCCAmount()
        {
            decimal cc = 0;
            foreach (var expen in CurrentSapParameter.ExpenceDetails)
            {
                //当信用卡被拒绝时，信用卡报销的金额按公司标准来算
                if (expen.IsPaidByCC == true){
                    cc += (expen.IsNeedApproved == true && expen.IsPaidByCC == true && expen.IsApproved == false ? expen.CompanyStd : expen.Amount * expen.ExchRate);
                }
            }
            
            return cc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<ExpenceDetail> GetExpenceDetails()
        {
            //var expList = CurrentSapParameter.ExpenceDetails.GroupBy(x => new { x.AccountGL, x.ItemText, x.CostCenter }).Select(
            //    g => new { g.Key, Amount = g.Sum(e => (e.IsPaidByCC && e.IsApproved == false ? e.CompanyStd : e.Amount * e.ExchRate)) });

            //var expenDetdils = new List<ExpenceDetail>();
            //foreach (var expen in expList)
            //{
            //    expenDetdils.Add(new ExpenceDetail()
            //    {
            //        AccountGL = expen.Key.AccountGL,
            //        ItemText = expen.Key.ItemText,
            //        CostCenter = expen.Key.CostCenter,
            //        Amount = expen.Amount
            //    });
            //}

            //在出差报销中，会计确认报销时可以调整各个费用类型的金额，不过总金额不变
            return CurrentSapParameter.GroupExpenceDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<object[]> GetPaidByCC()
        {
            List<object[]> ccList = new List<object[]>();
            foreach (var expen in CurrentSapParameter.ExpenceDetails)
            {
                //如果当前消费记录是信用卡支付
                if (expen.IsPaidByCC == true)
                {
                    string allocNmbr = expen.Amount.ToString() + @"/" + expen.Currency;
                    ccList.Add(new object[] { "", allocNmbr, -expen.Amount * expen.ExchRate });
                    //如果当前消费记录被拒绝，则再插入一行差额到SAP
                    if (expen.IsNeedApproved == true && expen.IsApproved == false)
                    {
                        //实际消费减去公司标准
                        ccList.Add(new object[] { "", allocNmbr, expen.Amount * expen.ExchRate - expen.CompanyStd });
                    }  
                }
            }

            return ccList;
        }

        /// <summary>
        /// 计算总金额
        /// </summary>
        /// <returns></returns>
        protected override decimal GetTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var expen in CurrentSapParameter.ExpenceDetails)
            {
                if (expen.IsNeedApproved == true && expen.IsApproved == false){
                    totalAmount += expen.CompanyStd;
                }
                else{
                    totalAmount += expen.Amount * expen.ExchRate;
                }
            }

            return totalAmount;
        }
    }
}
