namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CA.SharePoint.Utilities.Common;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Globalization;
    using System.Text;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using System.ComponentModel;
    using CodeArt.SharePoint.CamlQuery;
    using System.Configuration;
    using System.Collections;
    using GemBox.Spreadsheet;

    public partial class QueryReport : System.Web.UI.UserControl
    {
        #region Field
        private string queryType = string.Empty;
        private string status = string.Empty;
        private string department = string.Empty;
        private string paidDate = string.Empty;
        private string applicant = string.Empty;
        private string vendorName = string.Empty;
        private int startNO = 0;
        private int endNO = 0;
        private DateTime startCreate = DateTime.Now;
        private DateTime endCreate = DateTime.Now;
        private decimal startAmount = 0;
        private decimal endAmount = 0;
        private int startVendorNO = 0;
        private int endVendorNO = 0;
        private string currency = "RMB";
        private string pono = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int s_year = DateTime.Now.Year;
                int s_month = DateTime.Now.Month - 1;
                int s_date = DateTime.Now.Day;
                int e_year = DateTime.Now.Year;
                int e_month = DateTime.Now.Month;
                int e_date = DateTime.Now.Day;
                string startCreate = string.Empty;
                string endCreate = string.Empty;
                if (s_month == 2 && e_date > 28)
                {
                    s_date = 28;
                }
                if ((s_month == 4 && e_date > 30)
                    || (s_month == 6 && e_date > 30)
                    || (s_month == 9 && e_date > 30)
                    || (s_month == 11 && e_date > 30))
                {
                    s_date = 30;
                }
                if (s_month == 0)
                {
                    s_month = 12;
                    s_year = s_year - 1;
                }
                startCreate = s_year + "/" + s_month + "/" + s_date + " 12:59:59";
                endCreate = e_year + "/" + e_month + "/" + e_date + " 12:59:59";
                txtStartCreate.SelectedDate = DateTime.Parse(startCreate);
                txtEndCreate.SelectedDate = DateTime.Parse(endCreate);
            }
        }

        #region Query Report

        #region Query

        protected void btnQueryReport_Click(object sender, EventArgs e)
        {
            InitRepeaterDataSource();
            DataTable data = new DataTable();
            switch (dpQueryType.Value)
            {
                case "Opex":
                    data = GetPaymentRequestDataByList();
                    PaymentRequestReportData.DataSource = data;
                    PaymentRequestReportData.DataBind();
                    break;
                case "Capex":
                    data = GetPaymentRequestDataByList();
                    PaymentRequestReportData.DataSource = data;
                    PaymentRequestReportData.DataBind();
                    break;
                case "TravelExpenseClaim":
                    data = GetTravelExpenseClaimDataByList();
                    TravelExpenseClaimData.DataSource = data;
                    TravelExpenseClaimData.DataBind();
                    break;
                case "CreditCardClaim":
                    data = GetCreditCardClaimDataByList();
                    CreditCardClaimData.DataSource = data;
                    CreditCardClaimData.DataBind();
                    break;
                case "EmployeeExpenseClaim":
                    data = GetEmployeeExpenseClaimDataByList();
                    EmployeeExpenseClaimData.DataSource = data;
                    EmployeeExpenseClaimData.DataBind();
                    break;
                case "ExpatriateBenefitClaim":
                    data = GetExpatriateBenefitClaimDataByList();
                    ExpatriateBenefitClaimData.DataSource = data;
                    ExpatriateBenefitClaimData.DataBind();
                    break;
                case "CashAdvanceRequest":
                    data = GetCashAdvanceDataByList();
                    CashAdvanceReportData.DataSource = data;
                    CashAdvanceReportData.DataBind();
                    break;
            }
        }

        private void InitField()
        {
            queryType = dpQueryType.Value;
            status = dpStatus.Value;
            department = txtDepartment.Value.Trim();
            if (hfpaidstatus.Value == "1")
            {
                paidDate = txtPaidDate.SelectedDate.ToString("yyyy-MM-dd");
            }
            applicant = txtApplicant.Value.Trim();
            vendorName = txtVendorName.Value.Trim();
            startNO = Int32.Parse(txtStartNO.Value.Trim() == "" ? "0" : txtStartNO.Value.Trim().ToLower()
                                                                                         .Replace("pr", "00")
                                                                                         .Replace("te", "00")
                                                                                         .Replace("ccc", "00")
                                                                                         .Replace("eec", "00")
                                                                                         .Replace("ebc", "00")
                                                                                         .Replace("ca", "00")
                                                                                         .Replace("_", ""));
            endNO = Int32.Parse(txtEndNO.Value.Trim() == "" ? "0" : txtEndNO.Value.Trim().ToLower()
                                                                                         .Replace("pr", "00")
                                                                                         .Replace("te", "00")
                                                                                         .Replace("ccc", "00")
                                                                                         .Replace("eec", "00")
                                                                                         .Replace("ebc", "00")
                                                                                         .Replace("ca", "00")
                                                                                         .Replace("_", ""));
            DateTime.TryParse(txtStartCreate.SelectedDate.ToString("yyyy-M-d  00:00:00"), out startCreate);
            DateTime.TryParse(txtEndCreate.SelectedDate.ToString("yyyy-M-d  23:59:59"), out endCreate);
            startAmount = decimal.Parse(txtStartAmount.Value.Trim() == "" ? "0" : txtStartAmount.Value.Trim());
            endAmount = decimal.Parse(txtEndAmount.Value.Trim() == "" ? "0" : txtEndAmount.Value.Trim());
            startVendorNO = Int32.Parse(txtStartVendorNO.Value.Trim() == "" ? "0" : txtStartVendorNO.Value.Trim());
            endVendorNO = Int32.Parse(txtEndVendorNO.Value.Trim() == "" ? "0" : txtEndVendorNO.Value.Trim());
            pono = txtSystemPONO.Value.Trim();
        }

        private void InitRepeaterDataSource()
        {
            PaymentRequestReportData.DataSource = null;
            PaymentRequestReportData.DataBind();
            TravelExpenseClaimData.DataSource = null;
            TravelExpenseClaimData.DataBind();
            CreditCardClaimData.DataSource = null;
            CreditCardClaimData.DataBind();
            EmployeeExpenseClaimData.DataSource = null;
            EmployeeExpenseClaimData.DataBind();
            ExpatriateBenefitClaimData.DataSource = null;
            ExpatriateBenefitClaimData.DataBind();
            CashAdvanceReportData.DataSource = null;
            CashAdvanceReportData.DataBind();
        }

        public string GetEmployeeIDAndName(string appliacant)
        {
            string strEmployeeIDAndName = string.Empty;
            try
            {
                if (appliacant.IndexOf("(") != -1)
                {
                    appliacant = appliacant.Substring(appliacant.IndexOf('(') + 1, appliacant.IndexOf(')') - appliacant.IndexOf('(') - 1);
                }
                Employee employee = UserProfileUtil.GetEmployeeEx(appliacant);
                if (employee != null)
                {
                    strEmployeeIDAndName = employee.EmployeeID + ";" + employee.PreferredName;
                }
            }
            catch (Exception ex)
            {
                strEmployeeIDAndName = ";";
            }
            return strEmployeeIDAndName;
        }

        #endregion

        #region Travel Expense Claim

        private DataTable GetTravelExpenseClaimDataByList()
        {
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Travel Expense Claim");
            SPQuery query = new SPQuery();
            //EmployeeID EmployeeName
            query.Query = "<OrderBy>" +
                                "<FieldRef Name='Created' Ascending='False' />" +
                          "</OrderBy>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ID' />" +
                               "<FieldRef Name='Applicant' />" +
                               "<FieldRef Name='Created' />" +
                               "<FieldRef Name='Purpose' />" +
                               "<FieldRef Name='TotalCost' />" +
                               "<FieldRef Name='Status' />" +
                               "<FieldRef Name='Department' />" +
                               "<FieldRef Name='SAPNo' />" +
                               "<FieldRef Name='PaidDate' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();

                #region
                SPList delegationList1 = CA.SharePoint.SharePointUtil.GetList("Travel Expense Claim For SAP");
                SPQuery query1 = new SPQuery();
                query1.ViewFields = "<FieldRef Name='TCWorkflowNumber' />" +
                                   "<FieldRef Name='SAPNo' />";
                SPListItemCollection listItems1 = null;
                listItems1 = delegationList1.GetItems(query1);
                DataTable dt = new DataTable();
                if (listItems1.Count > 0)
                {
                    dt = listItems1.GetDataTable();
                }
                var item = from dts in dt.AsEnumerable()
                           join datas in data.AsEnumerable() on dts.Field<string>("TCWorkflowNumber")
                                                         equals datas.Field<string>("Title")
                           select new
                           {
                               dts,
                               datas
                           };
                DataTable returnTable = data.Clone();
                foreach (var d in item)
                {
                    DataRow dr = returnTable.NewRow();
                    dr["ID"] = d.datas.Field<int>("ID").ToString();
                    dr["Title"] = d.datas.Field<string>("Title").AsString();
                    dr["Applicant"] = d.datas.Field<string>("Applicant").AsString();
                    dr["Created"] = d.datas.Field<DateTime>("Created").ToString();
                    dr["Purpose"] = d.datas.Field<string>("Purpose").AsString();
                    dr["TotalCost"] = d.datas.Field<double>("TotalCost").ToString();
                    dr["Status"] = d.datas.Field<string>("Status").AsString();
                    dr["Department"] = d.datas.Field<string>("Department").AsString();
                    dr["PaidDate"] = d.datas.Field<string>("PaidDate").AsString();
                    dr["SAPNo"] = d.dts.Field<string>("SAPNo").AsString();
                    returnTable.Rows.Add(dr);
                }
                data = returnTable;
                #endregion

                data = GetTravelExpenseClaimDataByQuery(data);
            }

            return data;
        }

        private DataTable GetTravelExpenseClaimDataByQuery(DataTable table)
        {
            DataTable data = null;
            InitField();
            EnumerableRowCollection<DataRow> row = table.AsEnumerable();
            if (status.IsNotNullOrWhitespace())
            {
                if (row.Count() > 0)
                {
                    data = row.CopyToDataTable();
                }
                else
                {
                    return null;
                }
                data = GetTravelExpenseClaimDataByStatus(data, status);
                if (null == data)
                {
                    return null;
                }
                row = data.AsEnumerable();
            }
            if (department.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Department").AsString().ToLower().Contains(department.ToLower()));
            }
            if (paidDate.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("PaidDate").AsString().ToLower().Contains(paidDate));
            }
            if (applicant.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant.ToLower()));
            }

            if (startNO != 0)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("te", "00")) >= startNO
                                   && Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("te", "00")) <= endNO);
            }
            if (startCreate != DateTime.Parse("0001-01-01"))
            {
                row = row.Where(dr => dr.Field<DateTime>("Created") >= startCreate
                                   && dr.Field<DateTime>("Created") <= endCreate);
            }
            if (startAmount != 0)
            {
                row = row.Where(dr => decimal.Parse(dr.Field<double>("TotalCost").ToString()) >= startAmount
                                   && decimal.Parse(dr.Field<double>("TotalCost").ToString()) <= endAmount);
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        private DataTable GetTravelExpenseClaimDataByStatus(DataTable table, string status)
        {
            DataTable data = table;
            EnumerableRowCollection<DataRow> row = data.AsEnumerable();
            switch (status)
            {
                case "Paid":
                    row = row.Where(dr => dr.Field<string>("PaidDate").AsString().IsNotNullOrWhitespace());
                    break;
                case "Complete":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Posted to SAP":
                    #region
                    SPList delegationList = CA.SharePoint.SharePointUtil.GetList("Travel Expense Claim For SAP");
                    SPQuery query = new SPQuery();
                    query.ViewFields = "<FieldRef Name='TCWorkflowNumber' />" +
                                       "<FieldRef Name='SAPNo' />";
                    SPListItemCollection listItems = null;
                    query.Query = @"<Where>
                                                              <IsNotNull>
                                                                 <FieldRef Name='SAPNo' />
                                                              </IsNotNull>
                                                        </Where>";
                    listItems = delegationList.GetItems(query);
                    DataTable dt = new DataTable();
                    if (listItems.Count > 0)
                    {
                        dt = listItems.GetDataTable();
                    }
                    var item = from dts in dt.AsEnumerable()
                               join datas in data.AsEnumerable() on dts.Field<string>("TCWorkflowNumber")
                                                             equals datas.Field<string>("Title")
                               select new
                               {
                                   dts,
                                   datas
                               };
                    DataTable returnTable = data.Clone();
                    foreach (var d in item)
                    {
                        DataRow dr = returnTable.NewRow();
                        dr["ID"] = d.datas.Field<int>("ID").ToString();
                        dr["Title"] = d.datas.Field<string>("Title").AsString();
                        dr["Applicant"] = d.datas.Field<string>("Applicant").AsString();
                        dr["Created"] = d.datas.Field<DateTime>("Created").ToString();
                        dr["Purpose"] = d.datas.Field<string>("Purpose").AsString();
                        dr["TotalCost"] = d.datas.Field<double>("TotalCost").ToString();
                        dr["Status"] = d.datas.Field<string>("Status").AsString();
                        dr["Department"] = d.datas.Field<string>("Department").AsString();
                        dr["PaidDate"] = d.datas.Field<string>("PaidDate").AsString();
                        dr["SAPNo"] = d.dts.Field<string>("SAPNo").AsString();
                        returnTable.Rows.Add(dr);
                    }
                    data = returnTable;
                    row = data.AsEnumerable();
                    #endregion
                    break;
                case "Rejected":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Rejected", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Pending":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Pending", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "In Progress":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("In Progress", StringComparison.CurrentCultureIgnoreCase));
                    break;
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        #endregion

        #region Credit Card Claim

        private DataTable GetCreditCardClaimDataByList()
        {
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim Workflow");
            SPQuery query = new SPQuery();
            //EmployeeID EmployeeName
            query.Query = "<OrderBy>" +
                                "<FieldRef Name='Created' Ascending='False' />" +
                          "</OrderBy>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ID' />" +
                               "<FieldRef Name='Applicant' />" +
                               "<FieldRef Name='Created' />" +
                               "<FieldRef Name='ExpenseDescription' />" +
                               "<FieldRef Name='ApproveAmount' />" +
                               "<FieldRef Name='Status' />" +
                               "<FieldRef Name='SAPNo' />" +
                               "<FieldRef Name='SAPUSDNo' />" +
                               "<FieldRef Name='Department' />" +
                               "<FieldRef Name='PaidDate' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();
                data = GetCreditCardClaimDataByQuery(data);
            }
            return data;
        }

        private DataTable GetCreditCardClaimDataByQuery(DataTable table)
        {
            DataTable data = null;
            InitField();
            EnumerableRowCollection<DataRow> row = table.AsEnumerable();
            if (status.IsNotNullOrWhitespace())
            {
                if (row.Count() > 0)
                {
                    data = row.CopyToDataTable();
                }
                else
                {
                    return null;
                }
                data = GetCreditCardClaimDataByStatus(data, status);
                if (null == data)
                {
                    return null;
                }
                row = data.AsEnumerable();
            }
            if (department.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Department").AsString().ToLower().Contains(department.ToLower()));
            }
            if (paidDate.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("PaidDate").AsString().ToLower().Contains(paidDate));
            }
            if (applicant.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant.ToLower()));
            }

            if (startNO != 0)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("ccc", "00")) >= startNO
                                   && Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("ccc", "00")) <= endNO);
            }
            if (startCreate != DateTime.Parse("0001-01-01"))
            {
                row = row.Where(dr => dr.Field<DateTime>("Created") >= startCreate
                                   && dr.Field<DateTime>("Created") <= endCreate);
            }
            if (startAmount != 0)
            {
                row = row.Where(dr => decimal.Parse(dr.Field<double>("ApproveAmount").ToString()) >= startAmount
                                   && decimal.Parse(dr.Field<double>("ApproveAmount").ToString()) <= endAmount);
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        private DataTable GetCreditCardClaimDataByStatus(DataTable table, string status)
        {
            DataTable data = table;
            EnumerableRowCollection<DataRow> row = data.AsEnumerable();
            switch (status)
            {
                case "Paid":
                    row = row.Where(dr => dr.Field<string>("PaidDate").AsString().IsNotNullOrWhitespace());
                    break;
                case "Complete":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Posted to SAP":
                    row = row.Where(dr => dr.Field<string>("SAPNo").AsString().IsNotNullOrWhitespace()
                                        || dr.Field<string>("SAPUSDNo").AsString().IsNotNullOrWhitespace());
                    break;
                case "Rejected":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Rejected", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Pending":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Pending", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "In Progress":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("In Progress", StringComparison.CurrentCultureIgnoreCase));
                    break;
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        #endregion

        #region Employee Expense Claim

        private DataTable GetEmployeeExpenseClaimDataByList()
        {
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Employee Expense Claim Workflow");
            SPQuery query = new SPQuery();
            //EmployeeID EmployeeName
            query.Query = "<OrderBy>" +
                                "<FieldRef Name='Created' Ascending='False' />" +
                          "</OrderBy>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ID' />" +
                               "<FieldRef Name='RequestedBy' />" +
                               "<FieldRef Name='Created' />" +
                               "<FieldRef Name='ExpenseDescription' />" +
                               "<FieldRef Name='TotalAmount' />" +
                               "<FieldRef Name='Status' />" +
                               "<FieldRef Name='SAPNumber' />" +
                               "<FieldRef Name='Department' />" +
                               "<FieldRef Name='PaidDate' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();
                data = GetEmployeeExpenseClaimDataByQuery(data);
            }
            return data;
        }

        private DataTable GetEmployeeExpenseClaimDataByQuery(DataTable table)
        {
            DataTable data = null;
            InitField();
            EnumerableRowCollection<DataRow> row = table.AsEnumerable();
            if (status.IsNotNullOrWhitespace())
            {
                if (row.Count() > 0)
                {
                    data = row.CopyToDataTable();
                }
                else
                {
                    return null;
                }
                data = GetEmployeeExpenseClaimDataByStatus(data, status);
                if (null == data)
                {
                    return null;
                }
                row = data.AsEnumerable();
            }
            if (department.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Department").AsString().ToLower().Contains(department.ToLower()));
            }
            if (paidDate.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("PaidDate").AsString().ToLower().Contains(paidDate));
            }
            if (applicant.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("RequestedBy").AsString().ToLower().Contains(applicant.ToLower()));
            }

            if (startNO != 0)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("eec", "00")) >= startNO
                                   && Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("eec", "00")) <= endNO);
            }
            if (startCreate != DateTime.Parse("0001-01-01"))
            {
                row = row.Where(dr => dr.Field<DateTime>("Created") >= startCreate
                                   && dr.Field<DateTime>("Created") <= endCreate);
            }
            if (startAmount != 0)
            {
                row = row.Where(dr => decimal.Parse(dr.Field<double>("TotalAmount").ToString()) >= startAmount
                                   && decimal.Parse(dr.Field<double>("TotalAmount").ToString()) <= endAmount);
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        private DataTable GetEmployeeExpenseClaimDataByStatus(DataTable table, string status)
        {
            DataTable data = table;
            EnumerableRowCollection<DataRow> row = data.AsEnumerable();
            switch (status)
            {
                case "Paid":
                    row = row.Where(dr => dr.Field<string>("PaidDate").AsString().IsNotNullOrWhitespace());
                    break;
                case "Complete":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Posted to SAP":
                    row = row.Where(dr => dr.Field<string>("SAPNumber").AsString().IsNotNullOrWhitespace());
                    break;
                case "Rejected":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Rejected", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Pending":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Pending", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "In Progress":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("In Progress", StringComparison.CurrentCultureIgnoreCase));
                    break;
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        #endregion

        #region Expatriate Benefit Claim

        private DataTable GetExpatriateBenefitClaimDataByList()
        {
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Expatriate Benefit Claim Workflow");
            SPQuery query = new SPQuery();
            //EmployeeID EmployeeName
            query.Query = "<OrderBy>" +
                                "<FieldRef Name='Created' Ascending='False' />" +
                          "</OrderBy>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ID' />" +
                               "<FieldRef Name='Applicant' />" +
                               "<FieldRef Name='Created' />" +
                               "<FieldRef Name='ExpenseDescription' />" +
                               "<FieldRef Name='TotalAmount' />" +
                               "<FieldRef Name='Status' />" +
                               "<FieldRef Name='SAPNo' />" +
                               "<FieldRef Name='Department' />" +
                               "<FieldRef Name='PaidDate' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();
                data = GetExpatriateBenefitClaimDataByQuery(data);
            }
            return data;
        }

        private DataTable GetExpatriateBenefitClaimDataByQuery(DataTable table)
        {
            DataTable data = null;
            InitField();
            EnumerableRowCollection<DataRow> row = table.AsEnumerable();
            if (status.IsNotNullOrWhitespace())
            {
                if (row.Count() > 0)
                {
                    data = row.CopyToDataTable();
                }
                else
                {
                    return null;
                }
                data = GetExpatriateBenefitClaimDataByStatus(data, status);
                if (null == data)
                {
                    return null;
                }
                row = data.AsEnumerable();
            }
            if (department.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Department").AsString().ToLower().Contains(department.ToLower()));
            }
            if (paidDate.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("PaidDate").AsString().ToLower().Contains(paidDate));
            }
            if (applicant.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant.ToLower()));
            }

            if (startNO != 0)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("ebc", "00")) >= startNO
                                   && Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("ebc", "00")) <= endNO);
            }
            if (startCreate != DateTime.Parse("0001-01-01"))
            {
                row = row.Where(dr => dr.Field<DateTime>("Created") >= startCreate
                                   && dr.Field<DateTime>("Created") <= endCreate);
            }
            if (startAmount != 0)
            {
                row = row.Where(dr => decimal.Parse(dr.Field<double>("TotalAmount").ToString()) >= startAmount
                                   && decimal.Parse(dr.Field<double>("TotalAmount").ToString()) <= endAmount);
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        private DataTable GetExpatriateBenefitClaimDataByStatus(DataTable table, string status)
        {
            DataTable data = table;
            EnumerableRowCollection<DataRow> row = data.AsEnumerable();
            switch (status)
            {
                case "Paid":
                    row = row.Where(dr => dr.Field<string>("PaidDate").AsString().IsNotNullOrWhitespace());
                    break;
                case "Complete":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Posted to SAP":
                    row = row.Where(dr => dr.Field<string>("SAPNo").AsString().IsNotNullOrWhitespace());
                    break;
                case "Rejected":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Rejected", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Pending":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Pending", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "In Progress":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("In Progress", StringComparison.CurrentCultureIgnoreCase));
                    break;
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        #endregion

        #region Cash Advance

        private DataTable GetCashAdvanceDataByList()
        {
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequest");
            SPQuery query = new SPQuery();
            //EmployeeID EmployeeName
            query.Query = "<OrderBy>" +
                                "<FieldRef Name='Created' Ascending='False' />" +
                          "</OrderBy>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ID' />" +
                               "<FieldRef Name='Applicant' />" +
                               "<FieldRef Name='Created' />" +
                               "<FieldRef Name='Purpose' />" +
                               "<FieldRef Name='Amount' />" +
                               "<FieldRef Name='Status' />" +
                               "<FieldRef Name='SAPNumber' />" +
                               "<FieldRef Name='Department' />" +
                               "<FieldRef Name='PaidDate' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();
                data = GetCashAdvanceDataByQuery(data);
            }
            return data;
        }

        private DataTable GetCashAdvanceDataByQuery(DataTable table)
        {
            DataTable data = null;
            InitField();
            EnumerableRowCollection<DataRow> row = table.AsEnumerable();
            if (status.IsNotNullOrWhitespace())
            {
                if (row.Count() > 0)
                {
                    data = row.CopyToDataTable();
                }
                else
                {
                    return null;
                }
                data = GetCashAdvanceDataByStatus(data, status);
                if (null == data)
                {
                    return null;
                }
                row = data.AsEnumerable();
            }
            if (department.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Department").AsString().ToLower().Contains(department.ToLower()));
            }
            if (paidDate.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("PaidDate").AsString().ToLower().Contains(paidDate));
            }
            if (applicant.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant.ToLower()));
            }

            if (startNO != 0)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("ca_", "00")) >= startNO
                                   && Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("ca_", "00")) <= endNO);
            }
            if (startCreate != DateTime.Parse("0001-01-01"))
            {
                row = row.Where(dr => dr.Field<DateTime>("Created") >= startCreate
                                   && dr.Field<DateTime>("Created") <= endCreate);
            }
            if (startAmount != 0)
            {
                row = row.Where(dr => decimal.Parse(dr.Field<double>("Amount").ToString()) >= startAmount
                                   && decimal.Parse(dr.Field<double>("Amount").ToString()) <= endAmount);
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        private DataTable GetCashAdvanceDataByStatus(DataTable table, string status)
        {
            DataTable data = table;
            EnumerableRowCollection<DataRow> row = data.AsEnumerable();
            switch (status)
            {
                case "Paid":
                    row = row.Where(dr => dr.Field<string>("PaidDate").AsString().IsNotNullOrWhitespace());
                    break;
                case "Complete":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Posted to SAP":
                    row = row.Where(dr => dr.Field<string>("SAPNumber").AsString().IsNotNullOrWhitespace());
                    break;
                case "Rejected":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Rejected", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Pending":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Pending", StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "In Progress":
                    row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("In Progress", StringComparison.CurrentCultureIgnoreCase));
                    break;
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                data = null;
            }
            return data;
        }

        #endregion

        #region Payment Request

        private DataTable GetPaymentRequestDataByList()
        {
            DataTable data = null;
            var delegationList = CA.SharePoint.SharePointUtil.GetList("PaymentRequestItems");
            SPQuery query = new SPQuery();
            query.Query = "<OrderBy>" +
                                "<FieldRef Name='Created' Ascending='False' />" +
                          "</OrderBy>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ID' />" +
                               "<FieldRef Name='Applicant' />" +
                               "<FieldRef Name='Created' />" +
                               "<FieldRef Name='VendorNo' />" +
                               "<FieldRef Name='VendorName' />" +
                               "<FieldRef Name='PaymentDesc' />" +
                               "<FieldRef Name='TotalAmount' />" +
                               "<FieldRef Name='Currency' />" +
                               "<FieldRef Name='Status' />" +
                               "<FieldRef Name='RequestType' />" +
                               "<FieldRef Name='SAPNumber' />" +
                               "<FieldRef Name='Dept' />" +
                               "<FieldRef Name='PaidDate' />" +
                               "<FieldRef Name='SubPRNo' />" +
                               "<FieldRef Name='PONo' />" +
                               "<FieldRef Name='SystemPONo' />" +
                               "<FieldRef Name='PaidInd' />" +
                               "<FieldRef Name='IsSystemGR' />" +
                               "<FieldRef Name='IsAllSystemGR' />" +
                               "<FieldRef Name='IsAttachedInvoice' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (listItems.Count > 0)
            {
                data = listItems.GetDataTable();
                data = GetPaymentRequestDataByQuery(data);
            }
            return data;
        }

        private DataTable GetPaymentRequestDataByQuery(DataTable table)
        {
            DataTable data = null;
            InitField();
            EnumerableRowCollection<DataRow> row = table.AsEnumerable();
            if (queryType.Equals("Opex", StringComparison.CurrentCultureIgnoreCase)
                || queryType.Equals("Capex", StringComparison.CurrentCultureIgnoreCase))
            {
                row = row.Where(dr => dr.Field<string>("RequestType").AsString().ToLower().Contains(queryType.ToLower()));
            }

            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else
            {
                return null;
            }
            data = GetPaymentRequestDataByStatus(data, status);
            if (null == data)
            {
                return null;
            }
            row = data.AsEnumerable();

            if (department.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Dept").AsString().ToLower().Contains(department.ToLower()));
            }
            
            if (paidDate.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("PaidDate").AsString().ToLower().Contains(paidDate));
            }
            if (applicant.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant.ToLower()));
            }
            if (vendorName.IsNotNullOrWhitespace())
            {
                row = row.Where(dr => dr.Field<string>("VendorName").AsString().ToLower().Contains(vendorName.ToLower()));
            }
            if (startNO != 0 && txtStartNO.Value.IndexOf("_") == -1)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("pr", "00")) >= startNO
                                   && Int32.Parse(dr.Field<string>("Title").AsString().ToLower().Replace("pr", "00")) <= endNO);
            }
            if (startNO != 0 && txtStartNO.Value.IndexOf("_") != -1)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("SubPRNo").AsString().ToLower().Replace("pr", "00").Replace("_", "")) >= startNO
                                   && Int32.Parse(dr.Field<string>("SubPRNo").AsString().ToLower().Replace("pr", "00").Replace("_", "")) <= endNO);
            }
            
            if (startCreate != DateTime.Parse("0001-01-01"))
            {
                row = row.Where(dr => dr.Field<DateTime>("Created") >= startCreate
                                   && dr.Field<DateTime>("Created") <= endCreate);
            }
            if (startVendorNO != 0)
            {
                row = row.Where(dr => Int32.Parse(dr.Field<string>("VendorNo").AsString() == "" ? "0" : dr.Field<string>("VendorNo").AsString()) >= startVendorNO
                                   && Int32.Parse(dr.Field<string>("VendorNo").AsString() == "" ? "0" : dr.Field<string>("VendorNo").AsString()) <= endVendorNO);
            }
            if (raRMBCurrency.Checked)
            {
                row = row.Where(dr => dr.Field<string>("Currency").AsString() == currency);
            }
            else
            {
                row = row.Where(dr => dr.Field<string>("Currency").AsString() != currency);
            }
            if (pono != "")
            {
                row = row.Where(dr => dr.Field<string>("SystemPONo").AsString() == pono);
            }
            if (row.Count() > 0)
            {
                data = row.CopyToDataTable();
            }
            else 
            {
                data = null;
            }
            return data;
        }

        private DataTable GetPaymentRequestDataByStatus(DataTable table, string status)
        {
            DataTable data = table;
            EnumerableRowCollection<DataRow> row = data.AsEnumerable();
            #region
            SPList delegationList = CA.SharePoint.SharePointUtil.GetList("PaymentInstallment");
            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='PONo' />" +
                               "<FieldRef Name='PaidThisTimeAmount' />" +
                               "<FieldRef Name='TotalAmount' />" +
                               "<FieldRef Name='Paid' />" +
                               "<FieldRef Name='Index' />";
            SPListItemCollection listItems = null;
            listItems = delegationList.GetItems(query);
            DataTable dt = new DataTable();
            EnumerableRowCollection<DataRow> dataRow = null;
            if (listItems.Count > 0)
            {
                dataRow = listItems.GetDataTable().AsEnumerable();
                //int startIndex = 1;
                //int endIndex = 1;
                //if (txtStartNO.Value.IndexOf("_") != -1)
                //{
                //    startIndex = Int32.Parse(txtStartNO.Value.Trim().Substring(txtStartNO.Value.IndexOf("_") + 1, 1));
                //    endIndex = Int32.Parse(txtEndNO.Value.Trim().Substring(txtEndNO.Value.IndexOf("_") + 1, 1));
                //    dataRow = dataRow.Where(dr => Int32.Parse(dr.Field<double>("Index").ToString()) >= startIndex
                //                               && Int32.Parse(dr.Field<double>("Index").ToString()) <= endIndex);
                //} 
                if (startAmount != 0)
                {
                    dataRow = dataRow.Where(dr => decimal.Parse(dr.Field<string>("PaidThisTimeAmount").AsString() == "" ? (double.Parse(dr.Field<string>("TotalAmount").AsString()) * (double.Parse(dr.Field<string>("Paid").AsString()) / 100)).ToString() : dr.Field<string>("PaidThisTimeAmount").AsString()) >= startAmount
                                               && decimal.Parse(dr.Field<string>("PaidThisTimeAmount").AsString() == "" ? (double.Parse(dr.Field<string>("TotalAmount").AsString()) * (double.Parse(dr.Field<string>("Paid").AsString()) / 100)).ToString() : dr.Field<string>("PaidThisTimeAmount").AsString()) <= endAmount);

                }
            }
            var item = from dts in dataRow
                       from datas in data.AsEnumerable()
                       where dts.Field<string>("PONo").ToString() == datas.Field<string>("PONo").ToString()
                          && dts.Field<double>("Index").ToString() == datas.Field<string>("PaidInd").ToString()
                       select new
                       {
                           dts,
                           datas
                       };
            DataTable returnTable = data.Clone();
            foreach (var d in item)
            {
                DataRow dr = returnTable.NewRow();
                dr["Title"] = d.datas.Field<string>("Title").AsString();
                dr["ID"] = d.datas.Field<int>("ID").ToString();
                dr["Applicant"] = d.datas.Field<string>("Applicant").AsString();
                dr["Created"] = d.datas.Field<DateTime>("Created").ToString();
                dr["VendorNo"] = d.datas.Field<string>("VendorNo").AsString();
                dr["VendorName"] = d.datas.Field<string>("VendorName").AsString();
                dr["PaymentDesc"] = d.datas.Field<string>("PaymentDesc").AsString();
                dr["TotalAmount"] = d.dts.Field<string>("PaidThisTimeAmount").AsString() == "" ? (double.Parse(d.dts.Field<string>("TotalAmount").AsString()) * (double.Parse(d.dts.Field<string>("Paid").AsString()) / 100)).ToString() : d.dts.Field<string>("PaidThisTimeAmount").AsString();
                dr["Currency"] = d.datas.Field<string>("Currency").AsString();
                dr["Status"] = d.datas.Field<string>("Status").AsString();
                dr["SAPNumber"] = d.datas.Field<string>("SAPNumber").AsString();
                dr["RequestType"] = d.datas.Field<string>("RequestType").AsString();
                dr["PaidDate"] = d.datas.Field<string>("PaidDate").AsString();
                dr["SubPRNo"] = d.datas.Field<string>("SubPRNo").AsString();
                dr["Dept"] = d.datas.Field<string>("Dept").AsString();
                dr["PaidInd"] = d.datas.Field<string>("PaidInd").AsString();
                dr["PONo"] = d.datas.Field<string>("PONo").AsString();
                dr["SystemPONo"] = d.datas.Field<string>("SystemPONo").AsString();
                dr["IsAttachedInvoice"] = d.datas.Field<string>("IsAttachedInvoice").AsString();
                dr["IsSystemGR"] = d.datas.Field<string>("IsSystemGR").AsString();
                dr["IsAllSystemGR"] = d.datas.Field<string>("IsAllSystemGR").AsString();
                returnTable.Rows.Add(dr);
            }
            data = returnTable;
            #endregion
            row = data.AsEnumerable();

            if (row.Count() > 0)
            {
                switch (status)
                {
                    case "Paid":
                        row = row.Where(dr => dr.Field<string>("PaidDate").AsString().IsNotNullOrWhitespace());
                        break;
                    case "Complete":
                        row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase));
                        break;
                    case "Posted to SAP":
                        row = row.Where(dr => dr.Field<string>("SAPNumber").AsString().IsNotNullOrWhitespace()
                                           || dr.Field<string>("IsSystemGR").AsString() == "1"
                                           || dr.Field<string>("IsAllSystemGR").AsString() == "1");
                        break;
                    case "Rejected":
                        row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Rejected", StringComparison.CurrentCultureIgnoreCase));
                        break;
                    case "Pending":
                        row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("Pending", StringComparison.CurrentCultureIgnoreCase));
                        break;
                    case "In Progress":
                        row = row.Where(dr => dr.Field<string>("Status").AsString().Equals("In Progress", StringComparison.CurrentCultureIgnoreCase));
                        break;
                }
                if (row.Count() > 0)
                {
                    data = row.CopyToDataTable();
                }
                else 
                {
                    data = null;
                }
               
            }
            else
            {
                data = null;
            }
            return data;
        }

        #endregion

        #endregion

        #region Export Excel

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            switch (dpQueryType.Value)
            {
                case "Opex":
                    data = GetExportPaymentRequestItems();
                    break;
                case "Capex":
                    data = GetExportPaymentRequestItems();
                    break;
                case "TravelExpenseClaim":
                    data = GetExportTravelExpenseClaimItems();
                    break;
                case "CreditCardClaim":
                    data = GetExportCreditCardClaimItems();
                    break;
                case "EmployeeExpenseClaim":
                    data = GetExportEmployeeExpenseClaimItems();
                    break;
                case "ExpatriateBenefitClaim":
                    data = GetExportExpatriateBenefitClaimItems();
                    break;
                case "CashAdvanceRequest":
                    data = GetExportCashAdvanceItems();
                    break;
            }
            ExportExcel(data);
        }

        #region Create DataTable

        private DataTable CreateExportDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Applicant");
            table.Columns.Add("Title");
            table.Columns.Add("Created");
            table.Columns.Add("VendorNo");
            table.Columns.Add("VendorName");
            table.Columns.Add("PaymentDesc");
            table.Columns.Add("TotalAmount");
            table.Columns.Add("Currency");
            table.Columns.Add("Status");
            table.Columns.Add("PaidDate");
            table.Columns.Add("SystemPONo");
            table.Columns.Add("PONo");
            table.Columns.Add("IsAttachedInvoice");
            table.Columns.Add("SAPNumber");
            table.Columns.Add("Department");
            return table;
        }

        #endregion

        #region Get Export Items

        private DataTable GetExportPaymentRequestItems()
        {
            DataTable table = CreateExportDataTable();
            foreach (RepeaterItem item in this.PaymentRequestReportData.Items)
            {
                var ckbItems = (CheckBox)item.FindControl("ckbItems");
                var lblApplicant = (Label)item.FindControl("lblApplicant");
                var lblSubPRNo = (Label)item.FindControl("lblSubPRNo");
                var lblPONO = (Label)item.FindControl("lblPONO");
                //var lblNewPONO = (Label)item.FindControl("lblNewPONO");
                var lblCreated = (Label)item.FindControl("lblCreated");
                var lblVendorNo = (Label)item.FindControl("lblVendorNo");
                var lblVendorName = (Label)item.FindControl("lblVendorName");
                var lblPaymentDesc = (Label)item.FindControl("lblPaymentDesc");
                var lblTotalAmount = (Label)item.FindControl("lblTotalAmount");
                var lblCurrency = (Label)item.FindControl("lblCurrency");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var lblPaidDate = (Label)item.FindControl("lblPaidDate");
                var lblIsAttachedInvoice = (Label)item.FindControl("lblIsAttachedInvoice");

                var lblDepartment = (Label)item.FindControl("lblDepartment");
                var lblSAPNO = (Label)item.FindControl("lblSAPNO");

                if (ckbItems.Checked)
                {
                    DataRow row = table.Rows.Add();
                    row["Applicant"] = lblApplicant.Text.Trim();
                    row["Title"] = lblSubPRNo.Text.Trim();
                    row["SystemPONo"] = lblPONO.Text.Trim();
                    //row["PONo"] = lblNewPONO.Text.Trim();
                    row["Created"] = lblCreated.Text.Trim();
                    row["VendorNo"] = lblVendorNo.Text.Trim();
                    row["VendorName"] = lblVendorName.Text.Trim();
                    row["PaymentDesc"] = lblPaymentDesc.Text.Trim();
                    row["TotalAmount"] = lblTotalAmount.Text.Trim();
                    row["Currency"] = lblCurrency.Text.Trim();
                    row["Status"] = dpStatus.Value == "" ? lblStatus.Text.Trim() : dpStatus.Value;
                    row["PaidDate"] = lblPaidDate.Text.Trim();
                    row["IsAttachedInvoice"] = lblIsAttachedInvoice.Text.Trim();

                    row["SAPNumber"] = lblSAPNO.Text.Trim();
                    row["Department"] = lblDepartment.Text.Trim();
                }
            }
            return table;
        }

        private DataTable GetExportTravelExpenseClaimItems()
        {
            DataTable table = CreateExportDataTable();
            foreach (RepeaterItem item in this.TravelExpenseClaimData.Items)
            {
                var ckbItems = (CheckBox)item.FindControl("ckbItems");
                var lblApplicant = (Label)item.FindControl("lblApplicant");
                var lblSubPRNo = (Label)item.FindControl("lblSubPRNo");
                var lblCreated = (Label)item.FindControl("lblCreated");
                var lblVendorNo = (Label)item.FindControl("lblVendorNo");
                var lblVendorName = (Label)item.FindControl("lblVendorName");
                var lblPaymentDesc = (Label)item.FindControl("lblPaymentDesc");
                var lblTotalAmount = (Label)item.FindControl("lblTotalAmount");
                var lblCurrency = (Label)item.FindControl("lblCurrency");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var lblPaidDate = (Label)item.FindControl("lblPaidDate");

                var lblDepartment = (Label)item.FindControl("lblDepartment");
                var lblSAPNO = (Label)item.FindControl("lblSAPNO");

                if (ckbItems.Checked)
                {
                    DataRow row = table.Rows.Add();
                    row["Applicant"] = lblApplicant.Text.Trim();
                    row["Title"] = lblSubPRNo.Text.Trim();
                    row["Created"] = lblCreated.Text.Trim();
                    row["VendorNo"] = lblVendorNo.Text.Trim();
                    row["VendorName"] = lblVendorName.Text.Trim();
                    row["PaymentDesc"] = lblPaymentDesc.Text.Trim();
                    row["TotalAmount"] = lblTotalAmount.Text.Trim();
                    row["Currency"] = lblCurrency.Text.Trim();
                    row["Status"] = dpStatus.Value == "" ? lblStatus.Text.Trim() : dpStatus.Value;
                    row["PaidDate"] = lblPaidDate.Text.Trim();

                    row["SAPNumber"] = lblSAPNO.Text.Trim();
                    row["Department"] = lblDepartment.Text.Trim();
                }
            }
            return table;
        }

        private DataTable GetExportCreditCardClaimItems()
        {
            DataTable table = CreateExportDataTable();
            foreach (RepeaterItem item in this.CreditCardClaimData.Items)
            {
                var ckbItems = (CheckBox)item.FindControl("ckbItems");
                var lblApplicant = (Label)item.FindControl("lblApplicant");
                var lblSubPRNo = (Label)item.FindControl("lblSubPRNo");
                var lblCreated = (Label)item.FindControl("lblCreated");
                var lblVendorNo = (Label)item.FindControl("lblVendorNo");
                var lblVendorName = (Label)item.FindControl("lblVendorName");
                var lblPaymentDesc = (Label)item.FindControl("lblPaymentDesc");
                var lblTotalAmount = (Label)item.FindControl("lblTotalAmount");
                var lblCurrency = (Label)item.FindControl("lblCurrency");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var lblPaidDate = (Label)item.FindControl("lblPaidDate");

                var lblDepartment = (Label)item.FindControl("lblDepartment");
                var lblSAPNO = (Label)item.FindControl("lblSAPNO");

                if (ckbItems.Checked)
                {
                    DataRow row = table.Rows.Add();
                    row["Applicant"] = lblApplicant.Text.Trim();
                    row["Title"] = lblSubPRNo.Text.Trim();
                    row["Created"] = lblCreated.Text.Trim();
                    row["VendorNo"] = lblVendorNo.Text.Trim();
                    row["VendorName"] = lblVendorName.Text.Trim();
                    row["PaymentDesc"] = lblPaymentDesc.Text.Trim();
                    row["TotalAmount"] = lblTotalAmount.Text.Trim();
                    row["Currency"] = lblCurrency.Text.Trim();
                    row["Status"] = dpStatus.Value == "" ? lblStatus.Text.Trim() : dpStatus.Value;
                    row["PaidDate"] = lblPaidDate.Text.Trim();

                    row["SAPNumber"] = lblSAPNO.Text.Trim();
                    row["Department"] = lblDepartment.Text.Trim();
                }
            }
            return table;
        }

        private DataTable GetExportEmployeeExpenseClaimItems()
        {
            DataTable table = CreateExportDataTable();
            foreach (RepeaterItem item in this.EmployeeExpenseClaimData.Items)
            {
                var ckbItems = (CheckBox)item.FindControl("ckbItems");
                var lblApplicant = (Label)item.FindControl("lblApplicant");
                var lblSubPRNo = (Label)item.FindControl("lblSubPRNo");
                var lblCreated = (Label)item.FindControl("lblCreated");
                var lblVendorNo = (Label)item.FindControl("lblVendorNo");
                var lblVendorName = (Label)item.FindControl("lblVendorName");
                var lblPaymentDesc = (Label)item.FindControl("lblPaymentDesc");
                var lblTotalAmount = (Label)item.FindControl("lblTotalAmount");
                var lblCurrency = (Label)item.FindControl("lblCurrency");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var lblPaidDate = (Label)item.FindControl("lblPaidDate");

                var lblDepartment = (Label)item.FindControl("lblDepartment");
                var lblSAPNO = (Label)item.FindControl("lblSAPNO");

                if (ckbItems.Checked)
                {
                    DataRow row = table.Rows.Add();
                    row["Applicant"] = lblApplicant.Text.Trim();
                    row["Title"] = lblSubPRNo.Text.Trim();
                    row["Created"] = lblCreated.Text.Trim();
                    row["VendorNo"] = lblVendorNo.Text.Trim();
                    row["VendorName"] = lblVendorName.Text.Trim();
                    row["PaymentDesc"] = lblPaymentDesc.Text.Trim();
                    row["TotalAmount"] = lblTotalAmount.Text.Trim();
                    row["Currency"] = lblCurrency.Text.Trim();
                    row["Status"] = dpStatus.Value == "" ? lblStatus.Text.Trim() : dpStatus.Value;
                    row["PaidDate"] = lblPaidDate.Text.Trim();

                    row["SAPNumber"] = lblSAPNO.Text.Trim();
                    row["Department"] = lblDepartment.Text.Trim();
                }
            }
            return table;
        }

        private DataTable GetExportExpatriateBenefitClaimItems()
        {
            DataTable table = CreateExportDataTable();
            foreach (RepeaterItem item in this.ExpatriateBenefitClaimData.Items)
            {
                var ckbItems = (CheckBox)item.FindControl("ckbItems");
                var lblApplicant = (Label)item.FindControl("lblApplicant");
                var lblSubPRNo = (Label)item.FindControl("lblSubPRNo");
                var lblCreated = (Label)item.FindControl("lblCreated");
                var lblVendorNo = (Label)item.FindControl("lblVendorNo");
                var lblVendorName = (Label)item.FindControl("lblVendorName");
                var lblPaymentDesc = (Label)item.FindControl("lblPaymentDesc");
                var lblTotalAmount = (Label)item.FindControl("lblTotalAmount");
                var lblCurrency = (Label)item.FindControl("lblCurrency");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var lblPaidDate = (Label)item.FindControl("lblPaidDate");

                var lblDepartment = (Label)item.FindControl("lblDepartment");
                var lblSAPNO = (Label)item.FindControl("lblSAPNO");

                if (ckbItems.Checked)
                {
                    DataRow row = table.Rows.Add();
                    row["Applicant"] = lblApplicant.Text.Trim();
                    row["Title"] = lblSubPRNo.Text.Trim();
                    row["Created"] = lblCreated.Text.Trim();
                    row["VendorNo"] = lblVendorNo.Text.Trim();
                    row["VendorName"] = lblVendorName.Text.Trim();
                    row["PaymentDesc"] = lblPaymentDesc.Text.Trim();
                    row["TotalAmount"] = lblTotalAmount.Text.Trim();
                    row["Currency"] = lblCurrency.Text.Trim();
                    row["Status"] = dpStatus.Value == "" ? lblStatus.Text.Trim() : dpStatus.Value;
                    row["PaidDate"] = lblPaidDate.Text.Trim();

                    row["SAPNumber"] = lblSAPNO.Text.Trim();
                    row["Department"] = lblDepartment.Text.Trim();
                }
            }
            return table;
        }

        private DataTable GetExportCashAdvanceItems()
        {
            DataTable table = CreateExportDataTable();
            foreach (RepeaterItem item in this.CashAdvanceReportData.Items)
            {
                var ckbItems = (CheckBox)item.FindControl("ckbItems");
                var lblApplicant = (Label)item.FindControl("lblApplicant");
                var lblSubPRNo = (Label)item.FindControl("lblSubPRNo");
                var lblCreated = (Label)item.FindControl("lblCreated");
                var lblVendorNo = (Label)item.FindControl("lblVendorNo");
                var lblVendorName = (Label)item.FindControl("lblVendorName");
                var lblPaymentDesc = (Label)item.FindControl("lblPaymentDesc");
                var lblTotalAmount = (Label)item.FindControl("lblTotalAmount");
                var lblCurrency = (Label)item.FindControl("lblCurrency");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var lblPaidDate = (Label)item.FindControl("lblPaidDate");

                var lblDepartment = (Label)item.FindControl("lblDepartment");
                var lblSAPNO = (Label)item.FindControl("lblSAPNO");

                if (ckbItems.Checked)
                {
                    DataRow row = table.Rows.Add();
                    row["Applicant"] = lblApplicant.Text.Trim();
                    row["Title"] = lblSubPRNo.Text.Trim();
                    row["Created"] = lblCreated.Text.Trim();
                    row["VendorNo"] = lblVendorNo.Text.Trim();
                    row["VendorName"] = lblVendorName.Text.Trim();
                    row["PaymentDesc"] = lblPaymentDesc.Text.Trim();
                    row["TotalAmount"] = lblTotalAmount.Text.Trim();
                    row["Currency"] = lblCurrency.Text.Trim();
                    row["Status"] = dpStatus.Value == "" ? lblStatus.Text.Trim() : dpStatus.Value;
                    row["PaidDate"] = lblPaidDate.Text.Trim();

                    row["SAPNumber"] = lblSAPNO.Text.Trim();
                    row["Department"] = lblDepartment.Text.Trim();
                }
            }
            return table;
        }

        #endregion

        private void ExportExcel(DataTable exportDataTable)
        {
            string strSampleFileName = string.Empty;
            if (dpQueryType.Value.Equals("Opex", StringComparison.CurrentCultureIgnoreCase) ||
               dpQueryType.Value.Equals("Capex", StringComparison.CurrentCultureIgnoreCase))
            {
                strSampleFileName = "PaymentRequestReportSample.xls";
            }
            else 
            {
                strSampleFileName = "ClaimReportSample.xls";
            }
            string sSaveFileName = new Random().Next() + "_Report" + ".xls";
            string sFullPath = Server.MapPath("/tmpfiles/PaymentRequestReport/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];
            for (int i = 0; i < exportDataTable.Rows.Count; i++)
            {
                if (dpQueryType.Value.Equals("Opex", StringComparison.CurrentCultureIgnoreCase) ||
                    dpQueryType.Value.Equals("Capex", StringComparison.CurrentCultureIgnoreCase))
                {
                    worksheet1.Rows[i + 1].Cells[0].Value = exportDataTable.Rows[i]["Applicant"].ToString();
                    worksheet1.Rows[i + 1].Cells[1].Value = exportDataTable.Rows[i]["Department"].ToString();
                    worksheet1.Rows[i + 1].Cells[2].Value = exportDataTable.Rows[i]["SAPNumber"].ToString();
                    worksheet1.Rows[i + 1].Cells[3].Value = exportDataTable.Rows[i]["Title"].ToString();
                    worksheet1.Rows[i + 1].Cells[4].Value = exportDataTable.Rows[i]["SystemPONo"].ToString();
                    //worksheet1.Rows[i + 1].Cells[5].Value = exportDataTable.Rows[i]["PONo"].ToString();
                    worksheet1.Rows[i + 1].Cells[5].Value = exportDataTable.Rows[i]["Created"].ToString();
                    worksheet1.Rows[i + 1].Cells[6].Value = exportDataTable.Rows[i]["VendorNo"].ToString();
                    worksheet1.Rows[i + 1].Cells[7].Value = exportDataTable.Rows[i]["VendorName"].ToString();
                    worksheet1.Rows[i + 1].Cells[8].Value = exportDataTable.Rows[i]["PaymentDesc"].ToString();
                    worksheet1.Rows[i + 1].Cells[9].Value = exportDataTable.Rows[i]["TotalAmount"].ToString();
                    worksheet1.Rows[i + 1].Cells[10].Value = exportDataTable.Rows[i]["Currency"].ToString();
                    worksheet1.Rows[i + 1].Cells[11].Value = exportDataTable.Rows[i]["Status"].ToString();
                    worksheet1.Rows[i + 1].Cells[12].Value = exportDataTable.Rows[i]["PaidDate"].ToString();
                    worksheet1.Rows[i + 1].Cells[13].Value = exportDataTable.Rows[i]["IsAttachedInvoice"].ToString();
                }
                else
                {
                    worksheet1.Rows[i + 1].Cells[0].Value = exportDataTable.Rows[i]["Applicant"].ToString();
                    worksheet1.Rows[i + 1].Cells[1].Value = exportDataTable.Rows[i]["Department"].ToString();
                    worksheet1.Rows[i + 1].Cells[2].Value = exportDataTable.Rows[i]["SAPNumber"].ToString();
                    worksheet1.Rows[i + 1].Cells[3].Value = exportDataTable.Rows[i]["Title"].ToString();
                    worksheet1.Rows[i + 1].Cells[4].Value = exportDataTable.Rows[i]["Created"].ToString();
                    worksheet1.Rows[i + 1].Cells[5].Value = exportDataTable.Rows[i]["VendorNo"].ToString();
                    worksheet1.Rows[i + 1].Cells[6].Value = exportDataTable.Rows[i]["VendorName"].ToString();
                    worksheet1.Rows[i + 1].Cells[7].Value = exportDataTable.Rows[i]["PaymentDesc"].ToString();
                    worksheet1.Rows[i + 1].Cells[8].Value = exportDataTable.Rows[i]["TotalAmount"].ToString();
                    worksheet1.Rows[i + 1].Cells[9].Value = exportDataTable.Rows[i]["Currency"].ToString();
                    worksheet1.Rows[i + 1].Cells[10].Value = exportDataTable.Rows[i]["Status"].ToString();
                    worksheet1.Rows[i + 1].Cells[11].Value = exportDataTable.Rows[i]["PaidDate"].ToString();
                }
            }
            CellStyle cs = new CellStyle();
            cs.Font.Weight = ExcelFont.BoldWeight;
            string sSavePath = string.Concat(sFullPath, sSaveFileName);
            objExcelFile.SaveXls(sSavePath);
            SendExcelToClient(sSavePath, sSaveFileName);
        }

        private void SendExcelToClient(string sFileFullName, string sFileName)
        {
            string sApplicationPath = Request.ApplicationPath;
            string sFilePath = string.Empty;
            if (dpQueryType.Value.Equals("Opex", StringComparison.CurrentCultureIgnoreCase) ||
                dpQueryType.Value.Equals("Capex", StringComparison.CurrentCultureIgnoreCase))
            {
                sFilePath = string.Concat(sApplicationPath, "tmpfiles/PaymentRequestReport/", sFileName);
            }
            else 
            {
                sFilePath = string.Concat(sApplicationPath, "tmpfiles/PaymentRequestReport/", sFileName);
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(),
                                                    "alert",
                                                    string.Format("<script type='text/javascript' >var w = window.open('{0}', '_blank');w.location.href='{1}';</script>"
                                                    , sFilePath
                                                    , sFilePath));
        }

        #endregion

    }
}
