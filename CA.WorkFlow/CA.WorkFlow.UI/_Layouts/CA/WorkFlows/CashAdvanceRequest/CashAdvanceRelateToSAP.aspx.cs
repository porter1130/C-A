using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Text;
using CA.SharePoint;

namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class CashAdvanceRelateToSAP : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAccount();
            }
            btnCashAdvanceRelateToSAP.Click += this.DataForm1.btnCashAdvanceRelateToSAP_Click;
        }

        private void CheckAccount()
        {
            //wf_Accountants组的人可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!IsInGroups(current, new string[] { "wf_Accountants" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
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

    }
}