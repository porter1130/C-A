using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using QuickFlow.Core;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.POTypeChange
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            Actions.OnClientClick = "return beforeSubmit(this);";
            DataView1.isApproveStep = true;
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("Reject"))
            {
                CompleteWorkflow(CAWorkflowStatus.Rejected);
                //SendNoticeMail(CAWorkflowStatus.Rejected);
                return;
            }


            if (WorkflowContext.Current.Step == "DMMApprove")
            {
                DataView1.UpdateItemApproveStatus();
                CompleteWorkflow(CAWorkflowStatus.InProgress);
            }
            else//BDApprove 最后一步审批
            {
                bool isAllSuccess = DataView1.UpdateToSAP();
                if (isAllSuccess)
                {
                    CompleteWorkflow( CAWorkflowStatus.Completed);
                    //SendNoticeMail("Approved");
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

       /// <summary>
        /// 工作流完成
       /// </summary>
       /// <param name="sStatus"></param>
        void CompleteWorkflow(string sStatus)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            fields["Status"] = sStatus;
            fields["Approvers"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(CurrentEmployee.UserAccount);
            fields["ApproversSPUser"] = ApplicantSPUser;

            if (sStatus == CAWorkflowStatus.Rejected)
            {
                WorkflowContext context = WorkflowContext.Current;
                context.UpdateWorkflowVariable("EditTitle", "Please resubmit POTypeChange:" + fields["Title"].ToString());
            }
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
        /*
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sStatus"></param>
        void SendNoticeMail(string sStatus)
        {
            try
            {
                string sName = CurrentEmployee.DisplayName;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                Common comm = new Common();
                string sApprovers = fields["Approvers"].ToString();
                comm.SendMail(sApprovers, sStatus, fields["Title"].ToString(), sName);
            }
            catch (Exception e)
            {
                CommonUtil.logError("POTypeChange sent mail failed:" + e.ToString());
            }
        }*/

    }
}