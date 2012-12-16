using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.UI.Controls;
using QuickFlow.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using CA.SharePoint;
using CA.WorkFlow.UI.PADChangeRequest;


namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
        protected void Page_Load(object sender, EventArgs e)
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true))
            {
               
                RedirectToTask();
            }
            this.Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            switch (WorkflowContext.Current.Step)
            {
                case "ManagerApprove":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (WorkFlowUtil.GetApproverIsLastPAD(WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString())))))
                        {
                            if (sapcommonpad.SapUpdatePAD(fields["PONumber"].ToString(), Convert.ToDateTime(fields["NewPAD"]).ToString("yyyy-MM-dd")))
                            {
                                WorkflowContext.Current.UpdateWorkflowVariable("isOnlyApp", true);
                                WorkflowContext.Current.UpdateWorkflowVariable("updateResult", true);

                                fields["CurrManager"] = WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                                fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                                if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                                }

                                fields["Status"] = CAWorkflowStatus.Completed;
                            }
                            else
                            {
                                DisplayMessage("更新SAP数据失败，请联系IT人员或稍后审批.Error:" + sapcommonpad.ErrorMsg);
                                e.Cancel = true;
                                return;
                            }
                        }
                        else
                        {
                            QuickFlow.NameCollection SuperApproveUser = new QuickFlow.NameCollection();
                            var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString());
                            var supmanagerEmp = WorkFlowUtil.GetApproverIgnoreRight(WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant)));

                            if (supmanagerEmp == null)
                            {
                                DisplayMessage("此用户没有Level-4级以上的审批用户，无法提交");
                                e.Cancel = true;
                                return;
                            }
                            // ApproveUser.Add("ca\\function.head2");
                            SuperApproveUser.Add(supmanagerEmp.UserAccount);
                            var deleman = WorkFlowUtil.GetDeleman(supmanagerEmp.UserAccount, "127");
                            if (deleman != null)
                            {
                                SuperApproveUser.Add(deleman);
                                //fields["Delegates"] = deleman;
                            }
                            WorkflowContext.Current.UpdateWorkflowVariable("isOnlyApp", false);
                            WorkflowContext.Current.UpdateWorkflowVariable("secApproveU", SuperApproveUser);
                            WorkflowContext.Current.UpdateWorkflowVariable("SuperManagerT", "PAD Change Request needs to Approve");
                            WorkflowContext.Current.UpdateWorkflowVariable("approveUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/ApproveForm.aspx");

                            fields["CurrManager"] = WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                            fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                            if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                            }

                            fields["Status"] = CAWorkflowStatus.InProgress;
                        }
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Completed;
                    }
                    break;
                case "SuperManagerApprove":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (sapcommonpad.SapUpdatePAD(fields["PONumber"].ToString(), Convert.ToDateTime(fields["NewPAD"]).ToString("yyyy-MM-dd")))
                        {
                            WorkflowContext.Current.UpdateWorkflowVariable("updateResult", true);

                            var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString());
                            var supmanagerEmp = WorkFlowUtil.GetApproverIgnoreRight(WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant)));
                            fields["CurrManager"] = supmanagerEmp.UserAccount;
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                            if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                            }

                            fields["Status"] = CAWorkflowStatus.Completed;
                        }
                        else
                        {
                            DisplayMessage("更新SAP数据失败，请联系IT人员或稍后审批.Error:" + sapcommonpad.ErrorMsg);
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        fields["Status"] = CAWorkflowStatus.Completed;
                    }
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}