using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class DataEdit : QFUserControl
    {
        #region get property
        private string company;

        public string Company
        {
            get { return this.txtCompany.Text.Trim(); }
        }

        private string department;

        public string Department
        {
            get { return this.txtDept.Text.Trim(); }
        }
        private string requestBy;

        public string RequestBy
        {
            get { return this.txtRequestedBy.Text.Trim(); }
        }
        private string amount;

        public string Amount
        {
            get { return this.txtAmount.Text.Trim(); }
        }
        private string purpose;

        public string Purpose
        {
            get { return this.txtPurpose.Text.Trim(); }
        }
        private string term;

        public string Term
        {
            get
            {
                if (termVal.Value.Trim() != "")
                {
                    return termVal.Value.Trim();
                }
                else
                {
                    return this.dplTerm.SelectedValue;
                }
            }
        }

        private string cashAdvanceType;

        public string CashAdvanceType
        {
            get { return this.rblLevel.SelectedValue; }
        }
        private string remark;

        public string Remark
        {
            get { return this.txtRemark.Text.Trim(); }
        }

        private string msg;

        public string MSG { get { return msg; } }

        private string level;

        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        private string urgentRemark;

        public string UrgentRemark
        {
            get { return this.txtUrgentRemark.Text.Trim(); }
            set { urgentRemark = value; }
        }
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
        public string mode = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.SetControlMode();

                //Add Mode
                if (this.ControlMode == SPControlMode.New)
                {
                    DataBindTerm();
                    this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();
                    Employee employee = UserProfileUtil.GetEmployee(SPContext.Current.Web.CurrentUser.ToString());
                    this.txtRequestedBy.Text = employee.DisplayName + "(" + employee.UserAccount + ")";
                    this.txtDept.Text = employee.Department;
                    this.Applicant = employee;
                    mode = "new";

                    //this.txtDept.Text = this.CurrentEmployee.Department;
                    //txtRequestedBy.Text = SPContext.Current.Site.RootWeb.CurrentUser.LoginName;
                    //this.ffDepartment.Value = this.CurrentEmployee.Department;
                }
                //Edit Mode
                if (this.ControlMode == SPControlMode.Edit)
                {
                    this.cpfUser.CommaSeparatedAccounts = this.Applicant.UserAccount;
                    DataBindFields();
                    
                }
                //Display Mode
                if (this.ControlMode == SPControlMode.Display)
                {
                    DataBindFields();
                    SetReadOnly();
                    //this.ffUrgentRemark.ControlMode = SPControlMode.Display;
                    //this.ffDepartment.ControlMode = SPControlMode.Display;
                }
                if (this.ControlMode == SPControlMode.New)
                {
                    mode = "new";
                }
                this.cpfUser.Load += new EventHandler(cpfUser_Load);
            }
        }

        void cpfUser_Load(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());

            this.txtRequestedBy.Text = this.Applicant.DisplayName + "(" + this.Applicant.UserAccount + ")";
            this.txtDept.Text = this.Applicant.Department;
           
            mode = "new";
        }

        protected void btnPeopleInfo_Click(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());

            this.txtRequestedBy.Text = this.Applicant.DisplayName + "(" + this.Applicant.UserAccount + ")";
            this.txtDept.Text = this.Applicant.Department;

            mode = "new";
        }
        private void DataBindFields()
        {
             DataBindTerm();
            SPListItem curItem = SPContext.Current.ListItem;
            this.txtCompany.Text = curItem["Company"] == null ? "" : curItem["Company"].ToString();
            this.txtDept.Text = curItem["Department"] == null ? "" : curItem["Department"].ToString();
            this.txtRequestedBy.Text = curItem["Applicant"].ToString();
            this.txtPurpose.Text = curItem["Purpose"] == null ? "" : curItem["Purpose"].ToString();
            this.txtAmount.Text = curItem["Amount"] == null ? "" : curItem["Amount"].ToString(); 
            this.dplTerm.SelectedIndex = curItem["Term"].ToString() == "Cash" ? 0 : 1;
            this.termVal.Value = curItem["Amount"] == null ? "" : curItem["Amount"].ToString(); 
            this.rblLevel.SelectedIndex = curItem["CashAdvanceType"].ToString() == "Normal" ? 0 : 1;
            this.txtRemark.Text = curItem["Remark"]==null ? "" : curItem["Remark"].ToString();
            this.txtUrgentRemark.Text = curItem["UrgentRemark"] == null ? "" : curItem["UrgentRemark"].ToString();
            this.lblSapNumber.Text = curItem["SAPNumber"] == null ? "" : curItem["SAPNumber"].ToString();
        }

        private void SetReadOnly() 
        {
            //this.txtSAPNo.ReadOnly = true;
            this.txtCompany.ReadOnly = true;
            this.txtDept.ReadOnly = true;
            //this.txtRequestedBy.ReadOnly = true;
            this.txtPurpose.ReadOnly = true;
            this.txtAmount.ReadOnly = true;
            this.dplTerm.Enabled = false;
            this.Level = "1";
            this.txtRemark.ReadOnly = true;
            this.attacthment.ControlMode = SPControlMode.Display;
            this.txtUrgentRemark.ReadOnly = false;
        }

        
        private void DataBindTerm()
        {
            dplTerm.Items.Clear();
            dplTerm.Items.Add(new ListItem("Cash", "Cash"));
            dplTerm.Items.Add(new ListItem("Transfer", "Transfer"));
        }

        public string CheckInfo()
        {
            StringBuilder output = new StringBuilder();
            //if (string.IsNullOrEmpty(txtCompany.Text))
            //    output.Append("Please Input Company Name !");
            //if (string.IsNullOrEmpty(this.txtDept.Text))
            //    output.Append("Please Input Department Name !");
            //if (string.IsNullOrEmpty(this.txtRequestedBy.Text))
            //    output.Append("Please Input Applicant Name !");
            if (string.IsNullOrEmpty(txtAmount.Text))
                output.Append("Please Input Cash Advance Amount !");
            return output.ToString();
        }
        
    }
}