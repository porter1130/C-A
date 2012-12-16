using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Collections.Generic;

using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using System.Diagnostics;

namespace CA.SharePoint.WebControls
{
    public partial class Organization : System.Web.UI.UserControl
    {
        private List<Employee> employees
        {
            set { ViewState["employees"] = value; }
            get {
                if (ViewState["employees"] == null)
                    ViewState["employees"] = new List<Employee>();

                return (List<Employee>)ViewState["employees"]; }
        }
        //Hashtable deptNameXDisp = new Hashtable();
        protected string strDept = null;
        private List<string> deptlistHeads = new List<string>();
        private List<Employee> heads = new List<Employee>();
        private List<Employee> managers = new List<Employee>();
        //private List<Employee> others = new List<Employee>();

        protected void Page_Load(object sender, EventArgs e)
        {
            strDept = Page.Request["Dept"];
            if (string.IsNullOrEmpty(strDept))
                return;

            if (!IsPostBack)
            {
                //buildTree();

                if (strDept.Equals("Store Operations", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetAllEmployeeByStoreOperationsDept();
                }
                else
                {
                    buildTree();
                }

                //ucUserDetails.Visible = false;
            }

            if (trvEmployees.SelectedNode != null)
            {
                selectUser();
            }
        }


        private List<string> GetDepartmentManager(string Dept)
        {
            List<string> data = new List<string>();

            SPList list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);

            foreach (SPListItem item in list.Items)
            {
                if (item[CAConstants.FieldName.DisplayName].ToString().Equals(Dept, StringComparison.CurrentCultureIgnoreCase))
                {
                    //return (new SPFieldLookupValue(item[CAConstants.FieldName.ManagerAccount] + "")).LookupValue;
                    SPFieldLookupValue lv=new SPFieldLookupValue(item[CAConstants.FieldName.ManagerAccount].ToString());
                    if (!data.Contains(lv.LookupValue))
                    {
                        data.Add(lv.LookupValue.ToLower());
                    }
                }
            }

            return data;

        }

        #region Store Operations部门员工数据

        //取得Store Operations部门，及user profile中员工部门不在Department list的所有员工
        private void GetAllEmployeeByStoreOperationsDept()
        {
            trvEmployees.Nodes.Clear();
            employees.Clear();

            //string manager = UserProfileUtil.GetDepartmentManager(strDept);

            string strSPDept = Page.Request["Dept"];

            List<string> manager = GetDepartmentManager(strSPDept);

            System.Text.StringBuilder strDepartment = new System.Text.StringBuilder();

            //Department  List Items
            SPList listDepartment = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);
            foreach (SPListItem item in listDepartment.Items)
            {
                if (item["DisplayName"] == null)
                    continue;
                string strTempSPDept = item["Name"].ToString();
                if (!strTempSPDept.Equals(strDept, StringComparison.CurrentCultureIgnoreCase))
                {
                    strDepartment.Append(strTempSPDept + ";");
                }
            }

            Employee empl = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    Microsoft.Office.Server.ServerContext context = Microsoft.Office.Server.ServerContext.GetContext(site);
                    Microsoft.Office.Server.UserProfiles.UserProfileManager profileManager = new Microsoft.Office.Server.UserProfiles.UserProfileManager(context);
                    foreach (Microsoft.Office.Server.UserProfiles.UserProfile profile in profileManager)
                    {
                        if (profile[Microsoft.Office.Server.UserProfiles.PropertyConstants.Department].Value == null)
                            continue;

                        if (!strDepartment.ToString().Contains(profile[Microsoft.Office.Server.UserProfiles.PropertyConstants.Department].Value.ToString()))
                        {
                            empl = UserProfileUtil.InstanceEmployee(profile, site);
                            if (null != empl)
                            {
                                employees.Add(empl);
                            }
                        }
                    }
                }
            }
            );

            employees.Sort(delegate(Employee emp1, Employee emp2)
            {
                //if (emp1.Department != emp2.Department)
                //{
                //    return emp1.Department.CompareTo(emp2.Department);
                //}

                var jobIdx1 = emp1.JobLevel.IndexOf("-");
                var jobIdx2 = emp2.JobLevel.IndexOf("-");
                if (jobIdx1 == -1)
                {
                    if (jobIdx2 == -1)
                        return 0;
                    else
                        return 1;
                }
                else
                {
                    if (jobIdx2 == -1)
                        return -1;
                }

                var start = jobIdx1 + 1; //L-3
                var job1 = int.Parse(emp1.JobLevel.Substring(start));
                var job2 = int.Parse(emp2.JobLevel.Substring(start));

                if (emp1.JobLevel.Equals(emp2.JobLevel, StringComparison.CurrentCultureIgnoreCase))
                    return emp1.DisplayName.CompareTo(emp2.DisplayName);
                else
                    return job1.CompareTo(job2);

            });

            Dictionary<string, TreeNode> processed = new Dictionary<string, TreeNode>();
            TreeNode tempRoot = new TreeNode("All Employees", "All Employees");

            foreach (Employee employee in employees) 
            {
                TreeNode newNode = new TreeNode();
                newNode.Value = employee.DisplayName;

                //if (manager.Equals(employee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                if (manager.Contains(employee.UserAccount.ToLower()))
                {
                    newNode.Text = string.Format("<span style=\"font-weight:bold\" onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", employee.DisplayName, strDept);
                }
                else
                {
                    newNode.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", employee.DisplayName, strDept);
                }
                    tempRoot.ChildNodes.Add(newNode);
            }



            trvEmployees.Nodes.Add(tempRoot);
            

            //foreach (Employee employee in employees)
            //{
            //    if (processed.Keys.Contains(employee.DisplayName))
            //        continue;

            //    Employee tmpEmployee = employee;
            //    TreeNode managerNode = null;
            //    while (tmpEmployee != null)
            //    {
            //        if (processed.Keys.Contains(tmpEmployee.DisplayName))
            //        {
            //            tmpEmployee = null;
            //            continue;
            //        }
            //        else if (string.IsNullOrEmpty(tmpEmployee.Manager) || (tmpEmployee.UserAccount.Equals(tmpEmployee.Manager)))
            //        {
            //            //TreeNode newNode = new TreeNode(tmpEmployee.DisplayName, tmpEmployee.DisplayName);
            //            TreeNode newNode = new TreeNode();
            //            newNode.Value = tmpEmployee.DisplayName;
            //            newNode.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", tmpEmployee.DisplayName, strDept);
            //            if (managerNode != null)
            //            {
            //                newNode.ChildNodes.Add(managerNode);
            //            }
            //            managerNode = null;
            //            tempRoot.ChildNodes.Add(newNode);
            //            processed.Add(tmpEmployee.DisplayName, newNode);
            //            tmpEmployee = null;
            //            continue;
            //        }

            //        //TreeNode node = new TreeNode(tmpEmployee.DisplayName, tmpEmployee.DisplayName);
            //        TreeNode node = new TreeNode();
            //        node.Value = tmpEmployee.DisplayName;
            //        node.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", tmpEmployee.DisplayName, strDept);
            //        if (managerNode != null)
            //        {
            //            node.ChildNodes.Add(managerNode);
            //        }
            //        managerNode = node;
            //        processed.Add(tmpEmployee.DisplayName, managerNode);

            //        tmpEmployee = employees.Find(new Predicate<Employee>(delegate(Employee emp)
            //        {
            //            return emp.UserAccount == tmpEmployee.Manager;
            //        }));

            //        if (tmpEmployee == null)
            //        {
            //            tempRoot.ChildNodes.Add(managerNode);
            //        }
            //        else if (processed.TryGetValue(tmpEmployee.DisplayName, out node))
            //        {
            //            node.ChildNodes.Add(managerNode);
            //            tmpEmployee = null;
            //            continue;
            //        }
            //    }
            //}
            //trvEmployees.Nodes.Add(tempRoot);
        }

        #endregion

        protected void trvEmployees_SelectedNodeChanged(object sender, EventArgs e)
        {
            selectUser();
        }

        private void buildTree()
        {
            trvEmployees.Nodes.Clear();
            employees.Clear();

            string strSPDept = Page.Request["Dept"];

            List<string> manager = GetDepartmentManager(strSPDept);

            //string manager = UserProfileUtil.GetDepartmentManager(strDept);

            var found = false;
            var list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);

            if (strSPDept.Contains("Store"))
            {
                var allDepts = new List<string>();

                foreach (SPListItem item in list.Items)
                {
                    if (!string.IsNullOrEmpty(item["Name"] + ""))
                    {
                        allDepts.Add(item["Name"].ToString().ToLower());
                    }
                }
                employees = UserProfileUtil.GetEmployeeFromSSPNotInDepts(allDepts.ToArray());
            }
            else
            {
                foreach (SPListItem item in list.Items)
                {
                    if (!string.IsNullOrEmpty(item["DisplayName"] + ""))
                    {
                        var strTempSPDept = item["DisplayName"].ToString().ToLower();

                        if (strTempSPDept == strSPDept.ToLower())
                        {
                            employees.AddRange(UserProfileUtil.GetEmployeeFromSSPByDept(item["Name"].ToString()));
                            found = true;
                        }
                    }
                }

                if (!found)
                    employees = UserProfileUtil.GetEmployeeFromSSPByDept(strSPDept);
            }

            employees.Sort(delegate(Employee emp1, Employee emp2)
            {
                //if (emp1.Department != emp2.Department)
                //{
                //    return emp1.Department.CompareTo(emp2.Department);
                //}

                var jobIdx1 = emp1.JobLevel.IndexOf("-");
                var jobIdx2 = emp2.JobLevel.IndexOf("-");
                if (jobIdx1 == -1)
                {
                    if (jobIdx2 == -1)
                        return 0;
                    else
                        return 1;
                }
                else
                {
                    if (jobIdx2 == -1)
                        return -1;
                }

                var start = jobIdx1 + 1; //L-3
                var job1 = int.Parse(emp1.JobLevel.Substring(start));
                var job2 = int.Parse(emp2.JobLevel.Substring(start));

                if (emp1.JobLevel.Equals(emp2.JobLevel, StringComparison.CurrentCultureIgnoreCase))
                    return emp1.DisplayName.CompareTo(emp2.DisplayName);
                else
                    return job1.CompareTo(job2);

            });

            Dictionary<string, TreeNode> processed = new Dictionary<string, TreeNode>();
            TreeNode tempRoot = new TreeNode("All Employees", "All Employees");

            foreach (Employee employee in employees)
            {
                TreeNode newNode = new TreeNode();
                newNode.Value = employee.DisplayName;

                //if (manager.Equals(employee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                if (manager.Contains(employee.UserAccount.ToLower()))
                {
                    newNode.Text = string.Format("<span style=\"font-weight:bold\" onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", employee.DisplayName, strDept);
                }
                else
                {
                    newNode.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", employee.DisplayName, strDept);
                }

               // newNode.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", employee.DisplayName, strDept);
                tempRoot.ChildNodes.Add(newNode);
            }
            trvEmployees.Nodes.Add(tempRoot);

            //foreach (Employee employee in employees)
            //{
            //    if (processed.Keys.Contains(employee.DisplayName))
            //        continue;

            //    Employee tmpEmployee = employee;
            //    TreeNode managerNode = null;
            //    while (tmpEmployee != null)
            //    {
            //        if (processed.Keys.Contains(tmpEmployee.DisplayName))
            //        {
            //            tmpEmployee = null;
            //            continue;
            //        }
            //        else if (string.IsNullOrEmpty(tmpEmployee.Manager) || (tmpEmployee.UserAccount.Equals(tmpEmployee.Manager)))
            //        {
            //            //TreeNode newNode = new TreeNode(tmpEmployee.DisplayName, tmpEmployee.DisplayName);
            //            TreeNode newNode = new TreeNode();
            //            newNode.Value = tmpEmployee.DisplayName;
            //            newNode.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", tmpEmployee.DisplayName, strSPDept);
            //            if (managerNode != null)
            //            {
            //                newNode.ChildNodes.Add(managerNode);
            //            }
            //            managerNode = null;
            //            tempRoot.ChildNodes.Add(newNode);
            //            processed.Add(tmpEmployee.DisplayName, newNode);
            //            tmpEmployee = null;
            //            continue;
            //        }

            //        //TreeNode node = new TreeNode(tmpEmployee.DisplayName, tmpEmployee.DisplayName);
            //        TreeNode node = new TreeNode();
            //        node.Value = tmpEmployee.DisplayName;
            //        node.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", tmpEmployee.DisplayName, strSPDept);
            //        if (managerNode != null)
            //        {
            //            node.ChildNodes.Add(managerNode);
            //        }
            //        managerNode = node;
            //        processed.Add(tmpEmployee.DisplayName, managerNode);

            //        tmpEmployee = employees.Find(new Predicate<Employee>(delegate(Employee emp)
            //        {
            //            return emp.UserAccount == tmpEmployee.Manager;
            //        }));

            //        if (tmpEmployee == null)
            //        {
            //            tempRoot.ChildNodes.Add(managerNode);
            //        }
            //        else if (processed.TryGetValue(tmpEmployee.DisplayName, out node))
            //        {
            //            node.ChildNodes.Add(managerNode);
            //            tmpEmployee = null;
            //            continue;
            //        }
            //    }
            //}
            //trvEmployees.Nodes.Add(tempRoot);
           // trvEmployees.Nodes.Add(SortTree(tempRoot));
        }

        private TreeNode SortTree(TreeNode tree)
        {
            var deptheads = new Hashtable();
            
            var depts = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);
            foreach (SPListItem item in depts.Items)
            {
                try
                {
                    var disp = item["DisplayName"] + "";
                    var userField = item.Fields["ManagerAccount"] as SPFieldUser;
                    var userFieldValue = userField.GetFieldValue(item["ManagerAccount"].ToString()) as SPFieldUserValue;

                    //deptNameXDisp.Add(item["Title"] + "", item["DisplayName"] + "");

                    //deptheads.Add(userFieldValue.User.LoginName.ToLower());
                    if (deptheads.Contains(disp))
                    {
                        var value = deptheads[disp] as List<string>;
                        value.Add(userFieldValue.User.LoginName.ToLower());
                        deptheads[disp] = value;
                    }
                    else
                    {
                        deptheads.Add(disp, new List<string>(new string[] { userFieldValue.User.LoginName.ToLower() }));
                    }
                    deptlistHeads.Add(userFieldValue.User.LoginName.ToLower());
                }
                catch
                { }
            }

            var root = new TreeNode("All Employees", Page.Request["Dept"] + " Department");

            foreach (TreeNode child in tree.ChildNodes)
            {
                SortNode(child);
            }

            if (managers.Count > 0)
            {
                managers.Sort(delegate(Employee emp1, Employee emp2)
                {
                    var jobIdx1 = emp1.JobLevel.IndexOf("-");
                    var jobIdx2 = emp2.JobLevel.IndexOf("-");
                    if (jobIdx1 == -1)
                    {
                        if (jobIdx2 == -1)
                            return 0;
                        else
                            return 1;
                    }
                    else
                    {
                        if (jobIdx2 == -1)
                            return -1;
                    }

                    var start = jobIdx1 + 1; //L-3
                    var job1 = int.Parse(emp1.JobLevel.Substring(start));
                    var job2 = int.Parse(emp2.JobLevel.Substring(start));

                    if (emp1.JobLevel.Equals(emp2.JobLevel, StringComparison.CurrentCultureIgnoreCase))
                        return emp1.DisplayName.CompareTo(emp2.DisplayName);
                    else
                        return job1.CompareTo(job2);


                });
            }

            if (heads.Count > 0)
            {
                heads.Sort(delegate(Employee emp1, Employee emp2)
                {

                    //if (deptNameXDisp.Contains(emp1.Department)
                    //    && deptNameXDisp.Contains(emp2.Department)
                    //    && deptNameXDisp[emp1.Department] == deptNameXDisp[emp2.Department])
                    //{ 

                    //}
                    if (emp1.Department != emp2.Department)
                    {
                        return emp1.Department.CompareTo(emp2.Department);
                    }

                    var jobIdx1 = emp1.JobLevel.IndexOf("-");
                    var jobIdx2 = emp2.JobLevel.IndexOf("-");
                    if (jobIdx1 == -1)
                    {
                        if (jobIdx2 == -1)
                            return 0;
                        else
                            return 1;
                    }
                    else
                    {
                        if (jobIdx2 == -1)
                            return -1;
                    }

                    var start = jobIdx1 + 1; //L-3
                    var job1 = int.Parse(emp1.JobLevel.Substring(start));
                    var job2 = int.Parse(emp2.JobLevel.Substring(start));

                    if (emp1.JobLevel.Equals(emp2.JobLevel, StringComparison.CurrentCultureIgnoreCase))
                        return emp1.DisplayName.CompareTo(emp2.DisplayName);
                    else
                        return job1.CompareTo(job2);


                });
            }

            //if ((managers.Count > 0) && (managers[0].Department.Equals("MTM", StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    managers.Sort(delegate(Employee emp1, Employee emp2)
            //    {
            //        return emp1.Title.CompareTo(emp2.Title);
            //    });
            //}
            //else
            //{
            //    managers.Sort(delegate(Employee emp1, Employee emp2)
            //    {
            //        if (emp1.Department.Equals(emp2.Department, StringComparison.CurrentCultureIgnoreCase))
            //            return emp1.DisplayName.CompareTo(emp2.DisplayName);
            //        else
            //            return emp1.Department.CompareTo(emp2.Department);
            //    });
            //}

            //others.Sort(delegate(Employee emp1, Employee emp2)
            //{
            //    if (emp1.Department.Equals(emp2.Department, StringComparison.CurrentCultureIgnoreCase))
            //        return emp1.DisplayName.CompareTo(emp2.DisplayName);
            //    else if (emp1.Department.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
            //        return 1;
            //    else if (emp2.Department.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
            //        return -1;
            //    else
            //        return emp1.Department.CompareTo(emp2.Department);
            //});

            //var printHeads = new List<string>();
            foreach (Employee emp in managers)
            {
                var head = new TreeNode();
                head.Value = emp.DisplayName;
                var style = "style=\"font-weight:bold\"";
                
                head.Text = string.Format("<span {0} onclick=\"return SelectUser('{1}','{2}')\">{1}</span>",
                    style, emp.DisplayName, Page.Request["Dept"]);
                root.ChildNodes.Add(head);
            }

            foreach (Employee emp in heads)
            {
                //printHeads.Add(emp.UserAccount);
                //TreeNode head = new TreeNode("<b>" + emp.DisplayName + "</b>", emp.DisplayName);
                var head = new TreeNode();
                head.Value = emp.DisplayName;
                var style = string.Empty;
                //if (deptheads.Contains(emp.UserAccount.ToLower()))
                if (deptheads.Contains(Page.Request["Dept"]) 
                    && (deptheads[Page.Request["Dept"]] as List<string>).Contains(emp.UserAccount.ToLower()))
                {
                    style = "style=\"font-weight:bold\"";
                }
                head.Text = string.Format("<span {0} onclick=\"return SelectUser('{1}','{2}')\">{1}</span>", 
                    style, emp.DisplayName, Page.Request["Dept"]);
                root.ChildNodes.Add(head);
            }

            //WriteLog(string.Format("deptHeads:{0}\n\nprintHeads:{1}",
            //            string.Join(", ", deptheads.ToArray()),
            //            string.Join(", ", printHeads.ToArray())),
            //            EventLogEntryType.Error);

            //foreach (Employee emp in managers)
            //{
            //    //TreeNode manager = new TreeNode("<b>" + emp.DisplayName + "</b>", emp.DisplayName);
            //    TreeNode manager = new TreeNode();
            //    manager.Value = emp.DisplayName;
            //    manager.Text = string.Format("<span style=\"font-weight:bold\" onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", emp.DisplayName, Page.Request["Dept"]);
            //    root.ChildNodes.Add(manager);
            //}

            //foreach (Employee emp in others)
            //{
            //    TreeNode other = new TreeNode();
            //    other.Value = emp.DisplayName;
            //    other.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", emp.DisplayName, Page.Request["Dept"]);
            //    //root.ChildNodes.Add(new TreeNode(emp.DisplayName, emp.DisplayName));
            //    root.ChildNodes.Add(other);
            //}

            return root;
        }

        private void SortNode(TreeNode node)
        {
            Employee emp = getEmployee(node);
            //if (emp.ApproveRight)
            //{
            //    Employee manager = getEmployee(emp.Manager);
            //    if (manager == null)
            //        heads.Insert(0, emp);
            //    else
            //        managers.Add(emp);
            //}
            //else
            //{
            //    others.Add(emp);
            //}
            if (deptlistHeads.Contains(emp.UserAccount.ToLower()))
            {
                //managers优先置顶
                managers.Add(emp);
            }
            else
            {
                heads.Add(emp);
            }

            foreach (TreeNode child in node.ChildNodes)
            {
                SortNode(child);
            }
        }

        private Employee getEmployee(TreeNode node)
        {
            return employees.Find(new Predicate<Employee>(delegate(Employee emp)
            {
                return emp.DisplayName.ToLower() == node.Value.ToLower();
            }));
        }

        private Employee getEmployee(string userAccount)
        {
            return employees.Find(new Predicate<Employee>(delegate(Employee emp)
            {
                return emp.UserAccount.ToLower() == userAccount.ToLower();
            }));
        }
        public void selectUser()
        {
            //ucUserDetails.User = employees.Find(new Predicate<Employee>(delegate(Employee emp)
            //{
            //    return emp.DisplayName == trvEmployees.SelectedNode.Value;
            //}));

            //ucUserDetails.Visible = (ucUserDetails.User != null);
            //ucUserDetails.BindData();
        }

        void WriteLog(string err, EventLogEntryType type)
        {
            if (!EventLog.SourceExists("C&A"))
            {
                EventLog.CreateEventSource("C&A", "Mail");
            }
            var myLog = new EventLog();
            myLog.Source = "C&A";
            myLog.WriteEntry(err, type);
        }
    }
}