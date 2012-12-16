using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.PurchaseRequestGeneral
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            Actions.OnClientClick = "return beforeSubmit(this)";
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("Approve"))//审批通过
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                decimal TotalCost = GetTotalCost();
                if (TotalCost == 0)
                {
                    DisplayMessage("Rate or Cost error!");
                    e.Cancel = true;
                    return;
                }

                string levelType = "Contract Approval Limits";
                string sCurrentManager = CurrentEmployee.UserAccount;
                var quota = WorkFlowUtil.GetQuota(CurrentEmployee.UserAccount, levelType);///得到当前审批人的审批金额 


                WorkflowContext context = WorkflowContext.Current;
                if (TotalCost > quota)///当前审批者不够审批此金额。
                {
                    context.UpdateWorkflowVariable("IsApproveAgin", true);//

                    NameCollection manager = new NameCollection();
                    Employee managerEmp = WorkFlowUtil.GetNextApprover(sCurrentManager);
                    if (managerEmp == null && !WorkflowPerson.IsCEO(sCurrentManager))
                    {
                        DisplayMessage("The manager is not set in the system.");
                        e.Cancel = true;
                        return;
                    }
                    manager.Add(managerEmp.UserAccount);

                    string sDelePerson = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, WorkFlowUtil.GetModuleIdByListName("PurchaseRequestGeneral")); //查找代理人
                    if (sDelePerson != null)
                    {
                        manager.Add(sDelePerson);
                    }
                    context.UpdateWorkflowVariable("ApproveUsers", manager);
                }
                else//进入到财务审批
                {
                    context.UpdateWorkflowVariable("IsApproveAgin", false);//
                }

                fields["Approvers"] = ReturnAllApprovers(sCurrentManager);
                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", sCurrentManager);
            }
            else
            {
                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                string sWorkflowNo = fields["Title"].ToString();
                context.UpdateWorkflowVariable("EditTitle", "Please resubmit Purchase Ruequest-General:" + sWorkflowNo);//
            }
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        /// <summary>
        /// 得到计算了总金额（转换成RMB）
        /// </summary>
        /// <returns></returns>
        decimal GetTotalCost()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sRate = fields["Rate"] == null ? "0" : fields["Rate"].ToString();
            string sCost = fields["Cost"] == null ? "0" : fields["Cost"].ToString();
            decimal dRate = 0;
            decimal dCost = 0;
            decimal.TryParse(sRate, out dRate);
            decimal.TryParse(sCost, out dCost);
            decimal TotalCost = dRate * dCost;//
            return TotalCost;
        }
    }
}