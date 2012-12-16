using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.Office.Server;
using System.Data;
using SAP.Middleware.Exchange;
using System.IO;
using System.Configuration;

namespace CA.WorkFlow.Common.PostPOToSap
{
    public class Common
    {
        public static string sSiteName = ConfigurationManager.AppSettings["SiteName"];
        public static string sWebName = "WorkFlowCenter";


        /// <summary>
        /// 得到EmployeeName和EmployeeID
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public static string[] GetEmployeeID(string userAccount)//Employee
        {
            //string sEmployeeID = string.Empty;
            //string sName=string.Empty;
            string[] arrayEmployee = new string[2];
            try
            {
                //从SSP里面取用户。
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(sSiteName))
                    {
                        using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                        {
                            Microsoft.Office.Server.ServerContext context = Microsoft.Office.Server.ServerContext.GetContext(site);
                            UserProfileManager profileManager = new UserProfileManager(context);
                            if (profileManager.UserExists(userAccount))
                            {
                                UserProfile userProfile = profileManager.GetUserProfile(userAccount);
                                arrayEmployee[0] = userProfile["EmployeeId"].Value.ToString();
                                arrayEmployee[1] = string.Concat(userProfile["FirstName"].Value.ToString(), " ", userProfile["LastName"].Value.ToString());
                            }
                            else
                            {
                                // throw new Exception("共享服务(SSP)中没有该用户的信息，可能是共享服务(SSP)没有和AD同步所致，请联系IT管理员联系！");
                                WriteErrorLog(string.Format("共享服务(SSP)中没有用户：{0}的信息，可能是共享服务(SSP)没有和AD同步所致，请联系IT管理员联系！", userAccount));
                            }
                        }
                    }
                }
                );
            }
            catch (Exception e)
            {
                WriteErrorLog(string.Format("获取当前的用户 {0} 信息出错：{1}", userAccount, e.Message.ToString()));
            }

            return arrayEmployee;
        }

        /// <summary>
        /// AssetClass得到MardGroup 
        /// </summary>
        /// <param name="sAssetNO"></param>
        /// <returns></returns>
        public static string GetMalGroupByAssetNO(string sAssetNO)
        {
            string sMalGroup = "";

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                     <Eq>
                                                        <FieldRef Name='AssetClass' />
                                                        <Value Type='Text'>{0}</Value>
                                                     </Eq>
                                               </Where>";
                        query.Query = string.Format(sQueryFormat, sAssetNO);

                        SPListItemCollection splic = web.Lists["MappingForMard"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            sMalGroup = splic[0]["Title"] == null ? "" : splic[0]["Title"].ToString();
                        }
                    }
                }
            });
            return sMalGroup;
        }

        /// <summary>
        /// 得到po下的所有的信息
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        public static DataTable GetPOInfoByPONO(string sPONO)
        {
            SPListItemCollection splic = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                     <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                     </Eq>
                                               </Where>";
                        query.Query = string.Format(sQueryFormat, sPONO);
                        splic = web.Lists["PurchaseOrderItems"].GetItems(query);

                    }
                }
            });
            return splic.GetDataTable();
        }

        /// <summary>
        /// 得到退货的信息
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        public static DataTable POReturnInfo(string sPONO)
        {
            DataTable dt = new DataTable();
            string sPRNO = GetReturnPRNOByPO(sPONO);
            if (sPRNO.Length == 0)
            {
                return null;
            }

            SPListItemCollection splic = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                     <Eq>
                                                        <FieldRef Name='PRNumber' />
                                                        <Value Type='Text'>{0}</Value>
                                                     </Eq>
                                               </Where>";
                        query.Query = string.Format(sQueryFormat, sPRNO);
                        splic = web.Lists["PurchaseOrderItems"].GetItems(query);
                    }
                }
            });
            return splic.GetDataTable();
            //string sReturnTargetPONO= GetReturnPO(sPONO);
            //if (sReturnTargetPONO.Length > 0)//该po有做过退货。
            //{
            //    dt = GetPOInfoByPONO(sReturnTargetPONO);
            //}
        }

        /// <summary>
        /// 得到退货单所对应的退货的PO号 
        /// </summary>
        /// <param name="sPO"></param>
        /// <returns></returns>
        public static string GetReturnPO(string sPO)
        {
            string sReturnPO = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                    {
                        SPList list = web.Lists["Purchase Request Workflow"];
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
                        if (null != sic && sic.Count > 0)
                        {
                            sReturnPO = sic[0]["ReturnNumber"].ToString();
                        }
                    }
                }
            });
            return sReturnPO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        public static string GetReturnPRNOByPO(string sPONO)
        {
            string sPRNO = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                    {
                        SPList list = web.Lists["Purchase Request Workflow"];
                        SPQuery query = new SPQuery();
                        string sQueryCAML = @"<Where>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='IsReturn' />
                                                        <Value Type='Text'>1</Value>
                                                    </Eq>
                                                    <Eq>
                                                        <FieldRef Name='ReturnNumber' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </And>
                                            </Where>";
                        query.Query = string.Format(sQueryCAML, sPONO);
                        SPListItemCollection splic = list.GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            sPRNO = splic[0]["WorkflowNumber"].ToString();
                        }
                    }
                }
            });
            return sPRNO;
        }


        /// <summary>
        /// 处理Post到SAP后的结果(导入成功后，更新到PO的SAPNO，失败的话记录错误信息。)
        /// </summary>
        /// <param name="sapResult"></param>
        /// <param name="sSapColumnName"></param>
        /// <param name="sErrorColumnName"></param>
        public static void ProcessResult(List<object[]> sapResult, string sSapColumnName, string sErrorColumnName, bool isGRSR)
        {
            for (int i = 0; i < sapResult.Count; i++)///相当于每一个PO 
            {
                bool bl = (bool)sapResult[i][2];//是否成功
                SapParameter sp = (SapParameter)sapResult[i][0];//传入到的对象 。
                if (sapResult[i][1] is string)
                {
                    WriteErrorLog(string.Format("Error in ProcessResult() at line 270 :{0}", sapResult[i][1].ToString()));
                }
                else
                {
                    SapResult sr = (SapResult)sapResult[i][1];
                    var sPONO = sp.RefDocNo;
                    if (isGRSR)
                    {
                        sPONO = sp.Header;
                    }
                    if (bl)//成功
                    {
                        object sSapNO;// string.Empty;//SAP号
                        if (null == sr.OBJ_KEY)
                        {
                            sSapNO = "Success";// break;
                        }
                        else
                        {
                            sSapNO = sr.OBJ_KEY;//成功后的SAPNO
                        }
                        if (isGRSR)
                        {
                            sSapNO = true;
                            UpdateItemVal("PaymentRequestItems", "PONo", sPONO, "IsSystemGR", true);//PaymentRequestItems
                        }

                        UpdateSapNO(sSapNO, sPONO, sSapColumnName, "PurchaseOrderItems");///更新PurchaseOrderItems的SAPNumber
                        UpdateSapNO(sSapNO, sPONO, sSapColumnName, "Purchase Order Workflow");///更新"Purchase Order Workflow"SAPNumber
                        UpdateErrorInfo(sPONO, null, sErrorColumnName, "Purchase Order Workflow");
                    }
                    else
                    {
                        string sErrorInfo = GetErrorInfo(sr);
                        UpdateErrorInfo(sPONO, sErrorInfo, sErrorColumnName, "Purchase Order Workflow");
                    }
                }
            }
        }


        /// <summary>
        /// 根据PO号更新sListName里sSAPColumnName 的SAPNO
        /// </summary>
        /// <param name="sSAPNO"></param>
        /// <param name="sColumnValue"></param>
        /// <param name="sSAPColumnName"></param>
        /// <param name="sListName"></param>
        public static void UpdateSapNO(object sSAPNO, string sColumnValue, string sSAPColumnName, string sListName)
        {
            SPListItemCollection splic = null;

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
                        query.Query = string.Format(sQueryFormat, sColumnValue);
                        splic = web.Lists[sListName].GetItems(query);

                        web.AllowUnsafeUpdates = true;
                        foreach (SPListItem item in splic)
                        {
                            item[sSAPColumnName] = sSAPNO;
                            item.Update();
                        }
                    }
                }
            });
        }

        ///SapErrorInfo

        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        static string GetErrorInfo(SapResult sr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
            {
                if (sb.Length > 0)
                {
                    sb.Append("\n");
                }
                sb.Append(ret.MESSAGE);//ret.MESSAGE
            }
            return sb.ToString();
        }

        /// <summary>
        /// 记录Post到SAP到的错误信息
        /// </summary>
        /// <param name="sColumnValue"></param>
        /// <param name="sErrorInfo"></param>
        /// <param name="sErrorColumnName"></param>
        /// <param name="sListName"></param>
        public static void UpdateErrorInfo(object sColumnValue, string sErrorInfo, string sErrorColumnName, string sListName)//SapErrorInfo
        {
            //SapErrorInfo  
            SPListItemCollection splic = null;

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
                        query.Query = string.Format(sQueryFormat, sColumnValue);
                        splic = web.Lists[sListName].GetItems(query);//PurchaseOrderItems

                        web.AllowUnsafeUpdates = true;
                        foreach (SPListItem item in splic)
                        {
                            item[sErrorColumnName] = sErrorInfo;
                            item.Update();
                        }
                    }
                }
            });

        }

        public static void WriteErrorLog(string sErrorInfo)
        {
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }
            string sDate = DateTime.Now.ToString("yyyy-MM-dd");
            string sFileNme = string.Concat("Log/",sDate,".txt");
            StreamWriter sw = File.AppendText(sFileNme);
            string sErrorFormate = string.Format("{0}: {1}\r\n-----------------------------------------------------------\r\n", DateTime.Now.ToString(), sErrorInfo);

            sw.WriteLine(sErrorFormate);
            sw.Flush();
            sw.Dispose();
        }

        public static void UpdateItemVal(string sListName, string sWitchColumn, string sColumnVal, string sTargeColumn, object sValue)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryFormat = @"<Where>
                                                         <Eq>
                                                            <FieldRef Name='{0}' />
                                                            <Value Type='Text'>{1}</Value>
                                                         </Eq>
                                                </Where>";
                        query.Query = string.Format(sQueryFormat, sWitchColumn, sColumnVal);
                        SPListItemCollection splic = web.Lists[sListName].GetItems(query);//PaymentRequestItems
                        web.AllowUnsafeUpdates = true;
                        foreach (SPListItem item in splic)
                        {
                            item[sTargeColumn] = sValue;
                            item.Update();
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 得到转换后的MatlGroup
        /// </summary>
        /// <param name="sMartGroup"></param>
        /// <returns></returns>
        public static string ConvertMatlGroup(string sMartGroup)
        {
            string sResult = string.Empty;
            switch (sMartGroup)
            {
                case "ZNT_STSTY": sResult = "ZNT_STS_H";
                    break;
                case "ZNT_SFM": sResult = "ZNT_SFM_H";
                    break;
                case "ZNT_OTHER": sResult = "ZNT_OTH_H";
                    break;
                case "ZNT_MAIN":sResult="ZNT_MAI_H";
                    break;
                case "ZNT_SBAGS": sResult = "ZNT_SBA_H";
                    break;
                default :sResult=sMartGroup;
                    break;
            }
            return sResult;
        }

    }

    /// <summary>
    /// 退货PO
    /// </summary>
    public struct ReturnPO
    {
       public string sPONO;
       public string sTargetPO;
    }
}
