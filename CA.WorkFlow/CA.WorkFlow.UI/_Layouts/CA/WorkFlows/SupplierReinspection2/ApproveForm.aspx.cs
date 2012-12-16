using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using QuickFlow.Core;
using CA.WorkFlow.UI.Constants;
namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.SupplierReinspection2
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
            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.SupplierReinspectionApprovers, WorkFlowStep.SupplierReinspectionApproverLoginName);

            List<string> strGroupUser = WorkFlowUtil.UserListInGroup("wf_Finance_SR");
            QuickFlow.NameCollection GroupUsers = new QuickFlow.NameCollection();
            GroupUsers.AddRange(strGroupUser.ToArray());
            WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.SupplierReinspectionFinanceTaskUsers, GetDelemanNameCollection(GroupUsers, Constants.CAModules.SupplierReInspectionCharge));

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}
