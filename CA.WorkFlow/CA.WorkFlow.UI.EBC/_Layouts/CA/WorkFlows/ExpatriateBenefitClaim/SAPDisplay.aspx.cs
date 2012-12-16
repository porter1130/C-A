using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Collections.Generic;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;

namespace CA.WorkFlow.UI.EBC
{
    public partial class SAPDisplay : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CheckAccount();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
            if (fields["Status"].ToString() == "Completed")
            {
                this.btnClaimToSAPForm.Visible = true;
            }
            else
            {
                this.btnClaimToSAPForm.Visible = false;
            }
            this.TaskTrace1.Applicant = fields["Applicant"].ToString();
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