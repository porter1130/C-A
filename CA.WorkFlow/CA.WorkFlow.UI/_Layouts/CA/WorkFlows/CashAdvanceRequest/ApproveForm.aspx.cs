using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Text;
using CA.SharePoint;
using SAP.Middleware.Exchange;

namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSecurity();
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void CheckSecurity()
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true))
            {
                RedirectToTask();
            }
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            switch (WorkflowContext.Current.Step)
            {

                case "NextApproveTask":
                    if (e.Action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase)
                        && this.DataForm1.CashAdvanceType != "Urgent")
                    {
                        context.UpdateWorkflowVariable("IsUrgent", false);
                        WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", false);
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Cash Advance Request");
                        SendEmail("Reject");
                    }
                    else
                    {
                        string levelType = "Contract Approval Limits";
                        double total = Convert.ToDouble(fields["Amount"].ToString());
                        var quota = WorkFlowUtil.GetQuota(fields["CurrManager"].ToString(), levelType);
                        if (total > quota)
                        {
                            if (e.Action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
                            {
                                context.UpdateWorkflowVariable("IsUrgent", true);
                                fields["CashAdvanceType"] = "Normal";
                                string strNextTaskTitle1 = string.Format("{0} {1} {2}'s Cash Advance Request", WorkflowContext.Current.DataFields["WorkflowNumber"].AsString(), WorkflowContext.Current.DataFields["Amount"].AsString(), WorkflowContext.Current.DataFields["Applicant"].AsString());
                                context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle1);
                            }
                            var manager = new NameCollection();
                            var managerEmp = WorkFlowUtil.GetNextApprover(fields["CurrManager"].ToString());
                            if (managerEmp == null && !WorkflowPerson.IsCEO(fields["CurrManager"].ToString()))
                            {
                                DisplayMessage("The manager is not set in the system.");
                                e.Cancel = true;
                                return;
                            }
                            manager.Add(managerEmp.UserAccount);
                            fields["CurrManager"] = managerEmp.UserAccount;
                            NameCollection wfCFO = WorkFlowUtil.GetUsersInGroup("wf_Finace_FC");
                            NameCollection wfFinaceConfirm = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
                            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
                            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTask, manager.JoinString(","));
                            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceFinaceApprove, wfCFO.JoinString(","));
                            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceFinaceConfirmEnd, wfFinaceConfirm.JoinString(","));
                            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceFinaceConfirm, wfFinaceConfirm.JoinString(","));
                            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();
                            WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", true);
                            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(manager, Constants.CAModules.CashAdvanceRequest));
                        }
                        else
                        {
                            if (e.Action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
                            {
                                context.UpdateWorkflowVariable("IsUrgent", true);
                                WorkflowContext.Current.UpdateWorkflowVariable("IsFinance", true);
                                fields["CashAdvanceType"] = "Normal";
                                string strNextTaskTitle2 = string.Format("{0} {1} {2}'s Cash Advance Request", WorkflowContext.Current.DataFields["WorkflowNumber"].AsString(), WorkflowContext.Current.DataFields["Amount"].AsString(), WorkflowContext.Current.DataFields["Applicant"].AsString());
                                context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle2);
                            }
                            WorkflowContext.Current.UpdateWorkflowVariable("IsContinue", false);
                            NameCollection wfCFO1 = WorkFlowUtil.GetUsersInGroup("wf_Finace_FC");
                            string strNextTaskTitle = string.Format("{0} {1} {2}'s Cash Advance Request", WorkflowContext.Current.DataFields["WorkflowNumber"].AsString(), WorkflowContext.Current.DataFields["Amount"].AsString(), WorkflowContext.Current.DataFields["Applicant"].AsString());
                            if (!wfCFO1.Contains(WorkflowContext.Current.DataFields["Applicant"].ToString()) && this.DataForm1.CashAdvanceType == "Urgent")
                            {
                                NameCollection wfCFO2 = WorkFlowUtil.GetUsersInGroup("wf_Finace_FC");
                                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(wfCFO2, Constants.CAModules.CashAdvanceRequest));
                            }
                            if (wfCFO1.Contains(WorkflowContext.Current.DataFields["Applicant"].ToString()) && this.DataForm1.CashAdvanceType == "Urgent")
                            {
                                NameCollection wfFinaceConfirm1 = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
                                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(wfFinaceConfirm1, Constants.CAModules.CashAdvanceRequest));

                                context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle);
                            }
                            if (!wfCFO1.Contains(WorkflowContext.Current.DataFields["Applicant"].ToString()) && this.DataForm1.CashAdvanceType == "Normal")
                            {
                                NameCollection wfFinaceConfirm2 = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
                                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(wfFinaceConfirm2, Constants.CAModules.CashAdvanceRequest));
                                context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle);
                            }
                            if (wfCFO1.Contains(WorkflowContext.Current.DataFields["Applicant"].ToString()) && this.DataForm1.CashAdvanceType == "Normal")
                            {
                                NameCollection wfFinaceConfirm2 = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
                                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(wfFinaceConfirm2, Constants.CAModules.CashAdvanceRequest));
                                context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle);
                            }
                        }
                        fields["Status"] = CAWorkflowStatus.InProgress;
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApproversLoginName);
                    break;
                case "FinaceConfirmEnd":
                    if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //if (PostCashAdvanceToSAP(fields))
                        //{
                        context.UpdateWorkflowVariable("PostSAPResult", true);
                        fields["Status"] = CAWorkflowStatus.Completed;
                        SendEmail("Approve");
                        //    this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Post success! ');</script>");
                        //}
                        //else
                        //{
                        //    context.UpdateWorkflowVariable("PostSAPResult", false);
                        //    this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Post failed! ');</script>");
                        //}
                        if (IsExistSAP(fields["WorkflowNumber"].ToString()))
                        {
                            AddCashAdvanceSAP(fields);
                        }
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Cash Advance Request");
                        SendEmail("Reject");
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApproversLoginName);
                    break;
                case "FinaceApprove":
                    if (e.Action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase)
                        && this.DataForm1.CashAdvanceType != "Urgent")
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Cash Advance Request");
                        SendEmail("Reject");
                    }
                    else
                    {
                        if (e.Action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
                        {
                            fields["CashAdvanceType"] = "Normal";
                        }
                        NameCollection wfFinaceConfirm1 = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
                        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(wfFinaceConfirm1, Constants.CAModules.CashAdvanceRequest));
                        fields["Status"] = CAWorkflowStatus.InProgress;
                        string strNextTaskTitle = string.Format("{0} {1} {2}'s Cash Advance Request", WorkflowContext.Current.DataFields["WorkflowNumber"].AsString(), WorkflowContext.Current.DataFields["Amount"].AsString(), WorkflowContext.Current.DataFields["Applicant"].AsString());
                        context.UpdateWorkflowVariable("NextApproveTaskTitle", strNextTaskTitle);
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApproversLoginName);
                    break;
                case "FinaceConfirm":
                    if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //if (PostCashAdvanceToSAP(fields))
                        //{
                        context.UpdateWorkflowVariable("PostSAPResult", true);
                        fields["Status"] = CAWorkflowStatus.Completed;
                        SendEmail("Approve");
                        //    this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Post success! ');</script>");
                        //}
                        //else 
                        //{
                        //    context.UpdateWorkflowVariable("PostSAPResult", false);
                        //    this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Post failed! ');</script>");
                        //}
                        if (IsExistSAP(fields["WorkflowNumber"].ToString()))
                        {
                            AddCashAdvanceSAP(fields);
                        }
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit Cash Advance Request");
                        SendEmail("Reject");
                    }
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceApproversLoginName);
                    break;
                case "CompleteTask":
                    var managerEmp1 = WorkFlowUtil.GetNextApprover(CurrentEmployee);
                    NameCollection manager1 = new NameCollection();
                    manager1.Add(managerEmp1.UserAccount);
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.CashAdvanceNextApproveTaskUsers, GetDelemanNameCollection(manager1, Constants.CAModules.CashAdvanceRequest));
                    context.UpdateWorkflowVariable("IsContinue", true);
                    break;
            }
            WorkFlowUtil.UpdateWorkflowPath(context);
        }

        private bool IsExistSAP(string workFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequestSAP");
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='CAWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection listItems = delegationList.GetItems(query);
            return listItems.Count > 0 ? false : true;
        }

        #region SAP

        private void AddCashAdvanceSAP(WorkflowDataFields fields)
        {
            SPSite site = SPContext.Current.Site;

            SPList sAPList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequestSAP");
            SPListItem sAPListItem = sAPList.Items.Add();
            sAPListItem["WorkflowNumber"] = CreateWorkflowNumber();
            sAPListItem["CAWorkflowNumber"] = fields["WorkflowNumber"].ToString();
            sAPListItem["Amount"] = fields["Amount"].ToString();
            sAPListItem["Department"] = fields["Department"].ToString();
            sAPListItem["Status"] = CAWorkflowStatus.InProgress;
            sAPListItem["SAPStatus"] = "0";
            sAPListItem["AdvanceType"] = Int32.Parse(fields["Amount"].ToString()) > 2000 ? "Transfer" : "Cash";
            sAPListItem["AdvanceRemark"] = fields["Purpose"].AsString() + ";" + fields["UrgentRemark"].AsString() + ";" + fields["Remark"].AsString();

            Employee employee = UserProfileUtil.GetEmployee(fields["Applicant"].ToString());
            sAPListItem["EmployeeID"] = employee.EmployeeID;
            sAPListItem["EmployeeName"] = employee.DisplayName;
            sAPListItem["Applicant"] = fields["Applicant"].ToString();
            sAPListItem["ApplicantSPUser"] = this.EnsureUser(fields["Applicant"].ToString());

            NameCollection acAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_Accountants);
            NameCollection financeConfirmAccounts = WorkFlowUtil.GetUsersInGroup(WorkflowGroupName.WF_FinanceManager);
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", "ACReviewTask", acAccounts.JoinString(","));
            strStepAndUsers.AppendFormat("{0}:{1};", "FinanceConfirmTask", financeConfirmAccounts.JoinString(","));
            sAPListItem["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            sAPListItem.Web.AllowUnsafeUpdates = true;
            sAPListItem.Update();

            WorkflowVariableValues vs = new WorkflowVariableValues();
            vs["ACReviewUsers"] = GetDelemanNameCollection(acAccounts, Constants.CAModules.CashAdvanceRequestSAP);
            vs["FinanceConfirmUsers"] = GetDelemanNameCollection(financeConfirmAccounts, Constants.CAModules.CashAdvanceRequestSAP);
            var aCReviewTaskFormUrl = "/_Layouts/CA/WorkFlows/CashAdvanceRequest/ACReview.aspx";
            var financeConfirmTaskFormUrl = "/_Layouts/CA/WorkFlows/CashAdvanceRequest/FinanceConfirm.aspx";
            vs["ACReviewTaskFormUrl"] = aCReviewTaskFormUrl;
            vs["FinanceConfirmTaskFormUrl"] = financeConfirmTaskFormUrl;
            vs["ACReviewTitle"] = fields["Applicant"].ToString() + "'s Cash Advance Request";
            vs["FinanceConfirmTitle"] = fields["Applicant"].ToString() + "'s Cash Advance Request";
            var eventData = SerializeUtil.Serialize(vs);
            var wfName = "CashAdvanceRequestSAP";
            var wfAss = sAPList.WorkflowAssociations.GetAssociationByName(wfName, System.Globalization.CultureInfo.CurrentCulture);
            site.WorkflowManager.StartWorkflow(sAPListItem, wfAss, eventData);
            WorkFlowUtil.UpdateWorkflowPath(sAPListItem, eventData);
        }

        private static string CreateWorkflowNumber()
        {
            return "CASAP_" + WorkFlowUtil.CreateWorkFlowNumber("CashAdvanceRequestSAP").ToString("000000");
        }

        #endregion










        //private bool PostCashAdvanceToSAP(WorkflowDataFields fields) 
        //{
        //    bool postResult = false;
        //    string employeeID = "";
        //    string employeeName = "";
        //    string amount = "";
        //    string workflowNumber = "";
        //    string sAPNumber = "";
        //    string errorMsg = fields["ErrorMsg"].AsString();
        //    Employee employee = UserProfileUtil.GetEmployee(fields["Applicant"].ToString());
        //    employeeID = employee.EmployeeID;
        //    employeeName = employee.DisplayName;
        //    amount = fields["Amount"].ToString();
        //    workflowNumber = fields["WorkflowNumber"].ToString();

        //    ////Post SAP Method
        //    List<SapParameter> mSapParametersCD = new List<SapParameter>();
        //    SapParameter mSapParameters = new SapParameter()
        //    {
        //        BusAct = "RFBU",
        //        CompCode = "CA10",
        //        DocType = "SA",
        //        BusArea = "0001",
        //        Currency = "RMB",
        //        EmployeeID = employeeID,
        //        EmployeeName = employeeName,
        //        ExchRate = 1,
        //        Header = "Cash Advance",
        //        RefDocNo = workflowNumber,
        //        UserName = "acnotes",
        //        CashAmount = decimal.Parse(amount),
        //        PaidByCC = 100

        //    };
        //    mSapParametersCD.Add(mSapParameters);

        //    ISapExchange sapExchange = SapExchangeFactory.GetCashAdvance();
        //    List<object[]> result = sapExchange.ImportDataToSap(mSapParametersCD);
        //    for (int i = 0; i < result.Count; i++)
        //    {
        //        SapParameter sp = (SapParameter)result[i][0];
        //        bool bl = (bool)result[i][2];
        //        if (bl)
        //        {
        //            SapResult sr = (SapResult)result[i][1];
        //            sAPNumber = sr.OBJ_KEY;
        //            postResult = true;
        //        }
        //        else
        //        {
        //            if (result[i][1] is string)
        //            {
        //                errorMsg += fields["Applicant"].ToString() + "-" + DateTime.Now.ToShortDateString() + "：" + result[i][1].ToString() + " \n ";
        //            }
        //            else
        //            {
        //                string wfID = sp.RefDocNo;
        //                SapResult sr = (SapResult)result[i][1];
        //                foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
        //                {
        //                    errorMsg += fields["Applicant"].ToString() + "-" + DateTime.Now.ToShortDateString() + "：" + ret.MESSAGE + " \n ";
        //                }
        //            }
        //        }
        //    }
        //    fields["SAPNumber"] = sAPNumber;
        //    fields["ErrorMsg"] = errorMsg;
        //    return postResult;
        //}



        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        //private void SendEmail1(string emailType)
        //{
        //    var fields = WorkflowContext.Current.DataFields;
        //    var templateTitle = "CashAdvanceRequest";

        //    Employee emp = UserProfileUtil.GetEmployeeEx(this.DataForm1.RequestBy);


        //    string rootweburl = SPContext.Current.Web.Url;
        //    string detailLink = rootweburl + "/_Layouts/CA/WorkFlows/CashAdvanceRequest/DisplayForm.aspx?List="
        //                                     + Request.QueryString["List"]
        //                                     + "&ID=" + Request.QueryString["ID"];

        //    List<string> parameters = new List<string>();
        //    parameters.Add("");
        //    parameters.Add(detailLink);
        //    List<string> to = new List<string>();
        //    to.Add(this.DataForm1.RequestBy);

        //    SendNotificationMail(templateTitle, parameters, to, true);





        //    List<string> mailList = new List<string>();
        //    //SPUser user = EnsureUser(this.DataForm1.RequestBy);
        //    //Employee emp = WorkFlowUtil.get.GetEmployee(this.DataForm1.RequestBy);
        //    //mailList.Add(user.Email);

        //    mailList.Add(emp.WorkEmail);
        //    StringDictionary dict = new StringDictionary();
        //    //dict.Add("to", "liu_jun-lj1@vanceinfo.com");
        //    dict.Add("to", string.Join(";", mailList.ToArray()));
        //    dict.Add("subject", "Cash Advance Request");
        //    StringBuilder str = new StringBuilder();
        //    str.Append("Dear " + this.DataForm1.RequestBy + ",<br/>");
        //    switch (emailType)
        //    {
        //        case "Approve":
        //            str.Append("Your Cash Advance Request has been approved. The  Cash Advance Request form will now be passed to Finance for processing of payment.  The reimbursed amount will be transferred to your bank account in two weeks.");
        //            break;
        //        case "Reject":
        //            str.Append("Your Cash Advance Request has been rejected by " + SPContext.Current.Site.RootWeb.CurrentUser.LoginName + ".  Please click on the below link to check on the reject reason");
        //            break;
        //        case "NoObtainedApprove":
        //            str.Append("Your Cash Advance Request has been approved , but with line item(s) rejected due to amount exceeding the Company standard.  The Company standard amount will be paid out for the rejected item(s). Your expense claim form will now be passed to Finance for payment processing.  The reimbursed amount will be transferred to your bank account in two weeks.");
        //            break;
        //    }
        //    string mcontent = str.ToString()
        //        + SPContext.Current.ListItem["WorkflowNumber"].ToString() + ".<br/><br/>" + @"Please view the detail by clicking <a href='"
        //           + SPContext.Current.Web.Url + "/_layouts/CA/WorkFlows/CashAdvanceRequest/DisplayForm.aspx?List="
        //           + SPContext.Current.ListId.ToString()
        //           + "&ID="
        //           + SPContext.Current.ListItem.ID
        //           + "'>here</a>.";


        //    SPUtility.SendEmail(SPContext.Current.Web, dict, mcontent);
        //}

        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "CashAdvanceRequest" + emailType;

                var applicantAccount = fields["Applicant"].AsString();

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/CashAdvanceRequest/DisplayForm.aspx?List="
                                                 + Request.QueryString["List"]
                                                 + "&ID=" + Request.QueryString["ID"];

                List<string> parameters = new List<string>();
                List<string> to = new List<string>();
                to.Add(applicantAccount);

                switch (emailType)
                {
                    case "Approve":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].AsString());
                        parameters.Add(detailLink);
                        SendNotificationMail(templateTitle, parameters, to, true);
                        break;
                    case "Reject":
                        parameters.Add(applicantAccount);
                        parameters.Add(fields["WorkflowNumber"].AsString());
                        parameters.Add(CurrentEmployee.DisplayName);
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

    }
}

