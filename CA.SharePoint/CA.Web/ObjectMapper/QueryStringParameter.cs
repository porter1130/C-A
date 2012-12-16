using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.Web
{
    /// <summary>
    /// ��ѯ�ַ������ݲ���
    /// </summary>
    public class QueryStringParameter : Parameter
    {
        public override object Evaluate(HttpContext context, Control control)
        {
            if (string.IsNullOrEmpty(QueryStringField))
                throw new ObjectMapException("QueryStringFieldΪ��", this);

            if ((context != null) && (context.Request != null))
            {
                return context.Request.QueryString[this.QueryStringField];
            }
            return null;

        }

        private string _QueryStringField;

        //[WebCategory("Parameter"), WebSysDescription("QueryStringParameter_QueryStringField"), DefaultValue("")]
        
        /// <summary>
        /// ��ѯ�ַ�����
        /// </summary>
        public string QueryStringField
        {
            get
            {
                //object obj1 = base.ViewState["QueryStringField"];
                //if (obj1 == null)
                //{
                //    return string.Empty;
                //}
                return _QueryStringField;
            }
            set
            {
                _QueryStringField = value;

                //if (this.QueryStringField != value)
                //{
                //    base.ViewState["QueryStringField"] = value;
                //    base.OnParameterChanged();
                //}
            }
        }
 

    }
}
