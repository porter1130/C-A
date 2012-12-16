using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using System.Data;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Text;

namespace CA.WorkFlow.UI.POTypeChange
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            Actions.OnClientClick = "return beforeSubmit(this)";
            DataEdit1.SCurrentEmployee = CurrentEmployee.UserAccount;
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
        ///tss =CS-Nom
        ///cs= norm-CS
        
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

                bool isSetVariable = SetWorkflowVariable(sWorkflowNO, isSubmit);
                if (!isSetVariable)
                {
                    return false;
                }

                Common comm = new Common();
                comm.DeleteData(sWorkflowNO);//删除老数据
                comm.BatchAddToListByDatatable(dt, "POTypeChangeItems");//添加新数据
                WorkflowContext context = WorkflowContext.Current;
                string sSate = CAWorkflowStatus.InProgress;
              
                bool IsNeedApprove = DataEdit1.IsNeedApprove();
                if (isSubmit && !IsNeedApprove)//不需要审批
                {
                    List<string> lisSucPONOs = new List<string>();
                    bool isAllUpdated = DataEdit1.UpdateToSAP(sWorkflowNO, ref lisSucPONOs);
                    if (isAllUpdated)//全部更新成功。
                    {
                        comm.UpdateOSPSuccess(sWorkflowNO);
                        sSate = CAWorkflowStatus.Completed;
                    }
                    else
                    {
                        context.UpdateWorkflowVariable("IsSubmit", false);
                        context.UpdateWorkflowVariable("IsResubmit", false);
                        context.UpdateWorkflowVariable("EditTitle", "Please complete POTypeChange:" + sWorkflowNO);
                    }
                    if (lisSucPONOs.Count > 0)
                    {
                        comm.SendNoticeMail(lisSucPONOs.ToString(), CurrentEmployee.DisplayName, sWorkflowNO);
                    }
                }
                SetListValue(sSate);
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
           // DataEdit1.SetType(fields["ActionType"].ToString());
        }

        /// <summary>
        /// 设置list内容
        /// </summary>
        /// <param name="sState"></param>
        void SetListValue(string sState)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            //fields["Title"] = sWorkflowNo;
            fields["Status"] = sState;// CAWorkflowStatus.InProgress;

            string sUserAccount = CurrentEmployee.UserAccount;
            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(sUserAccount);
            fields["ApplicantSPUser"] = ApplicantSPUser;
            fields["Applicant"] = CurrentEmployee.DisplayName + "(" + sUserAccount + ")";
            fields["Approvers"] = ReturnAllApprovers(sUserAccount);
            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", sUserAccount);

        }



        /// <summary>
        /// 设置工作流变量
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <param name="isSubmit"></param>
       bool SetWorkflowVariable(string sWorkflowNo, bool isSubmit)
        {
            WorkflowContext context = WorkflowContext.Current;
            List<QuickFlow.NameCollection> listName = GetDMMApprover();
            if (null == listName)
            {
                return false;
            }
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            //fields["Status"] = CAWorkflowStatus.InProgress;
            //fields["Title"] = sWorkflowNo;
            //fields["Applicant"] = Userinfo1.Applicant.DisplayName + "(" + CurrentEmployee.UserAccount + ")";//xu
            //SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(Userinfo1.Applicant.UserAccount);
            //fields["ApplicantSPUser"] = ApplicantSPUser;

            context.UpdateWorkflowVariable("IsSubmit", isSubmit);
            context.UpdateWorkflowVariable("IsResubmit", isSubmit);
            context.UpdateWorkflowVariable("IsNeedApprove", DataEdit1.IsNeedApprove());//是否需要工作流审批  xu
            context.UpdateWorkflowVariable("EditURL", "/_Layouts/CA/WorkFlows/POTypeChange/EditForm.aspx");
            context.UpdateWorkflowVariable("EditTitle", "Please complete POTypeChange:" + sWorkflowNo);

            context.UpdateWorkflowVariable("DMMURL", "/_Layouts/CA/WorkFlows/POTypeChange/ApproveForm.aspx");
            context.UpdateWorkflowVariable("DMMTitle", sWorkflowNo + " needs approve");
            context.UpdateWorkflowVariable("DMMUsers", listName[0]);

            context.UpdateWorkflowVariable("BDURL", "/_Layouts/CA/WorkFlows/POTypeChange/ApproveForm.aspx");
            context.UpdateWorkflowVariable("BDTitle", sWorkflowNo + " needs approve");
            context.UpdateWorkflowVariable("BDUsers", listName[1]);
            return true;
        }

        /// <summary>
        /// 得到DMM,BD审批用户
        /// </summary>
        /// <returns></returns>
        List<QuickFlow.NameCollection> GetDMMApprover()
        {
            List<QuickFlow.NameCollection> listName = new List<NameCollection>();
            ///找DMM审批人
            QuickFlow.NameCollection DMMApprover = new QuickFlow.NameCollection();
            Employee managerEmp = WorkFlowUtil.GetApproverByLevelPAD(CurrentEmployee);
            if (managerEmp == null)
            {
                DisplayMessage("此用户没有Level-5级以上的审批用户，无法提交");
                return null;
            }
            string sManager = managerEmp.UserAccount;
            DMMApprover.Add(sManager);
            var deleman = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, WorkFlowUtil.GetModuleIdByListName("POTypeChangeWorkflow"));
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
            BDMApprover.Add(eBD.UserAccount);
            string delemanBD = WorkFlowUtil.GetDeleman(eBD.UserAccount, WorkFlowUtil.GetModuleIdByListName("POTypeChangeWorkflow"));
            if (delemanBD != null)
            {
                BDMApprover.Add(delemanBD);
            }
            listName.Add(BDMApprover);
            return listName;
        }

    }
}