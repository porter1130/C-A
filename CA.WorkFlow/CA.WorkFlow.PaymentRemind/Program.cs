using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using System.Data;

namespace CA.WorkFlow.PaymentRemind
{
    class Program
    {
        static void Main(string[] args)
        {
            Sendremindmail();
        }

        static void Sendremindmail()
        {
            DataTable dt = GetAvaliabledata();
            if (null == dt || dt.Rows.Count == 0)
            {
                return;
            }
            StringBuilder sbContent = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                string sPONO = dr["PONo"] == null ? string.Empty : dr["PONo"].ToString();
                string sComments = dr["Comments"] == null ? string.Empty : dr["Comments"].ToString();
                sbContent.Append(string.Format("PONo. {0}     {1}<br />",sPONO,sComments));
            }
            SPGroup group = Common.GetUserInGroup("wf_PaymentRemind");
            if(null!=group)
            {
                Common.EmptyLogFile();
                foreach (SPUser user in group.Users)
                {
                    if (user.IsSiteAdmin || user.Name == "System Account")
                    {
                        continue;
                    }
                    string sMail = Common.GetEmployeeMail(user.Name);
                    if (string.IsNullOrEmpty(sMail))
                    {
                        continue;
                    }
                    int i = user.LoginName.IndexOf("\\");
                    string sName = string.Empty;
                    if (i > -1)
                    {
                        sName = user.LoginName.Substring(i+2);
                    }

                    Common.SendMail(user.LoginName, sMail, sbContent.ToString());
                }
            }
        }

        /// <summary>
        /// 得到到期没有支付的数据。
        /// </summary>
        /// <returns></returns>
       static DataTable GetAvaliabledata()
        {
            DataTable dt = new DataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate { 
                using(SPSite site=new SPSite(Common.sSiteName))
                {
                    using(SPWeb web=site.OpenWeb(Common.sWebName))
                    {
                        SPQuery query = new SPQuery();
                        query.Query = GetSearchCalmel();

                        dt= web.Lists["PaymentInstallment"].GetItems(query).GetDataTable();
                    }
                }
            });
            return dt;
        }

        /// <summary>
        /// 得到 Camel查询条件：没有完成当次付款，有提醒，到了提醒期限，
        /// </summary>
        /// <returns></returns>
       static string GetSearchCalmel()
       {
           string sQury = string.Format(@"<Where>
                                              <And>
                                                 <And>
                                                    <Neq>
                                                       <FieldRef Name='IsPaid' />
                                                       <Value Type='Boolean'>1</Value>
                                                    </Neq>
                                                    <Eq>
                                                       <FieldRef Name='IsNeedRemind' />
                                                       <Value Type='Boolean'>1</Value>
                                                    </Eq>
                                                 </And>
                                                 <Leq>
                                                    <FieldRef Name='RemindDate' />
                                                    <Value Type='DateTime'>{0}</Value>
                                                 </Leq>
                                              </And>
                                           </Where>", DateTime.Now.ToString("yyyy-MM-dd"));
           return sQury;
       }
            
        
    }
}
