using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.SharePoint;
using System.Text;
using System.Data;
using QuickFlow;
using CA.SharePoint;

using SAP.Middleware.Exchange;
using CA.SharePoint.Utilities.Common;
using System.Text;
using System.Threading;
using System.Collections.Specialized;

namespace CA.WorkFlow.UI.POTypeChange
{
    public class Common
    {


        #region 类成员
        /*
        string sUB_DIV;

        public string SUB_DIV
        {
            get { return sUB_DIV; }
            set { sUB_DIV = value; }
        }
        string cLASS;

        public string CLASS
        {
            get { return cLASS; }
            set { cLASS = value; }
        }
        string pO;

        public string PO
        {
            get { return pO; }
            set { pO = value; }
        }
        string qTY;

        public string QTY
        {
            get { return qTY; }
            set { qTY = value; }
        }
        string oRIGINAL_OSP;

        public string ORIGINAL_OSP
        {
            get { return oRIGINAL_OSP; }
            set { oRIGINAL_OSP = value; }
        }
        string cURRENT_OMU;

        public string CURRENT_OMU
        {
            get { return cURRENT_OMU; }
            set { cURRENT_OMU = value; }
        }
        string cREATED_BY;

        public string CREATED_BY
        {
            get { return cREATED_BY; }
            set { cREATED_BY = value; }
        }
        string pAD;

        public string PAD
        {
            get { return pAD; }
            set { pAD = value; }
        }
        string sAD;

        public string SAD
        {
            get { return sAD; }
            set { sAD = value; }
        }
        string gR;

        public string GR
        {
            get { return gR; }
            set { gR = value; }
        }
        string aLLOCATED_DATE;

        public string ALLOCATED_DATE
        {
            get { return aLLOCATED_DATE; }
            set { aLLOCATED_DATE = value; }
        }*/
        string mESSAGE;

        public string MESSAGE
        {
            get { return mESSAGE; }
            set { mESSAGE = value; }
        }


        public StringBuilder sErrorMsg=new StringBuilder();


        #endregion

        static internal DataTable ChangeType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");

            DataRow drCS = dt.NewRow();
            drCS[0] = "NORM->CS";
            drCS[1] = "CS";
            dt.Rows.Add(drCS);

            DataRow drTSS = dt.NewRow();
            drTSS[0] = "CS->NORM";
            drTSS[1] = "NORM";
            dt.Rows.Add(drTSS);

            return dt;
        }
        static internal DataTable AllocationChangeType()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");

            DataRow drCS = dt.NewRow();
            drCS[0] = "NORM->CS";
            drCS[1] = "CS";
            dt.Rows.Add(drCS);

            return dt;
        }

        public const string Submit = "Submit";
        public const string Save = "Save";
        public const string End = "End";
       
   

        /// <summary>
        /// /Excel service 用于上传时的文档库名
        /// </summary>
        public static readonly string WorkflowDocumentLibrary = "Workflow Document Library";

        
        //  OSP Excel service 参数
        public static readonly string POTypeChangePosition = "POTypeChange_ExcelPos";
        public static readonly string POTypeChangePK = "POTypeChange_ExcelPrimary";
        public static readonly string POTypeChangeCol = "POTypeChange_ExcelCols";


       /// <summary>
       /// 从SAP中得到数据集
       /// </summary>
       /// <param name="listSeachCondition"></param>
       /// <returns></returns>
        public DataTable GetPOChangeTypeInfoFromSAP(List<string> listSeachCondition)
        {
            List<SapParameter> pareStyleNO = GetSapParaFormDT(listSeachCondition);
            DataTable dt = CreateData();
            ISapExchange iPOChangeType = SapExchangeFactory.GetPOTypeChangeQuery();
            List<object[]> listResult = iPOChangeType.ExportDataToCa(pareStyleNO);

            for (int i = 0; i < listResult.Count; i++)
            {
                bool bl = (bool)listResult[i][2];
                if (bl)
                {
                    SapResult sr = (SapResult)listResult[i][1];
                    string sPoNo = sr.OBJ_KEY;
                    string sMessage = sr.OBJ_POTypeChangeInfo.SMessage.Trim();
                    if (sMessage.Length > 0)
                    {
                        sErrorMsg.Append(string.Format("{0}:{1}", sPoNo, FormatStr(sMessage)));
                        continue;
                    }
                    DataRow dr = dt.NewRow();
                    dr["Title"] = sPoNo;
                    dr["WorkflowNumber"] = string.Empty;
                    dr["PAD"] = sr.OBJ_POTypeChangeInfo.PAD;
                    dr["SAD"] = sr.OBJ_POTypeChangeInfo.SAD;
                    dr["OMU"] = sr.OBJ_POTypeChangeInfo.OMU;
                    dr["Qty"] = sr.OBJ_POTypeChangeInfo.Qty;
                    dr["IsAllocated"] = sr.OBJ_POTypeChangeInfo.IsAllocated;
                    dr["IsSuccess"] = false;
                    dr["IsPADSuccess"] = false;
                    dt.Rows.Add(dr);
                }
                else
                {
                    if (listResult[i][1] is string)
                    {
                        this.sErrorMsg.Append(FormatStr(listResult[i][1].ToString()));
                    }
                    else
                    {
                        SapResult sr = (SapResult)listResult[i][1];
                        for (int j = 0; j < sr.RETURN_LIST.Count; j++)
                        {
                            this.sErrorMsg.Append(FormatStr(sr.RETURN_LIST[j].MESSAGE));
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 创建从SAP中读取的数据时所要用的Datatable格式
        /// </summary>
        /// <returns></returns>
        public DataTable CreateData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");//PO NO.
            dt.Columns.Add("WorkflowNumber");
            dt.Columns.Add("PAD");
            dt.Columns.Add("SAD");
            dt.Columns.Add("OMU");
            dt.Columns.Add("Qty");
            dt.Columns.Add("IsSuccess");
            dt.Columns.Add("NewType");
            dt.Columns.Add("NewTypeValue");
            dt.Columns.Add("IsPADSuccess");
            dt.Columns.Add("NewPAD");
            dt.Columns.Add("IsAllocated");
            return dt;
        }

        /// <summary>
        /// 得到更新到SAP后的结果集。
        /// </summary>
        /// <param name="dtUpdate"></param>
        /// <returns></returns>
        public DataTable UpdateOSPPrice(DataTable dtUpdate)
        {
            List<SapParameter> listUpdateSapPar = GetPOTypeChangePar(dtUpdate); 
            ISapExchange iPOTypeChange = SapExchangeFactory.GetPOTypeChange();
            List<object[]> listResult = iPOTypeChange.ExportDataToCa(listUpdateSapPar);
            DataTable dtReturn = new DataTable();
            dtReturn = GetUpdatedReturnDT();
            for (int i = 0; i < listResult.Count; i++)
            {
                bool bl = (bool)listResult[i][2];
                DataRow dr = dtReturn.NewRow();

                if (bl)
                {
                    SapResult sr = (SapResult)listResult[i][1];
                    string sPONO = sr.OBJ_KEY;
                    dr["PONO"] = sPONO;
                    if (sr.OBJ_POTypeChangeInfo.IsSuccess == true)
                    {
                        dr["Status"] = 1;
                    }
                    else
                    {
                        dr["ErrorInfo"] = "Unknow error";
                        CommonUtil.logError("OSP :Update NewPrice to SAP Error,Unknow error at line 240  CA.WorkFlow.UI.POTypeChange.Common.cs");
                    }
                }
                else
                {
                    dr["Status"] = 0;
                    StringBuilder sbError = new StringBuilder();
                    if (listResult[i][1] is string)
                    {
                        sbError.Append(FormatStr(listResult[i][1].ToString()));
                    }
                    else
                    {
                        SapResult sr = (SapResult)listResult[i][1];
                        string sPONO = sr.OBJ_KEY;
                        dr["PONO"] = sPONO;
                        for (int j = 0; j < sr.RETURN_LIST.Count; j++)
                        {
                            sbError.Append(FormatStr(sr.RETURN_LIST[j].MESSAGE));
                        }
                    }
                    dr["ErrorInfo"] = sbError.ToString();
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// 得到更新OSP后的返回结果 
        /// </summary>
        /// <returns></returns>
        public DataTable GetUpdatedReturnDT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONO");
            dt.Columns.Add("Status");
            dt.Columns.Add("ErrorInfo");
            return dt;
        }


        /// <summary>
        /// 得到和SAP交互时的参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        List<SapParameter> GetSapParaFormDT(List<string> listSeachCondition)
        {
            List<SapParameter> listPar = new List<SapParameter>();
            foreach (string str in listSeachCondition)
            {
                SapParameter par = new SapParameter();
                par.SapNumber = str;
                listPar.Add(par);
            }
            return listPar;
        }
        
        /// <summary>
        /// 得到和SAP交互时的参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        List<SapParameter> GetPOTypeChangePar(DataTable dt)
        {
            List<SapParameter> listPar = new List<SapParameter>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["NewType"] == null || dr["NewType"].ToString().Trim().Length==0)
                {
                    continue;
                }
                SapParameter par = new SapParameter();
                par.SapNumber = dr["PONO"].ToString();
                par.PaymentCond = dr["NewType"].ToString();
                listPar.Add(par);
            }
            return listPar;

        }

        /// <summary>
        /// 得到Excel的配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetExcelConfigInfo(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        /// <summary>
        /// 按照DataTable里的列和行，批量插入到sListName里 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sListName"></param>
        /// <returns></returns>
        public void BatchAddToListByDatatable(DataTable dt, string sListName)
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
                        string BatchXML = string.Format(sbBathtTitle.ToString(), sbBathBody.ToString());

                        web.AllowUnsafeUpdates = true;
                        string sBathResult = web.ProcessBatchData(BatchXML);
                        #endregion
                    }
                }
            });
        }


        //得到组里的用户
        public static NameCollection GetTaskUsers(string group)
        {
            NameCollection taskUsers = new NameCollection();
            List<string> groupUsers = null;
            groupUsers = WorkFlowUtil.UserListInGroup(group);
            taskUsers.AddRange(groupUsers.ToArray());
            return taskUsers;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sPONO"></param>
       public void DeleteData(string sPONO)
        {

            SPQuery query = new SPQuery();
            string sQueryFormat = @"
                                   <Where>
                                      <Eq>
                                         <FieldRef Name='WorkflowNumber' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>";
            query.Query = string.Format(sQueryFormat, sPONO);
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            WorkFlowUtil.BatchDeleteItems("POTypeChangeItems", query);
        }
        
        /// <summary>
        /// 得到数据
        /// </summary>
        /// <param name="sWorkflowNO"></param>
        /// <returns></returns>
       public DataTable GetData(string sWorkflowNO)
       {
           DataTable dt = new DataTable();
            SPQuery query = new SPQuery();
            query.Query = string.Format(@"<Where>
                                                <Eq>
                                                    <FieldRef Name='WorkflowNumber' />
                                                    <Value Type='Text'>{0}</Value>
                                                </Eq>
                                            </Where>", sWorkflowNO);
            dt = SPContext.Current.Web.Lists["POTypeChangeItems"].GetItems(query).GetDataTable();
            return dt;
       }

       /// <summary>
       /// 修改PO的审批状态及成功更新到SAP中
       /// </summary>
       /// <param name="sID"></param>
       /// <param name="isApproved"></param>
       /// <param name="isSuccess"></param>
       public void UpdateItemSapStatus(string sID, bool isApproved, bool isSuccess)
       {
           using (SPSite site = new SPSite(SPContext.Current.Site.ID))
           {
               using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
               {
                   web.AllowUnsafeUpdates = true;
                   SPQuery query = new SPQuery();
                   query.Query = string.Format(@"<Where>
                                                    <Eq>
                                                        <FieldRef Name='ID' />
                                                        <Value Type='Counter'>{0}</Value>
                                                    </Eq>
                                               </Where>", sID);
                   SPListItemCollection splic = web.Lists["POTypeChangeItems"].GetItems(query);
                   foreach (SPListItem listItem in splic)
                   {
                       listItem["IsApproved"] = isApproved;
                       listItem["IsSuccess"] = isSuccess;
                       listItem.Update();
                   }
               }
           }
       }

     /// <summary>
       /// 修改PO的审批状态及成功更新到SAP中,通过 sWorkflowNo和PO No.
     /// </summary>
     /// <param name="sWorkflowNo"></param>
     /// <param name="sStyleNo"></param>
     /// <param name="isApproved"></param>
     /// <param name="isSuccess"></param>
       public void UpdateItemSapStatus(string sWorkflowNo, string sPONo, bool isApproved, bool isSuccess)
       {
           using (SPSite site = new SPSite(SPContext.Current.Site.ID))
           {
               using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
               {
                   web.AllowUnsafeUpdates = true;
                   SPQuery query = new SPQuery();
                   query.Query = string.Format(@"<Where>
                                                    <And>
                                                        <Eq>
                                                            <FieldRef Name='Title' />
                                                            <Value Type='Text'>{0}</Value>
                                                        </Eq>
                                                        <Eq>
                                                            <FieldRef Name='WorkflowNumber' />
                                                            <Value Type='Text'>{1}</Value>
                                                        </Eq>
                                                    </And>
                                               </Where>", sPONo, sWorkflowNo);
                   SPListItemCollection splic = web.Lists["POTypeChangeItems"].GetItems(query);
                   foreach (SPListItem listItem in splic)
                   {
                       listItem["IsApproved"] = isApproved;
                       listItem["IsSuccess"] = isSuccess;
                       listItem.Update();
                   }
               }
           }
       }

       /// <summary>
       /// 修改PO的isPADSuccess是否成功更新到SAP中
       /// </summary>
       /// <param name="sWorkflowNo"></param>
       /// <param name="sPO"></param>
       /// <param name="isPADSuccess"></param>
       public void UpdatePADStatus(string sWorkflowNo, string sPO, bool isPADSuccess)
       {
           using (SPSite site = new SPSite(SPContext.Current.Site.ID))
           {
               using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
               {
                   web.AllowUnsafeUpdates = true;
                   SPQuery query = new SPQuery();
                   query.Query = string.Format(@"<Where>
                                                    <And>
                                                        <Eq>
                                                            <FieldRef Name='Title' />
                                                            <Value Type='Text'>{0}</Value>
                                                        </Eq>
                                                        <Eq>
                                                            <FieldRef Name='WorkflowNumber' />
                                                            <Value Type='Text'>{1}</Value>
                                                        </Eq>
                                                    </And>
                                               </Where>", sPO, sWorkflowNo);
                   SPListItemCollection splic = web.Lists["POTypeChangeItems"].GetItems(query);
                   foreach (SPListItem listItem in splic)
                   {
                       listItem["IsPADSuccess"] = isPADSuccess;
                       listItem.Update();
                   }
               }
           }
       }

        /// <summary>
        /// 将OSP的每个item更新为成功！
        /// </summary>
        /// <param name="sWrokflowNO"></param>
       public void UpdateOSPSuccess(string sWrokflowNO)
       {
           using (SPSite site = new SPSite(SPContext.Current.Site.ID))
           {
               using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
               {
                   web.AllowUnsafeUpdates = true;
                   SPQuery query = new SPQuery();
                   query.Query = string.Format(@"<Where>
                                                    <Eq>
                                                        <FieldRef Name='WorkflowNumber' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                  </Where>", sWrokflowNO);
                   SPListItemCollection splic = web.Lists["POTypeChangeItems"].GetItems(query);
                   foreach (SPListItem listItem in splic)
                   {
                       listItem["IsApproved"] = true;
                       listItem["IsSuccess"] = true;
                       listItem.Update();
                   }
               }
           }
       }

       public string FormatStr(string str)
       {
           return str.Replace("'", "‘").Replace("\n", "  ").Replace("\r","  ");
       }

        /// <summary>
        /// 去重复
        /// </summary>
        /// <param name="listStr"></param>
        /// <returns></returns>
       public List<string> GetDistingctList(List<string> listStr)
       {
           List<string> listResult = new List<string>();
           foreach (string item in listStr.Distinct())
           {
               listResult.Add(item);
           }
           return listResult;
       }

       /// <summary>
       /// 发送邮件
       /// </summary>
       /// <param name="sApprovers"></param>
       /// <param name="sNO"></param>
       /// <param name="sCurrentUserName"></param>
       /// <param name="sWorkflowNumber"></param>
       public void SendMail(string sApprovers,  string sNO,string sCurrentUserName,string sWorkflowNumber)
       {
           List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApprovers);
           List<string> listPars = new List<string>();//设置发送mail主体内容参数
           listPars.Add("RecieverName");
           listPars.Add(sCurrentUserName);
           listPars.Add(sNO);

           string title = "POTypeChange";
           SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
           if (listMailTemplate == null)
           {
               CommonUtil.logError("Send POTypeChange notice mail failed,Because mail template is null");
               return;
           }
           string bodyTemplate = listMailTemplate["Body"].AsString();
           if (bodyTemplate.IsNotNullOrWhitespace())
           {
               string sTitle = string.Format(listMailTemplate["Subject"].AsString(), sWorkflowNumber);
               WorkFlowUtil.SendMail(sTitle, bodyTemplate, listPars, employeReceiver);
           }
           else
           {
               CommonUtil.logError("Send POTypeChange notice mail failed,Because mail template Body is null");
           }
       }

        /// <summary>
        /// 更校每一个Item的审批状态
        /// </summary>
        /// <param name="sID"></param>
        /// <param name="isApproved"></param>
       public void UpdateItemApproveStatus(string sID, bool isApproved)
       {
           using (SPSite site = new SPSite(SPContext.Current.Site.ID))
           {
               using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
               {
                   web.AllowUnsafeUpdates = true;
                   SPQuery query = new SPQuery();
                   query.Query = string.Format(@"<Where>
                                                      <Eq>
                                                        <FieldRef Name='ID' />
                                                        <Value Type='Counter'>{0}</Value>
                                                    </Eq>
                                               </Where>", sID);
                   SPListItemCollection splic = web.Lists["POTypeChangeItems"].GetItems(query);
                   foreach (SPListItem listItem in splic)
                   {
                       listItem["IsApproved"] = isApproved;
                       listItem.Update();
                   }
               }
           }
       }


       /// <summary>
       /// wf_Allocation组人更新后发送邮件
       /// </summary>
       /// <param name="sPONOs"></param>
       /// <param name="sCurrentUserName"></param>
       /// <param name="sWorkflowNumber"></param>
       public void SendNoticeMail(string sPONOs,string sCurrentUserName,string sWorkflowNumber)
        {
            List<string> listNoitice =new List<string>();
          
            SPGroup groupIDS = WorkFlowUtil.GetUserGroup("wf_IDS");
            foreach (SPUser user in groupIDS.Users)
            {
                string sAccount = user.LoginName;
                if (user.IsSiteAdmin || sAccount.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                listNoitice.Add(sAccount);
            }

            SPGroup groupBSS = WorkFlowUtil.GetUserGroup("wf_Allocation");
            foreach (SPUser user in groupBSS.Users)
            {
                string sAccount = user.LoginName;
                if (user.IsSiteAdmin || sAccount.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                listNoitice.Add(sAccount);
            }

            StringBuilder sbApprover = new StringBuilder();
            foreach (string sAccount in listNoitice.Distinct())
            {
                sbApprover.Append(sAccount);
                sbApprover.Append(";");
            }

            try
            {
                SendMail(sbApprover.ToString(), sPONOs, sCurrentUserName, sWorkflowNumber);
               // SendPOTCMail(sbPOTC.ToString(), sPONOs);
                SendPOTCMail(sPONOs, sWorkflowNumber);
            }
            catch (Exception e)
            {
                CommonUtil.logError("Accelerator sent mail failed:" + e.ToString());
            }
        }

       /// <summary>
       /// 得到DMM,BD审批用户
       /// </summary>
       /// <returns></returns>
       public StringBuilder GetDMMBDManager(Employee emp)
       {
           StringBuilder sbManager = new System.Text.StringBuilder();
           Employee managerEmp = WorkFlowUtil.GetApproverByLevelPAD(emp);//DMM
           string sDMMManager = string.Empty;
           if (managerEmp != null)
           {
               sDMMManager = managerEmp.UserAccount;
               sbManager.Append(sDMMManager);
           }
           else
           {
               return null;
           }
           //查找BD审批人
           QuickFlow.NameCollection BDMApprover = new QuickFlow.NameCollection();
           Employee eBD = WorkFlowUtil.GetNextApprover(sDMMManager);
           if (null != eBD)
           {
               sbManager.Append(";");
               sbManager.Append(eBD.UserAccount);
           }
           return sbManager;
       }


       public string GetPONOs(DataTable dt)
       {
           StringBuilder sb = new StringBuilder();
           foreach (DataRow dr in dt.Rows)
           {
               if (sb.Length > 0)
               {
                   sb.Append("<br />");
               }
               sb.Append(dr["Title"]);
           }
           return sb.ToString();
       }


        #region 发送邮件
       /// <summary>
       /// 给POTypeChange流程里的特殊组发送邮件
       /// </summary>
       /// <param name="sNO"></param>
       void SendPOTCMail(string sNO, string sWorkflowNumber)
       {
           List<string> listMail = GetMailFromEmailAccount();

           List<string> listPars = new List<string>();//设置发送mail主体内容参数
           listPars.Add(sNO);

           string title = "POTCMail";
           SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
           if (listMailTemplate == null)
           {
               CommonUtil.logError("Send POTCMail notice mail failed,Because mail template is null");
               return;
           }
           string bodyTemplate = listMailTemplate["Body"].AsString();
           if (bodyTemplate.IsNotNullOrWhitespace())
           {
               string sSubject = string.Format(listMailTemplate["Subject"].AsString(), sWorkflowNumber);
               POTCSendMailContent(sSubject, bodyTemplate, listPars, listMail);
           }
           else
           {
               CommonUtil.logError("Send POTCMail notice mail failed,Because mail template Body is null");
           }
       }

        /// <summary>
        /// 得到配置的邮箱
        /// </summary>
        /// <returns></returns>
       List<string> GetMailFromEmailAccount()
       {
           StringBuilder sb = new System.Text.StringBuilder();
           List<string> listMail = new List<string>();
           SPQuery query = new SPQuery();
            query.Query = @"<Where>
                                <Eq>
                                    <FieldRef Name='GroupName' />
                                    <Value Type='Text'>POTCMail</Value>
                                </Eq>
                            </Where>";
            SPListItemCollection splic = SPContext.Current.Web.Lists["EmailAccount"].GetItems(query);
            if (null == splic)
            {
                return listMail;
            }
            foreach (SPListItem item in splic)
            {
                listMail.Add(item["Title"] == null ? string.Empty : item["Title"].ToString());
            }
            return listMail;
       }

       /// <summary>
       /// 发送邮件，直接给 邮件里发，而不是以前的通过找到人的邮件
       /// </summary>
       /// <param name="subject"></param>
       /// <param name="bodyTemplate"></param>
       /// <param name="paramters"></param>
       /// <param name="emailList"></param>
       public static void POTCSendMailContent(string subject, string bodyTemplate, List<string> paramters, List<string> emailList)
       {
           ThreadPool.QueueUserWorkItem(delegate
           {
               foreach (string mail in emailList)
               {
                   if (string.IsNullOrEmpty(mail))
                   {
                       continue;
                   }
                   IMailService mailService = MailServiceFactory.GetMailService();
                   //paramters[0] = emp.DisplayName; //Set the display name
                   string body = string.Format(bodyTemplate, paramters.ToArray());
                   StringCollection sc = new StringCollection();
                   sc.Add(mail);
                   try
                   {
                       mailService.SendMail(subject, body, sc);
                   }
                   catch (Exception ex)
                   {
                       var mails = new StringBuilder();
                       mails.Append(string.Format("Mail Count: {0}\n", sc.Count));
                       foreach (var s in sc)
                       {
                           mails.Append(string.Format("Mail: {0}\n", s));
                           mails.Append(string.Format("Subject: {0}\n", subject));
                           mails.Append(string.Format("Body: {0}\n", body));
                       }
                       CommonUtil.logError(string.Format("Send email error: {0} \n. Details: {1}\n", ex.Message, mails.ToString()));
                   }
               }
           });
       }

        #endregion
    }
}
