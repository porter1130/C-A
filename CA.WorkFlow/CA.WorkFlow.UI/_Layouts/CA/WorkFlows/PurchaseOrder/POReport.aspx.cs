using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using QuickFlow.UI.ApplicationPages;
using System.Data;
using System.Text;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.PurchaseOrder
{
    public partial class POReport : LayoutsPageBase
    {
        GemBox.Spreadsheet.LineStyle LineSyleThin = GemBox.Spreadsheet.LineStyle.Thin;//细线
        GemBox.Spreadsheet.LineStyle LineSyleMedium = GemBox.Spreadsheet.LineStyle.Medium;//
        GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线
        GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
        GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
        GemBox.Spreadsheet.MultipleBorders borderTop = GemBox.Spreadsheet.MultipleBorders.Top;
        GemBox.Spreadsheet.MultipleBorders borderBottom = GemBox.Spreadsheet.MultipleBorders.Bottom;
        System.Drawing.Color color = System.Drawing.Color.Black;

        private readonly ObjectDataSource DSCodePO = new ObjectDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DSCodePO.ID = "POSurceID";
            this.DSCodePO.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.DSCodePO.SelectMethod = "GetReportata";
            this.Controls.Add(this.DSCodePO);

            if (IsPostBack)
            {
                return;
            }
            string sFrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            string sTO = DateTime.Now.ToString("yyyy-MM-dd");
            CADateTimeFrom.SelectedDate = DateTime.Parse(sFrom);
            CADateTimeTo.SelectedDate = DateTime.Parse(sTO);
            BindCostCenter();
            Bind(sFrom, sTO);
        }

       /// <summary>
        /// 绑定数据
       /// </summary>
       /// <param name="sFrom"></param>
       /// <param name="sTo"></param>
        void Bind(string sFrom, string sTo)
        {
            string sCostcenter = DDLCostCenter.SelectedValue;
            this.DSCodePO.SelectParameters.Clear();
            this.DSCodePO.SelectParameters.Add("sFrom", DbType.String, sFrom);
            this.DSCodePO.SelectParameters.Add("sTo", DbType.String, sTo);
            this.DSCodePO.SelectParameters.Add("sCostcenter", DbType.String, sCostcenter);

            CreateColumnforGridControl();
            SPGridView1.DataSourceID = "POSurceID";/// GetReportata(dt);
            SPGridView1.DataBind();
        }

        /// <summary>
        /// 为数据呈现控件绑定数据列。 
        /// </summary>
        void CreateColumnforGridControl()
        {
            if (SPGridView1.Columns.Count > 0)
            {
                return;
            }
            DataTable dt = new DataTable();
            dt = GetCreateDataColumn();
            foreach (DataColumn dc in dt.Columns)
            {
                BoundField bf = new BoundField();
                bf.DataField = dc.ColumnName;
                bf.HeaderText = dc.ColumnName;
                SPGridView1.Columns.Add(bf);
            }

        }

     /// <summary>
     /// 构造报表数据源
     /// </summary>
     /// <param name="sFrom"></param>
     /// <param name="sTo"></param>
     /// <returns></returns>
      public  DataTable GetReportata(string sFrom,string sTo,string sCostcenter)
      {
            DataTable dt = new DataTable();
            dt = GetItemDataFromList(sFrom, sTo, sCostcenter);

            if (null == dt || dt.Rows.Count == 0)
            {
                return null;
            }
            var dtResul = from dr in dt.AsEnumerable()
                    group dr by new
                    {
                        PONO = dr["Title"],
                        ItemCode = dr["ItemCode"],
                        CostCenter = dr["CostCenter"],
                        Description = dr["Description"],
                        UnitPrice = dr["UnitPrice"],
                        VendorName = dr["VendorName"]
                    } into g
                    select new
                    {
                        PONO =g.Key.PONO,
                        ItemCode = g.Key.ItemCode,
                        CostCenter =g.Key.CostCenter,
                        Description =g.Key.Description,
                        RequestQuantity = g.Sum(row => Convert.ToDouble(row["RequestQuantity"])),//g.Key.RequestQuantity,
                        UnitPrice =g.Key.UnitPrice,
                        TotalPrice = g.Sum(row => Convert.ToDouble(row["TotalPrice"])),//g.Key.TotalPrice,
                        VendorName =g.Key.VendorName
                    };

            DataTable dtDatasour = GetCreateDataColumn();

            DataTable dtPOEWF = GetPOEWFData(sFrom, sTo);
            foreach (var item in dtResul)
            {
                DataRow dr = dtDatasour.NewRow();
                string sPO = item.PONO.ToString();
                DataRow[] drcolle = dtPOEWF.Select(string.Format("Title='{0}'", sPO));
                if (drcolle != null && drcolle.Count() > 0)
                {
                    string sDate = drcolle[0]["IssuedDate"] == null ? string.Empty : drcolle[0]["IssuedDate"].ToString();
                    DateTime time = DateTime.Now;
                    if (DateTime.TryParse(sDate, out time))
                    {
                        dr["IssuedDate"] = time.ToString("yyyy-MM-dd");
                    }

                    dr["Delivery Date"] = drcolle[0]["DeliveryDate"] == null ? string.Empty : drcolle[0]["DeliveryDate"].ToString();

                    dr["PO No."] = item.PONO;
                    dr["Item Code"] = item.ItemCode;
                    dr["Cost Center"] = item.CostCenter;
                    dr["Description"] = item.Description;
                    dr["Request Quantity"] = item.RequestQuantity;
                    dr["Unit Price"] = item.UnitPrice;
                    dr["Total Price"] = item.TotalPrice;
                    dr["Vendor Name"] = item.VendorName;
                    dtDatasour.Rows.Add(dr);
                }
            }
            return dtDatasour;
        }

        /// <summary>
        /// 得到需要绑定的数据源的列。 
        /// </summary>
        /// <returns></returns>
        DataTable GetCreateDataColumn()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PO No.");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Cost Center");
            dt.Columns.Add("Description");
            dt.Columns.Add("IssuedDate");
            dt.Columns.Add("Delivery Date");
            dt.Columns.Add("Request Quantity");
            dt.Columns.Add("Unit Price");
            dt.Columns.Add("Total Price");
            dt.Columns.Add("Vendor Name");
            return dt;
        }


        /// <summary>
        /// 从PO子list中得到数据
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        /// <returns></returns>
        DataTable GetItemDataFromList(string sFrom, string sTo,string sCostcenter)
        {
            DataTable dtResult = new DataTable();
            string sCamel = GetCamel(sFrom, sTo, sCostcenter);
            SPQuery query = new SPQuery();
            query.Query = sCamel;
            query.ViewFields = GetPolistDataFileds();
            dtResult = SPContext.Current.Web.Lists["PurchaseOrderItems"].GetItems(query).GetDataTable();
            return dtResult;
        }

        DataTable GetPOEWFData(string sFrom, string sTo)
        {
            DataTable dtResult = new DataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        string sCamel = GetEWFCamel(sFrom, sTo);
                        SPQuery query = new SPQuery();
                        query.Query = sCamel;
                        query.ViewFields = GetPOWEFFileds();
                        dtResult = web.Lists["Purchase Order Workflow"].GetItems(query).GetDataTable();
                    }
                }
            });
            return dtResult;
        }
        /// <summary>
        /// PurchaseOrderItems中所要查的字段
        /// </summary>
        /// <returns></returns>
        string GetPolistDataFileds()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldRef Name='Title'/>");
            sb.Append("<FieldRef Name='ItemCode'/>");
            sb.Append("<FieldRef Name='CostCenter'/>");
            sb.Append("<FieldRef Name='Description'/>");
            sb.Append("<FieldRef Name='RequestQuantity'/>");
            sb.Append("<FieldRef Name='UnitPrice'/>");
            sb.Append("<FieldRef Name='TotalPrice'/>");
            sb.Append("<FieldRef Name='VendorName'/>");
            return sb.ToString();
        }

        string GetPOWEFFileds()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldRef Name='Title'/>");
            sb.Append("<FieldRef Name='IssuedDate'/>");
            sb.Append("<FieldRef Name='DeliveryDate'/>");
            return sb.ToString();
        }


        
        /// <summary>
        /// 得到日期时间段,Costcenter的查询条件。 
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        /// <param name="sCostcenter"></param>
        /// <returns></returns>
        string GetCamel(string sFrom, string sTo,string sCostcenter)
        {
            string scondition = string.Empty;
            if (sCostcenter == "-1")
            {
                #region  条件
                if (!string.IsNullOrEmpty(sFrom) && string.IsNullOrEmpty(sTo))
                {
                    scondition = string.Format(@"<Where>
                                                <Geq>
                                                    <FieldRef Name='Created' />
                                                    <Value Type='DateTime'>{0}</Value>
                                                </Geq>
                                            </Where>", sFrom);

                }
                else if (!string.IsNullOrEmpty(sTo) && string.IsNullOrEmpty(sFrom))
                {
                    scondition = string.Format(@"<Where>
                                                    <Leq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime'>{0}</Value>
                                                    </Leq>
                                            </Where>", sTo);
                }
                else if (!string.IsNullOrEmpty(sTo) && !string.IsNullOrEmpty(sFrom))
                {
                    scondition = string.Format(@"<Where>
                                                    <And>
                                                        <Geq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                        </Geq>
                                                        <Leq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Leq>
                                                    </And>
                                                </Where>", sFrom, sTo);
                }
                #endregion
            }
            else
            {
                //CostCenter

                #region  条件
                if (!string.IsNullOrEmpty(sFrom) && string.IsNullOrEmpty(sTo))
                {
                    scondition = string.Format(@"<Where>
                                                    <And>
                                                        <Geq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                        </Geq>
                                                        <Eq>
                                                            <FieldRef Name='CostCenter' />
                                                            <Value Type='Text'>{1}</Value>
                                                        </Eq>
                                                    </And>
                                                 </Where>", sFrom,sCostcenter);

                }
                else if (!string.IsNullOrEmpty(sTo) && string.IsNullOrEmpty(sFrom))
                {
                    scondition = string.Format(@"<Where>
                                                    <And>
                                                        <Leq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                        </Leq>
                                                        <Eq>
                                                            <FieldRef Name='CostCenter' />
                                                            <Value Type='Text'>{1}</Value>
                                                        </Eq>
                                                    </And>
                                                </Where>", sTo,sCostcenter);
                }
                else if (!string.IsNullOrEmpty(sTo) && !string.IsNullOrEmpty(sFrom))
                {
                    scondition = string.Format(@"<Where>
                                                    <And>
                                                        <And>
                                                            <Geq>
                                                                <FieldRef Name='Created' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Geq>
                                                            <Leq>
                                                                <FieldRef Name='Created' />
                                                                <Value Type='DateTime'>{1}</Value>
                                                            </Leq>
                                                        </And>
                                                        <Eq>
                                                            <FieldRef Name='CostCenter' />
                                                            <Value Type='Text'>{2}</Value>
                                                        </Eq>
                                                    </And>
                                                </Where>", sFrom, sTo,sCostcenter);
                }
                #endregion
            }

            return scondition;
        }

        /// <summary>
        /// 得到日期时间段时的查询条件。
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        /// <returns></returns>
        string GetEWFCamel(string sFrom, string sTo)
        {
            string scondition = string.Empty;
            #region  条件
                if (!string.IsNullOrEmpty(sFrom) && string.IsNullOrEmpty(sTo))
                {
                    scondition = string.Format(@"<Where>
                                                <Geq>
                                                    <FieldRef Name='Created' />
                                                    <Value Type='DateTime'>{0}</Value>
                                                </Geq>
                                            </Where>", sFrom);

                }
                else if (!string.IsNullOrEmpty(sTo) && string.IsNullOrEmpty(sFrom))
                {
                    scondition = string.Format(@"<Where>
                                                    <Leq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime'>{0}</Value>
                                                    </Leq>
                                            </Where>", sTo);
                }
                else if (!string.IsNullOrEmpty(sTo) && !string.IsNullOrEmpty(sFrom))
                {
                    scondition = string.Format(@"<Where>
                                                    <And>
                                                        <Geq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                        </Geq>
                                                        <Leq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Leq>
                                                    </And>
                                                </Where>", sFrom, sTo);
                }
                #endregion
            return scondition;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string sFrom = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string sTO = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            
            Bind(sFrom, sTO);
        }

        protected void btnReportPRPO_Click(object sender, EventArgs e)
        {
            string sFrom = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string sTO = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            
            DataTable dt = new DataTable();
            CreateColumnforGridControl();
            string sCostcenter = DDLCostCenter.SelectedValue;
            dt = GetReportata(sFrom, sTO, sCostcenter);
            if (null == dt || dt.Rows.Count == 0)
            {
                return;

            }
            CreateExcel(dt);
        }

        void CreateExcel(DataTable dt)
        {
            string strSampleFileName = "POReportSample.xls";
            string sSaveFileName = "POReport.xls";

            string sFullPath = Server.MapPath("/tmpfiles/PurchaseOrder/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            DataColumnCollection dcc = dt.Columns;

            SetExcelTitle(dcc, worksheet1);
            int iCount = dt.Rows.Count;
            worksheet1.Rows[2].InsertCopy((iCount - 3), worksheet1.Rows[2]);
            SetContent(dt, worksheet1);


            string sSavePath = string.Concat(sFullPath, sSaveFileName);
            objExcelFile.SaveXls(sSavePath);
            SendExcelToClient(sSavePath, sSaveFileName);
        }

        void SetContent(DataTable dt, GemBox.Spreadsheet.ExcelWorksheet worksheet)
        {
            int iCount = dt.Rows.Count;
            int iColumnCount = dt.Columns.Count;
            DataColumnCollection dcc = dt.Columns;
            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < iColumnCount; j++)
                {
                    int iRowNO = i + 2;
                    if (j == 0)//第一列
                    {
                        worksheet.Rows[iRowNO].Cells[j].SetBorders(borderLeft, color, LineSyleThick);
                    }
                    else if (j == (iColumnCount - 1))//最后一列
                    {
                        worksheet.Rows[iRowNO].Cells[j].SetBorders(borderRight, color, LineSyleThick);
                        worksheet.Rows[iRowNO].Cells[j].SetBorders(borderLeft, color, LineSyleThin);
                    }
                    else//中间列
                    {
                        worksheet.Rows[iRowNO].Cells[j].SetBorders(borderLeft, color, LineSyleThin);
                    }

                    if (i != (iCount - 1))
                    {
                        worksheet.Rows[iRowNO].Cells[j].SetBorders(borderBottom, color, LineSyleThin);
                    }
                    else //最后一行。
                    {
                        worksheet.Rows[iRowNO].Cells[j].SetBorders(borderBottom, color, LineSyleThick);
                    }

                    worksheet.Rows[iRowNO].Cells[j].Value = dt.Rows[i][j];
                }
            }
        }


        /// <summary>
        /// 向户端发送生成的Excel
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="sFileName"></param>
        void SendExcelToClient(string sFileFullName, string sFileName)
        {
            string sApplicationPath = Request.ApplicationPath;
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/PurchaseOrder/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }

        /// <summary>
        /// 设置表格的title
        /// </summary>
        /// <param name="dcc"></param>
        /// <param name="worksheet"></param>
        void SetExcelTitle(DataColumnCollection dcc, GemBox.Spreadsheet.ExcelWorksheet worksheet)
        {
            int iColumnCount = dcc.Count;

            GemBox.Spreadsheet.CellRange range = worksheet.Cells.GetSubrangeAbsolute(0, 0, 0, iColumnCount - 1);//合并单元格
            range.Merged = true;
            worksheet.Rows[0].Cells[0].SetBorders(borderLeft, color, LineSyleThick);
            worksheet.Rows[0].Cells[0].SetBorders(borderRight, color, LineSyleThick);
            worksheet.Rows[0].Cells[0].SetBorders(borderTop, color, LineSyleThick);
            worksheet.Rows[0].Cells[0].SetBorders(borderBottom, color, LineSyleThick);

            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            worksheet.Rows[0].Cells[0].Value = string.Format("Nontrade List Report ({0} - {1})", from, to);
            for (int i = 0; i < iColumnCount; i++)
            {
                if (i == 0)
                {
                    worksheet.Rows[1].Cells[i].SetBorders(borderLeft, color, LineSyleThick);
                }
                else if (i == iColumnCount - 1)
                {
                    worksheet.Rows[1].Cells[i].SetBorders(borderLeft, color, LineSyleThin);
                    worksheet.Rows[1].Cells[i].SetBorders(borderRight, color, LineSyleThick);
                }
                else
                {
                    worksheet.Rows[1].Cells[i].SetBorders(borderLeft, color, LineSyleThin);
                }

                worksheet.Rows[1].Cells[i].SetBorders(borderTop, color, LineSyleThick);
                worksheet.Rows[1].Cells[i].SetBorders(borderBottom, color, LineSyleMedium);

                worksheet.Rows[1].Cells[i].Value = dcc[i].ColumnName;
            }
            //worksheet.Rows[1].Cells[iColumnCount - 1].SetBorders(borderRight, color, LineSyleThick);
        }


        protected void SPGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SPGridView1.PageIndex = e.NewPageIndex;
            this.SPGridView1.DataBind();
        }

        void BindCostCenter()
        { 
            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='Title'/>";

            DataTable dt = SPContext.Current.Web.Lists["Cost Centers"].GetItems(query).GetDataTable();
            DDLCostCenter.DataSource = dt;
            DDLCostCenter.DataTextField = "Title";
            DDLCostCenter.DataValueField = "Title";
            DDLCostCenter.DataBind();
            DDLCostCenter.Items.Insert(0,new ListItem("--Please select costcenter--","-1"));
        }

    }
}