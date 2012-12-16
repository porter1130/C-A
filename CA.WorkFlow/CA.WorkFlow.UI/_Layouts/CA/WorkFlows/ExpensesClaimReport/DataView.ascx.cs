using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using QuickFlow.Core;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Collections.Generic;
using GemBox.Spreadsheet;
using System.Collections;

namespace CA.WorkFlow.UI.ExpensesClaimReport
{
    public partial class DataView : BaseWorkflowUserControl
    {
        #region Update by LiuJun

        #region Fields

        internal DataTable ExpenseType
        {
            get
            {
                return (this.ViewState["ExpenseType"] as DataTable) ?? CreatExpenseType();
            }
            set
            {
                this.ViewState["ExpenseType"] = value;
            }
        }

        internal DataTable ReportNotContainNameColumn
        {
            get
            {
                return this.ViewState["ReportNotContainNameColumn"] as DataTable;
            }
            set
            {
                this.ViewState["ReportNotContainNameColumn"] = value;
            }
        }

        internal DataTable ReportContainNameColumn
        {
            get
            {
                return this.ViewState["ReportContainNameColumn"] as DataTable;
            }
            set
            {
                this.ViewState["ReportContainNameColumn"] = value;
            }
        }

        #endregion

        #region Load Related

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                DataBindFromAndTo();
                CreatExpenseType();
                DataBindExpenseType();
            }
        }

        private void DataBindFromAndTo()
        {
            this.ddlFrom.Items.Clear();
            this.ddlTo.Items.Clear();
            for (int i = 2010; i < 2021; i++)
            {
                this.ddlFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                this.ddlTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            this.ddlFrom.SelectedValue = DateTime.Now.Year.ToString();
            this.ddlTo.SelectedValue = DateTime.Now.Year.ToString();
        }

        private DataTable CreatExpenseType()
        {
            DataTable dt = WorkFlowUtil.GetCollectionByList("Expenses Claim Report Type").GetDataTable();
            ExpenseType = dt;
            return dt;
        }

        private void DataBindExpenseType()
        {
            this.ddlExpenseType.Items.Clear();
            string moduleName = this.ddlModule.SelectedValue;
            DataTable dt = ExpenseType;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ModuleName"].ToString().Equals(moduleName, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.ddlExpenseType.Items.Add(new ListItem(dr["ExpenseType"].ToString(), dr["ExpenseType"].ToString()));
                }
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindExpenseType();
        }

        #endregion

        #region Report Event

        public void btnQuery_Click(object sender, EventArgs e)
        {
            string module = this.ddlModule.SelectedValue;
            switch (module)
            {
                case "Travel Expense Claim":
                    DataBindTravelExpenseClaimReport();
                    break;
                case "Employee Expense Claim":
                    DataBindEmployeeExpenseClaimReport();
                    break;
            }
        }

        private void BindNULLToRPRepeater() 
        {
            rpDepartmentItem.DataSource = null;
            rpDepartmentItem.DataBind();

            rpDepartmentStaffItem.DataSource = null;
            rpDepartmentStaffItem.DataBind();
        }

        private DataTable GetReportNotContainNameColumn(DataTable dt) 
        {
            DataTable ReportNotContainNameColumn = dt.Clone();
            int noOfSpecialApprovalGranted = 0;
            double totalAmt = 0;
            foreach (DataRow row in dt.Rows)
            {
                noOfSpecialApprovalGranted +=Int32.Parse(row["TotalCount"].ToString());
                totalAmt += double.Parse(row["TotalAmount"].ToString());
                ReportNotContainNameColumn.ImportRow(row);
            }
            totalAmt = Math.Round(totalAmt,2);
            DataRow dr = ReportNotContainNameColumn.NewRow();
            dr["Department"] = "Total";
            dr["TotalCount"] = noOfSpecialApprovalGranted.ToString();
            dr["TotalAmount"] = totalAmt.ToString();
            ReportNotContainNameColumn.Rows.Add(dr);
            return ReportNotContainNameColumn;
        }

        private DataTable GetReportContainNameColumn(DataTable dt)
        {
            DataTable ReportContainNameColumn = dt.Clone();
            int noOfSpecialApprovalGranted = 0;
            double totalAmt = 0;
            foreach (DataRow row in dt.Rows)
            {
                noOfSpecialApprovalGranted += Int32.Parse(row["TotalCount"].ToString());
                totalAmt += double.Parse(row["TotalAmount"].ToString());
                ReportContainNameColumn.ImportRow(row);
            }
            totalAmt = Math.Round(totalAmt, 2);
            noOfSpecialApprovalGranted = noOfSpecialApprovalGranted / 2;
            totalAmt = Math.Round(totalAmt / 2, 2);
            DataRow dr = ReportContainNameColumn.NewRow();
            dr["Department"] = "Total";
            dr["Name"] = "";
            dr["TotalCount"] = noOfSpecialApprovalGranted.ToString();
            dr["TotalAmount"] = totalAmt.ToString();
            ReportContainNameColumn.Rows.Add(dr);
            return ReportContainNameColumn;
        }

        #endregion

        #region Travel Expense Claim Report

        private void DataBindTravelExpenseClaimReport()
        {
            DataTable tecItemDataTable= GetTravelExpenseClaimItemsDataTable().GetDataTable();
            if (null == tecItemDataTable) 
            {
                BindNULLToRPRepeater();
                return;
            }

            tecItemDataTable = tecItemDataTable.AsEnumerable().Where(dr => dr["CompanyStandards"].ToString() != "" && dr["CompanyStandards"].ToString() != "N/A").CopyToDataTable();

            DataTable travelExpenseClaimItemsDataTable = GetTravelExpenseClaimItemsDataTableByDT(tecItemDataTable);

            DataTable tecDataTable = GetTravelExpenseClaimDataTable().GetDataTable();
            if (null == tecDataTable)
            {
                BindNULLToRPRepeater();
                return;
            }
            DataTable travelExpenseClaimDataTable = GetTravelExpenseClaimDataTableByDT(tecDataTable);
           
            DataTable joinDataTableNotContainNameColumn = GetTECJoinDataTableNotContainNameColumn(travelExpenseClaimItemsDataTable, travelExpenseClaimDataTable);
            DataTable joinDataTableContainNameColumn = GetTECJoinDataTableContainNameColumn(travelExpenseClaimItemsDataTable, travelExpenseClaimDataTable);
            DataTable departmentStaffDataTable = GetTECDataTableContainNameColumn(joinDataTableContainNameColumn);

            ReportContainNameColumn = departmentStaffDataTable;
            ReportNotContainNameColumn = joinDataTableNotContainNameColumn;

            rpDepartmentItem.DataSource = joinDataTableNotContainNameColumn;
            rpDepartmentItem.DataBind();

            rpDepartmentStaffItem.DataSource = departmentStaffDataTable;
            rpDepartmentStaffItem.DataBind();

            hfExportStatus.Value = "1";
        }

        private SPListItemCollection GetTravelExpenseClaimItemsDataTable()
        {
            string expenseType = GetExpenseType(this.ddlExpenseType.SelectedValue);
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Travel Expense Claim Details");
            SPQuery query = new SPQuery();

            bool yes=true;
            query.Query = string.Format(
                                 "<Where>" +
                                    "<And>" +
                                          "<Eq>" +
                                               "<FieldRef Name='ExpenseType' />" +
                                               "<Value Type='Text'>{0}</Value>" +
                                           "</Eq>" +
                                           "<Eq>" +
                                               "<FieldRef Name='SpecialApproved' />" +
                                               "<Value Type='Text'>{1}</Value>" +
                                           "</Eq>" +
                                     "</And>" +
                                  "</Where>"
                                  , expenseType,
                                  yes);

            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='ApprovedRmbAmt' />" +
                               "<FieldRef Name='CompanyStandards' />" +
                               "<FieldRef Name='TravelDateFrom' />" +
                               "<FieldRef Name='TravelDateTo' />" +
                               "<FieldRef Name='OriginalAmt' />" +
                               "<FieldRef Name='Date' />";

            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems;
        }

        private DataTable GetTravelExpenseClaimItemsDataTableByDT(DataTable dt)
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("ItemsWorkFlowNumber");
            dtReturn.Columns.Add("TotalAmount");

            int fromYear = Int32.Parse(this.ddlFrom.SelectedValue);
            int toYear = Int32.Parse(this.ddlTo.SelectedValue);
            string expenseType = GetExpenseType(this.ddlExpenseType.SelectedValue);
            if (expenseType.Equals("Hotel", StringComparison.CurrentCultureIgnoreCase))
            {
                var data = from dr in dt.AsEnumerable()
                           where double.Parse(dr.Field<string>("ApprovedRmbAmt")) > double.Parse(dr.Field<string>("CompanyStandards"))
                                 //&& dr.Field<string>("CompanyStandards") != ""
                                 //&& dr.Field<string>("CompanyStandards") != "N/A"
                                 && Int32.Parse(dr.Field<string>("TravelDateFrom").ToString().Substring(dr.Field<string>("TravelDateFrom").ToString().LastIndexOf('/') + 1, 4)) >= fromYear
                                 && Int32.Parse(dr.Field<string>("TravelDateTo").ToString().Substring(dr.Field<string>("TravelDateTo").ToString().LastIndexOf('/') + 1, 4)) <= toYear
                           group dr by dr.Field<string>("Title") into g
                           select new
                           {
                               ItemsWorkFlowNumber = g.Key,
                               //Total Amt > Company Std (in Rmb) 计算为：金额总和 - 公司标准总和
                               TotalAmount = Math.Round((g.Sum(p => double.Parse(p.Field<string>("ApprovedRmbAmt"))) - g.Sum(p => double.Parse(p.Field<string>("CompanyStandards")))), 2)
                               //TotalAmount = g.Sum(p => p.Field<double>("Amount"))
                           };
                foreach (var item in data)
                {
                    DataRow dr = dtReturn.NewRow();
                    dr["ItemsWorkFlowNumber"] = item.ItemsWorkFlowNumber;
                    dr["TotalAmount"] = item.TotalAmount;
                    dtReturn.Rows.Add(dr);
                }
            }
            else
            {
                var data = from dr in dt.AsEnumerable()
                           where double.Parse(dr.Field<string>("ApprovedRmbAmt")) > double.Parse(dr.Field<string>("CompanyStandards"))
                                 //&& dr.Field<string>("CompanyStandards") != ""
                                 //&& dr.Field<string>("CompanyStandards") != "N/A"
                                 && Int32.Parse(dr.Field<string>("Date").ToString().Substring(dr.Field<string>("Date").ToString().LastIndexOf('/') + 1, 4)) >= fromYear
                                 && Int32.Parse(dr.Field<string>("Date").ToString().Substring(dr.Field<string>("Date").ToString().LastIndexOf('/') + 1, 4)) <= toYear
                           group dr by dr.Field<string>("Title") into g
                           select new
                           {
                               ItemsWorkFlowNumber = g.Key,
                               //Total Amt > Company Std (in Rmb) 计算为：金额总和 - 公司标准总和
                               TotalAmount = Math.Round((g.Sum(p => double.Parse(p.Field<string>("ApprovedRmbAmt"))) - g.Sum(p => double.Parse(p.Field<string>("CompanyStandards")))), 2)
                               //TotalAmount = g.Sum(p => p.Field<double>("Amount"))
                           };
                foreach (var item in data)
                {
                    DataRow dr = dtReturn.NewRow();
                    dr["ItemsWorkFlowNumber"] = item.ItemsWorkFlowNumber;
                    dr["TotalAmount"] = item.TotalAmount;
                    dtReturn.Rows.Add(dr);
                }
            }

            return dtReturn;
        }

        private SPListItemCollection GetTravelExpenseClaimDataTable()
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Travel Expense Claim");
            SPQuery query = new SPQuery();
            query.Query = "<Where>" +
                               "<Eq>" +
                                    "<FieldRef Name='Status' />" +
                                    "<Value Type='Text'>Completed</Value>" +
                               "</Eq>" +
                          "</Where>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='Applicant' />" +
                               "<FieldRef Name='Department' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems;
        }

        private DataTable GetTravelExpenseClaimDataTableByDT(DataTable dt)
        {
            var data = from dr in dt.AsEnumerable()
                       select new
                       {
                           Title = dr["Title"].ToString(),
                           RequestedBy = dr["Applicant"].ToString(),
                           Department = dr["Department"].ToString()
                       };
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("WorkFlowNumber");
            dtReturn.Columns.Add("RequestedBy");
            dtReturn.Columns.Add("Department");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["WorkFlowNumber"] = item.Title;
                dr["RequestedBy"] = item.RequestedBy;
                dr["Department"] = item.Department;
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        private DataTable GetTECJoinDataTableNotContainNameColumn(DataTable itemDataTable, DataTable dataTable)
        {
            var data = from idt in itemDataTable.AsEnumerable()
                       join dt in dataTable.AsEnumerable() on idt.Field<string>("ItemsWorkFlowNumber") equals dt.Field<string>("WorkFlowNumber")
                       select new
                       {
                           idt,
                           dt
                       };

            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("Department");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Department"] = item.dt.Field<string>("Department").ToString();
                dr["TotalAmount"] = item.idt.Field<string>("TotalAmount").ToString();
                dtReturn.Rows.Add(dr);
            }

            var table = from r in dtReturn.AsEnumerable()
                        group r by r.Field<string>("Department") into g
                        select new
                        {
                            Department = g.Key,
                            TotalCount = g.Count(),
                            TotalAmount =Math.Round(g.Sum(n => double.Parse(n.Field<string>("TotalAmount").ToString())),2)
                        };

            dtReturn = new DataTable();
            dtReturn.Rows.Clear();
            dtReturn.Columns.Clear();
            dtReturn.Columns.Add("Department");
            dtReturn.Columns.Add("TotalCount");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in table)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Department"] = item.Department;
                dr["TotalCount"] = item.TotalCount;
                dr["TotalAmount"] = item.TotalAmount;
                dtReturn.Rows.Add(dr);
            }
            dtReturn = dtReturn.AsEnumerable().OrderBy(dr => dr["Department"].ToString()).CopyToDataTable();
            return dtReturn;
        }

        private DataTable GetTECJoinDataTableContainNameColumn(DataTable itemDataTable, DataTable dataTable)
        {
            var data = from idt in itemDataTable.AsEnumerable()
                       join dt in dataTable.AsEnumerable() on idt.Field<string>("ItemsWorkFlowNumber") equals dt.Field<string>("WorkFlowNumber")
                       select new
                       {
                           idt,
                           dt
                       };

            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("Department");
            dtReturn.Columns.Add("Name");
            dtReturn.Columns.Add("TotalCount");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Department"] = item.dt.Field<string>("Department").ToString();
                dr["TotalAmount"] = item.idt.Field<string>("TotalAmount").ToString();
                string name = item.dt.Field<string>("RequestedBy").ToString();
                name = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
                dr["Name"] = name;
                dr["TotalCount"] = "1";
                dtReturn.Rows.Add(dr);
            }
            dtReturn = dtReturn.AsEnumerable().OrderBy(dr => dr["Department"].ToString()).ThenBy(dr => dr["Name"].ToString()).CopyToDataTable();
            return dtReturn;
        }

        private DataTable GetTECDataTableContainNameColumn(DataTable dt)
        {
            List<string> listDepartment = new List<string>();
            DataTable data = new DataTable();
            data.Columns.Add("Department");
            data.Columns.Add("Name");
            data.Columns.Add("TotalCount");
            data.Columns.Add("TotalAmount");
            foreach (DataRow row in dt.Rows)
            {
                if (!listDepartment.Contains(row["Department"].ToString()))
                {
                    DataRow dr = data.NewRow();
                    GetTECDataRow(out listDepartment, row["Department"].ToString(), dt, dr);
                    data.Rows.Add(dr);
                }
                DataRow drNew = data.NewRow();
                drNew["Department"] = "";
                drNew["Name"] = row["Name"].ToString();
                drNew["TotalCount"] = row["TotalCount"].ToString();
                drNew["TotalAmount"] = row["TotalAmount"].ToString();
                data.Rows.Add(drNew);
            }
            return data;
        }

        private void GetTECDataRow(out List<string> listDepartment, string department, DataTable dt, DataRow dr)
        {
            listDepartment = new List<string>();
            int rowCount = 0;
            double rowTotalAmount = 0;
            GetTECDataRowInfoContainNameColumn(department, dt, out rowCount, out rowTotalAmount);
            dr["Department"] = department;
            dr["Name"] = "";
            dr["TotalCount"] = rowCount.ToString();
            dr["TotalAmount"] = rowTotalAmount.ToString();
            listDepartment.Add(department);
        }

        private void GetTECDataRowInfoContainNameColumn(string department, DataTable dt, out int rowCount, out double rowTotalAmount)
        {
            rowCount = 0;
            rowTotalAmount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (department.Equals(dr["Department"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    ++rowCount;
                    rowTotalAmount += double.Parse(dr["TotalAmount"].ToString());
                }
            }
            rowTotalAmount = Math.Round(rowTotalAmount,2);
        }


        #endregion

        #region Employee Expense Claim Report

        private void DataBindEmployeeExpenseClaimReport()
        {
            DataTable eecItemDataTable = GetEmployeeExpenseClaimItemsDataTable().GetDataTable();
            if (null == eecItemDataTable)
            {
                BindNULLToRPRepeater();
                return;
            }
            eecItemDataTable = eecItemDataTable.AsEnumerable().Where(dr => dr["CompanyStandard"].ToString() != "" && dr["CompanyStandard"].ToString() != "no limit").CopyToDataTable();
            
            DataTable expenseClaimItemsDataTable = GetEmployeeExpenseClaimItemsDataTableByDT(eecItemDataTable);

            DataTable eecDataTable = GetEmployeeExpenseClaimDataTable().GetDataTable();
            if (null == eecDataTable)
            {
                BindNULLToRPRepeater();
                return;
            }
            DataTable employeeExpenseClaimDataTable = GetEmployeeExpenseClaimDataTableByDT(eecDataTable);

            DataTable joinDataTableNotContainNameColumn = GetEECJoinDataTableNotContainNameColumn(expenseClaimItemsDataTable, employeeExpenseClaimDataTable);
            DataTable joinDataTableContainNameColumn = GetEECJoinDataTableContainNameColumn(expenseClaimItemsDataTable, employeeExpenseClaimDataTable);
            DataTable departmentStaffDataTable = GetEECDataTableContainNameColumn(joinDataTableContainNameColumn);

            ReportContainNameColumn = departmentStaffDataTable;
            ReportNotContainNameColumn = joinDataTableNotContainNameColumn;

            rpDepartmentItem.DataSource = joinDataTableNotContainNameColumn;
            rpDepartmentItem.DataBind();

            rpDepartmentStaffItem.DataSource = departmentStaffDataTable;
            rpDepartmentStaffItem.DataBind();

            hfExportStatus.Value = "1";
        }

        private SPListItemCollection GetEmployeeExpenseClaimItemsDataTable()
        {
            string expenseType = GetExpenseType(this.ddlExpenseType.SelectedValue);
            var delegationList = CA.SharePoint.SharePointUtil.GetList("EmployeeExpenseClaimItems");
            SPQuery query = new SPQuery();

            query.Query = string.Format(
                                 "<Where>" +
                                      "<Eq>" +
                                           "<FieldRef Name='ExpenseType' />" +
                                           "<Value Type='Text'>{0}</Value>" +
                                       "</Eq>" +
                                  "</Where>"
                                  , expenseType);

            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='OriginalAmount' />" +
                               "<FieldRef Name='Amount' />" +
                               "<FieldRef Name='CompanyStandard' />" +
                               "<FieldRef Name='Dates' />";

            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems;
        }

        private SPListItemCollection GetEmployeeExpenseClaimDataTable()
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Employee Expense Claim Workflow");
            SPQuery query = new SPQuery();
            query.Query = "<Where>" +
                               "<Eq>" +
                                    "<FieldRef Name='Status' />" +
                                    "<Value Type='Text'>Completed</Value>" +
                               "</Eq>" +
                          "</Where>";
            query.ViewFields = "<FieldRef Name='Title' />" +
                               "<FieldRef Name='RequestedBy' />" +
                               "<FieldRef Name='Department' />";
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems;
        }

        private string GetExpenseType(string expenseType)
        {
            string listExpenseType = string.Empty;
            switch (expenseType)
            {
                case "Hotel":
                    listExpenseType = "Hotel";
                    break;
                case "Travel Meal Allowance":
                    listExpenseType = "Meal Allowance";
                    break;
                case "Overtime Meal Allowance":
                    listExpenseType = "OT - meal allowance";
                    break;
                case "Mobile Phone":
                    listExpenseType = "Mobile – local call and others";
                    break;
            }
            return listExpenseType;
        }

        private DataTable GetEmployeeExpenseClaimItemsDataTableByDT(DataTable dt)
        {
            int fromYear = Int32.Parse(this.ddlFrom.SelectedValue);
            int toYear = Int32.Parse(this.ddlTo.SelectedValue);
            var data = from dr in dt.AsEnumerable()
                       where dr.Field<double>("Amount") > double.Parse(dr.Field<string>("CompanyStandard"))
                             //&& dr.Field<string>("CompanyStandard") != ""
                             //&& dr.Field<string>("CompanyStandard") != "no limit"
                             && Int32.Parse(dr.Field<string>("Dates").ToString().Substring(dr.Field<string>("Dates").ToString().LastIndexOf('/') + 1, 4)) >= fromYear
                             && Int32.Parse(dr.Field<string>("Dates").ToString().Substring(dr.Field<string>("Dates").ToString().LastIndexOf('/') + 1, 4)) <= toYear
                       group dr by dr.Field<string>("Title") into g
                       select new
                       {
                           ItemsWorkFlowNumber = g.Key,
                           //Total Amt > Company Std (in Rmb) 计算为：金额总和 - 公司标准总和
                           TotalAmount = Math.Round((g.Sum(p => p.Field<double>("Amount")) - g.Sum(p => double.Parse(p.Field<string>("CompanyStandard")))), 2)
                           //TotalAmount = g.Sum(p => p.Field<double>("Amount"))
                       };

            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("ItemsWorkFlowNumber");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["ItemsWorkFlowNumber"] = item.ItemsWorkFlowNumber;
                dr["TotalAmount"] = item.TotalAmount;
                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        private DataTable GetEmployeeExpenseClaimDataTableByDT(DataTable dt)
        {
            var data = from dr in dt.AsEnumerable()
                       select new
                       {
                           Title = dr["Title"].ToString(),
                           RequestedBy = dr["RequestedBy"].ToString(),
                           Department = dr["Department"].ToString()
                       };
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("WorkFlowNumber");
            dtReturn.Columns.Add("RequestedBy");
            dtReturn.Columns.Add("Department");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["WorkFlowNumber"] = item.Title;
                dr["RequestedBy"] = item.RequestedBy;
                dr["Department"] = item.Department;
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        private DataTable GetEECJoinDataTableNotContainNameColumn(DataTable itemDataTable, DataTable dataTable)
        {
            var data = from idt in itemDataTable.AsEnumerable()
                       join dt in dataTable.AsEnumerable() on idt.Field<string>("ItemsWorkFlowNumber") equals dt.Field<string>("WorkFlowNumber")
                       select new
                       {
                           idt,
                           dt
                       };

            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("Department");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Department"] = item.dt.Field<string>("Department").ToString();
                dr["TotalAmount"] = item.idt.Field<string>("TotalAmount").ToString();
                dtReturn.Rows.Add(dr);
            }

            var table = from r in dtReturn.AsEnumerable()
                        group r by r.Field<string>("Department") into g
                        select new
                        {
                            Department = g.Key,
                            TotalCount = g.Count(),
                            TotalAmount = Math.Round(g.Sum(n => double.Parse(n.Field<string>("TotalAmount").ToString())), 2)
                        };

            dtReturn = new DataTable();
            dtReturn.Rows.Clear();
            dtReturn.Columns.Clear();
            dtReturn.Columns.Add("Department");
            dtReturn.Columns.Add("TotalCount");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in table)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Department"] = item.Department;
                dr["TotalCount"] = item.TotalCount;
                dr["TotalAmount"] = item.TotalAmount;
                dtReturn.Rows.Add(dr);
            }
            dtReturn = dtReturn.AsEnumerable().OrderBy(dr => dr["Department"].ToString()).CopyToDataTable();
            return dtReturn;
        }

        private DataTable GetEECJoinDataTableContainNameColumn(DataTable itemDataTable, DataTable dataTable)
        {
            var data = from idt in itemDataTable.AsEnumerable()
                       join dt in dataTable.AsEnumerable() on idt.Field<string>("ItemsWorkFlowNumber") equals dt.Field<string>("WorkFlowNumber")
                       select new
                       {
                           idt,
                           dt
                       };

            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("Department");
            dtReturn.Columns.Add("Name");
            dtReturn.Columns.Add("TotalCount");
            dtReturn.Columns.Add("TotalAmount");
            foreach (var item in data)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Department"] = item.dt.Field<string>("Department").ToString();
                dr["TotalAmount"] = item.idt.Field<string>("TotalAmount").ToString();
                string name = item.dt.Field<string>("RequestedBy").ToString();
                name = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
                dr["Name"] = name;
                dr["TotalCount"] = "1";
                dtReturn.Rows.Add(dr);
            }
            dtReturn = dtReturn.AsEnumerable().OrderBy(dr => dr["Department"].ToString()).ThenBy(dr => dr["Name"].ToString()).CopyToDataTable();
            return dtReturn;
        }

        private DataTable GetEECDataTableContainNameColumn(DataTable dt)
        {
            List<string> listDepartment = new List<string>();
            DataTable data = new DataTable();
            data.Columns.Add("Department");
            data.Columns.Add("Name");
            data.Columns.Add("TotalCount");
            data.Columns.Add("TotalAmount");
            foreach (DataRow row in dt.Rows)
            {
                if (!listDepartment.Contains(row["Department"].ToString()))
                {
                    DataRow dr = data.NewRow();
                    GetEECDataRow(out listDepartment, row["Department"].ToString(), dt, dr);
                    data.Rows.Add(dr);
                }
                DataRow drNew = data.NewRow();
                drNew["Department"] = "";
                drNew["Name"] = row["Name"].ToString();
                drNew["TotalCount"] = row["TotalCount"].ToString();
                drNew["TotalAmount"] = row["TotalAmount"].ToString();
                data.Rows.Add(drNew);
            }
            return data;
        }

        private void GetEECDataRow(out List<string> listDepartment, string department, DataTable dt, DataRow dr)
        {
            listDepartment = new List<string>();
            int rowCount = 0;
            double rowTotalAmount = 0;
            GetEECDataRowInfoContainNameColumn(department, dt, out rowCount, out rowTotalAmount);
            dr["Department"] = department;
            dr["Name"] = "";
            dr["TotalCount"] = rowCount.ToString();
            dr["TotalAmount"] = rowTotalAmount.ToString();
            listDepartment.Add(department);
        }

        private void GetEECDataRowInfoContainNameColumn(string department, DataTable dt, out int rowCount, out double rowTotalAmount)
        {
            rowCount = 0;
            rowTotalAmount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (department.Equals(dr["Department"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    ++rowCount;
                    rowTotalAmount += double.Parse(dr["TotalAmount"].ToString());
                }
            }
            rowTotalAmount = Math.Round(rowTotalAmount,2);
        }

        #endregion

        #region //Abandoned
        //private DataTable JoinDataTableContainNameColumn(DataTable itemDataTable, DataTable dataTable)
        //{
        //    var data = from idt in itemDataTable.AsEnumerable()
        //               join dt in dataTable.AsEnumerable() on idt.Field<string>("ItemsWorkFlowNumber") equals dt.Field<string>("WorkFlowNumber")
        //               select new
        //               {
        //                   idt,
        //                   dt
        //               };

        //    DataTable dtReturn = new DataTable();
        //    dtReturn.Columns.Add("Department");
        //    dtReturn.Columns.Add("TotalAmount");
        //    dtReturn.Columns.Add("Name");
        //    foreach (var item in data)
        //    {
        //        DataRow dr = dtReturn.NewRow();
        //        dr["Department"] = item.dt.Field<string>("Department").ToString();
        //        dr["TotalAmount"] = item.idt.Field<string>("TotalAmount").ToString();
        //        string name = item.dt.Field<string>("RequestedBy").ToString();
        //        name = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
        //        dr["Name"] = name;
        //        dtReturn.Rows.Add(dr);
        //    }

        //    var table = from r in dtReturn.AsEnumerable()
        //                group r by new 
        //                            {
        //                                Department=r.Field<string>("Department"),
        //                                Name=r.Field<string>("Name")
        //                            }  
        //                into g
        //                select new
        //                {
        //                    Department = g.Key.Department,
        //                    TotalCount = g.Count(),
        //                    TotalAmount = g.Sum(n => double.Parse(n.Field<string>("TotalAmount").ToString())),
        //                    Name = g.Key.Name
        //                };

        //    dtReturn = new DataTable();
        //    dtReturn.Rows.Clear();
        //    dtReturn.Columns.Clear();
        //    dtReturn.Columns.Add("Department");
        //    dtReturn.Columns.Add("TotalCount");
        //    dtReturn.Columns.Add("TotalAmount");
        //    dtReturn.Columns.Add("Name");
        //    foreach (var item in table)
        //    {
        //        DataRow dr = dtReturn.NewRow();
        //        dr["Department"] = item.Department;
        //        dr["TotalCount"] = item.TotalCount;
        //        dr["TotalAmount"] = item.TotalAmount;
        //        dr["Name"] = item.Name;
        //        dtReturn.Rows.Add(dr);
        //    }
        //    return dtReturn;
        //}
        #endregion

        #endregion

        #region 导出报表到Excel

        public void ExcelPort()
        {
            DataTable ReportNotContainNameColumnDataTable = GetReportNotContainNameColumn(ReportNotContainNameColumn);
            DataTable ReportContainNameColumnDataTable = GetReportContainNameColumn(ReportContainNameColumn);
            ExportExcel(ReportNotContainNameColumn, ReportContainNameColumn, 
                        ReportNotContainNameColumnDataTable, ReportContainNameColumnDataTable);
        }

        private void ExportExcel(DataTable ReportNotContainNameColumn, DataTable ReportContainNameColumn,
                                 DataTable ReportNotContainNameColumnDataTable, DataTable ReportContainNameColumnDataTable)
        {
            string strSampleFileName = "ExpensesClaimReportSample.xls";

            string sSaveFileName = this.ddlModule.SelectedValue+" Report" + ".xls";

            string sFullPath = Server.MapPath("/tmpfiles/ExpensesClaimReport/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            worksheet1.Rows[2].Cells[0].Value = ddlFrom.SelectedItem.Text;
            worksheet1.Rows[2].Cells[1].Value = ddlTo.SelectedItem.Text;
            worksheet1.Rows[2].Cells[2].Value = ddlModule.SelectedItem.Text;
            worksheet1.Rows[2].Cells[3].Value = ddlExpenseType.SelectedItem.Text;

            worksheet1.Rows[5].Cells[0].Value = "Expense Type - " + ddlExpenseType.SelectedItem.Text;

            int iNotCon = ReportNotContainNameColumn.Rows.Count;
            if (iNotCon > 1)
            {
                worksheet1.Rows[7].InsertCopy(iNotCon-1 , worksheet1.Rows[7]);
            }

            for (int i = 0; i < iNotCon; i++)
            {
                worksheet1.Rows[7 + i].Cells[0].Value = ReportNotContainNameColumn.Rows[i][0];
                worksheet1.Rows[7 + i].Cells[2].Value = ReportNotContainNameColumn.Rows[i][1];
                worksheet1.Rows[7 + i].Cells[3].Value = ReportNotContainNameColumn.Rows[i][2];
            }

            worksheet1.Rows[iNotCon + 7].Cells[0].Value
                                         = ReportNotContainNameColumnDataTable
                                           .Rows[ReportNotContainNameColumnDataTable.Rows.Count - 1]["Department"].ToString();
            worksheet1.Rows[iNotCon + 7].Cells[2].Value
                                        = ReportNotContainNameColumnDataTable
                                          .Rows[ReportNotContainNameColumnDataTable.Rows.Count - 1]["TotalCount"].ToString();
            worksheet1.Rows[iNotCon + 7].Cells[3].Value
                                        = ReportNotContainNameColumnDataTable
                                          .Rows[ReportNotContainNameColumnDataTable.Rows.Count - 1]["TotalAmount"].ToString();
            
            int iDep = ReportContainNameColumn.Rows.Count;
            int iCurrentIndedx = iNotCon + 7 + 7 - 1;
            if (iDep > 1)
            {
                worksheet1.Rows[iCurrentIndedx].InsertCopy(iDep - 1, worksheet1.Rows[iCurrentIndedx]);
            }

            CellStyle cs = new CellStyle();
            cs.Font.Weight = ExcelFont.BoldWeight;

            for (int i = 0; i < iDep; i++)
            {
                worksheet1.Rows[iCurrentIndedx + i].Cells[0].Value = ReportContainNameColumn.Rows[i][0];
                worksheet1.Rows[iCurrentIndedx + i].Cells[1].Value = ReportContainNameColumn.Rows[i][1];
                worksheet1.Rows[iCurrentIndedx + i].Cells[2].Value = ReportContainNameColumn.Rows[i][2];
                worksheet1.Rows[iCurrentIndedx + i].Cells[3].Value = ReportContainNameColumn.Rows[i][3];

                if (ReportContainNameColumn.Rows[i][0].ToString() != ""
                    && ReportContainNameColumn.Rows[i][1].ToString() == "")
                {
                    worksheet1.Rows[iCurrentIndedx + i].Style = cs;
                }
            }

            worksheet1.Rows[iCurrentIndedx - 2].Cells[0].Value = "Expense Type - " + ddlExpenseType.SelectedItem.Text;

            worksheet1.Rows[iCurrentIndedx + iDep].Cells[0].Value
                                         = ReportContainNameColumnDataTable
                                           .Rows[ReportContainNameColumnDataTable.Rows.Count - 1]["Department"].ToString();
            worksheet1.Rows[iCurrentIndedx + iDep].Cells[2].Value
                                        = ReportContainNameColumnDataTable
                                          .Rows[ReportContainNameColumnDataTable.Rows.Count - 1]["TotalCount"].ToString();
            worksheet1.Rows[iCurrentIndedx + iDep].Cells[3].Value
                                        = ReportContainNameColumnDataTable
                                          .Rows[ReportContainNameColumnDataTable.Rows.Count - 1]["TotalAmount"].ToString();

            string sSavePath = string.Concat(sFullPath, sSaveFileName);
            objExcelFile.SaveXls(sSavePath);
            SendExcelToClient(sSavePath, sSaveFileName);
        }
        
        //向户端发送生成的Excel
        private void SendExcelToClient(string sFileFullName, string sFileName)
        {
            string sApplicationPath = Request.ApplicationPath;
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/ExpensesClaimReport/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }

        #region Test
        //public void ForTest()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("a");
        //    dt.Columns.Add("b");
        //    dt.Columns.Add("c");
        //    for (int i = 0; i < 10; i++)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr[0] = i.ToString();
        //        dr[1] = i.ToString();
        //        dr[2] = i.ToString();
        //        dt.Rows.Add(dr);
        //    }

        //    DataTable dt2 = new DataTable();

        //    dt2.Columns.Add("e");
        //    dt2.Columns.Add("f");
        //    dt2.Columns.Add("g");
        //    dt2.Columns.Add("h");

        //    for (int i = 0; i < 5; i++)
        //    {
        //        DataRow dr = dt2.NewRow();
        //        dr[0] = i.ToString();
        //        dr[1] = i.ToString();
        //        dr[2] = i.ToString();
        //        dr[3] = i.ToString();
        //        dt2.Rows.Add(dr);
        //    }
        //    ExportExcel(dt, dt2);
        //}
        #endregion

        #endregion
    }
}