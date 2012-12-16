using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CA.SharePoint;
using Microsoft.SharePoint;

using QuickFlow.Core;

using QuickFlow.UI.Controls;
using QuickFlow;
using CA.SharePoint.Utilities.Common;



namespace CA.WorkFlow.UI.NewStoreBudgetApplication2
{
    public partial class NewForm : CAWorkFlowPage
    {
        private string _WorkFlowNumber;

        public string WorkFlowNumber
        {
            get { return _WorkFlowNumber; }
            set { _WorkFlowNumber = value; }
        }

        private string CreateWorkFlowNumber()
        {
            return "SBA_" + WorkFlowUtil.CreateWorkFlowNumber("NewStoreBudgetApplication").ToString("000000");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }
        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartWorkflowButton btnStart = sender as StartWorkflowButton;

            if (string.Equals(btnStart.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
            }
            else
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);

                string msg = DataForm1.validateGeneralInfo();
                if (!string.IsNullOrEmpty(msg))
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }

            //long Intamount = Convert.ToInt64(this.DataForm1.amount);

            float amount = string.IsNullOrEmpty(DataForm1.amount) ? 0 : Convert.ToSingle(DataForm1.amount);


            //string deptHead = WorkFlowUtil.GetEmployeeApprover(DataForm1.Applicant).UserAccount;
            //WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHead", deptHead);


            string strDeptNamemanger = "Construction";
            string strDeptNamemangerName = UserProfileUtil.GetDepartmentManager(strDeptNamemanger);
            //1
            //WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHead", strDeptNamemangerName);


            string CFOName = WorkFlowUtil.GetUserInGroup("wf_CFO");
            //2
            //WorkflowContext.Current.UpdateWorkflowVariable("CFOApprovalUser", CFOName);

            //WorkflowContext.Current.UpdateWorkflowVariable("Amount", Intamount);
            WorkflowContext.Current.UpdateWorkflowVariable("Amount", amount);

            string CEOName = WorkFlowUtil.GetUserInGroup("wf_CEO");
            //3
            //WorkflowContext.Current.UpdateWorkflowVariable("CEOApprovalUser", CEOName);

            List<string> strGroupUser = WorkFlowUtil.UserListInGroup("wf_Finance_BA");
            if (strGroupUser.Count == 0)
            {
                //Don
                DisplayMessage("Unable to submit the application. There is no user in wf_Finance_BA group. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            NameCollection GroupUsers = new NameCollection();
            GroupUsers.AddRange(strGroupUser.ToArray());
            //4
           // WorkflowContext.Current.UpdateWorkflowVariable("FinanceTaskUsers", GroupUsers);


            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetConstructionHeadApproval, strDeptNamemangerName);
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCFOApproval, CFOName);
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCEOApproval, CEOName);
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetFinanceTask, GroupUsers.JoinString(","));
            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetDepartmentHead, GetDelemanNameCollection(new NameCollection(strDeptNamemangerName), Constants.CAModules.NewStoreBudgetApplication));
            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCFOApprovalUser, GetDelemanNameCollection(new NameCollection(CFOName), Constants.CAModules.NewStoreBudgetApplication));
            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetCEOApprovalUser, GetDelemanNameCollection(new NameCollection(CEOName), Constants.CAModules.NewStoreBudgetApplication));
            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreBudgetFinanceTaskUsers, GetDelemanNameCollection(GroupUsers, Constants.CAModules.NewStoreBudgetApplication));


            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskTitle", DataForm1.Applicant.DisplayName + "'s New Store Construction Budget request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CFOApprovalTaskTitle", DataForm1.Applicant.DisplayName + "'s New Store Construction Budget request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CEOApprovalTaskTitle", DataForm1.Applicant.DisplayName + "'s New Store Construction Budget request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceUserTaskTitle", "Please complete New Store Construction Budget request for " + DataForm1.Applicant.DisplayName);

            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkflowNumber"] = this.WorkFlowNumber;
            DataForm1.WorkflowNumber = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["FileName"] = DataForm1.Submit();

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);

        }
        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {

        }

    }
}
