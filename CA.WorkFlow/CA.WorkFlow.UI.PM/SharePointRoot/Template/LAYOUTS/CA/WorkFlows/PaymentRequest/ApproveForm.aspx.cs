namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using CA.SharePoint.Utilities.Common;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.SharePoint.WebControls;
    using QuickFlow;
    using System.Configuration;
    using System.Collections;
    using System.Text.RegularExpressions;
    using System.Data;
    using System.Text;
    using CodeArt.SharePoint.CamlQuery;

    public partial class ApproveForm : CAWorkFlowPage
    {
        public string AlertMsg = "";
        public string HiddenButton = string.Empty; 
        protected void Page_Load(object sender, EventArgs e)
        {
            this.hfNoticeStatus.Value = "FromIMG";
            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"]) 
            {
                this.hfNoticeStatus.Value = "FromPO";
            }

            this.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            this.ApplicantLabel.Text = this.Applicant;
            this.PaymentRequestDataView.Step = WorkflowContext.Current.Step;  
            this.PaymentRequestDataView.RequestId = WorkflowContext.Current.DataFields["SubPRNo"].ToString();
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase)){
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true)){
                RedirectToTask();
            }

            if (WorkflowContext.Current.Step == "ApproveTask"){
                HiddenButton = (bool)WorkflowContext.Current.DataFields["IsContractPO"] == true ? "Approve" : "Confirm";
            }

            this.Actions.OnClientClick = "return dispatchAction(this);";
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            if (WorkflowContext.Current.Step == "ConfirmTask")
            {
                if (WorkflowContext.Current.DataFields["ReasonsResult"] != null)
                {
                    if (WorkflowContext.Current.DataFields["ReasonsResult"].ToString() == "1"){
                        this.PaymentRequestDataView.Pending = "Pending";
                    }
                }
            }

            if (!IsPostBack)
            {
                if (WorkflowContext.Current.Step == "ConfirmTask") {
                    CheckPaymentSapWorkflowStatus();
                }
            }
            this.PaymentRequestDataView.Wfstep = WorkflowContext.Current.Step;
        }

        public string Applicant
        {
            get
            {
                return this.ViewState["Applicant"].AsString();
            }
            set
            {
                this.ViewState["Applicant"] = value;
            }
        }

        private void CheckPaymentSapWorkflowStatus()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            
            bool isFromPO = (bool)fields["IsFromPO"];
            bool isNeedStartSAPWF = !isFromPO;
            if (isFromPO == true)
            {
                DataTable dt = PaymentRequestComm.GetPurchaseOrderWorkflowInfo(fields["PONo"].ToString()).GetDataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string s = dt.Rows[0]["IsSystemGR"].ToString();
                    if (s == "0" || s.IsNullOrWhitespace()){
                        isNeedStartSAPWF = true;
                    }
                }
            }

            ViewState["isNeedStartSAPWF"] = isNeedStartSAPWF;
            if (fields["RequestType"].AsString().ToLower() == "opex" || isFromPO == true)
            {
                AlertMsg = (isNeedStartSAPWF == false ? "System GR is completed; no need to start PR SAP process.  Please proceed to Invoice Verification" :
                "Please proceed to PR SAP process to start SAP posting");
            }
            if (fields["RequestType"].AsString().ToLower() == "capex" && isFromPO == false) 
            {
                AlertMsg = "当前没产生SAP流程，需要手工操作！";
            }
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            //string taskTitle = fields["Applicant"] + "'s Payment Request ";
            //SubPRNo
            string taskTitle = fields["SubPRNo"].AsString() + " " + PaymentRequestDataView.VendorNameText + " " + PaymentRequestDataView.ApproveAmount + " " + fields["Applicant"].AsString() + "'s Payment Request ";
            switch (WorkflowContext.Current.Step)
            {
                case "ApproveTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase) ||
                        e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
                    {
                        bool isConfirm = true;
                        string managerStr = fields["Manager"].ToString();
                        bool isCEO = PaymentRequestComm.IsCEO(managerStr);
                        if ((bool)fields["IsContractPO"] == false && isCEO == false)
                        {
                            double paid = Convert.ToDouble(fields["TotalAmount"].ToString()) - Convert.ToDouble(fields["PaidBefore"].ToString()) *
                                Convert.ToDouble(fields["TotalAmount"].ToString()) / 100;
                            long quota = PaymentRequestComm.GetQuota(managerStr);
                            paid = GetTotalAmountByExchangeRate(paid, fields["Currency"].AsString() == "" ? "RMB" : fields["Currency"].AsString());
                            if (paid > quota)
                            {
                                isConfirm = false;
                                //获取下一个审批者
                                Employee cfoEmp = null;
                                Employee managerEmp = PaymentRequestComm.GetNextApproverEmp(managerStr);
                                if (managerEmp == null)
                                {
                                    string errorMsg = @"The applicant\'s budget is greater than your approved amount limits, and your manager is not set in system";
                                    DisplayMessage(errorMsg);
                                    e.Cancel = true;
                                    return;
                                }

                                #region
                                bool isCeo = PaymentRequestComm.IsCEO(managerEmp.UserAccount);
                                if (isCeo)
                                {
                                    List<string> cfos = WorkflowPerson.GetCFO();
                                    if (cfos.Count == 0)
                                    {
                                        //DisplayMessage("The init error about WorkflowPerson in the system.");
                                        string errorMsg = "The init error about WorkflowPerson in the wf_CFO group.";
                                        DisplayMessage(errorMsg);
                                        e.Cancel = true;
                                        return;
                                    }
                                    cfoEmp = UserProfileUtil.GetEmployeeEx(cfos[0]);
                                    string cfoUserAccount = cfoEmp.UserAccount;
                                    //bool isCfo = PaymentRequestComm.IsExistsCFOByCurrentEmployee(fields, cfoUserAccount);
                                    if (!managerStr.Equals(cfoUserAccount, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        managerEmp = cfoEmp;
                                    }
                                }
                                #endregion

                                var manager = new NameCollection();
                                PaymentRequestComm.GetTaskUsersByModule(manager, managerEmp.UserAccount, "PaymentRequest");
                                context.UpdateWorkflowVariable("ApproveTaskTitle", taskTitle);
                                //context.UpdateWorkflowVariable("ApproveTaskUsers", manager);
                                context.UpdateWorkflowVariable("ApproveTaskUsers", GetDelemanNameCollection(manager, "129"));
                                context.UpdateWorkflowVariable("IsContinue", true);
                                context.UpdateWorkflowVariable("IsReject", false);
                                fields["Status"] = CAWorkflowStatus.InProgress;
                                fields["Manager"] = managerEmp.UserAccount;
                            }
                        }
                        if(isConfirm)
                        {
                            //string groupName = ((bool)fields["IsFromPO"] == true ? PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview : 
                            //                                                       PaymentRequestGroupNames.Opex_GeneralPO_SAPReview);
                            string groupName = "";
                            if ((bool)fields["IsFromPO"] == true)
                            {
                                if (fields["RequestType"].ToString().ToLower() == "opex")
                                {
                                    groupName = PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview;
                                }
                                else
                                {
                                    groupName = PaymentRequestGroupNames.Capex_ConstructionPO_SAPReview;
                                }
                            }
                            else 
                            {
                                if (fields["RequestType"].ToString().ToLower() == "opex")
                                {
                                    groupName = PaymentRequestGroupNames.Opex_GeneralPO_SAPReview;
                                }
                                else
                                {
                                    groupName = PaymentRequestGroupNames.Capex_GeneralPO_SAPReview;
                                }
                            }
                            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle );
                            context.UpdateWorkflowVariable("ConfirmTaskUsers", WorkFlowUtil.GetUsersInGroup(groupName));
                            context.UpdateWorkflowVariable("IsContinue", false);
                            context.UpdateWorkflowVariable("IsConfirm", true);
                            context.UpdateWorkflowVariable("IsReject", false);
                        }

                        
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Payment Request");
                        context.UpdateWorkflowVariable("IsContinue", false);
                        context.UpdateWorkflowVariable("IsReject", true);
                    }
                    //保存审批人信息
                    fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", CurrentEmployee.UserAccount);
                    fields["Approvers"] = ReturnAllApprovers(CurrentEmployee.UserAccount);
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPListItemCollection spListItems = PaymentRequestComm.GetPaymentRequestInfo(fields["PONo"].AsString());
                        foreach (SPListItem spListItem in spListItems)
                        {
                            spListItem["Approvers"] = ReturnAllApproversSPByPR("Approvers", CurrentEmployee.UserAccount);
                            spListItem.Web.AllowUnsafeUpdates = true;
                            spListItem.Update();
                        }
                    });
                    break;
                case "ConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        context.UpdateWorkflowVariable("IsReject", false);
                        context.UpdateWorkflowVariable("IsPending", false);
                        context.UpdateWorkflowVariable("IsEnd", true);
                        fields["Status"] = CAWorkflowStatus.InProgress;
                        //if (PaymentRequestComm.IsLastTask(Request.QueryString["List"], Request.QueryString["ID"]))
                        //{   
                            var rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                            SPFieldUrlValue payLink = new SPFieldUrlValue();
                            payLink.Description = "Pay";
                            payLink.Url = rootweburl + "WorkFlowCenter/lists/PaymentRequest/NewForm.aspx?PONO=" + fields["PONo"];
                            fields["Pay"] = payLink;

                            SPFieldUrlValue historyLink = new SPFieldUrlValue();
                            historyLink.Description = "History";
                            historyLink.Url = rootweburl + "WorkFlowCenter/_layouts/ca/workflows/PaymentRequest/HistoryForm.aspx?PONO=" + fields["PONo"];
                            fields["History"] = historyLink;
                            fields["Status"] = CAWorkflowStatus.Completed;
                            //UpdateFANORecord
                            UpdateFANORecord();


                            if (
                                     (fields["RequestType"].ToString().ToLower() == "opex" && (bool)fields["IsFromPO"] == false
                               )
                            ||
                            (
                                    (bool)fields["IsFromPO"] == true && (bool)ViewState["isNeedStartSAPWF"] == true)
                            )
                            {
                                if (IsExistSAP(fields["SubPRNo"].ToString()))
                                {
                                    AddPaymentRequestSAPWorkFlow(fields);
                                }
                            }


                            fields["ReasonsResult"] = "0";
                            PaymentRequestComm.SetInstallmentInfo(fields["PONo"].ToString(), fields["PaidInd"].ToString(), new List<object[]>() { new object[] { "IsPaid", 1 } });
                            PaymentRequestComm.SetPaymentRequestInfo(fields["PONo"].ToString(), new List<object[]>() { new object[] { "Status", (IsCompeted(fields["PONo"].ToString()) ? 
                            PaymentRequestStatus.Completed : PaymentRequestStatus.NoCompleted) } });
                        //}
                    }
                    if (e.Action == "Pending")
                    {
                        //context.UpdateWorkflowVariable("ApproveTaskTitle", taskTitle + "needs approval");
                        context.UpdateWorkflowVariable("IsReject", false);
                        context.UpdateWorkflowVariable("IsPending", true);
                        context.UpdateWorkflowVariable("IsEnd", false);

                        //string groupName = ((bool)fields["IsFromPO"] == true ? PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview :
                        //                                                           PaymentRequestGroupNames.Opex_GeneralPO_SAPReview);
                        context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle);
                        string groupName = "";
                        if ((bool)fields["IsFromPO"] == true)
                        {
                            if (fields["RequestType"].ToString().ToLower() == "opex")
                            {
                                groupName = PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview;
                            }
                            else
                            {
                                groupName = PaymentRequestGroupNames.Capex_ConstructionPO_SAPReview;
                            }
                        }
                        else
                        {
                            if (fields["RequestType"].ToString().ToLower() == "opex")
                            {
                                groupName = PaymentRequestGroupNames.Opex_GeneralPO_SAPReview;
                            }
                            else
                            {
                                groupName = PaymentRequestGroupNames.Capex_GeneralPO_SAPReview;
                            }
                        }
                        context.UpdateWorkflowVariable("ConfirmTaskUsers", WorkFlowUtil.GetUsersInGroup(groupName));
                        fields["Status"] = CAWorkflowStatus.Pending;
                        this.PaymentRequestDataView.SavePendingForm();

                        SendEmail("Pending");
                    }
                    if (e.Action == "Reject")
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", fields["SubPRNo"].AsString() + " " + "Please resubmit Payment Request");
                        context.UpdateWorkflowVariable("IsReject", true);
                        context.UpdateWorkflowVariable("IsPending", false);
                        context.UpdateWorkflowVariable("IsEnd", false);

                        this.PaymentRequestDataView.SavePendingForm();

                        SendEmail("Reject");
                    }
                    //保存审批人信息
                    fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", CurrentEmployee.UserAccount);
                    fields["Approvers"] = ReturnAllApprovers(CurrentEmployee.UserAccount);
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPListItemCollection spListItems = PaymentRequestComm.GetPaymentRequestInfo(fields["PONo"].AsString());
                        foreach (SPListItem spListItem in spListItems)
                        {
                            spListItem["Approvers"] = ReturnAllApproversSPByPR("Approvers", CurrentEmployee.UserAccount);
                            spListItem.Web.AllowUnsafeUpdates = true;
                            spListItem.Update();
                        }
                    });
                    break;
                default:
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current, "SubPRNo");
        }


        private bool IsExistSAP(string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Payment Request SAP WorkFlow");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='PRWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems.Count > 0 ? false : true;
        }

        private double GetTotalAmountByExchangeRate(double amount, string currency)
        {
            if (currency=="RMB") 
            {
                return amount;
            }
            var exchRate = 1.0;
            var rateItem = ConvertToRMB(currency);
            if (rateItem != null)
            {
                exchRate = Convert.ToDouble(rateItem["Rate"].AsString());
            }
            return amount * exchRate;
        }

        private SPListItem ConvertToRMB(string from)
        {
            return GetExchangeRate(from, "RMB");
        }

        private static SPListItem GetExchangeRate(string from, string to)
        {
            var qFrom = new QueryField("From", false);
            var qTo = new QueryField("To", false);

            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qFrom.Equal(from));
            exp = WorkFlowUtil.LinkAnd(exp, qTo.Equal(to));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("ExchangeRates"))
                .Where(exp)
                .GetItems();

            return lc.Count > 0 ? lc[0] : null;
        }

        protected SPFieldUserValueCollection ReturnAllApproversSPByPR(string approverCol, params string[] accounts)
        {
            var web = SPContext.Current.Web;
            SPUser user = null;
            SPFieldUserValue spUser = null;
            SPListItemCollection spListItems = PaymentRequestComm.GetPaymentRequestInfo(WorkflowContext.Current.DataFields["PONo"].AsString());
            foreach (SPListItem spListItem in spListItems)
            {
                SPFieldUser userField = spListItem.Fields[approverCol] as SPFieldUser;

                SPFieldUserValueCollection approvers = userField.GetFieldValue(spListItem[approverCol].AsString()) as SPFieldUserValueCollection;
                foreach (var account in accounts)
                {
                    user = web.AllUsers[account];
                    spUser = new SPFieldUserValue(web, user.ID, user.Name);

                    if (approvers == null)
                    {
                        approvers = new SPFieldUserValueCollection();
                    }
                    if (!approvers.Contains(spUser))
                    {
                        approvers.Add(spUser);
                    }
                }
                
                return approvers;
            }
            return null;
        }

        /// <summary>
        /// 判断是否已经完成所有分期付款
        /// </summary>
        /// <param name="poID"></param>
        /// <returns></returns>
        private bool IsCompeted(string poID)
        {
            DataTable dTable = PaymentRequestComm.GetPaymentInstallmentInfo(poID).GetDataTable();
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow row in dTable.Rows)
                {
                    if (row["IsPaid"].ToString() == "0" || row["IsPaid"].ToString().IsNullOrWhitespace())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetString(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        private Hashtable GetExpenseTypeHashtable()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string et = "";
            if (fields["RequestType"].AsString().ToLower() == "opex")
            {
                et = "opex";
            }
            else
            {
                et = "capex";
            }
            DataTable expenseType = WorkFlowUtil.GetCollectionByList("Payment Request Expense Type").GetDataTable()
                                                 .AsEnumerable()
                                                 .Where(dr => dr.Field<string>("OpexCapexType").ToString().ToLower() == et)
                                                 .CopyToDataTable();
            Hashtable ht = new Hashtable();
            foreach (DataRow row in expenseType.Rows)
            {
                ht.Add(row["ExpenseType"].ToString(), row["GLAccount"].ToString());
            }
            return ht;
        }

        /// <summary>
        /// Start Payment Request SAP WorkFlow
        /// </summary>
        /// <param name="fields"></param>
        private void AddPaymentRequestSAPWorkFlow(WorkflowDataFields fields)
        {
            SPSite site = SPContext.Current.Site;

            SPList sAPList = CA.SharePoint.SharePointUtil.GetList("Payment Request SAP WorkFlow");
            SPList sAPItemsList = CA.SharePoint.SharePointUtil.GetList("Payment Request SAP Items WorkFlow");
            SPListItem sAPListItem = sAPList.Items.Add();

            string workFlowNumber = "PRSAP_" + WorkFlowUtil.CreateWorkFlowNumber("PaymentRequestSAPWorkFlow").ToString("000000");
            sAPListItem["WorkflowNumber"] = workFlowNumber;
            //sAPListItem["PreTotalAmount"] = Math.Round(double.Parse(fields["TotalAmount"].ToString()) * (double.Parse(fields["PaidThisTime"].ToString()) / 100), 2);
            sAPListItem["Applicant"] = fields["Applicant"].ToString();
            sAPListItem["VendorNo"] = fields["VendorNo"].AsString();
            sAPListItem["FromPOStatus"] = bool.Parse(fields["IsFromPO"].ToString()) == true ? "1" : "0";
            string name = fields["Applicant"].ToString();
            string useraccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
            sAPListItem["ApplicantSPUser"] = this.EnsureUser(useraccount);
            sAPListItem["Status"] = CAWorkflowStatus.InProgress;
            sAPListItem["PRWorkflowNumber"] = fields["SubPRNo"].AsString();
            sAPListItem["PostSAPStatus"] = "0";
            sAPListItem["SystemPONo"] = fields["SystemPONo"].AsString();
            sAPListItem["PaidThisTime"] = fields["PaidThisTime"].AsString();
            sAPListItem["VendorName"] = fields["VendorName"].AsString();
            sAPListItem["PONo"] = fields["PONo"].AsString();
            sAPListItem["PaymentDesc"] = fields["PaymentDesc"].AsString();
            sAPListItem["RequestType"] = fields["RequestType"].AsString();

            sAPListItem["VendorCity"] = fields["VendorCity"].AsString();
            sAPListItem["VendorCountry"] = fields["VendorCountry"].AsString();
            sAPListItem["BankCity"] = fields["BankCity"].AsString();
            sAPListItem["SwiftCode"] = fields["SwiftCode"].AsString();//BankAccount
            sAPListItem["BankAccount"] = fields["BankAccount"].AsString();
            sAPListItem["TaxPrice"] = fields["TaxPrice"].AsString();
            sAPListItem["BankName"] = fields["BankName"].AsString();

            sAPListItem["ExchRate"] = fields["ExchRate"].AsString();
            sAPListItem["Currency"] = fields["Currency"].AsString();

            NameCollection acAccounts = null;
            NameCollection financeConfirmAccounts = null;
            if (bool.Parse(fields["IsFromPO"].ToString())==true)
            {
                if (fields["RequestType"].ToString().ToLower() == "opex")
                {
                    acAccounts = WorkFlowUtil.GetUsersInGroup(PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview);
                    financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(PaymentRequestGroupNames.Opex_ConstructionPO_SAPConfirm);
                }
                else
                {
                    acAccounts = WorkFlowUtil.GetUsersInGroup(PaymentRequestGroupNames.Capex_ConstructionPO_SAPReview);
                    financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(PaymentRequestGroupNames.Capex_ConstructionPO_SAPConfirm);
                }
            }
            else
            {
                if (fields["RequestType"].ToString().ToLower() == "opex")
                {
                    acAccounts = WorkFlowUtil.GetUsersInGroup(PaymentRequestGroupNames.Opex_GeneralPO_SAPReview);
                    financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(PaymentRequestGroupNames.Opex_GeneralPO_SAPConfirm);
                }
            }

            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", "ACReviewTask", acAccounts.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", "FinanceConfirmTask", financeConfirmAccounts.JoinString(","));
            sAPListItem["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            Hashtable ht = GetExpenseTypeHashtable();

            DataTable itemDetails = PaymentRequestComm.GetDataTable(fields["SubPRNo"].AsString());
            double totalAmount = 0;
            foreach (DataRow dr in itemDetails.Rows)
            {
                SPListItem sAPItemsListItem = sAPItemsList.Items.Add();
                sAPItemsListItem["WorkflowNumber"] = sAPListItem["WorkflowNumber"].ToString();
                sAPItemsListItem["PRWorkflowNumber"] = sAPListItem["PRWorkflowNumber"].ToString();
                sAPItemsListItem["ExpenseType"] = dr["ExpenseType"].ToString();
                sAPItemsListItem["AssetNo"] = dr["FANO"].AsString();
                sAPItemsListItem["BusinessArea"] = "";
                //sAPItemsListItem["ItemAmount"] = Math.Round(double.Parse(dr["ItemAmount"].ToString()) * (double.Parse(dr["ItemAmount"].ToString()) / 100), 2);

                if (dr["ItemInstallmentAmount"].AsString() == "")
                {
                    if ((bool)fields["IsFromPO"] == false && GetString(fields["PaidInd"]).IsNotNullOrWhitespace() == true
                            && int.Parse(GetString(fields["PaidInd"])) == 1)
                    {
                        sAPItemsListItem["ItemAmount"] = Math.Round(double.Parse(dr["ItemAmount"].ToString()) * (double.Parse(fields["PaidThisTime"].ToString()) / 100), 2); ;

                    }
                    else
                    {
                        sAPItemsListItem["ItemAmount"] = Math.Round(double.Parse(dr["ItemAmount"].ToString()), 2);
                    }
                }
                else 
                {
                    sAPItemsListItem["ItemAmount"] = Math.Round(double.Parse(dr["ItemInstallmentAmount"].ToString()), 2);
                }


                sAPItemsListItem["CostCenter"] = dr["CostCenter"].AsString();
                sAPItemsListItem["Status"] = "0";
                sAPItemsListItem["GLAccount"] = ht[dr["ExpenseType"].ToString()] != null ? ht[dr["ExpenseType"].ToString()].ToString() : "";
                sAPItemsListItem.Web.AllowUnsafeUpdates = true;
                sAPItemsListItem.Update();

                if (dr["ItemInstallmentAmount"].AsString() == "")
                {
                    if ((bool)fields["IsFromPO"] == false && GetString(fields["PaidInd"]).IsNotNullOrWhitespace() == true
                            && int.Parse(GetString(fields["PaidInd"])) == 1)
                    {
                        totalAmount += Math.Round(double.Parse(dr["ItemAmount"].ToString()) * (double.Parse(fields["PaidThisTime"].ToString()) / 100), 2);
                    }
                    else
                    {
                        totalAmount += Math.Round(double.Parse(dr["ItemAmount"].ToString()), 2);
                    }
                }
                else
                {
                    totalAmount += Math.Round(double.Parse(dr["ItemInstallmentAmount"].ToString()), 2);
                }
            }
            sAPListItem["PreTotalAmount"] = Math.Round(totalAmount, 2);
            //sAPListItem["PreTotalAmount"] = Math.Round(totalAmount * (double.Parse(fields["PaidThisTime"].ToString()) / 100), 2); ;

            sAPListItem.Web.AllowUnsafeUpdates = true;
            sAPListItem.Update();

            WorkflowVariableValues vs = new WorkflowVariableValues();
            vs["ACReviewUsers"] = GetDelemanNameCollection(acAccounts, "129");
            vs["FinanceConfirmUsers"] = GetDelemanNameCollection(financeConfirmAccounts, "129");
            var aCReviewTaskFormUrl = "/_Layouts/CA/WorkFlows/PaymentRequestSAP/ACReview.aspx";
            var financeConfirmTaskFormUrl = "/_Layouts/CA/WorkFlows/PaymentRequestSAP/FinanceConfirm.aspx";
            vs["ACReviewTaskFormUrl"] = aCReviewTaskFormUrl;
            vs["FinanceConfirmTaskFormUrl"] = financeConfirmTaskFormUrl;
            string taskTitle = fields["SubPRNo"].AsString() + " " + PaymentRequestDataView.VendorNameText + " " + PaymentRequestDataView.ApproveAmount + " " + fields["Applicant"].AsString() + "'s Payment Request ";
            vs["ACReviewTitle"] = taskTitle;
            vs["FinanceConfirmTitle"] = taskTitle ;
            var eventData = SerializeUtil.Serialize(vs);
            var wfName = "PaymentRequestSAPWorkFlow";
            var wfAss = sAPList.WorkflowAssociations.GetAssociationByName(wfName, System.Globalization.CultureInfo.CurrentCulture);
            site.WorkflowManager.StartWorkflow(sAPListItem, wfAss, eventData);
            WorkFlowUtil.UpdateWorkflowPath(sAPListItem, eventData);
        }

        private SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            });
            return user;
        }

        private void UpdateFANORecord()
        {
            List<string> hfFAList = this.PaymentRequestDataView.FANO.Split(';').ToList<string>();
            hfFAList.Remove("");
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("FANO");
            foreach (string str in hfFAList)
            {
                DataRow dr = dt.Rows.Add();
                dr["ID"] = str.Split(',')[0];
                dr["FANO"] = str.Split(',')[1];
            }
            UpdateFANORecord(dt);
        }

        static void UpdateFANORecord(DataTable dt)
        {
            SPList list = WorkFlowUtil.GetWorkflowList("PaymentRequestItemDetails");
            string listGuid = list.ID.ToString();
             StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat = "<Method ID=\"{0}\">" +
                           "<SetList Scope=\"Request\">{1}</SetList>" +
                           "<SetVar Name=\"ID\">{2}</SetVar>" +
                           "<SetVar Name=\"Cmd\">Save</SetVar>" +
                           "<SetVar Name=\"urn:schemas-microsoft-com:office:office#FANO\">{3}</SetVar>" +
                           "</Method>";
            foreach (DataRow dr in dt.Rows)
            {
                methodBuilder.AppendFormat(methodFormat, 0, listGuid, dr["ID"].ToString(),
                    dr["FANO"].ToString()
                    );
            }
            if (methodBuilder.ToString().Length > 0)
            {
                batch = string.Format(batchFormat, methodBuilder.ToString());
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
                
            }

        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                string opex_Capex_Status = this.PaymentRequestDataView.Opex_Capex_Status;
                var templateTitle = "";
                if (opex_Capex_Status == "1")
                {
                    templateTitle = "PaymentRequestPendingOrRejectCapex";
                }
                else
                {
                    templateTitle = "PaymentRequestPendingOrRejectOpex";
                }
                
                var applicant = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(applicant.Substring(applicant.IndexOf('(') + 1, applicant.IndexOf(')') - applicant.IndexOf('(') - 1));
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/PaymentRequest/DisplayForm.aspx?List="
                                                 + Request.QueryString["List"]
                                                 + "&ID=" + Request.QueryString["ID"];

                List<string> parameters = new List<string>();
                List<string> to = new List<string>();
                to.Add(applicantAccount);

                switch (emailType)
                {
                    case "Reject":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["SubPRNo"].ToString());
                        parameters.Add(emailType == "Pending" ? "pending" : "rejected");
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Pending":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["SubPRNo"].ToString());
                        parameters.Add(emailType == "Pending" ? "pending" : "rejected");
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                   default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.logError(string.Format("Payment Request Send Email Error：\r\n\r\n{0}\r\n\r\n", ex.Message));
            }
        }

    }
}
