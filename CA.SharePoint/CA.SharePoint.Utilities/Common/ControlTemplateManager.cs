//----------------------------------------------------------------
//Microsoft Consulting Service (2008)
//
//�ļ�����:
//
//�� �� ��: v-jianyz@microsoft.com
//��������: 2008-09-19
//
//�޶���¼: 
//
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;


namespace CA.SharePoint
{
    /// <summary>
    /// �ؼ�ģ�����
    /// </summary>
    public class ControlTemplateManager
    {
        public const string RESOURCES_PATH = "~/_Layouts/CA/WebControls/";

        private Page _page;
        private ControlTemplateManager( Page page )
        {
            _page = page;
        }

        public static ControlTemplateManager GetIntance(Page page)
        {
            ControlTemplateManager m = new ControlTemplateManager(page);
            return m;
        }

        public T GetTemplateControl<T>(string controlName, string templateId) where T : Control
        {
            Control ctl = LoadControl(controlName);

            Control temp = ctl.FindControl(templateId);

            if (temp == null)
                throw new ControlTemplateException(  "�ؼ�ģ����ش��󣺿ؼ�" + controlName );

            return (T)temp;
        }

        public Control GetTemplateControl(string controlName, string templateId) 
        {
            Control ctl = LoadControl(controlName);

            Control temp = ctl.FindControl(templateId);

            if (temp == null)
                throw new ControlTemplateException("�ؼ�ģ����ش��󣺿ؼ�" + controlName);

            return temp;
        }

        public Control GetTemplateControl(string controlName)
        {
            Control ctl = _page.LoadControl(RESOURCES_PATH + controlName);

            return ctl;
        }

        private Control LoadControl(string controlName)
        {
            if( controlName.StartsWith("~") )
                return _page.LoadControl(controlName);
            else
                return _page.LoadControl(RESOURCES_PATH + controlName); ;
        }
    }

    public class ControlTemplateException : Exception
    {
        public ControlTemplateException(string message ) : base( message )
        {
           
        }
    }

}
