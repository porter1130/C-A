using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;
using QuickFlow;

namespace CA.WorkFlow.UI.Accelerator
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
            Userinfo1.Action = "NEW";
        }

        void StartWorkflowButtonSave_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void StartWorkflowButtonSubmit_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void StartWorkflowButtonSave_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DataEdit1.CheckDate())
            {
                e.Cancel = true;
                return;
            }
            string sWorkflowNO = CreateWorkFlowNumber();

            bool bIsSuccess = SetWorkflowBaseVariable(false, sWorkflowNO);
            if (bIsSuccess)
            {
                SetListValue(sWorkflowNO);
            }
            else
            {
                e.Cancel = true;
                return;
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void StartWorkflowButtonSubmit_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DataEdit1.CheckDate())
            {
                e.Cancel = true;
                return;
            }
            string sWorkflowNO = CreateWorkFlowNumber();

            bool bIsSuccess = SetWorkflowBaseVariable(true, sWorkflowNO);
            if (bIsSuccess)
            {
                SetListValue(sWorkflowNO);
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
        /// <param name="bIsSubmit"></param>
        /// <param name="sWorkFlowNO"></param>
        bool SetWorkflowBaseVariable(bool bIsSubmit,string sWorkFlowNO)
        {
            string sTitle = sWorkFlowNO + " needs approve";
            string sURL = "/_Layouts/CA/WorkFlows/Accelerator/ApproveForm.aspx";

            List<NameCollection> listApprovers = GetDMMBDApprover();
            if (listApprovers == null)
            {
                return false;
            }
            
            WorkflowContext context = WorkflowContext.Current;

            NameCollection ncMMCBBSApprovers = GetACCBBSApprovers();
            if (ncMMCBBSApprovers == null)
            {
                return false;
            }

            context.UpdateWorkflowVariable("IsSubmit", bIsSubmit);
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
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                fields["IsSkipBD"] = true;
            }
            else
            {
                context.UpdateWorkflowVariable("BDUsers", listApprovers[1]);//
            }
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

            string sUserAccount = Userinfo1.Applicant.UserAccount;
            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(sUserAccount);
            fields["ApplicantSPUser"] = ApplicantSPUser;
            fields["Applicant"] = Userinfo1.Applicant.DisplayName + "(" + sUserAccount + ")";
            fields["Approvers"] = ReturnAllApprovers(sUserAccount);
            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", sUserAccount);
        }

        /// <summary>
        /// 生成工作流编号
        /// </summary>
        /// <returns></returns>
        string CreateWorkFlowNumber()
        {
            string sWorkFlowNo = HiddenFieldNO.Value;
            if (sWorkFlowNo.Trim().Length == 0)
            {
                var department = this.Userinfo1.Department;
                sWorkFlowNo = "ACC" + WorkFlowUtil.CreateWorkFlowNumber("ACC").ToString("00000");
                HiddenFieldNO.Value = sWorkFlowNo;
            }
            return sWorkFlowNo;
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
            Employee managerEmp =AcceleratorComm.GetDMMApprover(this.Userinfo1.Applicant); //WorkFlowUtil.GetApproverByLevelPAD(this.Userinfo1.Applicant);
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
            //string delemanBD = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, CA.WorkFlow.UI.Constants.CAModules.PADChangeRequest);
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
                string sName=user.LoginName;
                if (user.IsSiteAdmin|| sName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                ncMMCBBSApprovers.Add(sName);
                string deleman = WorkFlowUtil.GetDeleman(sName, WorkFlowUtil.GetModuleIdByListName("AcceleratorWorkflow"));
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
                string deleman = WorkFlowUtil.GetDeleman(sName, WorkFlowUtil.GetModuleIdByListName("AcceleratorWorkflow"));
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