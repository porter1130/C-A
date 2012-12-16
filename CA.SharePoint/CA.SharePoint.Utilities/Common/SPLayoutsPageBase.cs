
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using CA.Web;
using System.Collections;
using System.Web.UI.WebControls;
using System.Security;
using Microsoft.SharePoint;
using System.Web;
using System.Web.UI.HtmlControls;
using System.IO;

namespace CA.SharePoint
{
    /// <summary>
    /// sharepoint ҳ����࣬�Զ�Ӧ��վ���ģ��ҳ 
    /// </summary>
    public class SPLayoutsPageBase : Microsoft.SharePoint.WebControls.LayoutsPageBase, IPagePathProvider
    {
        private Script _script;
        public Script Script
        {
            get
            {
                if (_script == null)
                    _script = new Script(this);
                return _script;
            }
        }

        protected virtual string[] Roles
        {
            get
            {
                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                //if (false == CanAccess())
                //{
                //    throw new Exception("��û��Ȩ�޷��ʴ�ҳ�档");
                //}
              
            }
           
            //this.Form.Attributes.Add("onsubmit", "return OperateSubmit();");
        }

        //protected virtual bool CanAccess()
        //{
        //    string[] roles = this.Roles;

        //    if (roles != null && roles.Length > 0)
        //    {
        //        foreach (string r in roles)
        //        {
        //            if (this.SecurityContext.CurrentUser.IsInRole(r))
        //            {
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitSPContext();
        }

        public virtual string SiteUrl
        {
            get { return "http://127.17.1.45:83/"; }         
        }

        public void InitSPContext()
        {
            if (SPContext.Current != null)
                return;

            SPSite site = new SPSite(this.SiteUrl);
            SPWeb web = site.RootWeb;

            HttpContext.Current.Items["HttpHandlerSPWeb"] = web;

            SPContext context = SPContext.GetContext(web);
            HttpContext.Current.Items["DefaultSPContext"] = context;
        }

        protected override void OnPreInit(EventArgs e)
        {
            CodeArt.SharePoint.MultiLanSupport.UICultureManager.CurrentInstance.SetThreadCulture();
            base.OnPreInit(e);

            ApplySiteMaster();
        }

        protected virtual void ApplySiteMaster()
        {
            if (SPContext.Current != null)
                this.MasterPageFile = SPContext.Current.Web.MasterUrl;
        }

        Dictionary<String, String> _PagePath = new Dictionary<String, String>();
        /// <summary>
        /// ҳ��ĵ���·��
        /// </summary>
        public virtual Dictionary<String, String> PagePath
        {
            get
            {
                return _PagePath;
            }
        }

        /// <summary>
        /// ��ȡ��ѯ�ַ���ֵ���������ڣ��򷵻�Ĭ��ֵ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected virtual string GetFromQueryString(string name, string defaultValue)
        {
            if (Request.QueryString[name] == null) return defaultValue;

            return Request.QueryString[name].Trim();
        }


        /// <summary>
        /// ��ȡ��ѯ�ַ���ֵ���������ڣ����׳��쳣
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetFromQueryString(string name)
        {
            if (Request.QueryString[name] == null) throw new ArgumentNullException("name", "��ѯ�ַ���[ " + name + " ]������");

            return Request.QueryString[name].Trim();
        }

        /// <summary>
        /// ���URL����ĵ���·��
        /// </summary>
        /// <returns></returns>
        protected virtual bool AddSourcePagePath()
        {
            string sourceURL = GetFromQueryString("sourceUrl", "");
            string sourceName = GetFromQueryString("sourceName", "");

            if (sourceName != "")
            {
                this.PagePath.Add(sourceName, sourceURL);
                return true;
            }
            return false;
        }



        /// <summary>
        /// �滻Ĭ�ϵ�ҳ�浼��
        /// </summary>
        protected virtual void ApplySiteMapPath()
        {
            if (this.Master == null)
                return;

            Control ctl = this.Master.FindControl("PlaceHolderTitleBreadcrumb");

            if (ctl != null)
            {
                ctl.Controls.Clear();
                ctl.Controls.Add(new PageNavigation());
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.ApplySiteMapPath();
        }

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            try
            {
                Exception ex = Server.GetLastError();
                
                //LogUtil.WritePageLog(ex, this);
                //LogUtil.WriteLog("ҳ�����" + ex.Message, ex);  
            }
            catch { }

            this.Response.Clear();
            //Server.Transfer("/_layouts/smartform/error.aspx");
        }

    }

    /// <summary>
    /// ҳ����Ҫʵ�ִ˽ӿ��ṩ��������
    /// </summary>
    public interface IPagePathProvider
    {
        Dictionary<String, String> PagePath
        {
            get;
        }
    }

    /// <summary>
    /// ҳ�浼��
    /// </summary>
    public class PageNavigation : Control
    {
        private string _NavIcon = string.Empty;
        public string NavIcon
        {
            get
            {
                return this._NavIcon;
            }
            set
            {
                this._NavIcon = value;
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.Write("<div>");

            if (!string.IsNullOrEmpty(this.NavIcon))
            {
                writer.Write("<img src='" + this.NavIcon + "' style='vertical-align:bottom;'/>&nbsp;");
            }

            if (this.Page is IPagePathProvider)
            {
                IPagePathProvider pathProvider = (IPagePathProvider)this.Page;

                Dictionary<String, String> path = pathProvider.PagePath;

                SPWeb web=SPContext.Current.Web;
                writer.Write("<a href='" + web.Url + "' >" + web.Title + "</a>");
                if (!web.IsRootWeb)
                {
                    writer.Write("<span style='font-size:Smaller;'> &gt; </span> ");
                    writer.Write("<a href='" + web.Site.RootWeb.Url + "' >" + web.Site.RootWeb.Title + "</a>");

                }              
                 writer.Write("<span style='font-size:Smaller;'> &gt; </span> ");
                if (path != null && path.Count > 0)
                {
                    int index = 0;
                    foreach (string key in path.Keys)
                    {
                        index++;

                        if (index > 1)
                        writer.Write("<span style='font-size:Smaller;'> &gt; </span> ");//edit the css by caixiang

                        if (index == path.Count)
                        {
                            writer.Write("<a >" + key + "</a>");
                        }
                        else
                        {
                            writer.Write("<a href='" + path[key] + "'>" + key + "</a>");
                        }
                    }
                }
                else
                {
                    writer.Write("<a>" + this.Page.Title + "</a>");
                }
            }

            writer.Write("</div>");



        }


    }
}
