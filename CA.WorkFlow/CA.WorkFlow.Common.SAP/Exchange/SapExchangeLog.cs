using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 日志操作对象，方法均为静态
    /// </summary>
    public class SapExchangeLog
    {
        /// <summary>
        /// 输出结果到控制台
        /// </summary>
        public static void ShowResult(SapResult sapResult)
        {
            Console.WriteLine("Function finished:");
            foreach (SAP.Middleware.Table.RETURN ret in sapResult.RETURN_LIST)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine(
                    "TYPE: " + ret.TYPE + '\n' +
                    "ID: " + ret.ID + '\n' +
                    "Number: " + ret.NUMBER + '\n' +
                    "LOG_NO: " + ret.LOG_NO + '\n' +
                    "LOG_MSG_NO: " + ret.LOG_MSG_NO + '\n' +
                    "MESSAGE_V1: " + ret.MESSAGE_V1 + '\n' +
                    "MESSAGE_V2: " + ret.MESSAGE_V2 + '\n' +
                    "MESSAGE_V3: " + ret.MESSAGE_V3 + '\n' +
                    "MESSAGE_V4: " + ret.MESSAGE_V4 + '\n' +
                    "PARAMETER: " + ret.PARAMETER + '\n' +
                    "ROW: " + ret.ROW + '\n' +
                    "FIELD: " + ret.FIELD + '\n' +
                    "SYSTEM: " + ret.SYSTEM + '\n' +
                    "MESSAGE: " + ret.MESSAGE
                );
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("OBJ_TYPE: " + sapResult.OBJ_TYPE);
            Console.WriteLine("OBJ_KEY: " + sapResult.OBJ_KEY);
            Console.WriteLine("OBJ_SYS：" + sapResult.OBJ_SYS);

            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
        }
    }
}
