using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using Microsoft.SharePoint;
using QuickFlow;
using QuickFlow.UI.Controls;
using CA.SharePoint;
using System.Text.RegularExpressions;
using System.ComponentModel;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (!IsPostBack)
            {
                Response.Expires = 0;
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.AddHeader("pragma", "no-cache");
                Response.CacheControl = "no-cache";
               
                this.TaskTrace1.Applicant = fields["Applicant"].ToString();
                this.DataForm1.DataDataFields(fields);
            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.Actions.OnClientClick = "return dispatchAction(this);";
            this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].ToString();
            this.DataForm1.Step = WorkflowContext.Current.Step;
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
            }


            this.btnNotice.Click += this.btnNotice_Click;
           // this.btnPending.Click += this.btnPending_Click;
        }

        //private void btnPending_Click(object sender, EventArgs e)
        //{
        //    WorkflowContext context = WorkflowContext.Current;
        //    //WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            
        //    SPListItem curItem = SPContext.Current.ListItem;
        //    curItem.Web.AllowUnsafeUpdates = true;
        //    this.DataForm1.SavePendingForm(curItem);
        //    curItem["Status"] = CAWorkflowStatus.Pending;
        //    curItem.Update();
        //    curItem.Web.AllowUnsafeUpdates = false;
        //    NameCollection wfCFO1 = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm_CreditCard");
        //    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimConfirmTaskUsers, GetDelemanNameCollection(wfCFO1, Constants.CAModules.CreditCardClaim));
        //    RedirectToTask();
        //}

        private void btnNotice_Click(object sender, EventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            SPListItem curItem = SPContext.Current.ListItem;
            curItem.Web.AllowUnsafeUpdates = true;
            curItem["NoticeResult"] = "1";
            curItem.Update();
            curItem.Web.AllowUnsafeUpdates = false;
           // this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Notice Success!'); </script>");
            btnNotice.Enabled = false;
            //SendEmail("Notice");
            SendEmail("Approve");
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
                        var levelType = "Payment Approval Limits";
                        string totalnum = fields["ApproveAmount"].ToString();
                        double total = Convert.ToDouble(totalnum);
                        var quota = WorkFlowUtil.GetQuota(fields["CurrManager"].ToString(), levelType);
                        if (total > quota)
                        {
                            #region Set users for workflow
                            //Modify task users
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
                            NameCollection manager = new NameCollection();
                            manager.Add(managerEmp.UserAccount);
                            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimNextApproveTaskUsers, GetDelemanNameCollection(manager, Constants.CAModules.CreditCardClaim));
                            #endregion

                            fields["CurrManager"] = managerEmp.UserAccount;


                            //context.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                            context.UpdateWorkflowVariable("IsContinue", true);
                        }
                        else
                        {
                            NameCollection wfCFO = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm_CreditCard");
                            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimConfirmTaskUsers, GetDelemanNameCollection(wfCFO, Constants.CAModules.CreditCardClaim));
                            context.UpdateWorkflowVariable("IsContinue", false);
                            //fields["NoticeResult"] = "2";
                        }
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Credit Card Claim");
                        context.UpdateWorkflowVariable("IsContinue", false);
                        SendEmail("Reject");
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimApproversPerson, CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimClaimApprovers);
                    break;
                case "ConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        fields["Status"] = CAWorkflowStatus.Completed;
                        fields["ReasonsResult"] = "0";
                        if (fields["RMBSummaryExpenseType"].ToString().Length > 2 || fields["USDSummaryExpenseType"].ToString().Length > 2)
                        {
                            if (IsExistSAP(fields["WorkflowNumber"].ToString()))
                            {
                                AddExpenseClaimSAPWorkFlow(fields);
                            }
                        }
                        //SendEmail("Approve");
                    }
                    if (e.Action == "Reject") 
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        this.DataForm1.SavePendingForm();
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Credit Card Claim");
                        SendEmail("Reject");
                    }
                    if (e.Action == "Pending") 
                    {
                        //if (fields["NoticeResult"].ToString()=="2")
                        //{
                        //    fields["NoticeResult"] = "0";
                        //}
                        fields["Status"] = CAWorkflowStatus.Pending;
                        this.DataForm1.SavePendingForm();
                        NameCollection wfCFO1 = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm_CreditCard");
                        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimConfirmTaskUsers, GetDelemanNameCollection(wfCFO1, Constants.CAModules.CreditCardClaim));
                        SendEmail("Pending");
                    }
                   
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimApproversPerson, CA.WorkFlow.UI.Constants.WorkFlowStep.CreditCardClaimClaimApprovers);
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);

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
        private bool IsExistSAP(string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim SAP Workflow");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='CCCWWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems.Count > 0 ? false : true;
        }
        public void AddExpenseClaimSAPWorkFlow(WorkflowDataFields fields)
        {
            SPSite site = SPContext.Current.Site;

            SPList sAPList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim SAP Workflow");
            SPList sAPItemsList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim SAP Detail");
            SPListItem sAPListItem = sAPList.Items.Add();
            sAPListItem["WorkflowNumber"] = CreateWorkflowNumber();
            //sAPListItem["CashAdvanceAmount"] = fields["CashAdvance"].ToString();
            sAPListItem["TotalAmount"] = fields["PreTotalAmount"].ToString();
            //sAPListItem["PreTotalAmount"] = fields["AmountDue"].ToString();
            sAPListItem["Applicant"] = fields["Applicant"].ToString();
            sAPListItem["ExpenseDescription"] = fields["ExpenseDescription"].ToString();

            string name = fields["Applicant"].ToString();
            string useraccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
            sAPListItem["ApplicantSPUser"] = this.EnsureUser(useraccount);

            sAPListItem["Status"] = CAWorkflowStatus.InProgress;
            sAPListItem["CCCWWorkflowNumber"] = fields["WorkflowNumber"].ToString();

            NameCollection acAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.wf_FinanceConfirm_CreditCard);
            NameCollection financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_FinanceManager_CreditCard);
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", "ACReviewTask", acAccounts.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", "FinanceConfirmTask", financeConfirmAccounts.JoinString(","));
            sAPListItem["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            sAPListItem["PostSAPStatus"] = "0";

            string postSAPType = "";
            if (fields["RMBSummaryExpenseType"].ToString().Length > 2)
            {
                postSAPType += "RMB;";
            }
            if (fields["USDSummaryExpenseType"].ToString().Length > 2)
            {
                postSAPType += "USD;";
            }
            sAPListItem["PostSAPType"] = postSAPType;

            sAPListItem.Web.AllowUnsafeUpdates = true;
            sAPListItem.Update();
            string regexText = @"\{[^\{-\}]*\}";
            Regex regex = new Regex(regexText);
            if (fields["RMBSummaryExpenseType"].ToString().Length > 2)
            {
                MatchCollection mcRMB = regex.Matches(fields["RMBSummaryExpenseType"].ToString());
                foreach (Match m in mcRMB)
                {
                    List<string> itemList = m.Value.Replace("{", "").Replace("}", "").Split(',').ToList<string>();
                    SPListItem sAPItemsListItem = sAPItemsList.Items.Add();
                    sAPItemsListItem["WorkflowNumber"] = sAPListItem["WorkflowNumber"].ToString();
                    sAPItemsListItem["ExpenseType"] = itemList[0].Replace("name:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["DealAmount"] = itemList[1].Replace("val:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["CostCenter"] = itemList[2].Replace("costcenter:'", "").Replace("'", "").Trim();

                    sAPItemsListItem["DepositAmount"] = itemList[3].Replace("depositamt:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["PayAmount"] = itemList[4].Replace("payamt:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["TransactionDescription"] = itemList[5].Replace("transdesc:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["CreditCardBillID"] = itemList[6].Replace("creditCardBillID:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["AmountType"] = "RMB";

                    sAPItemsListItem["Status"] = "0";
                    sAPItemsListItem["ErrorMsg"] = "";
                    sAPItemsListItem.Web.AllowUnsafeUpdates = true;
                    sAPItemsListItem.Update();
                }
            }
            if (fields["USDSummaryExpenseType"].ToString().Length > 2)
            {
                MatchCollection mcUSD = regex.Matches(fields["USDSummaryExpenseType"].ToString());
                foreach (Match m in mcUSD)
                {
                    List<string> itemList = m.Value.Replace("{", "").Replace("}", "").Split(',').ToList<string>();
                    SPListItem sAPItemsListItem = sAPItemsList.Items.Add();
                    sAPItemsListItem["WorkflowNumber"] = sAPListItem["WorkflowNumber"].ToString();
                    sAPItemsListItem["ExpenseType"] = itemList[0].Replace("name:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["DealAmount"] = itemList[1].Replace("val:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["CostCenter"] = itemList[2].Replace("costcenter:'", "").Replace("'", "").Trim();

                    sAPItemsListItem["DepositAmount"] = itemList[3].Replace("depositamt:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["PayAmount"] = itemList[4].Replace("payamt:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["TransactionDescription"] = itemList[5].Replace("transdesc:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["CreditCardBillID"] = itemList[6].Replace("creditCardBillID:'", "").Replace("'", "").Trim();
                    sAPItemsListItem["AmountType"] = "USD";

                    sAPItemsListItem["Status"] = "0";
                    sAPItemsListItem["ErrorMsg"] = "";
                    sAPItemsListItem.Web.AllowUnsafeUpdates = true;
                    sAPItemsListItem.Update();
                }
            }

            WorkflowVariableValues vs = new WorkflowVariableValues();
            vs["ACReviewUsers"] = GetDelemanNameCollection(acAccounts, Constants.CAModules.CreditCardClaimSAP);
            vs["FinanceConfirmUsers"] = GetDelemanNameCollection(financeConfirmAccounts, Constants.CAModules.CreditCardClaimSAP);
            var aCReviewTaskFormUrl = "/_Layouts/CA/WorkFlows/CreditCardClaim/ACReview.aspx";
            var financeConfirmTaskFormUrl = "/_Layouts/CA/WorkFlows/CreditCardClaim/FinanceConfirm.aspx";
            vs["ACReviewTaskFormUrl"] = aCReviewTaskFormUrl;
            vs["FinanceConfirmTaskFormUrl"] = financeConfirmTaskFormUrl;
            vs["ACReviewTitle"] = fields["Applicant"].ToString() + "'s Credit Card Claim";
            vs["FinanceConfirmTitle"] = fields["Applicant"].ToString() + "'s Credit Card Claim";
            var eventData = SerializeUtil.Serialize(vs);
            var wfName = "CreditCardClaimSAP";
            var wfAss = sAPList.WorkflowAssociations.GetAssociationByName(wfName, System.Globalization.CultureInfo.CurrentCulture);
            site.WorkflowManager.StartWorkflow(sAPListItem, wfAss, eventData);
            WorkFlowUtil.UpdateWorkflowPath(sAPListItem, eventData);
        }

        private static string CreateWorkflowNumber()
        {
            return "CCCSAP_" + WorkFlowUtil.CreateWorkFlowNumber("CreditCardClaimSAPWorkflow").ToString("000000");
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "CreditCardClaim" + emailType;

                var applicant = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(applicant.Substring(applicant.IndexOf('(') + 1, applicant.IndexOf(')') - applicant.IndexOf('(') - 1));
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/CreditCardClaim/DisplayForm.aspx?List="
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