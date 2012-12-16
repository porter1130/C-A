using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using System.Text;
using System.Data;

namespace CA.WorkFlow.UI.POTypeChange
{
    public partial class Report : System.Web.UI.Page
    {

        GemBox.Spreadsheet.LineStyle LineSyleThin = GemBox.Spreadsheet.LineStyle.Thin;//细线
        GemBox.Spreadsheet.LineStyle LineSyleMedium = GemBox.Spreadsheet.LineStyle.Medium;//
        GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线

        GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
        GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
        GemBox.Spreadsheet.MultipleBorders borderTop = GemBox.Spreadsheet.MultipleBorders.Top;
        GemBox.Spreadsheet.MultipleBorders borderBottom = GemBox.Spreadsheet.MultipleBorders.Bottom;

        System.Drawing.Color color = System.Drawing.Color.Black;
        private readonly ObjectDataSource ods = new ObjectDataSource();

        protected override void OnLoad(EventArgs e)
        {
            this.ods.ID = "PRSurceID";
            this.ods.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.ods.SelectMethod = "GetDataSource";
            this.Controls.Add(this.ods);

            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            Inite();

            string sfrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            string sto = DateTime.Now.ToString("yyyy-MM-dd");
            BindData(sfrom,sto);
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string sFrom = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string sTo = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            BindData(sFrom,sTo);
        }

        protected void SPGridViewReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SPGridViewReport.PageIndex = e.NewPageIndex;
            this.SPGridViewReport.DataBind();
        }

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            string sFrom = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string sTo = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            DataTable dt= GetDataSource(sFrom,sTo, DDLType.SelectedValue);
            CreateExcel(dt);
        }

        /// <summary>
        /// 设置日期
        /// </summary>
        void Inite()
        {
            CADateTimeFrom.SelectedDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("MM/dd/yyy"));
            CADateTimeTo.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyy"));
        }

        /// <summary>
        /// 得到查询条件
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        /// <param name="sNewTypeVal"></param>
        /// <returns></returns>
        string Query(string sFrom, string sTo,string sNewTypeVal)
        {
            string scondition = string.Empty;//IsSuccess
            if (!string.IsNullOrEmpty(sFrom) && !string.IsNullOrEmpty(sTo))
            {
                scondition = string.Format(@"<Where>
                                                    <And>
                                                        <And>
                                                            <And>
                                                                <Geq>
                                                                    <FieldRef Name='Modified' />
                                                                    <Value Type='DateTime'>{0}</Value>
                                                                </Geq>
                                                                <Leq>
                                                                    <FieldRef Name='Modified' />
                                                                    <Value Type='DateTime'>{1}</Value>
                                                                </Leq>
                                                            </And>
                                                            <Eq>
                                                                <FieldRef Name='NewTypeValue' />
                                                                <Value Type='Text'>{2}</Value>
                                                            </Eq>
                                                        </And>
                                                        <Eq>
                                                            <FieldRef Name='IsSuccess' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                    </And>
                                               </Where>", sFrom, sTo,  sNewTypeVal);
            }
            else if (string.IsNullOrEmpty(sTo) && !string.IsNullOrEmpty(sFrom))
            {
                scondition = string.Format(@"<Where>
                                                    <And>
                                                        <And>
                                                            <Geq>
                                                                <FieldRef Name='Modified' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Geq>
                                                            <Eq>
                                                                <FieldRef Name='NewTypeValue' />
                                                                <Value Type='Text'>{1}</Value>
                                                            </Eq>
                                                        </And>
                                                        <Eq>
                                                            <FieldRef Name='IsSuccess' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                    </And>
                                              </Where>", sFrom, sNewTypeVal);
                
            }
            else if (!string.IsNullOrEmpty(sTo) && string.IsNullOrEmpty(sFrom))
            {
                scondition = string.Format(@"<Where>
                                                  <And>
                                                        <And>
                                                            <Leq>
                                                                <FieldRef Name='Modified' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Leq>
                                                            <Eq>
                                                                <FieldRef Name='NewTypeValue' />
                                                                <Value Type='Text'>{1}</Value>
                                                            </Eq>
                                                        </And>
                                                        <Eq>
                                                            <FieldRef Name='IsSuccess' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                    </And>
                                              </Where>", sTo,sNewTypeVal);
                
            }
            else
            {
                scondition = string.Format(@"<Where><And><Eq><FieldRef Name='NewTypeValue' /><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='IsSuccess' /><Value Type='Boolean'>1</Value></Eq></And></Where>", sNewTypeVal);
            }
            return scondition;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        void BindData(string sFrom, string sTo)
        {
            this.ods.SelectParameters.Clear();
            this.ods.SelectParameters.Add("sFrom", DbType.String, sFrom);
            this.ods.SelectParameters.Add("sTo", DbType.String, sTo);
            this.ods.SelectParameters.Add("sNewTypeVal", DbType.String, DDLType.SelectedValue);

            CreateGridColumn();

            this.SPGridViewReport.DataSourceID = "PRSurceID";
            this.SPGridViewReport.DataBind();
        }

        /// <summary>
        /// 创建gridview的列
        /// </summary>
        void CreateGridColumn()
        {
            if (SPGridViewReport.Columns.Count > 0)
            {
                return;
            }
            foreach (string str in GetDataColumns())
            {
                BoundField bf = new BoundField();
                bf.DataField = str;
                bf.HeaderText = str;
                SPGridViewReport.Columns.Add(bf);
            }
        }

        public DataTable GetDataSource(string sFrom, string sTo, string sNewTypeVal)
        {
            DataTable dt = CreateDataTable();
            SPQuery query = new SPQuery();
            query.Query = Query(sFrom, sTo, sNewTypeVal);
            query.ViewFields = GetField();

            SPListItemCollection splic = SPContext.Current.Web.Lists["POTypeChangeItems"].GetItems(query);
            if (splic != null && splic.Count > 0)
            {
                foreach (SPListItem item in splic)
                {
                    DataRow dr = dt.NewRow();
                    dr["Date"] = item["Modified"];
                    dr["PONb"] = item["Title"];
                    //[""] = item["DIV"];
                    //dr[""] = item["SubDIV"];
                    //dr[""] = item["class"];
                    dr["PAD"] = item["PAD"];
                    dr["SAD"] = item["SAD"];
                    dr["OMU"] = item["OMU"];
                    dr["Qty"] = item["Qty"];
                    dr["NewPOType"] = item["NewType"];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }


        /// <summary>
        /// 得到报表要显示的字段
        /// </summary>
        /// <returns></returns>
        string GetField()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldRef Name='Modified'/>");
            sb.Append("<FieldRef Name='Title'/>");
            //sb.Append("<FieldRef Name='DIV'/>");///?
            //sb.Append("<FieldRef Name='SubDIV'/>");//?
            //sb.Append("<FieldRef Name='class'/>");//?
            sb.Append("<FieldRef Name='PAD'/>");
            sb.Append("<FieldRef Name='SAD'/>");
            sb.Append("<FieldRef Name='OMU'/>");
            sb.Append("<FieldRef Name='Qty'/>");
            sb.Append("<FieldRef Name='NewType'/>");
            return sb.ToString();
        }

        /// <summary>
        /// 构造数据datatable
        /// </summary>
        /// <returns></returns>
        DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            List<string> listColumn = GetDataColumns();
            foreach (string item in listColumn)
            {
                dt.Columns.Add(item);
            }
            return dt;
        }

        /// <summary>
        /// 构造数据列
        /// </summary>
        /// <returns></returns>
        List<string> GetDataColumns()
        {
            List<string> listStr = new List<string>();
            listStr.Add("Date");
            listStr.Add("PONb");
            //listStr.Add("Div");
            //listStr.Add("Sub-div");
            //listStr.Add("Class");
            listStr.Add("PAD");
            listStr.Add("SAD");
            listStr.Add("OMU");
            listStr.Add("Qty");
            listStr.Add("NewPOType");
            return listStr;
        }


        void CreateExcel(DataTable dt)
        {
            string strSampleFileName = "POTypeChangeSample.xls";
            string sSaveFileName = "POTypeChangeReport.xls";

            string sFullPath = Server.MapPath("/tmpfiles/POTypeChangeWorkflow/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            DataColumnCollection dcc = dt.Columns;

            SetExcelTitle(dcc, worksheet1);
            int iCount = dt.Rows.Count;
            int iCopycount = 1;
            if (iCount - 3 >0)
            { 
                iCopycount=iCount - 3;
            }
            worksheet1.Rows[2].InsertCopy(iCopycount, worksheet1.Rows[2]);
            SetContent(dt, worksheet1);


            string sSavePath = string.Concat(sFullPath, sSaveFileName);
            objExcelFile.SaveXls(sSavePath);
            SendExcelToClient(sSavePath, sSaveFileName);
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

            worksheet.Rows[0].Cells[0].Value = string.Format("POType Change Report ({0} - {1})", from, to);
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
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/POTypeChangeWorkflow/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }
    }
}