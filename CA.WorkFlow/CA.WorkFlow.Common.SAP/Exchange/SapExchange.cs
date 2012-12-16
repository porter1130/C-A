using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using SAP.Middleware.Table;
using System.Threading;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 导入数据到SAP的基类，定义和SAP交互的方法集合
    /// 此类主要用到 模板方法 设计模式
    /// </summary>
    internal abstract class SapExchange : SapFunctions, ISapExchange
    {
        /// <summary>
        /// SAP参数对象，全局模式
        /// </summary>
        private SapParameter mCurrentSapParameter = null;
        protected SapParameter CurrentSapParameter
        {
            set { mCurrentSapParameter = value; }
            get { return mCurrentSapParameter; }
        }
       
        /// <summary>
        /// 记录操作失败的数据
        /// </summary>
        private Queue<object[]> mErrorSapParameters = new Queue<object[]>();
        /// <summary>
        /// 记录操作成功的结果数据
        /// </summary>
        private List<object[]> mSapResult = new List<object[]>();

        /// <summary>
        /// 完成数据插入，需要调用 Invoke() 方法，相当于告知服务器完成一次数据插入
        /// </summary>
        protected virtual void Completed()
        {
            if ((bool)CloseDestination()[0] == false)
            {
                //当关闭SAP连接失败时，应该把当前数据对象加入到mErrorSapParameters集合中
                mErrorSapParameters.Enqueue(new object[] { mCurrentSapParameter, (string)CloseDestination()[1], false });
                return;
            }

            SapResult sapResult = new SapResult();
            //当 OBJ_KEY == "$" 表示此次数据插入失败
            if (Completed(ref sapResult) == true)
            {
                //当数据插入成功时，当前数据对象加入到mCurrentResult集合中
                mSapResult.Add(new object[] { mCurrentSapParameter, sapResult, true });
            }
            else{
                //当数据插入失败时，应该把当前数据对象加入到mErrorSapParameters集合中
                mErrorSapParameters.Enqueue(new object[] { mCurrentSapParameter, sapResult, false });
            }
        }

        /// <summary>
        /// 批量导入数据到SAP
        /// </summary>
        /// <param name="sapParameters">SAP参数对象数组</param>
        /// <returns>数据插入到SAP是否成功，返回bool型的数组，数组值的顺序和传入的参数对象顺序一致</returns>
        public virtual List<object[]> OperationForSap(List<SapParameter> sapParameters)
        {
            //第一次循环传入的SAP数组对象
            foreach (SapParameter sp in sapParameters){
                OperationForSap(sp);
            }

            //再重新循环 2 次去导入第一次循环中出现错误的数据
            for (int i = 0; i < 2; i++)
            {
                for (int j = mErrorSapParameters.Count; j > 0; j--)
                {
                    if (mErrorSapParameters.Count > 0){
                        OperationForSap(mErrorSapParameters.Dequeue()[0] as SapParameter);
                    }
                }
            }

            foreach (object[] obj in mErrorSapParameters.ToList<object[]>()){
                mSapResult.Add(obj);
            }

            return mSapResult;
        }

        /// <summary>
        /// 导入数据到SAP
        /// </summary>
        /// <param name="sapParameters">SAP参数对象</param>
        /// <returns>数据插入到SAP是否成功</returns>
        private void OperationForSap(SapParameter sapParameter)
        {
            if (sapParameter == null){
                mErrorSapParameters.Enqueue(new object[] { mCurrentSapParameter, "Current object is null", false });
                return;
            }
            object[] obj = InitializeEnvironment();
            mCurrentSapParameter = sapParameter;
            if ((bool)obj[0] == false)
            {
                mErrorSapParameters.Enqueue(new object[] { mCurrentSapParameter, (string)obj[1], false });
                return;
            }

            OperationForSap();
        }

        /// <summary>
        /// 子类实现
        /// </summary>
        /// <returns></returns>
        protected abstract void OperationForSap();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected abstract bool Completed(ref SapResult sapResult);

        /// <summary>
        /// 从SAP导出数据到CA环境
        /// </summary>
        /// <param name="sapParameter">传入到SAP的参数变量</param>
        /// <returns></returns>
        public virtual List<object[]> ExportDataToCa(List<SapParameter> sapParameters) {
            return OperationForSap(sapParameters);
        }

        /// <summary>
        /// 批量导入数据到SAP
        /// </summary>
        /// <param name="sapParameters">SAP参数对象数组</param>
        /// <returns>数据插入到SAP是否成功，返回bool型的数组，数组值的顺序和传入的参数对象顺序一致</returns>
        public virtual List<object[]> ImportDataToSap(List<SapParameter> sapParameters){
            return OperationForSap(sapParameters);
        }
    }
}
