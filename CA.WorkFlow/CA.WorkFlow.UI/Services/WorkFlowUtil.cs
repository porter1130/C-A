using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using CA.SharePoint;
using CA.SharePoint.Utilities.Common;
using CodeArt.SharePoint.CamlQuery;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics;
using QuickFlow;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using QuickFlow.Core;
using System.Reflection;
using System.Web;

namespace CA.WorkFlow.UI
{
    public static class WorkFlowUtil
    {
        public static SPFieldUserValueCollection GetApproversValue()
        {
            SPListItem item = SPContext.Current.ListItem;

            SPFieldUserValueCollection col = item["Approvers"] as SPFieldUserValueCollection;

            if (col != null)
            {
                SPUser user = SPContext.Current.Web.CurrentUser;
                SPFieldUserValue value = new SPFieldUserValue(SPContext.Current.Site.RootWeb, user.ID, user.Name);

                bool IsExist = false;
                foreach (SPFieldUserValue v in col)
                {
                    if (v.LookupId == value.LookupId)
                    {
                        IsExist = true;
                        break;
                    }
                }
                if (!IsExist)
                {
                    col.Add(value);
                }
            }
            else
            {
                SPUser user = SPContext.Current.Web.CurrentUser;
                SPFieldUserValue value = new SPFieldUserValue(SPContext.Current.Site.RootWeb, user.ID, user.Name);
                col = new SPFieldUserValueCollection();
                col.Add(value);
            }

            return col;
        }

        public static int CreateWorkFlowNumber(string workflowName)
        {
            int nNum = 1;
            CA.SharePoint.ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.WorkFlowNumber.ToString());

            QueryField field = new QueryField("Title");

            SPListItemCollection items = sps.Query(list, field.Equal(workflowName), 1, null);
            if (items != null && items.Count > 0)
            {
                SPListItem item = list.GetItemById(items[0].ID);
                nNum = Convert.ToInt32(items[0]["Number"]) + 1;
                item["Number"] = Convert.ToDouble(nNum);
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }
            else
            {
                SPListItem item = list.Items.Add();
                item["WorkFlowName"] = workflowName;
                item["Number"] = nNum;
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }

            return nNum;
        }

        public static Employee GetEmployeeApprover(Employee emp)
        {
            Employee approver = null;
            try
            {
                emp = UserProfileUtil.GetEmployee(emp.Manager);
                do
                {
                    if (emp.ApproveRight)
                    {
                        approver = emp;
                    }
                    else
                    {
                        emp = UserProfileUtil.GetEmployee(emp.Manager);
                    }
                }
                while ((approver == null) && (emp != null));
            }
            catch (Exception ex)
            {
                approver = null; ;
            }

            return approver;
        }

        //20110914
        //
        public static Employee GetNextApprover(string account)
        {
            var emp = UserProfileUtil.GetEmployeeEx(account);
            return emp != null ? GetNextApprover(emp) : null;
        }

        //20111126为Construction部门特殊处理
        //
        public static Employee GetNextConstructionApprover(string account)
        {
            var emp = UserProfileUtil.GetEmployeeEx(account);
            return emp != null ? GetNextConstructionApprover(emp) : null;
        }

        //20110913
        //
        public static Employee GetNextApprover(Employee emp)
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

            while (level > 7)
            {
                approver = UserProfileUtil.GetEmployeeByProp("EmployeeId", approver.ManagerID);
                jobLevel = approver != null ? approver.JobLevel : "L-0";
                length = jobLevel.Length - start;
                level = Convert.ToInt32(jobLevel.Substring(start, length));
            }

            return approver;
        }

        public static Employee GetApproverIgnoreRight(Employee emp)
        {
            Employee approver = null;
            approver = string.IsNullOrEmpty(emp.ManagerID) ? null : UserProfileUtil.GetEmployeeByProp("EmployeeId", emp.ManagerID);
            return approver;
        }
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
            return approver;
        }

        public static bool GetApproverIsLastPAD(Employee emp)
        {
            bool flag = false;
            if (emp == null)
            {
                return flag;
            }
            var jobLevel = emp != null ? emp.JobLevel : "L-0";
            var start = jobLevel.IndexOf("-") + 1; //L-3
            var length = jobLevel.Length - start;
            var level = Convert.ToInt32(jobLevel.Substring(start, length));

            if (level < 5)
            {
                flag = true;
            }
            return flag;
        }
        //20111126为Constructiion部门特殊处理，Fanny的level为8
        //
        public static Employee GetNextConstructionApprover(Employee emp)
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

            while (level > 8)
            {
                approver = UserProfileUtil.GetEmployeeByProp("EmployeeId", approver.ManagerID);
                jobLevel = approver != null ? approver.JobLevel : "L-0";
                length = jobLevel.Length - start;
                level = Convert.ToInt32(jobLevel.Substring(start, length));
            }

            return approver;
        }

        //20110914
        //
        public static long GetQuota(string account, string type)
        {
            var emp = UserProfileUtil.GetEmployeeEx(account);
            return emp != null ? GetQuota(emp, type) : 0;
        }

        //20110913
        //
        public static long GetQuota(Employee emp, string type)
        {
            var quotaStr = GetQuotaByType(type);
            if (string.IsNullOrEmpty(quotaStr))
            {
                return -1;
            }

            char[] split = { ';' };
            string[] quotas = quotaStr.Split(split);

            long quota = 0;
            var start = emp.JobLevel.IndexOf("-") + 1; //L-3
            var length = emp.JobLevel.Length - start;
            var level = Convert.ToInt32(emp.JobLevel.Substring(start, length));

            if (WorkflowPerson.IsCEO(emp.UserAccount))
            {
                quota = Int64.MaxValue;
            }
            else if (WorkflowPerson.IsCFO(emp.UserAccount))
            {
                quota = Convert.ToInt32(quotas[3]);
            }
            else if (WorkflowPerson.IsMTM(emp.UserAccount))
            {
                quota = Convert.ToInt32(quotas[2]);
            }
            else if (level <= 5)
            {
                quota = Convert.ToInt32(quotas[1]);

            }
            else
            {
                quota = Convert.ToInt32(quotas[0]);
            }

            return quota;
        }

        public static void RemoveExistingRecord(SPList list, string key, string val)
        {
            CA.SharePoint.ISharePointService sps = CA.SharePoint.ServiceFactory.GetSharePointService(true);
            QueryField field = new QueryField(key, false);
            SPListItemCollection items = sps.Query(list, field.Equal(val), 0);

            for (int nIndex = items.Count - 1; nIndex >= 0; nIndex--)
            {
                items[nIndex].Web.AllowUnsafeUpdates = true;
                items[nIndex].Delete();
            }
        }

        //查找组中第一个用户
        public static string GetUserInGroup(string strGroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                for (int i = 0; i < group.Users.Count; i++)
                {
                    if (!group.Users[i].LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return group.Users[i].LoginName;
                    }
                }
            }

            return null;
        }

        public static QuickFlow.NameCollection GetUsersInGroup(string strGroupName)
        {
            QuickFlow.NameCollection names = new QuickFlow.NameCollection();
            //SPGroup group = null;
            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
            //    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            //    {
            //        using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
            //        {
            //            //group = web.Groups[strGroupName];
            //            group = web.SiteGroups[strGroupName];
            //        }
            //    }
            //});
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        group = web.SiteGroups[strGroupName];
                    }
                }
            });


            if (group != null)
            {
                foreach (SPUser user in group.Users)
                {
                    if (user.IsSiteAdmin || user.Name == "System Account")
                        continue;
                    names.Add(user.LoginName);
                }
            }

            return names;
        }

        public static List<SPUser> GetSPUsersInGroup(string strGroupName)
        {
            List<SPUser> users = new List<SPUser>();
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                foreach (SPUser user in group.Users)
                {
                    if (user.IsSiteAdmin)
                        continue;
                    users.Add(user);
                }
            }

            return users;
        }

        //取出组中用户
        public static List<string> UserListInGroup(string strGroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        group = web.SiteGroups[strGroupName];
                    }
                }
            });
            List<string> lst = new List<string>();
            if (group != null)
            {
                int len = group.Users.Count;
                for (int i = 0; i < len; i++)
                {
                    if (!group.Users[i].LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                    {
                        lst.Add(group.Users[i].LoginName);
                    }
                }
            }
            return lst;
        }

        //查找组名
        public static SPGroup GetUserGroup(string strGroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                return group;
            }
            else
                return null;
        }

        public static string GetAttachmentFolder(string workflowName, string trackingNumber)
        {
            if (string.IsNullOrEmpty(trackingNumber))
            {
                return ConfigurationManager.AppSettings["attachmentLibraryList"] + "/" + workflowName + "/temp/";
            }
            else
            {
                return ConfigurationManager.AppSettings["attachmentLibraryList"] + "/" + workflowName + "/" + trackingNumber + "/";
            }
        }

        public static QuickFlow.NameCollection GetNewsApproveUsers(string listTitle)
        {
            QuickFlow.NameCollection names = new QuickFlow.NameCollection();
            try
            {
                QueryField field = new QueryField("Title");
                CA.SharePoint.ISharePointService sps = CA.SharePoint.ServiceFactory.GetSharePointService(true);
                SPList list = sps.GetList("NewsApproveConfig");
                SPListItemCollection items = sps.Query(list, field.Equal(listTitle), 1);

                if (items != null && items.Count > 0)
                {
                    SPFieldUserValueCollection users = items[0]["Approvers"] as SPFieldUserValueCollection;
                    foreach (SPFieldUserValue user in users)
                    {
                        string userLoginName = user.LookupValue;
                        names.Add(userLoginName);
                    }
                }
            }
            catch { }
            return names;
        }

        //在同部门找上级审批人
        public static Employee GetEmployeeApproverInDept(Employee emp, bool isFindmanager, bool isFQ)
        {
            Employee approver = null;
            Employee old = emp;
            try
            {
                do
                {
                    if (!isFindmanager)
                    {
                        if (emp.ApproveRight)
                        {
                            approver = emp;
                        }
                        else
                        {
                            emp = UserProfileUtil.GetEmployee(emp.Manager);
                        }
                    }
                    else
                    {
                        emp = UserProfileUtil.GetEmployee(emp.Manager);
                        if (!IsSameDepartment(old.Department, emp.Department))
                        {
                            if (isFQ)
                            {
                                if (old.ApproveRight)
                                {
                                    approver = old;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (emp.ApproveRight)
                            {
                                approver = emp;
                                break;
                            }
                        }
                    }
                }
                while ((approver == null) && (emp != null) && (IsSameDepartment(old.Department, emp.Department)));
            }
            catch (Exception ex)
            {
                approver = null;
            }

            return approver;
        }

        public static bool IsSameDepartment(string Dept1, string Dept2)
        {
            string dept1, dept2;
            if (Dept1.Contains(';'))
                dept1 = Dept1.Substring(0, Dept1.IndexOf(';') + 1).ToLower();
            else
                dept1 = Dept1.ToLower();
            if (Dept2.Contains(';'))
                dept2 = Dept2.Substring(0, Dept2.IndexOf(';') + 1).ToLower();
            else
                dept2 = Dept2.ToLower();

            if (dept1.Equals(dept2))
            {
                return true;
            }

            if (UserProfileUtil.GetDepartmentDisplayName(dept1).Equals(UserProfileUtil.GetDepartmentDisplayName(dept2)))
                return true;

            return false;
        }

        public static decimal GetMixedDays(DateTime formBeginAt, DateTime formEndAt, string formBeginAt_am_or_pm, string formEndAt_am_or_pm, DateTime queryBeginAt, DateTime queryEndAt)
        {

            var days = decimal.Zero;

            //默认查询范围完全包含请假范围，取请假起始日期和截止日期
            DateTime beginAt = formBeginAt;
            DateTime endAt = formEndAt;

            //请假起始日期比查询起始日期早，取查询起始日期
            if (formBeginAt < queryBeginAt)
            {
                beginAt = queryBeginAt;
            }

            //请假截止日期比查询截止日期晚，取查询截止日期
            if (formEndAt > queryEndAt)
            {
                endAt = queryEndAt;
            }

            if (beginAt <= endAt)
            {
                //days = (endAt - beginAt).Days + 1;

                SPList festivalList = SPContext.Current.Web.Site.RootWeb.Lists["Festivals"];

                //遍历算天数，忽略双休，假期
                while (beginAt <= endAt)
                {
                    if (Convert.ToInt32(beginAt.DayOfWeek) != 0 && Convert.ToInt32(beginAt.DayOfWeek) != 6)
                    {
                        if (festivalList.Items.Count > 0)
                        {
                            if (!festivalList.Items.GetDataTable().AsEnumerable().Any(row =>
                                DateTime.Parse(row["OffDay"] + "").Date == beginAt.Date))
                            {
                                days += 1m;
                            }
                        }
                        else
                        {
                            days += 1m;
                        }
                    }

                    beginAt = beginAt.AddDays(1);
                }

                //如果这个表单起始日期在查询起始日期之后，且从下午开始请，那么少计半天
                if (Convert.ToInt32(formBeginAt.DayOfWeek) != 0 && Convert.ToInt32(formBeginAt.DayOfWeek) != 6
                    && formBeginAt >= queryBeginAt && formBeginAt_am_or_pm.ToLower() == "pm")
                {
                    days -= 0.5m;
                }

                //如果这个表单截止日期在查询截止日期之前，且请到上午，那么少计半天
                if (Convert.ToInt32(formEndAt.DayOfWeek) != 0 && Convert.ToInt32(formEndAt.DayOfWeek) != 6
                    && formEndAt <= queryEndAt && formEndAt_am_or_pm.ToLower() == "am")
                {
                    days -= 0.5m;
                }
            }

            return days;
        }

        public static decimal GetMixedDays(DateTime formBeginAt, DateTime formEndAt, string formBeginAt_am_or_pm, string formEndAt_am_or_pm)
        {

            var days = decimal.Zero;

            SPList festivalList = SPContext.Current.Web.Site.RootWeb.Lists["Festivals"];

            var tmpDate = formBeginAt;

            //遍历算天数，忽略双休，假期
            while (tmpDate <= formEndAt)
            {
                if (Convert.ToInt32(tmpDate.DayOfWeek) != 0 && Convert.ToInt32(tmpDate.DayOfWeek) != 6)
                {
                    if (festivalList.Items.Count > 0)
                    {
                        if (!festivalList.Items.GetDataTable().AsEnumerable().Any(row =>
                            DateTime.Parse(row["OffDay"] + "").Date == tmpDate.Date))
                        {
                            days += 1m;
                        }
                    }
                    else
                    {
                        days += 1m;
                    }
                }

                tmpDate = tmpDate.AddDays(1);
            }

            //如果这个表单从下午开始请，那么少计半天
            if (Convert.ToInt32(formBeginAt.DayOfWeek) != 0 && Convert.ToInt32(formBeginAt.DayOfWeek) != 6
                 && formBeginAt_am_or_pm.ToLower() == "pm")
            {
                days -= 0.5m;
            }

            //如果这个表单请到上午，那么少计半天
            if (Convert.ToInt32(formEndAt.DayOfWeek) != 0 && Convert.ToInt32(formEndAt.DayOfWeek) != 6
                 && formEndAt_am_or_pm.ToLower() == "am")
            {
                days -= 0.5m;
            }


            return days;
        }

        public static CamlExpression LinkAnd(CamlExpression expr1, CamlExpression expr2)
        {
            if (expr1 == null)
                return expr2;
            else
                return expr1 && expr2;
        }

        public static CamlExpression LinkOr(CamlExpression expr1, CamlExpression expr2)
        {
            if (expr1 == null)
                return expr2;
            else
                return expr1 || expr2;
        }

        //Example: 296;#CA\\test1;#297;#CA\test2
        //Return employee objects
        public static List<Employee> GetEmployees(string approversStr)
        {
            List<Employee> employees = new List<Employee>();
            List<string> accounts = GetEmployeeAccounts(approversStr);
            Employee emp = null;
            foreach (string a in accounts)
            {
                if (a.IsNullOrWhitespace())
                {
                    continue;
                }
                emp = UserProfileUtil.GetEmployeeEx(a);
                if (emp != null)
                {
                    employees.Add(emp);
                }
            }
            return employees;
        }

        //Example: 296;#CA\\test1;#297;#CA\test2
        //Return user accounts of employee
        public static List<string> GetEmployeeAccounts(string approversStr)
        {
            //List<string> accounts = new List<string>();
            //var accountPattern = @"\;\#(?<Account>[^;#]*)?\;\#";
            //var accountExpression = new Regex(accountPattern);
            //approversStr = approversStr + ";#";
            //var accountMatches = accountExpression.Matches(approversStr);
            //foreach (Match m in accountMatches)
            //{
            //    accounts.Add(m.Groups["Account"].Value);
            //}
            //return accounts;
            return approversStr.Split(';').ToList<string>();
        }

        //Example: Test(CA\\test1)
        //Return employee object
        public static Employee GetEmployee(string applicantStr)
        {
            string account = GetApplicantAccount(applicantStr);
            Employee emp = UserProfileUtil.GetEmployeeEx(account);
            return emp;

        }

        //Example: Test(CA\\test1)
        //Return user account of employee
        public static string GetApplicantAccount(string applicantStr)
        {
            var accountPattern = @"^.*?\((.*?)\)$";
            var accountExpression = new Regex(accountPattern);
            return accountExpression.Match(applicantStr).Groups[1].Value;
        }

        //Example: Test(CA\\test1)
        //Return user account of employee
        public static string GetDisplayName(string applicantStr)
        {
            var accountPattern = @"^(.*)\(.+$";
            var accountExpression = new Regex(accountPattern);
            return accountExpression.Match(applicantStr).Groups[1].Value;
        }

        /**
         * @param subject mail subject
         * @param bodyTemplate mail body template
         */
        public static void SendMail(string subject, string bodyTemplate, List<string> paramters, List<string> empAccountList)
        {
            List<Employee> empList = new List<Employee>();
            Employee emp = null;
            foreach (var temp in empAccountList)
            {
                emp = UserProfileUtil.GetEmployeeEx(temp);
                if (emp != null)
                {
                    empList.Add(emp);
                }
            }
            if (empList.Count > 0)
            {
                SendMail(subject, bodyTemplate, paramters, empList);
            }
        }

        public static void SendMail(string subject, string bodyTemplate, List<string> paramters, List<Employee> empList)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                foreach (Employee emp in empList)
                {
                    if (emp.WorkEmail.IsNullOrWhitespace())
                    {
                        continue;
                    }
                    IMailService mailService = MailServiceFactory.GetMailService();
                    paramters[0] = emp.DisplayName; //Set the display name
                    string body = string.Format(bodyTemplate, paramters.ToArray());
                    StringCollection sc = new StringCollection();
                    sc.Add(emp.WorkEmail);
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

        //Return the list instance
        public static SPList GetWorkflowList(string listName)
        {
            return SPContext.Current.Site.OpenWeb("workflowcenter").Lists[listName];
        }

        //Get the list item that contains email subject and body
        public static SPListItem GetEmailTemplateByTitle(string title)
        {
            var qTitle = new QueryField("Title", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qTitle.Equal(title));
            SPListItemCollection lc = ListQuery.Select()
                .From(GetWorkflowList("EmailTemplate"))
                .Where(exp)
                .GetItems();

            return lc.Count == 0 ? null : lc[0];
        }

        //Return the display name of employee
        public static string GetEmployeeName(string userAccount)
        {
            string displayName = string.Empty;
            Employee u = UserProfileUtil.GetEmployeeEx(userAccount);
            return u != null ? u.DisplayName : displayName;
        }

        public static string GetDisplayNames(List<string> empAccounts)
        {
            Employee emp;
            List<Employee> employees = new List<Employee>();
            foreach (var temp in empAccounts)
            {
                emp = UserProfileUtil.GetEmployeeEx(temp);
                if (emp != null)
                {
                    employees.Add(emp);
                }
            }
            return GetDisplayNames(employees);
        }

        //Return the approvers names
        public static string GetDisplayNames(List<Employee> employees)
        {
            if (employees.Count == 0)
            {
                return string.Empty;
            }
            //Get the display name of approvers and applicant from userprofile
            StringBuilder approverNames = new StringBuilder();
            foreach (Employee emp in employees)
            {
                approverNames.Append(emp.DisplayName + ", ");
            }
            string temp = approverNames.ToString();
            return temp.Substring(0, temp.Length - 2);
        }

        public static string GetDeleman(string loginName, string moduleId)
        {
            string deleman = null;
            var now = DateTime.Now;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPQuery query = new SPQuery();
                        var querystr = @"<Where>
                                            <And>
                                                <And>
                                                <And>
                                                   <Eq>
                                                      <FieldRef Name='ApproverLoginName' />
                                                      <Value Type='Text'>{0}</Value>
                                                   </Eq>
                                                   <Leq>
                                                      <FieldRef Name='BeginOn' />
                                                      <Value Type='Number'>{1}</Value>
                                                   </Leq>
                                                </And>
                                                <Geq>
                                                   <FieldRef Name='EndOn' />
                                                   <Value Type='Number'>{1}</Value>
                                                </Geq>
                                             </And>
                                             <Contains>
                                                <FieldRef Name='Modules' />
                                                <Value Type='Text'>{2}</Value>
                                             </Contains>
                                          </And>
                                       </Where>";

                        query.Query = string.Format(querystr, new object[] { loginName, now.ToString("yyyyMMdd"), moduleId });

                        var delegationList = web.Lists["Delegates"];
                        var listItems = delegationList.GetItems(query);

                        if (listItems.Count > 0)
                        {
                            var account = listItems[0]["DelegateToLoginName"].AsString();
                            // Need to check whether the delegate account is valid.
                            if (UserProfileUtil.GetEmployeeEx(account) != null)
                            {
                                deleman = account;
                            }
                        }
                    }
                }
            });


            return deleman;
        }

        /*
         * Get delegate users according to the given multi names.
         * @Return array that contains delegate users
         */
        public static List<string> GetDelemans(List<string> loginNames, string moduleId)
        {
            List<string> delemans = new List<string>();
            var now = DateTime.Now.ToString("yyyyMMdd");

            var qApprover = new QueryField("ApproverLoginName", false);
            var qBeginOn = new QueryField("BeginOn", false);
            var qEndOn = new QueryField("EndOn", false);
            var qModules = new QueryField("Modules", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qModules.Contains(moduleId));
            exp = WorkFlowUtil.LinkAnd(exp, qBeginOn.LessEqual(now));
            exp = WorkFlowUtil.LinkAnd(exp, qEndOn.MoreEqual(now));

            CamlExpression exp2 = null;
            foreach (string name in loginNames)
            {
                exp2 = WorkFlowUtil.LinkOr(exp2, qApprover.Equal(name));
            }

            exp = WorkFlowUtil.LinkAnd(exp, exp2);

            SPListItemCollection coll = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        coll = ListQuery.Select()
                            .From(web.Lists["Delegates"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });

            foreach (SPListItem item in coll)
            {
                // Need to check whether the delegate account is valid.
                if (UserProfileUtil.GetEmployeeEx(item["DelegateToLoginName"].ToString()) != null)
                {
                    delemans.Add(item["DelegateToLoginName"].ToString());
                }
            }

            return delemans;
        }

        //Return task users object according to special group
        public static NameCollection GetTaskUsers(string group, string moduleId)
        {
            var taskUsers = new NameCollection();
            List<string> delemans = null;
            List<string> groupUsers = null;

            groupUsers = WorkFlowUtil.UserListInGroup(group);
            taskUsers.AddRange(groupUsers.ToArray());

            delemans = WorkFlowUtil.GetDelemans(groupUsers, moduleId);
            //if (delemans.Count > 0)
            //{
            //    taskUsers.AddRange(delemans.ToArray());
            //}
            foreach (var delegateAccount in delemans)
            {
                // Need to check whether the delegate account is valid.
                if (UserProfileUtil.GetEmployeeEx(delegateAccount) != null)
                {
                    taskUsers.Add(delegateAccount);
                }
            }

            return taskUsers;
        }

        public static NameCollection GetTaskUsersWithoutDeleman(string group, string moduleId)
        {
            var taskUsers = new NameCollection();
            List<string> groupUsers = null;

            groupUsers = WorkFlowUtil.UserListInGroup(group);
            taskUsers.AddRange(groupUsers.ToArray());

            return taskUsers;
        }

        public static string GetListId(string listName)
        {
            return GetWorkflowList(listName).ID.ToString();
        }

        public static string GetQuotaByType(string type)
        {
            var approvalType = new QueryField("ApprovalType", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, approvalType.Equal(type));

            SPListItemCollection coll = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        coll = ListQuery.Select()
                            .From(web.Lists["QuotaLevel"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });
            return coll.Count > 0 ? coll[0]["Limits"].AsString() : null;
        }

        public static void BatchDeleteItems(string listName, string workflowNumber)
        {
            // Set up the variables to be used.
            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
                "<SetList Scope=\"Request\">{1}</SetList>" +
                "<SetVar Name=\"ID\">{2}</SetVar>" +
                "<SetVar Name=\"Cmd\">Delete</SetVar>" +
                "</Method>";

            // Get the list containing the items to update.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);

            // Query to get the unprocessed items.
            SPQuery query = new SPQuery();
            query.Query = @"<Where>
                                <Eq>
                                    <FieldRef Name='Title' />
                                    <Value Type='Text'>" + workflowNumber + @"</Value>
                                </Eq>
                            </Where>";
            query.ViewAttributes = "Scope='Recursive'";
            SPListItemCollection unprocessedItems = list.GetItems(query);

            if (unprocessedItems.Count > 0)
            {
                // Build the CAML delete commands.
                foreach (SPListItem item in unprocessedItems)
                {
                    methodBuilder.AppendFormat(methodFormat, "1", item.ParentList.ID, item.ID.ToString());
                }

                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch.ToString());
            }
        }

        /*
         * Return data table according to given REQUESTID and LISTNAME
         */
        public static DataTable GetDataTable(string requestId, string listName)
        {
            return GetCollection(requestId, listName).GetDataTable();
        }

        /*
         * Return data collection according to given REQUESTID and LISTNAME
         * 升级版方法为下面GetCollectionByColumn(string column, string value, string listName)
         */
        public static SPListItemCollection GetCollection(string requestId, string listName)
        {
            var qRequestId = new QueryField("Title", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qRequestId.Equal(requestId));
            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList(listName))
                .Where(exp)
                .GetItems();
            return lc;
        }

        /*
         * Return data collection according to given REQUESTID and LISTNAME
         * 这个方法将是GetCollection(string requestId, string listName)的替代方法，以后尽量不要用上面那个，而换成用这个
         */
        public static SPListItemCollection GetCollectionByColumn(string column, string value, string listName)
        {
            var qColumn = new QueryField(column, false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qColumn.Equal(value));

            SPListItemCollection lc = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                                .From(WorkFlowUtil.GetWorkflowList(listName))
                                .Where(exp)
                                .GetItems();
                    }
                }
            });

            return lc;
        }

        /**
         * 根据List名字查找里面所有记录
         * 
         */
        public static SPListItemCollection GetCollectionByList(string listName)
        {
            CamlExpression exp = null;
            SPListItemCollection lc = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                                .From(web.Lists[listName])
                                .Where(exp)
                                .GetItems();
                    }
                }
            });

            return lc;
        }

        /**
         * 根据金额和审批类型查找需要流程所需最低审批Level和最高Level
         */
        public static Hashtable GetApproveLevel(float amount, string approveType)
        {
            var qLowAmount = new QueryField("LowAmount", false);
            var qHighAmount = new QueryField("HighAmount", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qLowAmount.LessThan(amount));
            exp = WorkFlowUtil.LinkAnd(exp, qHighAmount.MoreEqual(amount));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("ApproveLevel"))
                .Where(exp)
                .GetItems();
            Hashtable ht = new Hashtable();
            if (lc.Count > 0)
            {
                ht.Add("LowLevel", lc[0]["LowLevel"].ToString());
                ht.Add("HighLevel", lc[0]["HighLevel"].ToString());
            }

            return ht;
        }

        public static string GetModuleIdByListName(string listName)
        {
            var moduleId = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPQuery query = new SPQuery();
                        var querystr = @"<Where>
                                                   <Eq>
                                                      <FieldRef Name='ListName' />
                                                      <Value Type='Text'>{0}</Value>
                                                   </Eq>
                                       </Where>";

                        query.Query = string.Format(querystr, listName);

                        var list = web.Lists["Modules"];
                        var listItems = list.GetItems(query);

                        if (listItems.Count > 0)
                        {
                            moduleId = listItems[0]["Tag"] + "";
                        }
                    }
                }
            });

            return moduleId;
        }

        /**
         * Check whether the account is existed in the gourps
         */
        public static bool IsInGroups(string account, params string[] groups)
        {
            bool isExist = false;
            foreach (var group in groups)
            {
                isExist = IsInGroup(account, group);
                if (isExist) break;
            }

            return isExist;
        }

        /**
         * Check whether the account is existed in the gourp
         */
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

        internal static void BatchInsertRepeaterData(Repeater repeater, string listName, string workflowNumber, Hashtable hashtable)
        {
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            StringBuilder fieldsBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"0\">" +
            "<SetList Scope=\"Request\">" + listGuid + "</SetList>" +
            "<SetVar Name=\"ID\">New</SetVar>" +
            "<SetVar Name=\"Cmd\">Save</SetVar>" +
            "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">" + workflowNumber + "</SetVar>{0}</Method>";

            string fieldFormat = "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>";


            foreach (RepeaterItem item in repeater.Items)
            {

                foreach (DictionaryEntry entry in hashtable)
                {

                    string[] value = entry.Value.ToString().Split(new string[] { ";#" }, StringSplitOptions.None);
                    string controlId = value[0];
                    string controlType = value[1];
                    Control control = item.FindControl(controlId);
                    string itemValue = string.Empty;

                    switch (controlType)
                    {
                        case "Label":
                            System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)control;
                            itemValue = label.Text;
                            break;
                        case "DropDownList":
                            DropDownList dropDownList = (DropDownList)control;
                            itemValue = dropDownList.SelectedValue;
                            break;
                        case "CheckBox":
                            CheckBox checkBox = (CheckBox)control;
                            itemValue = checkBox.Checked ? "1" : "0";
                            break;
                        default:
                            TextBox textBox = (TextBox)control;
                            itemValue = textBox.Text;
                            break;
                    }

                    fieldsBuilder.AppendFormat(fieldFormat,
                                                entry.Key.ToString(),
                                                itemValue);
                }

                methodBuilder.AppendFormat(methodFormat, fieldsBuilder.ToString());

            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = BatchExecute(batch);

                //SPSecurity.RunWithElevatedPrivileges(delegate {
                //    if (!EventLog.SourceExists("C&A"))
                //    {
                //        EventLog.CreateEventSource("C&A", "Mail");
                //    }

                //    // Create an EventLog instance and assign its source.
                //    EventLog myLog = new EventLog();
                //    myLog.Source = "C&A";

                //    // Write an informational entry to the event log.    
                //    myLog.WriteEntry("Tag Travel Requeset batch insert:"+batchReturn, EventLogEntryType.Information);    
                //});
            }

        }


        public static void BatchInsertDataTable(string listName, string workflowNumber, DataTable dt, Hashtable hash)
        {
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            StringBuilder fieldsBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"0\">" +
            "<SetList Scope=\"Request\">" + listGuid + "</SetList>" +
            "<SetVar Name=\"ID\">New</SetVar>" +
            "<SetVar Name=\"Cmd\">Save</SetVar>" +
            "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">" + workflowNumber + "</SetVar>{0}</Method>";

            string fieldFormat = "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>";

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    string colValue = dr[dc.ColumnName].ToString();

                    switch (dc.DataType.ToString())
                    {
                        case "System.Boolean":
                            colValue = Boolean.Parse(dr[dc.ColumnName].ToString()) ? "1" : "0";
                            break;
                        default:
                            break;
                    }
                    fieldsBuilder.AppendFormat(fieldFormat,
                                                dc.ColumnName,
                                                colValue);

                }

                if (hash != null && hash.Count > 0)
                {
                    foreach (DictionaryEntry entry in hash)
                    {
                        fieldsBuilder.AppendFormat(fieldFormat,
                                                     entry.Key.ToString(),
                                                     entry.Value.ToString());
                    }
                }
                methodBuilder.AppendFormat(methodFormat, fieldsBuilder.ToString());
                fieldsBuilder.Clear();
            }




            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = BatchExecute(batch);

                //SPSecurity.RunWithElevatedPrivileges(delegate {
                //    if (!EventLog.SourceExists("C&A"))
                //    {
                //        EventLog.CreateEventSource("C&A", "Mail");
                //    }

                //    // Create an EventLog instance and assign its source.
                //    EventLog myLog = new EventLog();
                //    myLog.Source = "C&A";

                //    // Write an informational entry to the event log.    
                //    myLog.WriteEntry("Tag Travel Requeset batch insert:"+batchReturn, EventLogEntryType.Information);    
                //});
            }

        }


        public static void BatchDeleteItems(string listName, SPQuery query)
        {
            // Set up the variables to be used.
            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
                "<SetList Scope=\"Request\">{1}</SetList>" +
                "<SetVar Name=\"ID\">{2}</SetVar>" +
                "<SetVar Name=\"Cmd\">Delete</SetVar>" +
                "</Method>";

            // Get the list containing the items to update.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);

            // Query to get the unprocessed items.

            SPListItemCollection unprocessedItems = list.GetItems(query);

            // Build the CAML delete commands.
            foreach (SPListItem item in unprocessedItems)
            {
                methodBuilder.AppendFormat(methodFormat, "1", item.ParentList.ID, item.ID.ToString());
            }

            // Put the pieces together.
            batch = string.Format(batchFormat, methodBuilder.ToString());

            // Process the batch of commands.
            string batchReturn = SPContext.Current.Web.ProcessBatchData(batch.ToString());
        }

        //批处理执行脚本
        public static string BatchExecute(string batch)
        {
            string batchReturn = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        batchReturn = web.ProcessBatchData(batch);
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

            return batchReturn;
        }

        public static DataTable GetDataTableSource(string workflowNumber, string listName, List<string> fieldsList)
        {
            DataTable dt = GetDataTable(workflowNumber, listName);

            return dt.DefaultView.ToTable(false, fieldsList.ToArray());
        }

        /// <summary>
        ///     Clears the contents of the string builder.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="StringBuilder"/> to clear.
        /// </param>
        public static void Clear(this StringBuilder value)
        {
            value.Length = 0;
            value.Capacity = 0;
        }
        /// <summary>
        /// 得到门店下的没有收货的PO
        /// </summary>
        /// <param name="sCostCenters"></param>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        public static DataTable GetNotRecievedPOForStore(string sCostCenters, string sPONO)
        {
            SPListItemCollection splic = null;
            CamlExpression ce = null;
            QueryField qfIsRecieved = new QueryField("IsReceived");
            ce = WorkFlowUtil.LinkAnd(ce, qfIsRecieved.Equal(false));
            QueryField PONO = new QueryField("Title");
            ce = WorkFlowUtil.LinkAnd(ce, PONO.Equal(sPONO));

            CamlExpression ceOR = null;
            QueryField qfCostCenter = new QueryField("CostCenter");
            foreach (string str in sCostCenters.Split(','))
            {
                ceOR = WorkFlowUtil.LinkOr(ceOR, qfCostCenter.Equal(str));
            }
            ce = WorkFlowUtil.LinkAnd(ce, ceOR);
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        splic = ListQuery.Select()
                                .From(WorkFlowUtil.GetWorkflowList("PurchaseOrderItems"))
                                .Where(ce)
                                .GetItems();
                    }
                }
            });
            if (splic.Count > 0)
            {
                return splic.GetDataTable();
            }
            else
            {
                return null;
            }

        }

        public static string GetQuery(string field, string value)
        {
            string queryFormat = @"<Where>
                                      <Eq>
                                         <FieldRef Name='{0}' />
                                         <Value Type='Text'>{1}</Value>
                                      </Eq>
                                   </Where>";

            return string.Format(queryFormat, field, value);

        }

        public static DataTable GetDataSourceBySort(DataTable sourceTable)
        {
            DataTable dt = null;
            if (sourceTable != null)
            {
                dt = sourceTable.Clone();
                DataRow[] rows = sourceTable.Select(null, "Display");

                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        dt.ImportRow(row);
                    }
                }
            }

            return dt;
        }


        public static void UpdateWorkflowPath(WorkflowContext context)
        {
            try
            {
                if (context != null)
                {
                    FieldInfo field = context.GetType().GetField("_WorkflowVariables", BindingFlags.NonPublic | BindingFlags.Instance);
                    WorkflowVariableValues workflowVariables = field.GetValue(context) as WorkflowVariableValues;

                    SPList unlockWorkflowList = SPContext.Current.Web.Lists[WorkflowListName.UnLockWorkflow];

                    QueryField titleField = new QueryField("Title");
                    QueryField wfListField = new QueryField("WorkflowListName");

                    CamlExpression exp = titleField.Equal(context.DataFields["Title"].AsString())
                                         && wfListField.Equal(SPContext.Current.List.Title);

                    SPListItemCollection items = ListQuery.Select().From(unlockWorkflowList).Where(exp).GetItems();

                    if (items.Count > 0)
                    {
                        SPListItem item = items[0];
                        item["EventData"] += string.Format("{0}#;", SerializeUtil.Serialize(workflowVariables));

                        item.Update();
                    }
                    else
                    {
                        SPListItem newItem = unlockWorkflowList.Items.Add();

                        newItem[SPBuiltInFieldId.Title] = context.DataFields["Title"].AsString();
                        newItem["WorkflowListName"] = SPContext.Current.List.Title;
                        newItem["EventData"] = string.Format("{0}#;", SerializeUtil.Serialize(workflowVariables));

                        newItem.Update();
                    }
                    //CommonUtil.logInfo(string.Format("TaskOutcome:{0}", context.Task.Outcome));
                }
            }
            catch (Exception ex)
            {

                CommonUtil.logError(string.Format("[UpdateWorkflowPath]:{0}", ex.StackTrace));
            }
        }

        public static void UpdateWorkflowPath(WorkflowContext context, string wfNoFieldName)
        {
            try
            {
                if (context != null)
                {
                    FieldInfo field = context.GetType().GetField("_WorkflowVariables", BindingFlags.NonPublic | BindingFlags.Instance);
                    WorkflowVariableValues workflowVariables = field.GetValue(context) as WorkflowVariableValues;

                    SPList unlockWorkflowList = SPContext.Current.Web.Lists[WorkflowListName.UnLockWorkflow];

                    QueryField titleField = new QueryField("Title");
                    QueryField wfListField = new QueryField("WorkflowListName");

                    CamlExpression exp = titleField.Equal(context.DataFields[wfNoFieldName].AsString())
                                         && wfListField.Equal(SPContext.Current.List.Title);

                    SPListItemCollection items = ListQuery.Select().From(unlockWorkflowList).Where(exp).GetItems();

                    if (items.Count > 0)
                    {
                        SPListItem item = items[0];
                        item["EventData"] += string.Format("{0}#;", SerializeUtil.Serialize(workflowVariables));

                        item.Update();
                    }
                    else
                    {
                        SPListItem newItem = unlockWorkflowList.Items.Add();

                        newItem[SPBuiltInFieldId.Title] = context.DataFields[wfNoFieldName].AsString();
                        newItem["WorkflowListName"] = SPContext.Current.List.Title;
                        newItem["EventData"] = string.Format("{0}#;", SerializeUtil.Serialize(workflowVariables));

                        newItem.Update();
                    }
                    //CommonUtil.logInfo(string.Format("TaskOutcome:{0}", context.Task.Outcome));
                }
            }
            catch (Exception ex)
            {

                CommonUtil.logError(string.Format("[UpdateWorkflowPath]:{0}", ex.StackTrace));
            }
        }

        public static void UpdateWorkflowPath(SPListItem item, string eventData)
        {
            try
            {
                SPList unlockWorkflowList = SPContext.Current.Web.Lists[WorkflowListName.UnLockWorkflow];

                SPListItem newItem = unlockWorkflowList.Items.Add();

                newItem[SPBuiltInFieldId.Title] = item.Title;
                newItem["WorkflowListName"] = item.ParentList.Title;
                newItem["EventData"] = string.Format("{0}#;", eventData);

                newItem.Update();

            }
            catch (Exception ex)
            {

                CommonUtil.logError(string.Format("[UpdateWorkflowPath]:{0}", ex.StackTrace));
            }
        }
    }
}
