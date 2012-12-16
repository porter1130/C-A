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



namespace CA.WorkFlow.UI.PurchaseRequestGeneral
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
            DataEdit1.ShowWorkFlowNo();
            //SetApplicant();
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
            bool bIsSubmit = false;
            if (e.Action.Equals("Submit"))
            {
                bIsSubmit = true;
            }
            bool bIsSuccess = SetWorkflowBaseVariable(bIsSubmit, sWorkflowNo);
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
        /// <param name="bIsSubmit"></param>
        /// <param name="sWorkFlowNO"></param>
        bool SetWorkflowBaseVariable(bool bIsSubmit, string sWorkFlowNO)
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
                DisplayMessage(this.Userinfo1.Applicant.UserAccount + " has no manager in System!");
                return false;
            }

            NameCollection manager = new NameCollection();
            manager.Add(employee.UserAccount);
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

        ///// <summary>
        ///// 设置申请人。
        ///// </summary>
        //void SetApplicant()
        //{ 
        //    WorkflowDataFields fields = WorkflowContext.Current.DataFields;
        //    if(null!=fields["Applicant"])
        //    {
        //        Employee emp = WorkFlowUtil.GetEmployee(fields["Applicant"].AsString());
        //        if(null!= emp)
        //        {
        //            string sApplicant = emp.DisplayName;
        //            Userinfo1.SetApplicant(sApplicant);
        //        }
        //    }   
        //}

    }
}