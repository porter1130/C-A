using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using System.Diagnostics;
using System.Web;

namespace CA.SharePoint
{
    /// <summary>
    /// 增加跳转到allitems
    /// </summary>
    public class CAGeneralLinks : BaseSPWebPart
    {
        private HyperLink hyperLink = null;

        protected override void CreateChildControls()
        {
            if (base.ChildControlsCreated) return;

            if (!string.IsNullOrEmpty(_groups))
            {
                var isInGroup = false;

                foreach (var gstr in _groups.Split(';'))
                {
                    if (!string.IsNullOrEmpty(gstr.Trim()))
                    {
                        try
                        {
                            foreach (SPUser user in SPContext.Current.Web.Groups[gstr.Trim()].Users)
                            {
                                if (user.LoginName.Equals(SPContext.Current.Web.CurrentUser.LoginName, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    isInGroup = true;
                                    break;
                                }
                            }

                        }
                        catch (Microsoft.SharePoint.SPException ex)
                        {
                            SPSecurity.RunWithElevatedPrivileges(delegate()
                            {
                                if (!EventLog.SourceExists("C&A"))
                                {
                                    EventLog.CreateEventSource("C&A", "Mail");
                                }
                                EventLog myLog = new EventLog();
                                myLog.Source = "C&A";
                                myLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                            });
                        }
                    }
                }

                if (isInGroup)
                {
                    hyperLink = new HyperLink();
                    hyperLink.CssClass = "CA_additem";

                    SPList list = SPContext.Current.List;
                    hyperLink.Text = string.IsNullOrEmpty(_linkDescription) ? "Switch to All Items" : _linkDescription;

                    SPView view = null;
                    try
                    {
                        view = SPContext.Current.List.Views[string.IsNullOrEmpty(_viewName) ? "All Items" : _viewName];
                    }
                    catch (ArgumentException ex)
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            if (!EventLog.SourceExists("C&A"))
                            {
                                EventLog.CreateEventSource("C&A", "Mail");
                            }
                            EventLog myLog = new EventLog();
                            myLog.Source = "C&A";
                            myLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                        });
                        view = SPContext.Current.List.Views["All Items"];
                    }
                    if (string.IsNullOrEmpty(_urlLink))
                    {
                        hyperLink.NavigateUrl = SPContext.Current.Web.Url + "/" + view.Url
                            + (string.IsNullOrEmpty(_columnName) ? string.Empty : "?FilterField1=" + _columnName + "&FilterValue1=" + HttpUtility.UrlDecode(SPContext.Current.Web.CurrentUser.Name));
                    }
                    else
                    {
                        hyperLink.NavigateUrl = SPContext.Current.Site.RootWeb.Url + "/WorkFlowCenter/" + _urlLink;
                    }
                    

                    hyperLink.Visible = true;
                    this.Controls.Add(hyperLink);
                }
            }
            else
            {
                hyperLink = new HyperLink();
                hyperLink.CssClass = "CA_additem";

                SPList list = SPContext.Current.List;
                hyperLink.Text = string.IsNullOrEmpty(_linkDescription) ? "Switch to All Items" : _linkDescription;

                SPView view = null;
                
                try
                {
                    view = SPContext.Current.List.Views[string.IsNullOrEmpty(_viewName)? "All Items" : _viewName];
                }
                catch (ArgumentException ex)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        if (!EventLog.SourceExists("C&A"))
                        {
                            EventLog.CreateEventSource("C&A", "Mail");
                        }
                        EventLog myLog = new EventLog();
                        myLog.Source = "C&A";
                        myLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                    });
                    view = SPContext.Current.List.Views["All Items"];
                }

                if (string.IsNullOrEmpty(_urlLink))
                {
                    hyperLink.NavigateUrl = SPContext.Current.Web.Url + "/" + view.Url
                        + (string.IsNullOrEmpty(_columnName) ? string.Empty : "?FilterField1=" + _columnName + "&FilterValue1=" + HttpUtility.UrlDecode(SPContext.Current.Web.CurrentUser.Name));
                }
                else
                {
                    hyperLink.NavigateUrl = SPContext.Current.Site.RootWeb.Url + "/WorkFlowCenter/" + _urlLink;
                }

                hyperLink.Visible = true;
                this.Controls.Add(hyperLink);
            }

            base.CreateChildControls();

        }

        private string _viewName = string.Empty;
        private string _columnName = string.Empty;
        private string _groups = string.Empty;
        private string _linkDescription = string.Empty;
        private string _urlLink = string.Empty;

        

        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("请输入链接显示文字")]
        [WebDescription("显示")]
        [Category("个性设置")]
        public string LinkDescription
        {
            get
            {
                return _linkDescription;
            }
            set
            {
                _linkDescription = value;
            }
        }

        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("请输入视图名")]
        [WebDescription("视图名")]
        [Category("个性设置")]
        public string ViewName
        {
            get 
            {
                return _viewName;
            }
            set
            {
                _viewName = value;
            }
        }

        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("请输入URL链接")]
        [WebDescription("URL")]
        [Category("个性设置")]
        public string URLLink
        {
            get
            {
                return _urlLink;
            }
            set
            {
                _urlLink = value;
            }
        }

        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("请输入过滤字段名")]
        [WebDescription("字段")]
        [Category("个性设置")]
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
            }
        }
        
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("请输入用户组,多个用分号分隔")]
        [WebDescription("用户组")]
        [Category("个性设置")]
        public string Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
            }
        }
        
    }
}