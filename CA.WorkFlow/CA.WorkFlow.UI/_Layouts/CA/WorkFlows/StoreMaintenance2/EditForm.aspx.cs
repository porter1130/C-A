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
using GemBox.Spreadsheet;
using System.IO;
using QuickFlow;
namespace CA.WorkFlow.UI.StoreMaintenance2
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Export(Server.MapPath("standard PO.xls"));
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";

            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            if (WorkflowContext.ContextInitialized)
            {
                switch (WorkflowContext.Current.Task.Step)
                {
                    case "CMManagerGroupReview":
                        //PanelComm.Visible = false;
                        //btnSave.Visible = false;
                        PFC.Visible = false;
                        break;
                    case "StoreManagerApprove":
                        btnSave.Visible = false;
                        PFC.Visible = false;
                        break;
                    case "AreaManagerApprove":
                        btnSave.Visible = false;
                        PFC.Visible = false;
                        break;
                    case "ConstructionHeadApprove":
                        btnSave.Visible = false;
                        PFC.Visible = false;
                        break;
                    case "SOTeamGroupApprove":
                        btnSave.Visible = false;
                        PFC.Visible = false;
                        break;

                    case "CMManagerGroupOrders":
                        btnSave.Visible = false;
                        PFC.Visible = false;
                        break;
                    case "StoreManagerEvaluates":
                        //Export();
                        btnSave.Visible = false;
                        PFC.Visible = false;
                        break;
                    case "RequestSubmit":
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

            switch (curContext.Task.Step)
            {
                case "CMManagerGroupReview":
                    string isCost = "";
                    if (this.DataForm1.Asso2.Rows.Count > 0)
                    {
                        isCost = "Yes";
                    }
                    else
                    {
                        isCost = "No";
                    }
                    curContext.UpdateWorkflowVariable("IsCost", isCost);

                    //this.DataForm1.Rpt1ToDt1();
                    //SaveAsso1();

                    this.DataForm1.Rpt2ToDt2();
                    SaveAsso2();
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName);

                    NameCollection constructionMaintenance = WorkFlowUtil.GetUsersInGroup("wf_ConstructionMaintenance");
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceCMManagerGroup, GetDelemanNameCollection(constructionMaintenance, Constants.CAModules.StoreMaintenance));

                    SPListItemCollection stores1 = GetSPColl("Stores", "Cost Center", fields["CostCenter"] + "", 1);
                    if (stores1.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(stores1[0]["Manager"] + ""))
                        {
                            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceStoreManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores1[0]["Manager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                        }
                    }
                    break;
                case "StoreManagerApprove":
                   // fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName); 
                  
                      SPListItemCollection stores2 = GetSPColl("Stores", "Cost Center", fields["CostCenter"] + "", 1);
                      if (stores2.Count > 0)
                      {
                          if (!string.IsNullOrEmpty(stores2[0]["Manager"] + ""))
                          {
                              WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceAreaManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores2[0]["AreaManager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                          }
                      }

                     string departmentManager = UserProfileUtil.GetDepartmentManager("Construction");
                     if (string.IsNullOrEmpty(departmentManager))
                     {
                         WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceConstructionHead, GetDelemanNameCollection(new NameCollection(departmentManager), Constants.CAModules.StoreMaintenance));
                     }
                    break;
                case "AreaManagerApprove":
                    //fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName);

                    string departmentManager1 = UserProfileUtil.GetDepartmentManager("Construction");
                    if (string.IsNullOrEmpty(departmentManager1))
                    {
                        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceConstructionHead, GetDelemanNameCollection(new NameCollection(departmentManager1), Constants.CAModules.StoreMaintenance));
                    }
                    break;
                case "ConstructionHeadApprove":
                    // fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName);

                    string departmentManager2 = UserProfileUtil.GetDepartmentManager("Store Operations");
                    if (string.IsNullOrEmpty(departmentManager2))
                    {
                        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceSOHead, GetDelemanNameCollection(new NameCollection(departmentManager2), Constants.CAModules.StoreMaintenance));
                    }
                    break;
                case "SOTeamGroupApprove":
                  //  fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName);
                    NameCollection constructionMaintenance1 = WorkFlowUtil.GetUsersInGroup("wf_ConstructionMaintenance");
                    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceCMManagerGroup, GetDelemanNameCollection(constructionMaintenance1, Constants.CAModules.StoreMaintenance));
                    
                    break;
                case "CMManagerGroupOrders":
                    Export();
                   // fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName);
                   
                     SPListItemCollection stores3 = GetSPColl("Stores", "Cost Center", fields["CostCenter"] + "", 1);
                     if (stores3.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(stores3[0]["Manager"] + ""))
                        {
                            WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceStoreManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores3[0]["Manager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                        }
                    }
                    break;
                case "StoreManagerEvaluates":
                    if (e.Action == "Confirm")
                    {
                        fields["Status"] = "Completed";
                    }
                   // fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                    AddWorkFlowStepApprovers(WorkflowContext.Current.Task.Step, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApprovers, CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceApproverLoginName);
                    break;
                case "RequestSubmit":

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
                    SaveMain();
                    this.DataForm1.Rpt1ToDt1();
                    SaveAsso1();

                     NameCollection constructionMaintenance2 = WorkFlowUtil.GetUsersInGroup("wf_ConstructionMaintenance");
                     WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceCMManagerGroup, GetDelemanNameCollection(constructionMaintenance2, Constants.CAModules.StoreMaintenance));

                    //SPListItemCollection stores = GetSPColl("Stores", "Cost Center", fields["CostCenter"] + "", 1);

                    //if (stores.Count > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(stores[0]["Manager"] + ""))
                    //    {
                    //        //curContext.UpdateWorkflowVariable("StoreManager",
                    //        //                new SPFieldLookupValue(stores[0]["Manager"] + "").LookupValue);

                    //        WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceStoreManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores[0]["Manager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                    //    }
                    //    //if (!string.IsNullOrEmpty(stores[0]["AreaManager"] + ""))
                    //    //{
                    //    //    //curContext.UpdateWorkflowVariable("AreaManager",
                    //    //    //    new SPFieldLookupValue(stores[0]["AreaManager"] + "").LookupValue);
                    //    //    WorkflowContext.Current.UpdateWorkflowVariable(CA.WorkFlow.UI.Constants.WorkFlowStep.StoreMaintenanceAreaManager, GetDelemanNameCollection(new NameCollection(new SPFieldLookupValue(stores[0]["AreaManager"] + "").LookupValue), Constants.CAModules.StoreMaintenance));
                    //    //}
                        
                    //}
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
            if (WorkflowContext.Current.Task.Step == "RequestSubmit")
            {
                SaveMain();

                this.DataForm1.Rpt1ToDt1();
                SaveAsso1();
            }
            else if (WorkflowContext.Current.Task.Step == "CMManagerGroupReview")
            {
                this.DataForm1.Rpt2ToDt2();
                SaveAsso2();
            }
        }

        void SaveMain()
        {
            SPListItem item = SPContext.Current.ListItem;
            item["Type1"] = ((DropDownList)DataForm1.FindControl("ddlType1")).SelectedValue;
            //item["CostCenter"] = ((TextBox)DataForm1.FindControl("txtCostCenter")).Text;
            item["CostCenter"] = ((DropDownList)DataForm1.FindControl("ddlCostCenter")).SelectedValue;
            item["BudgetApproved"] = ((DropDownList)DataForm1.FindControl("ddlBudgetApproved")).SelectedValue;
            item["BudgetValue"] = ((TextBox)DataForm1.FindControl("txtBudgetValue")).Text;
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

        void SaveAsso1()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreMaintenanceItems1.ToString());

            WorkFlowUtil.RemoveExistingRecord(list, "WorkflowNumber", DataForm1.WorkflowNumber);

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

        void SaveAsso2()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreMaintenanceItems2.ToString());

            WorkFlowUtil.RemoveExistingRecord(list, "WorkflowNumber", DataForm1.WorkflowNumber);

            foreach (DataRow row in this.DataForm1.Asso2.Rows)
            {
                SPListItem item = list.Items.Add();
                item["WorkflowNumber"] = DataForm1.WorkflowNumber;
                item["Seq"] = row["Seq"];
                item["Reason"] = row["Reason"];
                item["Price"] = row["Price"];
                item["Quantity"] = row["Quantity"];
                item["Total"] = row["Total"];
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

        SPListItemCollection GetSPColl(string listName, string queryField, object queryValue, int limit)
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);

            return sps.Query(sps.GetList(listName),
                        new QueryField(queryField, false).Equal(queryValue),
                        limit);
        }

        void Export()
        {

            SPListItem item = SPContext.Current.ListItem;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList items = sps.GetList("Store Maintenance Items2");
            QueryField field = new QueryField("WorkflowNumber", false);
            SPListItemCollection coll = sps.Query(items, field.Equal(item["WorkflowNumber"] + ""),0);
            int len = coll.Count;

            if (len > 0)
            {
                string strFilePath = Server.MapPath("standard PO.xls");
                string strCostCenter = item["CostCenter"] + "";
                string strDate = item["Created"] + "";
                //string strTotalPrice
                decimal coltotal = 0;
                string strInstallation = "0.00";
                string strFreigh = "0.00";
                string strPackaging = "0.00";

                string strFileName = this.DataForm1.WorkflowNumber + ".xls";

                string strPath = Server.MapPath("/tmpfiles/excel");
                DirectoryInfo dinfo = new DirectoryInfo(strPath);
                if (!dinfo.Exists)
                {
                    Directory.CreateDirectory(strPath);
                }
                string strSavePath = strPath + "/" + item["WorkflowNumber"] + ".xls";




                GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
                GemBox.Spreadsheet.ExcelFile objExcelFile = new ExcelFile();
                objExcelFile.LoadXls(strFilePath);
                GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];
                GemBox.Spreadsheet.ExcelWorksheet worksheet2 = objExcelFile.Worksheets[1];
                //objExcelFile = new ExcelFile();
                //GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets.AddCopy(worksheet1.Name, worksheet1);
                //GemBox.Spreadsheet.ExcelWorksheet worksheet2 = objExcelFile.Worksheets.AddCopy(worksheet2.Name, worksheet2);

                int j = 0;
                int k = 0;
                for (int i = 10; i < 10 + len; i++)
                {
                    decimal price = decimal.Parse(coll[k]["Price"] + "");
                    decimal total = decimal.Parse(coll[k]["Total"] + "");
                    int quantity = int.Parse(coll[k]["Quantity"] + "");
                    decimal rowtotal = price * quantity;
                    coltotal += rowtotal;

                    worksheet1.Rows[i].InsertEmpty(1);
                    worksheet1.Cells[i, 0].Value = ++j;
                    worksheet1.Cells[i, 1].Value = coll[k]["Seq"].ToString();
                    worksheet1.Cells[i, 2].Value = strCostCenter;
                    worksheet1.Cells.GetSubrangeAbsolute(i, 3, i, 6).Merged = true;
                    worksheet1.Cells[i, 3].Value = coll[k]["Reason"].ToString();
                    worksheet1.Cells[i, 7].Value = strDate;
                    worksheet1.Cells.GetSubrangeAbsolute(i, 8, i, 9).Merged = true;
                    worksheet1.Cells[i, 8].Value = coll[k]["Quantity"].ToString();
                    //worksheet1.Cells[i, 10].Value = coll[k]["Unit"].ToString();
                    worksheet1.Cells.GetSubrangeAbsolute(i, 11, i, 14).Merged = true;
                    worksheet1.Cells[i, 14].Value = price.ToString("N2");
                    worksheet1.Cells.GetSubrangeAbsolute(i, 15, i, 19).Merged = true;
                    worksheet1.Cells[i, 19].Value = total.ToString("N2");//rowtotal.ToString("N2");
                    worksheet1.Cells.GetSubrangeAbsolute(i, 20, i, 24).Merged = true;
                    worksheet1.Cells[i, 24].Value = "";

                    k++;

                }

                worksheet1.Cells[11 + len, 0].Value = coltotal.ToString("N2");
                worksheet1.Cells[11 + len, 2].Value = strInstallation;
                worksheet1.Cells[11 + len, 4].Value = strPackaging;
                worksheet1.Cells[11 + len, 6].Value = strFreigh;
                worksheet1.Cells[11 + len, 9].Value = coltotal.ToString("N2");

                j = 0; k = 0;
                for (int i = 10; i < 10 + len; i++)
                {
                    decimal price = decimal.Parse(coll[k]["Price"] + "");
                    decimal total = decimal.Parse(coll[k]["Total"] + "");
                    int quantity = int.Parse(coll[k]["Quantity"] + "");
                    decimal rowtotal = price * quantity;

                    worksheet2.Rows[i].InsertEmpty(1);
                    worksheet2.Cells[i, 0].Value = ++j;
                    worksheet2.Cells[i, 1].Value = coll[k]["Seq"].ToString();
                    worksheet2.Cells[i, 2].Value = strCostCenter;
                    worksheet2.Cells.GetSubrangeAbsolute(i, 3, i, 6).Merged = true;
                    worksheet2.Cells[i, 3].Value = coll[k]["Reason"].ToString();
                    worksheet2.Cells[i, 7].Value = "";
                    worksheet2.Cells.GetSubrangeAbsolute(i, 8, i, 9).Merged = true;
                    worksheet2.Cells[i, 8].Value = coll[k]["Quantity"].ToString();
                    //worksheet2.Cells[i, 10].Value = coll[k]["Unit"].ToString();
                    worksheet2.Cells.GetSubrangeAbsolute(i, 11, i, 14).Merged = true;
                    worksheet2.Cells[i, 14].Value = price.ToString("N2");
                    worksheet2.Cells.GetSubrangeAbsolute(i, 15, i, 19).Merged = true;
                    worksheet2.Cells[i, 19].Value = total.ToString("N2");//rowtotal.ToString("N2");
                    worksheet2.Cells.GetSubrangeAbsolute(i, 20, i, 24).Merged = true;
                    worksheet2.Cells[i, 24].Value = "";
                    k++;
                }
                worksheet2.Cells[11 + len, 0].Value = coltotal.ToString("N2");
                worksheet2.Cells[11 + len, 2].Value = strInstallation;
                worksheet2.Cells[11 + len, 4].Value = strPackaging;
                worksheet2.Cells[11 + len, 6].Value = strFreigh;
                worksheet2.Cells[11 + len, 9].Value = coltotal.ToString("N2");

                if (File.Exists(strSavePath))
                {
                    File.Delete(strSavePath);
                }
                objExcelFile.SaveXls(strSavePath);
            }
        }
    }
}
