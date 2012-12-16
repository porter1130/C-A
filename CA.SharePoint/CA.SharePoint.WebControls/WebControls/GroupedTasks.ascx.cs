using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace CA.SharePoint.WebControls.WebControls
{
    public partial class GroupedTasks : System.Web.UI.UserControl
    {
        DataTable EnsureDataTableRow()
        {
            var dt = new DataTable();
            dt.Columns.Add("TaskTitle");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("WorkflowUrl");
            dt.Columns.Add("WorkflowName");
            dt.Columns.Add("ModuleName");
            return dt;
        }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvList.DataSource = GetUserTasks();
            this.gvList.DataBind();
        }

        DataTable GetUserTasks()
        {
            var dtBind = EnsureDataTableRow();


            var web = SPContext.Current.Site.RootWeb;
            
            var query = new SPSiteDataQuery();

            var scompleted = SPUtility.GetLocalizedString("$Resources:core,Tasks_Completed;", "core", web.Language);

            string swhere = @"<Where><And>
        <Eq>
        <FieldRef Name=""AssignedTo"" LookupId=""TRUE""/>
        <Value Type=""Integer"">{0}</Value>
        </Eq>
        <Neq>
        <FieldRef Name=""Status"" />
        <Value Type=""Text"">{1}</Value>
        </Neq>
        </And></Where>";

            swhere = string.Format(swhere, web.CurrentUser.ID, scompleted);

            var sorderby = "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.WorkflowName.ToString("B") + "\"/><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\" Ascending=\"FALSE\"/></OrderBy>";
                
            query.Query = swhere + sorderby;

            query.Lists = "<Lists ServerTemplate=\"107\"/>";

            query.ViewFields = "<FieldRef ID=\"" + SPBuiltInFieldId.Title.ToString("B") + "\"/>";
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

            query.Webs = "<Webs Scope=\"Recursive\" />";

            string createdDateFieldId = SPBuiltInFieldId.Created_x0020_Date.ToString("B");

            string uIdField = SPBuiltInFieldId.UniqueId.ToString("B");
            string listField = SPBuiltInFieldId.FileRef.ToString("B");

            var t = web.GetSiteData(query);
            if (t != null && t.Rows.Count > 0)
            {
                t.DefaultView.Sort = createdDateFieldId + " Desc";
            }

            var sep = new string[] { ";#" };
            t.Columns.Add("WorkFlowUrl");

            foreach (DataRow row in t.Rows)
            {
                var dr = dtBind.Rows.Add();
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

                dr["TaskTitle"] = row[SPBuiltInFieldId.Title.ToString("B")];
                dr["StartTime"] = row[createdDateFieldId];
                //  dr["CreatedBy"] = row[SPBuiltInFieldId.Created_x0020_By.ToString("B")];
                dr["WorkflowName"] = row[SPBuiltInFieldId.WorkflowName.ToString("B")];
                dr["WorkflowUrl"] = workflowUrl + "&Source=" + this.Page.Request.RawUrl;
                dr["ModuleName"] = "TODO..";
            }

            
            return dtBind;
        }
    }
}