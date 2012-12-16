using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CA.WorkFlow.Common.UnlockWorkflow.Utility
{
    public class JavascriptHelper
    {
        private const string ScriptFormat = "<script type='text/javascript' language='javascript'>{0}</script>";

        public static string Custom(string script, string key)
        {
            return Custom(null, script, key);
        }

        public static string Custom(Page page, string script, string key)
        {
            string js = string.Format(ScriptFormat, script);

            Register(page, js, key);

            return js;
        }

        public static string Alert(string message, string key)
        {
            return Alert(null, message, key);
        }

        public static string Alert(Page page, string message, string key)
        {
            return Custom(page, string.Format("alert('{0}');", message), key);
        }

        private static void Register(Page page, string javascript, string key)
        {
            if (page != null)
            {
                if (!page.ClientScript.IsClientScriptBlockRegistered(key))
                {
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, javascript);
                }
            }
        }
    }
}