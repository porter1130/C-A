namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Data;
    using System.IO;
    using Microsoft.SharePoint;
    using System.Web.UI.WebControls;
    using CodeArt.SharePoint.CamlQuery;
    using System.Collections.Generic;

    public partial class DataReport : UserControl
    {
        private readonly ObjectDataSource DSCodePR = new ObjectDataSource();
        private readonly ObjectDataSource DSVenderList = new ObjectDataSource();
        private readonly ObjectDataSource DSCostCenter = new ObjectDataSource();
        private readonly ObjectDataSource DSVenderCode = new ObjectDataSource();


        protected override void OnLoad(EventArgs e)
        {
            this.DSCodePR.ID = "PRSurceID";
            this.DSCodePR.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.DSCodePR.SelectMethod = "GetPRSource";
            this.Controls.Add(this.DSCodePR);

            this.DSVenderList.ID = "VenderListSourceID";
            this.DSVenderList.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.DSVenderList.SelectMethod = "GetVenderListDataTable";
            this.Controls.Add(this.DSVenderList);

            this.DSCostCenter.ID = "CostCenterSourceID";
            this.DSCostCenter.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.DSCostCenter.SelectMethod = "GetPOByCostCenterCalculate";
            this.Controls.Add(this.DSCostCenter);

            this.DSVenderCode.ID = "VenderCodeSourceID";
            this.DSVenderCode.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.DSVenderCode.SelectMethod = "GetPOByVenCodCalculate";
            this.Controls.Add(this.DSVenderCode);

            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string sFrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                string sTO = DateTime.Now.ToString("yyyy-MM-dd");
                CADateTimeFrom.SelectedDate = DateTime.Parse(sFrom);
                CADateTimeTo.SelectedDate = DateTime.Parse(sTO);
                Bind(sFrom, sTO);
            }
        }

        protected void btnReportPRPO_Click(object sender, EventArgs e)
        {
            if (null != Request.QueryString["TYPE"])
            {
                string sType = Request.QueryString["TYPE"];
                switch (sType)
                {
                    case "PRPOREPORT": ExportPRExcel();
                        break;
                    case "VENDERLIST": ExportVendersReport();
                        break;
                    case "POCOSTCENTER": ExportPRCalcultByCost();
                        break;
                    case "POVENDERCODE": ExportVendCodExcel();
                        break;
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string sFrom = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string sTo = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            Bind(sFrom, sTo);
        }

        protected void SPGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SPGridView1.PageIndex = e.NewPageIndex;
            this.SPGridView1.DataBind();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTO"></param>
        void Bind(string sFrom, string sTO)
        {
            string sTitle = string.Empty;
            if (null != Request.QueryString["TYPE"])
            {
                string sType = Request.QueryString["TYPE"];
                switch (sType)
                {
                    case "PRPOREPORT": PRPOGrid(sFrom, sTO); sTitle = "PR,PO report";
                        break;
                    case "VENDERLIST": VenderListGrid(); sTitle = "Vender List Report"; //VENDERS
                        break;
                    case "POCOSTCENTER": POCostCenterGrid(sFrom, sTO); sTitle = "PO list by cost center report"; BindCostCenter(); ;//POCOSTCENTER
                        break;
                    case "POVENDERCODE": POVendorCodGrid(sFrom, sTO); sTitle = "PO list by vendor report";//POVENDERCODE  POVendorCodGrid
                        break;
                }
            }
            SetTitle(sTitle);
        }

        /// <summary>
        /// 由DataTable绑定SPGridView
        /// </summary>
        /// <param name="dt"></param>
        void BindGridView(DataTable dt)
        {
            if (SPGridView1.Columns.Count > 0)
            {
                return;
            }
            foreach (DataColumn dc in dt.Columns)
            {
                BoundField bf = new BoundField();
                bf.DataField = dc.ColumnName;
                bf.HeaderText = dc.ColumnName;
                SPGridView1.Columns.Add(bf);
            }
        }

        /// <summary>
        /// 设置页面标题
        /// </summary>
        /// <param name="sTitle"></param>
        void SetTitle(string sTitle)
        {
            HiddenField HFTitle = this.Parent.FindControl("HFTitle") as HiddenField;
            HFTitle.Value = sTitle;
        }

        #region CreageExcel

        /// <summary>
        /// 生成excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sExcelName">xx.xls</param>
        void CreatExcel(DataTable dt, string sExcelName)
        {
            if (null == dt || dt.Rows.Count <= 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('无数据！')</script>");
                return;
            }

            string sFileName = sExcelName;
            string tmpPath = "/tmpfiles/PurchaseRequest/";
            string strPath = Server.MapPath(tmpPath);
            DirectoryInfo dinfo = new DirectoryInfo(strPath);
            if (!dinfo.Exists)
            {
                Directory.CreateDirectory(strPath);
            }

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile();
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets.Add("Sheet1");
            /*
            GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
            GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
            GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线*/
            System.Drawing.Color color = System.Drawing.Color.Black;
            int iRowCount = dt.Rows.Count;

            for (int i = 0; i < iRowCount; i++)
            {
                int iColumnCount = dt.Columns.Count;
                for (int c = 0; c < iColumnCount; c++)
                {
                    /*if (c == 0)
                    {
                        worksheet1.Rows[i + 1].Cells[c].SetBorders(borderLeft, color, LineSyleThick);
                    }
                    if (c == iColumnCount - 1)
                    {
                        worksheet1.Rows[i + 1].Cells[c].SetBorders(borderRight, color, LineSyleThick);
                    }*/
                    worksheet1.Rows[i + 1].Cells[c].Value = dt.Rows[i][c];
                }
            }
            List<string> listColumnName = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                listColumnName.Add(dc.ColumnName);
            }

            //worksheet1.Columns[1].Width = 10000;
            //worksheet1.Columns[2].Width = 10000;
            SetStyle(worksheet1, iRowCount, listColumnName);
            string sFileFullName = string.Concat(strPath, sFileName);
            objExcelFile.SaveXls(sFileFullName);

            SendExcelToClient(sFileFullName, sFileName);
        }

        /// <summary>
        /// 设置excel内的样式
        /// </summary>
        /// <param name="worksheet1"></param>
        /// <param name="iLastRowNO"></param>
        /// <param name="listColumnName"></param>
        void SetStyle(GemBox.Spreadsheet.ExcelWorksheet worksheet1, int iLastRowNO, List<string> listColumnName)
        {
            /*GemBox.Spreadsheet.MultipleBorders borderTop = GemBox.Spreadsheet.MultipleBorders.Top;
            GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
            GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
            GemBox.Spreadsheet.MultipleBorders borderBottom = GemBox.Spreadsheet.MultipleBorders.Bottom;*/

            System.Drawing.Color color = System.Drawing.Color.Black;
            // GemBox.Spreadsheet.LineStyle LineSyleThin = GemBox.Spreadsheet.LineStyle.Thin;//细线
            GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线

            GemBox.Spreadsheet.CellStyle cs14 = new GemBox.Spreadsheet.CellStyle();
            cs14.Font.Size = 200;
            cs14.HorizontalAlignment = GemBox.Spreadsheet.HorizontalAlignmentStyle.Center;

            /* GemBox.Spreadsheet.CellRange range = worksheet1.Cells.GetSubrangeAbsolute(0, 0, 0, 3);//合并单元格
             range.Merged = true;
             worksheet1.Rows[0].Cells[0].Value = "PRPOREPORT";
             worksheet1.Rows[0].Cells[0].SetBorders(borderTop, color, LineSyleThick);
             worksheet1.Rows[0].Cells[0].SetBorders(borderLeft, color, LineSyleThick);
             worksheet1.Rows[0].Cells[0].SetBorders(borderRight, color, LineSyleThick);
             worksheet1.Rows[0].Cells[0].Style.Font.Size=50;// = cs20;
             */
            int iCo = listColumnName.Count;
            for (int i = 0; i < iCo; i++)
            {

                worksheet1.Rows[0].Cells[i].Value = listColumnName[i];
                worksheet1.Rows[0].Cells[i].Style = cs14;
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
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/PurchaseRequest/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }
        #endregion

        #region PurchaseRequest

        /// <summary>
        /// 绑定PurchaseRequest数据
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTO"></param>
        void PRPOGrid(string sFrom, string sTO)
        {
            this.DSCodePR.SelectParameters.Clear();
            this.DSCodePR.SelectParameters.Add("from", DbType.String, sFrom);
            this.DSCodePR.SelectParameters.Add("to", DbType.String, sTO);

            //DataTable dtTemplate = CreatePRPOReportDT();
            BindPRPOGrid();// BindGridView(dtTemplate);
            this.SPGridView1.DataSourceID = "PRSurceID";
            this.SPGridView1.DataBind();
        }

        /// <summary>
        /// 为PRPO的报表添加可查看PO详细信息的链接。 
        /// </summary>
        void BindPRPOGrid()
        {
            if (SPGridView1.Columns.Count > 0)
            {
                return;
            }

            BoundField BFPRNO = new BoundField();
            BFPRNO.DataField = "PR Number";
            BFPRNO.HeaderText = "PR Number";
            SPGridView1.Columns.Add(BFPRNO);

            HyperLinkField HLFPONO = new HyperLinkField();
            HLFPONO.HeaderText = "PO Number";
            string[] pram = { "PO Number" };
            HLFPONO.DataNavigateUrlFields = pram;
            HLFPONO.DataNavigateUrlFormatString = "PORedirct.aspx?PONO={0}";//PORedirct
            HLFPONO.DataTextField = "PO Number";
            SPGridView1.Columns.Add(HLFPONO);


            BoundField BFCost = new BoundField();
            BFCost.DataField = "PO Cost";
            BFCost.HeaderText = "PO Cost";
            SPGridView1.Columns.Add(BFCost);

            BoundField BFVendor = new BoundField();
            BFVendor.DataField = "Vendor Name";
            BFVendor.HeaderText = "Vendor Name";
            SPGridView1.Columns.Add(BFVendor);

            BoundField BFIsGR = new BoundField();
            BFIsGR.DataField = "Is Complete GR/SR";
            BFIsGR.HeaderText = "Is Complete GR/SR";
            SPGridView1.Columns.Add(BFIsGR);
        }

        /// <summary>
        /// 得到PurchaseOrderItems 的datable
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        DataTable GetPOTable(string from, string to, string sCostCenter)
        {
            SPListItemCollection lc = null;
            SPQuery query = new SPQuery();

            string condition = GetPOQueryStr(from, to, sCostCenter);
            query.Query = condition;

            lc = string.IsNullOrEmpty(condition) ? SPContext.Current.Web.Lists["PurchaseOrderItems"].Items : SPContext.Current.Web.Lists["PurchaseOrderItems"].GetItems(query);
           
            DataTable dt = lc.GetDataTable();
            return dt;
        }
        //得到PurchaseOrderItems 
        string GetPOQueryStr(string from, string to, string sCostCenter)
        {
            string condition = string.Empty;
            if (string.IsNullOrEmpty(sCostCenter))//无CostCenter筛选
            {
                if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                {
                    condition = string.Format(@"<Where>
                                                  <Geq>
                                                     <FieldRef Name='Created' />
                                                     <Value Type='DateTime'>{0}</Value>
                                                  </Geq>
                                               </Where>", from);
                }
                else if (!string.IsNullOrEmpty(to) && string.IsNullOrEmpty(from))
                {
                    condition = string.Format(@"<Where>
                                                  <Leq>
                                                     <FieldRef Name='Created' />
                                                     <Value Type='DateTime'>{0}</Value>
                                                  </Leq>
                                               </Where>", to);
                }
                else if (!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(from))
                {
                    condition = string.Format(@"<Where>
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
                                               </Where>", from, to);
                }
            }
            else//有CostCenter筛选
            {
                if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                {
                    condition = string.Format(@"<Where>
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
                                                  </Where>", from, sCostCenter);
                }
                else if (!string.IsNullOrEmpty(to) && string.IsNullOrEmpty(from))
                {
                    condition = string.Format(@"<Where>
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
                                           </Where>", to, sCostCenter);
                }
                else if (!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(from))
                {
                    condition = string.Format(@"<Where>
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
                                           </Where>", from, to, sCostCenter);
                }
            }
            return condition;
        }



        /// <summary>
        /// 得到PurchaseRequest的datable
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        DataTable GetPRTable(string from, string to)
        {
            string condition = string.Empty;
            if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
            {
                condition = string.Format(@"<Where>
                                              <Geq>
                                                 <FieldRef Name='Created' />
                                                 <Value Type='DateTime'>{0}</Value>
                                              </Geq>
                                           </Where>
                                           <OrderBy>
                                              <FieldRef Name='PONumber' />
                                           </OrderBy>", from);
            }
            else if (!string.IsNullOrEmpty(to) && string.IsNullOrEmpty(from))
            {
                condition = string.Format(@"<Where>
                                              <Leq>
                                                 <FieldRef Name='Created' />
                                                 <Value Type='DateTime'>{0}</Value>
                                              </Leq>
                                           </Where>
                                           <OrderBy>
                                              <FieldRef Name='PONumber' />
                                           </OrderBy>", to);
            }
            else if (!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(from))
            {
                condition = string.Format(@"<Where>
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
                                           </Where>
                                           <OrderBy>
                                              <FieldRef Name='PONumber' />
                                           </OrderBy>", from, to);
            }

            SPListItemCollection lc = null;
            SPQuery query = new SPQuery();
            query.Query = condition;

            lc = string.IsNullOrEmpty(condition) ? SPContext.Current.Web.Lists["PurchaseRequestItems"].Items : SPContext.Current.Web.Lists["PurchaseRequestItems"].GetItems(query);
          
            DataTable dt = lc.GetDataTable();
            return dt;
        }



        /// <summary>
        /// 得到PruchaseRequest的统计 的datable
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        /// <returns></returns>
        DataTable GetPRPOCalculateTable(string sFrom, string sTo)
        {
            DataTable dt = new DataTable();
            DataTable myDT = CreatePRPOReportDT();/// new DataTable();
            dt = GetPRTable(sFrom, sTo);

            //myDT.Columns.Add("PR Number");
            //myDT.Columns.Add("PO Number");
            //myDT.Columns.Add("Vendor Name");
            //myDT.Columns.Add("PO Cost");
            //myDT.Columns.Add("Is Complete GR/SR");
            if (dt == null)
            {
                return myDT;
            }
            var IEDR = from dr in dt.AsEnumerable()
                       orderby dr["Title"] ascending
                       group dr by new { PONumber = dr.Field<string>("PONumber"), VendorName = dr.Field<string>("VendorName"), PRNumber = dr["Title"] } into g//, PONumber = dr["PONumber"], VendorName = dr.Field<string>("VendorName") } into g
                       select new
                       {
                           PONumber = g.Key.PONumber,
                           PRNumber = g.Key.PRNumber,
                           VendorName = g.Key.VendorName
                       };


            foreach (var item in IEDR)
            {
                DataRow dr = myDT.NewRow();
                string sPONumber = item.PONumber == null ? "" : item.PONumber.ToString();
                string sPRNumber = item.PRNumber == null ? "" : item.PRNumber.ToString();
                string sStatus = string.Empty;

                string sGrandTotal = string.Empty;

                dr["PR Number"] = sPRNumber;
                dr["PO Number"] = sPONumber;
                dr["Vendor Name"] = item.VendorName == null ? "" : item.VendorName.ToString();
                GetPayMentCondition(sPONumber, out sPONumber, out sStatus);
                dr["PO Cost"] = sPONumber;
                string sSelectCodition = string.Format("PONumber='{0}' and Title='{1}'", sPONumber, sPRNumber);
                DataRow[] drArray = dt.Select(sSelectCodition);
                dr["Is Complete GR/SR"] = IsPRPORecieved(drArray);
                myDT.Rows.Add(dr);
            }
            return myDT;
        }

        /// <summary>
        /// 构造 PR,PO Report的 Datatable
        /// </summary>
        /// <returns></returns>
        DataTable CreatePRPOReportDT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PR Number");
            dt.Columns.Add("PO Number");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("PO Cost");
            dt.Columns.Add("Is Complete GR/SR");
            return dt;
        }

        /// <summary>
        /// PR,PO记录是否己经完成收货。
        /// </summary>
        /// <param name="drArray"></param>
        /// <returns></returns>
        string IsPRPORecieved(DataRow[] drArray)
        {
            if (null == drArray || drArray.Length == 0)
            {
                return "";
            }
            string sIsRecieved = "NO";
            foreach (DataRow dr in drArray)
            {
                if (null != dr["IsReceived"] && dr["IsReceived"].ToString().Equals("1", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "YES";
                }
            }

            return sIsRecieved;

        }

        /// <summary>
        /// 绑定PurchaseRequest的数据
        /// </summary>
        public DataTable GetPRSource(string from, string to)
        {
            DataTable dt = new DataTable();
            dt = GetPRPOCalculateTable(from, to);
            return dt;
        }

        /// <summary>
        /// 导出excel报表
        /// </summary>
        void ExportPRExcel()
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            DataTable dt = GetPRPOCalculateTable(from, to);
            CreatExcel(dt, "PRPOReport.xls");
        }
        #endregion

        #region Vendors

        /// <summary>
        /// 绑定venders的数据
        /// </summary>
        void VenderListGrid()
        {

            CADateTimeFrom.Visible = false;
            CADateTimeTo.Visible = false;
            DataTable dt = CrateVenderListReportDT();// GetVenderListDataTable();
            BindGridView(dt);
            this.SPGridView1.DataSourceID = "VenderListSourceID";
            this.SPGridView1.DataBind();

        }

        /// <summary>
        /// 得到Vendorslist 的Datable
        /// </summary>
        /// <returns></returns>
        public DataTable GetVenderListDataTable()
        {
            DataTable dt = null;
            dt = SPContext.Current.Web.Lists["Vendors"].Items.GetDataTable();
             
            DataTable dtCalculate = CrateVenderListReportDT();
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtCalculate.NewRow();
                dr["Vendor ID"] = item["VendorId"];
                dr["Name"] = item["Title"];
                dr["Address"] = item["Address"];
                dr["Post Code"] = item["PostCode"];
                dr["Contact Person"] = item["ContactPerson"];
                dr["Phone"] = item["Phone"];
                dr["Fax"] = item["Fax"];
                dr["Email"] = item["Email"];
                dtCalculate.Rows.Add(dr);
            }
            return dtCalculate;
        }

        /// <summary>
        /// 构造 Vender List Report的 DataTable
        /// </summary>
        /// <returns></returns>
        DataTable CrateVenderListReportDT()
        {
            DataTable dtCalculate = new DataTable();
            dtCalculate.Columns.Add("Vendor ID");
            dtCalculate.Columns.Add("Name");
            dtCalculate.Columns.Add("Address");
            dtCalculate.Columns.Add("Post Code");
            dtCalculate.Columns.Add("Contact Person");
            dtCalculate.Columns.Add("Phone");
            dtCalculate.Columns.Add("Fax");
            dtCalculate.Columns.Add("Email");
            return dtCalculate;
        }

        /// <summary>
        /// 导出VendorsRequest报表
        /// </summary>
        void ExportVendersReport()
        {
            DataTable dt = GetVenderListDataTable();
            CreatExcel(dt, "VenderListReport.xls");
        }



        #endregion

        #region PO list by  Cost Center coding

        /// <summary>
        /// PO list by cost center report的数据源
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="sCostCenter"></param>
        /// <returns></returns>
        public DataTable GetPOByCostCenterCalculate(string from, string to, string sCostCenter)
        {
            DataTable dtCaml = new DataTable();

            dtCaml = GetPOTable(from, to, sCostCenter);
            if (dtCaml == null || dtCaml.Rows.Count <= 0)
            {
                return null;
            }

            var varDR = from dr in dtCaml.AsEnumerable()
                        orderby dr.Field<string>("CostCenter") ascending
                        group dr by new { PONumber = dr.Field<string>("Title"), CostCenter = dr.Field<string>("CostCenter"), VendorName = dr.Field<string>("VendorName") } into g
                        select new
                        {
                            CostCenter = g.Key.CostCenter,
                            PONumber = g.Key.PONumber,
                            VendorName = g.Key.VendorName,
                            Netprice = g.Sum(row => ConvertToDouble(row["TotalPrice"]) - ConvertToDouble(row["TaxRate"])),
                            TaxValue = g.Sum(row => ConvertToDouble(row["TaxRate"])),
                            Totalvalue = g.Sum(row => ConvertToDouble(row["TotalPrice"]))
                        };//net price, tax value, total value
            DataTable dtCalculate = CreateByCostCenter();

            foreach (var item in varDR)
            {
                string sPONO = item.PONumber;
                string sPaid = string.Empty;
                string sUnPaid = string.Empty;
                string sStatus = string.Empty;
                string[] PayMentDetails = GetPayMentDetails(sPONO);
                if (null != PayMentDetails)
                {
                    sPaid = PayMentDetails[0];
                    sUnPaid = PayMentDetails[1];
                }
                string sGrandTotal = string.Empty;
                DataRow dr = dtCalculate.NewRow();
                dr["PO Number"] = item.PONumber;
                dr["Cost Center"] = item.CostCenter;
                dr["PaymentCondition"] = GetPayMentCondition(sPONO, out sGrandTotal, out sStatus);///付款条件
                dr["Paid"] = sPaid;///己付
                dr["UnPaid"] = sUnPaid;///未付
                dr["PO Cost"] = sGrandTotal;// item.TotalPrice;
                dr["Status"] = sStatus;// item.TotalPrice;
                dr["VendorName"] = item.VendorName;
                dr["Netprice"] = item.Netprice;
                dr["TaxValue"] = item.TaxValue;
                dr["Totalvalue"] = item.Totalvalue;
                dtCalculate.Rows.Add(dr);
            }
            return dtCalculate;
        }

        /// <summary>
        /// 构造 PO list by  Cost Center coding报表的DataTable
        /// </summary>
        /// <returns></returns>
        DataTable CreateByCostCenter()
        {
            DataTable dtCalculate = new DataTable();
            dtCalculate.Columns.Add("PO Number");
            dtCalculate.Columns.Add("Cost Center");
            dtCalculate.Columns.Add("VendorName");
            dtCalculate.Columns.Add("PaymentCondition");
            dtCalculate.Columns.Add("Paid");
            dtCalculate.Columns.Add("UnPaid");
            dtCalculate.Columns.Add("PO Cost");
            dtCalculate.Columns.Add("Status");
            dtCalculate.Columns.Add("Netprice");
            dtCalculate.Columns.Add("TaxValue");
            dtCalculate.Columns.Add("Totalvalue");
            return dtCalculate;
        }

        /// <summary>
        /// 绑定PO cost Center Grid
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTO"></param>
        void POCostCenterGrid(string sFrom, string sTO)
        {
            string sCostCenter = DDLCostCenter.SelectedValue == null ? "" : DDLCostCenter.SelectedValue;
            this.DSCostCenter.SelectParameters.Clear();
            this.DSCostCenter.SelectParameters.Add("from", DbType.String, sFrom);
            this.DSCostCenter.SelectParameters.Add("to", DbType.String, sTO);
            this.DSCostCenter.SelectParameters.Add("sCostCenter", DbType.String, sCostCenter);//sCostCenter

            DataTable dt = CreateByCostCenter();// GetPOByCostCenterCalculate(sFrom, sTO,sCostCenter);// GetPRDTCalculate();
            BindGridView(dt);
            this.SPGridView1.DataSourceID = "CostCenterSourceID";
            this.SPGridView1.DataBind();
        }

        /// <summary>
        /// 得到 PO by cost Center Grid的数据
        /// </summary>
        void ExportPRCalcultByCost()
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            string sCostCenter = DDLCostCenter.SelectedValue == null ? "" : DDLCostCenter.SelectedValue;

            DataTable dt = GetPOByCostCenterCalculate(from, to, sCostCenter);
            CreatExcel(dt, "CenterCodingPOReport.xls");
        }


        /// <summary>
        /// 得到PO付款条件
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="TotalPrice"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        string GetPayMentCondition(string sPONO, out string TotalPrice, out string Status)
        {
            string sPayMentCondition = string.Empty;
            string sTotalPrice = string.Empty;
            string sStatus = string.Empty;
            SPQuery queryCamle = new SPQuery();
            queryCamle.Query = string.Format(@"
                                                <Where>
                                                    <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </Where>", sPONO);
            SPListItemCollection splic = SPContext.Current.Web.Lists["Purchase Order Workflow"].GetItems(queryCamle);
            foreach (SPListItem item in splic)
            {
                sPayMentCondition = item["PaymentCondition"] == null ? "" : item["PaymentCondition"].ToString();
                sTotalPrice = item["GrandTotal"] == null ? "0" : item["GrandTotal"].ToString();
                sStatus = item["Status"] == null ? "0" : item["Status"].ToString();
                break;
            }
            TotalPrice = sTotalPrice;
            Status = sStatus;
            return sPayMentCondition;
        }

        /// <summary>
        /// 得到己付和未付数据。
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        string[] GetPayMentDetails(string sPONO)
        {
            List<string> listPayMent = new List<string>();

            string[] arrayPayMent = new string[2];

            string sPayMentCondition = string.Empty;
           
            SPQuery queryCamle = new SPQuery();
            queryCamle.Query = string.Format(@"
                                                <Where>
                                                    <Eq>
                                                        <FieldRef Name='PONo' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </Where>", sPONO);
            SPListItemCollection splic = SPContext.Current.Web.Lists["PaymentInstallment"].GetItems(queryCamle);
            if (null == splic)
            {
                arrayPayMent = null;
            }
            System.Text.StringBuilder sbPaid = new System.Text.StringBuilder();
            decimal dTotalUnPay = 0;
            foreach (SPListItem item in splic)
            {
                string sIsPaid = item["IsPaid"] == null ? "" : item["IsPaid"].ToString();
                string sPaid = item["Paid"] == null ? "0" : item["Paid"].ToString();
                string sTotalAmount = item["TotalAmount"] == null ? "0" : item["TotalAmount"].ToString();

                decimal dPaid = 0;
                decimal dTotalAmount = 0;
                decimal.TryParse(sPaid, out dPaid);
                decimal.TryParse(sTotalAmount, out dTotalAmount);
                decimal dCurrentPay = Math.Round(dPaid / 100 * dTotalAmount, 2);
                if (sIsPaid == "True")//己付 
                {
                    if (sbPaid.Length > 0)
                    {
                        sbPaid.Append(" ,");
                    }
                    sbPaid.Append(dCurrentPay);
                }
                else//未付款
                {
                    dTotalUnPay += dCurrentPay;
                }
            }
            arrayPayMent[0] = sbPaid.ToString();
            arrayPayMent[1] = dTotalUnPay.ToString();
                    
            
            return arrayPayMent;
        }

        /// <summary>
        /// 绑定CostCenter
        /// </summary>
        void BindCostCenter()
        {
            if (DDLCostCenter.Items.Count > 0)
            {
                return;
            }
            else
            {
                DataTable dtCostCenter = new DataTable();
              
                SPQuery queryCamle = new SPQuery();
                queryCamle.Query = string.Format(@"  
                                                    <Where>
                                                        <Neq>
                                                            <FieldRef Name='IsActive' />
                                                            <Value Type='Boolean'>0</Value>
                                                        </Neq>
                                                    </Where>");
                SPListItemCollection splic = SPContext.Current.Web.Lists["Cost Centers"].GetItems(queryCamle);
                dtCostCenter = splic.GetDataTable();
                 
                DDLCostCenter.Visible = true;
                DDLCostCenter.DataSource = dtCostCenter;
                DDLCostCenter.DataTextField = "Title";
                DDLCostCenter.DataValueField = "Title";
                DDLCostCenter.DataBind();
                DDLCostCenter.Items.Insert(0, new ListItem("--Please select CostCenter--", ""));
            }
        }
        #endregion

        #region Po list by Vendor coding

        /// <summary>
        /// 绑定PO list by Vendor coding
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTO"></param>
        void POVendorCodGrid(string sFrom, string sTO)
        {
            this.DSVenderCode.SelectParameters.Clear();
            this.DSVenderCode.SelectParameters.Add("from", DbType.String, sFrom);
            this.DSVenderCode.SelectParameters.Add("to", DbType.String, sTO);
            this.DSVenderCode.SelectParameters.Add("sCostCenter", DbType.String, DDLCostCenter.SelectedValue);

            DataTable dt = new DataTable();
            dt = CreateByVendorReport();// GetPOByVenCodCalculate(sFrom, sTO);
            BindGridView(dt);
            SPGridView1.DataSourceID = "VenderCodeSourceID";
            SPGridView1.DataBind();
        }

        /// <summary>
        /// 导出PO list by Vendor coding的excel
        /// </summary>
        void ExportVendCodExcel()
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            DataTable dt = new DataTable();
            dt = GetPOByVenCodCalculate(from, to, "");
            CreatExcel(dt, "VenderCodingPOReport.xls");
        }

        /// <summary>
        /// 得到 Po list by Vendor coding 的统计数据
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="sCostCenter"></param>
        /// <returns></returns>
        public DataTable GetPOByVenCodCalculate(string from, string to, string sCostCenter)
        {
            DataTable dt = new DataTable();
            dt = GetPOTable(from, to, sCostCenter);

            if (null == dt || dt.Rows.Count <= 0)
            {
                return dt;
            }

            var Calculate = from dr in dt.AsEnumerable()
                            orderby dr.Field<string>("Title") ascending
                            group dr by new { VendorName = dr.Field<string>("VendorName"), PONO = dr.Field<string>("Title") } into g
                            select new
                            {
                                VendorName = g.Key.VendorName,
                                PONO = g.Key.PONO
                            };

            DataTable CalcuDt = CreateByVendorReport();
            //new DataTable();
            //CalcuDt.Columns.Add("PO Number");
            //CalcuDt.Columns.Add("Vendor Name");
            //CalcuDt.Columns.Add("PO Cost");

            foreach (var item in Calculate)
            {
                string sPONO = item.PONO;
                string sGrandTotal = string.Empty;
                string sStatus = string.Empty;
                GetPayMentCondition(sPONO, out sGrandTotal, out sStatus);
                DataRow dr = CalcuDt.NewRow();
                dr["PO Number"] = sPONO;
                dr["Vendor Name"] = item.VendorName;
                dr["PO Cost"] = sGrandTotal;
                CalcuDt.Rows.Add(dr);
            }

            return CalcuDt;
        }

        /// <summary>
        /// 构造Po List By Vendor Report的DataTable
        /// </summary>
        /// <returns></returns>
        DataTable CreateByVendorReport()
        {
            DataTable CalcuDt = new DataTable();
            CalcuDt.Columns.Add("PO Number");
            CalcuDt.Columns.Add("Vendor Name");
            CalcuDt.Columns.Add("PO Cost");
            return CalcuDt;
        }

        double ConvertToDouble(object obj)
        {
            double dValue = 0;
            if (obj == null)
            {
                return dValue;
            }

            double.TryParse(obj.ToString(), out dValue);
            return dValue;
        }

        #endregion
    }
}