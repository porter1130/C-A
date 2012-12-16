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
using CodeArt.SharePoint.CamlQuery;
using System.Text;

namespace CA.WorkFlow.UI.Accelerator
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);

            this.Actions.OnClientClick = "return beforeSubmit(this);";
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (!e.Action.Equals("Approve"))//审批拒绝
            {
                WorkflowContext context = WorkflowContext.Current;
                SendNoticeMail("Rejected");
                fields["Status"] = CAWorkflowStatus.Completed;
            }
            else if (WorkflowContext.Current.Step == "BD" || (WorkflowContext.Current.Step == "DMM"&&fields["IsSkipBD"].AsString() == "True"))
            {
                SendMMCNoticeMail();
            }
            else if (WorkflowContext.Current.Step == "MMCBBS")
            {
                WorkflowContext context = WorkflowContext.Current;
                NameCollection nc = new NameCollection();
                if (WorkFlowUtil.IsInGroup(CurrentEmployee.UserAccount, "wf_BSS"))
                {
                    // DeleteTask(fields["ID"].AsString(), "wf_BSS");
                    nc = GetApproverInGroup("wf_ACC");
                }
                else if (WorkFlowUtil.IsInGroup(CurrentEmployee.UserAccount, "wf_ACC"))
                {
                    //DeleteTask(fields["ID"].AsString(), "wf_MMC");
                    nc = GetApproverInGroup("wf_BSS");
                }
                context.UpdateWorkflowVariable("EndUsers", nc);
            }
            else if (WorkflowContext.Current.Step == "End")//是最后一步的最后一个人审批。
            {
                WorkflowContext context = WorkflowContext.Current;
                SendNoticeMail("Approved");
                fields["Status"] = CAWorkflowStatus.Completed;
            }

            string sCurrentManager = CurrentEmployee.UserAccount;
            fields["Approvers"] = ReturnAllApprovers(sCurrentManager);
            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", sCurrentManager);
            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sStatus"></param>
        void SendNoticeMail(string sStatus)
        {
            try
            {
                string sName=CurrentEmployee.DisplayName;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                AcceleratorComm comm = new AcceleratorComm();
                string sApprovers = fields["Approvers"].AsString();
                

                comm.SendMail(sApprovers, sStatus, fields["Title"].ToString(), sName);
            }
            catch (Exception e)
            {
                CommonUtil.logError("Accelerator sent mail failed:"+e.ToString());
            }
        }

        
        /// <summary>
        /// 得到组里审批用户
        /// </summary>
        /// <returns></returns>
        NameCollection GetApproverInGroup(string sGroupName)
        {
            NameCollection ncMMCBBSApprovers = new NameCollection();
            SPGroup groupMMC = WorkFlowUtil.GetUserGroup(sGroupName);

            if (groupMMC == null || groupMMC.Users.Count == 0)
            {
                DisplayMessage("There are no users in"+sGroupName);
                return null;
            }
            foreach (SPUser user in groupMMC.Users)
            {
                string sName=user.LoginName;
                if (user.IsSiteAdmin|| sName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                ncMMCBBSApprovers.Add(sName);
                string deleman = WorkFlowUtil.GetDeleman(sName, WorkFlowUtil.GetModuleIdByListName("AcceleratorWorkflow"));
                if (deleman != null)
                {
                    ncMMCBBSApprovers.Add(deleman);
                }
            }
           // ncMMCBBSApprovers.Add("cnashsptest\\bss1");
            return ncMMCBBSApprovers;
        }

        /// <summary>
        /// 得到MMC组里的用户
        /// </summary>
        /// <returns></returns>
        string GetMMCUsers()
        {
            SPGroup groupMMC = WorkFlowUtil.GetUserGroup("wf_MMC");
            StringBuilder sb = new StringBuilder();
            foreach (SPUser user in groupMMC.Users)
            {
                string sAccount = user.LoginName;
                if (user.IsSiteAdmin || sAccount.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                if (sb.Length > 0)
                {
                    sb.Append(";");
                }
                sb.Append(sAccount);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 删除Tasks组里的其它人
        /// </summary>
        /// <param name="sItemId"></param>
        /// <param name="sGroup"></param>
        void DeleteTask(string sItemId,string sGroup)
        {
            CamlExpression exp = null;
            SPGroup spGroup = WorkFlowUtil.GetUserGroup("wf_BSS");
            SPGroup spGroupMMC = WorkFlowUtil.GetUserGroup("wf_MMC");
            ////(), "wf_BSS");
            ////        nc = GetApproverInGroup("wf_MMC")

            QueryField qWorkflowItemId = new QueryField("WorkflowItemId", false);
            QueryField qfWorkflowListId = new QueryField("WorkflowListId", false);
            QueryField qfStatus = new QueryField("Status", false);//Status
            exp = WorkFlowUtil.LinkAnd(exp, qWorkflowItemId.Equal(sItemId));///AcceleratorWorkflow 里的记录ID相等
            exp = WorkFlowUtil.LinkAnd(exp, qfWorkflowListId.Equal(SPContext.Current.ListId));//ListID相等
            exp = WorkFlowUtil.LinkAnd(exp, qfStatus.NotEqual("Completed"));//Completed没有完成的task

            QueryField qAssignedTo = new QueryField("AssignedTo", false);//task的执行人 
            CamlExpression ceOR = null;
            foreach (SPUser user in spGroup.Users)
            {
                string sLoginName = user.LoginName;
                if (sLoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase) || sLoginName.Equals(CurrentEmployee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                ceOR = WorkFlowUtil.LinkOr(ceOR, qAssignedTo.Equal(sLoginName));
                CommonUtil.logError(sLoginName);
            }

            foreach (SPUser user in spGroupMMC.Users)
            {
                string sLoginName = user.LoginName;
                if (sLoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase) || sLoginName.Equals(CurrentEmployee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                CommonUtil.logError(sLoginName);
                ceOR = WorkFlowUtil.LinkOr(ceOR, qAssignedTo.Equal(sLoginName));
            }

            exp = WorkFlowUtil.LinkAnd(exp, ceOR);

            SPQuery q = new SPQuery();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists["Tasks"];
                        SPQuery query = new SPQuery
                        {
                            Query = CamlBuilder.Where(list, exp)
                        };

                        CommonUtil.logError(query.Query);
                        SPListItemCollection splic = list.GetItems(query);
                        web.AllowUnsafeUpdates = true;
                        if (null != splic)
                        {
                            for (int i = 0; i < splic.Count; i++)
                            {
                                CommonUtil.logError("删除Task: " + splic[i]["AssignedTo"].ToString()+"   " + splic[i]["Title"].AsString());
                                splic[i].Delete();
                                //splic[i].Delete();
                            }
                        }
                    }
                }
            });

        }

        /// <summary>
        /// 发送邮件给MMC组里的用户，
        /// </summary>
        /// <param name="sStatus"></param>
        void SendMMCNoticeMail()
        {
            try
            {
                string sName = CurrentEmployee.DisplayName;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                AcceleratorComm comm = new AcceleratorComm();
                string sApplicant = fields["Applicant"].AsString();
                string sMMCUsers = GetMMCUsers();//MMC组的人不需要参与审批，但是能够收到一个邮件能够查看记录
                comm.SendMMCMail(sMMCUsers, sApplicant.Split('(')[0], fields["ID"].ToString(), sName);//, sStatus, fields["Title"].ToString(), sName);
            }
            catch (Exception e)
            {
                CommonUtil.logError("AcceleratorMMC sent mail failed:" + e.ToString());
            }
        }
    }
}