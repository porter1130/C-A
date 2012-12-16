using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Text;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace CA.SharePoint.WebControls.WebControls
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler1 : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //设置输出信息
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";
            
            //定义变量
            string strSPDept = context.Request["dept"].ToString();
            string strEmp = context.Request["user"].ToString();
            List<Employee> employees = new List<Employee>();
            Employee employee = null;
            SPList list = null;

            try
            {
                list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);
                var allDepts = new List<string>();
                if (strSPDept.Contains("Store"))
                {
                    foreach (SPListItem item in list.Items)
                    {
                        if (item["DisplayName"] == null)
                            continue;

                        //var strTempSPDept = item["DisplayName"].ToString().ToLower();
                        allDepts.Add(item["Title"].ToString().ToLower());
                    }
                    employees = UserProfileUtil.GetEmployeeFromSSPNotInDepts(allDepts.ToArray());
                }
                else
                {
                    foreach (SPListItem item in list.Items)
                    {
                        if (item["DisplayName"] == null)
                            continue;

                        var strTempSPDept = item["DisplayName"].ToString().ToLower();
                        if (strTempSPDept == strSPDept.ToLower()){
                            employees.AddRange(UserProfileUtil.GetEmployeeFromSSPByDept(item["Name"].ToString()));
                        }
                    }

                    //if (employees.Count == 0){
                    //    employees = UserProfileUtil.GetEmployeeFromSSPNotInDepts(allDepts.ToArray());
                    //}
                }

                //从用户列表中找出指定用户信息
                employee = employees.Find(new Predicate<Employee>(delegate(Employee emp){
                    return emp.DisplayName.Trim().ToLower() == strEmp.Trim().ToLower();
                }));
            }
            catch //(Exception ex)
            {
                return;
            }
            finally
            {
                //不管异常与否都显示用户详细信息结构
                StringBuilder str = new StringBuilder();
                str.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" runat=\"server\">");
                str.Append("<tr><th width=\"108\" valign=\"top\"><div id=\"projectthumnail\">");
                str.AppendFormat("<img width=\"100\" src=\"{0}\" style=\"vertical-align:top\" /></div></th>",
                    employee == null ? "" : employee.PhotoUrl);
                str.Append("<th  align=\"left\" valign=\"top\">");
                str.Append("<table width=\"96%\" border=\"0\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\">");
                str.AppendFormat("<tr><th width=\"15%\">Name:</th><th width=\"35%\" align=\"left\">{0}&nbsp;",
                    employee == null ? strEmp : employee.DisplayName);
                str.AppendFormat("</th><th width=\"15%\">Dept:</th><th width=\"35%\">{0}&nbsp;</th></tr>",
                    employee == null ? strSPDept : ReplaceMTM(employee.AllDepartment));
                str.AppendFormat("<tr><th>Cell:</th><th>{0}&nbsp;</th>",
                    employee == null ? "" : employee.Mobile);
                str.AppendFormat("<th>Phone:</th><th>{0}&nbsp;</th></tr>",
                    employee == null ? "" : employee.Phone);
                str.AppendFormat("<tr><th width=\"10%\">Email:</th><th colspan=\"3\"><a href=\"mailto:{0}\">{0}&nbsp;</a></th></tr>",
                    employee == null ? "" : employee.WorkEmail);
                str.AppendFormat("<tr><th>Title:</th><th colspan=\"3\">{0}&nbsp;</th></tr>",
                    employee == null ? "" : employee.Title);
                str.AppendFormat("</table></th></tr></table>",
                    employee == null ? "" : employee.More);
                context.Response.Write(str.ToString());
            }
        }

        private string ReplaceMTM(string strInput)
        {
            string strOutput = string.Empty;
            if (strInput.Contains("MTM"))
            {
                int len = strInput.IndexOf("MTM");
                strOutput = strInput.Substring(len, 3);
            }
            else
            {
                strOutput = strInput;
            }
            return strOutput;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
