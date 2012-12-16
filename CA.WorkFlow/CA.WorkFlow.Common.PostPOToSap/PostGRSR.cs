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

    /// <summary>
    /// https://cnshsps.cnaidc.cn
    /// </summary>
    /// 

   public class PostGRSR
    {
        /// <summary>
        /// 站点名。
        /// </summary>
        static string sSiteName = Common.sSiteName;
        static readonly string sWebName = "WorkFlowCenter";

       #region 收货

       /// <summary>
       /// GRSR Post到SAP
       /// </summary>
       public static void PostGRSRDataToSap(string sDate,string sPONO)
        {
            List<SapParameter> mSapParametersGR = new List<SapParameter>();
            DataTable dt = GetUnSAPGRSR(sPONO);///己经做过EWF GR/SR,且post到sap中去过，但没有 post 到 sys GR/SR
            if (null == dt)
            {
                return;
            }
            var poItem = from dr in dt.AsEnumerable()
                         group dr by new { Title = dr["Title"], SapNO = dr["SapNO"] } into g
                         select new
                         {
                             Title = g.Key.Title,
                             SapNO = g.Key.SapNO,
                         };

            foreach (var po in poItem)///
            {
                string sSapNO=po.SapNO.ToString();
                string sItemPONO = po.Title.ToString();//每项的PO号

                DataRow[] drArrayUnRecieve = dt.Select(string.Format("Title='{0}' and IsRecieved<>1", sItemPONO));
                if (drArrayUnRecieve.Count() != 0)//PO记录里有没有做完EWF收货的PO Item记录。
                {
                    continue;
                }

                if (sItemPONO.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                bool isConfigDate = isDateavalible(sItemPONO, sDate);
                if (!isConfigDate)
                {
                    continue;
                }
                decimal dUnDoPercent = GetUnPaiedPercent(sItemPONO);//没有做完的比例
                if (dUnDoPercent == 0)//都做过了SysGR/SR
                {
                    continue;
                }

                SapParameter mSapParGRSR = new SapParameter()
                {
                    DocDate = DateTime.Now.ToString("yyyyMMdd"),
                    RefDocNo = sSapNO,// "6500000198",
                    Header = sItemPONO// DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                string sCondigion = string.Concat("Title='", sItemPONO, "'");
                DataRow[] drArray = dt.Select(sCondigion);

                List<StoresReceiveItem> listGRSRItem = GetSAPGRSRItem(drArray, sSapNO, sItemPONO, dUnDoPercent);
                if (null == listGRSRItem || listGRSRItem.Count == 0)
                {
                    Common.WriteErrorLog(string.Format("There are no available items in PO '{0}' for Post to SAp System GR/SR", sItemPONO));
                    continue;
                }
                mSapParGRSR.StoresReceiveItems = listGRSRItem;
                mSapParametersGR.Add(mSapParGRSR);



                StringBuilder sbResult = new StringBuilder();
                sbResult.Append(sItemPONO);
                sbResult.Append(" POst SysGRSR到SAP参数：\n");
                sbResult.Append("DocDate:" + mSapParGRSR.DocDate);
                sbResult.Append("\n RefDocNo:" + mSapParGRSR.RefDocNo);
                sbResult.Append("\n PurGroup:" + mSapParGRSR.PurGroup);
                Common.WriteErrorLog(sbResult.ToString());
            }

            ISapExchange iseGRSR = SapExchangeFactory.GetStoresReceive();
            List<object[]> resultGRSR = iseGRSR.ImportDataToSap(mSapParametersGR);
            Common.ProcessResult(resultGRSR, "IsSystemGR", "GRSRSapErrorInfo", true);
        }

        static List<StoresReceiveItem> GetSAPGRSRItem(DataRow[] arrayDR, string sSapNO, string sPONO,decimal dUnDoPercent)
        {
            List<StoresReceiveItem> listGRSRItem = new List<StoresReceiveItem>();
           // DataTable dtReutrn = Common.POReturnInfo(sPONO);
            StringBuilder sbResult = new StringBuilder();
            sbResult.Append(sPONO);
            sbResult.Append(" PO SysGRSR到SAP Item参数：\n");
            foreach (DataRow dr in arrayDR)
            {
                string sItemCode = dr["ItemCode"].ToString();
                if (sItemCode.IndexOf("X", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    continue;
                }
                decimal dTotalRequestQuantity = 0;
              //  decimal dTotalReturnQuantity = 0;
               /* if (null != dtReutrn && dtReutrn.Rows.Count > 0)
                {
                    string sCostCenter = dr["CostCenter"].ToString();
                    string sCondition = string.Concat("ItemCode='", sItemCode, "'", " and CostCenter='", sCostCenter, "'");
                    DataRow[] arrayItem = dtReutrn.Select(sCondition);
                    if (null != arrayItem && arrayItem.Length > 0)
                    {
                        decimal.TryParse(arrayItem[0]["TotalQuantity"].ToString(), out dTotalReturnQuantity);
                    }
                }*/
                decimal.TryParse(dr["RequestQuantity"] == null ? string.Empty : dr["RequestQuantity"].ToString(), out dTotalRequestQuantity);
              // decimal.TryParse(dr["ReturnQuantityForSAP"] == null ? string.Empty : dr["ReturnQuantityForSAP"].ToString(), out dTotalReturnQuantity);
                string sDescriptin = dr["Description"] == null ? string.Empty : dr["Description"].ToString();

                ///(该PO申请数量-退货的数量)*没有做完的比例
                //decimal dQuantity = (dTotalRequestQuantity - Math.Abs(dTotalReturnQuantity)) * dUnDoPercent;
                decimal dQuantity = dTotalRequestQuantity * dUnDoPercent;
                listGRSRItem.Add(new StoresReceiveItem() { Quantity = Math.Round(dQuantity, 3), SapNumber = sSapNO, ItemNo = int.Parse(dr["ItemNO"].ToString()), ItemText = sDescriptin });
                
                //Common.WriteErrorLog(string.Format("PO: {0} 的{1} Post的数量为{2}", sPONO, dr["ItemNO"].ToString(), dQuantity.ToString()));
                sbResult.Append("Quantity:" + Math.Round(dQuantity, 3));
                sbResult.Append("\n SapNumber:" + sSapNO);
                sbResult.Append("\n ItemNo:" + int.Parse(dr["ItemNO"].ToString()));
                sbResult.Append("\n ItemText:" + sDescriptin);
            }
            Common.WriteErrorLog(sbResult.ToString());
            return listGRSRItem;
        }


        /// <summary>
        /// 己经做过WEF GR/SR,且post到sap中去过，但没有 post 到 sys GR/SR
        /// </summary>
        /// <returns></returns>
        static DataTable GetUnSAPGRSR(string sPONO)
        {
            DataTable dt = new DataTable();

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();//己经做过WEF GR/SR,且post到sap中去过，但没有 post 到 sys GR/SR
                        string sQuery = string.Empty;
                        if (sPONO.Length > 0)
                        {
                            sQuery = string.Format(@"<Where>
                                                       <And>
                                                          <And>
                                                            <IsNotNull>
                                                                <FieldRef Name='SapNO' />
                                                            </IsNotNull>
                                                            <Neq>
                                                                <FieldRef Name='IsSystemGR' />
                                                                <Value Type='Boolean'>1</Value>
                                                            </Neq>
                                                          </And>
                                                        <Eq>
                                                            <FieldRef Name='Title' />
                                                            <Value Type='Text'>{0}</Value>
                                                        </Eq>
                                                      </And>
                                                    </Where>", sPONO);
                        }
                        else
                        {
                            sQuery = @"<Where>
                                              <And>
                                                    <IsNotNull>
                                                       <FieldRef Name='SapNO' />
                                                    </IsNotNull>
                                                    <Neq>
                                                        <FieldRef Name='IsSystemGR' />
                                                        <Value Type='Boolean'>1</Value>
                                                    </Neq>
                                               </And>
                                           </Where>";
                        }

                        query.Query = sQuery;
                        SPListItemCollection splic = web.Lists["PurchaseOrderItems"].GetItems(query);
                        try
                        {
                            dt = splic.GetDataTable();
                        }
                        catch(Exception e)
                        {
                            Common.WriteErrorLog(e.ToString());
                        }
                        
                    }
                }
            });

            return dt;
        }

       /// <summary>
       /// 得到PO记录里支付了的百分比
       /// </summary>
       /// <param name="sPONO"></param>
       /// <returns></returns>
        static decimal GetUnPaiedPercent(string sPONO)
        {
            decimal dUnDoPercent = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate {
                using (SPSite site = new SPSite(sSiteName))
                { 
                    using(SPWeb web=site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                      <And>
                                                         <Eq>
                                                            <FieldRef Name='IsSystemGR' />
                                                            <Value Type='Boolean'>1</Value>
                                                         </Eq>
                                                         <Eq>
                                                            <FieldRef Name='PONo' />
                                                            <Value Type='Text'>{0}</Value>
                                                         </Eq>
                                                      </And>
                                                </Where>";
                        query.Query = string.Format(sQueryFormat,sPONO);
                        SPListItemCollection splic = web.Lists["PaymentRequestItems"].GetItems(query);
                            decimal dPaied = 0;
                            foreach (SPListItem item in splic)
                            {
                                decimal dItemPaied = 0;
                                decimal.TryParse(item["PaidThisTime"] == null ? "0" : item["PaidThisTime"].ToString(), out dItemPaied);
                                dPaied += dItemPaied;
                            }
                            dUnDoPercent = 1 - dPaied / 100;
                            Common.WriteErrorLog(string.Format("PO:{0} 记录里支付了的百分比 {1}  ,末支付的百分比:{2}", sPONO, dPaied, dUnDoPercent));
                    }
                }
            });
            return dUnDoPercent;
                
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="sPONO"></param>
       /// <param name="sDate"></param>
       /// <returns></returns>
       static bool isDateavalible(string sPONO,string sDate)
        {
            bool isDateAvalible = false;
            if (sDate.Length <= 0)
            {
                return true;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                         <Eq>
                                                            <FieldRef Name='PONumber' />
                                                            <Value Type='Text'>{0}</Value>
                                                         </Eq>
                                                </Where>";
                        query.Query = string.Format(sQueryFormat, sPONO);
                        SPListItemCollection splic = web.Lists["Purchase Order Workflow"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            if (null != splic[0]["IssuedDate"])
                            {
                                DateTime dtIssueDate = DateTime.Now;
                                if (DateTime.TryParse(splic[0]["IssuedDate"].ToString(), out dtIssueDate))
                                {
                                    DateTime dtConfigDate = DateTime.Now;
                                    if (DateTime.TryParse(sDate, out dtConfigDate))
                                    {
                                        if (dtConfigDate <= dtIssueDate)
                                        {
                                            isDateAvalible = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            return isDateAvalible;
       }


        #endregion
    }
}
