using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 创建SAP数据交互对象的工厂
    /// </summary>
    public class SapExchangeFactory
    {
        /// <summary>
        /// 返回 TravelClaim 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetTravelClaim()
        {
            return new SapTravelClaim();
        }

        /// <summary>
        /// 返回 EmployeeClaim 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetEmployeeClaim()
        {
            return new SapEmployeeClaim();
        }

        /// <summary>
        /// 返回 CashAdvance 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetCashAdvance()
        {
            return new SapCashAdvance();
        }

        /// <summary>
        /// 返回 EmployeeCreditCard 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetEmployeeCCClaim()
        {
            return new SapEmployeeCCClaim();
        }

        /// <summary>
        /// 返回 CreditCard 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetCreditCard()
        {
            return new SapCreditCard();
        }

        /// <summary>
        /// 返回 Purchase Order 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetPurchaseOrder()
        {
            return new SapPurchaseOrder();
        }

        /// <summary>
        /// 返回 Purchase Order Return 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetPurchaseOrderReturn()
        {
            return new SapPurchaseOrderRet();
        }

        /// <summary>
        /// 返回 Stores Receive 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetStoresReceive()
        {
            return new SapStoresReceive();
        }


        /// <summary>
        /// 返回 Purchase Order Mod 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetPurchaseOrderMod()
        {
            return new SapPurchaseOrderMod();
        }

        /// <summary>
        /// 返回 Purchase Order Query 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetPurchaseOrderQuery()
        {
            return new SapPurchaseOrderQuery();
        }

        /// <summary>
        /// 返回 PaymentRequest 对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetPaymentRequest()
        {
            //从图标进入PR  POST SAP 的方法和员工报销一样
            return new SapEmployeeClaim();
        }


        /// <summary>
        /// 得到OSP查询数据对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetOSP()
        {
            return new SapOSP();
        }


        /// <summary>
        /// 得到OSP更新Price对象
        /// </summary>
        /// <returns></returns>
        public static ISapExchange GetOSPMod()
        {
            return new SapOSPMod();
        }

        public static ISapExchange GetPOTypeChangeQuery()
        {
            return new SapPOTypeChangeQuery();
        }

        public static ISapExchange GetPOTypeChange()
        {
            return new SapPOTypeChange();
        }
    }
}
