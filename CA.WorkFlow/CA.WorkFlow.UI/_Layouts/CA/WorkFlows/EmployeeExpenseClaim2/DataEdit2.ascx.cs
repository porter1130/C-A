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
using QuickFlow.Core;

namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
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

        private string jobLevel;
        public string JobLevel
        {
            set { jobLevel = value; }
            get { return jobLevel; }
        }

        public string CostCenterValue 
        {
            set { this.hfCostCenter.Value = value; }
        }

        public string ExpenseTypeValue
        {
            set { this.hfExpenseType.Value = value; }
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
                LoadExpenseTypeAndCostCenter();
                LoadCompanyStandard();
            }
            this.cpfUser.Load += new EventHandler(cpfUser_Load);
        }

        private void LoadExpenseTypeAndCostCenter()
        {
            System.Text.StringBuilder strExpenseType = new System.Text.StringBuilder();
            strExpenseType.Append("[");
            //DataTable dtExpenseType = WorkFlowUtil.GetCollectionByList("Employee Expense Claim Type").GetDataTable();
            string workFlowNumber = string.Empty;
            if (this.DataFormMode.Equals("Edit", StringComparison.CurrentCultureIgnoreCase))
            {
                workFlowNumber = WorkflowContext.Current.DataFields["WorkflowNumber"].ToString();
            }
            DataTable dtExpenseType = WorkFlowUtil.GetCollectionByList("Employee Expense Claim Type").GetDataTable();//EmployeeExpenseClaimCommon.GetExpenseType(workFlowNumber, DataFormMode);
            if (dtExpenseType != null && dtExpenseType.Rows.Count > 0)
            {
                foreach (DataRow dr in dtExpenseType.Rows)
                {
                    strExpenseType.Append("{");
                    strExpenseType.AppendFormat("name:'{0}',val:'{1}'", dr["NewExpenseType"].ToString(), dr["ExpenseType"].ToString());
                    strExpenseType.Append("},");
                }
            }
            strExpenseType.Append("]");
            this.ExpenseTypeValue = strExpenseType.ToString();

            System.Text.StringBuilder strCostCenter = new System.Text.StringBuilder();
            strCostCenter.Append("[");
            DataTable dtCostCenter = WorkFlowUtil.GetDataSourceBySort(WorkFlowUtil.GetCollectionByList("Cost Centers").GetDataTable());
            if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCostCenter.Rows)
                {
                    strCostCenter.Append("{");
                    strCostCenter.AppendFormat("name:'{0}',val:'{1}'", dr["Display"].ToString(), dr["Title"].ToString());
                    strCostCenter.Append("},");
                }
            }
            strCostCenter.Append("]");
            this.CostCenterValue = strCostCenter.ToString();
        }

        private void LoadCompanyStandard()
        {
            string mobileStd = string.Empty;
            var mobileStdItem = EmployeeExpenseClaimCommon.GetClaimStdByLevel(Convert.ToInt32(JobLevel), "Mobile phone");
            if (mobileStdItem == null)
            {
                mobileStd = "0";
            }
            else if (mobileStdItem["Amount"].AsString().IsNullOrWhitespace())
            {
                mobileStd = "no limit";
            }
            else
            {
                mobileStd = mobileStdItem["Amount"].AsString();
            }

            string otMealStd = string.Empty;
            var otMealItem = EmployeeExpenseClaimCommon.GetClaimStdByLevel(Convert.ToInt32(JobLevel), "OT - meal allowance");
            otMealStd = otMealItem != null ? otMealItem["Amount"].AsString() : string.Empty;

            this.hfOTMealStandard.Value = otMealStd;
            this.hfMobileStandard.Value = mobileStd;
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
            DataBindCashAdvance("");
            LoadCompanyStandard();
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
