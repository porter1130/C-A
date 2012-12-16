using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    class SapPOTypeChangeQuery: SapExchange
    {
        public SapPOTypeChangeQuery()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_POTYPE_GET;//POTYPECHANGE的查询方法。
            CurrentDestinationStr = SapDestinationNames.NCO_POTYPE;//POTYPECHANGE的连接字符串。 
        }

        /// <summary>
        /// 设置得到由Style No.得到OSP信息方法的参数
        /// </summary>
        protected void SetPOTypeChangeArg()
        {
            SetPOTypeChangeArg(new POTypeChangeArp() { Number = CurrentSapParameter.SapNumber });
        }

        protected override void OperationForSap()
        {
            SetPOTypeChangeArg();
            Completed();
        }

        /// <summary>
        /// 查询完成一次后，给OBJ_OSPINFO赋值
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {
            sapResult.OBJ_POTypeChangeInfo = GetPOTypeChangeInfo();
            sapResult.OBJ_KEY = CurrentSapParameter.SapNumber;
            /*sapResult.OBJ_SYS = CurrentSapParameter.PaymentCond;*/
            return true;
        }
    }
}
