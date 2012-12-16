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
using CA.SharePoint.Utilities.Common;
using System.Text;

namespace CA.WorkFlow.UI.POTypeChange
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
            DataEdit1.SCurrentEmployee = CurrentEmployee.UserAccount; 
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
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
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
           // string.Format("/CA/MyTasks.aspx?TaskId={0}"
            RedirectToTask();
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "href", "<script>location.href('/CA/MyTasks.aspx')</script>");
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
            string sWorkFlow = HiddenFieldID.Value.Trim();
            if (sWorkFlow.Length == 0)
            {
              // string department = this.Userinfo1.Department;
               sWorkFlow = "POTC"  + WorkFlowUtil.CreateWorkFlowNumber("POTC").ToString("00000");
                HiddenFieldID.Value = sWorkFlow;
            }
            return sWorkFlow;
        }
        #region 设置工作流
        /// <summary>
        /// 开启工作流
        /// </summary>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        bool StartWorkflow(bool isSubmit)
        {
            bool isSuccess = true;
            bool isOK = DataEdit1.CheckData();//验证子项
            if (!isOK)
            {
                return false;
            }
            string sWorkflowNumber = CreateWorkFlowNumber();

            DataTable dt = DataEdit1.GetResultDt(sWorkflowNumber);
            if (null == dt || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                bool isSetVariable= SetWorkflowVariable(sWorkflowNumber, isSubmit);
                if (!isSetVariable)
                {
                    return false;
                }

                bool isNeedApprove = DataEdit1.IsNeedApprove();
                string sSate = CAWorkflowStatus.InProgress;
                Common comm = new Common();
                comm.BatchAddToListByDatatable(dt, "POTypeChangeItems");
                if (isSubmit&&!isNeedApprove)//不需要审批
                {
                    List<string> lisSucPONOs = new List<string>();
                    bool isAllUpdated = DataEdit1.UpdateToSAP(sWorkflowNumber, ref lisSucPONOs);
                    if (isAllUpdated)//全部更新成功。
                    {
                        comm.UpdateOSPSuccess(sWorkflowNumber);
                        sSate = CAWorkflowStatus.Completed;
                        //string sPONO = comm.GetPONOs(dt);
                    }
                    else
                    {
                        WorkflowContext context = WorkflowContext.Current;
                        context.UpdateWorkflowVariable("IsSubmit", false);
                        context.UpdateWorkflowVariable("IsResubmit", true);
                        context.UpdateWorkflowVariable("EditTitle", "Please complete POTypeChange:" + sWorkflowNumber);
                    }
                    if (lisSucPONOs.Count > 0)
                    {
                        comm.SendNoticeMail(lisSucPONOs.ToString(), CurrentEmployee.DisplayName, sWorkflowNumber);
                    }
                }

                SetListValue(sWorkflowNumber, sSate);
                return isSuccess;
            }
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
            context.UpdateWorkflowVariable("IsSubmit", isSubmit);
            context.UpdateWorkflowVariable("IsResubmit", false);
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

        /// <summary>
        /// 设置list内容
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        void SetListValue(string sWorkflowNo,string sState)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            fields["Title"] = sWorkflowNo;
            fields["Status"] = sState;// CAWorkflowStatus.InProgress;

            string sUserAccount = CurrentEmployee.UserAccount;
            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(sUserAccount);
            fields["ApplicantSPUser"] = ApplicantSPUser;
            fields["Applicant"] = CurrentEmployee.DisplayName + "(" + sUserAccount + ")";
            fields["Approvers"] = ReturnAllApprovers(sUserAccount);
            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", sUserAccount);
        }


        #endregion
    }
}