using Microsoft.SharePoint;
using System;
using CodeArt.SharePoint.CamlQuery;
using System.Data;

namespace CA.WorkFlow.UI
{
    public static class WorkflowListData
    {
        private static DataTable itemCodes = null;
        private static double lastTag = -1;

        private static void init()
        {
            double currTag = DateTime.Now.DayOfYear + DateTime.Now.Hour / 100.0;//365.23
            if (currTag == lastTag)
            {
                return;
            }

            itemCodes = GetActiveItemCode();

            lastTag = currTag;
            CommonUtil.logInfo(DateTime.Now.ToString() + "Init the workflow list successfully.");
        }

        /**
         * 获取有效的Item Code
         */
        private static DataTable GetActiveItemCode()
        {
            var qIsActive = new QueryField("IsActive", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qIsActive.Equal(true));

            SPListItemCollection lc = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                                .From(web.Lists["Item Codes"])
                                .Where(exp)
                                .GetItems();
                    }
                }
            });

            return lc.GetDataTable();
        }

        public static DataTable GetItemCodeDT()
        {
            init();
            return itemCodes;
        }
        
    }
}