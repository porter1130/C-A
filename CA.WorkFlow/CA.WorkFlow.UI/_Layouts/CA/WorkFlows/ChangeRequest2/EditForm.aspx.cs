﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint;
using System.Data;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Configuration;
using CA.WorkFlow.UI.Constants;
using QuickFlow;
namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.ChangeRequest2
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";

            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);

            if (WorkflowContext.ContextInitialized)
            {
                if(WorkflowContext.Current.Task.Step == "EmployeeSubmit")
                {
                    PanelComm.Visible = false;
                }
                else if (WorkflowContext.Current.Task.Step == "ITAppManagerGroupSupplies")
                {
                    actions.Attributes["onclick"] = "return validForm();";
                }
            }
           
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            
            if (WorkflowContext.ContextInitialized)
            {
                WorkflowContext curContext = WorkflowContext.Current;
                WorkflowDataFields fields = curContext.DataFields;
                fields["Status"] = "In Progress";

                switch (WorkflowContext.Current.Task.Step)
                {
                    case "ITAppManagerGroupSupplies":
                        var crNumber = ((TextBox)DataForm1.FindControl("txtChangeRequestNumber")).Text;
                        if (string.IsNullOrEmpty(crNumber))
                        {
                            base.Script.Alert("please supply a change request number.");
                            e.Cancel = true;
                            return;
                        }
                        
                        fields["ChangeRequestNumber"] = crNumber;
                        PushToReport();
                        //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                        AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.ChangeRequestApprovers, WorkFlowStep.ChangeRequestApproverLoginName);
                        break;
                    case "ITAppManagerSubmit"://有问题
                        fields["ChangeRequestNumber"] = ((TextBox)DataForm1.FindControl("txtChangeRequestNumber")).Text;
                        break;
                    case "EmployeeSubmit":
                        if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
                        {
                            WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                            return;
                        }
                        if (string.IsNullOrEmpty(((TextBox)DataForm1.FindControl("txtSubject")).Text))
                        {
                            e.Cancel = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please supply a Subject .');", true);
                            return;
                        }

                        fields["Priority"] = ((DropDownList)DataForm1.FindControl("ddlPriority")).SelectedValue;
                        fields["Area"] = ((DropDownList)DataForm1.FindControl("ddlArea")).SelectedValue;
                        fields["System"] = ((DropDownList)DataForm1.FindControl("ddlSystem")).SelectedValue;
                        fields["RequirementType"] = ((DropDownList)DataForm1.FindControl("ddlRequirementType")).SelectedItem.Text;
                        fields["Subject"] = ((TextBox)DataForm1.FindControl("txtSubject")).Text;
                        fields["Description"] = ((TextBox)DataForm1.FindControl("txtDescription")).Text;
                        fields["BusinessLogic"] = ((TextBox)DataForm1.FindControl("txtBusinessLogic")).Text;

                        string isNew = "Yes";
                        if (((DropDownList)DataForm1.FindControl("ddlRequirementType")).SelectedValue == "Bug fix")
                        {
                            isNew = "No";
                        }
                        curContext.UpdateWorkflowVariable("IsRequestNew", isNew);
                        break;
                    case "BusinessManagerGroupApprove":
                        //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                        AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.ChangeRequestApprovers, WorkFlowStep.ChangeRequestApproverLoginName);
                        break;
                    case "BusinessManagerGroupApprove2" :
                        if (e.Action == "Approve" && ((DropDownList)DataForm1.FindControl("ddlRequirementType")).SelectedValue == "Bug fix")
                        {
                            fields["Status"] = "Completed";
                        }
                       // fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                        AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.ChangeRequestApprovers, WorkFlowStep.ChangeRequestApproverLoginName);
                       break;
                    case "ITHeadApprove":
                       // fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                       AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.ChangeRequestApprovers, WorkFlowStep.ChangeRequestApproverLoginName);
                       break;
                    case "ITAppManagerGroupExecutes":
                       AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.ChangeRequestApprovers, WorkFlowStep.ChangeRequestApproverLoginName);
                       break;
                    case "ITHeadApprove2":

                        if (e.Action == "Approve")
                        {
                            fields["Status"] = "Completed";
                            List<string> mailList = new List<string>();
                            List<SPUser> users = WorkFlowUtil.GetSPUsersInGroup("wf_ITApplicationManager");
                            foreach (SPUser user in users)
                            {
                                mailList.Add(user.Email);
                            }

                            StringDictionary dict = new StringDictionary();
                            dict.Add("to", string.Join(";", mailList.ToArray()));
                            dict.Add("subject", "Workflow Notification");

                            string mcontent = @"An IT change request has been approved. Please view the detail by clicking <a href='"
                                + SPContext.Current.Web.Url + "/_layouts/CA/WorkFlows/ChangeRequest2/DisplayForm.aspx?List="
                                + SPContext.Current.ListId.ToString()
                                + "&ID="
                                + SPContext.Current.ListItem.ID
                                + "'>here</a>.";

                            SPUtility.SendEmail(SPContext.Current.Web, dict, mcontent);

                        }
                       // fields["Approvers"] = WorkFlowUtil.GetApproversValue();

                        AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, WorkFlowStep.ChangeRequestApprovers, WorkFlowStep.ChangeRequestApproverLoginName);
                       break;


                }
                //添加审批人  ChangeRequestITAppManagerGroupExecutes少此
                switch (WorkflowContext.Current.Task.Step) 
                {
                    
                    case WorkFlowStep.ChangeRequestBusinessManagerGroupApprove:
                        string ITManager = UserProfileUtil.GetDepartmentManager("IT");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITHead, GetDelemanNameCollection(new QuickFlow.NameCollection(ITManager), Constants.CAModules.ChangeRequest));
                        break;
                    case WorkFlowStep.ChangeRequestITHeadApprove:
                        QuickFlow.NameCollection iTApplicationManager = WorkFlowUtil.GetUsersInGroup("wf_ITApplicationManager");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITAppManagerGroup, GetDelemanNameCollection(iTApplicationManager, Constants.CAModules.ChangeRequest));
                        break;
                    case WorkFlowStep.ChangeRequestEmployeeTests:
                        QuickFlow.NameCollection iTApplicationManager1 = WorkFlowUtil.GetUsersInGroup("wf_ITApplicationManager");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITAppManagerGroup, GetDelemanNameCollection(iTApplicationManager1, Constants.CAModules.ChangeRequest));
                        break;
                    case WorkFlowStep.ChangeRequestITAppManagerGroupSupplies:
                        string ITManager1 = UserProfileUtil.GetDepartmentManager("IT");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITHead, GetDelemanNameCollection(new QuickFlow.NameCollection(ITManager1), Constants.CAModules.ChangeRequest));
                        QuickFlow.NameCollection businessManager = WorkFlowUtil.GetUsersInGroup("wf_BusinessManager");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestBusinessManagerGroup, GetDelemanNameCollection(businessManager, Constants.CAModules.ChangeRequest));
                        break;
                    case WorkFlowStep.ChangeRequestBusinessManagerGroupApprove2:
                        QuickFlow.NameCollection iTApplicationManager2 = WorkFlowUtil.GetUsersInGroup("wf_ITApplicationManager");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITAppManagerGroup, GetDelemanNameCollection(iTApplicationManager2, Constants.CAModules.ChangeRequest));
                        string ITManager2 = UserProfileUtil.GetDepartmentManager("IT");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITHead, GetDelemanNameCollection(new QuickFlow.NameCollection(ITManager2), Constants.CAModules.ChangeRequest));
                        break;
                    case WorkFlowStep.ChangeRequestITHeadApprove2:
                         QuickFlow.NameCollection iTApplicationManager3 = WorkFlowUtil.GetUsersInGroup("wf_ITApplicationManager");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITAppManagerGroup, GetDelemanNameCollection(iTApplicationManager3, Constants.CAModules.ChangeRequest));
                        break;
                    case WorkFlowStep.ChangeRequestEmployeeSubmit:
                        string ITManager3 = UserProfileUtil.GetDepartmentManager("IT");
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.ChangeRequestITHead, GetDelemanNameCollection(new QuickFlow.NameCollection(ITManager3), Constants.CAModules.ChangeRequest));
                        break;
                }

            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            //Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            RedirectToTask();
        }


        void PushToReport()
        {
           SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                WorkflowContext curContext = WorkflowContext.Current;
                WorkflowDataFields fields = curContext.DataFields;

                ISharePointService sps = ServiceFactory.GetSharePointService(true);
                SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ChangeRequestReport);

                string rootweburl = ConfigurationManager.AppSettings["rootweburl"] + "";
                if (string.IsNullOrEmpty(rootweburl))
                {
                    rootweburl = "https://portal.c-and-a.cn";
                }

                SPFieldUrlValue uv = new SPFieldUrlValue();

                //"https://cnshsps.cnaidc.cn/WorkFlowCenter/_layouts/CA/WorkFlows/ChangeRequest/DisplayForm.aspx?List="

                uv.Url = rootweburl + "/WorkFlowCenter/_layouts/CA/WorkFlows/ChangeRequest2/DisplayForm.aspx?List=" 
                    + SPContext.Current.ListId.ToString()
                    + "&ID="
                    + SPContext.Current.ListItem.ID;
                uv.Description = fields["WorkflowNumber"] + "";

                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                        {
                            SPListItem item = list.Items.Add();
                            item["WorkflowNumber"] = uv;
                            item["Change request number"] = fields["ChangeRequestNumber"];
                            item["Raised by"] = EnsureUser(new SPFieldLookupValue(fields["Created By"] + "").LookupValue);//SPContext.Current.Web.EnsureUser(new SPFieldLookupValue(fields["Created By"] + "").LookupValue);
                            item["System"] = fields["System"];
                            item["Business Logic"] = fields["BusinessLogic"];
                            item["Subject"] = fields["Subject"];
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
                
            }
             );
        }

        public static SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            }
             );

            return user;
        }


       
    }
}
