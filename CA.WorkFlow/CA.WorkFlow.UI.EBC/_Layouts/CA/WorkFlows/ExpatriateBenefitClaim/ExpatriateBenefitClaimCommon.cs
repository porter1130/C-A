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

namespace CA.WorkFlow.UI.EBC
{
    public class ExpatriateBenefitClaimCommon
    {
        #region

        internal static void SaveDetails(string expatriateBenefitForm, string workflowNumber)
        {
            BatchInsertItems("ExpatriateBenefitClaimItems", workflowNumber, expatriateBenefitForm);
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Remark\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpensePurpose\">{9}</SetVar>" +
               "</Method>";

            string regexText = @"\{[^\{-\}]*\}";
            Regex regex = new Regex(regexText);
            MatchCollection mc = regex.Matches(expatriateBenefitForm);
            foreach (Match m in mc)
            {
                List<string> itemList = m.Value.Replace("{", "").Replace("}", "").Split(',').ToList<string>();
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                            workflowNumber,
                                            itemList[0].Replace("BenefitType:'", "").Replace("'", "").Trim(),
                                            itemList[1].Replace("Date:'", "").Replace("'", "").Trim(),
                                            itemList[3].Replace("CostCenter:'", "").Replace("'", "").Trim(),
                                            itemList[4].Replace("Amount:'", "").Replace("'", "").Trim(),
                                            itemList[5].Replace("Remark:'", "").Replace("'", "").Trim(),
                                            itemList[2].Replace("ExpensePurpose:'", "").Replace("'", "").Trim());
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

        internal static void DeleteAllDraftItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("ExpatriateBenefitClaimItems", workflowNumber);
        }

        internal static NameCollection GetTaskUsers(NameCollection manager, string managerAccount)
        {
            manager.Add(managerAccount);
            var deleman = WorkFlowUtil.GetDeleman(managerAccount, WorkFlowUtil.GetModuleIdByListName("Expatriate Benefit Claim Workflow"));
            if (deleman != null)
            {
                manager.Add(deleman);
            }
            return manager;
        }

        internal static NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, WorkFlowUtil.GetModuleIdByListName("Expatriate Benefit Claim Workflow"));
        }

        #endregion

        #region SAP

        internal static void AddItemTable(DataListEdit dataForm)
        {
            AddItemTable(dataForm.ItemTable, dataForm.CRTable, dataForm);
        }

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
                    newdr["EBCWorkflowNumber"] = dr["EBCWorkflowNumber"].ToString();
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

        internal static void DeleteAllDraftSAPItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("ExpatriateBenefitClaimSAPItems", workflowNumber);
        }

        internal static void SaveSAPItemsDetails(DataListEdit dataForm, string workflowNumber)
        {
            BatchInsertSAPItems("ExpatriateBenefitClaimSAPItems", workflowNumber, dataForm.ItemTable);
        }

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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExpenseType\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#GLAccount\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemAmount\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Status\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeID\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EmployeeName\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#EBCWorkflowNumber\">{11}</SetVar>" +
               "</Method>";

            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                    dr["CostCenter"].ToString(),
                                    dr["ExpenseType"].ToString(),
                                    dr["GLAccount"].ToString(),
                                    dr["ItemAmount"].ToString(),
                                    "0",
                                    workflowNumber,
                                    dr["EmployeeID"].ToString(),
                                    dr["EmployeeName"].ToString(),
                                    dr["EBCWorkflowNumber"].ToString()
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
            Hashtable ht = new Hashtable();
            DataTable dt = WorkFlowUtil.GetCollectionByList("Expatriate BenefitType GLAccount").GetDataTable();
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["ExpenseType"].ToString(), dr["GLAccount"].ToString());
            }
            return ht;
        }

        public static string GetSpecialEmployeeForEBC()
        {
            StringBuilder list = new StringBuilder();
            DataTable dt = WorkFlowUtil.GetCollectionByList("SpecialEmployeeForEBC").GetDataTable();
            if (null != dt)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Append(dr["EmployeeID"].ToString() + ",");
                }
            }
            return list.ToString();
        }

        internal static DataTable GetCostCenterDT()
        {
            return WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
        }

        internal static DataTable GetDataTableToSAP(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "ExpatriateBenefitClaimSAPItems").GetDataTable();
        }

        internal static void BatchUpdateSAPItems(DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("ExpatriateBenefitClaimSAPItems");
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

        internal static SPListItemCollection GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber(string listName, string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList(listName);
            SPQuery query = new SPQuery();

            query.Query = string.Format("<Where><Eq><FieldRef Name='EBCWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
            return listItems;
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

        #endregion

    }

    public class ExpatriateBenefitClaimConstants 
    {
        public static string wf_EBC_HRDirector = "wf_EBC_HRDirector";
        public static string wf_EBC_FinanceConfirm = "wf_EBC_FinanceConfirm";

        public static string wf_EBC_Accountants = "wf_EBC_Accountants";
        public static string wf_EBC_FinanceManager = "wf_EBC_FinanceManager";

        public static string ExpatriateBenefitClaimSAP = "125";
    
    }
    
}