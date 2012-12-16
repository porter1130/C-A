
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
using System.Reflection;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using QuickFlow.Core;
//using System.Web.Extensions;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{

    public partial class DataEdit : BaseWorkflowUserControl
    {
        private string mode;

        public string Mode
        {
            set { mode = value; }
            get { return mode; }
        }

        public string ApplicantEmployeeId
        {
            get;
            set;
        }

        internal DataTable ExpenseTable
        {
            get
            {
                return (this.ViewState["ExpenseTable"] as DataTable) ?? CreateExpenseTable();
            }
            set
            {
                this.ViewState["ExpenseTable"] = value;
            }
        }

        private DataTable CreateExpenseTable()
        {
            ExpenseTable = new DataTable();
            ExpenseTable.TableName = "Expense";
            ExpenseTable.Columns.Add("ExpenseType");
            ExpenseTable.Columns.Add("CostCenter");
            ExpenseTable.Columns.Add("ApprovedRmbAmt");
            ExpenseTable.Columns.Add("GLAccount");
            ExpenseTable.Columns.Add("SAPSection");
            //ExpenseTable.Rows.Add();
            return ExpenseTable;
        }

        internal DataTable SAPSummaryTable
        {
            get
            {
                return (this.ViewState["SAPSummaryTable"] as DataTable) ?? CreateSAPSummaryTable();
            }
            set
            {
                this.ViewState["SAPSummaryTable"] = value;
            }
        }

        private DataTable CreateSAPSummaryTable()
        {
            SAPSummaryTable = new DataTable();
            SAPSummaryTable.TableName = "SAPSummary";
            SAPSummaryTable.Columns.Add("ExpenseType");
            SAPSummaryTable.Columns.Add("CostCenter");
            SAPSummaryTable.Columns.Add("ApprovedRmbAmt");
            SAPSummaryTable.Columns.Add("GLAccount");
            SAPSummaryTable.Columns.Add("SAPSection");

            //ExpenseTable.Rows.Add();
            return SAPSummaryTable;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (!this.Page.IsPostBack)
            {

                SourceDataBind(fields["WorkflowNumber"].AsString(), fields["TCWorkflowNumber"].AsString(), WorkflowListName.TravelExpenseClaimDetailsForSAP);
                LoadSourceData(fields["TCWorkflowNumber"].AsString());

            }

            string userAccount = fields["ApplicantSPUser"].AsString().Split(new string[] { ";#" }, StringSplitOptions.None)[1];
            this.ApplicantEmployeeId = UserProfileUtil.GetEmployee(userAccount).EmployeeID;
        }

        private void LoadSourceData(string id)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();

            if (id.IsNotNullOrWhitespace())
            {

                SPListItemCollection travelExpenseClaimItems = TravelExpenseClaimForSAPCommon.GetDataCollection(id, WorkflowListName.TravelExpenseClaim);

                SPListItemCollection sapGLAccountItems = SPContext.Current.Web.Lists[WorkflowListName.ExpenseClaimSAPGLAccount].Items;

                List<object> travelExpenseClaimInfo = TravelExpenseClaimForSAPCommon.GetSerializingList(travelExpenseClaimItems, new TravelExpenseClaimItem());
                List<object> sapGLAccountInfo = TravelExpenseClaimForSAPCommon.GetSerializingList(sapGLAccountItems, new SAPGLAccountItem());

                hidTravelDetails.Value = oSerializer.Serialize(travelExpenseClaimInfo);
                hidSAPGLAccount.Value = oSerializer.Serialize(sapGLAccountInfo);

                this.txtCCBalanceRmbAmt.Text = TravelExpenseClaimForSAPCommon.GetCreditCardBalance(id, WorkflowListName.TravelExpenseClaimDetails);

                if (travelExpenseClaimItems.Count > 0)
                {
                    TravelExpenseClaimForSAPCommon.GetNetPayable(travelExpenseClaimItems[0], this.txtCCBalanceRmbAmt.Text, this.txtEVRmbAmt, this.txtCABalanceRmbAmt);
                }
            }
        }

        private void SourceDataBind(string workflowNumber, string tcWorkflowNumber, string listName)
        {
            DataTable dt = TravelExpenseClaimForSAPCommon.GetDataTable(workflowNumber, listName);
            if (dt != null)
            {
                this.Mode = "Edit";
                this.rptExpense.DataSource = TravelExpenseClaimForSAPCommon.GetDataSource(dt, "SAPSection='0'");
            }
            else
            {
                this.Mode = "New";
                DataTable dtExpense = TravelExpenseClaimForSAPCommon.GetExpenseDataSource(TravelExpenseClaimForSAPCommon.GetDataTable(tcWorkflowNumber, WorkflowListName.TravelExpenseClaimDetails));

                this.rptExpense.DataSource = dtExpense;
            }
            rptExpense.DataBind();
        }

        private void Repeater_CostCenterDataBind(RepeaterItem item, DataRowView row)
        {
            DropDownList ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
            DataTable dtCostCenter = SPContext.Current.Web.Lists["Cost Centers"].Items.GetDataTable();
            ddlCostCenter.DataSource = WorkFlowUtil.GetDataSourceBySort(dtCostCenter);
            ddlCostCenter.DataTextField = "Display";
            ddlCostCenter.DataValueField = "Title";
            ddlCostCenter.DataBind();
            ddlCostCenter.Items.Insert(0, new ListItem("", ""));
            if (row["CostCenter"].ToString().IsNotNullOrWhitespace())
            {
                ddlCostCenter.Items.FindByValue(row["CostCenter"].ToString()).Selected = true;
            }
        }

        protected void rptExpense_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateDataTable(this.ExpenseTable, this.rptExpense);
                ExpenseTable.Rows.Remove(ExpenseTable.Rows[e.Item.ItemIndex]);
                this.rptExpense.DataSource = ExpenseTable;
                this.rptExpense.DataBind();
            }
        }

        protected void rptExpense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var row = item.DataItem as DataRowView;
                if (row != null)
                {
                    var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                    var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");

                    if (this.Mode.Equals("Edit", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var txtGLAccount = (TextBox)item.FindControl("txtGLAccount");
                        txtGLAccount.Text = row["GLAccount"].AsString();
                    }

                    ddlExpenseType.SelectedValue = ReturnExpenseType(row["ExpenseType"].ToString());
                    txtRmbAmt.Text = row["ApprovedRmbAmt"].ToString();

                    Repeater_CostCenterDataBind(item, row);

                }
            }
        }

        private string ReturnExpenseType(string type)
        {
            string expenseType = string.Empty;

            switch (type)
            {
                case "Hotel":
                    expenseType = "Travel - hotel";
                    break;
                case "Meal Allowance":
                    expenseType = "Travel - meal";
                    break;
                case "Local Transportation":
                    expenseType = "Travel - local transportation";
                    break;
                case "Sample Purchase":
                    expenseType = "Travel - sample purchase";
                    break;
                case "Others":
                    expenseType = "Travel - others";
                    break;
                default:
                    expenseType = type;
                    break;
            }

            return expenseType;
        }

        protected void btnAddExpense_Click(object sender, ImageClickEventArgs e)
        {

            UpdateDataTable(this.ExpenseTable, this.rptExpense);

            ExpenseTable.Rows.Add();

            this.rptExpense.DataSource = this.ExpenseTable;
            this.rptExpense.DataBind();
        }

        private void UpdateDataTable(DataTable dataTable, Repeater repeater)
        {
            dataTable.Rows.Clear();
            DataRow row;

            switch (dataTable.TableName)
            {
                case "SAPSummary":

                    if (txtCARmbAmt.Text.Trim() != "0")
                    {
                        row = dataTable.Rows.Add();
                        row["ExpenseType"] = "Cash Advance - OR";
                        row["ApprovedRmbAmt"] = txtCARmbAmt.Text;
                        row["CostCenter"] = "";
                        row["GLAccount"] = hidCAGLAccount.Value;
                        row["SAPSection"] = "1";
                    }

                    if (txtCCRmbAmt.Text.Trim() != "0")
                    {
                        row = dataTable.Rows.Add();
                        row["ExpenseType"] = "Credit Card - OR";
                        row["ApprovedRmbAmt"] = txtCCRmbAmt.Text;
                        row["CostCenter"] = "";
                        row["GLAccount"] = hidCCGLAccount.Value;
                        row["SAPSection"] = "1";
                    }

                    if (txtCCBalanceRmbAmt.Text.Trim() != "0")
                    {
                        row = dataTable.Rows.Add();
                        row["ExpenseType"] = "Credit Card - OR";
                        row["ApprovedRmbAmt"] = txtCCBalanceRmbAmt.Text;
                        row["CostCenter"] = "";
                        row["GLAccount"] = hidCCBalanceGLAccount.Value;
                        row["SAPSection"] = "1";
                    }

                    if (txtEVRmbAmt.Text.Trim() != "0")
                    {
                        row = dataTable.Rows.Add();
                        row["ExpenseType"] = "Employee Vendor - OP";
                        row["ApprovedRmbAmt"] = txtEVRmbAmt.Text;
                        row["CostCenter"] = "";
                        row["GLAccount"] = hidEVGLAccount.Value;
                        row["SAPSection"] = "1";
                    }

                    if (txtCABalanceRmbAmt.Text.Trim() != "0")
                    {
                        row = dataTable.Rows.Add();
                        row["ExpenseType"] = "Cash Advance - OR";
                        row["ApprovedRmbAmt"] = txtCABalanceRmbAmt.Text;
                        row["CostCenter"] = "";
                        row["GLAccount"] = hidCABalanceGLAccount.Value;
                        row["SAPSection"] = "1";
                    }

                    break;
                default:
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        var ddlExpenseType = (DropDownList)item.FindControl("ddlExpenseType");
                        var ddlCostCenter = (DropDownList)item.FindControl("ddlCostCenter");
                        var txtRmbAmt = (TextBox)item.FindControl("txtRmbAmt");
                        var txtGLAccount = (TextBox)item.FindControl("txtGLAccount");

                        row = dataTable.Rows.Add();
                        row["ExpenseType"] = ddlExpenseType.SelectedValue;
                        row["CostCenter"] = ddlCostCenter.SelectedValue;
                        row["ApprovedRmbAmt"] = txtRmbAmt.Text;
                        row["GLAccount"] = txtGLAccount.Text;
                        row["SAPSection"] = "0";
                    }
                    break;
            }

        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

        internal void Update()
        {
            UpdatePurpose();
            UpdateDataTable(this.ExpenseTable, this.rptExpense);
            UpdateDataTable(this.SAPSummaryTable, null);
        }

        private void UpdatePurpose()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            if (fields["TCWorkflowNumber"].AsString().IsNotNullOrWhitespace())
            {
                TravelExpenseClaimForSAPCommon.UpdateTargetFieldValue("Purpose", txtPurpose.Text.Trim(),
                                                                       "Title", fields["TCWorkflowNumber"].AsString(),
                                                                       WorkflowListName.TravelExpenseClaim);
            }

            string trWFNo = TravelExpenseClaimForSAPCommon.ReturnTargetFieldValue("TRWorkflowNumber",
                                                                                "Title", fields["TCWorkflowNumber"].AsString(),
                                                                                WorkflowListName.TravelExpenseClaim);
            if (trWFNo.IsNotNullOrWhitespace())
            {
                TravelExpenseClaimForSAPCommon.UpdateTargetFieldValue("TravelOtherPurpose", txtPurpose.Text.Trim(),
                                                                       "Title", trWFNo,
                                                                       WorkflowListName.TravelRequestWorkflow2);
            }
        }

        internal object ValidateForSave()
        {
            throw new NotImplementedException();
        }

        private bool ReturnCheckBoxValue(string s)
        {
            bool restult = false;
            int temp;
            if (!string.IsNullOrEmpty(s))
            {
                if (Int32.TryParse(s, out temp))
                {
                    restult = Convert.ToBoolean(temp);
                }
                else
                {
                    restult = Convert.ToBoolean(s);
                }
            }
            return restult;

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

        internal string ValidateForSubmit()
        {
            string errorMessage = string.Empty;

            if (rptExpense.Items.Count == 0)
            {
                errorMessage = "You have unclaimed items. Please make sure they are related to trips applied through Travel Request. If not, please claim them here.";
            }
            return errorMessage;
        }
    }

    public class TravelExpenseClaimItem
    {
        public string Applicant { get; set; }
        public string ChineseName { get; set; }
        public string Department { get; set; }
        public string EnglishName { get; set; }
        public string Mobile { get; set; }
        public string OfficeExt { get; set; }
        public string IDNumber { get; set; }
        public string CashAdvanced { get; set; }
        public string PaidByCreditCard { get; set; }
        public string NetPayable { get; set; }
        public string ComparedToApproved { get; set; }
        public string TotalCost { get; set; }
        public string TotalExceptFlight { get; set; }
        public string Purpose { get; set; }
    }

    public class SAPGLAccountItem
    {
        public string ExpenseType { get; set; }
        public string GLAccount { get; set; }
    }
}