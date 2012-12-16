namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using System.Collections.Generic;
    using CA.SharePoint;
   using Microsoft.SharePoint;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.ComponentModel;
    using System.Data;
    using System.Collections;
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                Response.Expires = 0;
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.AddHeader("pragma", "no-cache");
                Response.CacheControl = "no-cache";
            }

            this.DataForm1.Mode = "Approve";

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["RequestedBy"].AsString();
            if (!this.Page.IsPostBack)
            {
                this.DataForm1.SummaryExpenseType = WorkflowContext.Current.DataFields["SummaryExpenseType"].AsString();
            }
            this.Actions.OnClientClick = "return dispatchAction(this);";
            if (WorkflowContext.Current.Step == "ConfirmTask")
            {
                this.DataForm1.SetSpecialApproveResult("1");
            }
            this.DataForm1.Step = WorkflowContext.Current.Step;
            this.btnNotice.Click += this.btnNotice_Click;
            if (WorkflowContext.Current.Step == "ConfirmTask")
            {
                //btnPending.Visible = true;
                btnNotice.Visible = true;
                if (WorkflowContext.Current.DataFields["ReasonsResult"] != null)
                {
                    if (WorkflowContext.Current.DataFields["ReasonsResult"].ToString() == "1")
                    {
                        //if (WorkflowContext.Current.DataFields["NoticeResult"] != null)
                        //{
                        //    if (WorkflowContext.Current.DataFields["NoticeResult"].ToString() != "2")
                        //    {
                                this.DataForm1.Pending = "1";
                        //    }
                        //}
                    }
                }
                if (WorkflowContext.Current.DataFields["NoticeResult"] != null)
                {
                    if (WorkflowContext.Current.DataFields["NoticeResult"].ToString() == "1")
                    {
                        btnNotice.Enabled = false;
                    }
                }
            }
            else
            {
                btnNotice.Visible = false;
                //btnPending.Visible = false;
            }
            

            //this.btnNotice.Click += this.btnNotice_Click;
            //this.btnPending.Click += this.btnPending_Click;
        }
        //private void btnPending_Click(object sender, EventArgs e)
        //{
        //    WorkflowContext context = WorkflowContext.Current;
        //    WorkflowDataFields fields = WorkflowContext.Current.DataFields;
        //    fields["Status"] = CAWorkflowStatus.Pending;
        //    SPListItem curItem = SPContext.Current.ListItem;
        //    curItem.Web.AllowUnsafeUpdates = true;
        //    this.DataForm1.SavePendingForm(curItem);
        //    curItem.Web.AllowUnsafeUpdates = false;
        //    context.UpdateWorkflowVariable("ConfirmTaskUsers", EmployeeExpenseClaimCommon.GetTaskUsers("wf_FinanceConfirm"));
        //    RedirectToTask();
        //}

        private void btnNotice_Click(object sender, EventArgs e)
        {
            //CommonUtil.logInfo("SendEmail!");
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            SPListItem curItem = SPContext.Current.ListItem;
            curItem.Web.AllowUnsafeUpdates = true;
            curItem["NoticeResult"] = "1";
            curItem.Update();
            curItem.Web.AllowUnsafeUpdates = false;
           // this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Notice Success!');</script>");
            btnNotice.Enabled = false;
            //SendEmail("Notice");
            if (IsLimitApprove(fields["WorkflowNumber"].AsString()))
            {
                this.SendEmail("LimitApprove");
            }
            else
            {
                this.SendEmail("Approve");
            }

            this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">window.location = window.location;</script>");
        }
        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            switch (WorkflowContext.Current.Step)
            {
                case "NextApproveTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //Save SpecialApprove Info
                        this.DataForm1.SetSpecialApprove(); //将已拒绝的列表项的special approve置为false
                        fields["TotalAmount"] =Math.Round(this.DataForm1.TotalAmount,2).ToString();
                        fields["AmountDue"] = Math.Round(this.DataForm1.TotalAmount - Convert.ToDouble(fields["CashAdvance"].ToString()), 2).ToString(); 

                        var levelType = "Payment Approval Limits";
                        double total = Convert.ToDouble(fields["TotalAmount"].ToString());
                        var quota = WorkFlowUtil.GetQuota(fields["CurrManager"].ToString(), levelType);
                        if (total > quota)
                        {
                            #region Set users for workflow
                            //Modify task users
                            var manager = new NameCollection();

                            var managerEmp = WorkFlowUtil.GetNextApprover(fields["CurrManager"].ToString());

                            //if it's CEO
                            if (managerEmp == null)
                            {
                                List<string> ceos = WorkflowPerson.GetCEO();
                                if (ceos.Count == 0)
                                {
                                    DisplayMessage("The init error about WorkflowPerson in the system.");
                                    e.Cancel = true;
                                    return;
                                }
                                managerEmp = UserProfileUtil.GetEmployeeEx(ceos[0]);
                            }

                            //Get Task users include deleman
                            EmployeeExpenseClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);
                            #endregion

                            fields["CurrManager"] = managerEmp.UserAccount;
                            fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());

                            context.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                            context.UpdateWorkflowVariable("IsContinue", true);
                        }
                        else
                        {
                            this.DataForm1.SetApprovedSpecialApprove(); //将已批准的列表项的special approve置为false
                            context.UpdateWorkflowVariable("ConfirmTaskUsers", EmployeeExpenseClaimCommon.GetTaskUsers("wf_FinanceConfirm"));
                            context.UpdateWorkflowVariable("IsContinue", false);
                            //fields["NoticeResult"] = "2";
                        }
                        string set = this.DataForm1.SummaryExpenseType.Trim();
                        if (set.Length<5) 
                        {
                            throw new Exception("系统出错，请重试，或者联系管理员！");
                        }
                        fields["SummaryExpenseType"] = this.DataForm1.SummaryExpenseType;
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        fields["Status"] = CAWorkflowStatus.Rejected;

                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Employee Expense Claim");
                        context.UpdateWorkflowVariable("IsContinue", false);
                        SendEmail("Reject");
                        //this.SendEmail("Reject");
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimApproversPerson, CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimApprovers);
                    break;
                case "ConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        fields["Status"] = CAWorkflowStatus.Completed;
                        //if (this.DataForm1.Hidtas >= this.DataForm1.Hidcas && this.DataForm1.Hidcas != 0)
                        //{
                        if (fields["CashAdvanceWorkFlowNumber"] != null)
                        {
                            string workflowNumber = fields["CashAdvanceWorkFlowNumber"].ToString();
                            List<string> list = workflowNumber.Split(';').ToList<string>();
                            if (list.Count > 0)
                            {
                                foreach (string number in list)
                                {
                                    if (number != "")
                                    {
                                        UpdateCashAdvanceStatus(number);
                                    }
                                }
                            }
                        }
                        fields["ReasonsResult"] = "0";

                        if (IsExistSAP(fields["WorkflowNumber"].ToString()))
                        {
                            AddEmployeeExpenseClaimSAPWorkFlow(fields);
                        }
                        //if (IsLimitApprove(fields["WorkflowNumber"].AsString()))
                        //{
                        //    this.SendEmail("LimitApprove");
                        //}
                        //else
                        //{
                        //    this.SendEmail("Approve");
                        //}
                    }

                    if (e.Action == "Reject") 
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        this.DataForm1.SavePendingForm();
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Employee Expense Claim");
                        SendEmail("Reject");
                    }
                    if (e.Action == "Pending")
                    {
                        //if (fields["NoticeResult"].ToString() == "2")
                        //{
                        //    fields["NoticeResult"] = "0";
                        //}
                        fields["Status"] = CAWorkflowStatus.Pending;
                        this.DataForm1.SavePendingForm();

                        context.UpdateWorkflowVariable("ConfirmTaskUsers", EmployeeExpenseClaimCommon.GetTaskUsers("wf_FinanceConfirm"));
                        SendEmail("Pending");
                    }
                   
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimApproversPerson, CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimApprovers);
                    break;
                default:
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private bool IsLimitApprove(string workflowNumber)
        {
            SPListItemCollection tcDetailsItems = EmployeeExpenseClaimCommon.GetDataCollection(workflowNumber, "EmployeeExpenseClaimItems");
            foreach (SPListItem item in tcDetailsItems)
            {
                if (Convert.ToDouble(item["OriginalAmount"].ToString()) != Convert.ToDouble(item["Amount"].ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsExistSAP(string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Employee Expense Claim SAP WorkFlow");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='EECWWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems.Count > 0 ? false : true;
        }
        public void AddEmployeeExpenseClaimSAPWorkFlow(WorkflowDataFields fields)
        {
            SPSite site = SPContext.Current.Site;

            SPList sAPList = CA.SharePoint.SharePointUtil.GetList("Employee Expense Claim SAP WorkFlow");
            SPList sAPItemsList = CA.SharePoint.SharePointUtil.GetList("EmployeeExpenseClaimSAPItemsWorkFlow");
            SPListItem sAPListItem = sAPList.Items.Add();
            sAPListItem["WorkflowNumber"] = CreateWorkflowNumber();
            sAPListItem["CashAdvanceAmount"] = fields["CashAdvance"] == null ? "" : fields["CashAdvance"].ToString();
            sAPListItem["TotalAmount"] =Math.Round(double.Parse(fields["TotalAmount"].ToString()),2).ToString();
            sAPListItem["PreTotalAmount"] = Math.Round(double.Parse(fields["AmountDue"].ToString()), 2).ToString(); 
            sAPListItem["Applicant"] = fields["RequestedBy"].ToString();
            sAPListItem["ExpenseDescription"] = fields["ExpenseDescription"].ToString();

            string name = fields["RequestedBy"].ToString();
            string useraccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
            sAPListItem["ApplicantSPUser"] = this.EnsureUser(useraccount);
            
            sAPListItem["Status"] = CAWorkflowStatus.InProgress;
            sAPListItem["EECWWorkflowNumber"] = fields["WorkflowNumber"].ToString();
            sAPListItem["PostSAPStatus"] = "0";
            sAPListItem["CashAdvanceWorkFlowNumber"] = fields["CashAdvanceIDAndAmount"] == null ? "" : fields["CashAdvanceIDAndAmount"].ToString();


            NameCollection acAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_Accountants);
            NameCollection financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_FinanceManager);
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", "ACReviewTask", acAccounts.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", "FinanceConfirmTask", financeConfirmAccounts.JoinString(","));
            sAPListItem["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();
            
            sAPListItem.Web.AllowUnsafeUpdates = true;
            sAPListItem.Update();
            string regexText = @"\{[^\{-\}]*\}";
            Regex regex = new Regex(regexText);
            string set = fields["SummaryExpenseType"].ToString().Trim();
            if (set.Length < 5)
            {
                throw new Exception("系统出错，请重试，或者联系管理员！");
            }

            DataTable dt = WorkFlowUtil.GetCollectionByList("Employee Expense Claim Type").GetDataTable();
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["NewExpenseType"].ToString(), dr["ExpenseType"].ToString());
            }

            MatchCollection mc = regex.Matches(fields["SummaryExpenseType"].ToString());
            foreach (Match m in mc)
            {
                List<string> itemList = m.Value.Replace("{", "").Replace("}", "").Split(',').ToList<string>();
                SPListItem sAPItemsListItem = sAPItemsList.Items.Add();
                sAPItemsListItem["WorkflowNumber"] = sAPListItem["WorkflowNumber"].ToString();
                string et = itemList[0].Replace("name:'", "").Replace("'", "").Trim();
                if (ht[et] != null)
                {
                    et = ht[et].ToString();
                }
                sAPItemsListItem["ExpenseType"] = et;
                sAPItemsListItem["ItemAmount"] = itemList[1].Replace("val:'", "").Replace("'", "").Trim();
                sAPItemsListItem["CostCenter"] = itemList[2].Replace("costcenter:'", "").Replace("'", "").Trim();
                sAPItemsListItem["Status"] = "0";
                sAPItemsListItem["ErrorMsg"] = "";
                sAPItemsListItem.Web.AllowUnsafeUpdates = true;
                sAPItemsListItem.Update();
            }

            WorkflowVariableValues vs = new WorkflowVariableValues();
            vs["ACReviewUsers"] = GetDelemanNameCollection(acAccounts, Constants.CAModules.EmployeeExpenseClaimSAP);
            vs["FinanceConfirmUsers"] = GetDelemanNameCollection(financeConfirmAccounts, Constants.CAModules.EmployeeExpenseClaimSAP);
            var aCReviewTaskFormUrl = "/_Layouts/CA/WorkFlows/EmployeeExpenseClaim2/ACReview.aspx";
            var financeConfirmTaskFormUrl = "/_Layouts/CA/WorkFlows/EmployeeExpenseClaim2/FinanceConfirm.aspx";
            vs["ACReviewTaskFormUrl"] = aCReviewTaskFormUrl;
            vs["FinanceConfirmTaskFormUrl"] = financeConfirmTaskFormUrl;
            vs["ACReviewTitle"] = fields["RequestedBy"].ToString() + "'s Employee Expense Claim";
            vs["FinanceConfirmTitle"] = fields["RequestedBy"].ToString() + "'s Employee Expense Claim";
            var eventData = SerializeUtil.Serialize(vs);
            var wfName = "Employee Expense Claim SAP WorkFlow";
            var wfAss = sAPList.WorkflowAssociations.GetAssociationByName(wfName, System.Globalization.CultureInfo.CurrentCulture);
            site.WorkflowManager.StartWorkflow(sAPListItem, wfAss, eventData);
            WorkFlowUtil.UpdateWorkflowPath(sAPListItem, eventData);
        }
        public SPUser EnsureUser(string strUser)
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

        private static string CreateWorkflowNumber()
        {
            return "EECSAP_" + WorkFlowUtil.CreateWorkFlowNumber("EmployeeExpenseClaimSAPWorkFlow").ToString("000000");
        }

        private void UpdateCashAdvanceStatus(string workFlowNumber) 
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequest");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            if (null != listItems && listItems.Count == 1)
            {
                var tmp = listItems[0];
                tmp["CashAdvanceStatus"] = "1";
                tmp.Web.AllowUnsafeUpdates = true;
                tmp.Update();
                tmp.Web.AllowUnsafeUpdates = false;
            }
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private bool Validate(string action, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {
                DisplayMessage(this.DataForm1.MSG);
                e.Cancel = true;
                flag = false;
            }
            return flag;
        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "EmployeeExpenseClaim" + emailType;

                var applicant = fields["RequestedBy"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(applicant.Substring(applicant.IndexOf('(') + 1, applicant.IndexOf(')') - applicant.IndexOf('(') - 1));
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/EmployeeExpenseClaim2/DisplayForm.aspx?List="
                                                 + Request.QueryString["List"]
                                                 + "&ID=" + Request.QueryString["ID"];

                List<string> parameters = new List<string>();
                List<string> to = new List<string>();
                to.Add(applicantAccount);

                switch (emailType)
                {
                    case "Approve":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Reject":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(CurrentEmployee.DisplayName);
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Pending":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(CurrentEmployee.DisplayName);
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Notice":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "LimitApprove":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].ToString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Submit":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].AsString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}