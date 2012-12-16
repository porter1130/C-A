using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CA.SharePoint;
using CA.SharePoint.Utilities.Common;
using QuickFlow.Core;
using Microsoft.SharePoint;
using System.Collections.Generic;
namespace CA.WorkFlow.UI.NewSupplierCreation2
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.actions.Attributes.Add("onclick", "return CheckComments();");
            hdBody.Value = this.body.ClientID;
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            if (WorkflowContext.Current.Step.ToString() == "BSSTeam")
            {
                btnSave.Visible = true;
            }
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (WorkflowContext.Current.Step.ToString() == "BSSTeam")
            {
                string strStatus = DataForm1.Status.ToString();
                if (strStatus != "Factory Assessment Failed" && strStatus != "Contract Signed & System Setup OK")
                {
                    e.Cancel = true;
                    Page.RegisterStartupScript("msg", "<script>alert('Can not confirm with selected status!')</script>");
                    ((HtmlTableRow)DataForm1.FindControl("trShow")).Style.Add("display", "");
                    ((HtmlTableRow)DataForm1.FindControl("updateList")).Style.Add("display", "");
                    return;
                }
                else
                { 
                    SPListItem item = SPContext.Current.ListItem;
                    WorkflowContext.Current.DataFields["StatusList"] = item["StatusList"] + "<br>" + DataForm1.Status.ToString() + "     " + DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            WorkflowContext.Current.DataFields["Flag"] = "Approve";
            if (WorkflowContext.Current.Step == "BSSTeam" && e.Action == "Confirm")
            {
                WorkflowContext.Current.DataFields["RequestStatus"] = "Completed";
            }
            else
            {
                WorkflowContext.Current.DataFields["RequestStatus"] = "In Progress";
            }
            //SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            //WorkflowContext.Current.DataFields["Approvers"] = col;
            AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationApproverLoginName);

            switch (WorkflowContext.Current.Task.Step)
            {
                case CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationDivisionManagerApprove:
                    QuickFlow.NameCollection DirectorUser = new QuickFlow.NameCollection();
                    Employee employee = WorkFlowUtil.GetEmployeeApproverInDept(WorkFlowUtil.GetEmployeeApproverInDept(DataForm1.Applicant, true, true), true, false);
                    if (employee != null)
                    {
                        string strhead = employee.UserAccount;
                        if (!string.IsNullOrEmpty(strhead))
                        {
                            DirectorUser.Add(strhead);
                        }
                    }
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationBuyDirector, GetDelemanNameCollection(DirectorUser, Constants.CAModules.NewSupplierCreation));
                    
                     QuickFlow.NameCollection HeadUser1 = new QuickFlow.NameCollection();
                    List<string> lst1 = WorkFlowUtil.UserListInGroup("wf_CommercialHead");
                    HeadUser1.AddRange(lst1.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationHeader, GetDelemanNameCollection(HeadUser1, Constants.CAModules.NewSupplierCreation));
                   
                     QuickFlow.NameCollection BBSTeamUser1 = new QuickFlow.NameCollection();
                     List<string> lst2 = WorkFlowUtil.UserListInGroup("wf_BSSTeam");
                    BBSTeamUser1.AddRange(lst2.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationBBSTeamAccount, GetDelemanNameCollection(BBSTeamUser1, Constants.CAModules.NewSupplierCreation));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationBuyingDirector:
                    QuickFlow.NameCollection HeadUser = new QuickFlow.NameCollection();
                    List<string> lst = WorkFlowUtil.UserListInGroup("wf_CommercialHead");
                    HeadUser.AddRange(lst.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationHeader, GetDelemanNameCollection(HeadUser, Constants.CAModules.NewSupplierCreation));

                    QuickFlow.NameCollection BBSTeamUser3 = new QuickFlow.NameCollection();
                    List<string> lst3 = WorkFlowUtil.UserListInGroup("wf_BSSTeam");
                    BBSTeamUser3.AddRange(lst3.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationBBSTeamAccount, GetDelemanNameCollection(BBSTeamUser3, Constants.CAModules.NewSupplierCreation));

                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationCommercialHeader:
                    QuickFlow.NameCollection BBSTeamUser = new QuickFlow.NameCollection();
                    lst = WorkFlowUtil.UserListInGroup("wf_BSSTeam");
                    BBSTeamUser.AddRange(lst.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.SupplierCreationBBSTeamAccount, GetDelemanNameCollection(BBSTeamUser, Constants.CAModules.NewSupplierCreation));
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            //if ((item["StatusList"] + "").Replace("&amp;", "&").Contains(DataForm1.Status.ToString()))
            //{
            //    base.Back();
            //    return;
            //}

            if (string.IsNullOrEmpty(item["StatusList"] + ""))
            {
                item["StatusList"] = DataForm1.Status.ToString() + "      " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                item["StatusList"] = item["StatusList"] + "<br>" + DataForm1.Status.ToString() + "     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            item["Status"] = DataForm1.Status;

            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        item.Web.AllowUnsafeUpdates = true;
                        item.Update();
                        item.Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occured while updating the items");
            }

            //item.Web.AllowUnsafeUpdates = true;
            //item.Update();
            base.Back();
        }
    }
}
