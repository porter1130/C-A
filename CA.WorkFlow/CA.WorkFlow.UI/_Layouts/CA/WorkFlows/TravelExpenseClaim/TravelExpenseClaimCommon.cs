namespace CA.WorkFlow.UI.TravelExpenseClaim
{
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using System.Text;
    using System.Collections.Generic;
    using QuickFlow;
    using System;
    using System.Reflection;
    using System.Collections;
    using System.Web.UI.WebControls;
    using QuickFlow.Core;
    using System.Web;

    class TravelExpenseClaimCommon
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

        //internal static DataTable GetTravelExpenseClaimData(string requestId, string ExpenseType) {
        //    SPList list = WorkFlowUtil.GetWorkflowList("Travel Details2");
        //    DataTable dt=null;
        //    SPListItemCollection items = GetDataCollection(requestId, "Travel Details2");

        //    switch (ExpenseType) { 
        //        case "Hotel":
        //            string[] columns={"RequestID","HotelName","TravelDateFrom","TravelDateTo","CostCenter",
        //                                 "OriginalAmt","Currency","ExchRate","RmbAmt","CompanyStandards",
        //                                 "SpecialApprove","IsPaidByCredit"};
        //            dt = CreateDataTable(columns);
        //            foreach (SPListItem item in items) {
        //                DataRow dr = dt.Rows.Add();
        //                dr["RequestID"] = item["RequestID"].ToString();
        //                dr["TravelDateFrom"] = item["TravelDateFrom"].ToString();
        //                dr["TravelDateTo"] = item["TravelDateTo"].ToString();
        //                dr["CostCenter"] = item["CostCenter"].ToString();
        //                dr["CompanyStandards"] = GetTravelPolicyByArea(item["Area"].ToString())["HotelLimit"].ToString();
        //            }
        //            //SPListItemCollection hotelItems=
        //            break;
        //        case "Meal Allowance":
        //            break;
        //        case "Local Transportation":
        //            break;
        //        case "Sample Purchase":
        //            break;
        //        case "Others":
        //            break;
        //        default:
        //            break;
        //    }
        //    return dt;
        //    }
        internal static DataTable CreateDataTable(Array columns)
        {
            DataTable dt = new DataTable();
            foreach (string s in columns)
            {
                dt.Columns.Add(s);
            }
            return dt;
        }
        internal static SPListItem GetTravelPolicyByArea(string area)
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

        internal static SPListItem ConvertToRMB(string from)
        {
            return GetExchangeRate(from, "RMB");
        }

        //Return the exchange rate item
        internal static SPListItem GetExchangeRate(string from, string to)
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


        internal static void SaveDetails(DataEdit dataEdit, string workflowNumber)
        {
            BatchInsertItems("Travel Expense Claim Details", workflowNumber, dataEdit.HotelTable);
            BatchInsertItems("Travel Expense Claim Details", workflowNumber, dataEdit.MealTable);
            BatchInsertItems("Travel Expense Claim Details", workflowNumber, dataEdit.TransTable);
            BatchInsertItems("Travel Expense Claim Details", workflowNumber, dataEdit.SampleTable);
            BatchInsertItems("Travel Expense Claim Details", workflowNumber, dataEdit.OthersTable);
        }

        private static void BatchInsertItems(string listName, string workflowNumber, DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat;

            switch (listName)
            {
                case "Travel Expense Claim Details":
                    methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpenseType\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpenseDetail\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TravelDateFrom\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TravelDateTo\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Currency\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExchRate\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CompanyStandards\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApprove\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#IsPaidByCredit\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Remark\">{14}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OriginalAmt\">{15}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#RmbAmt\">{16}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ApprovedRmbAmt\">{17}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Date\">{18}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#HidMealItem\">{19}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OtherCurrency\">{20}</SetVar>" +
               "</Method>";

                    int PaidByCreditCard, SpecialApprove;

                    if (!dt.Columns.Contains("Date"))
                    {

                        // Build the CAML update commands.
                        foreach (DataRow dr in dt.Rows)
                        {
                            PaidByCreditCard = Convert.ToBoolean(dr["IsPaidByCredit"]) ? 1 : 0;
                            SpecialApprove = Convert.ToBoolean(dr["SpecialApprove"]) ? 1 : 0;
                            //var ApprovedRmbAmt = Convert.ToBoolean(dr["SpecialApprove"]) ? dr["CompanyStandards"] : dr["RmbAmt"];
                            methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                workflowNumber,
                                dt.TableName,
                                dr["ExpenseDetail"],
                                dr["TravelDateFrom"],
                                dr["TravelDateTo"],
                                dr["CostCenter"],
                                dr["Currency"],
                                dr["ExchRate"],
                                dr["CompanyStandards"],
                                SpecialApprove,
                                PaidByCreditCard,
                                HttpUtility.HtmlEncode(dr["Remark"].AsString()),
                                dr["OriginalAmt"],
                                dr["RmbAmt"],
                                dr["RmbAmt"],
                                string.Empty,
                                string.Empty,
                                dr["OtherCurrency"]
                                );
                        }

                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            PaidByCreditCard = Convert.ToBoolean(dr["IsPaidByCredit"]) ? 1 : 0;
                            SpecialApprove = Convert.ToBoolean(dr["SpecialApprove"]) ? 1 : 0;
                            //var ApprovedRmbAmt = Convert.ToBoolean(dr["SpecialApprove"]) ? dr["CompanyStandards"] : dr["RmbAmt"];
                            var hidMealItem = dt.TableName == "Meal Allowance" ? dr["HidMealItem"] : string.Empty;
                            methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                workflowNumber,
                                dt.TableName,
                                dr["ExpenseDetail"],
                                string.Empty,
                                string.Empty,
                                dr["CostCenter"],
                                dr["Currency"],
                                dr["ExchRate"],
                                dr["CompanyStandards"],
                                SpecialApprove,
                                PaidByCreditCard,
                                HttpUtility.HtmlEncode(dr["Remark"].AsString()),
                                dr["OriginalAmt"],
                                dr["RmbAmt"],
                                dr["RmbAmt"],
                                dr["Date"],
                                hidMealItem,
                                dr["OtherCurrency"]
                                );
                        }

                    }

                    break;

                default:
                    break;
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

        internal static void BatchUpdateItems(string listName, string workflowNumber, System.Web.UI.WebControls.Repeater repeater)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat;

            switch (listName)
            {
                case "Travel Expense Claim Details":
                    methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApproved\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ApprovedRmbAmt\">{4}</SetVar>" +
               "</Method>";


                    // Build the CAML update commands.
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        var itemID = (HiddenField)item.FindControl("ItemID");
                        var cbSpecialApprove = (CheckBox)item.FindControl("cbSpecialApproved");
                        var hidRmbAmt = (HiddenField)item.FindControl("hidRmbAmt");
                        var hidCompanyStandards = (HiddenField)item.FindControl("hidCompanyStandards");

                        var SpecialApproved = cbSpecialApprove.Checked ? 1 : 0;
                        var ApprovedRmbAmt = hidRmbAmt.Value;
                        methodBuilder.AppendFormat(methodFormat, 0, listGuid, itemID.Value,
                            SpecialApproved,
                            ApprovedRmbAmt
                            );

                    }

                    break;

                default:
                    break;
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

        internal static string SaveListFields(string requestId, string listName, QuickFlow.Core.WorkflowDataFields fields, List<string> fieldsList)
        {

            SPListItemCollection travelRequestItems = TravelExpenseClaimCommon.GetDataCollection(requestId, listName);
            var department = travelRequestItems[0]["Department"].AsString();

            foreach (string s in fieldsList)
            {
                fields[s] = travelRequestItems[0][s];
            }

            fields["WorkflowNumber"] = "TE" + WorkFlowUtil.CreateWorkFlowNumber("TravelExpenseClaim").ToString("000000");
            fields["TRWorkflowNumber"] = travelRequestItems[0]["WorkflowNumber"].AsString();

            return fields["WorkflowNumber"].ToString();
        }

        internal static string SaveListFields(QuickFlow.Core.WorkflowDataFields fields, SPListItem item, List<string> fieldsList)
        {
            string department = fields["Department"].AsString();
            foreach (string s in fieldsList)
            {
                item[s] = fields[s].AsString();
            }

            item["WorkflowNumber"] = "SAP" + WorkFlowUtil.CreateWorkFlowNumber(WorkflowConfigName.TravelExpenseClaimForSAP).ToString("000000");
            item["TCWorkflowNumber"] = fields["WorkflowNumber"].AsString();

            return item["WorkflowNumber"].AsString();
        }

        internal static void DeleteAllDraftItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("Travel Expense Claim Details", workflowNumber);
        }

        internal static DataTable GetDataSource(DataTable dataTable, DataTable sourceTable, string exp)
        {
            DataRow[] rows = sourceTable.Select(exp);

            foreach (DataRow row in rows)
            {
                dataTable.ImportRow(row);
            }

            return dataTable;
        }

        internal static bool IsLastTask(string listId, string itemId)
        {
            int count = 0;
            SPListItemCollection coll = null;
            coll = GetAllTasks(listId, itemId);
            foreach (SPListItem item in coll)
            {
                if (item["Status"].AsString().Equals("Not Started", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    count++;
                }
            }
            return count == 1;
        }

        internal static SPListItemCollection GetAllTasks(string listId, string itemId)
        {
            var qWorkflowListId = new QueryField("WorkflowListId", false);
            var qWorkflowItemId = new QueryField("WorkflowItemId", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qWorkflowListId.Equal(listId));
            exp = WorkFlowUtil.LinkAnd(exp, qWorkflowItemId.Equal(itemId));
            SPListItemCollection lc = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                            .From(web.Lists["Tasks"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });
            return lc;
        }

        internal static NameCollection GetTaskUsers(NameCollection manager, string managerAccount)
        {
            manager.Add(managerAccount);

            string moduleID = WorkFlowUtil.GetModuleIdByListName("TravelExpenseClaimWorkflow");

            var deleman = WorkFlowUtil.GetDeleman(managerAccount, moduleID);
            if (deleman != null)
            {
                manager.Add(deleman);
            }

            return manager;
        }


        internal static NameCollection GetTaskUsersWithoutDeleman(string group)
        {
            string moduleID = WorkFlowUtil.GetModuleIdByListName("TravelExpenseClaimWorkflow");
            return WorkFlowUtil.GetTaskUsersWithoutDeleman(group, moduleID);
        }

        internal static string GetRedirectTRListItemUrl(string requestId)
        {
            SPListItemCollection items = GetDataCollection(requestId, "Travel Request Workflow2");

            return string.Concat(items[0].ParentList.Forms[PAGETYPE.PAGE_DISPLAYFORM].ServerRelativeUrl,
                "?id=" + items[0].ID.ToString());

        }

        internal static void SwitchControl(string type, bool isValid)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            switch (type)
            {
                case "Pending":
                    if (!isValid)
                    {
                        fields["IsPending"] = "1";
                    }
                    else
                    {
                        fields["IsPending"] = "0";
                    }
                    break;
                case "Notice":
                    if (!isValid)
                    {
                        fields["IsNotice"] = "1";
                    }
                    else
                    {
                        fields["IsNotice"] = "0";
                    }
                    break;
                default:
                    if (!isValid)
                    {
                        fields["IsPending"] = "1";
                        fields["IsNotice"] = "1";
                    }
                    else
                    {
                        fields["IsPending"] = "0";
                        fields["IsNotice"] = "0";
                    }
                    break;
            }
        }
    }
}