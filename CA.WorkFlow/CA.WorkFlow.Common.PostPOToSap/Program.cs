using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SAP.Middleware.Exchange;
using System.Data;
using System.Configuration;

namespace CA.WorkFlow.Common.PostPOToSap
{
    class Program
    {
        /// <summary>
        /// 站点名。
        /// </summary>
        static string sSiteName = Common.sSiteName;
        static readonly string sWebName = "WorkFlowCenter";
        static string sDateTime = ConfigurationManager.AppSettings["PostAvailableTime"].Trim();
        static string sIsForTest = ConfigurationManager.AppSettings["IsForTest"].Trim();
        static readonly string sAllocatedValueError = ConfigurationManager.AppSettings["AllocatedValueError"].Trim();
        static void Main(string[] args)
        {
            if (sIsForTest == "0")
            {
                BatchPost();
            }
            else
            {
                PostForTest();
            }

            //唯一不一样就是 GetUnPostSapList方法

        }
        /// <summary>
        /// 批处理
        /// </summary>
        static void BatchPost()
        {
            Console.WriteLine("Post PO,GR/SR data to sap.\r\nstart......");
            DataTable dtPoList = GetUnPostSapList("PurchaseOrderItems");//得到list里没有post到SAP。
            DataTable dtCompletePO = GetUnPostSapCompletePO(dtPoList);//得到list里没有post到SAP，己经完成了PO流程的PO数据 。 
            PostPODataToSap(dtCompletePO, dtPoList);//将 PurchaseOrderItems 里没有post到SAP,且没有做GR/SR的，己经完成了PO流程的PO数据 。 
            //PostPOReturnDataToSap(dtCompletePO, dtPoList);
            PostGRSR.PostGRSRDataToSap(sDateTime, "");
        }

        static void PostForTest()
        {
            Console.WriteLine("Please input PONumber:");
            string sPONumber = Console.ReadLine();
            if (sPONumber.Length <= 0)
            {
                return;
            }
            Console.WriteLine("Post PO data to sap.\r\nstart......");
            DataTable dtPoList = GetUnPostSapList("PurchaseOrderItems", sPONumber);//得到list里没有post到SAP,且没有做GR/SR的数据。
            if (null != dtPoList && dtPoList.Rows.Count > 0)
            {
                DataTable dtCompletePO = GetUnPostSapCompletePO(dtPoList);//得到list里没有post到SAP,且没有做GR/SR的，己经完成了PO流程的PO数据 。 
                if (null != dtCompletePO && dtCompletePO.Rows.Count > 0)
                {
                    PostPODataToSap(dtCompletePO, dtPoList);//将 PurchaseOrderItems 里没有post到SAP,且没有做GR/SR的，己经完成了PO流程的PO数据 。 
                   // PostPOReturnDataToSap(dtCompletePO, dtPoList);

                    Console.WriteLine("Post PO To SAP Complete ! ");
                }
                else
                {
                    Console.WriteLine("Error! The EWF for " + sPONumber + " doesn't Complete ! Post PO to Sap failed.");
                }
            }
            else
            {
                Console.WriteLine("Eorror! " + sPONumber + " dose not existed! Post PO To SAP failed .");
            }

            PostGRSR.PostGRSRDataToSap(sDateTime, sPONumber);
        }

        #region PO数据保存到SAP中

        /// <summary>
        /// 得到list里没有post到SAP,且没有做GR/SR的，己经完成了PO流程的数据 。 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static DataTable GetUnPostSapCompletePO(DataTable dt)
        {
            DataTable dtPO = new DataTable();
            dtPO.Columns.Add("Title");
            dtPO.Columns.Add("VendorID");
            dtPO.Columns.Add("EmployeeName");
            dtPO.Columns.Add("CondValue");
            dtPO.Columns.Add("isCapx");
            dtPO.Columns.Add("Currency");

            if (null == dt)
            {
                return null;
            }

            List<SapParameter> mSapParametersPO = new List<SapParameter>();

            //对PO数据进行分组。
            var poBaseInfo = from drVal in dt.AsEnumerable()
                             group drVal by new { Title = drVal["Title"], VendorID = drVal["VendorID"], } into g
                             select new
                             {
                                 Title = g.Key.Title,
                                 VendorID = g.Key.VendorID,
                                 //Currency=g.Key.Currency
                                 //CondValue = g.Sum(row => decimal.Parse(row["TotalPrice"].ToString()))
                             };

            foreach (var item in poBaseInfo)//每一条PO，且要完成PO流程。 
            { 
                ///Currency = drVal["Currency"]
                string sPONO = item.Title.ToString();
                string sApplicante = string.Empty;
                string sGrandTotal = string.Empty;
                if (!IsPOComplete(sPONO, out sApplicante, out sGrandTotal))
                {
                    continue;
                }

                string sCurrency=string.Empty;
                string sGetCurrencyCondition =string.Concat("Title='",sPONO,"'");
                DataRow[] drArray= dt.Select(sGetCurrencyCondition);
                 

                //是否是Capex
                bool isCapx = IsCapex(sPONO);

                DataRow dr = dtPO.NewRow();
                dr["Title"] = item.Title;
                dr["VendorID"] = item.VendorID;
                dr["EmployeeName"] = GetAccount(sApplicante);// item.EmployeeName;
                dr["CondValue"] = sGrandTotal;// item.CondValue;
                dr["isCapx"] = isCapx;
                dr["Currency"] = drArray[0]["Currency"];// item.Currency;
                dtPO.Rows.Add(dr);
            }
            return dtPO;
        }


        /// <summary>
        /// 将PO的数据Post到SAP中。 
        /// </summary>
        static void PostPODataToSap(DataTable dtCompletePO, DataTable dtPoList)
        {
            if (null == dtCompletePO || null == dtPoList)
            {
                return;
            }

            List<SapParameter> mSapParametersPO = new List<SapParameter>();

            foreach (DataRow dr in dtCompletePO.Rows)//每一条PO,非退货。 
            {
                string sPONO = dr["Title"].ToString();
                if (sPONO.EndsWith("R"))//,退货。
                {
                    continue;
                }
                
                //得到EmployeeID
                string sEmployeeID = string.Empty;
                string sEmployeeName = string.Empty;
                try
                {
                    string[] arrayEmployee = Common.GetEmployeeID(dr["EmployeeName"].ToString());
                    sEmployeeID = arrayEmployee[0];
                    sEmployeeName = arrayEmployee[1];
                }
                catch (Exception e)
                {
                    Common.WriteErrorLog("CA.Workflow.PostPOToSap.cs at Line:129 :" + e.ToString());
                    continue;
                }
                //是否是Capex
                bool isCapx = Convert.ToBoolean(dr["isCapx"].ToString());// IsCapex(sPONO);

                //PO下的所有的Item
                string sCondition = string.Concat("Title='", sPONO, "'");
                DataRow[] arrDR = dtPoList.Select(sCondition, "ItemNO desc");//.OrderBy("ItemNO");
                bool isAllocateAvalible =true;
                isAllocateAvalible = CheckAllocateValue(arrDR, sPONO);//AllocateAvalible是否和总净价相等。
                if (!isAllocateAvalible)
                {
                    Common.UpdateErrorInfo(sPONO, "Error AllocateValue", "SapErrorInfo", "Purchase Order Workflow");
                    Common.WriteErrorLog(string.Format("Error AllocateValue for PO:{0}",sPONO));
                    continue;
                }
                List<PurchaseOrderItem> listPOI = GetPOParlist(arrDR, isCapx, sPONO);
                if (null == listPOI)
                {
                    Common.WriteErrorLog(string.Format("There are no items in PO:{0}", sPONO));
                    continue;
                }
                SapParameter POItemsArr = new SapParameter();
                POItemsArr.DocType = "SA";
                POItemsArr.Pmnttrms = "Z002";
                POItemsArr.PurGroup = isCapx ? "AST" : "NT1"; //如果是固定资产，PurGroup的值为AST；否则为 NT1
                POItemsArr.DocDate = DateTime.Now.ToString("yyyyMMdd");
                POItemsArr.Vendor = StrFormate(dr["VendorID"].ToString(),10);//临时0000008008
                if (sEmployeeID != null)
                {
                    POItemsArr.EmployeeID = sEmployeeID;//dr[AuthorID] sEmployeeID.ToString(),
                    POItemsArr.EmployeeName = sEmployeeName;// dr["EmployeeName"].ToString(),
                }
                POItemsArr.PaymentCond = dr["CondValue"].ToString();//PO单付款条件
                POItemsArr.RefDocNo = sPONO;// "PO"
                POItemsArr.PurchaseOrderItems = listPOI;
                POItemsArr.Currency = dr["Currency"]==null?"":dr["Currency"].ToString();
                mSapParametersPO.Add(POItemsArr);
                StringBuilder sbResult = new StringBuilder();
                sbResult.Append(sPONO);
                sbResult.Append(" PO Post到SAP参数：\n");
                sbResult.Append(".DocType:" + POItemsArr.DocType);
                sbResult.Append("\n .Pmnttrms:" + POItemsArr.Pmnttrms);
                sbResult.Append("\n .PurGroup:" + POItemsArr.PurGroup);
                sbResult.Append("\n .DocDate:" + POItemsArr.DocDate);
                sbResult.Append("\n .Vendor:" + POItemsArr.Vendor);
                sbResult.Append("\n .EmployeeID:" + POItemsArr.EmployeeID);
                sbResult.Append("\n .EmployeeName:" + POItemsArr.EmployeeName);
                sbResult.Append("\n .PaymentCond:" + POItemsArr.PaymentCond);
                sbResult.Append("\n .RefDocNo:" + POItemsArr.RefDocNo);
                sbResult.Append("\n .Currency:" + POItemsArr.Currency);

                Common.WriteErrorLog(sbResult.ToString());
            }
            try
            {
                ISapExchange sapPOExchange = SapExchangeFactory.GetPurchaseOrder();
                List<object[]> resultPO = sapPOExchange.ImportDataToSap(mSapParametersPO);//开始导入并返回post结果 。
                Common.ProcessResult(resultPO, "SapNO", "SapErrorInfo", false);//导入成功后，更新到PO的SAPNO，失败的话记录错误信息。
            }
            catch (Exception e)
            {
                Common.WriteErrorLog("执行PO post到SAP接口时出错：" + e.ToString());
            }
        }

        /// <summary>
        /// 得到List<PurchaseOrderItem>的参数列表。
        /// </summary>
        /// <param name="arrDR"></param>
        /// <param name="isCapx"></param>
        /// <returns></returns>
        static List<PurchaseOrderItem> GetPOParlist(DataRow[] arrDR, bool isCapx,string sPONo)
        {
            List<PurchaseOrderItem> POIList = new List<PurchaseOrderItem>();
            StringBuilder sbResult = new StringBuilder();
            sbResult.Append(sPONo);
            sbResult.Append(" Item参数：\n");
            foreach (DataRow dr in arrDR)
            {
                string sItemCode=dr["ItemCode"]==null?"":dr["ItemCode"].ToString();
                decimal dRequestQty = 0;
                if (!decimal.TryParse(dr["RequestQuantity"] == null ? "0" : dr["RequestQuantity"].ToString(), out dRequestQty))
                {
                    Common.WriteErrorLog(string.Format("PO:{0},ItemCode:{1} has a Error RequestQuantity ", sPONo, sItemCode));
                    return null;
                }

                if (dRequestQty == 0)
                {
                    Common.WriteErrorLog(string.Format("PO:{0},RequestQuantity:{1} has a Error RequestQuantity ", sPONo, dr["RequestQuantity"]));
                    return null;
                }

                decimal dCondValue = 0;
                if (sItemCode.IndexOf("X", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    continue;
                }
                decimal dAllocatedValue = 0;
                decimal.TryParse(dr["AllocatedValue"] == null ? "0" : dr["AllocatedValue"].ToString(), out dAllocatedValue);
                dCondValue = Math.Round(dAllocatedValue / dRequestQty, 2);
                int iItemNO = 0;
                int.TryParse(dr["ItemNO"] == null ? "0" : dr["ItemNO"].ToString(), out iItemNO);
                string sTaxRate=dr["TaxRate"] ==null?"":dr["TaxRate"].ToString();
                string sCostCenter = dr["CostCenter"] == null ? string.Empty : dr["CostCenter"].ToString();

                PurchaseOrderItem poi = new PurchaseOrderItem();
                poi.ItemNo = iItemNO;// int.Parse(dr["ItemNO"].ToString());//数据中唯一的ID 
                poi.Quantity = Math.Round(dRequestQty, 3);
                poi.CondValue = dCondValue;
                poi.AssetNo = string.Empty; //如果是固定资产，AssetNo的值为实际的AssetNo值；否则为空

                poi.CostCenter = sCostCenter;   //如果是固定资产，CostCenter的值为 空；否则为实际的CostCenter值 
                poi.Acctasscat = "K"; //如果是固定资产，Acctasscat的值为 A；否则为 K
                poi.TaxCode = sTaxRate == "0.17" ? "J1" : "J0"; // "如果item code中包含了17%的，就传一个J1
                if (isCapx)
                {
                    string sMatlGroup = Common.GetMalGroupByAssetNO(dr["AssetClass"].ToString()).Trim();
                    if (sMatlGroup == "")
                    {
                        sMatlGroup = "ZNT_OTHER";
                    }
                    poi.MatlGroup = sMatlGroup;//dr["AssetClass"].ToString());//临时 ZNT_OTHER 
                    poi.AssetNo = dr["ACNumber"] == null ? "" : StrFormate(dr["ACNumber"].ToString(), 12);//财务在创建PO时手工填写的值 临时如果是固定资产，AssetNo的值为实际的AssetNo值；否则为空
                    poi.CostCenter = string.Empty;  //如果是固定资产，CostCenter的值为 空；否则为实际的CostCenter值 
                    poi.Acctasscat = "A"; //如果是固定资产，Acctasscat的值为 A；否则为 K
                }
                else//Opex
                {
                    string sAssetClass = dr["AssetClass"] == null ? "" : dr["AssetClass"].ToString();
                    string sMatlGroup = Common.GetMalGroupByAssetNO(sAssetClass).Trim();//dr["AssetClass"].ToString());//临时 ZNT_OTHER 
                    if (sMatlGroup == "")
                    {
                        string sErrorInfo = string.Concat("Can't find MatlGroup for ", sAssetClass);
                        Common.UpdateErrorInfo(sPONo, sErrorInfo, "SapErrorInfo", "Purchase Order Workflow");
                        Common.WriteErrorLog(string.Format("PO:{0} {1}", sPONo, sErrorInfo));
                        return null;
                    }
                    if (sCostCenter.IndexOf("H10", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                       poi.MatlGroup= Common.ConvertMatlGroup(sMatlGroup);
                    }
                    else
                    {
                        poi.MatlGroup = sMatlGroup;
                    }
                }
                poi.Description = dr["Description"] == null ? "" : dr["Description"].ToString();
                poi.Currency = dr["Currency"] == null ? "" : dr["Currency"].ToString();
                POIList.Add(poi);

                sbResult.Append("\n ItemNo：" + poi.ItemNo);
                sbResult.Append("\n Quantity：" + poi.Quantity);
                sbResult.Append("\n CondValue：" + poi.CondValue);
                sbResult.Append("\n AssetNo：" + poi.AssetNo);
                sbResult.Append("\n MatlGroup：" + poi.MatlGroup);
                sbResult.Append("\n CostCenter：" + poi.CostCenter);
                sbResult.Append("\n Acctasscat：" + poi.Acctasscat);
                sbResult.Append("\n TaxCode：" + poi.TaxCode);
                sbResult.Append("\n Description：" + poi.Description);
                sbResult.Append("\n Currency：" + poi.Currency);
            }


            Common.WriteErrorLog(sbResult.ToString());
            return POIList;
        }

        /// <summary>
        /// PO是否为Capex
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        static bool IsCapex(string sPONO)
        {
            bool isCapex = false;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(
                                  @"<Where>
                                      <Eq>
                                         <FieldRef Name='PONumber' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>", sPONO);

                        SPListItemCollection splic = web.Lists["PurchaseRequestItems"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            string sCapexType = splic[0]["RequestType"].ToString();
                            if (sCapexType.Equals("Capex", StringComparison.InvariantCultureIgnoreCase))
                            {
                                isCapex = true;
                            }
                        }
                    }
                }

            });
            return isCapex;
        }


        /// <summary>
        ///  PO是否己经完成且在配置的时间里
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="sApplicante"></param>
        /// <param name="sNetPrice"></param>
        /// <returns></returns>
        static bool IsPOComplete(string sPONO, out string sApplicante, out string sPaymentCondition)
        {
            bool isPOComplete = false;
            string sReturnApplicante = string.Empty;
            string sReturnPaymentCondition = string.Empty;
            //PONumFinance
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormate = @"<Where>
                                                      <Eq>
                                                         <FieldRef Name='PONumber' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                   </Where>";
                        query.Query = string.Format(sQueryFormate, sPONO);
                        SPListItemCollection splic = web.Lists["Purchase Order Workflow"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            if (sDateTime.Length > 0)//有配置Post到SAP的起止时间
                            {
                                DateTime dtAvalibleDate = DateTime.Now;
                                if (DateTime.TryParse(sDateTime, out dtAvalibleDate))
                                {
                                    string sIssuedDate = splic[0]["IssuedDate"].ToString();

                                    DateTime dtIssueDate = DateTime.Now;
                                    if (DateTime.TryParse(sIssuedDate, out dtIssueDate))
                                    {
                                        if (dtAvalibleDate <= dtIssueDate)//配置时间之后的数据才为True.
                                        {
                                            string sState = splic[0]["Status"].ToString();
                                            if (sState.Equals("Completed", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                isPOComplete = true;
                                            }
                                            sReturnApplicante = splic[0]["Applicant"] == null ? "" : splic[0]["Applicant"].ToString();
                                            sReturnPaymentCondition = splic[0]["PaymentCondition"] == null ? "" : splic[0]["PaymentCondition"].ToString();
                                        }
                                    }
                                }
                            }
                            else //没有配置Post到SAP的起止时间
                            {
                                string sState = splic[0]["Status"].ToString();
                                if (sState.Equals("Completed", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    isPOComplete = true;
                                }
                                sReturnApplicante = splic[0]["Applicant"] == null ? "" : splic[0]["Applicant"].ToString();
                                sReturnPaymentCondition = splic[0]["PaymentCondition"] == null ? "" : splic[0]["PaymentCondition"].ToString();
                            }
                        }
                    }
                }
            });
            sApplicante = sReturnApplicante;
            sPaymentCondition = sReturnPaymentCondition;
            return isPOComplete;
        }

         /// <summary>
         /// 总AllocatedValue是否和总净价相等。
         /// </summary>
         /// <param name="arrDR"></param>
         /// <returns></returns>
         static bool CheckAllocateValue(DataRow[] arrDR,string sPONO)
         {
            bool isOK = false;
            decimal dTotalNetPrice = 0;
            decimal dTotalAllocatePrice = 0;
            foreach (DataRow dr in arrDR)
            {
                string sItemCode = dr["ItemCode"]==null?"":dr["ItemCode"].ToString();
                if (sItemCode.IndexOf("X", StringComparison.InvariantCultureIgnoreCase)==0)
                {
                    continue;
                }

                ///总AllocatedValue
                decimal dAllocatedValue = 0;
                if (decimal.TryParse(dr["AllocatedValue"] == null ? "" : dr["AllocatedValue"].ToString(), out dAllocatedValue))//总价
                {
                    dTotalAllocatePrice += dAllocatedValue;
                }
            }
            dTotalNetPrice = GetAllServeicevalue(sPONO);// dTotalNetPrice - 

             decimal dConAllocatedError=0;
             decimal.TryParse(sAllocatedValueError, out dConAllocatedError);
            decimal absllocatedErro=Math.Abs(dTotalAllocatePrice - dTotalNetPrice);
            if (absllocatedErro <= dConAllocatedError)
            {
                isOK = true;
            }
             Common.WriteErrorLog(string.Format("{0}:The Error for AllocatedValuer is {1}", sPONO, absllocatedErro));
           

            return isOK;

        }

        /// <summary>
         /// 得到PO的INSTALLATION，TRANSPORTATION，PACKAGING，DISCOUNT
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
         static decimal GetAllServeicevalue(string sPONO)
         {
             decimal dServeiceVlue = 0;

             DataTable dt = new DataTable();
             SPListItemCollection splic = null;

             SPSecurity.RunWithElevatedPrivileges(delegate
             {
                 using (SPSite site = new SPSite(sSiteName))
                 {
                     using (SPWeb web = site.OpenWeb(sWebName))//WorkFlowCenter
                     {
                         SPQuery query = new SPQuery();
                         string sQueryFormat = @"<Where>
                                                     <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                     </Eq>
                                               </Where>";
                         query.Query =string.Format(sQueryFormat,sPONO);
                         splic = web.Lists["Purchase Order Workflow"].GetItems(query);
                         if (null != splic && splic.Count > 0)
                         {
                             decimal dTotal = 0;
                            decimal dinstNetTotal=0;
                            decimal packNetTotal=0;
                            decimal transNetTotal=0;
                            decimal discNetTotal = 0;
                            decimal.TryParse(splic[0]["SiteInstallFee"]==null?"0":splic[0]["SiteInstallFee"].ToString(),out dinstNetTotal);
                            decimal.TryParse(splic[0]["FreightCost"] == null ? "0" : splic[0]["FreightCost"].ToString(), out transNetTotal);
                            decimal.TryParse(splic[0]["PackageCharge"] == null ? "0" : splic[0]["PackageCharge"].ToString(), out packNetTotal);
                            decimal.TryParse(splic[0]["Discount"] == null ? "0" : splic[0]["Discount"].ToString(), out discNetTotal);
                            decimal.TryParse(splic[0]["Total"] == null ? "0" : splic[0]["Total"].ToString(), out dTotal);

                            dServeiceVlue =dTotal+ dinstNetTotal + packNetTotal +transNetTotal- discNetTotal;
                         }
                     }
                 }
             });

             return dServeiceVlue;
         }
        #endregion

        #region 退货

        /// <summary>
        /// 己经做过系统收货的不需要POST到SAP中
        /// </summary>
        /// <param name="dtCompletePO">没有post到SAP,且没有做GR/SR的，己经完成了PO流程的PO数据， </param>
        /// <param name="dtPoList">没有post到SAP,且没有做GR/SR的Item数据</param>
        static void PostPOReturnDataToSap(DataTable dtCompletePO, DataTable dtPoList)
         {
            if (null == dtCompletePO || null == dtPoList)
            {
                return;
            }
            List<SapParameter> mSapParametersPOR = new List<SapParameter>();

            List<ReturnPO> listReturnPO = new List<ReturnPO>();


            foreach (DataRow dr in dtCompletePO.Rows)//每一条PO ,退货的PO
            {
                string sPONO = dr["Title"].ToString();
                if (!sPONO.EndsWith("R"))//非退货的PO
                {
                    continue;
                }
                string sSAPNO = string.Empty;//目标退货的PO的SAP号
                string sTargetPO = string.Empty;//退货的目标PO号。 
                bool isReturnPOPostSAP = IsReturnPOPostSAP(sPONO, out sSAPNO, out sTargetPO);

                if (!isReturnPOPostSAP)///退货的目标PO，没有Post到SAP中
                {
                    continue;
                }

                bool isComplete = IsSysGRSR(sTargetPO);//PO 是否有己做系统收货。
                if (isComplete)
                {
                    continue;
                }
                // PO下的所有的Item
                string sCondition = string.Concat("Title='", sPONO, "'");
                DataRow[] arrDR = dtPoList.Select(sCondition);
                List<PurchaseOrderItem> listPOI = GetReturnPOItemsPar(arrDR, sTargetPO, sPONO);//, isCapx);
                if (null == listPOI || listPOI.Count == 0)
                {
                    Common.WriteErrorLog(string.Format("There are no avaliable Items in {0} to Post return po to SAP", sPONO));
                    continue;
                }

                SapParameter mSapParReturnPO = new SapParameter()
                {
                    DocType = "SA",
                    SapNumber = sSAPNO,
                    RefDocNo = sPONO
                };
                mSapParReturnPO.PurchaseOrderItems = listPOI;
                mSapParametersPOR.Add(mSapParReturnPO);

                ReturnPO structRetunPO = new ReturnPO();
                structRetunPO.sPONO = sPONO;
                structRetunPO.sTargetPO = sTargetPO;
                listReturnPO.Add(structRetunPO);

                StringBuilder sbResult = new StringBuilder();
                sbResult.Append(sPONO);
                sbResult.Append(" 执行Post退货参数：\n");
                sbResult.Append(".DocType:" + mSapParReturnPO.DocType);
                sbResult.Append("\n .Pmnttrms:" + mSapParReturnPO.SapNumber);
                sbResult.Append("\n .PurGroup:" + mSapParReturnPO.RefDocNo);
                Common.WriteErrorLog(sbResult.ToString());
            }
            try
            {
                ISapExchange iseReturn = SapExchangeFactory.GetPurchaseOrderReturn();
                List<object[]> resultReturnPO = iseReturn.ImportDataToSap(mSapParametersPOR);
                Common.ProcessResult(resultReturnPO, "SapNO", "SapErrorInfo", false);///GRSRSaPErrorInfo
                UpdateReturnPOinfo(listReturnPO);
            }
            catch (Exception e)
            {
                Common.WriteErrorLog("执行PO退货PO  post到SAP接口时出错：" + e.ToString());
            }

        }

        /// <summary>
        /// 得到退货的PO的Item的参数列表   
        /// </summary>
        /// <param name="drArray"></param>
        /// <param name="sTargetPO">退货的目标PO</param>
        /// <param name="sPONO">PO</param>
        /// <returns></returns>
        static List<PurchaseOrderItem> GetReturnPOItemsPar(DataRow[] drArray, string sTargetPO, string sPONO)
        {
            List<PurchaseOrderItem> poReturn = new List<PurchaseOrderItem>();
            DataTable dt = Common.GetPOInfoByPONO(sTargetPO);
            StringBuilder sbResult = new StringBuilder();
            sbResult.Append(sPONO);
            sbResult.Append(" 执行Post退货Item参数：\n");
            foreach (DataRow dr in drArray)
            {
                string sItemCode = dr["ItemCode"] == null ? string.Empty : dr["ItemCode"].ToString();
                string sCostCenter = dr["CostCenter"] == null ? string.Empty : dr["CostCenter"].ToString();// dr["CostCenter"].ToString();
                if (sItemCode.IndexOf("X", StringComparison.InvariantCultureIgnoreCase)==0)
                {
                    continue;
                }
                string sCondition = string.Concat("ItemCode='", sItemCode, "'", " and CostCenter='", sCostCenter, "'");
                DataRow[] arrayDR = dt.Select(sCondition);
                if (null != arrayDR && arrayDR.Length>0)//
                {
                    //目标PO的Item的数量
                    decimal dRequestQuantity = 0;
                    decimal.TryParse(arrayDR[0]["RequestQuantity"] == null ? string.Empty : arrayDR[0]["RequestQuantity"].ToString(), out dRequestQuantity);

                    //退货的PO的Item数量
                    decimal dRRequestQuantity = 0;
                    decimal.TryParse(dr["RequestQuantity"] == null ? string.Empty : dr["RequestQuantity"].ToString(), out dRRequestQuantity);

                    int iTargetPOItemNO = 0;
                    int.TryParse(arrayDR[0]["ItemNO"].ToString(), out iTargetPOItemNO);
                    //原始PO数量-该退货PO的数量
                    poReturn.Add(new PurchaseOrderItem() { Quantity = decimal.Parse((dRequestQuantity - Math.Abs(dRRequestQuantity)).ToString("0.00")), ItemNo = iTargetPOItemNO });

                    sbResult.Append("PO Quantitye:" + decimal.Parse((dRequestQuantity - Math.Abs(dRRequestQuantity)).ToString("0.00")).ToString());
                    sbResult.Append("\n ItemNo:" + iTargetPOItemNO.ToString());
                }
            }
            Common.WriteErrorLog(sbResult.ToString());
            return poReturn;
        }

        /// <summary>
        /// 退货的目标PO是否Post到SAP中
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="sSAPNO"></param>
        /// <param name="sReturnPONO"></param>
        /// <returns></returns>
        static bool IsReturnPOPostSAP(string sPONO, out string sSAPNO, out string sReturnPONO)
        {
            bool isPostToSAP = false;
            sReturnPONO = Common.GetReturnPO(sPONO);
            isPostToSAP = IsPOPostSAP(sReturnPONO, out sSAPNO);
            return isPostToSAP;
        }

        /// <summary>
        /// PO是否Post到SAP中
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="sSAPNO"></param>
        /// <returns></returns>
        static bool IsPOPostSAP(string sPONO, out string sSAPNO)
        {
            bool isPostToSAP = false;
            string sReturnSAPPO = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                     <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                     </Eq>
                                               </Where>";
                        query.Query = string.Format(sQueryFormat, sPONO);

                        SPListItemCollection splic = web.Lists["PurchaseOrderItems"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            if (null != splic[0]["SapNO"] && splic[0]["SapNO"].ToString().Length > 0)
                            {
                                isPostToSAP = true;
                                sReturnSAPPO = splic[0]["SapNO"].ToString();
                            }
                        }
                    }
                }
            });
            sSAPNO = sReturnSAPPO;
            return isPostToSAP;
        }


        /// <summary>
        /// PO是否己做系统收货
        /// </summary>
        /// <param name="sPO"></param>
        /// <returns></returns>
        static bool IsSysGRSR(string sPONO)
        {
            bool isCmplete = false;
            string sReturnSAPPO = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                  <And>
                                                     <Eq>
                                                        <FieldRef Name='PONo' />
                                                        <Value Type='Text'>{0}</Value>
                                                     </Eq>
                                                     <Eq>
                                                        <FieldRef Name='IsSystemGR' />
                                                        <Value Type='Boolean'>1</Value>
                                                     </Eq>
                                                  </And>
                                               </Where>";//IsSystemGR
                        query.Query = string.Format(sQueryFormat, sPONO);

                        SPListItemCollection splic = web.Lists["PaymentRequestItems"].GetItems(query);
                        if (null != splic && splic.Count > 0)//所有做系统GR/SR的记录
                        {
                            isCmplete = true;
                        }
                    }
                }
            });
            return isCmplete;
        }




        #endregion



        /// <summary>
        /// 得到list里没有post到SAP,且没有做GR/SR的数据。
        /// </summary>
        /// <param name="sListName"></param>
        /// <returns></returns>
        static DataTable GetUnPostSapList(string sListName)
        {
            DataTable dt = new DataTable();
            SPListItemCollection splic = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))//WorkFlowCenter
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                     <IsNull>
                                                        <FieldRef Name='SapNO' />
                                                     </IsNull>
                                               </Where>";
                        query.Query = sQueryFormat;
                        splic = web.Lists[sListName].GetItems(query);
                    }
                }
            });
            return splic.GetDataTable();
        }


        /// <summary>
        /// 得到用户的帐号， 
        /// </summary>
        /// <param name="sApplicante"></param>
        /// <returns></returns>
        static string GetAccount(string sApplicante)
        {
            int iBegin = sApplicante.IndexOf('(') + 1;
            int iEnd = sApplicante.IndexOf(')');
            if (iBegin <= 1 || iEnd < 0 || iEnd < iBegin)
            {
                return "";
            }
            string sAccount = sApplicante.Substring(iBegin, iEnd - iBegin);
            string[] arrayAcount = sAccount.Split(new string[] { "\\" }, StringSplitOptions.None);
            if (arrayAcount.Length != 2)
            {
                return "";
            }
            string sUserName = arrayAcount[1];
            return sUserName;
        }

        /// <summary>
        /// 格式化VendorID使其长度达iLength位
        /// </summary>
        /// <param name="sVendorID"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        static string StrFormate(string sVendorID,int iLength)
        {
            while (sVendorID.Length < iLength)
            {
                sVendorID = "0" + sVendorID;
            }
            return sVendorID;
        }



        /// <summary>
        ///  得到list里没有post到SAP,且没有做GR/SR的数据。 For Test
        /// </summary>
        /// <param name="sListName"></param>
        /// <returns></returns>
        static DataTable GetUnPostSapList(string sListName, string sPONO)
        {
            DataTable dt = new DataTable();
            SPListItemCollection splic = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))//WorkFlowCenter
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                  <And>
                                                    <IsNull>
                                                        <FieldRef Name='SapNO' />
                                                    </IsNull>
                                                    <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </And>
                                               </Where>";
                        query.Query = string.Format(sQueryFormat, sPONO);
                        splic = web.Lists[sListName].GetItems(query);
                    }
                }
            });
            return splic.GetDataTable();
        }

        /// <summary>
        /// 退货成功post到SAP到SAP后，更新目标PO的ReturnQuantityForSAP
        /// </summary>
        /// <param name="listReturnPO"></param>
        static void UpdateReturnPOinfo( List<ReturnPO> listReturnPO)
        {

            StringBuilder sbResult = new StringBuilder();
            foreach (ReturnPO item in listReturnPO)//每一条Post到SAP的退PO
            {
                sbResult.Append(item.sPONO);
                sbResult.Append(" 执行Post退货,更新目标PO的ReturnQuantityForSAP参数：\n");
                DataTable dt= Common.GetPOInfoByPONO(item.sPONO);//当前Post到SAP的退货的数据
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["SapNO"].ToString()) && dr["SapNO"].ToString() == "Success")//退货成功POST到SAP
                    {
                        decimal dQuantity=0;
                        decimal.TryParse(dr["RequestQuantity"].ToString(), out dQuantity);
                        string sItemCode = dr["ItemCode"] == null ? string.Empty : dr["ItemCode"].ToString();
                        string sCostCenter = dr["CostCenter"] == null ? string.Empty : dr["CostCenter"].ToString();
                        UpdatePOReturnQuantityForSAP(item.sTargetPO, sItemCode, sCostCenter, dQuantity);
                        sbResult.Append("PO号：" + item.sTargetPO);
                        sbResult.Append("\n ItemCode：" +dr["ItemCode"].ToString());
                        sbResult.Append("\n ReturnQuantityForSAP：" + dQuantity.ToString());
                    }
                }
            }

            Common.WriteErrorLog(sbResult.ToString());
        }

        /// <summary>
        /// 更新PO的sItemCode的ReturnQuantityForSAP字段 
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="sItemCode"></param>
        /// <param name="dQuantity"></param>
        static void UpdatePOReturnQuantityForSAP(string sPONO, string sItemCode,string sCostCenter, decimal dQuantity)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate {
                using (SPSite site = new SPSite(Common.sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(Common.sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = string.Format(@"<Where>
                                                                  <And>
                                                                      <And>
                                                                         <Eq>
                                                                            <FieldRef Name='Title' />
                                                                            <Value Type='Text'>{0}</Value>
                                                                         </Eq>
                                                                         <Eq>
                                                                            <FieldRef Name='ItemCode' />
                                                                            <Value Type='Text'>{1}</Value>
                                                                         </Eq>
                                                                      </And>
                                                                      <Eq>
                                                                        <FieldRef Name='CostCenter' />
                                                                        <Value Type='Text'>{2}</Value>
                                                                      </Eq>
                                                                  </And>
                                                             </Where>", sPONO, sItemCode,sCostCenter);
                        query.Query = sQueryFormat;
                        SPListItemCollection splic= web.Lists["PurchaseOrderItems"].GetItems(query);
                        web.AllowUnsafeUpdates = true;
                        foreach (SPListItem item in splic)
                        {
                            item["ReturnQuantityForSAP"] = dQuantity;
                            item.Update();

                        }
                    }
                }
            });
        }
    }
}
