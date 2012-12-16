using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using System.Diagnostics;

namespace CA.SharePoint
{
    /// <summary>
    /// 增加跳转到allitems
    /// </summary>
    public class CATravelLinkToWFAllItems : BaseSPWebPart
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
                        catch(Microsoft.SharePoint.SPException ex)
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
                    string text = "Switch to All Items";

                    hyperLink.Text = text;

                    SPView view = null;
                    try
                    {
                        view = SPContext.Current.List.Views[_viewName];
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

                    hyperLink.NavigateUrl = SPContext.Current.Web.Url + "/" + view.Url;

                    hyperLink.Visible = true;
                    this.Controls.Add(hyperLink);
                }
            }
            base.CreateChildControls();

        }

        private string _viewName = string.Empty;

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

        private string _groups = string.Empty;

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