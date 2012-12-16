using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using QuickFlow.UI.Controls;
using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.NTSC
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataForm.Applicant = CurrentEmployee;
                DataForm.DataFormMode = "New";
            }
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string url = Request.UrlReferrer.ToString();
            if (!this.DataForm.SubmitStatus) 
            {
                Response.Write("<script type=\"text/javascript\">alert('You are not in Buying Department .');window.location = '" + url + "';</script>");
                Response.End();
                e.Cancel = true;
                return;
            }
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            var btn = sender as StartWorkflowButton;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                context.UpdateWorkflowVariable("IsSave", true);
                context.DataFields["Status"] = CAWorkflowStatus.Pending;
            }
            else
            {
                #region Set users for workflow
                Employee manager = WorkFlowUtil.GetApproverByLevelPAD(this.DataForm.Applicant);
                if (null == manager)
                {
                    Response.Write("<script type=\"text/javascript\">alert('The init error about applicant’s manager in the system.');window.location = '" + url + "';</script>");
                    Response.End();
                    e.Cancel = true;
                    return;
                }
                NameCollection DMMTask = new NameCollection();
                DMMTask.Add(manager.UserAccount);
                fields["CurrManager"] = manager.UserAccount;

                NameCollection wf_NTSC_QM = new NameCollection();
                //NameCollection wf_NTSC_QAD = new NameCollection();
                NameCollection wf_NTSC_SCM = new NameCollection();
                NameCollection wf_NTSC_SCMM = new NameCollection();
                List<string> qm = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_QM);
                //List<string> qad = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_QAD);
                List<string> scm = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_SCM);
                List<string> scmm = WorkFlowUtil.UserListInGroup(NewTradeSupplierCreationConstants.wf_NTSC_SCMM);
                System.Text.StringBuilder group = new System.Text.StringBuilder();
                if (qm.Count == 0)
                {
                    group.Append(" wf_NTSC_QM ");
                }
                //if (qad.Count == 0)
                //{
                //    group.Append(" wf_NTSC_QAD ");
                //}
                if (scm.Count == 0)
                {
                    group.Append(" wf_NTSC_SCM ");
                }
                if (scmm.Count == 0)
                {
                    group.Append(" wf_NTSC_SCMM ");
                }
                if (scm.Count == 0 || scmm.Count == 0 || qm.Count==0)
                {
                    Response.Write("<script type=\"text/javascript\">alert('The init error about WorkflowPerson in the " + group.ToString() + "');window.location = '" + url + "';</script>");
                    Response.End();
                    e.Cancel = true;
                    return;
                }
                wf_NTSC_QM.AddRange(qm.ToArray());
                //wf_NTSC_QAD.AddRange(qad.ToArray());
                wf_NTSC_SCM.AddRange(scm.ToArray());
                wf_NTSC_SCMM.AddRange(scmm.ToArray());

                System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
                strStepAndUsers.AppendFormat("{0}:{1};", "DMMTask", DMMTask.JoinString(","));
                strStepAndUsers.AppendFormat("{0}:{1};", "QMTask", wf_NTSC_QM.JoinString(","));
                //strStepAndUsers.AppendFormat("{0}:{1};", "QADTask", wf_NTSC_QAD.JoinString(","));
                strStepAndUsers.AppendFormat("{0}:{1};", "SCMTask", wf_NTSC_SCM.JoinString(","));
                strStepAndUsers.AppendFormat("{0}:{1};", "SCMConfirmTask", wf_NTSC_SCMM.JoinString(","));
                fields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

                context.UpdateWorkflowVariable("NextApproveTaskUsers", GetDelemanNameCollection(DMMTask, WorkFlowUtil.GetModuleIdByListName("New Trade Supplier Creation")));
                #endregion
                context.UpdateWorkflowVariable("IsSave", false);
                context.DataFields["Status"] = CAWorkflowStatus.InProgress;
            }
            #region Save the data
            string workflowNumber = CreateWorkflowNumber();
            fields["WorkFlowNumber"] = workflowNumber;
            fields["Applicant"] = this.DataForm.Applicant.UserAccount;
            fields["Department"] = this.DataForm.Applicant.Department;
            fields["ApplicantSPUser"] = NewTradeSupplierCreation.EnsureUser(this.DataForm.Applicant.UserAccount);
            fields["SupplierName"] = this.DataForm.SupplierName;
            fields["SubDivision"] = this.DataForm.Applicant.Department;
            fields["Reason"] = this.DataForm.Reason;
            fields["IsMondial"] = this.DataForm.IsMondial == "Yes" ? true : false;
            #endregion
            #region Set title for workflow
            //string taskTitle = string.Format("{0} {1} {2}'s New Trade Supplier Creation needs approval"
            //                                             , workflowNumber
            //                                             , this.DataForm.SupplierName
            //                                             , this.DataForm.Applicant.UserAccount);
            string taskTitle = string.Format("{0} {1}'s New Trade Supplier Creation needs approval"
                                                        , workflowNumber
                                                        , this.DataForm.Applicant.UserAccount);
            context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete New Trade Supplier Creation");
            context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle);
            #endregion
            #region Set page URL for workflow
            var editURL = "/_Layouts/CA/WorkFlows/NewTradeSupplierCreation/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/NewTradeSupplierCreation/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            #endregion
            SendEmail("Submit");
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private static string CreateWorkflowNumber()
        {
            return "NTSC" + WorkFlowUtil.CreateWorkFlowNumber("NewTradeSupplierCreation").ToString("00000000");
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
                CommonUtil.logError(string.Format("NewTradeSupplierCreation：{0}\nError：{1}", WorkflowContext.Current.DataFields["WorkFlowNumber"].ToString(), ex.Message));
            }
        }

    }
}