using System;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;

using System.Linq;
using System.Data;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class GenerateReport : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
        }

        private void CheckAccount()
        {
           // HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_HO" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }

        void GetPRPODT()
        {
            SPListItemCollection lc= SPContext.Current.Web.Lists["PurchaseRequestItems"].Items;
            DataTable DtPRItems = lc.GetDataTable();
            DataTable dt = new DataTable();
            dt = GetGroupByDatatable(DtPRItems);
            CreateExcel(dt);
        }

        DataTable GetGroupByDatatable(DataTable myDT)
        {
            DataTable dt = new DataTable();
            var IEDR = from dr in myDT.AsEnumerable()
                       //orderby dr.Field<string>("Title")
                       group dr by new { Title = dr.Field<string>("Title"), PONumber = dr["PONumber"], VendorName = dr.Field<string>("VendorName") } into g
                       select new
                       {
                           Title = g.Key.Title,
                           PONumber = g.Key.PONumber,
                           VendorName = g.Key.VendorName,
                           TotalPrice = g.Sum(row => Convert.ToDouble((row["TotalPrice"])))
                       };
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("PONumber", typeof(string));
            dt.Columns.Add("VendorName", typeof(string));
            dt.Columns.Add("TotalPrice", typeof(double));
            foreach (var item in IEDR)
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = item.Title.ToString();
                dr["PONumber"] = item.PONumber.ToString();
                dr["VendorName"] = item.VendorName.ToString();
                dr["TotalPrice"] = item.TotalPrice.ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        void CreateExcel(DataTable dt)
        {
            string strFilePath = @"C:\Inetpub\wwwroot\wss\VirtualDirectories\8081\wpresources\002.xls";

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile();
            objExcelFile.LoadXls(strFilePath);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
            GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
            GemBox.Spreadsheet.MultipleBorders borderBottom = GemBox.Spreadsheet.MultipleBorders.Bottom;
            GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线
            System.Drawing.Color color = System.Drawing.Color.Black;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet1.Rows[i + 1].Cells[0].SetBorders(borderLeft, color, LineSyleThick);
                worksheet1.Rows[i + 1].Cells[3].SetBorders(borderRight, color, LineSyleThick);
                worksheet1.Rows[i + 1].Cells[0].Value = dt.Rows[i][0];
                worksheet1.Rows[i + 1].Cells[1].Value = dt.Rows[i][1];
                worksheet1.Rows[i + 1].Cells[2].Value = dt.Rows[i][2];
                worksheet1.Rows[i + 1].Cells[3].Value = dt.Rows[i][3];
                if (i == (dt.Rows.Count - 1))
                {
                    worksheet1.Rows[i + 1].Cells[0].SetBorders(borderBottom, color, LineSyleThick);
                    worksheet1.Rows[i + 1].Cells[1].SetBorders(borderBottom, color, LineSyleThick);
                    worksheet1.Rows[i + 1].Cells[2].SetBorders(borderBottom, color, LineSyleThick);
                    worksheet1.Rows[i + 1].Cells[3].SetBorders(borderBottom, color, LineSyleThick);
                }
            }
            objExcelFile.SaveXls(@"C:\Inetpub\wwwroot\wss\VirtualDirectories\8081\wpresources\001.xls");//@"C:\Inetpub\wwwroot\wss\VirtualDirectories\8081\wpresources\Temp01.xls");
        }
    }
}