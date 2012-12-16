namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Data;
    using System.Globalization;
    using CA.SharePoint.Utilities.Common;
    using SharePoint;
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Collections.Generic;
    public partial class DataEdit : BaseWorkflowUserControl
    {
        private string msg = string.Empty;
        public string MSG { get { return msg; } }

        private string dept;
        public string Department { set { dept = value; } }

        private string applicant;
        public string Applicant { set { applicant = value; } }

        public Employee Applicant1
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

        private string requestId;
        public string RequestId { set { this.requestId = value; } }

        private string mode;
        public string Mode { set { mode = value; } }

        private string jobLevel;
        public string JobLevel { set { jobLevel = value; } }

        private string mobileStd;
        public string MobileStd { get { return mobileStd; } }

        private string otMealStd;
        public string OTMealStd { get { return otMealStd; } }

        private double totalAmount;
        public double TotalAmount { get { return GetTotalAmount(); } set { totalAmount = value; } }

        private double cashAdvance;
        public double CashAdvance { get { return GetCashAdvance(); } set { cashAdvance = value; } }

        private bool isAttachInvoice;
        public bool IsAttachInvoice { get { return this.rbAttInv.Checked; } set { isAttachInvoice = value; } }

        private string summaryExpenseType;

        public string SummaryExpenseType
        {
            get { return this.hidSummaryExpenseType.Value.Trim(); }
        }

        private string hidcd;

        public string Hidcd
        {
            get { return this.hidcashadvance.Value; }
        }
        private string hidcafid;

        public string Hidcafid
        {
            get { return this.hidcashadvancewfid.Value; }
        }

        internal DataTable ItemTable
        {
            get
            {
                return (this.ViewState["ItemTable"] as DataTable) ?? CreateItemTable();
            }
            set
            {
                this.ViewState["ItemTable"] = value;
            }
        }

        internal DataTable CostCenters
        {
            get
            {
                return this.ViewState["CostCenters"] as DataTable;
            }
            set
            {
                this.ViewState["CostCenters"] = value;
            }
        }
        private Employee userAccount;
        public Employee UserAccount
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
        private Employee curEmployee;
        public Employee CurEmployee
        {
            get
            {
                return curEmployee;
            }
            set
            {
                this.curEmployee = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.mode == "Edit")
                {
                    this.cpfUser.CommaSeparatedAccounts = this.Applicant1.UserAccount;
                }
                else
                {
                    this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();
                    Employee employee = UserProfileUtil.GetEmployee(SPContext.Current.Web.CurrentUser.ToString());
                    this.lbRequestedBy.Text = employee.DisplayName + "(" + employee.UserAccount + ")";
                    this.lbDept.Text = employee.Department;
                    this.CurEmployee = employee;
                    this.Applicant1 = employee;
                }
                CostCenters = EmployeeExpenseClaimCommon.GetCostCenterDT();
                LoadLevelStd();
                if (this.mode != null && (this.mode.Equals("Edit", StringComparison.CurrentCultureIgnoreCase)))
                {
                    DataTable itemDetails = EmployeeExpenseClaimCommon.GetDataTable(requestId);
                    if (itemDetails == null)
                    {
                        this.ItemTable.Rows.Clear();
                    }
                    this.rptItem.DataSource = itemDetails == null ? ItemTable : itemDetails;
                    this.rptItem.DataBind();

                    this.lbTotalAmount.Text = this.totalAmount.ToString();

                    DataBindCashAdvance(itemDetails.Rows[0]["TotalAmount"].ToString());

                    this.lbAmountDue.Text = (float.Parse(this.totalAmount.ToString()) - float.Parse(hidTotalAmount.Value == "" ? "0" : hidTotalAmount.Value)).ToString();
                    

                    if (this.isAttachInvoice)
                    {
                        this.rbAttInv.Checked = true;
                        this.rbNonAttInv.Checked = false;
                    }
                    else
                    {
                        this.rbAttInv.Checked = false;
                        this.rbNonAttInv.Checked = true;
                    }
                }
                else
                {
                    this.rptItem.DataSource = this.ItemTable;
                    this.rptItem.DataBind();
                    DataBindCashAdvance("");
                }
            }
           this.cpfUser.Load += new EventHandler(cpfUser_Load);
        }

        protected void LoadLevelStd() 
        {
          
            var mobileStdItem = EmployeeExpenseClaimCommon.GetClaimStdByLevel(Convert.ToInt32(jobLevel), "Mobile phone");
            //if (Convert.ToInt32(jobLevel) >= 9)
            //{
            //    this.jobLevelNumber.Value = "9";
            //}
            if (mobileStdItem == null)
            {
                mobileStd = string.Empty;
            }
            else if (mobileStdItem["Amount"].AsString().IsNullOrWhitespace())
            {
                mobileStd = "no limit";
            }
            else
            {
                mobileStd = mobileStdItem["Amount"].AsString();
            }
            var otMealItem = EmployeeExpenseClaimCommon.GetClaimStdByLevel(Convert.ToInt32(jobLevel), "OT - meal allowance");
            otMealStd = otMealItem != null ? otMealItem["Amount"].AsString() : string.Empty;

            this.hidOTMealStd.Value = otMealStd;
            this.hidMobileStd.Value = mobileStd;
        }

        void cpfUser_Load(object sender, EventArgs e)
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant1 = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            this.lbRequestedBy.Text = this.Applicant1.DisplayName;
            this.lbDept.Text = this.Applicant1.Department;
            this.JobLevel = this.Applicant1.JobLevel == "" ? "9" : this.Applicant1.JobLevel.Substring(this.Applicant1.JobLevel.IndexOf('-') + 1, 1);
            LoadLevelStd();
        }
        protected void btnPeopleInfo_Click(object sender, EventArgs e) 
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant1 = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            this.lbRequestedBy.Text = this.Applicant1.DisplayName;
            this.lbDept.Text = this.Applicant1.Department;
            DataBindCashAdvance("");
            this.JobLevel = this.Applicant1.JobLevel == "" ? "9" : this.Applicant1.JobLevel.Substring(this.Applicant1.JobLevel.IndexOf('-') + 1, 1);
            LoadLevelStd();
        }

        protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            UpdateItem();
            DataRow dr = ItemTable.Rows.Add();

            AddRow(dr);

            this.rptItem.DataSource = this.ItemTable;
            this.rptItem.DataBind();
           // ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "EndRequestHandler()", true);

        }

        protected void rptItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateItem();
                if (ItemTable.Rows.Count > 1)
                {
                    ItemTable.Rows.Remove(ItemTable.Rows[e.Item.ItemIndex]);
                }

                this.rptItem.DataSource = ItemTable;
                this.rptItem.DataBind();
            }
        }

        protected void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                    CADateTimeControl dtDates = (CADateTimeControl)item.FindControl("dtDates");
                    var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                    var txtAmount = (TextBox)item.FindControl("txtAmount");
                    var lbCompanyStandard = (Label)item.FindControl("lbCompanyStandard");
                    var txtRemark = (TextBox)item.FindControl("txtRemark");
                    var txtExpensePurpose = (TextBox)item.FindControl("txtExpensePurpose");
                    var txtDate = (TextBox)item.FindControl("txtDate");

                    DataBindDDL(ddlCostCenter, CostCenters);

                    ddlExpenseType.SelectedValue = row["ExpenseType"].ToString();
                    dtDates.SelectedDate = string.IsNullOrEmpty(row["Dates"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Dates"].ToString());

                    if (row["ExpenseType"].ToString().ToLower().Contains("mobile"))
                    {
                        int begin = row["Dates"].ToString().IndexOf("/");
                        int end = row["Dates"].ToString().LastIndexOf("/");
                        if (end > begin)
                        {
                            //DateTime dt=Convert.ToDateTime(row["Dates"].ToString());
                            //txtDate.Text = dt.Month.ToString() + "/" + dt.Year.ToString();
                            txtDate.Text = "";
                        }
                        else
                        {
                            txtDate.Text = row["Dates"].ToString();
                        }
                    }
                    
                    ddlCostCenter.SelectedValue = row["CostCenter"].ToString();
                    txtAmount.Text = row["Amount"].ToString();
                    lbCompanyStandard.Text = row["CompanyStandard"].AsString();
                    //if (row["CompanyStandard"].AsString()=="9")
                    //{
                    //    lbCompanyStandard.Text = "";
                    //}
                    txtRemark.Text = row["Remark"].ToString();
                    txtExpensePurpose.Text = row["ExpensePurpose"].ToString();
                }
            }
        }

        private void AddRow(DataRow row)
        {
            RepeaterItem item = this.rptItem.Items[this.rptItem.Items.Count - 1];

            var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
            CADateTimeControl dtDates = (CADateTimeControl)item.FindControl("dtDates");
            var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
            var txtAmount = (TextBox)item.FindControl("txtAmount");
            var hidCompanyStandard = (HiddenField)item.FindControl("hidCompanyStandard");

            var hidddlTotalAmount = (HiddenField)item.FindControl("hidddlTotalAmount");

            var txtRemark = (TextBox)item.FindControl("txtRemark");
            var txtExpensePurpose = (TextBox)item.FindControl("txtExpensePurpose");

            var txtDate = (TextBox)item.FindControl("txtDate");


            //DataRow row = this.ItemTable.Rows.Add();
            row["ExpenseType"] = ddlExpenseType.SelectedValue;



            //row["Dates"] = dtDates.IsDateEmpty ? string.Empty : dtDates.SelectedDate.ToShortDateString().Replace("-","/"); ;
            if (row["ExpenseType"].ToString().ToLower().Contains("mobile"))
            {
                if (txtDate.Text.Trim() != "")
                {
                    row["Dates"] = txtDate.Text.Trim().Replace("-", "/");
                }
                //row["Dates"] = dtDates.IsDateEmpty ? string.Empty : dtDates.SelectedDate.Month.ToString() + "/" + dtDates.SelectedDate.Year.ToString();
            }
            else
            {
                row["Dates"] = dtDates.SelectedDate.ToShortDateString().Replace("-", "/");
            }

            row["CostCenter"] = ddlCostCenter.SelectedValue;
            row["Amount"] = txtAmount.Text;
            row["CompanyStandard"] = hidCompanyStandard.Value;

            //if (this.jobLevelNumber.Value=="9")
            //{
            //    row["CompanyStandard"] = "9";
            //}

            row["DdlTotalAmount"] = this.hidcashadvance.Value.Trim();
            row["Remark"] = txtRemark.Text.Trim();

            row["ExpensePurpose"] = txtExpensePurpose.Text.Trim();

        }

        private void UpdateItem()
        {
            this.ItemTable.Rows.Clear();

            foreach (RepeaterItem item in this.rptItem.Items)
            {
                var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                CADateTimeControl dtDates = (CADateTimeControl)item.FindControl("dtDates");
                var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                var txtAmount = (TextBox)item.FindControl("txtAmount");
                var hidCompanyStandard = (HiddenField)item.FindControl("hidCompanyStandard");

                var hidddlTotalAmount = (HiddenField)item.FindControl("hidddlTotalAmount");

                var txtRemark = (TextBox)item.FindControl("txtRemark");
                var txtExpensePurpose=(TextBox)item.FindControl("txtExpensePurpose");

                var txtDate = (TextBox)item.FindControl("txtDate");


                DataRow row = this.ItemTable.Rows.Add();
                row["ExpenseType"] = ddlExpenseType.SelectedValue;



                //row["Dates"] = dtDates.IsDateEmpty ? string.Empty : dtDates.SelectedDate.ToShortDateString().Replace("-","/"); ;
                if (row["ExpenseType"].ToString().ToLower().Contains("mobile"))
                {
                    if (txtDate.Text.Trim() != "")
                    {
                        row["Dates"] = txtDate.Text.Trim().Replace("-", "/");
                    }
                    //row["Dates"] = dtDates.IsDateEmpty ? string.Empty : dtDates.SelectedDate.Month.ToString() + "/" + dtDates.SelectedDate.Year.ToString();
                }
                else 
                {
                    row["Dates"] = dtDates.SelectedDate.ToShortDateString().Replace("-", "/");
                }
                
                row["CostCenter"] = ddlCostCenter.SelectedValue;
                row["Amount"] = txtAmount.Text;
                row["CompanyStandard"] = hidCompanyStandard.Value;

                //if (this.jobLevelNumber.Value=="9")
                //{
                //    row["CompanyStandard"] = "9";
                //}

                row["DdlTotalAmount"] = this.hidcashadvance.Value.Trim();
                row["Remark"] = txtRemark.Text.Trim();

                row["ExpensePurpose"] = txtExpensePurpose.Text.Trim();
            }
        }

        private DataTable CreateItemTable()
        {
            ItemTable = new DataTable();
            ItemTable.Columns.Add("ExpenseType");
            ItemTable.Columns.Add("Dates");
            ItemTable.Columns.Add("CostCenter");
            ItemTable.Columns.Add("Amount");
            ItemTable.Columns.Add("CompanyStandard");

            ItemTable.Columns.Add("DdlTotalAmount");

            ItemTable.Columns.Add("Remark");

            ItemTable.Columns.Add("ExpensePurpose");

            ItemTable.Rows.Add();
            return ItemTable;
        }

        public override bool Validate()
        {
            ValidateForSave();

            if (string.IsNullOrEmpty(this.ffExpenseDescription.Value.AsString()))
            {
                msg += "Please supply Claims Description.\\n";
            }

            //if (string.IsNullOrEmpty(this.ffRemark.Value.AsString()))
            //{
            //    msg += "Please supply reason for remark.\\n";
            //}

            return msg.Length == 0;
        }

        public bool ValidateForSave()
        {
            //Check the number type data
            for (int i = 0; i < rptItem.Items.Count; i++)
            {
                RepeaterItem item = rptItem.Items[i];

                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");

                if (IsNotNumberic(txtAmount.Text))
                {
                    msg += "Please supply valid number.\\n";
                    break;
                }
            }
            return msg.Length == 0;
        }

        private bool IsNotNumberic(string oText)
        {
            if (oText == "NaN")
            {
                return true;
            }
            float fnum = 0;
            if (float.TryParse(oText, NumberStyles.Any, CultureInfo.InvariantCulture, out fnum))
            {
                return false;
            }
            else
                return true;
        }

        public void Update()
        {
            UpdateItem();
        }

        private void DataBindDDL_Load()
        {
            if (this.rptItem.Items.Count > 0)
            {
                DropDownList ddlCostCenter = this.rptItem.Items[0].FindControl("ddlCostCenter") as DropDownList;
                DataBindDDL(ddlCostCenter, CostCenters);
            }
        }

        private void DataBindDDL(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("","0"));
            foreach(DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["Display"].ToString(), dr["Title"].ToString());
                ddl.Items.Add(li);
            }
            //ddl.DataSource = dt;
            //ddl.DataTextField = "Display";
            //ddl.DataValueField = "Title";
            //ddl.DataBind();
        }

        private double GetTotalAmount()
        {
            double total = 0;
            foreach (DataRow dr in ItemTable.Rows)
            {
                total += Convert.ToDouble(dr["Amount"]);
            }
            return total;
        }

        private double GetCashAdvance()
        {
            if (hidTotalAmount.Value!="")
            {
                return double.Parse(hidTotalAmount.Value);
            }
            return 0;
        }

        private void DataBindCashAdvance(string type)
        {
            ddlTotalAmount.Items.Clear();
            var delegationList = SharePointUtil.GetList("CashAdvanceRequest");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><And><And><Eq><FieldRef Name='Applicant' /><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='Status' /><Value Type='Text'>Completed</Value></Eq></And><Neq><FieldRef Name='CashAdvanceStatus' /><Value Type='Text'>1</Value></Neq></And></Where><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>",this.Applicant1.UserAccount);// SPContext.Current.Web.CurrentUser.LoginName
            SPListItemCollection listItems = delegationList.GetItems(query);
            
            double amount = 0;
            string wfid = "";
            string wf = "";
            if (null != listItems && listItems.Count >= 1)
            {
                System.Text.StringBuilder html = new System.Text.StringBuilder();
                html.Append("<ul>");
                foreach (SPListItem spi in listItems)
                {
                    //    ListItem li = new ListItem();
                    //    li.Text = spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString();
                    //    li.Value = spi["Amount"].ToString();
                    //    ddlTotalAmount.Items.Add(li);
                    //    if (type != "" && string.Equals(li.Text, type, StringComparison.CurrentCultureIgnoreCase))
                    //    {
                    //        li.Selected = true;
                    //        hidTotalAmount.Value = spi["Amount"].ToString();
                    //    }
                    //    else 
                    //    {
                    //        hidTotalAmount.Value = ddlTotalAmount.Items[0].Value;
                    //    }


                    if (type != "" && type.Contains(spi["WorkflowNumber"].ToString()))
                    {
                        html.Append("<li><input type=\"checkbox\" checked=\"checked\"  value=\"" + spi["Amount"].ToString() + "\"  title=\"" + spi["WorkflowNumber"].ToString() + "\"/>" + spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString() + "</li>");
                        amount += Double.Parse(spi["Amount"].ToString());
                        wfid += spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString()+";";
                        wf += spi["WorkflowNumber"].ToString() + ";";
                    }
                    else
                    {
                        html.Append("<li><input type=\"checkbox\" value=\"" + spi["Amount"].ToString() + "\"  title=\"" + spi["WorkflowNumber"].ToString() + "\"/>" + spi["WorkflowNumber"].ToString() + "-" + spi["Amount"].ToString() + "</li>");
                    }


                }
                html.Append("</ul>");
                cardiv.InnerHtml = html.ToString();
                if (amount != 0)
                {
                    titlediv.InnerHtml = amount.ToString();
                    this.hidTotalAmount.Value = amount.ToString();
                    this.hidcashadvancewfid.Value = wfid;
                    this.hidcashadvance.Value = wf;
                }
                else 
                {
                    titlediv.InnerHtml = "Select Cash Advance to be deducted";
                    this.hidTotalAmount.Value = "0";
                    this.hidcashadvancewfid.Value = "";
                    this.hidcashadvance.Value = "";
                }
            }
            else
            {
                //ddlTotalAmount.Items.Add(new ListItem("No Cash Advance",""));
                titlediv.InnerHtml = "No Cash Advance";
                this.hidTotalAmount.Value = "0";
                this.hidcashadvancewfid.Value = "";
                this.hidcashadvance.Value = "";
            }
        }

    }
}
