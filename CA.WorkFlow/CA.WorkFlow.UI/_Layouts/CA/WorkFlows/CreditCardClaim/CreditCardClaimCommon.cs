using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.SharePoint;
using System.Text;
using CA.SharePoint.Utilities.Common;
using System.Collections;
using CodeArt.SharePoint.CamlQuery;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI;
using SAP.Middleware.Exchange;
using CA.SharePoint;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    public class CreditCardClaimCommon
    {

        internal static void AddItemTable(DataListEdit dataForm)
        {
            AddItemTable(dataForm.ItemTable, dataForm.CRTable, dataForm.USDItemTable, dataForm);
        }
        internal static void AddItemTable(DataTable itemTable, DataTable cRTable, DataTable usdItemTable, DataListEdit dataForm)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ExpenseType");
            dt.Columns.Add("DealAmount");
            dt.Columns.Add("DepositAmount");
            dt.Columns.Add("PayAmount");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("GLAccount");
            dt.Columns.Add("AmountType");
            dt.Columns.Add("TransactionDescription");
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("CCCWWorkflowNumber");
            dt.Columns.Add("CreditCardBillID");
            if (GetExpenseTypeResult(itemTable))
            {
                foreach (DataRow dr in itemTable.Rows)
                {
                    DataRow newdr = dt.Rows.Add();
                    newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                    newdr["CostCenter"] = dr["CostCenter"].ToString();
                    newdr["DealAmount"] = dr["DealAmount"].ToString();
                    newdr["DepositAmount"] = dr["DepositAmount"].ToString();
                    newdr["PayAmount"] = dr["PayAmount"].ToString();
                    newdr["GLAccount"] = dr["GLAccount"].ToString();
                    newdr["EmployeeID"] = dr["EmployeeID"].ToString();
                    newdr["EmployeeName"] = dr["EmployeeName"].ToString();
                    newdr["AmountType"] = dr["AmountType"].ToString();
                    newdr["CCCWWorkflowNumber"] = dr["CCCWWorkflowNumber"].ToString();
                    newdr["TransactionDescription"] = dr["TransactionDescription"].ToString();
                    newdr["CreditCardBillID"] = dr["CreditCardBillID"].ToString();
                }
                foreach (DataRow dr in cRTable.Rows)
                {
                    if (dr["AmountType"].ToString() == "RMB")
                    {
                        DataRow newdr = dt.Rows.Add();
                        newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                        newdr["CostCenter"] = dr["CostCenter"].ToString();
                        newdr["DealAmount"] = dr["DealAmount"].ToString();
                        newdr["DepositAmount"] = dr["DepositAmount"].ToString();
                        newdr["PayAmount"] = dr["PayAmount"].ToString();
                        newdr["GLAccount"] = dr["GLAccount"].ToString();
                        newdr["EmployeeID"] = dr["EmployeeID"].ToString();
                        newdr["EmployeeName"] = dr["EmployeeName"].ToString();
                        newdr["AmountType"] = dr["AmountType"].ToString();
                        newdr["CCCWWorkflowNumber"] = dr["CCCWWorkflowNumber"].ToString();
                        newdr["TransactionDescription"] = dr["TransactionDescription"].ToString();
                        newdr["CreditCardBillID"] = dr["CreditCardBillID"].ToString();
                    }
                }
                foreach (DataRow dr in usdItemTable.Rows)
                {
                    DataRow newdr = dt.Rows.Add();
                    newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                    newdr["CostCenter"] = dr["CostCenter"].ToString();
                    newdr["DealAmount"] = dr["DealAmount"].ToString();
                    newdr["DepositAmount"] = dr["DepositAmount"].ToString();
                    newdr["PayAmount"] = dr["PayAmount"].ToString();
                    newdr["GLAccount"] = dr["GLAccount"].ToString();
                    newdr["EmployeeID"] = dr["EmployeeID"].ToString();
                    newdr["EmployeeName"] = dr["EmployeeName"].ToString();
                    newdr["AmountType"] = dr["AmountType"].ToString();
                    newdr["CCCWWorkflowNumber"] = dr["CCCWWorkflowNumber"].ToString();
                    newdr["TransactionDescription"] = dr["TransactionDescription"].ToString();
                    newdr["CreditCardBillID"] = dr["CreditCardBillID"].ToString();
                }
                foreach (DataRow dr in cRTable.Rows)
                {
                    if (dr["AmountType"].ToString() == "USD")
                    {
                        DataRow newdr = dt.Rows.Add();
                        newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                        newdr["CostCenter"] = dr["CostCenter"].ToString();
                        newdr["DealAmount"] = dr["DealAmount"].ToString();
                        newdr["DepositAmount"] = dr["DepositAmount"].ToString();
                        newdr["PayAmount"] = dr["PayAmount"].ToString();
                        newdr["GLAccount"] = dr["GLAccount"].ToString();
                        newdr["EmployeeID"] = dr["EmployeeID"].ToString();
                        newdr["EmployeeName"] = dr["EmployeeName"].ToString();
                        newdr["AmountType"] = dr["AmountType"].ToString();
                        newdr["CCCWWorkflowNumber"] = dr["CCCWWorkflowNumber"].ToString();
                        newdr["TransactionDescription"] = dr["TransactionDescription"].ToString();
                        newdr["CreditCardBillID"] = dr["CreditCardBillID"].ToString();
                    }
                }

                dataForm.ItemTable = dt;
            }
        }
        private static bool GetExpenseTypeResult(DataTable itemTable)
        {
            bool result = true;
            foreach (DataRow dr in itemTable.Rows)
            {
                if (dr["ExpenseType"].ToString() == "OR - employee vendor")
                {
                    result = false;
                }
            }
            return result;
        }
        internal static DataTable GetDataSource(DataTable sourceTable, string exp)
        {
            DataTable dt = null;
            if (sourceTable != null)
            {
                dt = sourceTable.Clone();
                DataRow[] rows = sourceTable.Select(exp);

                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        dt.ImportRow(row);
                    }
                }
            }

            return dt;
        }



        internal static void BatchInsertRepeaterData(Repeater repeater, string listName, string workflowNumber, Hashtable hashtable)
        {
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            StringBuilder fieldsBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"0\">" +
            "<SetList Scope=\"Request\">" + listGuid + "</SetList>" +
            "<SetVar Name=\"ID\">New</SetVar>" +
            "<SetVar Name=\"Cmd\">Save</SetVar>" +
            "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">" + workflowNumber + "</SetVar>{0}</Method>";

            string fieldFormat = "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>";


            foreach (RepeaterItem item in repeater.Items)
            {

                foreach (DictionaryEntry entry in hashtable)
                {

                    string[] value = entry.Value.ToString().Split(new string[] { ";#" }, StringSplitOptions.None);
                    string controlId = value[0];
                    string controlType = value[1];
                    Control control = item.FindControl(controlId);
                    string itemValue = string.Empty;

                    switch (controlType)
                    {
                        case "Label":
                            Label label = (Label)control;
                            itemValue = label.Text;
                            break;
                        case "DropDownList":
                            DropDownList dropDownList = (DropDownList)control;
                            itemValue = dropDownList.SelectedValue;
                            break;
                        case "CheckBox":
                            CheckBox checkBox = (CheckBox)control;
                            itemValue = checkBox.Checked ? "1" : "0";
                            break;
                        default:
                            TextBox textBox = (TextBox)control;
                            itemValue = textBox.Text;
                            break;
                    }
                    itemValue = itemValue.Replace("<", "")
                                         .Replace(">", "");
                    fieldsBuilder.AppendFormat(fieldFormat,
                                                entry.Key.ToString(),
                                                itemValue);
                }

                methodBuilder.AppendFormat(methodFormat, fieldsBuilder.ToString());

            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
                CommonUtil.logError(String.Format("workflowNumber:{0}\r\n batchReturn:{1}", workflowNumber, batchReturn));
                //SPSecurity.RunWithElevatedPrivileges(delegate {
                //    if (!EventLog.SourceExists("C&A"))
                //    {
                //        EventLog.CreateEventSource("C&A", "Mail");
                //    }

                //    // Create an EventLog instance and assign its source.
                //    EventLog myLog = new EventLog();
                //    myLog.Source = "C&A";

                //    // Write an informational entry to the event log.    
                //    myLog.WriteEntry("Tag Travel Requeset batch insert:"+batchReturn, EventLogEntryType.Information);    
                //});
            }

        }

        internal static void BatchInsertExcelData(DataTable dt, string listName, Hashtable hashtable)
        {
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            StringBuilder fieldsBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"0\">" +
            "<SetList Scope=\"Request\">" + listGuid + "</SetList>" +
            "<SetVar Name=\"ID\">New</SetVar>" +
            "<SetVar Name=\"Cmd\">Save</SetVar>{0}</Method>";

            string fieldFormat = "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>";

            int i = 0;
            List<string> colNameList = new List<string>();

            foreach (DictionaryEntry enry in hashtable)
            {
                fieldsBuilder.AppendFormat(fieldFormat, enry.Value.ToString(), "{" + (i++).ToString() + "}");
                colNameList.Add(enry.Key.ToString());
            }

            if (fieldsBuilder.ToString().IsNotNullOrWhitespace())
            {
                methodFormat = string.Format(methodFormat, fieldsBuilder.ToString());
                // Build the CAML insert commands............................................................................
                foreach (DataRow dr in dt.Rows)
                {
                    string[] valueList = GetValueList(dr, colNameList);

                    methodBuilder.AppendFormat(methodFormat, valueList);
                }

                if (methodBuilder.ToString().IsNotNullOrWhitespace())
                {
                    // Put the pieces together.
                    batch = string.Format(batchFormat, methodBuilder.ToString());

                    // Process the batch of commands.
                    string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);

                    //SPSecurity.RunWithElevatedPrivileges(delegate {
                    //    if (!EventLog.SourceExists("C&A"))
                    //    {
                    //        EventLog.CreateEventSource("C&A", "Mail");
                    //    }

                    //    // Create an EventLog instance and assign its source.
                    //    EventLog myLog = new EventLog();
                    //    myLog.Source = "C&A";

                    //    // Write an informational entry to the event log.    
                    //    myLog.WriteEntry("Tag Travel Requeset batch insert:"+batchReturn, EventLogEntryType.Information);    
                    //});
                }
            }
        }

        internal static void BatchInsertExcelData(DataTable dt, string listName, Hashtable hashtable, string month)
        {
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            StringBuilder fieldsBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"0\">" +
            "<SetList Scope=\"Request\">" + listGuid + "</SetList>" +
            "<SetVar Name=\"ID\">New</SetVar>" +
            "<SetVar Name=\"Cmd\">Save</SetVar>" +
            "<SetVar Name=\"urn:schemas-microsoft-com:office:office#UploadDate\">" + month + "</SetVar>" +
            "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SAPID\">" + DateTime.Now.ToString("MMddhhmmssffff") + "</SetVar>{0}</Method>";

            string fieldFormat = "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>";

            int i = 0;
            List<string> colNameList = new List<string>();

            foreach (DictionaryEntry enry in hashtable)
            {
                fieldsBuilder.AppendFormat(fieldFormat, enry.Value.ToString(), "{" + (i++).ToString() + "}");
                colNameList.Add(enry.Key.ToString());
            }

            if (fieldsBuilder.ToString().IsNotNullOrWhitespace())
            {
                methodFormat = string.Format(methodFormat, fieldsBuilder.ToString());
                // Build the CAML insert commands............................................................................
                foreach (DataRow dr in dt.Rows)
                {
                    string[] valueList = GetValueList(dr, colNameList);

                    methodBuilder.AppendFormat(methodFormat, valueList);
                }

                if (methodBuilder.ToString().IsNotNullOrWhitespace())
                {
                    // Put the pieces together.
                    batch = string.Format(batchFormat, methodBuilder.ToString());

                    // Process the batch of commands.
                    string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);

                    //SPSecurity.RunWithElevatedPrivileges(delegate {
                    //    if (!EventLog.SourceExists("C&A"))
                    //    {
                    //        EventLog.CreateEventSource("C&A", "Mail");
                    //    }

                    //    // Create an EventLog instance and assign its source.
                    //    EventLog myLog = new EventLog();
                    //    myLog.Source = "C&A";

                    //    // Write an informational entry to the event log.    
                    //    myLog.WriteEntry("Tag Travel Requeset batch insert:"+batchReturn, EventLogEntryType.Information);    
                    //});
                }
            }
        }

        internal static void DeleteAllDraftItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems(WorkflowConfigName.CreditCardClaimDetail, workflowNumber);
        }

        private static string[] GetValueList(DataRow dr, List<string> colNameList)
        {
            List<string> temp = new List<string>();

            foreach (string colName in colNameList)
            {
                temp.Add(dr[colName].AsString());
            }
            return temp.ToArray();
        }

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



        internal static string FormatDateString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        internal static bool IsUniqueValue(List<string> list, string str)
        {
            bool isUnique = true;
            foreach (string s in list)
            {
                if (str.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                {
                    isUnique = false;
                    break;
                }
            }
            return isUnique;
        }

        internal static SPListItem ConvertToUSD(string from)
        {
            return GetExchangeRate(from, "USD");
        }

        internal static SPListItem ConvertToRMB(string from)
        {
            return GetExchangeRate(from, "RMB");
        }

        //Return the exchange rate item
        private static SPListItem GetExchangeRate(string from, string to)
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

        internal static List<object> GetSerializingList(DataTable dt, CreditCardBillItem creditCardBillItem)
        {
            throw new NotImplementedException();
        }



        internal static DataTable GetCostCenterDT()
        {
            return WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
        }

        internal static DataTable GetDataTableToSAP(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "Credit Card Claim SAP Detail").GetDataTable();
        }

        internal static string GetGLAccountByExpenseType(string key)
        {
            Hashtable ht = GetExpenseTypeAndGLAccountHashTable();
            string gLAccount = "";
            if (key != "")
            {
                gLAccount = ht[key].ToString();
            }
            return gLAccount;
        }

        internal static Hashtable GetExpenseTypeAndGLAccountHashTable()
        {
            DataTable dt = WorkFlowUtil.GetCollectionByList("Expense Claim SAP GLAccount").GetDataTable();
            Hashtable ht = new Hashtable();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
                }
            }
            return ht;
        }

        internal static void DeleteAllDraftSAPItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("Credit Card Claim SAP Detail", workflowNumber);
        }
        internal static void SaveSAPItemsDetails(DataListEdit dataForm, string workflowNumber)
        {
            BatchInsertSAPItems("Credit Card Claim SAP Detail", workflowNumber, dataForm.ItemTable);
        }
        //Batch Insert
        private static void BatchInsertSAPItems(string listName, string workflowNumber, DataTable dt)
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#DealAmount\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SAPNumber\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Status\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeID\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeName\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CCCWWorkflowNumber\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#DepositAmount\">{14}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PayAmount\">{15}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#AmountType\">{16}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TransactionDescription\">{17}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CreditCardBillID\">{18}</SetVar>" +
               "</Method>";

            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                    dr["CostCenter"].ToString(),
                                    "",
                                    dr["ExpenseType"].ToString(),
                                    dr["GLAccount"].ToString(),
                                    dr["DealAmount"].ToString(),
                                    "",
                                    "0",
                                    workflowNumber,
                                    dr["EmployeeID"].ToString(),
                                    dr["EmployeeName"].ToString(),
                                    dr["CCCWWorkflowNumber"].ToString(),
                                    dr["DepositAmount"].ToString(),
                                    dr["PayAmount"].ToString(),
                                    dr["AmountType"].ToString(),
                                    dr["TransactionDescription"].ToString(),
                                    dr["CreditCardBillID"].ToString()
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
        internal static void BatchUpdateSAPItems(DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("Credit Card Claim SAP Detail");
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
        internal static SPListItemCollection GetCollectionByList(string listName)
        {
            //var qStatus = new QueryField("Status", false);
            //CamlExpression exp = null;
            //exp = WorkFlowUtil.LinkAnd(exp, qStatus.Equal("1"));
            //SPListItemCollection lc = null;

            //SPSecurity.RunWithElevatedPrivileges(delegate
            //{
            //    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            //    {
            //        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
            //        {
            //            lc = ListQuery.Select()
            //                             .From(WorkFlowUtil.GetWorkflowList(listName))
            //                             .Where(exp)
            //                             .GetItems();
            //        }
            //    }
            //});

            //return lc;

            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = "<Where><Eq><FieldRef Name='Status' /><Value Type='Text'>1</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='False' /></OrderBy>";
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        #region SAP

        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber(string listName, string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><Eq><FieldRef Name='CCCWWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber(string listName, string workFlowNumber, string amountType)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><And><Eq><FieldRef Name='CCCWWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='AmountType' /><Value Type='Text'>{1}</Value></Eq></And></Where>", workFlowNumber, amountType);
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }

        #endregion


        internal static string BatchPostToSAP(DataTable dataTable)
        {
            string sapNo = "";
            List<string> sapNoList = new List<string>();
            if (dataTable != null)
            {

                // Employee employee = UserProfileUtil.GetEmployeeEx(SPContext.Current.Web.CurrentUser.LoginName);

                List<SapParameter> mSapParametersCC = new List<SapParameter>();

                SapParameter sapParameterForRMB = InitializeSapParameter("RMB", dataTable);
                SapParameter sapParameterForUSD = InitializeSapParameter("USD", dataTable);

                if (sapParameterForRMB.RefDocNo.IsNotNullOrWhitespace())
                {
                    mSapParametersCC.Add(sapParameterForRMB);
                }
                if (sapParameterForUSD.RefDocNo.IsNotNullOrWhitespace())
                {
                    mSapParametersCC.Add(sapParameterForUSD);
                }
                ISapExchange tec1 = SapExchangeFactory.GetCreditCard();
                List<object[]> result = tec1.ImportDataToSap(mSapParametersCC);
                for (int i = 0; i < result.Count; i++)
                {
                    bool bl = (bool)result[i][2];
                    SapParameter sp = (SapParameter)result[i][0];
                    SPListItemCollection items = CreditCardClaimCommon.GetDataCollectionByRefDocNo(sp.RefDocNo, WorkflowListName.CreditCardBill);
                    if (items != null)
                    {
                        SPListItem sapWFItem = items[0];
                        if (bl)
                        {
                            SapResult sr = (SapResult)result[i][1];
                            sapWFItem["SAPNo"] += sr.OBJ_KEY + ";";
                            sapNoList.Add(sr.OBJ_KEY);
                        }
                        else
                        {
                            string errorFormat = "{0} {1}({2}) Post fails:\n";
                            sapWFItem["ErrorMsg"] += string.Format(errorFormat, DateTime.Now.ToShortTimeString(), sp.EmployeeName, sp.EmployeeID);
                            if (result[i][1] is string)
                            {
                                sapWFItem["ErrorMsg"] += result[i][1].ToString() + "\n";
                            }
                            else
                            {
                                SapResult sr = (SapResult)result[i][1];

                                foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
                                {
                                    sapWFItem["ErrorMsg"] += ret.MESSAGE + "\n";
                                }

                            }
                        }

                        //Update WF Item
                        SPContext.Current.Web.AllowUnsafeUpdates = true;
                        sapWFItem.Update();
                        SPContext.Current.Web.AllowUnsafeUpdates = false;
                    }
                }

                sapNo = string.Join(",", sapNoList.ToArray());
            }
            return sapNo;
        }

        private static SPListItemCollection GetDataCollectionByRefDocNo(string refDocNo, string listName)
        {
            SPQuery query = new SPQuery();
            string queryFormat = @"<Where>
                                  <Eq>
                                     <FieldRef Name='SAPID' />
                                     <Value Type='Text'>{0}</Value>
                                  </Eq>
                               </Where>";
            query.Query = string.Format(queryFormat, refDocNo);

            SPListItemCollection items = SPContext.Current.Web.Lists[listName].GetItems(query);

            return items.Count > 0 ? items : null;

        }

        private static SapParameter InitializeSapParameter(string type, DataTable dataTable)
        {
            string bankInfo = ExcelService.GetExcelConfigInfo("CC_BankInfo");
            string bankId = bankInfo.Split(new string[] { ";#" }, StringSplitOptions.None)[0];
            string bankName = bankInfo.Split(new string[] { ";#" }, StringSplitOptions.None)[1];

            SapParameter mSapParameters = new SapParameter()
            {
                BusAct = "RFBU",
                BusArea = "0001",
                //ExchRate = 1,
                BankId = bankId,
                DocType = "KR",
                BankName = bankName,
                Header = "Credit Card" //工作流名字
            };



            Hashtable parametersHash = new Hashtable();
            //parametersHash.Add("RefDocNo", refDocNo);

            switch (type)
            {
                case "RMB":

                    parametersHash.Add("Currency", "RMB");
                    parametersHash.Add("ExchRate", "1");
                    DataTable rmbDataTable = CreditCardClaimCommon.GetDataSource(dataTable, "Currency='RMB'");
                    if (rmbDataTable.Rows.Count > 0)
                    {
                        parametersHash.Add("RefDocNo", CreditCardClaimCommon.GetDataSource(dataTable, "Currency='RMB'").Rows[0]["SAPID"].AsString());
                    }
                    else
                    {
                        parametersHash.Add("RefDocNo", string.Empty);
                    }
                    SetSapParameter(mSapParameters, parametersHash);

                    DataTable rmbDT = ConvertToSAPDataSource(dataTable, type);
                    mSapParameters.ExpenceDetails = GetSapParametersList(rmbDT);
                    break;
                case "USD":
                    parametersHash.Add("Currency", "USD");
                    parametersHash.Add("ExchRate", "0");
                    DataTable usdDataTable = CreditCardClaimCommon.GetDataSource(dataTable, "Currency<>'RMB'");
                    if (usdDataTable.Rows.Count > 0)
                    {
                        parametersHash.Add("RefDocNo", CreditCardClaimCommon.GetDataSource(dataTable, "Currency<>'RMB'").Rows[0]["SAPID"].AsString());
                    }
                    else
                    {
                        parametersHash.Add("RefDocNo", string.Empty);
                    }
                    SetSapParameter(mSapParameters, parametersHash);

                    DataTable usdDT = ConvertToSAPDataSource(dataTable, type);
                    mSapParameters.ExpenceDetails = GetSapParametersList(usdDT);
                    break;
                default:
                    break;
            }

            return mSapParameters;

        }

        private static void SetSapParameter(SapParameter mSapParameters, Hashtable parametersHash)
        {
            foreach (DictionaryEntry entry in parametersHash)
            {
                foreach (PropertyInfo prop in mSapParameters.GetType().GetProperties())
                {
                    if (entry.Key.AsString() == prop.Name)
                    {

                        switch (prop.PropertyType.ToString())
                        {
                            case "System.Decimal":
                                prop.SetValue(mSapParameters, decimal.Parse(entry.Value.AsString()), null);
                                break;
                            default:
                                prop.SetValue(mSapParameters, entry.Value.AsString(), null);
                                break;
                        }

                        break;
                    }
                }
            }
        }

        private static List<ExpenceDetail> GetSapParametersList(DataTable dt)
        {
            List<ExpenceDetail> oList = new List<ExpenceDetail>();
            ExpenceDetail expenceDetail;

            decimal tmp;

            foreach (DataRow dr in dt.Rows)
            {
                expenceDetail = new ExpenceDetail();
                foreach (PropertyInfo prop in expenceDetail.GetType().GetProperties())
                {
                    if (dt.Columns.Contains(prop.Name))
                    {
                        switch (prop.PropertyType.ToString())
                        {
                            case "System.Decimal":
                                if (decimal.TryParse(dr[prop.Name].AsString(), out tmp))
                                {
                                    prop.SetValue(expenceDetail, tmp, null);
                                }
                                break;
                            case "System.Boolean":
                                prop.SetValue(expenceDetail, Convert.ToBoolean(int.Parse(dr[prop.Name].AsString())), null);
                                break;
                            default:
                                prop.SetValue(expenceDetail, dr[prop.Name].AsString(), null);
                                break;
                        }

                    }
                }
                oList.Add(expenceDetail);
            }

            return oList;
        }

        private static DataTable ConvertToSAPDataSource(DataTable dataTable, string type)
        {
            var query = from e in dataTable.AsEnumerable()
                        where e["Currency"].AsString() == type
                        select new
                        {
                            RefKey = e["ID"].AsString(),
                            Amount = decimal.Parse(e["PayAmt"].AsString()) - decimal.Parse(e["DepositAmt"].AsString()),
                            ItemText = e["MerchantName"].AsString(),
                            EmpID = e["EmployeeId"].AsString(),
                            Currency= type
                        };

            return query.AsDataTable();
        }

        internal static string GetQuery(string field, string value)
        {
            string queryFormat = @"<Where>
                                      <Eq>
                                         <FieldRef Name='{0}' />
                                         <Value Type='Text'>{1}</Value>
                                      </Eq>
                                   </Where>";

            return string.Format(queryFormat, field, value);

        }
    }
}