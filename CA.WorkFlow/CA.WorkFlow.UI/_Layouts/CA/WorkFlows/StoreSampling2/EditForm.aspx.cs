using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using CodeArt.SharePoint.CamlQuery;
using System.Data;
using CA.WorkFlow.UI.Code;
using Microsoft.SharePoint.WebControls;
using QuickFlow;
using CA.WorkFlow.UI.Constants;

namespace CA.WorkFlow.UI.StoreSampling2
{
    public partial class EditForm : CAWorkFlowPage
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";

            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);

            this.btnSave.Click += new EventHandler(btnSave_Click);
            if (WorkflowContext.ContextInitialized)
            {
                switch (WorkflowContext.Current.Task.Step)
                {
                    case "BuyerApprove":
                        btnSave.Visible = false;
                        this.ControlMode = SPControlMode.Display;
                        break;
                    case "StoreManagerApprove":
                        btnSave.Visible = false;
                        this.ControlMode = SPControlMode.Display;
                        break;
                    case "AreaManagerApprove":
                        btnSave.Visible = false;
                        this.ControlMode = SPControlMode.Display;
                        break;
                    case "BSSHeadApprove":
                        btnSave.Visible = false;
                        this.ControlMode = SPControlMode.Display;
                        break;
                    case "BSSTeamApprove":
                        btnSave.Visible = false;
                        this.ControlMode = SPControlMode.Display;
                        break;
                    case "FinanceConfirm":
                        btnSave.Visible = false;
                        this.ControlMode = SPControlMode.Display;
                        break;
                    case "StoreAdminSubmit":
                        PanelComm.Visible = false;
                        break;
                }
            }
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            WorkflowContext curContext = WorkflowContext.Current;
            WorkflowDataFields fields = curContext.DataFields;
            fields["Status"] = "In Progress";

            ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);
            SPList stores = sps.GetList("Stores");
            QueryField field = new QueryField("Store Number", false);
            SPListItemCollection coll = sps.Query(stores, field.Equal(fields["Store Number"] + ""), 1);

            switch (curContext.Task.Step)
            {
                case "BuyerApprove":
                    if (coll.Count > 0)
                    {
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.StoreSamplingStoreManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(coll[0]["Manager"] + "").LookupValue), Constants.CAModules.StoreSampling));
                    }
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApproverLoginName);
                    break;
                case "StoreManagerApprove":
                    if (coll.Count > 0)
                    {
                        WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.StoreSamplingAreaManagerApproveUser, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(coll[0]["AreaManager"] + "").LookupValue), Constants.CAModules.StoreSampling));
                    }
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApproverLoginName);
                    break;
                case "AreaManagerApprove":
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    NameCollection bSSHead = WorkFlowUtil.GetUsersInGroup("wf_BSSHead");
                    WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.StoreSamplingBSSHeadGroup, GetDelemanNameCollection(bSSHead, Constants.CAModules.StoreSampling));
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApproverLoginName);
                    break;
                case "BSSHeadApprove":
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    NameCollection BBSTeamUser = new NameCollection();
                    List<string> lst = WorkFlowUtil.UserListInGroup("wf_NSC");
                    BBSTeamUser.AddRange(lst.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.StoreSamplingBSSTeamAccount, GetDelemanNameCollection(BBSTeamUser, Constants.CAModules.StoreSampling));
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApproverLoginName);
                    break;
                case "BSSTeamApprove":
                    if(e.Action=="Approve")
                    {
                        if (string.IsNullOrEmpty(((DropDownList)DataForm1.FindControl("ddlCostCenter")).SelectedValue)&&string.IsNullOrEmpty(DataForm1.TextBoxCostCenter))
                        {
                            e.Cancel = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please supply a cost center.');", true);
                            DataForm1.PickedBy = new SPFieldLookupValue(SPContext.Current.ListItem["Picked by"] + "").LookupValue;
                            ((CADateTimeControl)DataForm1.FindControl("CADateTime1")).Enabled = false;
                            return;
                        }

                        if (string.IsNullOrEmpty(((DropDownList)DataForm1.FindControl("ddlCostCenter")).SelectedValue))
                        {
                            fields["Cost Center"] = DataForm1.TextBoxCostCenter;
                        }
                        else
                        {
                            fields["Cost Center"] = ((DropDownList)DataForm1.FindControl("ddlCostCenter")).SelectedValue;
                        }
                     }
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    NameCollection finance_SS = WorkFlowUtil.GetUsersInGroup("wf_Finance_SS");
                    WorkflowContext.Current.UpdateWorkflowVariable(WorkFlowStep.StoreSamplingFinanceGroup, GetDelemanNameCollection(finance_SS, Constants.CAModules.StoreSampling));
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApproverLoginName);
                    break;
                case "FinanceConfirm":
                  //  if (e.Action == "Approve")
                    if(e.Action=="Confirm")
                    {
                        fields["Status"] = "Completed";
                        fields["Completed Date"] = System.DateTime.Now.ToShortDateString();
                       
                    }
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingApproverLoginName);
                    break;
                case "StoreAdminSubmit":

                    if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
                    {
                        WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                        return;
                    }
                    string msg = DataForm1.Validate();
                    if (!string.IsNullOrEmpty(msg))
                    {
                        DisplayMessage(msg);
                        e.Cancel = true;
                        return;
                    }

                    string passTo = DataForm1.PickedBy;
                    fields["Store Number"] = ((DropDownList)DataForm1.FindControl("ddlStoreNumber")).SelectedValue; //((TextBox)DataForm1.FindControl("txtStoreNumber")).Text;
                    fields["Cost Center"] = string.Empty;
                    fields["Issued to"] = ((DropDownList)DataForm1.FindControl("ddlIssuedTo")).SelectedValue;
                    if (!string.IsNullOrEmpty(passTo))
                    {
                        fields["Picked by"] = EnsureUser(passTo);
                    }
                    //fields["Comments"] = ((TextBox)DataForm1.FindControl("txtComments")).Text;
                    fields["FileName"] = DataForm1.Submit();
                    //curContext.UpdateWorkflowVariable("Buyer", passTo);
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreSamplingBuyer, GetDelemanNameCollection(new NameCollection(passTo), Constants.CAModules.StoreSampling));
                    break;
                
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();
            //base.Back();
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        void SaveForm()
        {
            if (WorkflowContext.Current.Task.Step == "StoreAdminSubmit")
            {
                string passTo = DataForm1.PickedBy;

                SPListItem item = SPContext.Current.ListItem;
                item["Store Number"] = ((DropDownList)DataForm1.FindControl("ddlStoreNumber")).SelectedValue; //((TextBox)DataForm1.FindControl("txtStoreNumber")).Text;
                item["Cost Center"] = string.Empty; 

                item["Issued to"] = ((DropDownList)DataForm1.FindControl("ddlIssuedTo")).SelectedValue;
                item["Actual Quantity"] = ((TextBox)DataForm1.FindControl("txtActualQuantity")).Text;
                if (!string.IsNullOrEmpty(passTo))
                {
                    item["Picked by"] = EnsureUser(passTo);
                }
                item["Picked Time"] = ((CADateTimeControl)DataForm1.FindControl("CADateTime1")).SelectedDate.ToShortDateString();
                item["FileName"] = DataForm1.Submit();
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
            }
        }

        

        void PushToReport()
        {
            //WorkflowContext curContext = WorkflowContext.Current;
            //WorkflowDataFields fields = curContext.DataFields;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreSamplingReport);

            //foreach (DataRow row in DataForm1)
            //{
            //    SPListItem item = list.Items.Add();
            //    //item["WorkflowNumber"] = fields["WorkflowNumber"];
            //    //item["Store Number"] = fields["Store Number"];
            //    item["WorkflowNumber"] = DataForm1.WorkflowNumber;
            //    item["Store Number"] = DataForm1.StoreNumber;
            //    item["Actual Quantity"] = row["ActualQuantity"];
            //    item["Picked"] = row["Picked"];
            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //}
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
