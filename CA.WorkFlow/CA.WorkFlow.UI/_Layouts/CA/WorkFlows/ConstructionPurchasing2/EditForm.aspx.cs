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

using QuickFlow.Core;
using Microsoft.SharePoint;
using System.IO;
using CodeArt.SharePoint.CamlQuery;
using System.Collections.Generic;
namespace CA.WorkFlow.UI.ConstructionPurchasing2
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.actions.Attributes.Add("onclick", "return CheckComments();");
            hdBody.Value = this.body.ClientID;
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            if (WorkflowContext.Current.Step.ToString() != "Construction")
            {
                btnSave.Style.Add("display", "none");
            }
            if (WorkflowContext.Current.Step.ToString() != "PlacesTheOrder")
            {
                hrefPDF.Style.Add("display", "none");
            }
            //linkexcel.HRef = "testExportExcel.aspx";
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (WorkflowContext.Current.Task.Step == "PlacesTheOrder")
            {
                string strForkFlowNumber = ((Label)DataForm1.FindControl("lblWorkflowNumber")).Text;
                if (!string.IsNullOrEmpty(strForkFlowNumber))
                {
                    hdUrl.Value = "testExportExcel.aspx?List=" + Request["List"] + "&ID=" + Request["ID"] + "&WorkFlowNumber=" + strForkFlowNumber;
                }
                else
                {
                    hrefPDF.Visible = false;
                }
            }
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            if ((WorkflowContext.Current.Step.ToString() == "Construction") && TaskOutcome.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
            {
                DataTable dtRecords = this.DataForm1.DataTableRecord;

                ISharePointService sps = ServiceFactory.GetSharePointService(true);
                SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionItems.ToString());

                //frist delete the items
                string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
                WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", strTimeOffNumber);

                SPListItem item = null;
                foreach (DataRow dr in dtRecords.Rows)
                {
                    item = listRecord.Items.Add();
                    item["Discription"] = dr["Discription"];
                    item["ItemCode"] = dr["ItemCode"];
                    item["Quantity"] = dr["Quantity"];
                    item["Unit"] = dr["Unit"];
                    item["UnitPrice"] = dr["UnitPrice"];
                    item["TotalPrice"] = dr["TotalPrice"];
                    item["Remark"] = dr["Remark"];
                    item["WorkFlowNumber"] = this.DataForm1.WorkflowNumber;
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
            RedirectToTask();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            switch (WorkflowContext.Current.Step.ToString())
            {
                case "Construction":
                    string msg = DataForm1.Validate;
                    if (!string.IsNullOrEmpty(msg))
                    {
                        DisplayMessage(msg);
                        e.Cancel = true;
                        return;
                    }

                    WorkflowContext.Current.DataFields["Installation"] = this.DataForm1.Installation;
                    WorkflowContext.Current.DataFields["Freight"] = this.DataForm1.Freight;
                    WorkflowContext.Current.DataFields["Packaging"] = this.DataForm1.Packaging;
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApproverLoginName);
                    break;
                case "PlacesTheOrder":
                    CheckBox chk = DataForm1.FindControl("chkPlacesOrder") as CheckBox;
                    if (!chk.Checked)
                    {
                        e.Cancel = true;
                        Page.RegisterStartupScript("msg1", "<script>alert('Can not confirm with Places the order unchecked!')</script>");
                        return;
                    }
                    else
                    {
                        WorkflowContext.Current.DataFields["PlacesOrder"] = "Yes";
                    }
                    //WorkflowContext.Current.DataFields["Approvers"] = col;
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApproverLoginName);
                    break;
                case "OrderHandover":
                    CheckBox chk1 = DataForm1.FindControl("chkOrderHandover") as CheckBox;
                    if (!chk1.Checked)
                    {
                        e.Cancel = true;
                        Page.RegisterStartupScript("msg2", "<script>alert('Can not confirm with Order handover unchecked!')</script>");
                        return;
                    }
                    else
                    {
                        WorkflowContext.Current.DataFields["OrderHandover"] = "Yes";
                    }
                    //WorkflowContext.Current.DataFields["Approvers"] = col;
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApproverLoginName);
                    break;
                default:
                    // WorkflowContext.Current.DataFields["Approvers"] = col;
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingApproverLoginName);
                    break;
            }
            if (WorkflowContext.Current.Step == "OrderHandover" && e.Action == "Confirm")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
            else
            {
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }

            switch (WorkflowContext.Current.Task.Step)
            {
                case CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstructionHead:
                    QuickFlow.NameCollection ConstructionUser = new QuickFlow.NameCollection();
                    List<string> lst = WorkFlowUtil.UserListInGroup("wf_Construction");
                    ConstructionUser.AddRange(lst.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstructionAccount, GetDelemanNameCollection(ConstructionUser, Constants.CAModules.ConstructionPurchasingRequest));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstruction:
                    string strConstructionHeadAccount = UserProfileUtil.GetDepartmentManager("Construction");
                    QuickFlow.NameCollection ConstructionHead = new QuickFlow.NameCollection();
                    ConstructionHead.Add(strConstructionHeadAccount);
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstructionHeadAccount, GetDelemanNameCollection(ConstructionHead, Constants.CAModules.ConstructionPurchasingRequest));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstructionHeadAgain:
                    string department = string.Empty;
                    if (DataForm1.Applicant.Department.Contains(';'))
                        department = DataForm1.Applicant.Department.Substring(0, DataForm1.Applicant.Department.IndexOf(';') + 1);
                    else
                        department = DataForm1.Applicant.Department;
                    string departmentManager = UserProfileUtil.GetDepartmentManager(department);
                    QuickFlow.NameCollection DepartmentHead = new QuickFlow.NameCollection();
                    DepartmentHead.Add(departmentManager);
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingDepartmentHeadAccount, GetDelemanNameCollection(DepartmentHead, Constants.CAModules.ConstructionPurchasingRequest));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingDepartmentHead:
                    QuickFlow.NameCollection StoreOperationTeamUser = new QuickFlow.NameCollection();
                    string strStoreOperations = UserProfileUtil.GetDepartmentManager("Store Operations");
                    StoreOperationTeamUser.Add(strStoreOperations);
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingStoreOperationTeamAccount, GetDelemanNameCollection(StoreOperationTeamUser, Constants.CAModules.ConstructionPurchasingRequest));
                    QuickFlow.NameCollection ConstructionUser1 = new QuickFlow.NameCollection();
                    List<string> lst1 = WorkFlowUtil.UserListInGroup("wf_Construction");
                    ConstructionUser1.AddRange(lst1.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstructionAccount, GetDelemanNameCollection(ConstructionUser1, Constants.CAModules.ConstructionPurchasingRequest));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingStoreOperationTeam:
                    QuickFlow.NameCollection CFOUser = new QuickFlow.NameCollection();
                    List<string> lst2 = WorkFlowUtil.UserListInGroup("wf_CFO");
                    CFOUser.AddRange(lst2.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingCFOAccount, GetDelemanNameCollection(CFOUser, Constants.CAModules.ConstructionPurchasingRequest));
                    break;
                case CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingCFO:
                    QuickFlow.NameCollection ConstructionUser2 = new QuickFlow.NameCollection();
                    List<string> lst3 = WorkFlowUtil.UserListInGroup("wf_Construction");
                    ConstructionUser2.AddRange(lst3.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.ConstructionPurchasingConstructionAccount, GetDelemanNameCollection(ConstructionUser2, Constants.CAModules.ConstructionPurchasingRequest));
                    break;

            }




            TaskOutcome = e.Action;


            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = DataForm1.ValidSavedate;
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                return;
            }

            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionItems.ToString());

            //frist delete the items
            string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", strTimeOffNumber);

            //WorkflowContext.Current.DataFields["Installation"] = this.DataForm1.Installation;
            //WorkflowContext.Current.DataFields["Freight"] = this.DataForm1.Freight;
            //WorkflowContext.Current.DataFields["Packaging"] = this.DataForm1.Packaging;

            SPListItem item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                item = listRecord.Items.Add();
                item["Discription"] = dr["Discription"];
                item["ItemCode"] = dr["ItemCode"];
                item["Quantity"] = dr["Quantity"];
                item["Unit"] = dr["Unit"];
                item["UnitPrice"] = dr["UnitPrice"];
                item["TotalPrice"] = dr["TotalPrice"];
                item["Remark"] = dr["Remark"];
                item["WorkFlowNumber"] = this.DataForm1.WorkflowNumber;
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
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionPurchasingWorkflow.ToString());
            SPListItemCollection items = sps.Query(list, new QueryField("WorkFlowNumber", false).Equal(this.DataForm1.WorkflowNumber), 1);
            //item = null;
            //item = list.Items.Add();
            SPListItem updateItem = items[0];
            updateItem["Installation"] = this.DataForm1.Installation;
            updateItem["Freight"] = this.DataForm1.Freight;
            updateItem["Packaging"] = this.DataForm1.Packaging;
            //item["WorkFlowNumber"] = this.DataForm1.WorkflowNumber;
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        updateItem.Web.AllowUnsafeUpdates = true;
                        updateItem.Update();
                        updateItem.Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occured while updating the items");
            }
            base.Back();
        }

        protected void Exported_Click(object sender, EventArgs e)
        {
            //Response.Redirect("testExportExcel.aspx");
            string strFileName = this.DataForm1.WorkflowNumber + ".xls";
            string strPath = Server.MapPath("/tmpfiles/excel");// "d:/pdf";
            DirectoryInfo dinfo = new DirectoryInfo(strPath);
            if (!dinfo.Exists)
            {
                Directory.CreateDirectory(strPath);
            }
            string strFilePath = strPath + "/" + strFileName;
            DataTable dtRecords = this.DataForm1.DataTableRecord;
            OperateExcel.ExportConstructionPurchasing(Server.MapPath("standard PO.xls"), dtRecords, this.DataForm1.CostCenter, DataForm1.ProduceandDeliveryDate,
                ((TextBox)this.DataForm1.FindControl("txtSumCost")).Text, DataForm1.Installation, DataForm1.Freight, DataForm1.Packaging, strFilePath);
        }
    }
}
