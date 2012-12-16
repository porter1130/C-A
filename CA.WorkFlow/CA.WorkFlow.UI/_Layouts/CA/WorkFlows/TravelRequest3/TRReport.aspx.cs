using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using CA.SharePoint.Utilities.Common;
using System.Data;
using System.Reflection;
using System.IO;
using Microsoft.SharePoint.WebControls;
using CA.SharePoint.CamlQuery;
using System.Text;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.TravelRequest3
{
    public partial class TRReport : LayoutsPageBase
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            Refresh();
            if (!IsPostBack)
            {
                //CostCenterDataBind();
                dtPeriodFrom.SelectedDate = DateTime.Now.AddMonths(-1);
                dtPeriodTo.SelectedDate = DateTime.Now;
                TRReportDataBind(dtPeriodFrom.SelectedDate, dtPeriodTo.SelectedDate);
            }
        }

        private void Refresh()
        {
            string script = "_spOriginalFormAction = document.forms[0].action;\n_spSuppressFormOnSubmitWrapper = true;";

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alterFormSubmitEvent", script, true);
        }

        private void TRReportDataBind(DateTime dateFrom, DateTime dateTo)
        {
            SPList travelRequestList = SPContext.Current.Web.Lists[WorkflowListName.TravelRequestWorkflow2];
            TypeQueryField<DateTime> queryField = new TypeQueryField<DateTime>("Created");

            //SPUtility.FormatDate(SPContext.Current.Web, dateFrom, SPDateFormat.DateOnly)
            CAMLExpression<object> exp = queryField.MoreEqual(dateFrom);

            exp &= queryField.LessEqual(dateTo);

            SPQuery query = new SPQuery();
            query.Query = CAMLBuilder.Where(exp);

            SPListItemCollection items = travelRequestList.GetItems(query);

            if (items.Count > 0)
            {
                SPGridView1.DataSource = GetDataSource(items);
            }
            else
            {
                SPGridView1.DataSource = null;
            }
            SPGridView1.DataBind();
        }

        private DataTable GetDataSource(SPListItemCollection items)
        {
            DataTable reportDT = new DataTable();
            SPWeb currWeb = SPContext.Current.Web;

            DataTable detailsDT = currWeb.Lists[WorkflowListName.TravelDetails2].Items.GetDataTable();

            TRReportItem trReportItem = new TRReportItem();

            CommonUtil.logInfo(items.GetDataTable().AsEnumerable().Count().ToString());
            CommonUtil.logInfo(detailsDT.AsEnumerable().Count().ToString());

            var leftJoin = from parent in items.GetDataTable().AsEnumerable()
                           join child in detailsDT.AsEnumerable()
                           on parent[trReportItem.Title].AsString() equals child[trReportItem.Title].AsString() into Joined
                           from child in Joined.DefaultIfEmpty()
                           select new TRReportItem
                           {
                               Title = parent != null ? parent[trReportItem.Title].AsString() : string.Empty,
                               ChineseName = parent != null ? parent[trReportItem.ChineseName].AsString() : string.Empty,
                               Department = parent != null ? parent[trReportItem.Department].AsString() : string.Empty,
                               CostCenter = child != null ? child[trReportItem.CostCenter].AsString() : string.Empty,
                               TravelDateFrom = child != null ? child[trReportItem.TravelDateFrom].AsString() : string.Empty,
                               TravelDateTo = child != null ? child[trReportItem.TravelDateTo].AsString() : string.Empty,
                               TravelLocationFrom = child != null ? child[trReportItem.TravelLocationFrom].AsString() : string.Empty,
                               TravelLocationTo = child != null ? child[trReportItem.TravelLocationTo].AsString() : string.Empty
                           };


            if (leftJoin.Any())
            {
                reportDT = leftJoin.AsDataTable();
            }

            return reportDT;

        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            TRReportDataBind(dtPeriodFrom.SelectedDate, dtPeriodTo.SelectedDate);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            ExportToExcel(string.Format("TravelRequestReport_{0}.xls", DateTime.Now.ToShortDateString()), SPGridView1);
        }

        private void ExportToExcel(string fileName, Microsoft.SharePoint.WebControls.SPGridView spGV)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Table table = new Table();

                    if (spGV.HeaderRow != null)
                    {
                        table.Rows.Add(spGV.HeaderRow);
                    }

                    foreach (GridViewRow gvRow in spGV.Rows)
                    {
                        table.Rows.Add(gvRow);
                    }

                    if (spGV.FooterRow != null)
                    {
                        table.Rows.Add(spGV.FooterRow);
                    }

                    table.BorderWidth = 1;

                    foreach (TableRow tr in table.Rows)
                    {
                        foreach (TableCell td in tr.Cells)
                        {
                            td.BorderWidth = 1;
                        }
                    }

                    //htw.RenderBeginTag(HtmlTextWriterTag.Span);
                    //htw.Write("Test");
                    //htw.RenderEndTag();

                    table.RenderControl(htw);

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                    HttpContext.Current.Response.ContentType = "application/ms-excel";
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Charset = Encoding.UTF8.ToString();
                    HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                    Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
                    HttpContext.Current.Response.Write(sw.ToString());
                    Response.Write("</body></html>");
                    HttpContext.Current.Response.End();
                }
            }
        }



        protected void SPGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SPGridView1.PageIndex = e.NewPageIndex;
            this.SPGridView1.DataBind();
        }


    }

    public class TRReportItem
    {
        private string _title = "Title";

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _chineseName = "ChineseName";

        public string ChineseName
        {
            get { return _chineseName; }
            set { _chineseName = value; }
        }

        private string _department = "Department";

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        private string _costCenter = "CostCenter";

        public string CostCenter
        {
            get { return _costCenter; }
            set { _costCenter = value; }
        }

        private string _travelDateFrom = "TravelDateFrom";

        public string TravelDateFrom
        {
            get { return _travelDateFrom; }
            set { _travelDateFrom = value; }
        }


        private string _travelDateTo = "TravelDateTo";

        public string TravelDateTo
        {
            get { return _travelDateTo; }
            set { _travelDateTo = value; }
        }


        private string _travelLocationFrom = "TravelLocationFrom";

        public string TravelLocationFrom
        {
            get { return _travelLocationFrom; }
            set { _travelLocationFrom = value; }
        }


        private string _travelLocationTo = "TravelLocationTo";

        public string TravelLocationTo
        {
            get { return _travelLocationTo; }
            set { _travelLocationTo = value; }
        }
    }
}