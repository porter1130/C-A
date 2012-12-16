namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System.Data;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using CodeArt.SharePoint.CamlQuery;
    using System;
    using QuickFlow;
    using System.Collections;
    using QuickFlow.Core;
    using Microsoft.SharePoint.Workflow;
    using CA.SharePoint;
    using System.Configuration;
    using System.Web;
    using System.Text.RegularExpressions;

    class PurchaseRequestCommon
    {
        /// <summary>
        /// Itemcode缓存的Key
        /// </summary>
        internal static string sItemCacheKey = "ItemCode";
        internal static TimeSpan tsCache = TimeSpan.FromMinutes(PurchaseRequestCommon.GetPRCacheMinutes());
        /// <summary>
        /// Vendor缓存的Key
        /// </summary>
        internal static string sVendorCacheKey = "Vendor";

        internal static void SaveDetails(DataEdit dataForm, string workflowNumber, string capexType, string requestType, bool isHO, string itemIdentity)
        {
            BatchInsertItems("PurchaseRequestItems", workflowNumber, dataForm.ItemTable, capexType, requestType, isHO, itemIdentity);
        }
        internal static void SaveDetails(DataTable dtPRData, string workflowNumber, string capexType, string requestType, bool isHO, string itemIdentity)
        {
            BatchInsertItems("PurchaseRequestItems", workflowNumber, dtPRData, capexType, requestType, isHO, itemIdentity);
        }

        internal static string SavePOItemDetails(DataTable dataTable, string workflowNumber)
        {
            System.Data.DataView dv = dataTable.DefaultView;
            dv.Sort = "ItemCode Asc";
            DataTable dtPO = dv.ToTable();
            return BatchInsertItemsForPOItems("PurchaseOrderItems", workflowNumber, dtPO);
        }

        internal static string GetDeleman(string account)
        {
            return WorkFlowUtil.GetDeleman(account, WorkFlowUtil.GetModuleIdByListName("PurchaseRequestWorkflow"));
        }

        internal static NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, WorkFlowUtil.GetModuleIdByListName("PurchaseRequestWorkflow"));
        }

        internal static string GetDelemanForPO(string account)
        {
            return WorkFlowUtil.GetDeleman(account, WorkFlowUtil.GetModuleIdByListName("PurchaseOrderWorkflow"));
        }

        internal static void DeleteAllDraftItems(string workflowNumber)
        {
            WorkFlowUtil.BatchDeleteItems("PurchaseRequestItems", workflowNumber);
        }

        //Return data table from "PurchaseRequestItems" list according by given requestId.
        internal static DataTable GetDataTable(string requestId)
        {
            return WorkFlowUtil.GetCollection(requestId, "PurchaseRequestItems").GetDataTable();
        }

        internal static DataTable GetPRByTitle(string workflowNumber)
        {
            return WorkFlowUtil.GetCollection(workflowNumber, "Purchase Request Workflow").GetDataTable();
        }

        /**
         * 为HO查询所有有效的cost centers
         */
        internal static DataTable GetAllCostCenterDT()
        {
            CamlExpression exp = null;
            var qIsActive = new QueryField("IsActive", false);
            exp = WorkFlowUtil.LinkAnd(exp, qIsActive.Equal(true));

            SPListItemCollection lc = null;
           
            lc = ListQuery.Select()
                    .From(SPContext.Current.Web.Lists["Cost Centers"])
                    .Where(exp)
                    .GetItems();
           
            return lc.GetDataTable();
        }

        internal static DataTable GetVendorDT()
        {
            return WorkFlowUtil.GetCollectionByList("Vendors").GetDataTable();
        }

        internal static Hashtable CreatePOByReqestIds(params string[] requestIds)
        {
            var itemTable = GetPRItemsByRequestId(requestIds);
            return itemTable.Rows.Count > 0 ? CreatePOFromDT(itemTable) : null;
        }

        private static Hashtable CreatePOFromDT(DataTable dt)
        {
            SPListItem listItem = null;
            string prePoNumber = null;
            string workflowNumber = null;
            double tmpItemTotal = 0;
            DataRow standRow = null;
            string preItemCode = null;
            string tmpRequestId = null;
            //string tmpItemCode = null;
            string vendorId = null;

            List<SPListItem> items = new List<SPListItem>();
            List<string> poNumbers = new List<string>();
            Hashtable hashPOs = new Hashtable();
            SPList list = WorkFlowUtil.GetWorkflowList("Purchase Order Workflow");
            List<DataTable> orders = CreatePurchaseOrders(dt, "ItemIdentity", "VendorID");//根据记录识别类型和vendID进行联合分组，每组为一个订单
            DataTable vendors = GetVendorDT();

            List<string> costCenters = new List<string>();
            var itemIdentity = string.Empty;
            string orderNumFir = null;//订单编号第一部分，为SHHO,SHSO或者CostCenter
            string orderNumSec = null;//订单编号第二部分，为ItemCode前四位，但是首字母按业务逻辑而定
            foreach (var order in orders)//处理不同订单
            {
                orderNumFir = string.Empty;
                orderNumSec = string.Empty;
                bool isService = true;//所有ITEM均为X开头的Service类型
                bool isSingleStore = true;
                string lastCC = string.Empty;
                string currCC = string.Empty;
                int count = 0;

                #region Check the items in order
                //For PO# rules
                //If the items of PO are all came from only one store, the No. should be like S001C0170001.
                //If the items of PO are came from different stores, the No. should be like SHSOC0170001.
                //If the items of PO are requested for another department, the No. should be like SHHOC0170001.
                //If the items of PO only contain service item(X0010001), the No. should be like SHHOX0010005, SHSOX001005, S001X0010005
                standRow = null;
                foreach (DataRow orderItem in order.Rows)
                {
                    if (isService && !orderItem["ItemCode"].ToString().StartsWith("X"))
                    {
                        isService = false;
                        standRow = orderItem;
                    }

                    currCC = orderItem["CostCenter"].ToString();
                    if (!lastCC.Equals(currCC))
                    {
                        count++;
                        lastCC = currCC;
                    }
                }
                if (count > 1)//若count大于1，表示该订单中包含多个门店
                {
                    isSingleStore = false;
                }
                if (standRow == null)//若standRow此时为空，表示订单中所有ITEM均为X开头的
                {
                    standRow = order.Rows[0];
                }
                #endregion

                itemIdentity = standRow["ItemIdentity"].ToString();
                bool isReturn = itemIdentity.EndsWith("R");//ItemIdentity列结尾为R表示退货，N表示非退货。参见DataEdit.ascx.cs中GetItemIdentity方法
                preItemCode = standRow["ItemCode"].ToString();
                //capexType = standRow["CapexType"].ToString();
                //requestType = standRow["RequestType"].ToString();
                vendorId = standRow["VendorID"].ToString();

                #region Design the OrderNum Second Part
                if (standRow["RequestType"].ToString().Equals("Capex", StringComparison.CurrentCultureIgnoreCase))
                {//如果为Capex类型，则不同的Capex类型有不同的编号规则
                    switch (standRow["CapexType"].ToString())
                    {
                        case "Refurbishment":
                            preItemCode = "R" + preItemCode.Substring(1, 3);
                            break;
                        case "Maintenance":
                            preItemCode = "M" + preItemCode.Substring(1, 3);
                            break;
                        default:
                            preItemCode = "C" + preItemCode.Substring(1, 3);
                            break;
                    }
                }
                else
                {
                    preItemCode = "E" + preItemCode.Substring(1, 3);
                }

                if (isService)//如果订单中只包含X项目，则订单编号包含X
                {
                    preItemCode = "X" + preItemCode.Substring(1, 3);
                }
                orderNumSec = preItemCode;
                #endregion

                #region Design the OrderNum First Part
                if (itemIdentity.Contains("COP"))
                {
                    orderNumFir = "SHHO";
                }
                else if (itemIdentity.Contains("DTP"))
                {
                    orderNumFir = "SHHO";
                }
                else if (itemIdentity.StartsWith("PB"))
                {
                    orderNumFir = "SHHO";
                }
                else
                {
                    orderNumFir = isSingleStore ? standRow["CostCenter"].ToString() : "SHSO";
                }
                #endregion
                prePoNumber = orderNumFir + orderNumSec;//PO编号前缀
                workflowNumber = CreatePONumber(prePoNumber);
                workflowNumber = isReturn ? workflowNumber + "R" : workflowNumber;//退货订单在编号后加R
                poNumbers.Add(workflowNumber);//依次记录生成的PO Number，稍后需要将生成的PO Number更新到PR Item列表中

                //检查订单中每条记录，将PR item对应的PR和PO number记录下来，稍后更新到PR主表。同时计算订单总价。
                double goodsNetTotal = 0;
                double instNetTotal = 0;
                //double removeTotal = 0;
                double transNetTotal = 0;
                double packNetTotal = 0;
                double discNetTotal = 0;
                double taxTotal = 0;
                double tmpTaxValue = 0;
                double tmpNetItemTotal = 0;
                //double goodsTaxTotal = 0;
                foreach (DataRow dr in order.Rows)
                {
                    tmpRequestId = dr["Title"].ToString();
                    if (hashPOs.ContainsKey(tmpRequestId))
                    {
                        if (!hashPOs[tmpRequestId].ToString().Contains(workflowNumber + ";"))
                        {
                            hashPOs[tmpRequestId] = hashPOs[tmpRequestId] + workflowNumber + ";";
                        }
                    }
                    else
                    {
                        hashPOs.Add(tmpRequestId, workflowNumber + ";");
                    }

                    #region Calc Total
                    //计算总价
                    string currItemCode = dr["ItemCode"].ToString().ToLower();
                    string currDesc = dr["Description"].ToString().ToLower();
                    tmpItemTotal = Math.Round(Convert.ToDouble(dr["UnitPrice"]) * Convert.ToDouble(dr["RequestQuantity"]), 2);
                    tmpTaxValue = Convert.ToDouble(dr["TaxValue"]);
                    tmpNetItemTotal = tmpItemTotal - tmpTaxValue;
                    if (currItemCode.StartsWith("x") && currDesc.Contains(WorkflowItemCode.INSTALLATION))
                    {
                        instNetTotal += tmpNetItemTotal;
                        taxTotal += tmpTaxValue;
                    }
                    else if (currItemCode.StartsWith("x") && currDesc.Contains(WorkflowItemCode.TRANSPORTATION))
                    {
                        transNetTotal += tmpNetItemTotal;
                        taxTotal += tmpTaxValue;
                    }
                    else if (currItemCode.StartsWith("x") && currDesc.Contains(WorkflowItemCode.PACKAGING))
                    {
                        packNetTotal += tmpNetItemTotal;
                        taxTotal += tmpTaxValue;
                    }
                    else if (currItemCode.StartsWith("x") && currDesc.Contains(WorkflowItemCode.DISCOUNT))
                    {
                        discNetTotal += tmpNetItemTotal;
                        taxTotal += isReturn ? tmpTaxValue : -tmpTaxValue;
                    }
                    else
                    {
                        goodsNetTotal += tmpNetItemTotal;
                        taxTotal += isReturn ? -tmpTaxValue : tmpTaxValue;
                    }
                    #endregion
                }
                
                listItem = list.Items.Add();//添加PO记录
                listItem["WorkflowNumber"] = workflowNumber;
                listItem["PONumber"] = workflowNumber;

                DataRow[] rows = vendors.Select("VendorId='" + vendorId + "'");
                if (rows.Length > 0)
                {
                    var vendor = rows[0];
                    listItem["Vendor"] = vendor["Title"].AsString(); //title是vendor name
                    listItem["VendorAddress"] = vendor["Address"].AsString();
                    listItem["VendorCode"] = vendor["PostCode"].AsString();
                    listItem["VendorCity"] = string.Empty;
                    listItem["VendorPhone"] = vendor["Phone"].AsString();
                    listItem["VendorFax"] = vendor["Fax"].AsString();
                    listItem["VendorMail"] = vendor["Email"].AsString();
                    listItem["VendorContact"] = vendor["ContactPerson"].AsString();
                    listItem["VendorNo"] = vendorId;
                    listItem["VendorRegNum"] = string.Empty;
                }

                var currentEmp = UserProfileUtil.GetEmployeeEx(SPContext.Current.Web.CurrentUser.LoginName);
                listItem["Applicant"] = string.Format("{0}({1})", currentEmp.DisplayName, currentEmp.UserAccount);
                listItem["Buyer"] = currentEmp.DisplayName;
                listItem["Department"] = currentEmp.Department;
                listItem["Email"] = currentEmp.WorkEmail;
                listItem["Phone"] = currentEmp.Phone;
                listItem["Fax"] = currentEmp.Fax;
                listItem["RegNum"] = string.Empty;
                listItem["IssuedDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                listItem["TaxValue"] = taxTotal;//税额显示为正值,退货的显示为负值。
                listItem["SiteInstallFee"] = instNetTotal;
                listItem["FreightCost"] = transNetTotal;
                listItem["PackageCharge"] = packNetTotal;
                listItem["Discount"] = discNetTotal;
                listItem["IsSkipChop"] = standRow["ItemIdentity"].ToString().IndexOf("PB") == 0;//如果是PaperBag的不需要PO在线盖章
                
                double grandTotal = isReturn ? (-goodsNetTotal) + instNetTotal + packNetTotal + transNetTotal + discNetTotal + taxTotal :
                                                goodsNetTotal + instNetTotal + packNetTotal + transNetTotal - discNetTotal + taxTotal;
                listItem["GrandTotal"] = grandTotal;
                listItem["Total"] = isReturn ? -goodsNetTotal: goodsNetTotal;
                listItem["IsReturn"] = isReturn;
                listItem["Status"] = CAWorkflowStatus.Pending;

                listItem.Web.AllowUnsafeUpdates = true;
                listItem.Update();

                SavePOItemDetails(order, workflowNumber);//批量生成PO

                items.Add(listItem);
            }

            UpdatePRItems(orders, poNumbers);//更新PO Number到PR Item列表中

            StartPOWorkflow(list, items); //启动PO工作流

            return hashPOs;
        }

        private static void StartPOWorkflow(SPList list, List<SPListItem> items)
        {
            if (items.Count == 0)
            {
                return;
            }

            SPSite site = SPContext.Current.Site;
            var wfName = ConfigurationManager.AppSettings["POWFName"];
            if (wfName.IsNullOrWhitespace())
            {
                CommonUtil.logError("PO Workflow name is set in web.config. Please check it first whether key \"POWFName\" is set.");
                return;
            }
            var currUser = SPContext.Current.Web.CurrentUser.LoginName;
            NameCollection checkManager = new NameCollection();

            checkManager.Add(SPContext.Current.Web.CurrentUser.LoginName);
            var deleman = PurchaseRequestCommon.GetDelemanForPO(currUser); //查找代理人
            if (deleman != null)
            {
                checkManager.Add(deleman);
            }

            //启动工作流
            foreach (var item in items)
            {
                WorkflowVariableValues vs = new WorkflowVariableValues();
                vs["CompleteTaskFormURL"] = "/_Layouts/CA/WorkFlows/PurchaseOrder/EditForm.aspx";
                vs["CompleteTaskTitle"] = item["WorkflowNumber"].AsString() + ": Please complete the PO";
                vs["CompleteTaskUsers"] = checkManager;
                vs["IsSkip"] = item["IsSkipChop"];//纸袋不用盖章

                var eventData = SerializeUtil.Serialize(vs);
                
                var wfAss = list.WorkflowAssociations.GetAssociationByName(wfName, System.Globalization.CultureInfo.CurrentCulture);
                SPWorkflow wf = site.WorkflowManager.StartWorkflow(item, wfAss, eventData);
                WorkFlowUtil.UpdateWorkflowPath(item, eventData);
            }
        }

        //按给定的列进行分组
        private static List<DataTable> CreatePurchaseOrders(DataTable dt, string itemIdentityCol, string vendorIdCol)
        {
            List<DataTable> orders = new List<DataTable>();
            var result = from p in dt.AsEnumerable() group p by new { ItemIdentity = p[itemIdentityCol], VendorId = p[vendorIdCol] } into pp select pp;
            foreach (var ig in result)
            {
                var order = dt.Clone();
                foreach (var dr in ig)
                {
                    order.ImportRow(dr);
                }
                orders.Add(order);
            }

            return orders;
        }

        //Batch Insert
        private static void BatchInsertItems(string listName, string workflowNumber, DataTable dt, string capexType, string requestType, bool isHO, string itemIdentity)
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemNo\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemCode\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Description\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#RequestQuantity\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Unit\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VendorID\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#UnitPrice\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalPrice\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenterID\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#AssetClass\">{14}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#DeliveryPeriod\">{15}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TaxRate\">{16}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CapexType\">{17}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#RequestType\">{18}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VendorName\">{19}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TransQuantity\">{20}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalQuantity\">{21}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TaxValue\">{22}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Currency\">{23}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ExchangeRate\">{24}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#IsHO\">{25}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemIdentity\">{26}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenterName\">{27}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemType\">{28}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PackagedRegulation\">{29}</SetVar>" +
               "</Method>";
            // Build the CAML update commands.
            foreach (DataRow dr in dt.Rows)
            {
                var tmpTotal = Convert.ToDouble(dr["RequestQuantity"]) * Convert.ToDouble(dr["UnitPrice"]);
                var taxValue = Math.Round(tmpTotal - (tmpTotal/(Convert.ToDouble(dr["TaxRate"]) + 1)), 2);
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                    workflowNumber,
                    dr["Item"],
                    dr["ItemCode"],
                    dr["Description"],
                    dr["RequestQuantity"],
                    dr["Unit"],
                    dr["VendorID"],
                    dr["UnitPrice"],
                    dr["TotalPrice"],
                    dr["CostCenterID"],
                    dr["CostCenter"],
                    dr["AssetClass"],
                    dr["DeliveryPeriod"].AsString().IsNullOrWhitespace() ? 0 : dr["DeliveryPeriod"],
                    dr["TaxRate"],
                    capexType,
                    requestType,
                    dr["VendorName"],
                    dr["TransQuantity"],
                    dr["TotalQuantity"],
                    taxValue,
                    dr["Currency"],
                    dr["ExchangeRate"],
                    isHO,
                    itemIdentity,
                    dr["CostCenterName"],
                    dr["ItemType"],
                    dr["PackagedRegulation"]
                    );
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                var web = SPContext.Current.Web;
                web.AllowUnsafeUpdates = true;
                string batchReturn = web.ProcessBatchData(batch);
            }
        }

        //Batch Insert
        private static string BatchInsertItemsForPOItems(string listName, string workflowNumber, DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchReturn = null;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat;
            methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemCode\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#DeliveryPeriod\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Description\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Unit\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#RequestQuantity\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#UnitPrice\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalPrice\">{11}</SetVar>" +          
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TaxRate\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PRNumber\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PRItemID\">{14}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TransQuantity\">{15}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalQuantity\">{16}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TaxValue\">{17}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Currency\">{18}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#AssetClass\">{19}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VendorID\">{20}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VendorName\">{21}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PackagedRegulation\">{22}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemNO\">{23}</SetVar>" + 
               "</Method>";

            bool isReturn = dt.Rows.Count > 0 ? dt.Rows[0]["ItemIdentity"].ToString().EndsWith("R") : false;
            bool isAllowNegative = true;
            // Build the CAML update commands.
            int iItemNO = 1;
            foreach (DataRow dr in dt.Rows)
            {
                isAllowNegative = true;
                if (dr["ItemCode"].ToString().StartsWith("X"))
                {
                    isAllowNegative = false;
                }
                double requestQuantity = isReturn && isAllowNegative ? -Convert.ToDouble(dr["RequestQuantity"]) : Convert.ToDouble(dr["RequestQuantity"]);
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                    workflowNumber,
                    dr["ItemCode"],
                    dr["DeliveryPeriod"],
                    dr["CostCenter"],
                    dr["Description"],
                    dr["Unit"],
                    requestQuantity,
                    dr["UnitPrice"],
                    Math.Round(Convert.ToDouble(dr["UnitPrice"]) * requestQuantity, 2),
                    dr["TaxRate"],
                    dr["Title"],
                    dr["ID"],
                    isReturn && isAllowNegative ? -Convert.ToDouble(dr["TransQuantity"]) : dr["TransQuantity"],
                    isReturn && isAllowNegative ? -Convert.ToDouble(dr["TotalQuantity"]) : dr["TotalQuantity"],
                    isReturn && isAllowNegative ? -Convert.ToDouble(dr["TaxValue"]) : dr["TaxValue"],
                    dr["Currency"],
                    dr["AssetClass"],
                    dr["VendorID"],
                    dr["VendorName"],
                    dr["PackagedRegulation"],
                    iItemNO
                    );
                iItemNO++;
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                var web = SPContext.Current.Web;
                web.AllowUnsafeUpdates = true;
                batchReturn = web.ProcessBatchData(batch);
            }
            return batchReturn;
        }

        //Batch Update
        private static void UpdatePRItems(List<DataTable> orders, List<string> poNumbers)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("PurchaseRequestItems");
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#IsPOCreated\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PONumber\">{4}</SetVar>" +
               "</Method>";

            int index = 0;
            var poNumber = string.Empty;
            foreach (var order in orders)
            {
                poNumber = poNumbers[index++];
                foreach (DataRow dr in order.Rows)
                {
                    methodBuilder.AppendFormat(methodFormat, 0, listGuid, dr["ID"].ToString(),
                        1,
                        poNumber
                        );
                }
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
            }
        }

        //Batch Update
        internal static void UpdatePRTable(string[] ids, string[] requestIds, Hashtable hashPOs)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("Purchase Request Workflow");
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#POStatus\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PONumber\">{4}</SetVar>" +
               "</Method>";

            int index = 0;
            var poNumber = string.Empty;
            foreach (var id in ids)
            {
                poNumber = hashPOs[requestIds[index++]].AsString();
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, id,
                        "Created",
                        poNumber
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

        private static string CreatePONumber(string snSample)
        {
            return snSample + WorkFlowUtil.CreateWorkFlowNumber(snSample).ToString("0000");
        }

        internal static DataTable GetPRTableByPOStatus(string poStatus,string sAccount)
        {
            CamlExpression exp = null;

            var qPOStatus = new QueryField("POStatus", false);
            exp = WorkFlowUtil.LinkAnd(exp, qPOStatus.Equal(poStatus));

            var qAccount = new QueryField("CheckPerson", false);
            exp = WorkFlowUtil.LinkAnd(exp, qAccount.Equal(sAccount));

            SPListItemCollection lc = ListQuery.Select()
                    .From(WorkFlowUtil.GetWorkflowList("Purchase Request Workflow"))
                    .Where(exp)
                    .GetItems();

            return lc.GetDataTable();
        }

        private static DataTable GetPRItemsByRequestId(params string[] requestIds)
        {
            var qRequestId = new QueryField("Title", false);
            CamlExpression exp = null;
            foreach (var tmp in requestIds)
            {
                exp = WorkFlowUtil.LinkOr(exp, qRequestId.Equal(tmp));
            }
            
            SPListItemCollection lc = ListQuery.Select()
                    .From(WorkFlowUtil.GetWorkflowList("PurchaseRequestItems"))
                    .Where(exp)
                    .GetItems();

            return lc.GetDataTable();
        }

        internal static DataTable GetItemCodeByTitle(params string[] titles)
        {
            //var qTitle = new QueryField("Title", false);
            //var qIsActive = new QueryField("IsActive", false);
            //CamlExpression exp = null;
            //foreach (var title in titles)
            //{
            //    exp = WorkFlowUtil.LinkOr(exp, qTitle.Equal(title));
            //}
            //exp = WorkFlowUtil.LinkAnd(exp, qIsActive.Equal(true));

            //SPListItemCollection lc = ListQuery.Select()
            //        .From(WorkFlowUtil.GetWorkflowList("Item Codes"))
            //        .Where(exp)
            //        .GetItems();

            //return lc.GetDataTable();

            return WorkflowListData.GetItemCodeDT();

            //StringBuilder filter = new StringBuilder("Title='");
            //foreach (var title in titles)
            //{
            //    filter.Append(title);
            //    filter.Append("' OR Title='");
            //}
            //filter.Remove(filter.Length - 11, 11);

            //DataTable dt = WorkflowListData.GetItemCodeDT();
            //DataRow[] drs = dt.Select(filter.ToString());
            //DataTable items = dt.Clone();
            //foreach (var dr in drs)
            //{
            //    items.ImportRow(dr);
            //}

            //return items;
        }

        internal static DataTable GetPRItemsById(params string[] ids)
        {
            var qId = new QueryField("ID", false);
            CamlExpression exp = null;
            foreach (var id in ids)
            {
                exp = WorkFlowUtil.LinkOr(exp, qId.Equal(id));
            }

            SPListItemCollection lc = ListQuery.Select()
                    .From(WorkFlowUtil.GetWorkflowList("PurchaseRequestItems"))
                    .Where(exp)
                    .GetItems();

            return lc.GetDataTable();
        }

        internal static DataTable GetPOItemsByRequestId(params string[] requestIds)
        {
            var qRequestId = new QueryField("Title", false);
            CamlExpression exp = null;
            foreach (var requestId in requestIds)
            {
                exp = WorkFlowUtil.LinkOr(exp, qRequestId.Equal(requestId));
            }


            SPListItemCollection lc = ListQuery.Select()
                    .From(WorkFlowUtil.GetWorkflowList("PurchaseOrderItems"))
                    .Where(exp)
                    .GetItems();

            return lc.GetDataTable();
        }

        internal static List<string> GetRecordId(string listName, string disName, string internalName, params string[] values)
        {
            var qColumn = new QueryField(internalName, false);
            CamlExpression exp = null;
            foreach (var value in values)
            {
                exp = WorkFlowUtil.LinkOr(exp, qColumn.Equal(value));
            }

            SPListItemCollection lc = ListQuery.Select()
                    .From(WorkFlowUtil.GetWorkflowList(listName))
                    .Where(exp)
                    .GetItems();

            List<string> ids = new List<string>();
            foreach (SPListItem item in lc)
            {
                ids.Add(item["ID"].ToString());
            }
            
            return ids;
        }

        /**
         * 根据itemcode各种属性进行查询
         * startWiths, partItemCode, partDesc, itemScope
         */
        internal static DataTable GetItemCodeForDS(string startWiths, string startItemCode, string partDesc, string itemScope, bool isFilter)
        {
            //C,X
            var split = new char[] { ',' };
            var starts = startWiths.Split(split);
            StringBuilder filter = new StringBuilder();
            
            DataTable dt = WorkflowListData.GetItemCodeDT();
            if (starts.Length == 0)
            {//出现无限制时直接返回空表
                return dt.Clone();
            }

            itemScope = string.IsNullOrEmpty(itemScope) ? string.Empty : itemScope;
            var drs = from ro in dt.AsEnumerable()
                                       where (string.IsNullOrEmpty(startItemCode) || ro.Field<string>("Title").StartsWith(startItemCode, StringComparison.CurrentCultureIgnoreCase))
                                       && (string.IsNullOrEmpty(partDesc) || ro.Field<string>("Description").Contains(partDesc))
                                       && (isFilter ? ro.Field<string>("ItemScope").AsString().Trim().Equals(itemScope) : true)
                                       select ro;

            DataTable items = dt.Clone();

            if (drs.Count<DataRow>() > 0)
            {
                var preDrs = drs.CopyToDataTable();

                if (starts.Length > 0)
                {
                    filter = new StringBuilder("Title LIKE '");
                    foreach (var start in starts)
                    {
                        filter.Append(start);
                        filter.Append("%' OR Title LIKE '");
                    }
                    filter.Remove(filter.Length - 16, 16);
                }

                DataRow[] finalDrs = preDrs.Select(filter.ToString());
                foreach (var dr in finalDrs)
                {
                    items.ImportRow(dr);
                }
            }

            return items;
        }

        internal static bool IsStore(string userAccount)
        {
            return IsInGroup(userAccount, "wf_Store");
        }

        internal static bool IsHO(string userAccount)
        {
            return IsInGroup(userAccount, "wf_HO");
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
        /**
         * Check whether the account is existed in the gourps
         */
        internal static bool IsInGroups(string account, params string[] groups)
        {
            bool isExist = false;
            foreach (var group in groups)
            {
                isExist = IsInGroup(account, group);
                if (isExist) break;
            }

            return isExist;
        }

        /**
         * Check whether the account is existed in the gourp
         */
        internal static bool IsInGroup(string account, string group)
        {
            bool isLegal = false;
            var users = UserProfileUtil.UserListInGroup(group);
            foreach (var user in users)
            {
                if (user.Equals(account, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    isLegal = true;
                    break;
                }
            }

            return isLegal;
        }

        /**
         * 为门店查找对应的CostCenters信息
         */
        internal static DataTable GetCostCenterForStore(string managerAccount)
        {
            var qManagerAccount = new QueryField("ManagerAccount", false);
            var qIsActive = new QueryField("IsActive", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qManagerAccount.Equal(managerAccount));
            exp = WorkFlowUtil.LinkAnd(exp, qIsActive.Equal(true));

            SPListItemCollection lc = null;
            
            lc = ListQuery.Select()
                    .From(SPContext.Current.Web.Lists["Cost Centers"])
                    .Where(exp)
                    .GetItems();
             
            return lc.GetDataTable();
        }

        /**
         * 找出PR中季度订单在一月中已完成数量
         * @param account 用户帐号
         * @param currentMonth 当前月
         */
        internal static int GetQuartOrderCountInMonth(string account, int currMonth)
        {
            int count = 0;
            var currYearStart = Convert.ToDateTime(string.Format("{0}-{1}-01", DateTime.Now.Year, currMonth));
            var currYearEnd = Convert.ToDateTime(string.Format("{0}-{1}-1", DateTime.Now.Year, currMonth + 1));


            SPQuery query = new SPQuery();
            var queryCondition = @"<Where>
                                      <And>
                                         <And>
                                            <Eq>
                                               <FieldRef Name='Author' />
                                               <Value Type='User'>{0}</Value>
                                            </Eq>
                                            <Geq>
                                               <FieldRef Name='Created' />
                                               <Value Type='DateTime'>{1}</Value>
                                            </Geq>
                                         </And>
                                         <Lt>
                                            <FieldRef Name='Created' />
                                            <Value Type='DateTime'>{2}</Value>
                                         </Lt>
                                      </And>
                                   </Where>";
            query.Query = string.Format(SPContext.Current.Web.CurrentUser.Name, queryCondition, currYearStart.ToString("yyyy-MM-dd"), currYearEnd.ToString("yyyy-MM-dd"));
            count = SPContext.Current.Web.Lists[SPContext.Current.List.Title].GetItems(query).Count;

            return count;
        }

        /**
         * 检查当Store申请季度订单时，是否超过一年只能申请4次的规定
         */
        internal static bool IsLegalStoreRequest(string storePurpose,DateTime dtPRDate)
        {
            bool isLegal = true;
            if (storePurpose.Equals("QuarterlyOrder", StringComparison.CurrentCultureIgnoreCase))
            {
               /// int currMonth = DateTime.Now.Month;
                string allowMonths = ConfigurationManager.AppSettings["AllowQuartOrderMonth"];
                string allowDeadline = ConfigurationManager.AppSettings["AllowQODeadLine"];
                char[] split = new char[] { ',' };
                string[] months = allowMonths.Split(split);
                bool isAllow = months.Contains(Convert.ToString(dtPRDate.Month));
                if (isAllow)
                {
                    //int day = DateTime.Now.Day;//每月25号HO开始统计季度订单
                    int deadline = string.IsNullOrEmpty(allowDeadline) ? 25 : Convert.ToInt32(allowDeadline);
                    isLegal = dtPRDate.Day < deadline ? true : false;
                }
                else
                {
                    isLegal = false;
                }
            }

            return isLegal;
        }
        /**
       * 检查当Store申请季度订单时，是否超过一年只能申请4次的规定
       */
        internal static bool IsLegalStoreRequest(string storePurpose, string reqAccount)
        {
            bool isLegal = true;
            if (storePurpose.Equals("QuarterlyOrder", StringComparison.CurrentCultureIgnoreCase))
            {
                int currMonth = DateTime.Now.Month;
                string allowMonths = ConfigurationManager.AppSettings["AllowQuartOrderMonth"];
                string allowDeadline = ConfigurationManager.AppSettings["AllowQODeadLine"];
                char[] split = new char[] { ',' };
                string[] months = allowMonths.Split(split);
                bool isAllow = months.Contains(Convert.ToString(currMonth));
                if (isAllow)
                {
                    //int preRequestCount = PurchaseRequestCommon.GetQuartOrderCountInMonth(reqAccount, currMonth);
                    //if (preRequestCount >= 1)
                    //{
                    //    isLegal = false;
                    //}
                    int day = DateTime.Now.Day;//每月25号HO开始统计季度订单
                    int deadline = string.IsNullOrEmpty(allowDeadline) ? 25 : Convert.ToInt32(allowDeadline);
                    isLegal = day < deadline ? true : false;
                }
                else
                {
                    isLegal = false;
                }
            }

            return isLegal;
        }
        //Batch Update PO Item table
        internal static void UpdatePOReceivedItems(List<string> itemIds)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList("PurchaseOrderItems");
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
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#IsPOCreated\">{3}</SetVar>" +
               "</Method>";

            foreach (var id in itemIds)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, id, 1);
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                WorkFlowUtil.BatchExecute(batch);
            }
        }

        //Batch Update PO main table
        internal static void UpdateReceivedPO(List<string> paidPOs)
        {
            if (paidPOs == null || paidPOs.Count == 0)
            {
                return;
            }

            string listGUID = string.Empty;
            var qRequestId = new QueryField("Title", false);
            CamlExpression exp = null;
            foreach (var poNumber in paidPOs)
            {
                exp = WorkFlowUtil.LinkOr(exp, qRequestId.Equal(poNumber));
            }
            SPListItemCollection lc = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList poList = web.Lists["Purchase Order Workflow"];
                        listGUID = poList.ID.ToString();

                        lc = ListQuery.Select()
                                .From(poList)
                                .Where(exp)
                                .GetItems();
                    }
                }
            });

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat;
            methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Action\">{3}</SetVar>" +
               "</Method>";

            foreach (SPListItem item in lc)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGUID, item["ID"],
                    string.Format("{0}WorkFlowCenter/lists/PaymentRequestWorkflow/NewForm.aspx?WorkflowNumber={1}, Paid", WebURL.RootURL, item["WorkflowNumber"]));
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                WorkFlowUtil.BatchExecute(batch);
            }
        }

        /**
         * Query items by requestId. If there is still have item not received, the result should be contain the requestId.
         */
        internal static DataTable GetNotRecPOItemsByReqId(params string[] requestIds)
        {
            var qRequestId = new QueryField("Title", false);
            var qIsReceived = new QueryField("IsReceived", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qIsReceived.Equal(false));

            CamlExpression exp2 = null;
            foreach (var requestId in requestIds)
            {
                exp2 = WorkFlowUtil.LinkOr(exp2, qRequestId.Equal(requestId));
            }

            exp = WorkFlowUtil.LinkAnd(exp, exp2);

            SPListItemCollection lc = null;
            lc = ListQuery.Select()
                    .From(SPContext.Current.Web.Lists["PurchaseOrderItems"])
                    .Where(exp)
                    .GetItems();
            return lc.GetDataTable();
        }

        /// <summary>
        ///  prListName里的prNumber是否是currentAccount所建
        /// </summary>
        /// <param name="sPRListName"></param>
        /// <param name="sPRNumber"></param>
        /// <param name="sCurrentAccount"></param>
        /// <returns></returns>
        internal static bool IsItemCreateByUser(string sPRListName, string sPRNumber, string sCurrentAccount)
        {
            bool isAuthorCreated = true;
            
            SPList list = SPContext.Current.Web.Lists[sPRListName];
            SPQuery query = new SPQuery();
            string sQueryCAML = @"<Where>
                                                            <And>
                                                                <Eq>
                                                                <FieldRef Name='Title' />
                                                                <Value Type='Text'>{0}</Value>
                                                                </Eq>
                                                                <Contains>
                                                                <FieldRef Name='Applicant' />
                                                                <Value Type='Text'>{1}</Value>
                                                                </Contains>
                                                            </And>
                                                        </Where>";
            query.Query = string.Format(sQueryCAML, sPRNumber, sCurrentAccount);
            SPListItemCollection sic= list.GetItems(query);
            if (null == sic || sic.Count == 0)
            {
                isAuthorCreated = false;
            }

            return isAuthorCreated;
        }

        /// <summary>
        ///  由PRNO,PONO得到数据
        /// </summary>
        /// <param name="sListName"></param>
        /// <param name="sPRColumn"></param>
        /// <param name="sPOColumn"></param>
        /// <param name="sPRVal"></param>
        /// <param name="sPOVal"></param>
        /// <returns></returns>
        internal static DataTable GetDataByPRPO(string sListName, string sPRColumn, string sPOColumn, string sPRVal, string sPOVal)
        {
            DataTable dt = new DataTable();

            SPList list = SPContext.Current.Web.Lists[sListName];
            SPQuery query = new SPQuery();
            string sQueryCAML = @"<Where>
                                    <And>
                                        <Eq>
                                            <FieldRef Name='{0}' />
                                            <Value Type='Text'>{1}</Value>
                                        </Eq>
                                        <Eq>
                                            <FieldRef Name='{2}' />
                                            <Value Type='Text'>{3}</Value>
                                        </Eq>
                                    </And>
                                </Where>";
            query.Query = string.Format(sQueryCAML, sPRColumn, sPRVal,sPOColumn,sPOVal);
            dt= list.GetItems(query).GetDataTable();

            return dt;
        }

        /// <summary>
        /// 得到退货单所对应的退货的PO号 
        /// </summary>
        /// <param name="sPO"></param>
        /// <returns></returns>
        internal static string GetReturnPO(string sPO)
        {
            string sReturnPO = string.Empty;

            SPList list = SPContext.Current.Web.Lists["Purchase Request Workflow"];
            SPQuery query = new SPQuery();
            string sQueryCAML = @"<Where>
                                    <And>
                                        <Eq>
                                            <FieldRef Name='IsReturn' />
                                            <Value Type='Text'>{0}</Value>
                                        </Eq>
                                        <Contains>
                                            <FieldRef Name='PONumber' />
                                            <Value Type='Text'>{1}</Value>
                                        </Contains>
                                    </And>
                                </Where>";
            query.Query = string.Format(sQueryCAML, true, sPO);
            SPListItemCollection sic = list.GetItems(query);
            if (null!= sic && sic.Count > 0)
            {
                sReturnPO= sic[0]["ReturnNumber"].ToString();
            }
                   
            return sReturnPO;
        }

        /// <summary>
        /// 按照DataTable里的列和行，批量插入到sListName里 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sListName"></param>
        /// <returns></returns>
        public static void BatchAddToListByDatatable(DataTable dt,string sListName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
           {
               using (SPSite site = new SPSite(SPContext.Current.Site.ID))
               {
                   using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                   {
                       #region  批插入方法体
                       SPList list = web.Lists[sListName];

                       StringBuilder sbBathtTitle = new StringBuilder();
                       sbBathtTitle.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                       sbBathtTitle.Append("<Batch onError=\"Return\">{0}</Batch>");

                       StringBuilder sbBathBody = new StringBuilder();
                       foreach (DataRow dr in dt.Rows)
                       {
                           sbBathBody.Append(string.Format("<Method ID=\"{0}\">", Guid.NewGuid().ToString()));
                           sbBathBody.Append(string.Format("<SetList Scope=\"Request\">{0}</SetList>", list.ID.ToString()));
                           sbBathBody.Append("<SetVar Name=\"ID\">New</SetVar>");
                           sbBathBody.Append("<SetVar Name=\"Cmd\">Save</SetVar>");

                           foreach (DataColumn item in dt.Columns)
                           {
                               sbBathBody.Append(string.Format("<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>", item.ColumnName, dr[item.ColumnName]));
                           }
                           sbBathBody.Append("</Method>");
                       }
                       string BatchXML = string.Format(sbBathtTitle.ToString(),sbBathBody.ToString());

                       web.AllowUnsafeUpdates = true;
                       string sBathResult = web.ProcessBatchData(BatchXML);
                       #endregion
                   }
               }
           });
        }
        /// <summary>
        /// 得到可用的ItemCode的数据（从缓存中读取。）
        /// </summary>
        /// <returns></returns>
        public static DataTable GetActiveItemCode()
        {
            DataTable dt = new DataTable();
            dt = HttpContext.Current.Cache[sItemCacheKey] as DataTable;
            if (dt == null)///缓存过期或缓存中没有
            {
                SPQuery query = new SPQuery();
                query.Query = @"
                            <Where>
                                <Eq>
                                    <FieldRef Name='IsActive' />
                                    <Value Type='Boolean'>1</Value>
                                </Eq>
                            </Where>";
                ///query.RowLimit = 10;
                query.ViewFields = GetQueryFiled();
                dt = SPContext.Current.Web.Lists["Item Codes"].GetItems(query).GetDataTable();
                HttpContext.Current.Cache.Insert(sItemCacheKey, dt, null, System.Web.Caching.Cache.NoAbsoluteExpiration, tsCache);
            }
            return dt;
        }


        /// <summary>
        /// 得到可用的Vendors的数据（从缓存中读取。）
        /// </summary>
        /// <returns></returns>
        public static DataTable GetVendorFromCache()
        {
            DataTable dt = new DataTable();
            dt = HttpContext.Current.Cache[sVendorCacheKey] as DataTable;
            if (dt == null)///缓存过期或缓存中没有
            {
                SPQuery query = new SPQuery();
                query.ViewFields = GetVendorFieldVeiw();
                dt = SPContext.Current.Web.Lists["Vendors"].GetItems(query).GetDataTable();//.GetDataTable();
                HttpContext.Current.Cache.Insert(sVendorCacheKey, dt, null, System.Web.Caching.Cache.NoAbsoluteExpiration, tsCache);
            }
            return dt;
        }

        /// <summary>
        /// 筛选Vendor数据Title,VendorId
        /// </summary>
        /// <returns></returns>
        static string GetVendorFieldVeiw()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldRef Name='Title'/>");
            sb.Append("<FieldRef Name='VendorId'/>");
            return sb.ToString();
        }



        /// <summary>
        /// 筛选ItemCoded数据列
        /// </summary>
        /// <returns></returns>
        static string GetQueryFiled()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldRef Name='Title'/>");
            sb.Append("<FieldRef Name='ItemType'/>");
            sb.Append("<FieldRef Name='Description'/>");
            sb.Append("<FieldRef Name='Unit'/>");
            sb.Append("<FieldRef Name='AssetClass'/>");
            sb.Append("<FieldRef Name='VendorID'/>");
            sb.Append("<FieldRef Name='DeliveryPeriod'/>");
            sb.Append("<FieldRef Name='UnitPrice'/>");
            sb.Append("<FieldRef Name='TaxValue'/>");
            sb.Append("<FieldRef Name='IsAccpetDecimal'/>");
            sb.Append("<FieldRef Name='Currency'/>");
            sb.Append("<FieldRef Name='ItemScope'/>");
            sb.Append("<FieldRef Name='PackagedRegulation'/>");
            return sb.ToString();
        }

        /// <summary>
        /// 将datatable转成json
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="dt"></param>
        /// <param name="iTotalCount"></param>
        /// <returns></returns>
        public static string DataTableToJson(string sName, DataTable dt, int iTotalCount)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{");
            Json.Append(string.Format("\"{0}\":[", sName));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"");
                        Json.Append(dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString().Replace("\\"," ").Replace("\""," ") + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("],");

            Json.Append("\"TotalCount\":");//iTotalCount
            Json.Append("[{");
            Json.Append(string.Format("\"Count\":\"{0}", iTotalCount));
            Json.Append("\"}]");
            Json.Append("}");//结束标记
            return Json.ToString();
        }

        /// <summary>
        /// PR页面一次加载的Item的行数
        /// </summary>
        /// <returns></returns>
        public static int GetPRItemLoadCount()
        {
            string sItemLoadCount = ConfigurationManager.AppSettings["PRItemloadCount"];
            int itemLoadCount = 0;
            if (!int.TryParse(sItemLoadCount, out itemLoadCount))
            {
                itemLoadCount = 150;
            }
            return itemLoadCount;
        }

        /// <summary>
        /// 得到提交PR时将 ItemCode和Vendor缓存的分钟数据
        /// </summary>
        /// <returns></returns>
        public static int GetPRCacheMinutes()
        {
            string sPRCacheMinutes = ConfigurationManager.AppSettings["PRCacheMinutes"];
            int iPRCacheMinutes = 0;
            if (!int.TryParse(sPRCacheMinutes, out iPRCacheMinutes))
            {
                iPRCacheMinutes = 30;
            }
            return iPRCacheMinutes;
        }

        /// <summary>
        /// 得到可选择纸袋的配置时间区间
        /// </summary>
        /// <returns></returns>
        public static string GetPaperBagConfig()
        {
            string sAllowPaperBagDate = ConfigurationManager.AppSettings["AllowPaperBagDate"];
            if (Regex.IsMatch(sAllowPaperBagDate, @"^\d{1,2}-\d{1,2},\d{1,2}-\d{1,2}$"))
            {
                return sAllowPaperBagDate;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
