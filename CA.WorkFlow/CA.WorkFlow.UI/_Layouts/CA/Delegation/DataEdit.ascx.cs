namespace CA.WorkFlow.UI.Delegation
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint;
    using System.Collections;
    using System.Collections.Generic;

    public partial class DataEdit : BaseWorkflowUserControl
    {
        #region Field

        private static readonly string DelegableModulesListName = "Modules";
        private static readonly string DelegableModulesListKeyName = "BelongsTo";
        private static readonly string DelegationListName = "Delegates";
        private int count = 132;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                Response.Expires = 0;
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.AddHeader("pragma", "no-cache");
                Response.CacheControl = "no-cache";

                DataTable dt = WorkFlowUtil.GetCollectionByList(DelegableModulesListName).GetDataTable();
                count = dt.Rows.Count + 101;

                DataTable delegates = GetDelegatesByCurEmp();
                string beginOnStr = string.Empty;
                string endOnStr = string.Empty;
                var today = DateTime.Now;
                DateTime begindate = today;
                DateTime enddate = today.AddDays(1);
                string tag = string.Empty;
                if (delegates.Rows.Count > 0)
                {
                    foreach (DataRow row in delegates.Rows)
                    {
                        tag += row["Modules"].ToString() + ";";
                        string pfID = "pf" + row["Modules"].ToString();
                        CAPeopleFinder pf = (CAPeopleFinder)this.FindControl(pfID);
                        pf.CommaSeparatedAccounts = row["DelegateToLoginName"].ToString();
                        if (beginOnStr == "")
                        {
                            beginOnStr = row["BeginOn"].ToString();
                            endOnStr = row["EndOn"].ToString();
                            this.dtBegin.SelectedDate = DateTime.Parse(string.Format("{0}-{1}-{2}",
                                                                        beginOnStr.Substring(0, 4),
                                                                        beginOnStr.Substring(4, 2),
                                                                        beginOnStr.Substring(6, 2)));
                            this.dtEnd.SelectedDate = DateTime.Parse(string.Format("{0}-{1}-{2}",
                                                                        endOnStr.Substring(0, 4),
                                                                        endOnStr.Substring(4, 2),
                                                                        endOnStr.Substring(6, 2)));
                            begindate = this.dtBegin.SelectedDate;
                            enddate = this.dtEnd.SelectedDate;
                        }
                    }
                }
                this.dtBegin.SelectedDate = begindate;
                this.dtEnd.SelectedDate = enddate;

                this.hfModule.Value = tag;

                var currentUser = SPContext.Current.Web.CurrentUser;
                this.lblUser.Text = currentUser.Name;
                this.hidUserAccount.Value = currentUser.LoginName;
            }
        }

        #region NEW

        private bool Validate(out string error)
        {
            bool valid = true;
            error = string.Empty;
            if (this.dtBegin.SelectedDate > this.dtEnd.SelectedDate)
            {
                valid = false;
                error += "The start date should be smalller than or equal with the end date.";
            }
            return valid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (!this.Validate(out error))
            {
                Response.Write("<script type='text/javascript' language='javascript'>" +
                                "alert('" + error + "');" +
                                "window.location = window.location;" +
                                "</script>");
                return;
            }
            var delegationList = SharePointUtil.GetList(DelegationListName);
            string delegated = this.hidUserAccount.Value;
            string startDate = this.dtBegin.SelectedDate.ToString("yyyyMMdd");
            string endDate = this.dtEnd.SelectedDate.ToString("yyyyMMdd");
            List<string> moduleList = this.hfModule.Value.Split(';').ToList<string>();
            moduleList.Remove("");
            foreach (string moduleID in moduleList)
            {
                string pfID = "pf" + moduleID;
                CAPeopleFinder pf = (CAPeopleFinder)this.FindControl(pfID);
                SPListItem delegation;
                if (this.IsValidDelegation(out delegation, moduleID))
                {
                    if (pf.Accounts.Count == 0)
                    {
                        delegation.Web.AllowUnsafeUpdates = true;
                        delegation.Delete();
                        delegation.Web.AllowUnsafeUpdates = false;
                    }
                    else
                    {
                        string agent = pf.Accounts[0].ToString();
                        delegation["Approver"] = GetUser(delegated);
                        delegation["DelegateTo"] = GetUser(agent);
                        delegation["ApproverLoginName"] = delegated;
                        delegation["DelegateToLoginName"] = agent;
                        delegation["BeginOn"] = startDate;
                        delegation["EndOn"] = endDate;
                        delegation["Modules"] = moduleID;
                        delegation["Title"] = "My Delegation";
                        delegation.Web.AllowUnsafeUpdates = true;
                        delegation.Update();
                        delegation.Web.AllowUnsafeUpdates = false;
                    }
                }
                else
                {
                    if (pf.Accounts.Count == 0)
                    {
                        string errorMsg = "Please select a user account for your delegation";
                        this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('" + errorMsg + "');window.location = window.location;</script>");
                        return;
                    }
                    delegation = delegationList.Items.Add();
                    string agent = pf.Accounts[0].ToString();
                    delegation["Approver"] = GetUser(delegated);
                    delegation["DelegateTo"] = GetUser(agent);
                    delegation["ApproverLoginName"] = delegated;
                    delegation["DelegateToLoginName"] = agent;
                    delegation["BeginOn"] = startDate;
                    delegation["EndOn"] = endDate;
                    delegation["Modules"] = moduleID;
                    delegation["Title"] = "My Delegation";
                    delegation.Web.AllowUnsafeUpdates = true;
                    delegation.Update();
                    delegation.Web.AllowUnsafeUpdates = false;
                }
            }
            this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Modify Success! '); window.location = window.location;</script>");
        }

        private bool IsValidDelegation(out SPListItem delegation, string ModulesId)
        {
            bool valid = false;
            delegation = null;
            //创建代理规则，一个用户只能创建一个代理，如果第一次则是创建代理，如果第二次则是修改代理
            var delegationList = SharePointUtil.GetList(DelegationListName);
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><And><Eq><FieldRef Name='ApproverLoginName' /><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='Modules' /><Value Type='Text'>{1}</Value></Eq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>", SPContext.Current.Web.CurrentUser.LoginName, ModulesId);
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (null != listItems && listItems.Count >= 1)
            {
                delegation = listItems[0];
                valid = true;
            }
            return valid;
        }

        private DataTable GetDelegatesByCurEmp() 
        {
            var delegationList = SharePointUtil.GetList(DelegationListName);
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='ApproverLoginName' /><Value Type='Text'>{0}</Value></Eq></Where>", SPContext.Current.Web.CurrentUser.LoginName);
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count>0) 
            {
                return listItems.GetDataTable();
            }
            return new DataTable();
        }

        #endregion
        private static SPUser GetUser(string loginName)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                var currentSite = SPContext.Current.Site;
                using (var site = new SPSite(currentSite.ID))
                {
                    using (SPWeb web = site.OpenWeb(currentSite.RootWeb.ID))
                    {
                        user = web.EnsureUser(loginName);
                    }
                }
            });
            return user;
        }
       

    }
}