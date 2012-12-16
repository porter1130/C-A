using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 连接ASP
    /// </summary>
    internal class SapConnector
    {   
        private static object mObj = new object();
        private static volatile RfcDestination mDestination = null;      

        /// <summary>
        /// 连接SAP的通用静态对象
        /// </summary>
        public static RfcDestination Destination         
        {
            get
            {
                lock (mObj)
                {
                    if (mDestination == null || (mDestination != null && mDestination.IsShutDown)){
                        mDestination = RfcDestinationManager.GetDestination(SapDestinationNames.NCO_TESTS_WITHOUT_POOL);
                    }
                }
                return mDestination;
            }
        }
    }
}
