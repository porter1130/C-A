using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Collections.Generic;

namespace CA.WorkFlow.UI.EBC
{
    public partial class DataEdit2 : BaseWorkflowUserControl
    {
        #region Field

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

        private string dataFormMode;

        public string DataFormMode
        {
            get { return dataFormMode; }
            set { dataFormMode = value; }
        }

        public string SummaryExpenseType
        {
            set { this.hidSummaryExpenseType.Value = value; }
            get { return this.hidSummaryExpenseType.Value.Trim(); }
        }

        public string ExpatriateBenefitForm
        {
            set { this.hidExpatriateBenefitForm.Value = value; }
            get { return this.hidExpatriateBenefitForm.Value.Trim(); }
        }

        public string CashAdvanceAmount
        {
            set { this.hidCashAdvanceAmount.Value = value; }
            get { return this.hidCashAdvanceAmount.Value.Trim(); }
        }

        public string CashAdvanceID
        {
            set { this.hidCashAdvanceID.Value = value; }
            get { return this.hidCashAdvanceID.Value.Trim(); }
        }

        public string CashAdvanceIDAndAmount
        {
            set { this.hidCashAdvanceIDAndAmount.Value = value; }
            get { return this.hidCashAdvanceIDAndAmount.Value.Trim(); }
        }

        public string TotalAmount
        {
            set { this.hidTotalAmount.Value = value; }
            get { return this.hidTotalAmount.Value.Trim(); }
        }

        public bool IsAttachInvoice
        {
            get { return this.rbAttInv.Checked; }
            set
            {
                this.rbAttInv.Checked = value;
                this.rbNonAttInv.Checked = !value;
            }
        }

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.DataFormMode != "" && (this.DataFormMode.Equals("New", StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();
                    DataBindCashAdvance("");
                }
                if (this.DataFormMode != "" && (this.DataFormMode.Equals("Edit", StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.cpfUser.CommaSeparatedAccounts = this.Applicant.UserAccount;
                    DataBindCashAdvance(CashAdvanceID);
                }
            }
            this.cpfUser.Load += new EventHandler(cpfUser_Load);
        }

        private void LoadUser()
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            this.lbRequestedBy.Text = Applicant.DisplayName + "(" + Applicant.UserAccount + ")";
            this.lbDept.Text = Applicant.Department;
            
        }

        protected void cpfUser_Load(object sender, EventArgs e)
        {
            LoadUser();
        }

        protected void btnPeopleInfo_Click(object sender, EventArgs e)
        {
            LoadUser();
            if (!this.Applicant.EmployeeID.StartsWith("2"))
            {
                Response.Write("<script type=\"text/javascript\">alert('The Expatriate Benefit Claim EWF only cover expat employees benefit');window.location = window.location;</script>");
                Response.End();
                return;
            }
            DataBindCashAdvance("");
        }

        private void DataBindCashAdvance(string type)
        {
            var delegationList = SharePointUtil.GetList("CashAdvanceRequest");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><And><And><Eq><FieldRef Name='Applicant' /><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='Status' /><Value Type='Text'>Completed</Value></Eq></And><Neq><FieldRef Name='CashAdvanceStatus' /><Value Type='Text'>1</Value></Neq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>", this.Applicant.UserAccount);
            SPListItemCollection listItems = delegationList.GetItems(query);

            double cashAdvanceAmount = 0;
            string cashAdvanceIDAndAmount = "";
            string cashAdvanceID = "";
            if (null != listItems && listItems.Count >= 1)
            {
                System.Text.StringBuilder html = new System.Text.StringBuilder();
                html.Append("<ul>");
                foreach (SPListItem spi in listItems)
                {
                    if (type != "" && type.Contains(spi["WorkflowNumber"].ToString()))
                    {
                        html.Append("<li><input type=\"checkbox\" checked=\"checked\"  value=\"" + spi["Amount"].ToString() + "\"  title=\"" + spi["WorkflowNumber"].ToString() + "\"/>" + spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString() + "</li>");
                        cashAdvanceAmount += Double.Parse(spi["Amount"].ToString());
                        cashAdvanceIDAndAmount += spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString() + ";";
                        cashAdvanceID += spi["WorkflowNumber"].ToString() + ";";
                    }
                    else
                    {
                        html.Append("<li><input type=\"checkbox\" value=\"" + spi["Amount"].ToString() + "\"  title=\"" + spi["WorkflowNumber"].ToString() + "\"/>" + spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString() + "</li>");
                    }
                }
                html.Append("</ul>");
                cardiv.InnerHtml = html.ToString();
                if (cashAdvanceAmount != 0)
                {
                    titlediv.InnerHtml = cashAdvanceAmount.ToString();
                    this.CashAdvanceAmount = cashAdvanceAmount.ToString();
                    this.CashAdvanceIDAndAmount = cashAdvanceIDAndAmount;
                    this.CashAdvanceID = cashAdvanceID;
                }
                else
                {
                    titlediv.InnerHtml = "Select Cash Advance to be deducted";
                    this.CashAdvanceAmount = "0";
                    this.CashAdvanceIDAndAmount = "";
                    this.CashAdvanceID = "";
                }
            }
            else
            {
                titlediv.InnerHtml = "No Cash Advance";
                this.CashAdvanceAmount = "0";
                this.CashAdvanceIDAndAmount = "";
                this.CashAdvanceID = "";
            }
        }

    }
}
