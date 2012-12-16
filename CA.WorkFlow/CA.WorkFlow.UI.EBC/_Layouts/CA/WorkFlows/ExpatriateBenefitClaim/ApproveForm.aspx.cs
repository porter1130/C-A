using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Collections.Generic;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace CA.WorkFlow.UI.EBC
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Response.Expires = 0;
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.AddHeader("pragma", "no-cache");
                Response.CacheControl = "no-cache";

                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                DataForm.SummaryExpenseType = fields["SummaryExpenseType"].AsString();
                DataForm.ExpatriateBenefitForm = fields["ExpatriateBenefitForm"].AsString();
                this.TaskTrace.Applicant = fields["Applicant"].ToString();
                this.DataForm.Step = WorkflowContext.Current.Step;
            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            this.Actions.OnClientClick = "return dispatchAction(this);";
            this.btnNotice.Click += this.btnNotice_Click;
            if (WorkflowContext.Current.Step == "ConfirmTask")
            {
                btnNotice.Visible = true;
                if (WorkflowContext.Current.DataFields["NoticeResult"].AsString() == "1")
                {
                    btnNotice.Enabled = false;
                }
                if (WorkflowContext.Current.DataFields["ReasonsResult"].AsString() == "1")
                {
                    this.DataForm.Pending = "Pending";
                }
            }
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            SPListItem curItem = SPContext.Current.ListItem;
            curItem.Web.AllowUnsafeUpdates = true;
            curItem["NoticeResult"] = "1";
            curItem.Update();
            curItem.Web.AllowUnsafeUpdates = false;
            this.SendEmail("Approve");
            Response.Write("<script type=\"text/javascript\">window.location = window.location;</script>");
            Response.End();
        }

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "ExpatriateBenefitClaim" + emailType;

                var applicant = fields["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(applicant.Substring(applicant.IndexOf('(') + 1, applicant.IndexOf(')') - applicant.IndexOf('(') - 1));
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/ExpatriateBenefitClaim/DisplayForm.aspx?List="
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
                CommonUtil.logError(string.Format("Expatriate Benefit Claim Form：{0}\nError：{1}", WorkflowContext.Current.DataFields["WorkflowNumber"].ToString(), ex.Message));
            }
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
                        double total = Convert.ToDouble(fields["TotalAmount"].ToString());
                        var quota = WorkFlowUtil.GetQuota(fields["CurrManager"].ToString(), levelType);
                        if (total > quota)
                        {
                            #region Set users for workflow
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
                            ExpatriateBenefitClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);
                            #endregion

                            fields["CurrManager"] = managerEmp.UserAccount;
                            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");

                            context.UpdateWorkflowVariable("NextApproveTaskUsers", manager);
                            context.UpdateWorkflowVariable("IsContinue", true);
                        }
                        else
                        {
                            context.UpdateWorkflowVariable("ConfirmTaskUsers", ExpatriateBenefitClaimCommon.GetTaskUsers(ExpatriateBenefitClaimConstants.wf_EBC_FinanceConfirm));
                            context.UpdateWorkflowVariable("IsContinue", false);
                        }
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;

                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Expatriate Benefit Claim");
                        context.UpdateWorkflowVariable("IsContinue", false);
                        SendEmail("Reject");
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
                    break;
                case "ConfirmTask":
                    if (e.Action == "Confirm")
                    {
                        fields["Status"] = CAWorkflowStatus.Completed;
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
                            AddExpatriateBenefitClaimSAPWorkflow(fields);
                        }
                    }

                    if (e.Action == "Reject")
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        this.DataForm.SavePendingForm();
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Expatriate Benefit Claim");
                        SendEmail("Reject");
                    }
                    if (e.Action == "Pending")
                    {
                        fields["Status"] = CAWorkflowStatus.Pending;
                        this.DataForm.SavePendingForm();

                        context.UpdateWorkflowVariable("ConfirmTaskUsers", ExpatriateBenefitClaimCommon.GetTaskUsers(ExpatriateBenefitClaimConstants.wf_EBC_FinanceConfirm));
                        SendEmail("Pending");
                    }

                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
                    break;
                default:
                    break;
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }
        private bool IsExistSAP(string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Expatriate Benefit Claim SAP Workflow");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='EBCWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems.Count > 0 ? false : true;
        }
        public void AddExpatriateBenefitClaimSAPWorkflow(WorkflowDataFields fields)
        {
            SPSite site = SPContext.Current.Site;

            SPList sAPList = CA.SharePoint.SharePointUtil.GetList("Expatriate Benefit Claim SAP Workflow");
            SPList sAPItemsList = CA.SharePoint.SharePointUtil.GetList("ExpatriateBenefitClaimSAPItems");
            SPListItem sAPListItem = sAPList.Items.Add();
            sAPListItem["WorkflowNumber"] = CreateWorkflowNumber();
            sAPListItem["CashAdvanceAmount"] = fields["CashAdvanceAmount"].AsString();
            sAPListItem["TotalAmount"] = Math.Round(double.Parse(fields["TotalAmount"].AsString()), 2).ToString();
            sAPListItem["PreTotalAmount"] = Math.Round(double.Parse(fields["AmountDue"].AsString()), 2).ToString();
            sAPListItem["Applicant"] = fields["Applicant"].AsString();
            sAPListItem["ExpenseDescription"] = fields["ExpenseDescription"].AsString();

            string name = fields["Applicant"].AsString();
            string useraccount = name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1);
            sAPListItem["ApplicantSPUser"] = this.EnsureUser(useraccount);

            sAPListItem["Status"] = CAWorkflowStatus.InProgress;
            sAPListItem["EBCWorkflowNumber"] = fields["WorkflowNumber"].ToString();
            sAPListItem["PostSAPStatus"] = "0";
            sAPListItem["CashAdvanceWorkFlowNumber"] = fields["CashAdvanceIDAndAmount"].AsString();


            NameCollection acAccounts = WorkFlowUtil.GetUsersInGroup(ExpatriateBenefitClaimConstants.wf_EBC_Accountants);
            NameCollection financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(ExpatriateBenefitClaimConstants.wf_EBC_FinanceManager);
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", "ACReviewTask", acAccounts.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", "FinanceConfirmTask", financeConfirmAccounts.JoinString(","));
            sAPListItem["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            sAPListItem.Web.AllowUnsafeUpdates = true;
            sAPListItem.Update();
            string regexText = @"\{[^\{-\}]*\}";
            Regex regex = new Regex(regexText);
            MatchCollection mc = regex.Matches(fields["SummaryExpenseType"].ToString());
            foreach (Match m in mc)
            {
                List<string> itemList = m.Value.Replace("{", "").Replace("}", "").Split(',').ToList<string>();
                SPListItem sAPItemsListItem = sAPItemsList.Items.Add();
                sAPItemsListItem["WorkflowNumber"] = sAPListItem["WorkflowNumber"].ToString();
                sAPItemsListItem["ExpenseType"] = itemList[0].Replace("name:'", "").Replace("'", "").Trim();
                sAPItemsListItem["ItemAmount"] = itemList[1].Replace("val:'", "").Replace("'", "").Trim();
                sAPItemsListItem["CostCenter"] = itemList[2].Replace("costcenter:'", "").Replace("'", "").Trim();
                sAPItemsListItem["Status"] = "0";
                sAPItemsListItem.Web.AllowUnsafeUpdates = true;
                sAPItemsListItem.Update();
            }

            WorkflowVariableValues vs = new WorkflowVariableValues();
            vs["ACReviewUsers"] = GetDelemanNameCollection(acAccounts, ExpatriateBenefitClaimConstants.ExpatriateBenefitClaimSAP);
            vs["FinanceConfirmUsers"] = GetDelemanNameCollection(financeConfirmAccounts, ExpatriateBenefitClaimConstants.ExpatriateBenefitClaimSAP);
            var aCReviewTaskFormUrl = "/_Layouts/CA/WorkFlows/ExpatriateBenefitClaim/ACReview.aspx";
            var financeConfirmTaskFormUrl = "/_Layouts/CA/WorkFlows/ExpatriateBenefitClaim/FinanceConfirm.aspx";
            vs["ACReviewTaskFormUrl"] = aCReviewTaskFormUrl;
            vs["FinanceConfirmTaskFormUrl"] = financeConfirmTaskFormUrl;
            vs["ACReviewTitle"] = fields["Applicant"].ToString() + "'s Expatriate Benefit Claim";
            vs["FinanceConfirmTitle"] = fields["Applicant"].ToString() + "'s Expatriate Benefit Claim";
            var eventData = SerializeUtil.Serialize(vs);
            var wfName = "Expatriate Benefit Claim SAP Workflow";
            var wfAss = sAPList.WorkflowAssociations.GetAssociationByName(wfName, System.Globalization.CultureInfo.CurrentCulture);
            site.WorkflowManager.StartWorkflow(sAPListItem, wfAss, eventData);
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

        private static string CreateWorkflowNumber()
        {
            return "EBCSAP_" + WorkFlowUtil.CreateWorkFlowNumber("ExpatriateBenefitClaimSAPWorkflow").ToString("000000");
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

    }
}