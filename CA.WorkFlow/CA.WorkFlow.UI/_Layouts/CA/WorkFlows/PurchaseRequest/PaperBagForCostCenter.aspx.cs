using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using System.IO;
using System.Text;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class PaperBagForCostCenter : Microsoft.SharePoint.WebControls.LayoutsPageBase
    {
         
        GemBox.Spreadsheet.LineStyle LineSyleThin = GemBox.Spreadsheet.LineStyle.Thin;//细线
        GemBox.Spreadsheet.LineStyle LineSyleMedium = GemBox.Spreadsheet.LineStyle.Medium;//
        GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Thick;//粗实线

        GemBox.Spreadsheet.MultipleBorders borderLeft = GemBox.Spreadsheet.MultipleBorders.Left;
        GemBox.Spreadsheet.MultipleBorders borderRight = GemBox.Spreadsheet.MultipleBorders.Right;
        GemBox.Spreadsheet.MultipleBorders borderTop = GemBox.Spreadsheet.MultipleBorders.Top;
        GemBox.Spreadsheet.MultipleBorders borderBottom = GemBox.Spreadsheet.MultipleBorders.Bottom;

        System.Drawing.Color color = System.Drawing.Color.Black;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
            if (IsPostBack)
            {
                return;
            }
            Inite();

            string from =  this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            Bind(from, to);
        }
        void Inite()
        {
            if (null != Request.QueryString["Log"])
            {
                ButtonViewLog.Visible = true;
                LiteralLog.Visible = true;
            }
            else
            {
                ButtonViewLog.Visible = false;
                LiteralLog.Visible = false;
            }
            CADateTimeFrom.SelectedDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("MM/dd/yyy"));
            CADateTimeTo.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyy"));
        }
        void CheckAccount()
        {
            // wf_HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_HO" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }

        void Bind(string from, string to)
        {
            DataTable dt = CreateItemReportDT(from,to);
            ViewState["ItemReportDT"] = dt;
            BindSpGridView(dt);
        }

        /// <summary>
        /// 创建报表数据源
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        DataTable CreateItemReportDT(string from,string to)
        {
            DataTable dt = CreateItemCodeDatatable();/// GetItemCodeDatatable();///就一个列标题（空datatable）从Cost Center中得到ItemCode 只是一行数据

            DataTable dtAllItemCode = GetPaperItemCode();//就两列:所有的纸袋的ItemCode，和单价数据,从Cost Center中得到ItemCode

            DataTable dtPagerBagData = GetPaperBagData(from, to);//costCenter下的所有ItemCode纸袋数据
            if (null == dtPagerBagData)
            {
                return null;
            }

            DataTable dtCostCenter = GetDistingctCostCenter(dtPagerBagData);///全部的CostCenter，就一列：CostCenter

            double dTotalPrice = 0;
            BindCostCenter(dtCostCenter);

            foreach (DataRow dr in dtCostCenter.Rows)//Cost Center形成行数据
            {
                string sSelect = string.Format("CostCenter='{0}'", dr[0].ToString());
                DataRow[] drArray = dtPagerBagData.Select(sSelect);//当前CostCenter下的所申请的ItemCode数据。

                DataRow drReport = dt.NewRow();
                drReport["CostCenter"] = dr[0].ToString();
                drReport["Store"] = drArray[0]["CostCenterName"].ToString();

                double dPrice = 0;
                foreach (DataRow drItem in dtAllItemCode.Rows)//遍历列标题中所有的纸袋ItemCode数据
                {
                    double dEachPrice = 0;
                    string sItemCode = drItem["ItemCode"].ToString();
                    double sQuantity = GetItemTotalQuantity(drArray, sItemCode, out dEachPrice); //drItem["UnitPrice"].ToString(), out dEachPrice);
                    drReport[sItemCode] = sQuantity;
                    dPrice += dEachPrice;
                }
                drReport["Cost"] = dPrice.ToString();
                dt.Rows.Add(drReport);
                dTotalPrice += dPrice;// 总价。
            }
            DataTable dtEveryItemPrice = GetEveryItemTotalPrice(dtPagerBagData);
            dt = CreateBottomSummaryDT(dt, dTotalPrice, dtEveryItemPrice);
            //sw.Flush();
            //sw.Dispose();
            return dt;
        }
        
        /// <summary>
        /// 得到报表数据源的统计信息
        /// </summary>
        /// <param name="dtTitle">标题列</param>
        /// <param name="dTotalCost"></param>
        /// <param name="dtEveryItemPrice"></param>
        /// <returns></returns>
        DataTable CreateBottomSummaryDT(DataTable dtTitle, double dTotalCost, DataTable dtEveryItemPrice)
        {
            if (null == dtTitle)
            {
                return null;
            }
            //dtPagerBagData
            DataRow drQtySum = dtTitle.NewRow();
            DataRow drUnitPrice = dtTitle.NewRow();
            DataRow drCostSum = dtTitle.NewRow();
            DataTable dtItemCode = GetPaperItemCode();

            foreach (DataRow drColumnName in dtItemCode.Rows)// dt.Columns)
            {
                string sColumnName = drColumnName["ItemCode"].ToString();
                foreach (DataRow dr in dtTitle.Rows)
                {
                    int iReturnColumnValue = 0;
                    int.TryParse(drQtySum[sColumnName].ToString(), out iReturnColumnValue);

                    int iCurrentRowValue = 0;
                    int.TryParse(dr[sColumnName].ToString(), out iCurrentRowValue);
                    drQtySum[sColumnName] = (iReturnColumnValue + iCurrentRowValue).ToString();//统计每个Item的个数 
                }
               string sUnitPrice = drColumnName["UnitPrice"].ToString();
               if (null != dtEveryItemPrice)
               {
                   DataRow[] drArray = dtEveryItemPrice.Select("ItemCode='" + sColumnName + "'");
                   if (null != drArray && drArray.Count() > 0)
                   {
                       drCostSum[sColumnName] = drArray[0]["TotalPrice"];
                   }
               }
               drUnitPrice[sColumnName] = sUnitPrice;//显示每个Item的单价UnitPrice
            }

            drQtySum[0] = "Qty.Sum";
            drUnitPrice[0] = "Unit Price(RMB)";
            drCostSum[0] = "Cost Sum(RMB)";

            drQtySum[1] = "";
            drUnitPrice[1] = "";
            drCostSum[1] = "";

            drCostSum["Cost"] = dTotalCost;
            drQtySum["Cost"] = "";

            dtTitle.Rows.Add(drQtySum);
            dtTitle.Rows.Add(drUnitPrice);
            dtTitle.Rows.Add(drCostSum);
            return dtTitle;
        }


        /// <summary>
        /// 创建SPGridview列并绑定。
        /// </summary>
        /// <param name="dt"></param>
        void BindSpGridView(DataTable dt)
        {
            SPGridViewPaperbage.Columns.Clear();
            if (null != dt)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    BoundField bf = new BoundField();
                    bf.DataField = dc.ColumnName;
                    bf.HeaderText = dc.ColumnName;
                    SPGridViewPaperbage.Columns.Add(bf);
                }
            }
            SPGridViewPaperbage.DataSource = dt;
            SPGridViewPaperbage.DataBind();
        }

        /// <summary>
        /// 得到Item的TotalQuantity(数量非箱数)和总价
        /// </summary>
        /// <param name="drArray"></param>
        /// <param name="sItemCode">产品编码</param>
        /// <param name="dTotalPrice">输出总价</param>
        /// <returns></returns>
        double GetItemTotalQuantity(DataRow[] drArray, string sItemCode,out double dTotalPrice)
        {
            double  dCount =0;
            dTotalPrice = 0;
            foreach (DataRow dr in drArray)
            {
                if (dr["ItemCode"].ToString() == sItemCode)
                {
                    double.TryParse(dr["TotalQuantity"].ToString(),out dCount);
                    double.TryParse(dr["TotalPrice"].ToString(), out dTotalPrice);

                    break;
                }
            }
            return dCount;
        }

        /// <summary>
        /// 只得到 CostCenter
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable GetDistingctCostCenter(DataTable dt)
        {
            if (null == dt)
            {
                return null;
            }
            DataTable dtReurn = new DataTable();
            dtReurn.Columns.Add("CostCenter");
            var calculate = (
                                from dr in dt.AsEnumerable()
                                select dr["CostCenter"]
                             ).Distinct();

            foreach (var item in calculate)
            {
                DataRow dr = dtReurn.NewRow();
                dr["CostCenter"] = item;
                dtReurn.Rows.Add(dr);
            }
            return dtReurn;
        }


        /// <summary>
        /// 得到纸袋所有记录
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        DataTable GetPaperBagData(string from, string to)
        {
            DataTable dtLinq = GetQueryListData(from, to);
            if (null == dtLinq||dtLinq.Rows.Count<=0)
            {
                return null;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("CostCenterName");
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("TotalQuantity");
            dt.Columns.Add("TotalPrice");
          
            var ItemCalculate = from dr in dtLinq.AsEnumerable()
                                orderby dr["CostCenter"] ascending
                                group dr by new { CostCenter = dr["CostCenter"], CostCenterName = dr["CostCenterName"], ItemCode = dr["ItemCode"], PackagedRegulation = dr["PackagedRegulation"]} into g
                                select new
                                {
                                    CostCenter = g.Key.CostCenter,
                                    CostCenterName = g.Key.CostCenterName,
                                    ItemCode = g.Key.ItemCode,
                                    TotalQuantity = g.Sum(row => Convert.ToDouble(row["TotalQuantity"])),
                                    PackagedRegulation = g.Key.PackagedRegulation,
                                    TotalPrice = g.Sum(row => Convert.ToDouble(row["TotalPrice"]))
                                };
            foreach (var item in ItemCalculate)
            {
                string sItemCode = item.ItemCode.ToString();
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                PurchaseRequest.PaperBagForVendor pbv = new PaperBagForVendor();
                string sTotalQuantity= pbv.GetCount(item.PackagedRegulation.ToString(), item.TotalQuantity.ToString()).ToString();

                DataRow dr = dt.NewRow();
                dr["CostCenter"] = item.CostCenter;
                dr["CostCenterName"] = item.CostCenterName;
                dr["ItemCode"] = sItemCode;
                dr["TotalQuantity"] = sTotalQuantity.ToString();
                dr["TotalPrice"] = item.TotalPrice;

                dt.Rows.Add(dr);
            }
            return dt;
        }

        DataTable GetQueryListData(string from, string to)
        {
            string sQureyformat = QueryCamle(from, to);
            if (string.IsNullOrEmpty(sQureyformat))
            {
                return null;
            }
            SPListItemCollection splic = null;
            
            SPQuery spQuery = new SPQuery();
            spQuery.Query = sQureyformat;
            splic = SPContext.Current.Web.Lists["PurchaseRequestItems"].GetItems(spQuery);
            if (null == splic || splic.Count == 0)
            {
                return null;
            }
            DataTable dtLinq = splic.GetDataTable();
            return dtLinq;
        }

        /// <summary>
        /// 创建包含所有ItemCode的报表datatable（列）
        /// </summary>
        /// <returns></returns>
        DataTable CreateItemCodeDatatable()
        {
            DataTable dt = new DataTable();
            dt = GetPaperItemCode();
           
            DataTable dtColumn = new DataTable();
            dtColumn.Columns.Add("CostCenter");
            dtColumn.Columns.Add("Store");
            foreach (DataRow dr in dt.Rows)
            {
                dtColumn.Columns.Add(dr[0].ToString());
            }
            dtColumn.Columns.Add("Cost");

            return dtColumn;
        }


        /// <summary>
        /// 从list中得到ItemCode
        /// </summary>
        /// <returns></returns>
        DataTable GetPaperItemCode()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("UnitPrice");
            CamlExpression ce = null;
            QueryField qfItemScope = new QueryField("ItemScope");
            ce = WorkFlowUtil.LinkAnd(ce, qfItemScope.Equal("PB"));

            SPListItemCollection splic = ListQuery.Select().From(SPContext.Current.Web.Lists["Item Codes"]).Where(ce).GetItems();
            foreach (SPListItem item in splic)
            {
                string sItemCode = item["ItemCode"].ToString();
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))//排除以X开头的。
                {
                    continue;
                }

                DataRow dr = dt.NewRow();
                dr["ItemCode"] = sItemCode;
                dr["UnitPrice"] = item["UnitPrice"];

                DataRow[] drArray = dt.Select("ItemCode='" + sItemCode + "'");//去重复的项
                if (drArray.Count() == 0)
                {
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            Bind(from, to);
        }

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            DataTable dt =ViewState["ItemReportDT"] as DataTable;
            if (null == dt)
            {
                return;
            }
            CreateExcel(dt);
        }

        void CreateExcel(DataTable dt)
        {
            string strSampleFileName = "PaperBagReportForItemCodeSample.xls";
            string sSaveFileName = "PaperBagReportForItemCode.xls";

            string sFullPath = Server.MapPath("/tmpfiles/PaperBag/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            DataColumnCollection dcc = dt.Columns;

            SetExcelTitle(dcc, worksheet1);
            int iCount = dt.Rows.Count;
            worksheet1.Rows[2].InsertCopy((iCount-3), worksheet1.Rows[2]);
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
            
            GemBox.Spreadsheet.CellRange range = worksheet.Cells.GetSubrangeAbsolute(0, 0, 0, iColumnCount-1);//合并单元格
            range.Merged = true;
            worksheet.Rows[0].Cells[0].SetBorders(borderLeft, color, LineSyleThick);
            worksheet.Rows[0].Cells[0].SetBorders(borderRight, color, LineSyleThick);
            worksheet.Rows[0].Cells[0].SetBorders(borderTop, color, LineSyleThick);
            worksheet.Rows[0].Cells[0].SetBorders(borderBottom, color, LineSyleThick);

            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            worksheet.Rows[0].Cells[0].Value = string.Format("Paper Bag Report-Items Cost ({0} - {1})", from, to);
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
            int iColumnCount = dt.Columns.Count ;
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
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/PaperBag/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }

        /// <summary>
        /// 得到每一个Item的总价 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable GetEveryItemTotalPrice(DataTable dt)
        {
            DataTable dtTotalPrice = new DataTable();
            dtTotalPrice.Columns.Add("ItemCode");
            dtTotalPrice.Columns.Add("TotalPrice");
            if (null == dt || dt.Rows.Count <= 0)
            {
                return null;
            }

            var priceLinq = from dr in dt.AsEnumerable()
                            group dr by new { ItemCode = dr["ItemCode"] } into g
                            select new
                            {
                                ItemCode = g.Key.ItemCode,
                                TotalPrice = g.Sum(row => Convert.ToDouble(row["TotalPrice"]))
                            };
            foreach (var item in priceLinq)
            {
                DataRow dr = dtTotalPrice.NewRow();
                dr["ItemCode"] = item.ItemCode;
                dr["TotalPrice"] = item.TotalPrice;
                dtTotalPrice.Rows.Add(dr);
            }
            return dtTotalPrice;
        }

        void BindCostCenter(DataTable dt) 
        {
            if (DDLCostCenter.Items.Count == 0)
            {
                DDLCostCenter.DataSource = dt;
                DDLCostCenter.DataTextField = "CostCenter";
                DDLCostCenter.DataValueField = "CostCenter";
                DDLCostCenter.DataBind();
                DDLCostCenter.Items.Insert(0,new ListItem("--Please select CostCenter--",""));
            }
        }

       
       /// <summary>
        /// 得到在所选日期内和是否生成了PO的PR号
       /// </summary>
       /// <param name="from"></param>
       /// <param name="to"></param>
       /// <returns></returns>
        List<string> GetPRNO(string from, string to)
        {
            List<string> listPRNO = new List<string>();

            string scondition = string.Empty;
            string sBase = DDLBaseOn.SelectedValue;

            #region  条件
                if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                {
                    #region
                    if (sBase == "1")
                    {
                        scondition = string.Format(@"<Where>
                                                        <And>
                                                            <Geq>
                                                                <FieldRef Name='Created' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Geq>
                                                            <IsNotNull>
                                                                <FieldRef Name='PONumber' />
                                                            </IsNotNull>
                                                        </And>
                                                     </Where>", from);
                    }
                    else
                    {
                        scondition = string.Format(@"<Where>
                                                            <Geq>
                                                                <FieldRef Name='Created' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Geq>
                                                     </Where>", from);
                    }
                    #endregion
                }
                else if (!string.IsNullOrEmpty(to) && string.IsNullOrEmpty(from))
                {
                    #region
                    if (sBase == "1")
                    {
                        scondition = string.Format(@"<Where>
                                                        <And>
                                                            <Leq>
                                                                <FieldRef Name='Created' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Leq>
                                                            <IsNotNull>
                                                                <FieldRef Name='PONumber' />
                                                            </IsNotNull>
                                                        </And>
                                                     </Where>", to);
                    }
                    else
                    {
                        scondition = string.Format(@"<Where>
                                                        <Leq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                        </Leq>
                                                     </Where>", to);
                    }
                    #endregion

                }
                else if (!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(from))
                {
                    #region
                    if (sBase == "1")
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
                                                            <IsNotNull>
                                                                <FieldRef Name='PONumber' />
                                                            </IsNotNull>
                                                        </And>
                                                 </Where>", from, to);

                    }
                    else
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
                                                     </Where>", from, to);
                    }
                    #endregion
                }
                else
                {
                    #region
                    if (sBase == "1")
                    {
                        scondition = string.Format(@"<Where>
                                                        <IsNotNull>
                                                            <FieldRef Name='PONumber' />
                                                        </IsNotNull>
                                                     </Where>", from, to);
                    }
                    else
                    {
                         listPRNO.Add("");// return null;
                         return listPRNO;
                    }
                    #endregion
                }
                #endregion

            SPQuery query = new SPQuery();
            query.Query = scondition;
            SPListItemCollection splic = SPContext.Current.Web.Lists["Purchase Request Workflow"].GetItems(query);
            if (null == splic || splic.Count == 0)
            {
                listPRNO = null;
            }
            else
            {
                foreach (SPListItem item in splic)
                {
                    if (null == item["Title"])
                    {
                        continue;
                    }

                    if (!listPRNO.Contains(item["Title"].ToString()))
                    {
                        listPRNO.Add(item["Title"].ToString());
                    }
                }
            }

            return listPRNO;
        }


        string GetOrCamle(List<string>  listPR)
        {
            string sCamle = string.Empty;
            if (listPR.Count == 1)
            {
                sCamle=string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", listPR[0]);
                return sCamle.ToString();
            }
            for (int i = 0; i < listPR.Count; i++)
            {
                 string sOrCondition= string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", listPR[i]);
                 if (i == 1 || sCamle.ToString().IndexOf("<Or>") == 0)
                 {
                     sCamle += sOrCondition;
                     sCamle = string.Format("<Or>{0}</Or>", sCamle);
                 }
                 else
                 {
                     sCamle+=(sOrCondition);
                 }
            }
            return sCamle.ToString();
        }

        string QueryCamle(string from, string to)
        {
            string sCostCenter = DDLCostCenter.SelectedValue;

            StringBuilder sb = new StringBuilder();

            if (sCostCenter.Length > 0)
            {
                sb.AppendFormat(@"<And><And><Or><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>S</Value></BeginsWith><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>H10S</Value></BeginsWith></Or><IsNotNull><FieldRef Name='PackagedRegulation' /></IsNotNull></And><Eq><FieldRef Name='CostCenter' /><Value Type='Text'>{0}</Value></Eq></And>", sCostCenter);
            }
            else
            {
                sb.AppendFormat(@"<And><Or><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>S</Value></BeginsWith><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>H10S</Value></BeginsWith></Or><IsNotNull><FieldRef Name='PackagedRegulation' /></IsNotNull></And>");
            }

            List<string> listPRNO = GetPRNO(from, to);
            if (null == listPRNO)
            {
                return null;
            }
            string sQureyformat = string.Empty;
            if (listPRNO.Count > 1 && listPRNO[0]!="")
            {
                string sOrCondition = GetOrCamle(listPRNO);
                sb.Append(sOrCondition);
                sQureyformat = string.Format(@"<Where><And>{0}</And></Where>", sb.ToString());
            }
            else
            {
                if (listPRNO[0] != "")
                {
                    string sOrCondition = GetOrCamle(listPRNO);
                    sb.Append(sOrCondition);
                    sQureyformat = string.Format(@"<Where><And>{0}</And></Where>", sb.ToString());
                }
                else
                {
                    sQureyformat = string.Format(@"<Where>{0}</Where>", sb.ToString());
                }
                
            }
         
            return sQureyformat;
        }

        protected void ButtonViewLog_Click(object sender, EventArgs e)
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            DataTable dtLinq = GetQueryListData(from, to);
            if (null == dtLinq || dtLinq.Rows.Count <= 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            var ItemCalculate = from dr in dtLinq.AsEnumerable()
                                orderby dr["CostCenter"] ascending
                                group dr by new { CostCenter = dr["CostCenter"], CostCenterName = dr["CostCenterName"], ItemCode = dr["ItemCode"], PackagedRegulation = dr["PackagedRegulation"], Title = dr["Title"], PONumber = dr["PONumber"] } into g
                                select new
                                {
                                    CostCenter = g.Key.CostCenter,
                                    CostCenterName = g.Key.CostCenterName,
                                    ItemCode = g.Key.ItemCode,
                                    TotalQuantity = g.Sum(row => Convert.ToDouble(row["TotalQuantity"])),
                                    PackagedRegulation = g.Key.PackagedRegulation,
                                    TotalPrice = g.Sum(row => Convert.ToDouble(row["TotalPrice"])),
                                    PRNO = g.Key.Title,
                                    PONO = g.Key.PONumber
                                };
            foreach (var item in ItemCalculate)
            {
                string sItemCode = item.ItemCode.ToString();
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                PurchaseRequest.PaperBagForVendor pbv = new PaperBagForVendor();
                string sTotalQuantity = pbv.GetCount(item.PackagedRegulation.ToString(), item.TotalQuantity.ToString()).ToString();
                sb.AppendFormat("PRNO: {0}   ", item.PRNO);
                sb.AppendFormat("PONO: {0}    ", item.PONO);
                sb.AppendFormat("TotalPrice: {0}    ", item.TotalPrice);
                sb.AppendFormat("ItemCode: {0}    ", item.ItemCode);
                sb.AppendFormat("TotalQuantity: {0}    ", item.TotalQuantity);
                sb.AppendFormat("CostCenter: {0}   ", item.CostCenter);
                sb.Append("<br /><br />");
            }
            LiteralLog.Text = sb.ToString();

        }
    }
}