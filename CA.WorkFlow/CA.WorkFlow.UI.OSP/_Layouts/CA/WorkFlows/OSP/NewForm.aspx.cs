using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using QuickFlow.Core;
using QuickFlow;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.OSP
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StartWorkflowButtonSubmit.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButtonSubmit_Executing);
            StartWorkflowButtonSubmit.Executed += new EventHandler(StartWorkflowButtonSubmit_Executed);
            StartWorkflowButtonSave.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButtonSave_Executing);
            StartWorkflowButtonSave.Executed += new EventHandler(StartWorkflowButtonSave_Executed);

            StartWorkflowButtonSubmit.OnClientClick = "return beforeSubmit(this)";
            StartWorkflowButtonSave.OnClientClick = "return beforeSubmit(this)";
            if (IsPostBack)
            {
                return;
            }
        }

        void StartWorkflowButtonSave_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isSuccess= StartWorkflow(false);
            if (!isSuccess)
            {
                e.Cancel = true;
                return;
            }
        }


        void StartWorkflowButtonSubmit_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isSuccess = StartWorkflow(true);
            if (!isSuccess)
            {
                e.Cancel = true;
                return;
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void StartWorkflowButtonSubmit_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void StartWorkflowButtonSave_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }


        /// <summary>
        /// 生成工作流编号
        /// </summary>
        /// <returns></returns>
        string CreateWorkFlowNumber()
        {
            var department = this.Userinfo1.Department;
            return "OSP" + department + WorkFlowUtil.CreateWorkFlowNumber("OSP" + department).ToString("00000");
        }

        /// <summary>
        /// 设置工作流变量
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <param name="isSubmit"></param>
        bool SetWorkflowVariable(string sWorkflowNo,bool isSubmit)
        {
            WorkflowContext context = WorkflowContext.Current;
            
            OSPCommon comm = new OSPCommon();
            NameCollection manager = OSPCommon.GetTaskUsers("wf_OSP");
            if (null == manager || manager.Count==0)
            {
                DisplayMessage("There are no user in group wf_OSP,\\n please contact IT For further help!");
                return false;
            }
            else
            {
                string sOSPActions = string.Empty;
                bool isAllUpdateToSap = true;//是否已经全部成功更新到SAP
                if (isSubmit)//提交
                {
                    bool isNotNeedApprove = DataEditEdit.IsNotNeedApprove();
                    if (isNotNeedApprove)//不需要工作流审批。
                    {
                        isAllUpdateToSap = DataEditEdit.UpdateToSAP(sWorkflowNo);
                        HiddenFieldIsNotNeedApprove.Value = "1";
                    }
                    else//需要工作流审批。
                    {
                        sOSPActions = OSPCommon.Submit;
                        HiddenFieldIsNotNeedApprove.Value = "0";
                    }
                }
                else//保存
                {
                    sOSPActions = OSPCommon.Save;
                    HiddenFieldIsNotNeedApprove.Value = "0";
                }

                WorkflowDataFields fields = WorkflowContext.Current.DataFields;

                fields["Status"] = CAWorkflowStatus.InProgress;
                fields["Title"] = sWorkflowNo;
                fields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")"; 
                SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(Userinfo1.Applicant.UserAccount);
                fields["ApplicantSPUser"] = ApplicantSPUser;

                context.UpdateWorkflowVariable("OSPActions", sOSPActions);
                context.UpdateWorkflowVariable("EditURL", "/_Layouts/CA/WorkFlows/OSP/EditForm.aspx");
                context.UpdateWorkflowVariable("EditTitle", "Please complete OSP:" + sWorkflowNo);
                context.UpdateWorkflowVariable("ApproveURL", "/_Layouts/CA/WorkFlows/OSP/ApproveForm.aspx");
                context.UpdateWorkflowVariable("ApproveTitle", sWorkflowNo + " needs approve");
                context.UpdateWorkflowVariable("ApproveUser", manager);
                return isAllUpdateToSap;
            }
            
        }

        /// <summary>
        /// 开启工作流
        /// </summary>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        bool StartWorkflow(bool isSubmit)
        {
            bool isOK = DataEditEdit.CheckData();
            if (!isOK)
            {
                return false;
            }
            string sWorkflowNumber = CreateWorkFlowNumber();
            
            DataTable dt = DataEditEdit.GetResultDt(sWorkflowNumber);
            if (null == dt || dt.Rows.Count == 0)
            {
                DisplayMessage("No avaliable data!");
                return false;
            }
            else
            {
                if (SetWorkflowVariable(sWorkflowNumber, isSubmit))//保存
                {
                    OSPCommon comm = new OSPCommon();
                    comm.BatchAddToListByDatatable(dt, "OSPItems");
                    if (HiddenFieldIsNotNeedApprove.Value=="1")//不需要通过审批
                    {
                        WorkflowContext context = WorkflowContext.Current;
                        WorkflowDataFields fields = WorkflowContext.Current.DataFields;

                        context.UpdateWorkflowVariable("OSPActions", OSPCommon.End);
                        fields["Status"] = CAWorkflowStatus.Completed;
                        comm.UpdateOSPSuccess(sWorkflowNumber);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}