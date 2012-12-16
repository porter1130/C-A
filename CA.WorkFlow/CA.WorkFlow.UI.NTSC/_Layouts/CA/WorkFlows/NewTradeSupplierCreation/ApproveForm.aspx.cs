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

namespace CA.WorkFlow.UI.NTSC
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

                this.TaskTrace.Applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();
            }
            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e) 
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            switch (WorkflowContext.Current.Step)
            {
                case "DMMTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        NameCollection wf_NTSC_QM = new NameCollection();
                        List<string> qm = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_QM);
                        wf_NTSC_QM.AddRange(qm.ToArray());
                        fields["CurrManager"] = qm.ToArray();
                        context.UpdateWorkflowVariable("NextApproveTaskUsers", GetDelemanNameCollection(wf_NTSC_QM, WorkFlowUtil.GetModuleIdByListName("New Trade Supplier Creation")));
                    }
                    else 
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit New Trade Supplier Creation");
                    }
                    break;
                case "QMTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        NameCollection wf_NTSC_SCM = new NameCollection();
                        List<string> scm = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_SCM);
                        wf_NTSC_SCM.AddRange(scm.ToArray());
                        fields["CurrManager"] = scm.ToArray();
                        context.UpdateWorkflowVariable("NextApproveTaskUsers", GetDelemanNameCollection(wf_NTSC_SCM, WorkFlowUtil.GetModuleIdByListName("New Trade Supplier Creation")));
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit New Trade Supplier Creation");
                    }
                    break;
                case "SCMTask":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        NameCollection wf_NTSC_SCMM = new NameCollection();
                        List<string> scmm = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_SCMM);
                        wf_NTSC_SCMM.AddRange(scmm.ToArray());
                        fields["CurrManager"] = scmm.ToArray();
                        context.UpdateWorkflowVariable("NextApproveTaskUsers", GetDelemanNameCollection(wf_NTSC_SCMM, WorkFlowUtil.GetModuleIdByListName("New Trade Supplier Creation")));
                        //string taskTitle = string.Format("{0} {1} {2}'s New Trade Supplier Creation needs confirm"
                        //                                    , fields["WorkFlowNumber"].AsString()
                        //                                    , fields["SupplierName"].AsString()
                        //                                    , fields["Applicant"].AsString());
                        string taskTitle = string.Format("{0} {1}'s New Trade Supplier Creation needs confirm"
                                                            , fields["WorkFlowNumber"].AsString()
                                                            , fields["Applicant"].AsString());
                        context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle);
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit New Trade Supplier Creation");
                    }
                    break;
                case "SCMConfirmTask":
                    if (e.Action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fields["Status"] = CAWorkflowStatus.Completed;
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Rejected;
                        context.UpdateWorkflowVariable("CompleteTaskTitle", "Please resubmit New Trade Supplier Creation");
                    }
                    break;
            }
            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, "ApproversSPUser", "Approvers");
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }
        private void SendEmail(string emailType)
        {
            try
            {
                var fields = WorkflowContext.Current.DataFields;
                var templateTitle = "NewTradeSupplierCreation" + emailType;

                Employee employee = UserProfileUtil.GetEmployee(fields["Applicant"].ToString());
                string applicantAccount = employee.UserAccount;

                string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
                string detailLink = rootweburl + "WorkFlowCenter/_layouts/CA/WorkFlows/NewTradeSupplierCreation/DisplayForm.aspx?List="
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
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.logError(string.Format("New Trade Supplier Creation：{0}\nError：{1}", WorkflowContext.Current.DataFields["WorkflowNumber"].ToString(), ex.Message));
            }
        }
        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

    }
}