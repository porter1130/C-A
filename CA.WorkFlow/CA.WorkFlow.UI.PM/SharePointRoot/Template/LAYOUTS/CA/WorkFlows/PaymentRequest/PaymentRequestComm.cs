using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFlow;
using Microsoft.SharePoint;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint.Utilities.Common;
using System.Configuration;
using QuickFlow.Core;
using CA.SharePoint;

namespace CA.WorkFlow.UI.PaymentRequest
{
    public class PaymentRequestComm
    {
        /// <summary>
        /// 是否CEO
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public static bool IsCEO(string userLoginName)
        {
            bool isCeo = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPGroup group = SPContext.Current.Web.Groups["wf_CEO"];
                foreach (SPUser user in group.Users)
                {
                    if (user.LoginName.Equals(userLoginName, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        isCeo = true;
                        break;
                    }
                }

            });  
            return isCeo;
        }

        /// <summary>
        /// 返回供应商的信息
        /// </summary>
        /// <param name="venderId">供应商ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetVendorInfo(string venderId)
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["Vendors"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='VendorId'/>" +
                                       "<Value Type='Text'>" + venderId + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        public static DataTable GetPRInfoByapplicant(string paymentRequestType)
        {
            SPListItemCollection pRInfo = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentRequestItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                    "<And>"+
                                    " <Eq>" +
                                       "<FieldRef Name='IsFromPO'/>" +
                                       "<Value Type='Boolean'>0</Value>" +
                                     "</Eq>" +
                                     "<Eq>"+
                                        "<FieldRef Name='RequestType' />"+
                                        "<Value Type='Text'>" + paymentRequestType + "</Value>" +
                                    "</Eq>"+
                                    "</And>"+
                                  "</Where>";
                pRInfo = spList.GetItems(spQuery);
            });
            if (pRInfo.Count > 0)
            {
                return pRInfo.GetDataTable();
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 返回供应商的信息
        /// </summary>
        /// <param name="venderId">供应商ID</param>
        /// <returns></returns>
        public static DataTable GetVendorInfoByVendorID(string venderId)
        {
            return GetVendorInfo(venderId).GetDataTable();
        }

        /// <summary>
        ///  返回某个PO单Items
        ///  </summary>
        /// <param name="poNo">PO ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPurchaseOrderItemsInfo(string poNo)
        {
            SPListItemCollection poItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PurchaseOrderItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     "<Eq>" +
                                        "<FieldRef Name='Title'/>" +
                                        "<Value Type='Text'>" + poNo + "</Value>" +
                                    "</Eq>" +
                                  "</Where>";
                poItems = spList.GetItems(spQuery);
            });
            return poItems;
        }
        


        /// <summary>
        /// 返回某个PO单Items
        /// </summary>
        /// <param name="poNo">PO ID</param>
        /// <returns></returns>
        public static DataTable GetPurchaseOrderItemsInfo1(string poNo)
        {
            SPListItemCollection poItems = null;
            DataTable dtSult = new DataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PurchaseOrderItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     "<Eq>" +
                                        "<FieldRef Name='Title'/>" +
                                        "<Value Type='Text'>" + poNo + "</Value>" +
                                    "</Eq>" +
                                  "</Where>";
                poItems = spList.GetItems(spQuery);
            });
            
            if (null != poItems)
            {
                DataTable dt = new DataTable();
                dt = poItems.GetDataTable();
                bool isReturnPO = IsReturnedPO(dt);
                //CommonUtil.logError("isReturnPO" + isReturnPO.ToString());

                //CommonUtil.logError("dt.Rows.Count" + dt.Rows.Count.ToString());
                if (isReturnPO)//该PO有成功做过退货（且其退货成功Post 到SAP中）
                {
                    dtSult = GetNewAllocatedDataSource(dt);
                }
                else
                {
                    dtSult = dt;
                }
            }
           // dtSult = poItems.GetDataTable();
            return dtSult;
        }


        /// <summary>
        /// 该PO里是否有退货成功的记录（退货的PO成功Post到SAP中） by xu 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static bool IsReturnedPO(DataTable dt)
        {
            bool isReturn = false;
            foreach (DataRow item in dt.Rows)
            {
                string sAllocatedValue = item["ReturnQuantityForSAP"] == null ? string.Empty : item["ReturnQuantityForSAP"].ToString();//
                decimal dAllocatedValue = 0;
                if (decimal.TryParse(sAllocatedValue, out dAllocatedValue))
                {
                    if (dAllocatedValue > 0)
                    {
                        isReturn = true;
                        break;
                    }
                }
            }
            return isReturn;
        }

        /// <summary>
        /// 得到有做过退货的PO的数据源(AllocatedValue已经重新计算)  by xu 
        /// </summary>
        /// <param name="dtList"></param>
        /// <returns></returns>
        static DataTable GetNewAllocatedDataSource(DataTable dtList)
        {
            decimal dService = 0;//总服务费
            decimal dTotalRequestQuantity = 0;
            foreach (DataRow item in dtList.Rows)
            {
                string sItemCode = item["ItemCode"] == null ? string.Empty : item["ItemCode"].ToString();
                decimal dUnitPrice = 0;
                decimal dRequestQuantity = 0;
                decimal dReturnQuantity = 0;
                decimal dTaxRate = 0;
                decimal.TryParse(item["UnitPrice"] == null ? "0" : item["UnitPrice"].ToString(), out dUnitPrice);
                decimal.TryParse(item["RequestQuantity"] == null ? "0" : item["RequestQuantity"].ToString(), out dRequestQuantity);
                decimal.TryParse(item["ReturnQuantityForSAP"] == null ? "0" : item["ReturnQuantityForSAP"].ToString(), out dReturnQuantity);
                decimal.TryParse(item["TaxRate"] == null ? "0" : item["TaxRate"].ToString(), out dTaxRate);

                decimal dTotalPrice = Math.Round(dUnitPrice * (dRequestQuantity - dReturnQuantity), 2);//当前Item的总价。
                decimal dRateValue = dTotalPrice - (dTotalPrice / (1 + dTaxRate));//当前Item的税额
                decimal dNetPrice = dTotalPrice - dRateValue;//当前Item的净价。 
                decimal dRequestQty = dRequestQuantity - dReturnQuantity;

                item["TaxValue"] = dRateValue;//数据源里税额更新。
                item["TotalPrice"] = dTotalPrice;//数据源里总价更新。
                item["RequestQuantity"] = dRequestQty;//数据源里的数量更新。 
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    string sDescription = item["Description"] == null ? "0" : item["Description"].ToString();
                    string sdiscount = WorkflowItemCode.DISCOUNT.ToLower();//CA.WorkFlow.UI.i
                    if (sDescription.ToLower().Contains(sdiscount))//包含打折的就要减掉
                    {
                        dService -= dNetPrice;
                    }
                    else //非服务费就是相加。 
                    {
                        dService += dNetPrice;
                    }
                }
                else //非服务费，则计算净价和税额
                {
                    dTotalRequestQuantity += dRequestQty;//非服务费的Item的总数量。 
                }
            }
            //CommonUtil.logError("dService+"+dService.ToString()+"     dTotalRequestQuantity:"+dTotalRequestQuantity.ToString());
            return GetNewAllocatedValue(dtList, dService, dTotalRequestQuantity);
        }

        /// <summary>
        /// 重新计算数据源中的AllocatedValue值。   by xu 
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="dTotalService"></param>
        /// <param name="dTotalRequestQuantity"></param>
        /// <returns></returns>
        static DataTable GetNewAllocatedValue(DataTable dtList, decimal dTotalService, decimal dTotalRequestQuantity)
        {
            //DataTable dt = new DataTable();
            foreach (DataRow item in dtList.Rows)
            {
                string sItemCode = item["ItemCode"] == null ? string.Empty : item["ItemCode"].ToString();
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                decimal dRequestQty = 0;
                decimal.TryParse(item["RequestQuantity"] == null ? "0" : item["RequestQuantity"].ToString(), out dRequestQty);
                item["AllocatedValue"] = dRequestQty / dTotalRequestQuantity * dTotalService;
            }
            return dtList;
        }


        /// <summary>
        /// 返回某个PO单
        /// </summary>
        /// <param name="poNo">PO ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPurchaseOrderWorkflowInfo(string poNo)
        {
            SPListItemCollection poItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["Purchase Order Workflow"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     "<Eq>" +
                                        "<FieldRef Name='PONumber'/>" +
                                        "<Value Type='Text'>" + poNo + "</Value>" +
                                    "</Eq>" +
                                  "</Where>";
                poItems = spList.GetItems(spQuery);
            });
            return poItems;
        }

        /// <summary>
        /// 返回PR付款申请单信息
        /// </summary>
        /// <param name="prNo">PR申请单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPaymentRequestItemsInfoBySUBPRNO(string subPRNo)
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentRequestItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @" <Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='SubPRNo'/>" +
                                       "<Value Type='Text'>" + subPRNo + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        /// <summary>
        /// 根据PO单ID返回PR付款申请单信息
        /// </summary>
        /// <param name="poNo">PO申请单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPaymentRequestItemsInfoByPONO(string poNo)
        {
            SPListItemCollection spItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentRequestItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                      "<Eq>" +
                                        "<FieldRef Name='PONo'/>" +
                                        "<Value Type='Text'>" + poNo + "</Value>" +
                                      "</Eq>" +
                                 "</Where>" +
                                 "<OrderBy>" +
                                        "<FieldRef Name='SubPRNo' Ascending='True'/> " +
                                 "</OrderBy>";
                spItems = spList.GetItems(spQuery);
            });
            return spItems;
        }

        /// <summary>
        /// 根据PO单ID返回PR付款申请单信息
        /// </summary>
        /// <param name="poNo">PO申请单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPaymentRequestInfo(string poNo)
        {
            SPListItemCollection spItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentRequest"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                      "<Eq>" +
                                        "<FieldRef Name='PONo'/>" +
                                        "<Value Type='Text'>" + poNo + "</Value>" +
                                      "</Eq>" +
                                 "</Where>";
                spItems = spList.GetItems(spQuery);
            });
            return spItems;
        }
        public static SPListItemCollection GetPaymentRequestInfoByContractPONo(string ContractPONo)
        {
            SPListItemCollection spItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentRequestItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                      "<Eq>" +
                                        "<FieldRef Name='ContractPONo'/>" +
                                        "<Value Type='Text'>" + ContractPONo + "</Value>" +
                                      "</Eq>" +
                                 "</Where>";
                spItems = spList.GetItems(spQuery);
            });
            return spItems;
        }

        /// <summary>
        /// 返回最后一条分期付款记录
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLastPaymentRequestInfo()
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentRequest"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<OrderBy>" +
                                        "<FieldRef Name='WorkFlowNumber' Ascending='False'/> " +
                                 "</OrderBy>";
                venderItems = spList.GetItems(spQuery);
            });

            return venderItems.GetDataTable();
        }

        /// <summary>
        /// 返回采购单信息
        /// </summary>
        /// <param name="poNo">PO单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPruchaseOrderInfo(string poNo)
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["Purchase Order WorkFlow"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='Title'/>" +
                                       "<Value Type='Text'>" + poNo + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        /// <summary>
        /// 返回采购单信息
        /// </summary>
        /// <param name="poNo">PO单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPruchaseRequestInfo(string poNo)
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["Purchase Request Workflow"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='PONumber'/>" +
                                       "<Value Type='Text'>" + poNo + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        /// <summary>
        /// 新增付款申请单
        /// </summary>
        /// <param name="prInfo">申请单数据，object[]数组第一个参数：字段名，第二个参数：值</param>
        /// <returns></returns>
        public static void SetPaymentRequestInfo(List<object[]> prInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItem spListItem = SPContext.Current.Web.Lists["PaymentRequest"].Items.Add();
                foreach (object[] field in prInfo)
                {
                    spListItem[field[0].ToString()] = field[1];
                }
                spListItem.Web.AllowUnsafeUpdates = true;
                spListItem.Update();
            });
        }

        /// <summary>
        /// 新增分期付款明细
        /// </summary>
        /// <param name="prInfo">申请单数据，object[]数组第一个参数：字段名，第二个参数：值</param>
        /// <returns></returns>
        public static void SetInstallmentInfo(List<List<object[]>> prInfos)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                foreach (List<object[]> prInfo in prInfos)
                {
                    SPListItem spListItem = SPContext.Current.Web.Lists["PaymentInstallment"].Items.Add();
                    foreach (object[] field in prInfo)
                    {
                        spListItem[field[0].ToString()] = field[1];
                    }
                    spListItem.Web.AllowUnsafeUpdates = true;
                    spListItem.Update();
                }
            });
        }

        /// <summary>
        /// 修改付款申请单信息
        /// </summary>
        /// <param name="prInfo">付款申请单数据，object[]数组第一个参数：字段名，第二个参数：值</param>
        /// <returns></returns>
        public static void SetPaymentRequestInfo(string poNo, List<object[]> prInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItemCollection spListItems = GetPaymentRequestInfo(poNo);
                foreach (SPListItem spListItem in spListItems)
                {
                    foreach (object[] field in prInfo)
                    {
                        spListItem[(string)field[0]] = field[1];
                    }
                    spListItem.Web.AllowUnsafeUpdates = true;
                    spListItem.Update();
                }
            });
        }

        /// <summary>
        /// 修改付款申请单信息
        /// </summary>
        /// <param name="prInfo">付款申请单数据，object[]数组第一个参数：字段名，第二个参数：值</param>
        /// <returns></returns>
        public static void SetPaymentRequestItemsInfo(string subPRNo, List<object[]> prInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItemCollection spListItems = GetPaymentRequestItemsInfoBySUBPRNO(subPRNo);
                foreach (SPListItem spListItem in spListItems)
                {
                    foreach (object[] field in prInfo)
                    {
                        spListItem[(string)field[0]] = field[1];
                    }
                    spListItem.Web.AllowUnsafeUpdates = true;
                    spListItem.Update();
                }
            });
        }

        /// <summary>
        /// 修改分期付款明细
        /// </summary>
        /// <param name="prInfo">分期付款数据</param>
        /// <returns></returns>
        public static void DelInstallmentInfo(string poNo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentInstallment"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @" <Where>" +
                                     "<Eq>" +
                                       "<FieldRef Name='PONo'/>" +
                                       "<Value Type='Text'>" + poNo + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
              //  WorkFlowUtil.BatchDeleteItems("PaymentInstallment", spQuery);
                SPListItemCollection spListItems = spList.GetItems(spQuery);
                if (spListItems.Count > 0)
                {
                    for (int i = spListItems.Count - 1; i >= 0; i--){
                        spListItems[i].Delete();
                    }
                } 
            });
        }

        /// <summary>
        /// 修改分期付款明细
        /// </summary>
        /// <param name="prInfo">分期付款数据，object[]数组第一个参数：字段名，第二个参数：值</param>
        /// <returns></returns>
        public static void SetInstallmentInfo(string poNo, string index, List<object[]> prInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentInstallment"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @" <Where>" +
                                    "<And>" +
                                     "<Eq>" +
                                       "<FieldRef Name='PONo'/>" +
                                       "<Value Type='Text'>" + poNo + "</Value>" +
                                     "</Eq>" +
                                      "<Eq>" +
                                       "<FieldRef Name='Index'/>" +
                                       "<Value Type='Text'>" + index + "</Value>" +
                                     "</Eq>" +
                                      "</And>" +
                                  "</Where>";
                SPListItemCollection  spListItems = spList.GetItems(spQuery);

                foreach (SPListItem spListItem in spListItems)
                {
                    foreach (object[] field in prInfo)
                    {
                        spListItem[(string)field[0]] = field[1];
                    }
                    spListItem.Web.AllowUnsafeUpdates = true;
                    spListItem.Update();
                }
            });
        }

        /// <summary>
        /// 返回分期付款信息
        /// </summary>
        /// <param name="poNo"> PO单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPaymentInstallmentInfo(string poNo)
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentInstallment"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='PONo'/>" +
                                       "<Value Type='Text'>" + poNo + "</Value>" +
                                     "</Eq>" +
                                 "</Where>" +
                                 "<OrderBy>" +
                                        "<FieldRef Name='Index' Ascending='True'/> " +
                                 "</OrderBy>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        /// <summary>
        /// 返回ConstCenter
        /// </summary>
        /// <returns></returns>
        public static DataTable GetConstCenterInfo()
        {
            DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
            return dtCostCenter;
        }

        /// <summary>
        /// 返回某张采购单的 资产、费用明细
        /// </summary>
        /// <returns></returns>
        public static SPListItemCollection GetConstCenterInfo(string prNo)
        {

            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentExpenseItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='PRNo'/>" +
                                       "<Value Type='Text'>" + prNo + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        /// <summary>
        /// 新增资产、费用明细
        /// </summary>
        /// <param name="prInfo">资产、费用明细数据，object[]数组第一个参数：字段名，第二个参数：值</param>
        /// <returns></returns>
        public static void SetPaymentExpenseItemsInfo(List<object[]> prInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItem spListItem = SPContext.Current.Web.Lists["PaymentExpenseItems"].Items.Add();
                foreach (object[] field in prInfo)
                {
                    spListItem[(string)field[0]] = field[1];
                }
                spListItem.Web.AllowUnsafeUpdates = true;
                spListItem.Update();
            });
        }

        /// <summary>
        /// 修改资产、费用明细
        /// </summary>
        /// <param name="poNo"></param>
        /// <param name="poInfo"></param>
        public static void SetPaymentExpenseItemsInfo(string prNo, List<object[]> prInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItemCollection spListItems = GetPaymentExpenseItemsInfo(prNo);
                foreach (SPListItem spListItem in spListItems){
                    spListItem.Delete();
                }
                SetPaymentExpenseItemsInfo(prInfo);
            });
        }

        /// <summary>
        /// 返回资产、费用明细
        /// </summary>
        /// <param name="poNo"> PO单ID</param>
        /// <returns></returns>
        public static SPListItemCollection GetPaymentExpenseItemsInfo(string prNo)
        {
            SPListItemCollection venderItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList spList = SPContext.Current.Web.Lists["PaymentExpenseItems"];
                SPQuery spQuery = new SPQuery();
                spQuery.Query = @"<Where>" +
                                     " <Eq>" +
                                       "<FieldRef Name='PRNo'/>" +
                                       "<Value Type='Text'>" + prNo + "</Value>" +
                                     "</Eq>" +
                                  "</Where>";
                venderItems = spList.GetItems(spQuery);
            });
            return venderItems;
        }

        /// <summary>
        /// 修改分期付款状态
        /// </summary>
        /// <param name="poNo"></param>
        /// <param name="poInfo"></param>
        public static void SetPaymentInstallmentInfo(string poNo, List<object[]> poInfo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItemCollection spListItems = GetPaymentInstallmentInfo(poNo);
                foreach (SPListItem spListItem in spListItems)
                {
                    foreach (object[] field in poInfo)
                    {
                        spListItem[(string)field[0]] = field[1];
                    }
                    spListItem.Web.AllowUnsafeUpdates = true;
                    spListItem.Update();
                }
            });
        }

        /// <summary>
        /// 返回用户名组
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="mName">模块名</param>
        /// <returns></returns>
        internal static NameCollection GetTaskUsersByModuleWithoutDeleman(string group, string module)
        {
            string moduleID = WorkFlowUtil.GetModuleIdByListName(module);
            return WorkFlowUtil.GetTaskUsersWithoutDeleman(group, moduleID);
        }

        /// <summary>
        /// 根据组和模块返回用户
        /// </summary>
        /// <param name="group">组名<param>
        /// <param name="module">模块名</param>
        /// <returns>用户集合</returns>
        internal static NameCollection GetTaskUsersByModuleAndGroup(string group, string module)
        {
            string moduleID = WorkFlowUtil.GetModuleIdByListName(module);
            return WorkFlowUtil.GetTaskUsers(group, moduleID);
        }

        public static List<string> GetMailMembers(params string[] personTypes)
        {
            List<string> members = new List<string>();

            var qType = new QueryField("PersonType", false);
            CamlExpression exp = null;
            foreach (var type in personTypes)
            {
                exp = WorkFlowUtil.LinkOr(exp, qType.Equal(type));
            }

            SPListItemCollection lc = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                            .From(web.Lists["MailMember"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });

            foreach (SPListItem item in lc)
            {
                members.Add(item["Account"].ToString());
            }
            return members;
        }

        public static bool IsLastTask(string listId, string itemId)
        {
            int count = 0;
            SPListItemCollection coll = null;
            coll = GetAllTasks(listId, itemId);
            foreach (SPListItem item in coll)
            {
                if (item["Status"].ToString().Equals("Not Started", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    count++;
                }
            }
            return count == 1;
        }

        public static SPListItemCollection GetAllTasks(string listId, string itemId)
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

        /// <summary>
        /// 获取当前登录用户的配额，参数：账户
        /// </summary>
        /// <returns>配额</returns>
        public static long GetQuota(string manager)
        {
            string levelType = "Contract Approval Limits";
            return WorkFlowUtil.GetQuota(manager, levelType);
        }

        public static int GetLevel(string account)
        {
            var emp = UserProfileUtil.GetEmployeeEx(account);
            var quotaStr = WorkFlowUtil.GetQuotaByType("Contract Approval Limits");
            if (string.IsNullOrEmpty(quotaStr)){
                return -1;
            }

            char[] split = { ';' };
            string[] quotas = quotaStr.Split(split);
            var start = emp.JobLevel.IndexOf("-") + 1; //L-3
            var length = emp.JobLevel.Length - start;
            var level = Convert.ToInt32(emp.JobLevel.Substring(start, length));

            return level;
        }

        /// <summary>
        /// 获取下一个审批者信息
        /// </summary>
        /// <param name="mStr">当前审批人</param>
        public static Employee GetNextApproverEmp(string mStr)
        {
            var managerEmp = WorkFlowUtil.GetNextApprover(mStr);
            //if (managerEmp == null)
            //{
            //    List<string> ceos = WorkflowPerson.GetCEO();
            //    if (ceos.Count == 0){
            //  DisplayMessage("The init error about WorkflowPerson in the system.");
            //        return null;
            //    }
            //    managerEmp = UserProfileUtil.GetEmployeeEx(ceos[0]);
            //}
            return managerEmp;
        }

        public static List<string> ConvertToList(NameCollection coll)
        {
            List<string> list = new List<string>();
            foreach (var temp in coll)
            {
                list.Add(temp);
            }
            return list;
        }

        public static void GetManagers(NameCollection managerNameColl, ref string manager, string userName, bool isCeo)
        {
            Employee managerEmp = WorkFlowUtil.GetNextApprover(userName);
            if (managerEmp == null)
            {
                if (isCeo)
                {
                    string groupName = ConfigurationManager.AppSettings["PRPAccount"].ToString().Split(';')[0];
                    managerNameColl = PaymentRequestComm.GetTaskUsersByModuleWithoutDeleman(groupName, "PaymentRequest");
                }
            }
            else
            {
                manager = managerEmp.UserAccount;
                GetTaskUsersByModule(managerNameColl, manager, "PaymentRequest");
            }
        }

        internal static NameCollection GetTaskUsersByModule(NameCollection manager, string managerAccount, string module)
        {
            manager.Add(managerAccount);
            string moduleID = WorkFlowUtil.GetModuleIdByListName(module);
            var deleman = WorkFlowUtil.GetDeleman(managerAccount, moduleID);
            if (deleman != null){
                manager.Add(deleman);
            }

            return manager;
        }

        private void SendMailForSubmit(NameCollection departmentManagerTaskUsers, string workflowNumber)
        {
            //Send mail to Onsite and Receptionist
            var templateTitle = "PaymentRequest";
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            List<string> to = PaymentRequestComm.GetMailMembers("Receptionist", "C-Trip");
            string rootweburl = System.Configuration.ConfigurationManager.AppSettings["rootweburl"];
            string detailLink = rootweburl + "/WorkFlowCenter/Lists/TravelRequestWorkflow2/TRPending.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(workflowNumber);
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);

            //Send mail to Applicant
            to = new List<string>();
            parameters = new List<string>();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            var approverNames = WorkFlowUtil.GetDisplayNames(PaymentRequestComm.ConvertToList(departmentManagerTaskUsers));
            templateTitle = "PaymentRequest";
            //detailLink = rootweburl + "WorkFlowCenter/Lists/TravelRequestWorkflow2/MyApply.aspx";
            to.Add(applicantAccount);
            parameters.Add("");
            parameters.Add(approverNames);
            //parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, true);

            //Send mail to Department Manager
            parameters = new List<string>();
            to = PaymentRequestComm.ConvertToList(departmentManagerTaskUsers);
            templateTitle = "PaymentRequest";
            detailLink = rootweburl + "CA/MyTasks.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);
        }

        protected void SendNotificationMail(string templateName, List<string> parameters, List<string> to, bool toApplicant)
        {
            var emailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(templateName);
            if (emailTemplate == null)
            {
                return;
            }
            string bodyTemplate = emailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string subject = emailTemplate["Subject"].AsString();

                WorkFlowUtil.SendMail(toApplicant ? subject : parameters[1] + ":" + subject, bodyTemplate, parameters, to);
            }
        }


        internal static void AddItemTable(DataForm dataForm, WorkflowDataFields fields)
        {
            AddItemTable(dataForm.ItemTable, dataForm,fields );
        }

        internal static void AddItemTable1(Installment dataForm, WorkflowDataFields fields)
        {
            AddItemTable1(dataForm.ItemTable, dataForm, fields, dataForm.SummaryAmount);
        }

        internal static void AddItemTable(DataTable itemTable, DataForm dataForm, WorkflowDataFields fields)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ExpenseType");
            dt.Columns.Add("ItemAmount");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("GLAccount");
            dt.Columns.Add("PRWorkFlowNumber");
            dt.Columns.Add("SubPRNo");
            dt.Columns.Add("Title");
            dt.Columns.Add("FANO");
            dt.Columns.Add("ItemInstallmentAmount");
           foreach (DataRow dr in itemTable.Rows)
            {
                DataRow newdr = dt.Rows.Add();
                newdr["ExpenseType"] = dr["ExpenseType"].ToString();
                newdr["GLAccount"] = dr["GLAccount"].ToString();
                newdr["CostCenter"] = dr["CostCenter"].ToString();
                newdr["ItemAmount"] = dr["ItemAmount"].ToString();
                newdr["PRWorkFlowNumber"] = fields["Title"].ToString();
                newdr["SubPRNo"] = fields["SubPRNo"].ToString();
                newdr["Title"] = fields["SubPRNo"].ToString();
                newdr["FANO"] = dr["FANO"].AsString();
                newdr["ItemInstallmentAmount"] = dr["ItemAmount"].ToString();
            }
            dataForm.ItemTable = dt;
        }

        internal static void AddItemTable1(DataTable itemTable, Installment dataForm, WorkflowDataFields fields, string summaryAmount)
        {
            List<string> list = summaryAmount.Split(';').ToList<string>();
            list.Remove("");
            DataTable dt = new DataTable();
            dt.Columns.Add("ExpenseType");
            dt.Columns.Add("ItemAmount");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("GLAccount");
            dt.Columns.Add("PRWorkFlowNumber");
            dt.Columns.Add("SubPRNo");
            dt.Columns.Add("Title");
            dt.Columns.Add("FANO");
            dt.Columns.Add("ItemInstallmentAmount");
            //foreach (DataRow dr in itemTable.Rows)
            //{
            //    DataRow newdr = dt.Rows.Add();
            //    newdr["ExpenseType"] = dr["ExpenseType"].ToString();
            //    newdr["GLAccount"] = dr["GLAccount"].ToString();
            //    newdr["CostCenter"] = dr["CostCenter"].ToString();
            //    newdr["ItemAmount"] = dr["ItemAmount"].ToString();
            //    newdr["PRWorkFlowNumber"] = fields["Title"].ToString();
            //    newdr["SubPRNo"] = fields["SubPRNo"].ToString();
            //    newdr["Title"] = fields["SubPRNo"].ToString();
            //    newdr["FANO"] = dr["FANO"].AsString();
            //}
            for (int i = 0; i < itemTable.Rows.Count;i++ )
            {
                DataRow newdr = dt.Rows.Add();
                newdr["ExpenseType"] = itemTable.Rows[i]["ExpenseType"].ToString();
                newdr["GLAccount"] = itemTable.Rows[i]["GLAccount"].ToString();
                newdr["CostCenter"] = itemTable.Rows[i]["CostCenter"].ToString();
                newdr["ItemAmount"] = itemTable.Rows[i]["ItemAmount"].ToString();
                newdr["PRWorkFlowNumber"] = fields["Title"].ToString();
                newdr["SubPRNo"] = fields["SubPRNo"].ToString();
                newdr["Title"] = fields["SubPRNo"].ToString();
                newdr["FANO"] = itemTable.Rows[i]["FANO"].AsString();
                newdr["ItemInstallmentAmount"] = list[i].Trim();
            }

            dataForm.ItemTable = dt;
        }

        internal static void DeleteAllDraftSAPItems(string workflowNumber)
        {
            //DataTable dt = PaymentRequestComm.GetDataTable(workflowNumber);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            BatchDeleteItems("PaymentRequestItemDetails", workflowNumber);
            //}
        }

        private static void BatchDeleteItems(string listName, string workflowNumber)
        {
            // Set up the variables to be used.
            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
                "<SetList Scope=\"Request\">{1}</SetList>" +
                "<SetVar Name=\"ID\">{2}</SetVar>" +
                "<SetVar Name=\"Cmd\">Delete</SetVar>" +
                "</Method>";

            // Get the list containing the items to update.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);

            // Query to get the unprocessed items.
            SPQuery query = new SPQuery();
            query.Query = @"<Where>
                                <Eq>
                                    <FieldRef Name='SubPRNo' />
                                    <Value Type='Text'>" + workflowNumber + @"</Value>
                                </Eq>
                            </Where>";
            query.ViewAttributes = "Scope='Recursive'";
            SPListItemCollection unprocessedItems = list.GetItems(query);

            if (unprocessedItems.Count > 0)
            {
                // Build the CAML delete commands.
                foreach (SPListItem item in unprocessedItems)
                {
                    methodBuilder.AppendFormat(methodFormat, "1", item.ParentList.ID, item.ID.ToString());
                }

                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch.ToString());
            }
        }

        internal static void SaveSAPItemsDetails(DataForm dataForm)
        {
            BatchInsertSAPItems("PaymentRequestItemDetails", dataForm.ItemTable);
        }

        internal static void SaveSAPItemsDetails1(Installment dataForm)
        {
            BatchInsertSAPItems("PaymentRequestItemDetails", dataForm.ItemTable);
        }

        //Batch Insert
        private static void BatchInsertSAPItems(string listName, DataTable dt)
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
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{5}</SetVar>" +
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemAmount\">{6}</SetVar>" +
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#PRWorkFlowNumber\">{7}</SetVar>" +
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SubPRNo\">{8}</SetVar>" +
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#GLAccount\">{9}</SetVar>" +
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#FANO\">{10}</SetVar>" +
                  "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemInstallmentAmount\">{11}</SetVar>" +
              "</Method>";

            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                                    dr["Title"].ToString(),
                                    dr["ExpenseType"].ToString(),
                                    dr["CostCenter"].ToString(),
                                    dr["ItemAmount"].ToString(),
                                    dr["PRWorkFlowNumber"].ToString(),
                                    dr["SubPRNo"].ToString(),
                                    dr["GLAccount"].ToString(),
                                    dr["FANO"].AsString(),
                                    dr["ItemInstallmentAmount"].AsString()
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

        internal static DataTable GetDataTable(string requestId)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("PaymentRequestItemDetails");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='SubPRNo' /><Value Type='Text'>{0}</Value></Eq></Where>", requestId);
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems.GetDataTable();
        }

        internal static DataTable GetPRExpenseTypeDataTable(string opexCapexStatus)
        {
            DataTable table = new DataTable();
            string opexCapexType = "";
            switch (opexCapexStatus)
            {
                case OpexCapexStatus.Opex:
                    opexCapexType = "Opex";
                    break;
                case OpexCapexStatus.Capex_AssetNo:
                    opexCapexType = "Capex";
                    break;
                case OpexCapexStatus.Capex_NoAssetNo:
                    opexCapexType = "Capex";
                    break;
            }
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Payment Request Expense Type");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='OpexCapexType' /><Value Type='Text'>{0}</Value></Eq></Where>", opexCapexType);
            SPListItemCollection listItems = delegationList.GetItems(query);
            table = listItems.GetDataTable()
                            .AsEnumerable()
                            .Where(dr => dr.Field<string>("ExpenseType").ToString().IndexOf("Accrued") == -1
                                      && dr.Field<string>("ExpenseType").ToString().IndexOf("Accrual") == -1)
                            .CopyToDataTable();
            #region
            //dr.Field<string>("ExpenseType").ToString().IndexOf("Accrual") == -1
            //dr.Field<string>("ExpenseType").ToString().IndexOf("Prepaid") == -1
            #endregion
            DataTable tableNew = new DataTable();
            tableNew.Columns.Add("ExpenseType");
            tableNew.Columns.Add("GLAccount");
            tableNew.Columns.Add("OriginalExpenseType");

            //DataTable tableOld = new DataTable();
            //tableOld.Columns.Add("ExpenseType");
            //tableOld.Columns.Add("GLAccount");

            //StringBuilder expenseType = new StringBuilder();

            foreach (DataRow dr in table.Rows)
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

            #region
            ////WorkflowContext的DataFields判断有问题
            //bool mode = false;
            //try
            //{
            //    string title = WorkflowContext.Current.DataFields["SubPRNo"].ToString();
            //}
            //catch (Exception ex)
            //{
            //    mode = true;
            //}

            //if (mode)
            //{
            //    return tableNew;
            //}
            //else
            //{
            //    DataTable dtEdit = GetDataCollection(WorkflowContext.Current.DataFields["SubPRNo"].ToString(), "PaymentRequestItemDetails").GetDataTable();
            //    if (null != dtEdit)
            //    {
            //        if (dtEdit.Rows.Count > 0)
            //        {
            //            bool exists = false;
            //            foreach (DataRow row in dtEdit.Rows)
            //            {
            //                if (expenseType.ToString().IndexOf(row["ExpenseType"].ToString()) != -1)
            //                {
            //                    exists = true;
            //                    break;
            //                }
            //            }
            //            if (exists)
            //            {
            //                return tableOld;
            //            }
            //            else
            //            {
            //                return tableNew;
            //            }
            //        }
            //    }
            //    CommonUtil.logError(string.Format("PaymentRequestItem：{0} :: NULL", WorkflowContext.Current.DataFields["SubPRNo"].ToString()));
            //    return null;
            //}
            #endregion
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

    }
}
