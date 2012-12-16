using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    internal class POTypeChangeArp
    {
        private string number;
        /// <summary>
        /// Style编号
        /// </summary>
        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        private string newType;

        public string NewType
        {
            get { return newType; }
            set { newType = value; }
        }
    }
}
