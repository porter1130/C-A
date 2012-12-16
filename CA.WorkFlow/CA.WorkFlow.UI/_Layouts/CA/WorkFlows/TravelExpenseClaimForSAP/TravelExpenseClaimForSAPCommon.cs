namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using System.Text;
    using System.Collections.Generic;
    using QuickFlow;
    using System.Diagnostics;
    using System.Collections;
    using System.Reflection;
    using System.Web.UI.WebControls;
    using System;
    using System.Linq;

    class TravelExpenseClaimForSAPCommon
    {
        /// <summary>
        /// Return DataTable according to DataTable according to workflow number and list name
        /// </summary>
        /// <param name="requestId">workflow number</param>
        /// <param name="listName">list name</param>
        /// <returns>DataTable</returns>
        internal static DataTable GetDataTable(string requestId, string listName)
        {
            return GetDataCollection(requestId, listName).GetDataTable();
        }

        /// <summary>
        /// Return SPListItemCollection according to workflow number and list name
        /// </summary>
        /// <param name="requestId">workflow number</param>
        /// <param name="listName">list name</param>
        /// <returns>SPListItemCollection</returns>
        internal static SPListItemCollection GetDataCollection(string requestId, string listName)
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

        /// <summary>
        /// Return a new datatable according to source datatable and expression
        /// </summary>
        /// <param name="sourceTable">source datatable</param>
        /// <param name="exp">select expression</param>
        /// <returns>the new datatable</returns>
        internal static DataTable GetDataSource(DataTable sourceTable, string exp)
        {
            DataTable targetDataTable = sourceTable.Clone();
            DataRow[] rows = sourceTable.Select(exp);

            foreach (DataRow row in rows)
            {
                targetDataTable.ImportRow(row);
            }

            return targetDataTable;
        }

        /// <summary>
        /// Convert SPList to list object for serlization
        /// </summary>
        /// <param name="list">splist</param>
        /// <param name="obj">object type</param>
        /// <returns>list</returns>
        internal static List<object> GetSerializingList(SPListItemCollection items, object obj)
        {
            List<object> oList = new List<object>();
            Hashtable hashtable;
            foreach (SPListItem item in items)
            {
                hashtable = new Hashtable();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (items.List.Fields.ContainsField(prop.Name))
                    {
                        hashtable.Add(prop.Name, item[prop.Name].AsString());
                    }
                }
                oList.Add(hashtable);
            }
            return oList;
        }

        internal static NameCollection GetTaskUsersWithoutDeleman(string group)
        {
            string moduleID = WorkFlowUtil.GetModuleIdByListName(WorkflowListName.TravelExpenseClaimForSAP);
            return WorkFlowUtil.GetTaskUsersWithoutDeleman(group, moduleID);
        }

        internal static void SaveDetails(DataEdit dataEdit, string workflowNumber)
        {
            List<DataTable> dataTableList = new List<DataTable>() { dataEdit.ExpenseTable, dataEdit.SAPSummaryTable };
            foreach (DataTable dt in dataTableList)
            {
                switch (dt.TableName)
                {
                    case "SAPSummary":
                        Hashtable hash = new Hashtable();
                        hash.Add("SAPSection", "1");

                        WorkFlowUtil.BatchInsertDataTable(WorkflowListName.TravelExpenseClaimDetailsForSAP, workflowNumber, dt, hash);
                        break;
                    default:
                        WorkFlowUtil.BatchInsertDataTable(WorkflowListName.TravelExpenseClaimDetailsForSAP, workflowNumber, dt, null);
                        break;
                }
            }

        }

        internal static DataTable GetExpenseDataSource(DataTable dt)
        {
            decimal tmp;
            var query = from e in dt.AsEnumerable()
                        group e by new { ExpenseType = e["ExpenseType"].ToString(), CostCenter = e["CostCenter"].ToString() } into grp
                        select new
                        {
                            ExpenseType = grp.Key.ExpenseType,
                            CostCenter = grp.Key.CostCenter,
                            ApprovedRmbAmt = grp.Sum(cost => Decimal.TryParse(cost["ApprovedRmbAmt"].AsString(), out tmp) ? tmp : 0)
                        };

            return query.AsDataTable();

        }

        internal static string GetCreditCardBalance(string id, string listName)
        {
            DataTable dt = GetDataTable(id, listName);
            string balance = "0";

            var query = from e in dt.AsEnumerable()
                        where e["IsPaidByCredit"].ToString() == "1"
                        group e by e["SpecialApproved"] into grp
                        select new
                        {
                            IsSpecialApprove = grp.Key.ToString(),
                            RmbAmt = grp.Sum(cost => Decimal.Parse(string.IsNullOrEmpty(cost["RmbAmt"].ToString()) ? "0" : cost["RmbAmt"].ToString())),
                            ApprovedRmbAmt = grp.Sum(cost => Decimal.Parse(string.IsNullOrEmpty(cost["ApprovedRmbAmt"].ToString()) ? "0" : cost["ApprovedRmbAmt"].ToString()))
                        };
            foreach (var item in query)
            {
                if (!Convert.ToBoolean(int.Parse(item.IsSpecialApprove)))
                {
                    balance = (item.RmbAmt - item.ApprovedRmbAmt).ToString();
                }
            }

            return balance;
        }



        internal static void GetNetPayable(SPListItem travelExpenseClaimItem, string creditCardBalance, TextBox txtEV, TextBox txtCA)
        {
            decimal cashAdvanced = decimal.Parse(travelExpenseClaimItem["CashAdvanced"].AsString());
            decimal paidByCreditCard = decimal.Parse(travelExpenseClaimItem["PaidByCreditCard"].AsString());

            decimal totalCost = decimal.Parse(travelExpenseClaimItem["TotalCost"].AsString());

            decimal netPayable = cashAdvanced + paidByCreditCard - decimal.Parse(creditCardBalance) - totalCost;
            if (cashAdvanced + paidByCreditCard > totalCost)
            {
                txtCA.Text = netPayable.ToString();
                txtEV.Text = "0";
            }
            else
            {
                txtCA.Text = "0";
                txtEV.Text = netPayable.ToString();
            }

        }


        internal static string ReturnApplicant(string tcId)
        {
            string applicantStr = string.Empty;

            SPListItemCollection items = GetDataCollection(tcId, WorkflowListName.TravelExpenseClaim);

            if (items.Count > 0)
            {
                applicantStr = items[0]["Applicant"].AsString();
            }

            return applicantStr;
        }

        internal static string ReturnTargetFieldValue(string targetField, string queryField, string queryFieldValue, string listName)
        {
            string targetFieldValue = string.Empty;
            SPQuery query = new SPQuery();
            query.Query = WorkFlowUtil.GetQuery(queryField, queryFieldValue);

            SPListItemCollection items = SPContext.Current.Web.Lists[listName].GetItems(query);
            if (items.Count > 0)
            {
                targetFieldValue = items[0][targetField].AsString();
            }

            return targetFieldValue;
        }

        internal static void UpdateTargetFieldValue(string targetField, string targetFieldValue, string queryField, string queryFieldValue, string listName)
        {
            SPQuery query = new SPQuery();
            query.Query = WorkFlowUtil.GetQuery(queryField, queryFieldValue);

            SPListItemCollection items = SPContext.Current.Web.Lists[listName].GetItems(query);
            if (items.Count > 0)
            {
                items[0][targetField] = targetFieldValue;
                items[0].Update();
            }
        }
    }
}