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
using CA.WorkFlow.UI.Code;
using CodeArt.SharePoint.CamlQuery;
using QuickFlow;
namespace CA.WorkFlow.UI.StoreMaintenance2
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton2_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton2_Executed);

            DataForm1.BindRpt2Display();
        }

        //提交
        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg = DataForm1.Validate();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            SaveFormToWf();
            WorkflowContext.Current.DataFields["Status"] = "In Progress";
            WorkflowContext.Current.UpdateWorkflowVariable("IsSubmit", "Yes");

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            //base.Back();
            RedirectToTask();
        }

        //保存
        void StartWorkflowButton2_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            SaveFormToWf();
            WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            WorkflowContext.Current.UpdateWorkflowVariable("IsSubmit", "No");
            //base.Back();
            
        }

        void StartWorkflowButton2_Executed(object sender, EventArgs e)
        {
            //base.Back();
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        void SaveFormToWf()
        {
            WorkflowContext curContext = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string taskTitle = SPContext.Current.Web.CurrentUser.Name + "'s Store Maintenance";

            fields["Type1"] = ((DropDownList)DataForm1.FindControl("ddlType1")).SelectedValue;
            //fields["CostCenter"] = ((TextBox)DataForm1.FindControl("txtCostCenter")).Text;
            fields["CostCenter"] = ((DropDownList)DataForm1.FindControl("ddlCostCenter")).SelectedValue;
            fields["BudgetApproved"] = ((DropDownList)DataForm1.FindControl("ddlBudgetApproved")).SelectedValue;
            fields["BudgetValue"] = ((TextBox)DataForm1.FindControl("txtBudgetValue")).Text;

            DataForm1.WorkflowNumber = CreateWorkflowNumber();
            fields["WorkflowNumber"] = DataForm1.WorkflowNumber;
            this.DataForm1.Rpt1ToDt1();
            SaveAsso1();
            //SaveAsso2();

            
            curContext.UpdateWorkflowVariable("RequestSubmitTitle", "please complete store maintenance");
            curContext.UpdateWorkflowVariable("ConstructionHeadApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("SOHeadApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("CMManagerGroupReviewTitle", "please update store maintenance");
            curContext.UpdateWorkflowVariable("CMManagerGroupOrdersTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("StoreManagerApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("StoreManagerEvaluatesTitle", taskTitle + " needs evaluate");
            curContext.UpdateWorkflowVariable("AreaManagerApproveTitle", taskTitle + " needs approval");

            SPListItemCollection stores = GetSPColl("Stores", "Cost Center", fields["CostCenter"] + "", 1);
            //1
            //curContext.UpdateWorkflowVariable("CMManagerGroup", "wf_ConstructionMaintenance");

            NameCollection constructionMaintenance = WorkFlowUtil.GetUsersInGroup("wf_ConstructionMaintenance");
            System.Text.StringBuilder strStepAndUsers = new System.Text.StringBuilder();
            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceCMManagerGroupReview, constructionMaintenance.JoinString(","));
            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceCMManagerGroup, GetDelemanNameCollection(constructionMaintenance, Constants.CAModules.StoreMaintenance));
            if (stores.Count > 0)
            {
                if (!string.IsNullOrEmpty(stores[0]["Manager"] + ""))
                {
                    //2
                   // curContext.UpdateWorkflowVariable("StoreManager",
                                  //  new SPFieldLookupValue(stores[0]["Manager"] + "").LookupValue);

                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceStoreManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores[0]["Manager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                    
                    strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceStoreManagerApprove, new SPFieldLookupValue(stores[0]["Manager"] + "").LookupValue);
                    strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceStoreManagerEvaluates, new SPFieldLookupValue(stores[0]["Manager"] + "").LookupValue);
                }
                if (!string.IsNullOrEmpty(stores[0]["AreaManager"] + ""))
                {
                    //3
                  //  curContext.UpdateWorkflowVariable("AreaManager",
                        //new SPFieldLookupValue(stores[0]["AreaManager"] + "").LookupValue);

                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceAreaManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores[0]["AreaManager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                    strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceAreaManagerApprove, new SPFieldLookupValue(stores[0]["AreaManager"] + "").LookupValue);
                }
            }

            string departmentManager = UserProfileUtil.GetDepartmentManager("Construction");
            if (string.IsNullOrEmpty(departmentManager))
            {
                //4
               // curContext.UpdateWorkflowVariable("ConstructionHead",departmentManager);
                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceConstructionHead, GetDelemanNameCollection(new NameCollection(departmentManager), Constants.CAModules.StoreMaintenance));
                strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceConstructionHeadApprove, departmentManager);
            }

            departmentManager = UserProfileUtil.GetDepartmentManager("Store Operations");
            if (string.IsNullOrEmpty(departmentManager))
            {
                //5
                //curContext.UpdateWorkflowVariable("SOHead",departmentManager);

                WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceSOHead, GetDelemanNameCollection(new NameCollection(departmentManager), Constants.CAModules.StoreMaintenance));
                strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceSOHeadApprove, departmentManager);
            }

            strStepAndUsers.AppendFormat("{0}:{1};", CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceCMManagerGroupOrders, constructionMaintenance.JoinString(","));

            WorkflowContext.Current.DataFields["WorkFlowStepsAndUsers"] = strStepAndUsers.ToString();

            


            //curContext.UpdateWorkflowVariable("SOTeamGroup", "wf_StoreOperationTeam");
            
            
        }

        private string CreateWorkflowNumber()
        {
            return "SM_" + WorkFlowUtil.CreateWorkFlowNumber("StoreMaintenance").ToString("000000");
        }

        void SaveAsso1()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreMaintenanceItems1.ToString());

            foreach (DataRow row in this.DataForm1.Asso1.Rows)
            {
                SPListItem item = list.Items.Add();
                item["WorkflowNumber"] = DataForm1.WorkflowNumber;
                item["Seq"] = row["Seq"];
                item["Reason"] = row["Reason"];
                item["Description"] = row["Description"];
                item["Remark"] = row["Remark"];
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

        //void SaveAsso2()
        //{
        //    ISharePointService sps = ServiceFactory.GetSharePointService(true);

        //    SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreMaintenanceItems2.ToString());

        //    foreach (DataRow row in this.DataForm1.Asso2.Rows)
        //    {
        //        SPListItem item = list.Items.Add();
        //        item["WorkflowNumber"] = DataForm1.WorkflowNumber;
        //        item["Seq"] = row["Seq"];
        //        item["Reason"] = row["Reason"];
        //        item["Price"] = row["Price"];
        //        item["Quantity"] = row["Quantity"];
        //        item.Web.AllowUnsafeUpdates = true;
        //        item.Update();
        //    }
        //}

        SPListItemCollection GetSPColl(string listName, string queryField, object queryValue, int limit)
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);

            return sps.Query(sps.GetList(listName),
                        new QueryField(queryField, false).Equal(queryValue),
                        limit);
        }
    }
}
