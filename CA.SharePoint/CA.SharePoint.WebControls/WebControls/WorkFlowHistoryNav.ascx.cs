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
using Microsoft.SharePoint;
using System.Collections.Generic;



namespace CA.SharePoint.WebControls
{
    public partial class WorkFlowHistoryNav : System.Web.UI.UserControl
    {
        public string status = "";

        private WorkFlowPage _WFPage = WorkFlowPage.HomePage;
        public WorkFlowPage WFPage
        {
            get
            {
                return this._WFPage;
            }
            set
            {
                this._WFPage = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool result = CheckAccount();
                if (result)
                {
                    status = "No";
                }
            }

            if (this.WFPage == WorkFlowPage.WorkFlowPage)
            {
                this.divWrokFlowNav.Style.Remove("overflow-x");
                this.divWrokFlowNav.Style.Remove("overflow-y");
                this.divWrokFlowNav.Style.Remove("height");
            }
        }

        private bool CheckAccount()
        {
            //wf_Accountants、wf_FinanceManager组的人可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (current.ToLower() == "SHAREPOINT\\system".ToLower()) 
            {
                return false;
            }
            string obj = ConfigurationManager.AppSettings["SAPAccount"];
            List<string> list = obj.Split(';').ToList<string>();
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

    }
}