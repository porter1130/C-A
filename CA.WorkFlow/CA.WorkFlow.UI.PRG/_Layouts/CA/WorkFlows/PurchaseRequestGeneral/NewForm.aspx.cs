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

namespace CA.WorkFlow.UI.PurchaseRequestGeneral
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
            string sWorkflowNO = HiddenFieldWorkflowNumber.Value;
            if (string.IsNullOrEmpty(sWorkflowNO))
            {
                sWorkflowNO = CreateWorkFlowNumber();
                HiddenFieldWorkflowNumber.Value = sWorkflowNO;
            }

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
            string sWorkflowNO = HiddenFieldWorkflowNumber.Value;
            if (string.IsNullOrEmpty(sWorkflowNO))
            {
                sWorkflowNO = CreateWorkFlowNumber();
                HiddenFieldWorkflowNumber.Value = sWorkflowNO;
            }

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
            NameCollection ncFinance = WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
            if (null == ncFinance || ncFinance.Count == 0)
            {
                DisplayMessage("There are no users in wf_FinanceConfirm");
                return false;
            }
            Employee employee = WorkFlowUtil.GetNextApprover(this.Userinfo1.Applicant);/// WorkFlowUtil.GetUsersInGroup("wf_FinanceConfirm");
            if (null == employee)
            {
                DisplayMessage(this.Userinfo1.Applicant.UserAccount+" has no manager in System!");
                return false;
            }
            NameCollection manager = new NameCollection();
            manager.Add(employee.UserAccount);
            string sDelePerson = WorkFlowUtil.GetDeleman(employee.UserAccount, WorkFlowUtil.GetModuleIdByListName("PurchaseRequestGeneral")); //查找代理人
            if (sDelePerson != null)
            {
                manager.Add(sDelePerson);
            }

            WorkflowContext context = WorkflowContext.Current;
            context.UpdateWorkflowVariable("IsSubmit", bIsSubmit);
            context.UpdateWorkflowVariable("IsSaveAgin", false);
            context.UpdateWorkflowVariable("EditUrl", "/_Layouts/CA/WorkFlows/PurchaseRequestGeneral/EditForm.aspx");//EditUrl 
            context.UpdateWorkflowVariable("EditTitle", "Please complete Purchase Ruequest-General:" + sWorkFlowNO);//
            context.UpdateWorkflowVariable("ApproveURL", "/_Layouts/CA/WorkFlows/PurchaseRequestGeneral/ApproveForm.aspx");//ApproveURL
            context.UpdateWorkflowVariable("ApproveTitle", sWorkFlowNO + " needs approve");//ApproveTitle
            context.UpdateWorkflowVariable("ApproveUsers", manager);
            context.UpdateWorkflowVariable("ComfirmURL", "/_Layouts/CA/WorkFlows/PurchaseRequestGeneral/ConfirmForm.aspx");//ComfirmURL
            context.UpdateWorkflowVariable("ConfirmTitle", sWorkFlowNO + " needs confirm"); //ConfirmTitle
            context.UpdateWorkflowVariable("IsApproveAgin", false);
            context.UpdateWorkflowVariable("ConfirmUsers", ncFinance);
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
            fields["Currency"] = DataEdit1.GetCurrency();
            fields["Rate"] = DataEdit1.GetRate();
            fields["IsAnnualBudget"] = DataEdit1.IsAnnualBudget();
            fields["IsNeedBid"] = DataEdit1.IsNeedBid();
            fields["PeriodFrom"] = DataEdit1.PeriodFrom();
            fields["PeriodTo"] = DataEdit1.PeriodTo();
            fields["IsIncurred"] = DataEdit1.IsIncurred();
            fields["IncurredFrom"] = DataEdit1.IncurredFrom();
            fields["IncurredTo"] = DataEdit1.IncurredTo();

            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(Userinfo1.Applicant.UserAccount);
            fields["ApplicantSPUser"] = ApplicantSPUser;
        }

        /// <summary>
        /// 生成工作流编号
        /// </summary>
        /// <returns></returns>
        string CreateWorkFlowNumber()
        {
            var department = this.Userinfo1.Department;
            return "PRG" + department + WorkFlowUtil.CreateWorkFlowNumber("PRG" + department).ToString("00000");
        }

    }
}