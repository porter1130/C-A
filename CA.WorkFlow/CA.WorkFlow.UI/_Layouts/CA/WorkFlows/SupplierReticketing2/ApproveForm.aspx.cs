using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using QuickFlow.Core;
using CA.WorkFlow.UI.Constants;
using QuickFlow;
namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.SupplierReticketing2
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            //SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            //WorkflowContext.Current.DataFields["Approvers"] = col;
            //添加审批人
            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.SupplierReticketingApprovers, WorkFlowStep.SupplierReticketingApproverLoginName);
            switch (WorkflowContext.Current.Task.Step)
            {
                case WorkFlowStep.SupplierReticketingBuyingApprove:
                    WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.SupplierReticketingDepartmentHead, GetDelemanNameCollection(new NameCollection(WorkFlowUtil.GetUserInGroup("wf_BSSHead")), Constants.CAModules.SupplierReTicketingCharge));
                    break;
                case WorkFlowStep.SupplierReticketingDepartmentHeadApproval:
                    List<string> strGroupUser = WorkFlowUtil.UserListInGroup("wf_Finance_SR");
                    NameCollection GroupUsers = new NameCollection();
                    GroupUsers.AddRange(strGroupUser.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.SupplierReticketingFinanceTaskUsers, GetDelemanNameCollection(GroupUsers, Constants.CAModules.SupplierReTicketingCharge));
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}
