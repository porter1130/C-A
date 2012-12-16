namespace CA.WorkFlow.UI.PurchaseOrder
{
    using System.Data;
    using QuickFlow;
    using Microsoft.SharePoint;
    using System;

    class PurchaseOrderCommon
    {
        //Return data table from "PurchaseOrderItems" list according by given requestId.
        internal static DataTable GetDataTable(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "PurchaseOrderItems").GetDataTable();
        }

        internal static NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, WorkFlowUtil.GetModuleIdByListName("PurchaseOrderWorkflow"));
        }

        /// <summary>
        /// 当前PO是否是Capex 
        /// </summary>
        /// <param name="sPONumber"></param>
        /// <returns></returns>
        internal static bool IsComPex(string sPONumber)
        {
            bool bResult = false;

            SPQuery query = new SPQuery();
            query.Query = string.Format(
                      @"<Where>
                                      <Eq>
                                         <FieldRef Name='PONumber' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>", sPONumber);

            SPListItemCollection splic = SPContext.Current.Web.Lists["PurchaseRequestItems"].GetItems(query);
            if (null != splic && splic.Count > 0)
            {
                string sCapexType = splic[0]["RequestType"].ToString();
                if (sCapexType.Equals("Capex", StringComparison.InvariantCultureIgnoreCase))
                {
                    bResult = true;
                }
            }
            return bResult;
        }
        internal static bool isAdmin()
        {
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}