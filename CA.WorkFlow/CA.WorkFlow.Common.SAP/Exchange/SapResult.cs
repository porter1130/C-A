using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// SAP结果对象
    /// </summary>
    public class SapResult
    {
        public string OBJ_TYPE { get; set; }
        public string OBJ_KEY { get; set; }
        public string OBJ_SYS { get; set; }
        public POINFO OBJ_POINFO { get; set; }
        public List<RETURN> RETURN_LIST { get; set; }
        public OSPInfo OBJ_OSPINFO { get; set; }
        public POTypeChangeInfo OBJ_POTypeChangeInfo { get; set; }
    }
}
