using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.NewStoreBudgetApplication2
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
            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetApproverLoginName);
            if (WorkflowContext.Current.Task.Step == "FinanceTask")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
            else
            {
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }
            switch (WorkflowContext.Current.Task.Step)
            {
                case CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetConstructionHeadApproval:
                    string CFOName = WorkFlowUtil.GetUserInGroup("wf_CFO");
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCFOApprovalUser, GetDelemanNameCollection(new QuickFlow.NameCollection(CFOName), Constants.CAModules.NewStoreBudgetApplication));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCFOApproval:
                    string CEOName = WorkFlowUtil.GetUserInGroup("wf_CEO");
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCEOApprovalUser, GetDelemanNameCollection(new QuickFlow.NameCollection(CEOName), Constants.CAModules.NewStoreBudgetApplication));
                    List<string> strGroupUser = WorkFlowUtil.UserListInGroup("wf_Finance_BA");
                    QuickFlow.NameCollection GroupUsers = new QuickFlow.NameCollection();
                    GroupUsers.AddRange(strGroupUser.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetFinanceTaskUsers, GetDelemanNameCollection(GroupUsers, Constants.CAModules.NewStoreBudgetApplication));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCEOApproval:
                     List<string> strGroupUser1 = WorkFlowUtil.UserListInGroup("wf_Finance_BA");
                    QuickFlow.NameCollection GroupUsers1 = new QuickFlow.NameCollection();
                    GroupUsers1.AddRange(strGroupUser1.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetFinanceTaskUsers, GetDelemanNameCollection(GroupUsers1, Constants.CAModules.NewStoreBudgetApplication));
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
