namespace CA.WorkFlow.UI.TravelRequest2
{
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;

    public class TravelRequest2Control : BaseWorkflowUserControl
    {
        /*
         * Return travel policy item according to "TO" value
         */
        protected SPListItem GetTravelPolicyByCity(string to)
        {
            var qToEN = new QueryField("Title", false);
            var qToCN = new QueryField("City", false);            

            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkOr(exp, qToEN.Equal(to));
            exp = WorkFlowUtil.LinkOr(exp, qToCN.Equal(to));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("Travel Policy"))
                .Where(exp)
                .GetItems();

            return lc.Count > 0 ? lc[0] : null;
        }

        //Return the list item according by area column
        protected SPListItem GetTravelPolicyByArea(string area)
        {
            var qLocation = new QueryField("Location", false);

            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qLocation.Equal(area));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("Travel Policy"))
                .Where(exp)
                .GetItems();

            return lc.Count > 0 ? lc[0] : null;
        }

        protected SPListItem ConvertToRMB(string from)
        {
            return GetExchangeRate(from, "RMB");
        }

        //Return the exchange rate item
        protected SPListItem GetExchangeRate(string from, string to)
        {
            var qFrom = new QueryField("From", false);
            var qTo = new QueryField("To", false);

            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qFrom.Equal(from));
            exp = WorkFlowUtil.LinkAnd(exp, qTo.Equal(to));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("ExchangeRates"))
                .Where(exp)
                .GetItems();

            return lc.Count > 0 ? lc[0] : null;
        }

        /*
         * Return the flight price according to from value and to value
         */
        protected string GetFlightPrice(string from, string to)
        {
            SPListItem item = GetFlightPriceItem(from.ToLower(), to.ToLower());
            return item != null ? item["Price"].AsString() : "0";
        }

        /*
         * Return list item according to from value and to value
         */
        protected SPListItem GetFlightPriceItem(string from, string to)
        {
            var qFromCN = new QueryField("FromCN", false);
            var qFromEN = new QueryField("FromEN", false);
            var qToEN = new QueryField("ToEN", false);
            var qToCN = new QueryField("ToCN", false);

            //From->To
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkOr(exp, qFromEN.Equal(from));
            exp = WorkFlowUtil.LinkOr(exp, qFromCN.Equal(from));
            CamlExpression exp1 = null;
            exp1 = WorkFlowUtil.LinkOr(exp1, qToEN.Equal(to));
            exp1 = WorkFlowUtil.LinkOr(exp1, qToCN.Equal(to));
            CamlExpression exp2 = WorkFlowUtil.LinkAnd(exp, exp1);

            //To->From
            exp = WorkFlowUtil.LinkOr(exp, qFromEN.Equal(to));
            exp = WorkFlowUtil.LinkOr(exp, qFromCN.Equal(to));
            exp1 = WorkFlowUtil.LinkOr(exp1, qToEN.Equal(from));
            exp1 = WorkFlowUtil.LinkOr(exp1, qToCN.Equal(from));
            CamlExpression exp3 = WorkFlowUtil.LinkAnd(exp, exp1);

            //From<->To
            CamlExpression exp4 = WorkFlowUtil.LinkOr(exp2, exp3);
            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("Flight Price"))
                .Where(exp4)
                .GetItems();

            return lc.Count > 0 ? lc[0] : null;
        }

        /*
         * Return data table according to given REQUESTID and LISTNAME
         */
        protected DataTable GetDataTable(string requestId, string listName)
        {
            return GetDataCollection(requestId, listName).GetDataTable();
        }

        /*
         * Return data collection according to given REQUESTID and LISTNAME
         */
        protected SPListItemCollection GetDataCollection(string requestId, string listName)
        {
            var qRequestId = new QueryField("Title", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qRequestId.Equal(requestId));
            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList(listName))
                .Where(exp)
                .GetItems();
            return lc;
        }
    }
}