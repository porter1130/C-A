using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Microsoft.SharePoint;
using System.Text;
using CodeArt.SharePoint.CamlQuery;

namespace CA.WorkFlow.UI.OSP
{
    public partial class OSPReport : CAWorkFlowPage
    {
        GemBox.Spreadsheet.LineStyle LineSyleThin = GemBox.Spreadsheet.LineStyle.Thin;//细线
        GemBox.Spreadsheet.LineStyle LineSyleMedium = GemBox.Spreadsheet.LineStyle.Medium;//
        GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线

        GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
        GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
        GemBox.Spreadsheet.MultipleBorders borderTop = GemBox.Spreadsheet.MultipleBorders.Top;
        GemBox.Spreadsheet.MultipleBorders borderBottom = GemBox.Spreadsheet.MultipleBorders.Bottom;

        System.Drawing.Color color = System.Drawing.Color.Black;

        private readonly ObjectDataSource odsOSP = new ObjectDataSource();


        protected override void OnLoad(EventArgs e)
        {
            this.odsOSP.ID = "OSPSurceID";
            this.odsOSP.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.odsOSP.SelectMethod = "GetOSPTable";
            this.Controls.Add(this.odsOSP);

            base.OnLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
            if (!this.IsPostBack)
            {
                string sFrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                string sTO = DateTime.Now.ToString("yyyy-MM-dd");
                CADateTimeFrom.SelectedDate = DateTime.Parse(sFrom);
                CADateTimeTo.SelectedDate = DateTime.Parse(sTO);
                Bind(sFrom, sTO);
            }
        }

        void CheckAccount()
        {
            
            // wf_HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!OSPCommon.IsInGroup(current, "wf_OSPReport" )&&!OSPCommon.isAdmin())
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }


        /// <summary>
        /// 绑定PurchaseRequest数据
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTO"></param>
        void Bind(string sFrom, string sTO)
        {
            this.odsOSP.SelectParameters.Clear();
            this.odsOSP.SelectParameters.Add("from", DbType.String, sFrom);
            this.odsOSP.SelectParameters.Add("to", DbType.String, sTO);

            this.SPGridViewOsp.DataSourceID = "OSPSurceID";
            this.SPGridViewOsp.DataBind();
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            Bind(from, to);
        }

        protected void SPGridViewOsp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SPGridViewOsp.PageIndex = e.NewPageIndex;
            SPGridViewOsp.DataBind();
        }
        /// <summary>
        /// 得到PurchaseOrderItems 的datable
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public DataTable GetOSPTable(string from, string to)
        {
            DataTable dtResult = DataColumn();

            SPListItemCollection lc = null;
            SPQuery query = new SPQuery();

            string condition = GetPOQueryStr(from, to);
            query.Query = condition;
            query.ViewFields = QueryFields();
            lc = string.IsNullOrEmpty(condition) ? SPContext.Current.Web.Lists["OSPItems"].Items : SPContext.Current.Web.Lists["OSPItems"].GetItems(query);

            DataTable dtSource = lc.GetDataTable();

            //dtSource.Columns.Add("Dep");
            dtResult.Columns.Add("Dep");

            DataTable dtDep=new DataTable();
            dtDep = GetDep(dtSource);//得到工作流编号所对应的DEP
            foreach (DataRow dr in dtSource.Rows)
            {
                string sOriginalOsp = ObjToString(dr["OriginalOsp"]);
                string sNewOSP = ObjToString(dr["NewOSP"]);

                decimal dOriginalOsp = 0;
                decimal dNewOSP = 0;
                if (!decimal.TryParse(sOriginalOsp, out dOriginalOsp) || !decimal.TryParse(sNewOSP, out dNewOSP))
                {
                    continue;
                }
                if (!(dOriginalOsp - dNewOSP > 0))
                {
                    continue;
                }

                string sWorkflowNO = dr["WorkflowNumber"] == null ? string.Empty : dr["WorkflowNumber"].ToString();
                string sDep = GetDepByWorkflowNO(dtDep, sWorkflowNO);

                DataRow drResult = dtResult.NewRow();
                foreach (string columnName in GetDataColumn())
                {
                    drResult[columnName] = dr[columnName];
                }
                drResult["Dep"] = sDep;

                dtResult.Rows.Add(drResult);
            }
            return dtResult;
        }

        //得到PurchaseOrderItems 
        string GetPOQueryStr(string from, string to)
        {

            //IsSuccess 
            string condition = string.Empty;
            if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
            {
                condition = string.Format(@"<Where>
                                                <And>
                                                    <Geq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime'>{0}</Value>
                                                    </Geq>
                                                    <Eq>
                                                        <FieldRef Name='IsSuccess' />
                                                        <Value Type='Text'>1</Value>
                                                    </Eq>
                                                </And>
                                           </Where>", from);
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
                                                        <FieldRef Name='IsSuccess' />
                                                        <Value Type='Text'>1</Value>
                                                    </Eq>
                                                </And>
                                            </Where>", to);
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
                                                        <FieldRef Name='IsSuccess' />
                                                        <Value Type='Text'>1</Value>
                                                    </Eq>
                                                </And>
                                            </Where>", from, to);
            }
            return condition;
        }

        string QueryFields()
        {
            StringBuilder sb = new StringBuilder();
            List<string> listColumn = GetDataColumn();
            foreach (string item in listColumn)
            {
                sb.Append(string.Format("<FieldRef Name='{0}'/>",item));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到数据源的列名
        /// </summary>
        /// <returns></returns>
        List<string> GetDataColumn()
        {
            List<string> listColumn = new List<string>();
            listColumn.Add("WorkflowNumber");
            listColumn.Add("CreatedBy");
            listColumn.Add("Created");
            listColumn.Add("Modified");
            listColumn.Add("Title");
            listColumn.Add("SubDiv");
            listColumn.Add("Class");
            listColumn.Add("PAD");
            listColumn.Add("SAD");
            listColumn.Add("OriginalOsp");
            listColumn.Add("CurrentOMU");
            listColumn.Add("NewOSP");
            listColumn.Add("NewOMU");
            listColumn.Add("PONO");
            listColumn.Add("Qty");
            listColumn.Add("OMUReduction");
            return listColumn;

        }

        DataTable DataColumn()
        {
            DataTable dt = new DataTable();
            List<string> listColumn = GetDataColumn();
            foreach (string item in listColumn)
            {
                dt.Columns.Add(item);
            }
            return dt;
        }

        /// <summary>
        /// 得到工作流创建者的部门集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable GetDep(DataTable dt)
        {
            DataTable dtDep = new DataTable();

            QueryField qWorkflowNumber = new QueryField("Title", false);
            CamlExpression exp = null;
            var workflowNOs = (from dr in dt.AsEnumerable()
                     select dr["WorkflowNumber"]).Distinct();
            foreach (var item in workflowNOs)
            {
                exp = WorkFlowUtil.LinkOr(exp, qWorkflowNumber.Equal(item)); 
            }
            
            SPListItemCollection lc = null;
            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='Title'/><FieldRef Name='Department'/>";
            query.Query = exp.ToString();

            //lc =ListQuery.Select()
            //      .From(SPContext.Current.Web.Lists["OSPWorkflow"])
            //       .Where(exp)
            //       .GetItems();
            lc = SPContext.Current.Web.Lists["OSPWorkflow"].GetItems(query);
            if (lc != null)
            {
                dtDep= lc.GetDataTable();
            }
            return dtDep;
        }

        /// <summary>
        /// 由工作流编号得到 Dep
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sWorkflowNO"></param>
        /// <returns></returns>
        string GetDepByWorkflowNO(DataTable dt,string sWorkflowNO)
        {
            string sDep = string.Empty;
            var Deps = from dr in dt.AsEnumerable()
                       where ObjToString(dr["Title"]).Equals(sWorkflowNO)
                              select dr["Department"];
            foreach (var item in Deps)
            {
                sDep = ObjToString(item);
            }
            return sDep;
        }

        string ObjToString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            string from = this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            DataTable dt= GetOSPTable(from,to);
            CreateExcel(dt);
        }
        void CreateExcel(DataTable dt)
        {
            string strSampleFileName = "OSPSample.xls";
            string sSaveFileName = "OSP.xls";

            string sFullPath = Server.MapPath("/tmpfiles/OSP/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            DataColumnCollection dcc = dt.Columns;

            SetExcelTitle(dcc, worksheet1);
            int iCount = dt.Rows.Count;
            int iCopyCount=iCount - 3;
            if (iCopyCount <= 0)
            {
                iCopyCount = 1;
            }
            worksheet1.Rows[2].InsertCopy(iCopyCount, worksheet1.Rows[2]);
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

            worksheet.Rows[0].Cells[0].Value = string.Format("OSP Report({0} - {1})", from, to);
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

            worksheet.Rows[1].Cells[2].Value = "Creation date";
            worksheet.Rows[1].Cells[3].Value = "Completion date";
            worksheet.Rows[1].Cells[4].Value = "Style No.";
            worksheet.Rows[1].Cells[10].Value = "Original OMU";
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
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/OSP/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }

    }
}