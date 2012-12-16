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

namespace CA.WorkFlow.UI.NewOSP
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
            List<NameCollection> listApprovers = GetDMMBDApprover();// OSPCommon.GetTaskUsers("wf_OSP");
            if (null == listApprovers || listApprovers.Count == 0)
            {
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
                context.UpdateWorkflowVariable("EditURL", "/_Layouts/CA/WorkFlows/NewOSP/EditForm.aspx");
                context.UpdateWorkflowVariable("EditTitle", "Please complete OSP:" + sWorkflowNo);
                context.UpdateWorkflowVariable("DMMURL", "/_Layouts/CA/WorkFlows/NewOSP/ApproveForm.aspx");
                context.UpdateWorkflowVariable("DMMTitle", sWorkflowNo + " needs approve");
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
                context.UpdateWorkflowVariable("BDTitle", sWorkflowNo + " needs approve");
                context.UpdateWorkflowVariable("BDURL", "/_Layouts/CA/WorkFlows/NewOSP/ApproveForm.aspx");
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