using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Web;
using System.Configuration;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;


namespace CA.WorkFlow.Common.SendNoticeGRSRMail
{
    public class Common
    {
        public static string sSiteName = ConfigurationManager.AppSettings["SiteName"];
        static string sWebName = ConfigurationManager.AppSettings["WebName"].Trim();
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
                                
                                if (null!=userProfile["WorkEmail"].Value)
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
