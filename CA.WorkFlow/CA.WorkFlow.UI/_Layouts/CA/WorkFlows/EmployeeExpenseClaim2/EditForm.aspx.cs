namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    using SharePoint.Utilities.Common;
    using System;
    using QuickFlow.Core;
    using System.ComponentModel;
    using System.Collections.Generic;
    using CA.SharePoint;
    using QuickFlow;
    using Microsoft.SharePoint;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                DataForm.IsAttachInvoice = Convert.ToBoolean(fields["IsAttachInvoice"]);
                DataForm.SummaryExpenseType = fields["SummaryExpenseType"].AsString();
                DataForm.ExpatriateBenefitForm = fields["ExpatriateBenefitForm"].AsString();
                DataForm.CashAdvanceAmount = fields["CashAdvanceAmount"].AsString();
                DataForm.CashAdvanceID = fields["CashAdvanceWorkFlowNumber"].AsString();
                DataForm.CashAdvanceIDAndAmount = fields["CashAdvanceIDAndAmount"].AsString();
                DataForm.TotalAmount = fields["TotalAmount"].AsString();

                string userAccount = fields["RequestedBy"].AsString().Substring(fields["RequestedBy"].AsString().IndexOf("(") + 1, fields["RequestedBy"].AsString().IndexOf(")") - fields["RequestedBy"].AsString().IndexOf("(") - 1);
                Employee employee = UserProfileUtil.GetEmployee(userAccount);
                DataForm.Applicant = employee;
                DataForm.JobLevel = employee.JobLevel == "" ? "9" : employee.JobLevel.Substring(employee.JobLevel.IndexOf('-') + 1, 1);
            }
            this.DataForm.DataFormMode = "Edit";

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            this.btnSave.Click += this.btnSave_Click;

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
                //string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/EmployeeExpenseClaim2/DisplayForm.aspx?List="
                //                                 + Request.QueryString["List"]
                //                                 + "&ID=" + Request.QueryString["ID"];
                string detailLink = rootweburl + "WorkFlowCenter/Lists/EmployeeExpenseClaimWorkflow/MyApply.aspx";
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
                        parameters.Add(fields["WorkflowNumber"].ToString());
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            string url = Request.UrlReferrer.ToString();
            if (this.DataForm.ExpatriateBenefitForm.Length <= 2)
            {
                Response.Write("<script type=\"text/javascript\">alert('Please fill in the Employee Expense Claim Details.');window.location = '" + url + "';</script>");
                Response.End();
                return;
            }
            else
            {
                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                fields["Applicant"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";
                fields["RequestedBy"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";
                string workflowNumber = fields["WorkflowNumber"].AsString();

                #region Save the data
                EmployeeExpenseClaimCommon.DeleteAllDraftItems(workflowNumber);
                EmployeeExpenseClaimCommon.SaveDetails1(this.DataForm.ExpatriateBenefitForm, workflowNumber);

                fields["ApplicantSPUser"] = this.EnsureUser(this.DataForm.Applicant.UserAccount);
                fields["Department"] = this.DataForm.Applicant.Department;

                fields["CashAdvanceAmount"] = this.DataForm.CashAdvanceAmount;
                fields["CashAdvance"] = this.DataForm.CashAdvanceAmount;
                fields["CashAdvanceWorkFlowNumber"] = this.DataForm.CashAdvanceID;
                fields["CashAdvanceIDAndAmount"] = this.DataForm.CashAdvanceIDAndAmount;
                fields["IsAttachInvoice"] = this.DataForm.IsAttachInvoice;
                fields["SummaryExpenseType"] = this.DataForm.SummaryExpenseType;
                fields["ExpatriateBenefitForm"] = this.DataForm.ExpatriateBenefitForm;

                var totalAmount = decimal.Parse(this.DataForm.TotalAmount);
                var cashAdvanceAmount = decimal.Parse(this.DataForm.CashAdvanceAmount);
                fields["TotalAmount"] = Math.Round(totalAmount, 2).ToString();

                var amountDue = Math.Round(totalAmount - cashAdvanceAmount, 2);
                fields["AmountDue"] = amountDue.ToString();
                #endregion

                context.SaveTask();
                RedirectToTask();

            }
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
        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            string url = Request.UrlReferrer.ToString();
            if (this.DataForm.ExpatriateBenefitForm.Length <= 2)
            {
                Response.Write("<script type=\"text/javascript\">alert('Please fill in the Employee Expense Claim Details.');window.location = '" + url + "';</script>");
                Response.End();
                e.Cancel = true;
                return;
            }
            
            //string taskTitle = this.DataForm.Applicant.DisplayName + "'s Employee Expense Claim ";
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string taskTitle = fields["WorkflowNumber"].AsString() + " " + Math.Round(double.Parse(this.DataForm.TotalAmount), 2) + " " + this.DataForm.Applicant.DisplayName + "'s Employee Expense Claim ";
            string workflowNumber = fields["WorkflowNumber"].AsString();
            fields["Applicant"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";
            fields["RequestedBy"] = this.DataForm.Applicant.DisplayName + "(" + DataForm.Applicant.UserAccount + ")";
            #region Set users for workflow

            var managerEmp = WorkFlowUtil.GetNextApprover(this.DataForm.Applicant);
            if (managerEmp == null)
            {
                if (!WorkflowPerson.IsCEO(this.DataForm.Applicant.UserAccount))
                {
                    Response.Write("<script type=\"text/javascript\">alert('The manager is not set in the system.');window.location = '" + url + "';</script>");
                    Response.End();
                    e.Cancel = true;
                    return;
                }
                else
                {
                    List<string> cfos = WorkflowPerson.GetCFO();
                    if (cfos.Count == 0)
                    {
                        Response.Write("<script type=\"text/javascript\">alert('The init error about WorkflowPerson in the system.');window.location = '" + url + "';</script>");
                        Response.End();
                        e.Cancel = true;
                        return;
                    }
                    managerEmp = UserProfileUtil.GetEmployeeEx(cfos[0]);
                }
            }
            var manager = new NameCollection();
            EmployeeExpenseClaimCommon.GetTaskUsers(manager, managerEmp.UserAccount);
            fields["CurrManager"] = managerEmp.UserAccount;
            context.UpdateWorkflowVariable("NextApproveTaskUsers", manager);

            NameCollection wfCFO = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimNextApproveTask, managerEmp.UserAccount);
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.EmployeeExpenseClaimConfirmTask, wfCFO.JoinString(","));
            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            #endregion

            context.UpdateWorkflowVariable("IsSave", false);
            context.DataFields["Status"] = CAWorkflowStatus.InProgress;

            SendEmail("Submit");
            #region Save the data
            EmployeeExpenseClaimCommon.DeleteAllDraftItems(workflowNumber);
            EmployeeExpenseClaimCommon.SaveDetails1(this.DataForm.ExpatriateBenefitForm, workflowNumber);

            fields["ApplicantSPUser"] = this.EnsureUser(this.DataForm.Applicant.UserAccount);
            fields["Department"] = this.DataForm.Applicant.Department;

            fields["CashAdvanceAmount"] = this.DataForm.CashAdvanceAmount;
            fields["CashAdvance"] = this.DataForm.CashAdvanceAmount;
            fields["CashAdvanceWorkFlowNumber"] = this.DataForm.CashAdvanceID;
            fields["CashAdvanceIDAndAmount"] = this.DataForm.CashAdvanceIDAndAmount;
            fields["IsAttachInvoice"] = this.DataForm.IsAttachInvoice;
            fields["SummaryExpenseType"] = this.DataForm.SummaryExpenseType;
            fields["ExpatriateBenefitForm"] = this.DataForm.ExpatriateBenefitForm;

            var totalAmount = decimal.Parse(this.DataForm.TotalAmount);
            var cashAdvanceAmount = decimal.Parse(this.DataForm.CashAdvanceAmount);
            fields["TotalAmount"] = Math.Round(totalAmount, 2).ToString();

            var amountDue = Math.Round(totalAmount - cashAdvanceAmount, 2);
            fields["AmountDue"] = amountDue.ToString();
            #endregion

            #region Set title for workflow
            context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Employee Expense Claim");
            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "");
            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle + "");
            #endregion

            #region Set page URL for workflow
            var editURL = "/_Layouts/CA/WorkFlows/EmployeeExpenseClaim2/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/EmployeeExpenseClaim2/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormURL", approveURL);
            #endregion


            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }

}