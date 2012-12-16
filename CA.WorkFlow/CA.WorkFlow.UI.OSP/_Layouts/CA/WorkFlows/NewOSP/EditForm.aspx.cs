using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using System.Data;
using Microsoft.SharePoint;
using QuickFlow;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.NewOSP
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
            List<NameCollection> listApprovers = GetDMMBDApprover();// OSPCommon.GetTaskUsers("wf_OSP");
            if (null == listApprovers || listApprovers.Count == 0)
            {
                return false;
            }

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
                string sOSPActions = string.Empty;
                if (isSubmit)//是提交
                {
                    bool isNotNeedApprove = DataEdit1.IsNotNeedApprove();
                    if (isNotNeedApprove)//不需要审批
                    {
                        bool isAllUpdated = DataEdit1.UpdateToSAP(sWorkflowNO);
                        if (isAllUpdated)//全部更新成功。
                        {
                            sOSPActions = OSPCommon.End;
                            fields["Status"] = CAWorkflowStatus.Completed;
                        }
                        else
                        {
                            isSuccess = false;
                        }
                    }
                    else//需要审批
                    {
                        sOSPActions = OSPCommon.Submit;
                    }
                }
                else//save agin
                {
                    sOSPActions = OSPCommon.Save;
                }
                fields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
                SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(Userinfo1.Applicant.UserAccount);
                fields["ApplicantSPUser"] = ApplicantSPUser;

                context.UpdateWorkflowVariable("DMMApprover", listApprovers[0]);
                if (listApprovers[1] == null)
                {
                    context.UpdateWorkflowVariable("IsSkipBD", true);
                    fields["IsSkipBD"] = true;
                }
                else
                {
                    context.UpdateWorkflowVariable("IsSkipBD", false);
                    fields["IsSkipBD"] = false;
                    context.UpdateWorkflowVariable("BDApprover", listApprovers[1]);//
                }
                context.UpdateWorkflowVariable("OSPActions", sOSPActions);

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

        /// <summary>
        /// 得到DMM,BD审批用户(DMM的用户级别为4以上则不用BD审批)
        /// </summary>
        /// <returns></returns>
        List<QuickFlow.NameCollection> GetDMMBDApprover()
        {
            List<QuickFlow.NameCollection> listName = new List<NameCollection>();
            ///找DMM审批人
            QuickFlow.NameCollection DMMApprover = new QuickFlow.NameCollection();
            Employee managerEmp = OSPCommon.GetDMMApprover(this.Userinfo1.Applicant); //WorkFlowUtil.GetApproverByLevelPAD(this.Userinfo1.Applicant);
            if (managerEmp == null)
            {
                DisplayMessage("此用户没有Level-5或Level-4级的审批用户，无法提交");
                return null;
            }
            string sManager = managerEmp.UserAccount;
            DMMApprover.Add(sManager);
            var deleman = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, WorkFlowUtil.GetModuleIdByListName("OSPWorkflow"));
            if (deleman != null)
            {
                DMMApprover.Add(deleman);
            }
            listName.Add(DMMApprover);

            //查找BD审批人
            QuickFlow.NameCollection BDMApprover = new QuickFlow.NameCollection();
            Employee eBD = WorkFlowUtil.GetNextApprover(sManager);
            if (null == eBD)
            {
                DisplayMessage("Can not find next approver for " + sManager);
                return null;
            }

            int iLevel = GetLevel(eBD.JobLevel.AsString());
            if (iLevel < 4)
            {
                listName.Add(null);
                return listName;//BD的用户级别为4以上则不用BD审批。
            }


            BDMApprover.Add(eBD.UserAccount);
            //string delemanBD = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, CA.WorkFlow.UI.Constants.CAModules.PADChangeRequest);
            string delemanBD = WorkFlowUtil.GetDeleman(eBD.UserAccount, WorkFlowUtil.GetModuleIdByListName("OSPWorkflow"));
            if (delemanBD != null)
            {
                BDMApprover.Add(delemanBD);
            }
            listName.Add(BDMApprover);
            return listName;
        }


        int GetLevel(string sJobLevel)
        {
            int iLevel = 0;
            string[] sBDLevelArr = sJobLevel.Split('-');
            if (sBDLevelArr[1] != null)
            {
                int.TryParse(sBDLevelArr[1], out iLevel);
            }
            return iLevel;
        }

    }
}