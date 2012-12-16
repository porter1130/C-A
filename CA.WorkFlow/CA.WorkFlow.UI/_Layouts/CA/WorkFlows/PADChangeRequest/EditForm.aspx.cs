using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.UI.Controls;
using QuickFlow.Core;
using CA.SharePoint;
using CA.WorkFlow.UI.PADChangeRequest;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest
{
    public partial class EditForm : CAWorkFlowPage
    {
        SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.lblCurrentPAD.Text = WorkflowContext.Current.DataFields["CurrentPAD"].ToString();
            this.lblPONumber.Text = WorkflowContext.Current.DataFields["PONumber"].ToString();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (e.Action.Equals("Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("isSave", true);
                fields["Status"] = CAWorkflowStatus.Pending;
                WorkflowContext.Current.UpdateWorkflowVariable("completeTaskT", "PAD Change Request needs to Submit");

                WorkflowContext.Current.UpdateWorkflowVariable("editUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/EditForm.aspx");
            }
            else
            {
                if (Convert.ToDateTime(fields["CurrentPAD"].ToString()) < Convert.ToDateTime(fields["NewPAD"].ToString()))
                {
                    if (sapcommonpad.SapUpdatePAD(fields["PONumber"].ToString(), Convert.ToDateTime(fields["NewPAD"].ToString()).ToString("yyyy-MM-dd")))
                    {
                        WorkflowContext.Current.UpdateWorkflowVariable("isSave", false);
                        WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", true);
                        WorkflowContext.Current.UpdateWorkflowVariable("updateResult", true);
                        WorkflowContext.Current.UpdateWorkflowVariable("isFree", true);
                        fields["Status"] = CAWorkflowStatus.Completed;
                    }
                    else
                    {
                        DisplayMessage("更新SAP数据失败，请联系IT人员或稍后提交.Error:" + sapcommonpad.ErrorMsg);
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    QuickFlow.NameCollection ApproveUser = new QuickFlow.NameCollection();
                    var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString());
                    
                    var managerEmp = WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant));
                    //// ApproveUser.Add("ca\\function.head2");
                    ApproveUser.Add(managerEmp.UserAccount);
                    var deleman = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, "127");
                    if (deleman != null)
                    {
                        ApproveUser.Add(deleman);
                        //fields["Delegates"] = deleman;
                    }
                    WorkflowContext.Current.UpdateWorkflowVariable("isSave", false);
                    WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", true);
                    WorkflowContext.Current.UpdateWorkflowVariable("isFree", false);
                    WorkflowContext.Current.UpdateWorkflowVariable("firstApproveUser", ApproveUser);
                    WorkflowContext.Current.UpdateWorkflowVariable("ManagerApproveT", "PAD Change Request needs to Approve");
                    WorkflowContext.Current.UpdateWorkflowVariable("approveUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/ApproveForm.aspx");
                    fields["Status"] = CAWorkflowStatus.InProgress;
                }
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToSaveTask();
        }
    }
}