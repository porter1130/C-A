using System.Data;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using System.Text;
using QuickFlow;
using CodeArt.SharePoint.CamlQuery;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using QuickFlow.Core;
using CA.SharePoint;
using System.Web;
namespace CA.WorkFlow.UI.NTSC
{
    public class NewTradeSupplierCreation
    {
        public static SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            });
            return user;
        }
    }

    public class NewTradeSupplierCreationConstants
    {
        public static string wf_NTSC_QM = "wf_NTSC_QM";
        public static string wf_NTSC_QAD = "wf_NTSC_QAD";
        public static string wf_NTSC_SCM = "wf_NTSC_SCM";
        public static string wf_NTSC_SCMM = "wf_NTSC_SCMM";
        
    }

}