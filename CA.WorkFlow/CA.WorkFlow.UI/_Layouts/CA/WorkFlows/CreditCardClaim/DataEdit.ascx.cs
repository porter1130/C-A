using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using Microsoft.SharePoint.Workflow;
namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class DataEdit : QFUserControl
    {
        #region Field

        public Employee ApplicantEmployee
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

        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        private string applicant;

        public string Applicant
        {
            get { return applicant; }
            set { applicant = value; }
        }

        private string _department;

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        private string month;

        public string Month
        {
            get { return this.ddlMonth.SelectedValue; }
            set { month = value; }
        }
        private string mode;

        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        private string travelRequestStatus;

        public string TravelRequestStatus
        {
            get { return this.hidTravelRequest.Value.Trim(); }
            set { travelRequestStatus = value; }
        }
        private string rMBSummaryExpenseType;

        public string RMBSummaryExpenseType
        {
            get { return this.hidRMBSummaryExpenseType.Value.Trim(); }
            set { this.hidRMBSummaryExpenseType.Value = value; }
        }
        private string uSDSummaryExpenseType;

        public string USDSummaryExpenseType
        {
            get { return this.hidUSDSummaryExpenseType.Value.Trim(); }
            set { this.hidUSDSummaryExpenseType.Value = value; }
        }

        internal DataTable BillInfo
        {
            get
            {
                return this.ViewState["BillInfo"] as DataTable;
            }
            set
            {
                this.ViewState["BillInfo"] = value;
            }
        }

        #endregion

        #region Method

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.ControlMode == SPControlMode.New)
                {
                    this.ApplicantEmployee = this.CurrentEmployee;
                    this.Applicant = this.ApplicantEmployee.DisplayName + "(" + this.ApplicantEmployee.UserAccount + ")";
                    this.Department = this.ApplicantEmployee.Department;
                    this.lblRequestedBy.Text = this.Applicant;
                    this.lblDepartment.Text = this.ApplicantEmployee.Department;
                    BindDDLEmployeeList(this.ApplicantEmployee.UserAccount, true);
                    LoadSourceData(this.ApplicantEmployee.EmployeeID, WorkflowConfigName.CreditCardEmployeeMapping,"");
                }

                if (this.ControlMode == SPControlMode.Edit)
                {
                    WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                    object obj = Request.QueryString["userAccount"];
                    if (null == obj)
                    {
                        string name = fields["Applicant"].AsString();
                        string userAccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
                        Employee employee = UserProfileUtil.GetEmployee(userAccount);
                        if (employee != null)
                        {
                            this.ApplicantEmployee = employee;
                            this.Applicant = this.ApplicantEmployee.DisplayName + "(" + this.ApplicantEmployee.UserAccount + ")";
                            if (fields["Status"].AsString() == "Finance Pending" || fields["Status"].AsString() == "Rejected")
                            {
                                BindDDLEmployeeList(this.ApplicantEmployee.UserAccount, false);
                            }
                            else
                            {
                                BindDDLEmployeeList(this.CurrentEmployee.UserAccount, true);
                            }
                        }
                    }
                    else
                    {
                        string userAccount = obj.ToString();
                        Employee employee = UserProfileUtil.GetEmployee(userAccount);
                        //string name = fields["Applicant"].AsString();
                        //string userAccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
                        //Employee employee = UserProfileUtil.GetEmployee(userAccount);
                        if (employee != null)
                        {
                            this.ApplicantEmployee = employee;
                            this.Applicant = this.ApplicantEmployee.DisplayName + "(" + this.ApplicantEmployee.UserAccount + ")";
                            if (fields["Status"].AsString() == "Finance Pending" || fields["Status"].AsString() == "Rejected")
                            {
                                BindDDLEmployeeList(this.ApplicantEmployee.UserAccount, false);
                            }
                            else
                            {
                                BindDDLEmployeeList(this.ApplicantEmployee.UserAccount, true);
                            }
                        }
                    }

                    LoadSourceData(this.ApplicantEmployee.EmployeeID, WorkflowConfigName.CreditCardEmployeeMapping, fields["Status"].AsString());
                    DataDataFields(fields);
                }
            }
        }

        private void BindDDLEmployeeList(string userAccount, bool status)
        {
            this.ddlEmployeeList.Items.Clear();
            this.ddlEmployeeList.Items.Add(new ListItem(userAccount, userAccount));
            if (status)
            {
                var delegationList = SharePointUtil.GetList("Credit Card Employee Delegate Mapping");
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><Eq><FieldRef Name='DelegateCardHolderAccount' /><Value Type='Text'>{0}</Value></Eq></Where>", userAccount);
                SPListItemCollection listItems = delegationList.GetItems(query);
                if (null != listItems && listItems.Count > 0)
                {
                    foreach (SPListItem spli in listItems)
                    {
                        ListItem li = new ListItem();
                        li.Text = spli["OriginalCardHolderAccount"].ToString();
                        li.Value = spli["OriginalCardHolderAccount"].ToString();
                        this.ddlEmployeeList.Items.Add(li);
                    }
                }
            }
            if (!status)
            {
                this.ddlEmployeeList.Enabled = false;
                this.ddlMonth.Enabled = false;
            }
        }

        private void LoadSourceData(string employeeId, string listName, string wfStatus)
        {
            SPListItemCollection items = CreditCardClaimCommon.GetDataCollection(employeeId, listName);

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            if (items.Count > 0)
            {
                string cardNo = items[0]["CardNo"].AsString();

                SPListItemCollection cardItems = CreditCardClaimCommon.GetDataCollection(cardNo, WorkflowConfigName.CreditCardBill);

                BillInfo = cardItems.GetDataTable();

                DateTime curDate = DateTime.Now;
                List<string> monthList = new List<string>();

                List<string> monthListData = new List<string>();
                var delegationList = SharePointUtil.GetList("Credit Card Claim Workflow");
                SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><And><Eq><FieldRef Name='Applicant' /><Value Type='Text'>{0}</Value></Eq><Neq><FieldRef Name='Status' /><Value Type='Text'>Finance Pending</Value></Neq></And></Where>", this.Applicant);
                query.Query = string.Format("<Where><Eq><FieldRef Name='Applicant' /><Value Type='Text'>{0}</Value></Eq></Where>", this.Applicant);
                SPListItemCollection listItems = delegationList.GetItems(query);
                if (null != listItems && listItems.Count > 0)
                {
                    foreach (SPListItem li in listItems)
                    {
                        if (this.ControlMode == SPControlMode.New)
                        {
                            if (li["Status"].ToString() != "Finance Pending")
                            {
                                monthListData.Add(li["Month"].ToString());
                            }
                        }
                        if (this.ControlMode == SPControlMode.Edit)
                        {
                            if (li["Status"].ToString() == "In Progress" || li["Status"].ToString() == "Completed")
                            {
                                monthListData.Add(li["Month"].ToString());
                            }
                        }
                    }
                }

                foreach (SPListItem item in cardItems)
                {
                    DateTime date = DateTime.Parse(item["UploadDate"].AsString());
                    string month = date.ToString("yyyy-MM");
                    //if (wfStatus == "Pending" || wfStatus == "Finance Pending" || wfStatus == "Rejected")
                    //if (wfStatus != "")
                    //{
                    //    if (wfStatus == "Pending" || wfStatus == "Finance Pending")
                    //    {
                    //        if (date.Year == curDate.Year
                    //           && CreditCardClaimCommon.IsUniqueValue(monthList, month))
                    //        {
                    //            monthList.Add(month);
                    //        }
                    //    }
                    //    if (wfStatus == "Rejected") 
                    //    {

                    //    }
                    //}
                    //else
                    //{
                    //if (date.Year == curDate.Year
                    //    && CreditCardClaimCommon.IsUniqueValue(monthList, month) && !monthListData.Contains(month))
                    if (CreditCardClaimCommon.IsUniqueValue(monthList, month) && !monthListData.Contains(month))
                    {
                        monthList.Add(month);
                    }
                    //}
                }

                monthList.Sort(Compare);

                this.ddlMonth.Items.Clear();
                foreach (string month in monthList)
                {
                    this.ddlMonth.Items.Add(new ListItem(month, month));
                }
                ddlMonth.DataBind();

                //if (this.ControlMode == SPControlMode.New
                //    || WorkflowContext.Current.DataFields["Status"].AsString() == CAWorkflowStatus.CCFinancePending)
                //{
                //    if (monthList.Count > 0)
                //    {
                //        DataBindTradeInfo();
                //    }
                //}
                //else
                //{
                //    if (monthList.Count > 0)
                //    {
                //        DataBindTradeInfoForEdit();
                //    }
                //}

                if (this.ControlMode == SPControlMode.New)
                {
                    if (monthList.Count > 0)
                    {
                        DataBindTradeInfo();
                    }
                }
                if (this.ControlMode == SPControlMode.Edit)
                {
                    WorkflowDataFields fields = WorkflowContext.Current.DataFields;

                    if (fields["SaveStatus"].AsString() == "SaveStatus")
                    {
                        ddlMonth.SelectedValue = fields["Month"].AsString();
                        DataBindTradeInfoForEdit();
                    }
                    else
                    {
                        if (wfStatus == "Rejected")
                        {
                            if (monthList.Count > 0)
                            {
                                ddlMonth.SelectedValue = fields["Month"].AsString();
                                DataBindTradeInfoForEdit();
                            }
                        }
                        if (wfStatus == "Finance Pending")
                        {
                            if (monthList.Count > 0)
                            {
                                ddlMonth.SelectedValue = fields["Month"].AsString();
                                DataBindTradeInfo();
                            }
                        }
                        if (wfStatus == "")
                        {
                            if (monthList.Count > 0)
                            {
                                DataBindTradeInfo();
                            }
                        }
                        if (wfStatus == "Pending")
                        {
                            ddlMonth.SelectedValue = fields["Month"].AsString();
                            DataBindTradeInfoForEdit();
                        }
                    }
                }

                DataBindCostCenter();

                //List<object> costCenterInfo = CreditCardClaimCommon.GetSerializingList(SPContext.Current.Web.Lists["Cost Centers"].Items,
                //                                                                        new CostCenterItem());

                //hidCostCenterInfo.Value = oSerializer.Serialize(costCenterInfo);               
            }
            else
            {
                this.ddlMonth.Items.Clear();
                rptTradeInfo.DataSource = null;
                rptTradeInfo.DataBind();
            }
        }

        protected void DataBindCostCenter()
        {
            DataTable dtCostCenter = WorkFlowUtil.GetDataSourceBySort(SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable());
            foreach (RepeaterItem item in this.rptTradeInfo.Items)
            {
                DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                ddlCostCenter.DataSource = dtCostCenter;
                ddlCostCenter.DataTextField = "Display";
                ddlCostCenter.DataValueField = "Title";
                ddlCostCenter.DataBind();
                ddlCostCenter.Items.Insert(0, new ListItem("", "-1"));
            }

        }

        internal void DataBindTradeInfo()
        {
            string month = this.ddlMonth.SelectedValue;

            DataTable dt = CreditCardClaimCommon.GetDataSource(BillInfo, "UploadDate like '" + month + "%'");
            if (dt != null)
            {
                dt.Columns.Add("rCurrency");
                dt.Columns.Add("IsTravelRequest");
                dt.Columns.Add("IsPersonal");
                dt.Columns.Add("IsClaim");
                dt.Columns.Add("ExpensePurpose");
                dt.Columns.Add("ExpenseType");
                dt.Columns.Add("CostCenter");

                foreach (DataRow dr in dt.Rows)
                {
                    string currency = dr["Currency"].AsString();
                    string transAmt = dr["TransAmt"].AsString();
                    string depositAmt = dr["DepositAmt"].AsString();
                    string payAmt = dr["PayAmt"].AsString();

                    switch (currency)
                    {
                        case "RMB":
                            dr["rCurrency"] = "RMB";

                            break;
                        default:
                            dr["TransAmt"] = transAmt;
                            dr["DepositAmt"] = depositAmt;
                            dr["PayAmt"] = payAmt;
                            dr["rCurrency"] = "USD";
                            break;
                    }
                }

                //JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                //List<object> creditCardBillInfo = CreditCardClaimCommon.GetSerializingList(dt, new CreditCardBillItem());

                //hidCostCenterInfo.Value = oSerializer.Serialize(creditCardBillInfo);

                rptTradeInfo.DataSource = dt;
                rptTradeInfo.DataBind();
            }
            else 
            {
                rptTradeInfo.DataSource = null;
                rptTradeInfo.DataBind();
            }

        }

        private void DataBindTradeInfoForEdit()
        {
            DataTable dt = WorkFlowUtil.GetCollection(requestId, WorkflowConfigName.CreditCardClaimDetail).GetDataTable();
            if (dt != null)
            {
                dt.Columns.Add("rCurrency");
                dt.Columns.Add("Currency");

                foreach (DataRow dr in dt.Rows)
                {
                    string transAmt = dr["TransAmt"].AsString();
                    string depositAmt = dr["DepositAmt"].AsString();
                    string payAmt = dr["PayAmt"].AsString();

                    dr["TransAmt"] = transAmt.Split('/')[0];
                    dr["DepositAmt"] = depositAmt.Split('/')[0];
                    dr["PayAmt"] = payAmt.Split('/')[0];
                    if (depositAmt.Split('/')[1].Equals("RMB"))
                    {
                        dr["rCurrency"] = "RMB";
                    }
                    else
                    {
                        dr["rCurrency"] = "USD";
                    }
                    dr["Currency"] = transAmt.Split('/')[1];
                    dr["Title"] = dr["CardNo"].AsString();
                }

                rptTradeInfo.DataSource = dt;
                rptTradeInfo.DataBind();
            }
            else
            {
                rptTradeInfo.DataSource = null;
                rptTradeInfo.DataBind();
            }
        }

        private int Compare(string x, string y)
        {
            return x.CompareTo(y);
        }

        public void DataDataFields(WorkflowDataFields fields)
        {
            string name = fields["Applicant"].AsString();
            string userAccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
            foreach(ListItem li in this.ddlEmployeeList.Items)
            {
                if (userAccount.Equals(li.Value, StringComparison.CurrentCultureIgnoreCase)) 
                {
                    li.Selected = true;
                }
            }
            
            this.lblDepartment.Text = fields["Department"].AsString();
            this.lblRequestedBy.Text = fields["Applicant"].AsString();
            this.rbtAttachInvoice.SelectedValue = fields["InvoiceStatus"].AsString();
            this.RMBSummaryExpenseType = fields["RMBSummaryExpenseType"].AsString();
            this.USDSummaryExpenseType = fields["USDSummaryExpenseType"].AsString();
        }

        public string CheckInfo()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            //if (rbtAttachInvoice.SelectedValue=="1") 
            //{
            //    if (attacthment..File==null) 
            //    {
            //        output.Append("please upload Attachment");
            //    }
            //}

            return output.ToString();
        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

        protected void btnBillInfoBind_Click(object sender, EventArgs e)
        {
            string userAccount = this.ddlEmployeeList.SelectedValue;
            //if (userAccount == this.ApplicantEmployee.UserAccount)
            if (userAccount.Equals(this.ApplicantEmployee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
            {
                DataBindTradeInfo();
                DataBindCostCenter();
            }
            else
            {
                Employee employee = UserProfileUtil.GetEmployee(userAccount);
                if (employee != null)
                {
                    this.ApplicantEmployee = employee;
                    this.Applicant = this.ApplicantEmployee.DisplayName + "(" + this.ApplicantEmployee.UserAccount + ")";
                    this.lblRequestedBy.Text = this.Applicant;
                    this.lblDepartment.Text = this.ApplicantEmployee.Department;
                    WorkflowDataFields fields = WorkflowContext.Current.DataFields;

                    LoadSourceData(this.ApplicantEmployee.EmployeeID, WorkflowConfigName.CreditCardEmployeeMapping, "");

                }
            }
        }

        internal void SaveCommonData()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            fields["WorkflowNumber"] = this.requestId;
            fields["Department"] = lblDepartment.Text.Trim();
            fields["Month"] = ddlMonth.SelectedValue;
            fields["InvoiceStatus"] = rbtAttachInvoice.SelectedValue;
            //PreTotalAmount
            //fields["PreTotalAmount"] = this.DataForm1.PreTotalAmount;

            fields["RMBSummaryExpenseType"] = this.RMBSummaryExpenseType.Trim();
            fields["USDSummaryExpenseType"] = this.USDSummaryExpenseType.Trim();
        }

        internal void SaveDetails(string workflowNumber)
        {

            Hashtable colshash = new Hashtable();
            colshash.Add("TransDate", "lblTransDate;#Label");
            colshash.Add("TransDesc", "lblTransDesc;#Label");
            colshash.Add("CardNo", "lblCardNo;#Label");
            colshash.Add("MerchantName", "lblMerchantName;#Label");
            colshash.Add("TransAmt", "lblTransAmt;#Label");
            colshash.Add("DepositAmt", "lblDepositAmt;#Label");
            colshash.Add("PayAmt", "lblPayAmt;#Label");
            colshash.Add("IsClaim", "cbClaim;#CheckBox");
            colshash.Add("ExpensePurpose", "txtExpensePurpose;#TextBox");
            colshash.Add("ExpenseType", "ddlExpenseType;#DropDownList");
            colshash.Add("CostCenter", "ddlCostCenter;#DropDownList");

            colshash.Add("IsTravelRequest", "cbTravelRequest;#CheckBox");
            colshash.Add("IsPersonal", "cbPersonal;#CheckBox");

            CreditCardClaimCommon.BatchInsertRepeaterData(rptTradeInfo, WorkflowConfigName.CreditCardClaimDetail, workflowNumber, colshash);
        }

        protected string GetExchRate(string currency)
        {
            var exchRate = "";
            var rateItem = CreditCardClaimCommon.ConvertToRMB(currency);
            if (rateItem != null)
            {
                exchRate = (Convert.ToDouble(rateItem["Rate"].AsString())).ToString();

            }
            return exchRate;
        }

        #endregion


        internal void TerminateWorkflow(string type,string currentId)
        {
            string claimedMonth = ddlMonth.SelectedValue;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPListItem wfItem = GetFinancePendingWFItem(claimedMonth, web, this.ApplicantEmployee, type, currentId);
                        if (wfItem != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                SPWorkflowManager wfManager = site.WorkflowManager;

                                foreach (SPWorkflow workflow in wfManager.GetItemActiveWorkflows(wfItem))
                                {
                                    foreach (SPWorkflowTask task in workflow.Tasks)
                                    {
                                        task["Status"] = "Canceled";
                                        task.Update();
                                    }
                                    SPWorkflowManager.CancelWorkflow(workflow);
                                }
                            }
                            catch (Exception ex)
                            {

                                CommonUtil.logInfo(string.Format("{0}:Cancel Workflow {1}({2}) fails.\n more details:{3}.",
                                                                   wfItem.ParentList.Title, wfItem.Title, wfItem.ID, ex.Message));
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }

                        }
                    }
                }
            });
        }

        private SPListItem GetFinancePendingWFItem(string claimedMonth, SPWeb web, Employee applicantEmployee, string type, string currentId)
        {
            SPListItem wfItem = null;
            SPList list = web.Lists[WorkflowListName.CreditCardClaim];
            foreach (SPListItem item in list.Items)
            {
                if (IsSpecificWFItem(item, claimedMonth, applicantEmployee.UserAccount, type, currentId))
                {
                    wfItem = item;
                    break;
                }
            }

            return wfItem;
        }

        private bool IsSpecificWFItem(SPListItem item, string claimedMonth, string userAccount, string type, string currentId)
        {
            bool isSpecificWFItem = false;
            if (item["Month"].AsString().Equals(claimedMonth, StringComparison.CurrentCultureIgnoreCase)
                && IsSpecificUser(item["ApplicantSPUser"].AsString(), userAccount))
            {
                if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(userAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (item["Status"].AsString().Equals(CAWorkflowStatus.CCFinancePending, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isSpecificWFItem = true;
                    }
                }
                else
                {
                    switch (type) { 
                        case "NewMode":
                            if (item["Status"].AsString().Equals(CAWorkflowStatus.CCFinancePending, StringComparison.CurrentCultureIgnoreCase))
                            {
                                isSpecificWFItem = true;
                            }
                            break;
                        case "EditMode":
                            if (!item["Title"].AsString().Equals(currentId, StringComparison.CurrentCultureIgnoreCase))
                            {
                                isSpecificWFItem = true;
                            }
                            break;
                        default:
                            break;

                    }
                }
            }
            return isSpecificWFItem;
        }



        private bool IsSpecificUser(string applicantSPUser, string userAccount)
        {
            bool isSpecificUser = false;
            if (applicantSPUser.IsNotNullOrWhitespace())
            {
                int userId = int.Parse(applicantSPUser.Split(new string[] { ";#" }, StringSplitOptions.None)[0]);
                try
                {
                    SPUser user = SPContext.Current.Web.AllUsers.GetByID(userId);
                    if (user.LoginName.Equals(userAccount, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isSpecificUser = true;
                    }
                }
                catch (Exception e)
                {
                    CommonUtil.logError(string.Format("Credit Card Claim：User :: {0}\n MSG ::{1}", applicantSPUser, e.Message));
                }
            }
            return isSpecificUser;
        }
    }

    public class CreditCardBillItem
    {
        public string CardNo { get; set; }
        public string TransDesc { get; set; }
        public string TransAmt { get; set; }
        public string DepositAmt { get; set; }
        public string PayAmt { get; set; }
        public string Currency { get; set; }
    }

    public class CostCenterItem
    {
        public string Display { get; set; }
        public string Title { get; set; }
    }

}