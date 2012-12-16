namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    using System.Data;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using System.Text;
    using QuickFlow;
    using CodeArt.SharePoint.CamlQuery;
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System.Linq;
    class EmployeeExpenseClaimCommon
    {
        #region

        internal static void SaveDetails(DataEdit dataForm, string workflowNumber)
        {
            BatchInsertItems("EmployeeExpenseClaimItems", workflowNumber, dataForm.ItemTable);
        }

        internal static void SaveDetails1(string expatriateBenefitForm, string workflowNumber)
        {
            BatchInsertItems("EmployeeExpenseClaimItems", workflowNumber, expatriateBenefitForm);
        }

        internal static void SaveSAPItemsDetails(DataListEdit dataForm, string workflowNumber)
        {
            BatchInsertSAPItems("EmployeeExpenseClaimSAPItemsWorkFlow", workflowNumber, dataForm.ItemTable);
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

        internal static void AddItemTable(DataListEdit dataForm)
        {
            AddItemTable(dataForm.ItemTable, dataForm.CRTable, dataForm);
        }

        internal static string GetDeleman(string account)
        {
            return WorkFlowUtil.GetDeleman(account, WorkFlowUtil.GetModuleIdByListName("EmployeeExpenseClaimWorkflow"));
        }

        internal static NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, WorkFlowUtil.GetModuleIdByListName("EmployeeExpenseClaimWorkflow"));
        }

        internal static NameCollection GetTaskUsers(NameCollection manager, string managerAccount)
        {
            manager.Add(managerAccount);

            var deleman = WorkFlowUtil.GetDeleman(managerAccount, WorkFlowUtil.GetModuleIdByListName("EmployeeExpenseClaimWorkflow"));
            if (deleman != null)
            {
                manager.Add(deleman);
            }

            return manager;
        }

        internal static void DeleteAllDraftItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("EmployeeExpenseClaimItems", workflowNumber);
        }

        internal static void DeleteAllDraftSAPItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("EmployeeExpenseClaimSAPItemsWorkFlow", workflowNumber);
        }

        //Return data table from "EmployeeExpenseClaimItems" list according by given requestId.
        internal static DataTable GetDataTable(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "EmployeeExpenseClaimItems").GetDataTable();
        }
        internal static DataTable GetDataTableToSAP(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "EmployeeExpenseClaimSAPItemsWorkFlow").GetDataTable();
        }
        internal static DataTable GetCostCenterDT()
        {
            return WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
        }

        //Batch Insert
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
            methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpenseType\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Dates\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Amount\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PreAmount\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CompanyStandard\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#NeedSpecialApprove\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApproveResult\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalAmount\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Remark\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpensePurpose\">{14}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OriginalAmount\">{15}</SetVar>" +
               "</Method>";

            // Build the CAML update commands.
            var needSpecialApprove = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["CompanyStandard"].AsString() == "no limit")
                {
                    needSpecialApprove = false;
                }
                else
                {
                    needSpecialApprove = string.IsNullOrEmpty(dr["CompanyStandard"].AsString()) ? true : Convert.ToInt32(dr["CompanyStandard"]) < 0 ? false : Convert.ToDouble(dr["Amount"]) > Convert.ToDouble(dr["CompanyStandard"]);
                }
                string cs = dr["CompanyStandard"].AsString();

                if (cs == "-1")
                {
                    cs = "";
                }
                string dates = dr["Dates"].ToString();
                int begin = dates.IndexOf("/");
                int end = dates.LastIndexOf("/");
                if (end > begin)
                {
                    DateTime time = Convert.ToDateTime(dr["Dates"].ToString());
                    dates = time.Month.ToString() + "/" + time.Day.ToString() + "/" + time.Year.ToString();
                }

                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                    workflowNumber,
                    dr["ExpenseType"],
                    dates,
                    dr["CostCenter"],
                    dr["Amount"],
                    dr["Amount"],
                    //dr["CompanyStandard"].AsString() == "-1" ? "" : dr["CompanyStandard"],
                  cs,
                   needSpecialApprove,
                   needSpecialApprove == true ? "1" : "0",
                    dr["DdlTotalAmount"],
                    dr["Remark"],
                     dr["ExpensePurpose"],
                      dr["Amount"]
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

        private static void BatchInsertItems(string listName, string workflowNumber, string expatriateBenefitForm)
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpenseType\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Dates\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Amount\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PreAmount\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Remark\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpensePurpose\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CompanyStandard\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#NeedSpecialApprove\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApproveResult\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OriginalAmount\">{14}</SetVar>" +
               "</Method>";
            string regexText = @"\{[^\{-\}]*\}";
            Regex regex = new Regex(regexText);
            MatchCollection mc = regex.Matches(expatriateBenefitForm);
            foreach (Match m in mc)
            {
                List<string> itemList = m.Value.Replace("{", "").Replace("}", "").Split(',').ToList<string>();

                var needSpecialApprove = false;
                string std = itemList[5].Replace("ComStd:'", "").Replace("'", "").Trim();
                string amount = itemList[4].Replace("Amount:'", "").Replace("'", "").Trim();
                if (std == "no limit" || string.IsNullOrEmpty(std))
                {
                    needSpecialApprove = false;
                }
                else
                {
                    needSpecialApprove = Convert.ToDouble(amount) > Convert.ToDouble(std);
                }
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                            workflowNumber,
                                            itemList[0].Replace("BenefitType:'", "").Replace("'", "").Trim(),
                                            itemList[1].Replace("Date:'", "").Replace("'", "").Trim(),
                                            itemList[3].Replace("CostCenter:'", "").Replace("'", "").Trim(),
                                            itemList[4].Replace("Amount:'", "").Replace("'", "").Trim(),
                                            itemList[4].Replace("Amount:'", "").Replace("'", "").Trim(),
                                            itemList[6].Replace("Remark:'", "").Replace("'", "").Trim(),
                                            itemList[2].Replace("ExpensePurpose:'", "").Replace("'", "").Trim(),
                                            itemList[5].Replace("ComStd:'", "").Replace("'", "").Trim(),
                                            needSpecialApprove,
                                            needSpecialApprove == true ? "1" : "0",
                                            itemList[4].Replace("Amount:'", "").Replace("'", "").Trim()
                                            );
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
                CommonUtil.logInfo(workflowNumber + "\n\n" + batchReturn);
            }
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemAmount\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SAPNumber\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Status\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeID\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeName\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EECWWorkflowNumber\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CashAmount\">{14}</SetVar>" +
              "</Method>";

            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                    dr["CostCenter"].ToString(),
                                    "",
                                    dr["ExpenseType"].ToString(),
                                    dr["GLAccount"].ToString(),
                                    dr["ItemAmount"].ToString(),
                                    "",
                                    "0",
                                    workflowNumber,
                                    dr["EmployeeID"].ToString(),
                                    dr["EmployeeName"].ToString(),
                                    dr["EECWWorkflowNumber"].ToString(),
                                    dr["CashAmount"].ToString()
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

        internal static void BatchUpdateItems(DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("EmployeeExpenseClaimItems");
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Amount\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApproveResult\">{4}</SetVar>" +
               "</Method>";

            // Build the CAML update commands.
            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, dr["ID"].ToString(),
                    dr["ApprovedAmount"].ToString(),
                    dr["IsSpecialApprove"].ToString()
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
            SPList list = WorkFlowUtil.GetWorkflowList("EmployeeExpenseClaimSAPItemsWorkFlow");
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

        internal static void BatchUpdateItems(DataTable dt, string type)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("EmployeeExpenseClaimItems");
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Amount\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApproveResult\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OriginalAmount\">{5}</SetVar>" +
               "</Method>";

            // Build the CAML update commands.
            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, dr["ID"].ToString(),
                    dr["ApprovedAmount"].ToString(),
                    dr["IsSpecialApprove"].ToString(),
                     dr["OriginalAmount"].ToString()
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

        internal static void BatchUpdateApprovedItems(DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("EmployeeExpenseClaimItems");
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SpecialApproveResult\">{3}</SetVar>" +
               "</Method>";

            // Build the CAML update commands.
            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, dr["ID"].ToString(),
                    dr["IsSpecialApprove"].ToString()
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

        internal static SPListItem GetClaimStdByLevel(int jobLevel, string type)
        {
            var qLowLevel = new QueryField("LowLevel", false);
            var qHighLevel = new QueryField("HighLevel", false);
            var qType = new QueryField("ExpenseType", false);
            CamlExpression exp = null;
            if (type.Equals("Mobile phone", System.StringComparison.CurrentCultureIgnoreCase))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qLowLevel.LessEqual(jobLevel));
                exp = WorkFlowUtil.LinkAnd(exp, qHighLevel.MoreEqual(jobLevel));
            }
            exp = WorkFlowUtil.LinkAnd(exp, qType.MoreEqual(type));

            SPListItemCollection lc = ListQuery.Select()
                    .From(WorkFlowUtil.GetWorkflowList("ExpenseClaimStd"))
                    .Where(exp)
                    .GetItems();

            return lc.Count > 0 ? lc[0] : null;
        }


        internal static string GetGLAccountByExpenseType(string key,DataTable data)
        {
            Hashtable ht = GetExpenseTypeAndGLAccountHashTable(data);
            string gLAccount = "";
            if (key != "")
            {
                gLAccount = ht[key].AsString();
            }
            return gLAccount;
        }

        internal static Hashtable GetExpenseTypeAndGLAccountHashTable(DataTable data)
        {
            Hashtable ht = new Hashtable();
            //ht.Add("Mobile", "15510402");
            //ht.Add("OT - meal allowance", "15511004");
            //ht.Add("OT - local transportation", "15511200");
            //ht.Add("Entertainment - food", "15510601");
            //ht.Add("Entertainment - gift", "15510602");
            //ht.Add("Expatriate benefit - rental", "15510204");
            //ht.Add("Expatriate benefit - car related", "15510204");
            //ht.Add("Government charges - visa application", "15512000");
            //ht.Add("Government charges - penalty charges", "15750200");
            //ht.Add("Travel - transportation", "15511200");
            //ht.Add("Travel - meal", "15511004");
            //ht.Add("Magazine/newspaper", "15511407");
            //ht.Add("Office stationery", "15511401");
            //ht.Add("Postage", "15510404");
            //ht.Add("Sample purchase", "15511406");
            //ht.Add("Others (specify)", "");
            
            //DataTable dt = WorkFlowUtil.GetCollectionByList("Expense Claim SAP GLAccount").GetDataTable();
            DataTable dt = data;
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["NewExpenseType"].ToString(), dr["GLAccount"].ToString());
            }
            return ht;
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

            query.Query = string.Format("<Where><Eq><FieldRef Name='EECWWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
        }


        #endregion

        internal static void AddItemTable(DataTable itemTable, DataTable cRTable, DataListEdit dataForm)
        {
            if (GetExpenseTypeResult(itemTable))
            {
                foreach (DataRow dr in cRTable.Rows)
                {
                    DataRow newdr = itemTable.Rows.Add();
                    newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                    newdr["CostCenter"] = dr["CostCenter"].ToString();
                    newdr["ItemAmount"] = dr["ItemAmount"].ToString();
                    newdr["GLAccount"] = dr["GLAccount"].ToString();
                    newdr["EmployeeID"] = dr["EmployeeID"].ToString();
                    newdr["EmployeeName"] = dr["EmployeeName"].ToString();
                    newdr["EECWWorkflowNumber"] = dr["EECWWorkflowNumber"].ToString();
                    newdr["CashAmount"] = dr["CashAmount"].ToString();
                }
                dataForm.ItemTable = itemTable;
            }
        }

        private static bool GetExpenseTypeResult(DataTable itemTable)
        {
            bool result = true;
            foreach (DataRow dr in itemTable.Rows)
            {
                if (dr["ExpenseType"].ToString() == "OR - employee vendor" || dr["ExpenseType"].ToString() == "OR - cash advance")
                {
                    result = false;
                }
            }
            return result;
        }

        #endregion

        #region ExpenseType

        public static DataTable GetExpenseType(string requestId, string dataFormMode)
        {
            DataTable tableNew = new DataTable();
            tableNew.Columns.Add("ExpenseType");

            DataTable tableOld = new DataTable();
            tableOld.Columns.Add("ExpenseType");

            StringBuilder expenseType = new StringBuilder();

            DataTable dataNew = WorkFlowUtil.GetCollectionByList("Employee Expense Claim Type").GetDataTable();
            foreach (DataRow dr in dataNew.Rows)
            {
                DataRow rowNew = tableNew.Rows.Add();
                rowNew["ExpenseType"] = dr["NewExpenseType"].ToString();

                DataRow rowOld = tableOld.Rows.Add();
                rowOld["ExpenseType"] = dr["ExpenseType"].ToString();

                if (dr["ModifyStatus"].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    expenseType.Append(dr["ExpenseType"].ToString() + ";");
                }
            }

            if (dataFormMode.Equals("New", StringComparison.CurrentCultureIgnoreCase))
            {
                return tableNew;
            }
            else
            {
                DataTable dtEdit = GetDataCollection(requestId, "EmployeeExpenseClaimItems").GetDataTable();
                if (null != dtEdit)
                {
                    if (dtEdit.Rows.Count > 0)
                    {
                        bool exists = false;
                        foreach (DataRow row in dtEdit.Rows)
                        {
                            if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (exists)
                        {
                            return tableOld;
                        }
                        else
                        {
                            return tableNew;
                        }
                    }
                }
                CommonUtil.logError(string.Format("Employee Expense Claim Type：{0} :: NULL", requestId));
                return null;
            }
        }

        public static DataSet GetExpenseTypeAndGLAccount(string requestId)
        {
            DataSet ds = new DataSet();

            DataTable tableNewET = new DataTable();
            tableNewET.Columns.Add("ExpenseType");

            DataTable tableOldET = new DataTable();
            tableOldET.Columns.Add("ExpenseType");

            DataTable tableNewGL = new DataTable();
            tableNewGL.Columns.Add("ExpenseType");
            tableNewGL.Columns.Add("GLAccount");

            DataTable tableOldGL = new DataTable();
            tableOldGL.Columns.Add("ExpenseType");
            tableOldGL.Columns.Add("GLAccount");

            StringBuilder expenseType = new StringBuilder();

            DataTable dataNew = WorkFlowUtil.GetCollectionByList("Expense Claim SAP GLAccount").GetDataTable();
            foreach (DataRow dr in dataNew.Rows)
            {
                DataRow rowNewET = tableNewET.Rows.Add();
                rowNewET["ExpenseType"] = dr["NewExpenseType"].ToString();

                DataRow rowOldET = tableOldET.Rows.Add();
                rowOldET["ExpenseType"] = dr["ExpenseType"].ToString();

                DataRow rowNewGL = tableNewGL.Rows.Add();
                rowNewGL["ExpenseType"] = dr["NewExpenseType"].ToString();
                rowNewGL["GLAccount"] = dr["GLAccount"].ToString();

                DataRow rowOldGL = tableOldGL.Rows.Add();
                rowOldGL["ExpenseType"] = dr["ExpenseType"].ToString();
                rowOldGL["GLAccount"] = dr["GLAccount"].ToString();

                if (dr["ModifyStatus"].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    expenseType.Append(dr["ExpenseType"].ToString() + ";");
                }
            }

            DataTable dtEdit = GetDataCollection(requestId, "EmployeeExpenseClaimSAPItemsWorkFlow").GetDataTable();
            if (null != dtEdit)
            {
                if (dtEdit.Rows.Count > 0)
                {
                    bool exists = false;
                    foreach (DataRow row in dtEdit.Rows)
                    {
                        if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (exists)
                    {
                        ds.Tables.Add(tableOldET);
                        ds.Tables.Add(tableOldGL);
                    }
                    else
                    {
                        ds.Tables.Add(tableNewET);
                        ds.Tables.Add(tableNewGL);
                    }
                }
            }

            return ds;
        }

        #endregion

    }

}