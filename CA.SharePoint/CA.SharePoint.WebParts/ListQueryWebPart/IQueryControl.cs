using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

using Microsoft.SharePoint;
using CA.SharePoint.CamlQuery;

namespace CA.SharePoint
{

    /// <summary>
    /// ��ѯ�ֶνӿ�
    /// </summary>
    interface IQueryControl
    {
        /// <summary>
        /// �ֶ���
        /// </summary>
        String FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// �ֶβ����Ĳ�ѯ����������Ϊnull
        /// </summary>
        CAMLExpression<object> QueryExpression
        {
            get;
        }

        IPropertyPersistenceService PropertyPersistenceService
        {
            set;
        }
    }
    /// <summary>
    /// �ؼ����Գ־û�����---���ؼ�������ĳ����Χ�ڱ���
    /// </summary>
    interface IPropertyPersistenceService
    {
        string GetPropertyValue( Control ctl , string name );

        void SetPropertyValue(Control ctl, string name, string value);
    }
     
}
