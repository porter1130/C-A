using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 客户端和SAP交互的统一接口，公开方法：ImportDataToSap()、ExportDataToCa()
    /// </summary>
    public interface ISapExchange
    {
        /// <summary>
        /// 批量插入多个数据记录
        /// </summary>
        /// <param name="sapParameters">SapParameter 数组对象</param>
        /// <returns>返回插入到SAP失败的参数对象和相关错误信息
        /// object[] 数组包括三个对象，第一个：SapParameter，第二个：SapResult 或 String，第三个：true or false, 在使用时需进行强制转换
        /// SapResult：当插入到SAP的数据存在错误时，返回从SAP获取到的详细错误信息
        /// String：当出现连接SAP失败的情况时，返回字符串错误</returns>
        List<object[]> ImportDataToSap(List<SapParameter> sapParameters);

        /// <summary>
        /// 从SAP导出数据到CA环境
        /// </summary>
        /// <param name="sapParameter">传入到SAP的参数变量</param>
        /// <returns></returns>
        List<object[]> ExportDataToCa(List<SapParameter> sapParameters);
    }
}
