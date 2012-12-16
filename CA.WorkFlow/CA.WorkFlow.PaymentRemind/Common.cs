using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using Microsoft.SharePoint;
using System.Configuration;
using Microsoft.Office.Server.UserProfiles;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data;

namespace CA.WorkFlow.PaymentRemind
{
    public class Common
    {
        
        public static string sSiteName = ConfigurationManager.AppSettings["SiteName"];
        public static string sWebName = ConfigurationManager.AppSettings["WebName"].Trim();
        static string sLogFile = "Log.txt";
        /// <summary>
        /// 得到EmployeeMail
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public static string GetEmployeeMail(string userAccount)//Employee
        {
            string sMail = string.Empty;
            if (string.IsNullOrEmpty(userAccount))
            {
                return sMail;
            }
            try
            {
                //从SSP里面取用户。
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(sSiteName))
                    {
                        using (SPWeb web = site.OpenWeb(sWebName))
                        {
                            Microsoft.Office.Server.ServerContext context = Microsoft.Office.Server.ServerContext.GetContext(site);
                            UserProfileManager profileManager = new UserProfileManager(context);
                            if (profileManager.UserExists(userAccount))
                            {
                                UserProfile userProfile = profileManager.GetUserProfile(userAccount);

                                if (null != userProfile["WorkEmail"].Value)
                                {
                                    sMail = userProfile["WorkEmail"].Value.ToString().Trim();
                                }
                                else
                                {
                                    WriteErrorLog(string.Format("共享服务(SSP)中没有为用户：{0}的配置邮件信息，可能是共享服务(SSP)没有和AD同步所致，请联系IT管理员联系！", userAccount));
                                }
                            }
                            else
                            {
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

            return sMail;
        }


       public static void SendMail(string sName, string sEmailAccount, string sContent)
        {
            string sSmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
            string sSendAccount = ConfigurationManager.AppSettings["SendAccount"].ToString();
            string sSendPwd = ConfigurationManager.AppSettings["SendPwd"].ToString();
            DataTable dt = GetEmailTemplateByTitle("PaymentRemind");
            if (null == dt || dt.Rows.Count == 0)
            {
                WriteErrorLog("Send PaymentRemind notice mail failed,Because mail template is null");
                return;
            }
            string bodyTemplate = dt.Rows[0]["Body"].ToString();
            string sSubJect = dt.Rows[0]["Subject"].ToString();
            string sBody = string.Format(bodyTemplate, sName, sContent);

            MailMessage mailtoSent = new MailMessage();
            mailtoSent.From = new MailAddress(sSendAccount);
            mailtoSent.Subject = sSubJect;
            mailtoSent.Body = sBody;
            mailtoSent.IsBodyHtml = true;
            mailtoSent.To.Add(sEmailAccount);

            SmtpClient mailServer = new SmtpClient(sSmtpServer);
            mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
            try
            {
                mailServer.Send(mailtoSent);
                Common.WriteErrorLog(string.Concat("Send mail to ", sEmailAccount, " Sucess! "));
            }
            catch (Exception e)
            {
                Common.WriteErrorLog("发送邮件失败！" + e.ToString());
            }
        }


/*
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sApplicant"></param>
        /// <param name="sContent"></param>
        /// <param name="sApproverName"></param>
        public static void SendMail(string sApplicant, string sContent)
        {
            List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApplicant);
            List<string> listPars = new List<string>();//设置发送mail主体内容参数
            listPars.Add(employeReceiver[0].DisplayName);
            listPars.Add(sContent);

            string title = "PaymentRemind";
            SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
            if (listMailTemplate == null)
            {
                WriteErrorLog("Send PaymentRemind notice mail failed,Because mail template is null");
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
                WriteErrorLog("Send Payment Remind notice mail failed,Because mail template Body is null");
            }
        }
*/
        /// <summary>
        /// 得到用户组里的用户
        /// </summary>
        /// <param name="sgroupName"></param>
        /// <returns></returns>
        public static SPGroup GetUserInGroup(string sgroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
             {
                 using (SPSite site = new SPSite(sSiteName))
                 {
                     using (SPWeb web = site.OpenWeb(sWebName))
                     {
                         group = web.SiteGroups[sgroupName];
                     }
                 }
             });
            return group;
        }

        /// <summary>
        /// 得到发送mail模板
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static DataTable GetEmailTemplateByTitle(string title)
        {
            DataTable dt = new DataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(Common.sSiteName))
                {
                    using (SPWeb web = site.OpenWeb(Common.sWebName))
                    {
                        SPQuery query = new SPQuery();
                        string sQury = string.Format(@" <Where>
                                                              <Eq>
                                                                 <FieldRef Name='Title' />
                                                                 <Value Type='Text'>{0}</Value>
                                                              </Eq>
                                                           </Where>", title);
                        query.Query = sQury;
                        dt = web.Lists["EmailTemplate"].GetItems(query).GetDataTable();
                    }
                }
            });
            return dt;
        }



        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="sErrorInfo"></param>
        public static void WriteErrorLog(string sErrorInfo)
        {
            StreamWriter sw = File.AppendText(sLogFile);
            string sErrorFormate = string.Format("{0}: {1}\r\n-----------------------------------------------------------\r\n", DateTime.Now.ToString(), sErrorInfo);

            sw.WriteLine(sErrorFormate);
            sw.Flush();
            sw.Dispose();
        }


        /// <summary>
        ///  清空日志
        /// </summary>
        public static void EmptyLogFile()
        {
            if (File.Exists(sLogFile))
            {
                StreamWriter sw = new StreamWriter(sLogFile, false);
                sw.Write("");
                sw.Flush();
                sw.Dispose();
            }
        }
    }
}
