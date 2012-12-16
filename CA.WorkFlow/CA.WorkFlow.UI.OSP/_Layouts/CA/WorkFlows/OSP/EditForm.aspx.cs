using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using System.Data;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.OSP
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            Actions.OnClientClick = "return beforeSubmit(this)";
            if (IsPostBack)
            {
                return; 
            }
            DataEdit1.bIsEdt = true;
            Bind();
        }
        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("Submit"))
            {
                bool isSuccess = RunWorkflow(true);
                if (!isSuccess)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else //save
            {
                bool isSuccess = RunWorkflow(false);
                if (!isSuccess)
                {
                    e.Cancel = true;
                    return;
                }  
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        /// <summary>
        /// 执行工作流
        /// </summary>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        bool RunWorkflow(bool isSubmit)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNO = fields["Title"].ToString();
            bool isSuccess = true;

            DataTable dt = DataEdit1.GetResultDt(sWorkflowNO);
            if (null == dt || dt.Rows.Count == 0)
            {
                DisplayMessage("No avaliable data!");
                return false;
            }
            else
            {
                OSPCommon comm = new OSPCommon();
                comm.DeleteData(sWorkflowNO);//删除老数据
                comm.BatchAddToListByDatatable(dt, "OSPItems");//添加新数据
                WorkflowContext context = WorkflowContext.Current;
                if (isSubmit)//是提交
                {
                    bool isNotNeedApprove = DataEdit1.IsNotNeedApprove();
                    if (isNotNeedApprove)//不需要审批
                    {
                        bool isAllUpdated = DataEdit1.UpdateToSAP(sWorkflowNO);
                        if (isAllUpdated)//全部更新成功。
                        {
                            context.UpdateWorkflowVariable("OSPActions", OSPCommon.End);
                            fields["Status"] = CAWorkflowStatus.Completed;

                            //comm.UpdateOSPSuccess(sWorkflowNO);
                        }
                        else
                        {
                            isSuccess = false;
                        }
                    }
                }


                fields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
                SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(Userinfo1.Applicant.UserAccount);
                fields["ApplicantSPUser"] = ApplicantSPUser;
                return isSuccess;
            }
        }

        /// <summary>
        /// 给 Repeater绑定数据（用于显示）
        /// </summary>
        void Bind()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNO = fields["Title"].ToString();
            DataEdit1.BindData(sWorkflowNO); 
        }


    }
}