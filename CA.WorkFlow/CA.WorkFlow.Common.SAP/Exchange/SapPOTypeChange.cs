using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SAP.Middleware.Table;
namespace SAP.Middleware.Exchange
{
    class SapPOTypeChange : SapExchange
    {
        public SapPOTypeChange()
        {
            CurrentFunctionStr = SapFunctionNames.Z_EWF_POTYPE_CHANGE;//OSP的查询方法。
            CurrentDestinationStr = SapDestinationNames.NCO_POTYPE;//OSP的连接字符串。 
        }

        /// <summary>
        /// 设置修改OSP的参数
        /// </summary>
        protected void SetPOTypeChangePars()
        {
            SetPOTypeChangePars(new POTypeChangeArp() { Number = CurrentSapParameter.SapNumber, NewType =  CurrentSapParameter.PaymentCond });
        }

        protected override void OperationForSap()
        {
            SetPOTypeChangePars();
            Completed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {
            sapResult = GetSapResult();
            if (SapReturnStatus == "Y")
            {
                //Y 标示更新OSP里的价格成功
                sapResult.OBJ_POTypeChangeInfo = new POTypeChangeInfo() { IsSuccess = true };
                sapResult.OBJ_KEY = CurrentSapParameter.SapNumber;
                return true;
            }
            else
            {
                //Y 标示更新OSP里的价格
                sapResult.OBJ_POTypeChangeInfo = new POTypeChangeInfo() { IsSuccess = false };
                sapResult.OBJ_KEY = CurrentSapParameter.SapNumber;
                return false;
            }
        }
    }
}
