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

namespace CA.WorkFlow.UI.NewOSP
{
    public class OSPCommon
    {


        #region 类成员

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
        }
        string mESSAGE;

        public string MESSAGE
        {
            get { return mESSAGE; }
            set { mESSAGE = value; }
        }


        public StringBuilder sErrorMsg=new StringBuilder();


        #endregion

        public const string Submit = "Submit";
        public const string Save = "Save";
        public const string End = "End";
       
        ///// <summary>
        ///// 工作流的动作
        ///// </summary>
        //public enum OSPActions
        //{
        //    Submit = "Submit",
        //    Save = "Save",
        //    End = "End"
        //}


        /// <summary>
        /// /Excel service 用于上传时的文档库名
        /// </summary>
        public static readonly string WorkflowDocumentLibrary = "Workflow Document Library";

        
        //  OSP Excel service 参数
        public static readonly string OSPPosition = "OSP_ExcelPos";
        public static readonly string OSPPK = "OSP_ExcelPrimary";
        public static readonly string OSPCol = "OSP_ExcelCols";


        /// <summary>
        /// 从SAP中得到数据集
        /// </summary>
        /// <param name="dtSearchCondition"></param>
        /// <returns></returns>
        public DataTable GetOSPInfoFromSAP(DataTable dtSearchCondition)
        {
            List<SapParameter> pareStyleNO=GetSapParaFormDT(dtSearchCondition);
            DataTable dt = CreateData();
            ISapExchange iOSP = SapExchangeFactory.GetOSP();
            List<object[]> listResult = iOSP.ExportDataToCa(pareStyleNO);

            for (int i = 0; i < listResult.Count; i++)
            {
                bool bl = (bool)listResult[i][2];
                if (bl)
                {
                    SapResult sr = (SapResult)listResult[i][1];
                    string sStyleNO = sr.OBJ_KEY;
                    string sMessage = sr.OBJ_OSPINFO.MESSAGE.Trim();
                    if (sMessage.Length > 0)
                    {
                        sErrorMsg.Append(string.Format("{0}:{1}",sStyleNO, FormatStr(sMessage)));
                        continue;
                    }

                    DataRow dr = dt.NewRow();
                    dr["Title"] = sStyleNO;
                    dr["WorkflowNumber"] = string.Empty;
                    dr["NewOSP"] =sr.OBJ_SYS;
                    dr["SubDiv"] = sr.OBJ_OSPINFO.SUB_DIV;
                    dr["Class"] = sr.OBJ_OSPINFO.CLASS;
                    dr["PONO"] = sr.OBJ_OSPINFO.PO;
                    dr["Qty"] = sr.OBJ_OSPINFO.QTY;
                    dr["OriginalOsp"] = sr.OBJ_OSPINFO.ORIGINAL_OSP;
                    dr["CurrentOMU"] = sr.OBJ_OSPINFO.CURRENT_OMU;
                    dr["CreatedBy"] = sr.OBJ_OSPINFO.CREATED_BY;
                    dr["PAD"] = sr.OBJ_OSPINFO.PAD;
                    dr["SAD"] = sr.OBJ_OSPINFO.SAD;
                    dr["GR"] = sr.OBJ_OSPINFO.GR;
                    //dr["NewOMU"] = sr.OBJ_OSPINFO.COST;
                    dr["Cost"] = sr.OBJ_OSPINFO.COST;
                    
                    string sAllocatedDate = sr.OBJ_OSPINFO.ALLOCATED_DATE;
                    DateTime dtDate=DateTime.Now;
                    if (DateTime.TryParse(sAllocatedDate, out dtDate))
                    {
                        sAllocatedDate = dtDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        sAllocatedDate = string.Empty;
                    }

                    dr["AllocatedDate"] = sAllocatedDate;
                    dr["IsSuccess"] = "0";
                    
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
            dt.Columns.Add("Title");//Style NO.
            dt.Columns.Add("WorkflowNumber");
            dt.Columns.Add("NewOSP");
            dt.Columns.Add("IsApproved");
            dt.Columns.Add("IsSuccess");
            dt.Columns.Add("SubDiv");
            dt.Columns.Add("Class");
            dt.Columns.Add("PONO");
            dt.Columns.Add("Qty");
            dt.Columns.Add("OriginalOsp");
            dt.Columns.Add("CurrentOMU");
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("PAD");
            dt.Columns.Add("SAD");
            dt.Columns.Add("GR");
            dt.Columns.Add("AllocatedDate");
            dt.Columns.Add("Cost");
            dt.Columns.Add("NewOMU");
            dt.Columns.Add("OMUReduction");
            dt.Columns["IsSuccess"].DefaultValue = false;
            dt.Columns["IsApproved"].DefaultValue = false;
            return dt;
        }

        /// <summary>
        /// 得到更新到SAP后的结果集。
        /// </summary>
        /// <param name="dtUpdate"></param>
        /// <returns></returns>
        public DataTable UpdateOSPPrice(DataTable dtUpdate)
        {
            List<SapParameter> listUpdateSapPar = GetSapParaFormDT(dtUpdate);
            ISapExchange iOSP = SapExchangeFactory.GetOSPMod();
            List<object[]> listResult = iOSP.ExportDataToCa(listUpdateSapPar);
            DataTable dtReturn = new DataTable();
            dtReturn = GetUpdatedReturnDT();
            for (int i = 0; i < listResult.Count; i++)
            {
                bool bl = (bool)listResult[i][2];
                DataRow dr = dtReturn.NewRow();

                if (bl)
                {
                    SapResult sr = (SapResult)listResult[i][1];
                    string sStyleNO = sr.OBJ_KEY;
                    dr["StyleNO"] = sStyleNO;
                    if (sr.OBJ_OSPINFO.IsSuccess == true)
                    {
                        dr["Status"] = 1;
                    }
                    else
                    {
                        dr["ErrorInfo"] = "Unknow error";
                        CommonUtil.logError("OSP :Update NewPrice to SAP Error,Unknow error at line 240  CA.WorkFlow.UI.OSP.OSPCommon.cs");
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
                        string sStyleNO = sr.OBJ_KEY;
                        dr["StyleNO"] = sStyleNO;
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
            dt.Columns.Add("StyleNO");
            dt.Columns.Add("Status");
            dt.Columns.Add("ErrorInfo");
            return dt;
        }


        /// <summary>
        /// 得到和SAP交互时的参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        List<SapParameter> GetSapParaFormDT(DataTable dt)
        {
            List<SapParameter> listPar = new List<SapParameter>();
            foreach (DataRow dr in dt.Rows)
            {
                SapParameter par = new SapParameter();
                par.SapNumber = dr["StyleNO"].ToString();
                par.PaymentCond = dr["NewOSP"].ToString();
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
            WorkFlowUtil.BatchDeleteItems("OSPItems", query);
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
            dt = SPContext.Current.Web.Lists["OSPItems"].GetItems(query).GetDataTable();
            return dt;
       }

       /// <summary>
       /// 修改PO的审批状态及成功更新到SAP中
       /// </summary>
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
                   SPListItemCollection splic = web.Lists["OSPItems"].GetItems(query);
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
       /// 修改PO的审批状态及成功更新到SAP中
       /// </summary>
       public void UpdateItemSapStatus(string sWorkflowNo, string sStyleNo, bool isApproved, bool isSuccess)
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
                                               </Where>", sStyleNo, sWorkflowNo);
                   SPListItemCollection splic = web.Lists["OSPItems"].GetItems(query);
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
                   SPListItemCollection splic = web.Lists["OSPItems"].GetItems(query);
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

       public static bool IsInGroup(string account, string group)
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

       public static bool isAdmin()
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
       public void SendMail(string sApplicant, string sStatus, string sOSPNO, string sApproverName)
       {
           List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApplicant);
           List<string> listPars = new List<string>();//设置发送mail主体内容参数
           listPars.Add(employeReceiver[0].DisplayName);
           listPars.Add(sOSPNO);
           listPars.Add(sStatus);
           listPars.Add(UserProfileUtil.GetEmployeeEx(sApproverName).DisplayName);

           string title = "OSPChangeRequest";
           SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
           if (listMailTemplate == null)
           {
               CommonUtil.logError("Send OSP notice mail failed,Because mail template is null");
               return;
           }
           string bodyTemplate = listMailTemplate["Body"].AsString();
           if (bodyTemplate.IsNotNullOrWhitespace())
           {
               string sTitle = listMailTemplate["Subject"].AsString();
               WorkFlowUtil.SendMail(sTitle, bodyTemplate, listPars, employeReceiver);
           }
           else
           {
               CommonUtil.logError("Send OSP notice mail failed,Because mail template Body is null");
           }
       }


       /// <summary>
       /// 查找提交人的DMM级用户等级为L-4或L-5的
       /// </summary>
       /// <param name="emp"></param>
       /// <returns></returns>
       public static Employee GetDMMApprover(Employee emp)
       {
           Employee approver = null;
           approver = string.IsNullOrEmpty(emp.ManagerID) ? null : UserProfileUtil.GetEmployeeByProp("EmployeeId", emp.ManagerID);

           if (approver == null)
           {
               return null;
           }
           var jobLevel = approver != null ? approver.JobLevel : "L-0";

           var start = jobLevel.IndexOf("-") + 1; //L-3
           var length = jobLevel.Length - start;
           var level = Convert.ToInt32(jobLevel.Substring(start, length));
           while (level > 5)
           {
               approver = UserProfileUtil.GetEmployeeByProp("EmployeeId", approver.ManagerID);
               jobLevel = approver != null ? approver.JobLevel : "L-0";
               if (jobLevel.Trim().Length == 0)
               {
                   return null;
               }
               length = jobLevel.Length - start;
               level = Convert.ToInt32(jobLevel.Substring(start, length));
           }

           if (level < 4)
           {
               return null;
           }
           return approver;
       }



    }
}
