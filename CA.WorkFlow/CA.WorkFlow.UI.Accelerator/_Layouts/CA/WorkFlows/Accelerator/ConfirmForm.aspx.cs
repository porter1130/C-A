using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.Accelerator
{
    public partial class ConfirmForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            Actions.OnClientClick = "return beforeSubmit(this)";
           // DataView1.ShowInernalNO();
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("Confirm"))//审批通过
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                fields["Status"] = CAWorkflowStatus.Completed;

                string sCurrentManager = CurrentEmployee.UserAccount;
                fields["Approvers"] = ReturnAllApprovers(sCurrentManager);
                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", sCurrentManager);

                SendNoticeMail("Approved");
            }
            else
            {
                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                string sWorkflowNo = fields["Title"].ToString();
                context.UpdateWorkflowVariable("EditTitle", "Please resubmit Purchase Ruequest-General:" + sWorkflowNo);//
                SendNoticeMail("Rejected");
            }
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sStatus"></param>
        void SendNoticeMail(string sStatus)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            AcceleratorComm comm = new AcceleratorComm();
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