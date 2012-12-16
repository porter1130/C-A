using System;
using System.Collections.Generic;
using System.Linq;
using CA.SharePoint;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest
{
    public partial class DataEdit : System.Web.UI.UserControl
    {
        public Employee Applicant
        {
            get
            {
                return (this.ViewState["Applicant"] as Employee);
            }
            set
            {
                this.ViewState["Applicant"] = value;
            }
        }

        private string userAccount;
        public string UserAccount
        {
            get
            {
                return userAccount;
            }
            set
            {
                this.userAccount = value;
            }
        }

        public string Department
        {
            get { return this.Applicant.Department; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //DisplayMessage("您当前登录的是系统帐号，属非法提交！");
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
                //return;
                //spsadmin will ignore the security check
            }
            if (!this.Page.IsPostBack)
            {
                this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();
                
            }

            this.cpfUser.Load += new EventHandler(cpfUser_Load);
        }
        void cpfUser_Load(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            

            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            FillEmployeeData(this.Applicant);
            //FillEmployeeData(this.Applicant, false); //Dont overwrite the personal fields when loading if the fields have content
        }


        protected void btnPeopleInfo_Click(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            FillEmployeeData(this.Applicant);
            //this.ApplicantField.Value = this.Applicant.DisplayName;
            //this.testTxb.Text = this.testTxb.Text.ToString() + "7";
          
        }

        private void FillEmployeeData(Employee employee)
        {
            this.ApplicantField.Value = this.Applicant.DisplayName + "(" + this.Applicant.UserAccount + ")";
            this.DepartmentField.Value = this.Applicant.Department;
            this.ChineseNameField.Value = this.Applicant.DisplayName;
        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }
    }
}