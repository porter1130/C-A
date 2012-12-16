using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using Microsoft.SharePoint;
using CA.SharePoint;
using System.Configuration;

namespace CA.WorkFlow.UI.ExpensesClaimReport
{
    public partial class ExpensesClaimReport : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool result=CheckAccount();
                if (result) 
                {
                    RedirectToTask();
                }
            }
            btnQuery.Click += this.QueryForm.btnQuery_Click;
        }

        #region Check Account

        private bool CheckAccount()
        {
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (current.ToLower() == "SHAREPOINT\\system".ToLower())
            {
                return false;
            }
            string obj = ConfigurationManager.AppSettings["ClaimReportAccount"];
            List<string> list = obj.Split(';').ToList<string>();
            list.Remove("");
            if(list.Count==0)
            {
                return false;
            }
            string[] strAccount = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                strAccount[i] = list[i];
            }

            if (!IsInGroups(current, strAccount))
            {
                return true;
            }
            return false;
        }

        private bool IsInGroups(string account, params string[] groups)
        {
            bool isExist = false;
            foreach (var group in groups)
            {
                isExist = IsInGroup(account, group);
                if (isExist) break;
            }
            return isExist;
        }

        private bool IsInGroup(string account, string group)
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

        #endregion

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            QueryForm.ExcelPort();
        }
    }
}