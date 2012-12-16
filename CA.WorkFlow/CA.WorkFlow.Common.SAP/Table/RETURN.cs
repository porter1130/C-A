using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Middleware.Table
{
    public class RETURN
    {
        /// <summary>
        /// Message type: S Success, E Error, W Warning, I Info, A Abort
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// Message Class
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Message Number
        /// </summary>
        public string NUMBER { get; set; }

        /// <summary>
        /// Message Text
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// Application log: log number
        /// </summary>
        public string LOG_NO { get; set; }

        /// <summary>
        /// Application log: Internal message serial number
        /// </summary>
        public string LOG_MSG_NO { get; set; }

        /// <summary>
        /// Message Variable
        /// </summary>
        public string MESSAGE_V1 { get; set; }

        /// <summary>
        /// Message Variable
        /// </summary>
        public string MESSAGE_V2 { get; set; }

        /// <summary>
        /// Message Variable
        /// </summary>
        public string MESSAGE_V3 { get; set; }

        /// <summary>
        /// Message Variable
        /// </summary>
        public string MESSAGE_V4 { get; set; }

        /// <summary>
        /// Parameter Name
        /// </summary>
        public string PARAMETER { get; set; }

        /// <summary>
        /// Lines in parameter
        /// </summary>
        public string ROW { get; set; }

        /// <summary>
        /// Field in parameter
        /// </summary>
        public string FIELD { get; set; }

        /// <summary>
        /// Logical system from which message originates
        /// </summary>
        public string SYSTEM { get; set; }
    }
}
