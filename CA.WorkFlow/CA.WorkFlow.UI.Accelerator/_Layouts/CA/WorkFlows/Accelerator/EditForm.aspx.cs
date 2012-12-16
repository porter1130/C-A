using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.Accelerator
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            Actions.OnClientClick = "return beforeSubmit(this)";
            DataEdit1.bIsEdt = true;
            if (IsPostBack)
            {
                return;
            }
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (!DataEdit1.CheckDate())
            {
                e.Cancel = true;
                return;
            }

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNo = fields["Title"].ToString();
            bool bIsReSubmit = false;
            if (e.Action.Equals("Submit"))
            {
                bIsReSubmit = true;
            }
            bool bIsSuccess = SetWorkflowBaseVariable(bIsReSubmit, sWorkflowNo);
            if (bIsSuccess)
            {
                SetListValue(sWorkflowNo);
            }
            else
            {
                e.Cancel = true;
                return;
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }


        /// <summary>
        /// 设置工作流变量
        /// </summary>
        /// <param name="bIsReSubmit"></param>
        /// <param name="sWorkFlowNO"></param>
        /// <returns></returns>
        bool SetWorkflowBaseVariable(bool bIsReSubmit, string sWorkFlowNO)
        {
            string sTitle = sWorkFlowNO + " needs approve";
            string sURL = "/_Layouts/CA/WorkFlows/Accelerator/ApproveForm.aspx";

            List<NameCollection> listApprovers = GetDMMBDApprover();
            if (listApprovers == null)
            {
                return false;
            }

            WorkflowContext context = WorkflowContext.Current;
            //NameCollection ncCMOApprovers = WorkFlowUtil.GetUsersInGroup("wf_CMO");
            //if (null == ncCMOApprovers || ncCMOApprovers.Count == 0)
            //{
            //    context.UpdateWorkflowVariable("IsSkipCMO", true);
            //}
            NameCollection ncMMCBBSApprovers = GetACCBBSApprovers();
            if (ncMMCBBSApprovers == null)
            {
                return false;
            }

            //context.UpdateWorkflowVariable("IsSubmit", bIsReSubmit);
            //context.UpdateWorkflowVariable("IsReSubmit", bIsReSubmit);
            context.UpdateWorkflowVariable("EditURL", "/_Layouts/CA/WorkFlows/Accelerator/EditForm.aspx");//EditUrl 
            context.UpdateWorkflowVariable("EditTitle", "Please complete Accelerator:" + sWorkFlowNO);//
            context.UpdateWorkflowVariable("DMMTitle", sTitle);//
            context.UpdateWorkflowVariable("DMMURL", sURL);//
            context.UpdateWorkflowVariable("DMMUsers", listApprovers[0]);//
            context.UpdateWorkflowVariable("BDTitle", sTitle);//
            context.UpdateWorkflowVariable("BDURL", sURL);//
            if (listApprovers[1] == null)
            {
                context.UpdateWorkflowVariable("IsSkipBD", true);
            }
            else
            {
                context.UpdateWorkflowVariable("BDUsers", listApprovers[1]);//
            }
            //context.UpdateWorkflowVariable("CMOTitle", sTitle);//
            //context.UpdateWorkflowVariable("CMOURL", sURL);//
            //context.UpdateWorkflowVariable("CMOUsers", ncCMOApprovers);//
            context.UpdateWorkflowVariable("MMCBBSTitle", sTitle);//
            context.UpdateWorkflowVariable("MMCBBSURL", sURL);//
            context.UpdateWorkflowVariable("MMCBBSUsers", ncMMCBBSApprovers);//
            context.UpdateWorkflowVariable("EndTitle", sTitle);//
            context.UpdateWorkflowVariable("EndURL", sURL);//
            return true;
        }

        /// <summary>
        /// 设置list内容
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        void SetListValue(string sWorkflowNo)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            fields["Title"] = sWorkflowNo;
            fields["Status"] = CAWorkflowStatus.InProgress;
            fields["AcceleratorContent"] = DataEdit1.GetAcceleratorType();
            fields["AcceleratorID"] = DataEdit1.GetAcceleratorID();
            fields["FromDate"] = DataEdit1.GetFromDate();
            fields["ToDate"] = DataEdit1.GetToDate();

            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(Userinfo1.Applicant.UserAccount);
            fields["ApplicantSPUser"] = ApplicantSPUser;
            fields["Applicant"] = Userinfo1.Applicant.DisplayName + "(" + Userinfo1.Applicant.UserAccount + ")";
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
            Employee managerEmp = AcceleratorComm.GetDMMApprover(this.Userinfo1.Applicant); //WorkFlowUtil.GetApproverByLevelPAD(this.Userinfo1.Applicant);
            if (managerEmp == null)
            {
                DisplayMessage("此用户没有Level-5或Level-4级的审批用户，无法提交");
                return null;
            }
            string sManager = managerEmp.UserAccount;
            DMMApprover.Add(sManager);
            var deleman = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, WorkFlowUtil.GetModuleIdByListName("AcceleratorWorkflow"));
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
            string delemanBD = WorkFlowUtil.GetDeleman(eBD.UserAccount, WorkFlowUtil.GetModuleIdByListName("AcceleratorWorkflow"));
            if (delemanBD != null)
            {
                BDMApprover.Add(delemanBD);
            }
            listName.Add(BDMApprover);
            return listName;
        }

        /// <summary>
        /// 得到最后一步审批组MMC,BBS里的审批用户
        /// </summary>
        /// <returns></returns>
        NameCollection GetACCBBSApprovers()
        {
            NameCollection ncMMCBBSApprovers = new NameCollection();

            SPGroup groupMMC = WorkFlowUtil.GetUserGroup("wf_ACC");

            if (groupMMC == null || groupMMC.Users.Count == 0)
            {
                DisplayMessage("There are no users in wf_ACC");
                return null;
            }
            foreach (SPUser user in groupMMC.Users)
            {
                string sName = user.LoginName;
                if (user.IsSiteAdmin || sName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                ncMMCBBSApprovers.Add(sName);
                string deleman = WorkFlowUtil.GetDeleman(sName, WorkFlowUtil.GetModuleIdByListName("POTypeChangeWorkflow"));
                if (deleman != null)
                {
                    ncMMCBBSApprovers.Add(deleman);
                }
            }

            if (ncMMCBBSApprovers.Count == 0)
            {
                DisplayMessage("There are no users in wf_ACC");
                return null;
            }

            SPGroup groupBBS = WorkFlowUtil.GetUserGroup("wf_BSS");
            if (groupBBS == null || groupBBS.Users.Count == 0)
            {
                DisplayMessage("There are no users in wf_BSS");
                return null;
            }
            bool bExistBBS = false;
            foreach (SPUser user in groupBBS.Users)
            {
                string sName = user.LoginName;
                if (user.IsSiteAdmin || sName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                ncMMCBBSApprovers.Add(sName);
                string deleman = WorkFlowUtil.GetDeleman(sName, WorkFlowUtil.GetModuleIdByListName("POTypeChangeWorkflow"));
                if (deleman != null)
                {
                    ncMMCBBSApprovers.Add(deleman);
                }
                bExistBBS = true;
            }

            if (!bExistBBS)
            {
                DisplayMessage("There are no users in wf_BSS");
                return null;
            }
            return ncMMCBBSApprovers;
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