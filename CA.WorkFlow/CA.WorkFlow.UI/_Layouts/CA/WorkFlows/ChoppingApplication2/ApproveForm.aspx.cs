using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using System.Data;
using System.Configuration;
using QuickFlow.UI.Controls;
using System.Collections.Specialized;
using QuickFlow;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI.ChoppingApplication2
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataForm1.ControlMode = SPControlMode.Display;
            this.actions.ActionExecuting += new EventHandler<ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            string strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication2/ApproveForm.aspx";
            string strNextTaskTitle = string.Format("{0}'s chopping application needs approval", new SPFieldLookupValue(SPContext.Current.ListItem["Created By"] + "").LookupValue);

            if (string.Equals(e.Action, "Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication2/ApplicantEditForm.aspx";
                strNextTaskTitle = "Please modify your chopping application";
            }

            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

            if ((WorkflowContext.Current.Task.Step == DataForm.Constants.CEOApprove ||
                (WorkflowContext.Current.Task.Step == DataForm.Constants.LegalHeadApprove
                    && string.IsNullOrEmpty(this.DataForm1.CEOAccount)))
                && e.Action == "Approve")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }

            //SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            //WorkflowContext.Current.DataFields["Approvers"] = col;
            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingApproverLoginName);
            //switch (WorkflowContext.Current.Task.Step)
            //{
            //    case CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingDepartHeadApprove:
            //        if (string.IsNullOrEmpty(DataForm1.CFCOAccount) && !string.IsNullOrEmpty(DataForm1.CFCO2Account))
            //        {
            //            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingCFCOAccount, GetDelemanNameCollection(new NameCollection(this.DataForm1.CFCO2Account), Constants.CAModules.ChoppingApplication));
            //        }
            //        else
            //        {
            //            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingCFCOAccount, GetDelemanNameCollection(new NameCollection(this.DataForm1.CFCOAccount), Constants.CAModules.ChoppingApplication));
            //        }
            //        string LegalCounsel = WorkFlowUtil.GetUserInGroup("wf_LegalCounsel");
            //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalCounselAccount, GetDelemanNameCollection(new NameCollection(LegalCounsel), Constants.CAModules.ChoppingApplication));
            //        break;
            //    case CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingCFCOApprove:
            //        if (string.IsNullOrEmpty(DataForm1.CFCOAccount) && !string.IsNullOrEmpty(DataForm1.CFCO2Account))
            //        {
            //            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingManagerAccount, GetDelemanNameCollection(new NameCollection(this.DataForm1.CFCOAccount), Constants.CAModules.ChoppingApplication));
            //        }
            //        else
            //        {
            //            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingManagerAccount, GetDelemanNameCollection(new NameCollection(this.DataForm1.CFCO2Account), Constants.CAModules.ChoppingApplication));
            //        }
            //        string LegalCounsel1 = WorkFlowUtil.GetUserInGroup("wf_LegalCounsel");
            //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalCounselAccount, GetDelemanNameCollection(new NameCollection(LegalCounsel1), Constants.CAModules.ChoppingApplication));
            //        break;
            //    case CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalApprove:
            //        //OtherLegalApprove 无
            //        string LegalHead = UserProfileUtil.GetDepartmentManager("legal");
            //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalHeadAccount, GetDelemanNameCollection(new NameCollection(LegalHead), Constants.CAModules.ChoppingApplication));
            //        break;
            //    case CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalHeadApprovet:
            //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingCEOAccount, GetDelemanNameCollection(new NameCollection(this.DataForm1.CEOAccount), Constants.CAModules.ChoppingApplication));
            //        break;
            //    case CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingManagerApprove:
            //        string LegalCounsel2 = WorkFlowUtil.GetUserInGroup("wf_LegalCounsel");
            //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalCounselAccount, GetDelemanNameCollection(new NameCollection(LegalCounsel2), Constants.CAModules.ChoppingApplication));
            //        break;
            //    case CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingOtherLegalApprove:
            //        string LegalHead1 = UserProfileUtil.GetDepartmentManager("legal");
            //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ChoppingLegalHeadAccount, GetDelemanNameCollection(new NameCollection(LegalHead1), Constants.CAModules.ChoppingApplication));
            //        break;
            //}

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

    }
}
