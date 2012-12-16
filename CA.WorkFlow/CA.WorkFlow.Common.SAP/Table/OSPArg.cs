using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
   internal class OSPArg
    {
        private string styleNO;
        /// <summary>
        /// Style编号
        /// </summary>
        public string StyleNO
        {
            get { return styleNO; }
            set { styleNO = value; }
        }

        private string price;
        /// <summary>
        /// 价格
        /// </summary>
        public string Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
