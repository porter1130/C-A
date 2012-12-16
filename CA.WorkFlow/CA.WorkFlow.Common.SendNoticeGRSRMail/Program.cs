using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;


namespace CA.WorkFlow.Common.SendNoticeGRSRMail
{
    class Program
    {
      static string sSiteName = ConfigurationManager.AppSettings["SiteName"].Trim();
      static string sWebName = ConfigurationManager.AppSettings["WebName"].Trim();

        static void Main(string[] args)
        {
           Common.EmptyLogFile();
           SendMailForPO();
        }

        static void SendMailForPO()                                   
        {
            DataTable dt = new DataTable();
            dt = GetUnRecievedPO();
            if (null == dt)
            {
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                string sPoNO = dr["PONO"] == null ? "" : dr["PONO"].ToString();
                string sPOAuthor = dr["Author"] == null ? "" : dr["Author"].ToString();
                if (sPoNO.Length == 0)
                {
                    Common.WriteErrorLog(string.Concat("PONO  is empty PO: ", sPoNO, "  sApplicant "));
                    continue;
                }
                string sCCEmailAcount = Common.GetEmployeeMail(sPOAuthor);///得到 ToPO者的邮箱帐号，并给它CC一份。

                List<string> listAccount = new List<string>();
                listAccount = GetAccountByPO(sPoNO);
                foreach (string sAccount in listAccount)
                {
                    string sEmailAccount = Common.GetEmployeeMail(sAccount);
                    if (sEmailAccount.Length == 0)
                    {
                        Common.WriteErrorLog(string.Concat("PONO: " + sPoNO, ", Can't find email account for", sAccount));
                        continue;
                    }
                   string sName= GetNameByAccount(sAccount);
                   SendMail(sName, sEmailAccount, sCCEmailAcount, sPoNO); 
                }
            }
        }
        static void SendMail(string sName, string sEmailAccount, string sCCEmailAcount, string sPoNO)
        {
            string sSmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
            string sSendAccount = ConfigurationManager.AppSettings["SendAccount"].ToString();
            string sSendPwd = ConfigurationManager.AppSettings["SendPwd"].ToString();
            string sSendTitle = ConfigurationManager.AppSettings["SendTitle"].ToString();
            string sSendBody = ConfigurationManager.AppSettings["SendBody"].ToString();

            string sSubJect = string.Format(sSendTitle, sPoNO);
            string sBody = string.Concat("Dear,", sName, "<br/>", string.Format(sSendBody, sPoNO));

            MailMessage mailtoSent = new MailMessage();
            mailtoSent.From = new MailAddress(sSendAccount);
            mailtoSent.Subject = sSubJect;
            mailtoSent.Body = sBody;
            mailtoSent.IsBodyHtml = true;
            mailtoSent.To.Add(sEmailAccount);
            if (!string.IsNullOrEmpty(sCCEmailAcount) && sCCEmailAcount != sEmailAccount)
            {
                mailtoSent.CC.Add(sCCEmailAcount);
            }

            SmtpClient mailServer = new SmtpClient(sSmtpServer);
            mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
            try
            {
                mailServer.Send(mailtoSent);
                Common.WriteErrorLog(string.Concat("Send mail to ", sEmailAccount, " And CC To ", sCCEmailAcount, ", for PO:", sPoNO, " Sucess! "));
            }
            catch(Exception e)
            {
                Common.WriteErrorLog("发送邮件失败！"+e.ToString());
            }
            
            
        }
        public static string GetApplicantAccount(string applicantStr)
        {
            var accountPattern = @"^.*?\((.*?)\)$";
            var accountExpression = new Regex(accountPattern);
            return accountExpression.Match(applicantStr).Groups[1].Value;
        }
        /// <summary>
        /// 得到超过指定天数，没有做EWF收货的PO信息
        /// </summary>
        static DataTable GetUnRecievedPO()
        {
            DataTable dtReturn = new DataTable();
            string sDate = ConfigurationManager.AppSettings["Date"].Trim();
            string sBeginDate = ConfigurationManager.AppSettings["BeginDate"].Trim();
            DateTime deliveryDate;
            if (string.IsNullOrEmpty(sDate))
            {
                    deliveryDate = DateTime.Now.AddDays(-15);
            }
            else             
            {
                int iDay = 0;
                if (int.TryParse(sDate, out iDay))
                {
                    deliveryDate = DateTime.Now.AddDays(-iDay);
                }
                else
                {
                    deliveryDate = DateTime.Now.AddDays(-15);
                }
            }
            DateTime beginDate = DateTime.Now;
            if (sBeginDate.Length > 0 && !DateTime.TryParse(sBeginDate, out beginDate))
            {
                Common.WriteErrorLog(string.Concat("DateTime Convert error in App.config at Date Node"));
                return null;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQuery = string.Empty;
                        if (sBeginDate.Length == 0)
                        {
                            sQuery = string.Format(@"<Where>
                                              <And>
                                                <Neq>
                                                    <FieldRef Name='IsRecieved' />
                                                    <Value Type='Boolean'>1</Value>
                                                </Neq>
                                                 <Leq>
                                                    <FieldRef Name='Created' />
                                                    <Value Type='DateTime'>{0}</Value>
                                                 </Leq>
                                              </And>
                                           </Where>", deliveryDate.ToString("yyyy-MM-dd"));//
                        }
                        else
                        {
                            sQuery = string.Format(@"<Where>
                                                            <And>
                                                                <And>
                                                                    <Neq>
                                                                        <FieldRef Name='IsRecieved' />
                                                                        <Value Type='Boolean'>1</Value>
                                                                    </Neq>
                                                                    <Leq>
                                                                        <FieldRef Name='Created' />
                                                                        <Value Type='DateTime'>{0}</Value>
                                                                    </Leq>
                                                                </And>
                                                                <Geq>
                                                                    <FieldRef Name='Created' />
                                                                    <Value Type='DateTime'>{1}</Value>
                                                                </Geq>
                                                            </And>
                                                        </Where>", deliveryDate.ToString("yyyy-MM-dd"), beginDate.ToString("yyyy-MM-dd"));
                        }
                        query.Query = sQuery;/// string.Format(sQuery, dtNow);
                        DataTable dt = web.Lists["PurchaseOrderItems"].GetItems(query).GetDataTable();
                        if (null != dt && dt.Rows.Count > 0)
                        {
                            dtReturn = GetAvaliblePO(dt);
                        }

                    }
                }
            });
            return dtReturn;
        }

        /// <summary>
        /// 只得可操作的PO号
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static DataTable GetAvaliblePO(DataTable dt)
        {
            //var linqResult = (from dr in dt.AsEnumerable()
            //                  select dr["Title"]).Distinct();


            var linqResult = from dr in dt.AsEnumerable()
                             group dr by new { Title = dr["Title"], Author = dr["Author"] } into g
                             select new {
                                 Title=g.Key.Title,
                                 Author=g.Key.Author
                             };
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("PONO");
            dtReturn.Columns.Add("Author");

            foreach (var item in linqResult)
            {
                if (item.Title.ToString().EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                DataRow drNew = dtReturn.NewRow();
                drNew["PONO"] = item.Title;
                drNew["Author"] = item.Author;
                dtReturn.Rows.Add(drNew);
            }
            return dtReturn;
        }
        /// <summary>
        /// 得到po中所能做GRSR的账号
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        static List<string> GetAccountByPO(string sPONO)
        {
            string sSiteName = ConfigurationManager.AppSettings["SiteName"].Trim();
            string sWebName = ConfigurationManager.AppSettings["WebName"].Trim();
            //string sAccount = string.Empty;

            List<string> listAccount = new List<string>();

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQuery = string.Empty;
                        sQuery = string.Format(@"<Where>
                                                    <And>
                                                        <Contains>
                                                            <FieldRef Name='PONumber' />
                                                            <Value Type='Text'>{0}</Value>
                                                        </Contains>
                                                        <Neq>
                                                            <FieldRef Name='IsReceived' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Neq>
                                                    </And>
                                                </Where>", sPONO);

                        query.Query = sQuery;
                        SPListItemCollection splic = web.Lists["PurchaseRequestItems"].GetItems(query);

                        if (null != splic && splic.Count > 0)
                        {
                            List<string> listCenter = new List<string>();
                            string sAuthor = string.Empty;
                            foreach (SPListItem item in splic)
                            {
                                if (null != item["CostCenter"])
                                {
                                    string sCostCenter = item["CostCenter"].ToString();
                                    if (sCostCenter.IndexOf("S", StringComparison.InvariantCultureIgnoreCase) == 0)//Store申请
                                    {
                                        if (listCenter.Contains(sCostCenter))
                                        {
                                            continue;
                                        }
                                        listCenter.Add(sCostCenter);
                                    }
                                    else //HO
                                    {
                                        if (null != item["Author"])
                                        {
                                            if (sAuthor.Length == 0)
                                            {
                                                string sPRNO = splic[0]["Title"].ToString();
                                                string sApplicantAccount = GetPRApplicantAccount(sPRNO);///Title
                                                if (sApplicantAccount.Trim().Length > 0)
                                                {
                                                    sAuthor = sApplicantAccount;
                                                }
                                                else
                                                {
                                                    Common.WriteErrorLog("Can not find Applicant for " + sPRNO);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(sAuthor))//HO申请的PO中有HO亲自去收货的情况,收货人就为该HO用户
                            {
                                if (!listAccount.Contains(sAuthor))
                                {
                                    listAccount.Add(sAuthor);
                                }
                            }

                            if (listCenter.Count > 0)// HO申请的PO数据中有为门店申请的数据 ,则收货人为门店用户
                            {
                                List<string> listCostCenterAccounts = new List<string>();
                                listCostCenterAccounts = GetAccountFromCostCenter(listCenter);
                                foreach (string sAccount in listCostCenterAccounts)
                                {
                                    if (listAccount.Contains(sAccount))
                                    {
                                        continue;
                                    }
                                    listAccount.Add(sAccount);
                                }
                            }
                        }
                    }
                }
            });
            return listAccount;
        }

        /// <summary>
        /// 得到CosterCenter集合中所对应的帐号集合
        /// </summary>
        /// <param name="listCostCenter"></param>
        /// <returns></returns>
        static List<string> GetAccountFromCostCenter(List<string> listCostCenter)
        {
            List<string> listAccount = new List<string>();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQueryInner = GetOrCamle(listCostCenter);
                        string sQueryFormate = string.Format("<Where>{0}</Where>", sQueryInner);
                        query.Query = sQueryFormate;
                        SPListItemCollection splic = web.Lists["Cost Centers"].GetItems(query);
                        if (null != splic)
                        {
                            foreach (SPListItem item in splic)
                            {
                                if (null != item["ManagerAccount"])
                                {
                                    string sManagerAccount = item["ManagerAccount"].ToString();
                                    if (listAccount.Contains(sManagerAccount))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        listAccount.Add(sManagerAccount);
                                    }
                                }
                            }
                        }
                    }
                }
            });
            return listAccount;
        }

        /// <summary>
        /// 得到拼接而成的Camle
        /// </summary>
        /// <param name="listStr"></param>
        /// <returns></returns>
        static string GetOrCamle(List<string> listStr)
        {
            string sCamle = string.Empty;
            if (listStr.Count == 1)
            {
                sCamle = string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", listStr[0]);
                return sCamle.ToString();
            }
            for (int i = 0; i < listStr.Count; i++)
            {
                string sOrCondition = string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", listStr[i]);
                sCamle += sOrCondition;
                if (i >=1)
                {
                    sCamle = string.Format("<Or>{0}</Or>", sCamle);
                }
            }
            return sCamle.ToString();
        }
        
        /// <summary>
        /// 得到创建者名字
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
       static string GetUserNameFromAuthor(SPListItem item)
        {
            SPFieldUser spfu = item.Fields["Created By"] as SPFieldUser;
            SPFieldUserValue spfuv = spfu.GetFieldValue(item["Author"].ToString()) as SPFieldUserValue;
            string sUserName = spfuv.User.Name;
            return sUserName;
        }

        /// <summary>
       /// 得到Applicant中的帐号
        /// </summary>
        /// <param name="sPRNO"></param>
        /// <returns></returns>
       static string GetPRApplicantAccount(string sPRNO)
       {
           string sApplicantAccount = string.Empty;
           SPSecurity.RunWithElevatedPrivileges(delegate
           {
               using (SPSite site = new SPSite(sSiteName))
               {
                   using (SPWeb web = site.OpenWeb(sWebName))
                   {
                       SPQuery query = new SPQuery();
                       string sQuery = string.Empty;
                       sQuery = string.Format(@"<Where>
                                                    <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </Where>", sPRNO);
                       query.Query = sQuery;
                       SPListItemCollection splic= web.Lists["Purchase Request Workflow"].GetItems(query);
                       if (null != splic && splic.Count > 0)
                       { 
                           if(null!=splic[0]["Applicant"])
                           {
                              string sApplicant = splic[0]["Applicant"].ToString();
                              sApplicantAccount=GetApplicantAccount(sApplicant);
                           }
                        
                       }
                   }
               }
           });
           return sApplicantAccount;
       }

        /// <summary>
        /// 得到帐号中的用户名 
        /// </summary>
        /// <param name="sAccount"></param>
        /// <returns></returns>
       static string GetNameByAccount(string sAccount)
       {
           string sName = string.Empty;
           if (string.IsNullOrEmpty(sAccount))
           {
               return sName;
           }
           else
           {
               int iIndex = sAccount.IndexOf("\\");
               if (iIndex > 0)
               {
                  sName= sAccount.Substring(iIndex+1);
               }
               return sName;
           }
       }
    }
}
