using System.Data;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using System.Text;
using QuickFlow;
using CodeArt.SharePoint.CamlQuery;
using System;
using System.Collections;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.PaymentRequestSAP
{
    class PaymentRequestSAPCommon
    {
        #region Payment Request SAP Common Method

        internal static void AddItemTable(DataListEdit dataForm)
        {
            AddItemTable(dataForm.ItemTable, dataForm.CRTable, dataForm);
        }

        internal static void AddItemTable(DataTable itemTable, DataTable cRTable, DataListEdit dataForm)
        {
            //if (GetExpenseTypeResult(itemTable))
            //{
            foreach (DataRow dr in cRTable.Rows)
            {
                DataRow newdr = itemTable.Rows.Add();
                newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                newdr["CostCenter"] = dr["CostCenter"].ToString();
                newdr["ItemAmount"] = dr["ItemAmount"].ToString();
                newdr["GLAccount"] = dr["GLAccount"].ToString();
                newdr["EmployeeID"] = dr["EmployeeID"].ToString();
                newdr["EmployeeName"] = dr["EmployeeName"].ToString();
                newdr["PRWorkflowNumber"] = dr["PRWorkflowNumber"].ToString();
                newdr["AssetNo"] = dr["AssetNo"].ToString();
                newdr["BusinessArea"] = dr["BusinessArea"].ToString();
            }
            dataForm.ItemTable = itemTable;
            //}
        }

        internal static bool GetExpenseTypeResult(DataTable itemTable)
        {
            bool result = true;
            foreach (DataRow dr in itemTable.Rows)
            {
                if (dr["ExpenseType"].ToString() == "OP-Non-trade vendor" || dr["ExpenseType"].ToString() == "OR - cash advance")
                {
                    result = false;
                }
            }
            return result;
        }

        internal static void DeleteAllDraftSAPItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("Payment Request SAP Items WorkFlow", workflowNumber);
        }

        internal static void SaveSAPItemsDetails(DataListEdit dataForm, string workflowNumber)
        {
            BatchInsertSAPItems("Payment Request SAP Items WorkFlow", workflowNumber, dataForm.ItemTable);
        }

        internal static void BatchInsertSAPItems(string listName, string workflowNumber, DataTable dt)
        {
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat;
            methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ErrorMsg\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpenseType\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#GLAccount\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemAmount\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SAPNumber\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Status\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeID\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeName\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PRWorkflowNumber\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#AssetNo\">{14}</SetVar>" +
                "<SetVar Name=\"urn:schemas-microsoft-com:office:office#BusinessArea\">{15}</SetVar>" +
              "</Method>";

            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                    dr["CostCenter"].ToString(),
                                    "",
                                    dr["ExpenseType"].ToString(),
                                    dr["GLAccount"].ToString(),
                                    Math.Round(double.Parse(dr["ItemAmount"].ToString()), 2),
                                    "",
                                    "0",
                                    workflowNumber,
                                    dr["EmployeeID"].ToString(),
                                    dr["EmployeeName"].ToString(),
                                    dr["PRWorkflowNumber"].ToString(),
                                    dr["AssetNo"].ToString(),
                                     dr["BusinessArea"].ToString()
                    );
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
            }
        }

        internal static string GetGLAccountByExpenseType(string key, DataTable table)
        {
            Hashtable ht = GetExpenseTypeAndGLAccountHashTable(table);
            string gLAccount = "";
            if (key != "")
            {
                gLAccount = ht[key] != null ? ht[key].ToString() : "";
            }
            return gLAccount;
        }

        internal static Hashtable GetExpenseTypeAndGLAccountHashTable(DataTable table)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string et = "";
            if (fields["RequestType"].AsString().ToLower() == "opex")
            {
                et = "opex";
            }
            else
            {
                et = "capex";
            }
            Hashtable ht = new Hashtable();
            //DataTable dt = WorkFlowUtil.GetCollectionByList("Payment Request Expense Type").GetDataTable()
            //                                     .AsEnumerable()
            //                                     .Where(dr => dr.Field<string>("OpexCapexType").ToString().ToLower() == et)
            //                                     .CopyToDataTable();
            DataTable dt = table;
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
            }
            return ht;
        }

        internal static void BatchUpdateSAPItems(DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("Payment Request SAP Items WorkFlow");
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Status\">{3}</SetVar>" +
               "</Method>";
            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, dr["ID"].ToString(), "1");
            }


            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());
                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
            }
        }

        internal static DataTable GetDataTableToSAP(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "Payment Request SAP Items WorkFlow").GetDataTable();
        }

        public static DataTable GetExpenseTypeAndGLAccount(string requestId, DataTable dataNew)
        {
            DataTable tableNew = new DataTable();
            tableNew.Columns.Add("ExpenseType");
            tableNew.Columns.Add("GLAccount");
            tableNew.Columns.Add("OriginalExpenseType");

            //DataTable tableOld = new DataTable();
            //tableOld.Columns.Add("ExpenseType");
            //tableOld.Columns.Add("GLAccount");

            //StringBuilder expenseType = new StringBuilder();

            foreach (DataRow dr in dataNew.Rows)
            {
                DataRow rowNew = tableNew.Rows.Add();
                rowNew["ExpenseType"] = dr["NewExpenseType"].ToString();
                rowNew["GLAccount"] = dr["GLAccount"].ToString();
                rowNew["OriginalExpenseType"] = dr["ExpenseType"].ToString();

                //DataRow rowOld = tableOld.Rows.Add();
                //rowOld["ExpenseType"] = dr["ExpenseType"].ToString();
                //rowOld["GLAccount"] = dr["GLAccount"].ToString();

                //if (dr["ModifyStatus"].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                //{
                //    expenseType.Append(dr["ExpenseType"].ToString() + ";");
                //}
            }
            return tableNew;

            //DataTable dtEdit = GetDataTableToSAP(requestId);
            //if (null != dtEdit)
            //{
            //    if (dtEdit.Rows.Count > 0)
            //    {
            //        bool exists = false;
            //        foreach (DataRow row in dtEdit.Rows)
            //        {
            //            if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
            //            {
            //                exists = true;
            //                break;
            //            }
            //        }
            //        if (exists)
            //        {
            //            return tableOld;
            //        }
            //        else
            //        {
            //            return tableNew;
            //        }
            //    }
            //}
            //CommonUtil.logError(string.Format("PaymentRequestSAP：{0} :: NULL", requestId));
            //return null;
        }

        internal static SPListItemCollection GetCollectionByList(string listName)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = "<Where><Eq><FieldRef Name='Status' /><Value Type='Text'>1</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='False' /></OrderBy>";
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber(string listName, string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><Eq><FieldRef Name='PRWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            //query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber1(string listName, string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            //query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }
        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber2(string listName, string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><Eq><FieldRef Name='SubPRNo' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByPONumber(string listName, string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><Eq><FieldRef Name='PONumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            //query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        internal static DataTable GetCostCenterDT()
        {
            return WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
        }

        #endregion

        internal static string GetExpenseTypeByExpenseType(string expenseType, DataTable expenseTypeMapping) 
        {
            string newExpenseType = expenseType;
            foreach (DataRow dr in expenseTypeMapping.Rows)
            {
                string et = dr["OriginalExpenseType"].ToString();
                if (newExpenseType.Equals(et, StringComparison.CurrentCultureIgnoreCase))
                {
                    newExpenseType = dr["ExpenseType"].ToString();
                    break;
                }
            }
            return newExpenseType;
        }

    }
}