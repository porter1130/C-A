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
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections.Generic;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;

namespace CA.SharePoint.WebControls
{
    public partial class AllTasks : System.Web.UI.UserControl, IPostBackEventHandler
    {
        DataTable EnsureDataTableRow()
        {
            var dt = new DataTable();
            dt.Columns.Add("TaskId");
            dt.Columns.Add("TaskTitle");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("WorkflowUrl");
            dt.Columns.Add("WorkflowName");
            dt.Columns.Add("ModuleTitle");
            dt.Columns.Add("Status");
            dt.Columns.Add("MenuTemplateIdField");
            return dt;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            DataBindMyTasks();

        }

        private void DataBindMyTasks()
        {
            var dtBind = EnsureDataTableRow();

            var web = SPContext.Current.Site.RootWeb;

            var query = new SPSiteDataQuery();

            var swhere = @"<Where>
            <And>
                <And>
                    <Eq>
                    <FieldRef Name=""AssignedTo"" LookupId=""TRUE""/>
                    <Value Type=""Integer"">{0}</Value>
                    </Eq>
                    <Neq>
                    <FieldRef Name=""Status"" />
                    <Value Type=""Text"">{1}</Value>
                    </Neq>
                </And>
                <Neq>
                    <FieldRef Name='ID' />
                    <Value Type='Counter'>{2}</Value>
                 </Neq>
            </And>
            </Where>";

            var scompleted = SPUtility.GetLocalizedString("$Resources:core,Tasks_Completed;", "core", web.Language);
            string currTaskId = Request.QueryString["TaskId"].IsNullOrWhitespace() ? "0" : Request.QueryString["TaskId"];

            swhere = String.Format(swhere, web.CurrentUser.ID, scompleted, currTaskId);

            //var sorderby = "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.WorkflowName.ToString("B") + "\"/><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\" Ascending=\"FALSE\"/></OrderBy>";
            var sorderby = "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\" Ascending=\"FALSE\"/></OrderBy>";
            query.Query = swhere + sorderby;

            query.Lists = "<Lists ServerTemplate=\"107\"/>";

            query.ViewFields = "<FieldRef ID=\"" + SPBuiltInFieldId.Title.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.ID.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskDueDate.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.UniqueId.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Completed.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.PercentComplete.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskStatus.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.WorkflowLink.ToString("B") + "\" Nullable=\"TRUE\" Type=\"URL\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FileRef.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.ID.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FSObjType.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.WorkflowName.ToString("B") + "\"/>";
            //query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_By.ToString("B") + "\"/>";

            query.Webs = "<Webs Scope=\"Recursive\" />";

            var createdDateFieldId = SPBuiltInFieldId.Created_x0020_Date.ToString("B");

            var uIdField = SPBuiltInFieldId.UniqueId.ToString("B");
            var listField = SPBuiltInFieldId.FileRef.ToString("B");

            var t = web.GetSiteData(query);
            if (t != null && t.Rows.Count > 0)
            {
                t.DefaultView.Sort = createdDateFieldId + " Desc";
            }

            var sep = new string[] { ";#" };
            t.Columns.Add("WorkFlowUrl");

            var mxw = new Hashtable();
            var workflowcenter = SPContext.Current.Site.OpenWeb("workflowcenter");
            var modules = workflowcenter.Lists["Modules"];

            foreach (SPListItem item in modules.Items)
            {
                var workflownames = (item["WorkflowNames"].AsString() + "").Replace("<p>", "").Replace("</p>", "");
                if (!string.IsNullOrEmpty(workflownames.Trim()))
                {
                    foreach (var workflowname in workflownames.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(workflowname.Trim()) && !mxw.Contains(workflowname.Trim()))
                        {
                            mxw.Add(workflowname.Trim(), item["Title"] + "");
                        }
                    }
                }
            }

            DataRow dr = null;
            foreach (DataRow row in t.Rows)
            {
                dr = dtBind.Rows.Add();
                var createdDate = "" + row[createdDateFieldId];
                var tempArr = createdDate.Split(sep, StringSplitOptions.None);

                if (tempArr.Length > 1)
                    row[createdDateFieldId] = Convert.ToDateTime(tempArr[1]).ToString("yyyy-MM-dd");

                row[uIdField] = row[uIdField].ToString().Split(sep, StringSplitOptions.None)[1];
                //35;#WorkFlowCenter/Lists/Tasks/35_.000
                var workflowUrl = row[listField].ToString().Split(sep, StringSplitOptions.None)[1];
                var index = workflowUrl.LastIndexOf(@"/");
                workflowUrl = SPContext.Current.Site.RootWeb.Url + "/" + workflowUrl.Remove(index) + "/DispForm.aspx?ID=" + row[SPBuiltInFieldId.ID.ToString("B")];
                row["WorkFlowUrl"] = workflowUrl;

                dr["TaskId"] = row[SPBuiltInFieldId.ID.ToString("B")];
                dr["TaskTitle"] = row[SPBuiltInFieldId.Title.ToString("B")];
                if (IsNeedDelete(row[SPBuiltInFieldId.Title.ToString("B")].AsString()))
                {
                    dr["MenuTemplateIdField"] = "MenuList";
                }
                else
                {
                    dr["MenuTemplateIdField"] = "MenuTemplate1";
                }
                dr["StartTime"] = row[createdDateFieldId];
                //  dr["CreatedBy"] = row[SPBuiltInFieldId.Created_x0020_By.ToString("B")];
                dr["WorkflowName"] = row[SPBuiltInFieldId.WorkflowName.ToString("B")];
                dr["WorkflowUrl"] = workflowUrl + "&Source=" + this.Page.Request.RawUrl;
                if (mxw.Contains(row[SPBuiltInFieldId.WorkflowName.ToString("B")]))
                {
                    dr["ModuleTitle"] = mxw[row[SPBuiltInFieldId.WorkflowName.ToString("B")]];
                }
                else
                {
                    dr["ModuleTitle"] = row[SPBuiltInFieldId.WorkflowName.ToString("B")];
                }
                dr["Status"] = row[SPBuiltInFieldId.TaskStatus.ToString("B")];
            }

            //this.gvList.DataSource = dtBind;
            this.gvList.DataSource = dtBind.AsEnumerable().OrderBy(d => d["ModuleTitle"]).AsDataView();
            this.gvList.DataBind();
        }




        //protected override void CreateChildControls()
        //{
        //    //SPBoundField f = new SPBoundField();
        //    //f.DataField = SPBuiltInFieldId.Title.ToString("B");
        //    //f.HeaderText = "Task";
        //    //this.gvList.Columns.Add(f);

        //    //f = new SPBoundField();
        //    //f.DataField = SPBuiltInFieldId.Created_x0020_Date.ToString("B");
        //    //f.HeaderText = "Start Time";
        //    //this.gvList.Columns.Add(f);

        //    ////f = new SPBoundField();
        //    ////f.DataField = SPBuiltInFieldId.Created_x0020_Date.ToString("B");
        //    ////f.HeaderText = "";
        //    ////this.gvList.Columns.Add(f);

        //    //HyperLinkField link = new HyperLinkField();
        //    //link.HeaderText = "Approve";
        //    //link.Text = "Approve";

        //    base.CreateChildControls();
        //}

        //        DataTable GetRootTasks()
        //        {

        //            SPWeb web = SPContext.Current.Site.RootWeb;

        //            SPSiteDataQuery query = new SPSiteDataQuery();


        //            string swhere = @"<Where><And>
        //            <Eq>
        //            <FieldRef Name=""AssignedTo"" LookupId=""TRUE""/>
        //            <Value Type=""Integer"">{0}</Value>
        //            </Eq>
        //            <Neq>
        //            <FieldRef Name=""Status"" />
        //            <Value Type=""Text"">{1}</Value>
        //            </Neq>
        //            </And></Where>";

        //            string scompleted = SPUtility.GetLocalizedString("$Resources:core,Tasks_Completed;", "core", web.Language);

        //            swhere = String.Format(swhere, web.CurrentUser.ID, scompleted);

        //            string sorderby = "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\"/></OrderBy>";
        //            query.Query = swhere + sorderby;

        //            query.Lists = "<Lists ServerTemplate=\"107\"/>";

        //            query.ViewFields = "<FieldRef ID=\"" + SPBuiltInFieldId.Title.ToString("B") + "\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskDueDate.ToString("B") + "\" Nullable=\"TRUE\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.UniqueId.ToString("B") + "\" Nullable=\"TRUE\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Completed.ToString("B") + "\" Nullable=\"TRUE\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.PercentComplete.ToString("B") + "\" Nullable=\"TRUE\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskStatus.ToString("B") + "\" Nullable=\"TRUE\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.WorkflowLink.ToString("B") + "\" Nullable=\"TRUE\" Type=\"URL\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FileRef.ToString("B") + "\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.ID.ToString("B") + "\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\"/>";
        //            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FSObjType.ToString("B") + "\"/>";

        //            query.Webs = "<Webs Scope=\"Recursive\" />";

        //            string createdDateFieldId = SPBuiltInFieldId.Created_x0020_Date.ToString("B");

        //            string uIdField = SPBuiltInFieldId.UniqueId.ToString("B");
        //            string listField = SPBuiltInFieldId.FileRef.ToString("B");

        //            query.RowLimit = 5;
        //            DataTable t = web.GetSiteData(query);
        //            if (t != null && t.Rows.Count > 0)
        //            {
        //                t.DefaultView.Sort = createdDateFieldId + " Desc";
        //            }

        //            string[] sep = new string[] { ";#" };
        //            t.Columns.Add("WorkFlowUrl");
        //            foreach (DataRow row in t.Rows)
        //            {
        //                string createdDate = "" + row[createdDateFieldId];
        //                string[] tempArr = createdDate.Split(sep, StringSplitOptions.None);

        //                if (tempArr.Length > 1)
        //                    row[createdDateFieldId] = Convert.ToDateTime(tempArr[1]).ToString("yyyy-MM-dd");

        //                row[uIdField] = row[uIdField].ToString().Split(sep, StringSplitOptions.None)[1];
        //                //35;#WorkFlowCenter/Lists/Tasks/35_.000
        //                string workflowUrl = row[listField].ToString().Split(sep, StringSplitOptions.None)[1];
        //                int index = workflowUrl.LastIndexOf(@"/");
        //                workflowUrl = SPContext.Current.Site.RootWeb.Url + "/DispForm.aspx?ID=" + row[SPBuiltInFieldId.ID.ToString("B")];
        //                row["WorkFlowUrl"] = workflowUrl;
        //            }

        //            return t;
        //        }

        public void RaisePostBackEvent(string eventArgument)
        {
            string[] events = eventArgument.Split(',');

            switch (events[0].Trim())
            {
                case "DELETE":
                    try
                    {
                        SPList taskList = SPContext.Current.Site.AllWebs["WorkflowCenter"].Lists["Tasks"];
                        int taskId = int.Parse(events[1].Trim());

                        SPListItem taskItem = taskList.GetItemById(taskId);

                        if (IsNeedDelete(taskItem[SPBuiltInFieldId.Title].AsString()))
                        {
                            TerminateWF(taskItem);
                        }
                        DataBindMyTasks();
                    }
                    catch (Exception ex)
                    {
                        string strAlert = "There is an error occur when deleting the task, please try it again!";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Msg", "<script>alert('" + strAlert + "')</script>");
                        Response.Write(ex.Message + ex.InnerException);
                    }

                    break;
                default:
                    break;
            }
        }

        private void TerminateWF(SPListItem taskItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite osite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oweb = osite.OpenWeb("WorkflowCenter"))
                    {
                        oweb.AllowUnsafeUpdates = true;

                        SPWorkflow workflow = new SPWorkflow(oweb, new Guid(taskItem[SPBuiltInFieldId.WorkflowInstanceID].AsString()));

                        foreach (SPWorkflowTask wfTask in workflow.Tasks)
                        {
                            wfTask["Status"] = "Canceled";
                            wfTask.SystemUpdate();
                        }
                        SPWorkflowManager.CancelWorkflow(workflow);


                        oweb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public bool IsNeedDelete(string taskTitle)
        {
            bool isNeedDelete = false;
            if (taskTitle.StartsWith("Please resubmit", StringComparison.CurrentCultureIgnoreCase)
                        || taskTitle.StartsWith("Please complete", StringComparison.CurrentCultureIgnoreCase)
                        || taskTitle.StartsWith("Please finish", StringComparison.CurrentCultureIgnoreCase)
                        || taskTitle.StartsWith("Please modify", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                isNeedDelete = true;
            }
            return isNeedDelete;
        }
    }
}