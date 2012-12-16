using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAP.Middleware.Exchange;

using System.Data;
using System.Text;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;

namespace CA.WorkFlow.UI.PADChangeRequest
{

    public class SapCommonPADChangeRequest
    {
        private string _pad;
        public string PAD
        {
            get
            {
                return _pad;
            }
            set
            {
                _pad = value;
            }
        }
        private bool _delivered;
        public bool Delivered
        {
            get
            {
                return _delivered;
            }
            set
            {
                _delivered = value;
            }
        }
        private string _valueforstory;
        public string ValueForStory
        {
            get { return _valueforstory; }
            set {  _valueforstory = value; }
        }
        private string _padyear;
        public string PADyear
        {
            get { return _padyear; }
            set { _padyear = value; }
        }
        private string _padweek;
        public string PADweek
        {
            get { return _padweek; }
            set { _padweek = value; }
        }
        private string _sadyear;
        public string SADyear
        {
            get { return _sadyear; }
            set { _sadyear = value; }
        }
        private string _sadweek;
        public string SADweek
        {
            get { return _sadweek; }
            set { _sadweek = value; }
        }
        private string _osp;
        public string OSP
        {
            get { return _osp; }
            set { _osp = value; }
        }
        private string _omu;
        public string OMU
        {
            get { return _omu; }
            set { _omu = value; }
        }
        private string _suppliername;
        public string SupplierName
        {
            get { return _suppliername; }
            set { _suppliername = value; }
        }
        private string _error;
        public string ErrorMsg
        {
            get { return _error; }
            set { _error = value; }
        }

        private string sPOQTY;

        public string SPOQTY
        {
            get { return sPOQTY; }
            set { sPOQTY = value; }
        }
        private string sTYLENUMBER;

        public string STYLENUMBER
        {
            get { return sTYLENUMBER; }
            set { sTYLENUMBER = value; }
        }

        public bool SapSearchPAD(string ponumber)
        {
            bool flag = false;
            //ISapExchange tec10 = SapExchangeFactory.GetPurchaseOrderQuery();
            //List<object[]> result10 = tec10.ExportDataToCa(new List<SapParameter>() { new SapParameter() { SapNumber = ponumber } });

            ISapExchange tec10 = SapExchangeFactory.GetPurchaseOrderQuery();
            List<object[]> result10 = tec10.ExportDataToCa(new List<SapParameter>() { new SapParameter() { SapNumber = ponumber } });

            for (int i = 0; i < result10.Count; i++)
            {
                bool bl = (bool)result10[i][2];
                if (bl)
                {
                    SapResult sr = (SapResult)result10[i][1];
                    if (sr.OBJ_POINFO.STATUS != "")
                    {
                        flag = true;
                        this.PAD = sr.OBJ_POINFO.DATE;
                        this.OMU = sr.OBJ_POINFO.DMBTROMU;
                        this.OSP = sr.OBJ_POINFO.DMBTROSP;
                        this.PADweek = sr.OBJ_POINFO.WEEKP;
                        this.PADyear = sr.OBJ_POINFO.YEARP;
                        this.SADweek = sr.OBJ_POINFO.WEEKS;
                        this.SADyear = sr.OBJ_POINFO.YEARS;
                        this.SupplierName = sr.OBJ_POINFO.NAME;
                        this.ValueForStory = sr.OBJ_POINFO.ATWRT;
                        this.sPOQTY = sr.OBJ_POINFO.POQTY;
                        this.sTYLENUMBER = sr.OBJ_POINFO.STYLENUMBER;
                        if (sr.OBJ_POINFO.STATUS.ToString() == "Y")
                        {
                            this.Delivered = true;
                        }
                        else
                        {
                            this.Delivered = false;
                        }
                    }

                }
                else
                {
                    if (result10[i][1] is string)
                    {
                       this.ErrorMsg = result10[i][1].ToString();
                    }
                    else
                    {
                        SapResult sr = (SapResult)result10[i][1];
                        for (int j = 0; j < sr.RETURN_LIST.Count; j++)
                        {
                            this.ErrorMsg += sr.RETURN_LIST[j].MESSAGE;
                        }
                    }
                }
            }
            return flag;
        }



        public bool SapUpdatePAD(string ponumber, string pad)
        {
            bool flag = false;
            ISapExchange tec9 = SapExchangeFactory.GetPurchaseOrderMod();
            List<object[]> result9 = tec9.ExportDataToCa(new List<SapParameter>() { new SapParameter() { SapNumber = ponumber, DocDate = pad } });
            for (int i = 0; i < result9.Count; i++)
            {
                bool bl = (bool)result9[i][2];
                if (bl)
                {
                    SapResult sr = (SapResult)result9[i][1];
                    if (sr.OBJ_POINFO.STATUS == "Y")
                    {
                        flag = true;

                    }
                }
                else
                {
                    if (result9[i][1] is string)
                    {
                        this.ErrorMsg = result9[i][1].ToString();
                    }
                    else
                    {
                        SapResult sr = (SapResult)result9[i][1];
                        for (int j = 0; j < sr.RETURN_LIST.Count; j++)
                        {
                            this.ErrorMsg += sr.RETURN_LIST[j].MESSAGE;
                        }
                    }
                }
               
            }

            return flag;
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


        /// <summary>
        /// 修改PO的SAP更新状态
        /// </summary>
       public void UpdateItemSapStatus(string sPONO,string sWorkFlowNO)
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
                                                                    <FieldRef Name='PONumber' />
                                                                    <Value Type='Text'>{1}</Value>
                                                                </Eq>
                                                            </And>
                                       </Where>",sWorkFlowNO, sPONO);
                        SPListItemCollection splic = web.Lists["PADChangeRequestItems"].GetItems(query);
                        foreach (SPListItem listItem in splic)
                        {
                            listItem["IsSuccess"] = true;
                            listItem.Update();
                        }
                    }
                }
            }



       public void SendMail(string sApplicant,string sStatus,string sOSPNO,string sApproverName)
       {
           List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApplicant);
           List<string> listPars=new List<string>();//设置发送mail主体内容参数
           listPars.Add(employeReceiver[0].DisplayName);
           listPars.Add(sOSPNO);
           listPars.Add(sStatus);
           listPars.Add(UserProfileUtil.GetEmployeeEx(sApproverName).DisplayName);

           string title = "PADChangeRequest";
           SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
           if (listMailTemplate == null)
           {
               CommonUtil.logError("Send PAD notice mail failed,Because mail template is null");
               return;
           }
           //string sMailBody = string.Format("Dear {0},\n\n\r Your PAD change request:{1} was {2} by {0}.");
           string bodyTemplate = listMailTemplate["Body"].AsString();
           if (bodyTemplate.IsNotNullOrWhitespace())
           {
               string sTitle = listMailTemplate["Subject"].AsString();
               WorkFlowUtil.SendMail(sTitle, bodyTemplate, listPars, employeReceiver);
           }
           else
           {
               CommonUtil.logError("Send PAD notice mail failed,Because mail template Body is null");
           }
       }

       /// <summary>
       /// 查找提交人的DMM级用户等级为L-4或L-5的
       /// </summary>
       /// <param name="emp"></param>
       /// <returns></returns>
       public static Employee GetApproverByLevelPAD(Employee emp)
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
               length = jobLevel.Length - start;
               level = Convert.ToInt32(jobLevel.Substring(start, length));
           }

           if (level < 4)
           {
               return null;
           }
           return approver;
       }
       
       public static int GetLevel(string sJobLevel)
       {
           int iLevel = 0;
           string[] sBDLevelArr = sJobLevel.Split('-');
           if (sBDLevelArr[1] != null)
           {
               int.TryParse(sBDLevelArr[1], out iLevel);
           }
           return iLevel;
       }

    }
}