using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using QuickFlow.Core;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.OSP
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            DataView1.isApproveStep = true;
            Actions.OnClientClick = "return beforeSubmit(this);";
           // TaskTrace2.Applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("Approve"))
            {

                bool isAllSuccess = DataView1.UpdateToSAP();
                if (isAllSuccess)
                {
                    CompleteWorkflow();
                    SendNoticeMail("Approved");
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                CompleteWorkflow();
                SendNoticeMail("Rejected");
            }
        }

        /// <summary>
        /// 工作流完成
        /// </summary>
        void CompleteWorkflow()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            fields["Status"] = CAWorkflowStatus.Completed;
            fields["Approvers"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(CurrentEmployee.UserAccount);
            fields["ApproversSPUser"] = ApplicantSPUser;
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sStatus"></param>
        void SendNoticeMail(string sStatus)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            OSPCommon comm = new OSPCommon();
            try
            {
                comm.SendMail(WorkFlowUtil.GetApplicantAccount(fields["Applicant"].ToString()), sStatus, fields["Title"].ToString(), CurrentEmployee.UserAccount);
            }
            catch (Exception e)
            {
                CommonUtil.logError(e.ToString());
            }
        }

    }
}