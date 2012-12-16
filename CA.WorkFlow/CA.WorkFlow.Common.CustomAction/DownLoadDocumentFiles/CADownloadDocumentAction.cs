using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace CA.WorkFlow.Common.CustomAction.DownLoadDocumentFiles
{
    public class CADownloadDocumentAction : WebControl
    {
        public const string RESOURCES_PATHCSS = "~/_layouts/CAResources/themeCA/css/";
        public const string RESOURCES_PATHJS = "~/_layouts/CAResources/themeCA/js/";

        public CADownloadDocumentAction()
        {
            this.ID = "CADownloadDocumentAction";
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterCommonJs("downloaddocument.js");
            RegisterCommonCss("downloaddocument.css");
        }

        protected override void CreateChildControls()
        {
            LinkButton lb = new LinkButton();
            lb.Text = "DownLoad";
            lb.OnClientClick = "CheckFiles();return false;";
            lb.CssClass = "ms-menubuttoninactivehover";
            this.Controls.Add(lb);
        }
        #region 注册JS，CSS

        protected void RegisterCommonJs(string jsFileName)
        {
            string jsPath =  base.ResolveUrl(RESOURCES_PATHJS) + jsFileName;
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), jsFileName.ToLower(), jsPath);
        }
        protected void RegisterCommonCss(string fileName)
        {
            string id = fileName.ToLower().Replace(".", "_");
            foreach (System.Web.UI.Control ctl in this.Page.Header.Controls)
            {
                if (ctl.ID == id)
                    return;
            }
            HtmlLink link1 = new HtmlLink();
            link1.ID = id;
            link1.Attributes["type"] = "text/css";
            link1.Attributes["rel"] = "stylesheet";
            link1.Attributes["href"] =  base.ResolveUrl(RESOURCES_PATHCSS) + fileName;
            this.Page.Header.Controls.Add(link1);
        }
        #endregion


    }
}
