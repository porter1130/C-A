namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Microsoft.SharePoint;
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using System.Text.RegularExpressions;
    using System.Text;

    public partial class PaperBagForVendor : Microsoft.SharePoint.WebControls.LayoutsPageBase
    {
        ObjectDataSource ods = new ObjectDataSource();

        protected override void OnLoad(EventArgs e)
        {
            this.ods.ID = "PaperBagDSID";
            this.ods.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.ods.SelectMethod = "GetPaperBag";
            this.Controls.Add(this.ods);

            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
            if (IsPostBack)
            {
                return;
            }
            Inite();
            BindCostCenter();

            string from = this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            Bind(from, to);
        }
        void Inite()
        {
            if (null != Request.QueryString["Log"])
            {
                ButtonLog.Visible = true;
                LiteralLog.Visible = true;
            }
            else
            {
                ButtonLog.Visible = false;
                LiteralLog.Visible = false;
            }
            CADateTimeFrom.SelectedDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("MM/dd/yyy"));
            CADateTimeTo.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyy"));
        }
        void CheckAccount()
        {
            // wf_Store可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_HO" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }
        void BindCostCenter()
        {
            DataTable dt = new DataTable();

            CamlExpression ceIsActive = null;
            CamlExpression ceTitle = null;
            CamlExpression ce = null;

            QueryField qfIsActive = new QueryField("IsActive");
            ceIsActive = WorkFlowUtil.LinkAnd(ceIsActive, qfIsActive.Equal(true));

            QueryField qfTitle = new QueryField("Title");
            ceTitle = WorkFlowUtil.LinkOr(ceTitle, qfTitle.BeginsWith("S"));
            ceTitle = WorkFlowUtil.LinkOr(ceTitle, qfTitle.BeginsWith("H10S"));

            ce = WorkFlowUtil.LinkAnd(ceIsActive, ceTitle);


            dt = ListQuery.Select().From(SPContext.Current.Web.Lists["Cost Centers"]).Where(ce).GetItems().GetDataTable();
             
            DDLCostCenter.DataSource = dt;
            DDLCostCenter.DataTextField = "Display";
            DDLCostCenter.DataValueField = "ID";
            DDLCostCenter.DataBind();
            DDLCostCenter.Items.Insert(0,new ListItem("--Please select CostCenter--",""));
        }

        void Bind(string from, string to)
        {
            string sCosterCenter=DDLCostCenter.SelectedValue;
            string sBase = DDLBaseOn.SelectedValue;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("sFrom", from);
            ods.SelectParameters.Add("sTo", to);
            ods.SelectParameters.Add("sVendorID", sCosterCenter);
            ods.SelectParameters.Add("sBase", sBase);

            SPGridViewPaperPabage.DataSourceID = "PaperBagDSID";//读PurchaseRequestItems
            SPGridViewPaperPabage.DataBind();

            DataTable dtPaperBag = GetPaperBag(from, to, sCosterCenter, sBase);
            DataTable itemCodeDt = GetItemCalculate(dtPaperBag);
            SPGridViewItemCode.DataSource = itemCodeDt;
            SPGridViewItemCode.DataBind();
        }

        public DataTable GetPaperBag(string sFrom,string sTo,string sVendorID,string sBase)
        {
            string sQueryCamle=QueryCamle(sVendorID, sFrom, sTo, sBase);// scondition;
            if (string.IsNullOrEmpty(sQueryCamle))
            {
                return null;
            }
            SPQuery query = new SPQuery();
            query.Query = sQueryCamle;
            DataTable dt = new DataTable();
            SPListItemCollection splic = SPContext.Current.Web.Lists["PurchaseRequestItems"].GetItems(query);
            dt = splic.GetDataTable();
        
            return GetCalculateDatable(dt);
        }

        

        DataTable GetCalculateDatable(DataTable dt)
        {
            if (null == dt)
            {
                return null;
            }
            DataTable dtResult = new DataTable();
            var DataCalculate = from dr in dt.AsEnumerable()
                                orderby dr["CostCenter"] ascending
                                group dr by new { ItemCode = dr["ItemCode"], Description = dr["Description"], Unit = dr["Unit"], CostCenterName = dr["CostCenterName"], CostCenter = dr["CostCenter"], PackagedRegulation = dr["PackagedRegulation"] } into g
                                  select new
                                  {
                                      ItemCode=g.Key.ItemCode,
                                      Description=g.Key.Description,
                                      Carton = g.Sum(row => Convert.ToDouble(row["TotalQuantity"])),
                                      Unit=g.Key.Unit,
                                      CostCenterName=g.Key.CostCenterName,
                                      CostCenter = g.Key.CostCenter,
                                      PackagedRegulation = g.Key.PackagedRegulation
                                  };
            dtResult.Columns.Add("ItemCode");
            dtResult.Columns.Add("Description");
            dtResult.Columns.Add("TotalQuantity");//总数
            dtResult.Columns.Add("PackagedUnite");//包装单位
            dtResult.Columns.Add("PackagedRegulation");//包装规则
            dtResult.Columns.Add("Carton");//箱数
            dtResult.Columns.Add("Unit");
            dtResult.Columns.Add("CostCenterName");
            dtResult.Columns.Add("CostCenter");

            foreach (var item in DataCalculate)
            {
                string sItemCode = item.ItemCode.ToString();
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                string sPackagedRegulation = item.PackagedRegulation.ToString();
                string sCarton=item.Carton.ToString();

                DataRow dr = dtResult.NewRow();
                dr["ItemCode"]=item.ItemCode;
                dr["Description"]=item.Description;
                dr["TotalQuantity"] = GetCount(sPackagedRegulation, sCarton);//总数
                dr["PackagedUnite"] = GetUnitFromPackagedRegulation(sPackagedRegulation);
                dr["PackagedRegulation"] = item.PackagedRegulation;
                dr["Carton"] = sCarton;//箱数
                dr["Unit"] = item.Unit;
                dr["CostCenterName"] = item.CostCenterName;
                dr["CostCenter"] = item.CostCenter;
                dtResult.Rows.Add(dr);
            }
            return dtResult;
        }

       /// <summary>
       /// 得到产品总数量
       /// </summary>
       /// <param name="sPackagedRegulation"></param>
       /// <param name="sQuantity"></param>
       /// <returns></returns>
       public double GetCount(string sPackagedRegulation,string sQuantity)
        {
            int iNumber = 0;
            double iQuantity = 0;
            Regex reg = new Regex(@"^\d+");///^([1-9][0-9]*)(\.?)(\d*)$/
            string sResult = reg.Match(sPackagedRegulation).Groups[0].ToString();
            if (sResult != "")
            {
                iNumber = int.Parse(sResult);
            }

            double.TryParse(sQuantity, out iQuantity);

            return iNumber * iQuantity;
        }




        protected void SPGridViewPaperPabage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SPGridViewPaperPabage.PageIndex = e.NewPageIndex;
            SPGridViewPaperPabage.DataBind();
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            Bind(from, to);
        }

        protected void ButtonExportExcel_Click(object sender, EventArgs e)
        {
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            string sBase = DDLBaseOn.SelectedValue;
            DataTable dt = GetPaperBag(from, to, DDLCostCenter.SelectedValue, sBase);
            if (null == dt)
            {
                return;
            }
            DataTable ItemcodeDt = GetItemCalculate(dt);
            CreateExcel(dt, ItemcodeDt);
            

        }

        /// <summary>
        /// 得到itemcode的统计数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable GetItemCalculate(DataTable dt)
        {
            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("ItemCode");
            dtResult.Columns.Add("Description");
            dtResult.Columns.Add("TotalQuantity");//总数量
            dtResult.Columns.Add("PackagedUnite");//包装单位
            dtResult.Columns.Add("PackagedRegulation");//包装规则 
            dtResult.Columns.Add("Carton");//箱数
            dtResult.Columns.Add("Unit");

            if (null == dt)
            {
                return null;
            }

            var DataCalculate = from dr in dt.AsEnumerable()
                                group dr by new { ItemCode = dr["ItemCode"], Description = dr["Description"], Unit = dr["Unit"], PackagedRegulation = dr["PackagedRegulation"], PackagedUnite = dr["PackagedUnite"] } into g
                                select new
                                {
                                    ItemCode = g.Key.ItemCode,
                                    Description=g.Key.Description,
                                    Carton = g.Sum(row => Convert.ToDecimal(row["Carton"])),
                                    Unit = g.Key.Unit,
                                    PackagedRegulation = g.Key.PackagedRegulation,
                                    TotalQuantity = g.Sum(row => Convert.ToDecimal(row["TotalQuantity"])),
                                    PackagedUnite = g.Key.PackagedUnite
                                };

            foreach (var item in DataCalculate)
            {
                string sPackagedRegulation = item.PackagedRegulation.ToString();
                string sCarton = item.Carton.ToString();
                
                DataRow dr = dtResult.NewRow();
                dr["ItemCode"] = item.ItemCode;
                dr["Description"] = item.Description;
                dr["TotalQuantity"] = item.TotalQuantity;// GetCount(sPackagedRegulation, sCarton);
                dr["PackagedUnite"] = item.PackagedUnite;// GetUnitFromPackagedRegulation(sPackagedRegulation);
                dr["PackagedRegulation"] = sPackagedRegulation;
                dr["Carton"] = sCarton;
                dr["Unit"] = item.Unit;

                dtResult.Rows.Add(dr);
            }

            return dtResult;
        }

        /// <summary>
        /// 得到包装规则里的单位 
        /// </summary>
        /// <param name="sPackagedRegulation"></param>
        /// <returns></returns>
        string GetUnitFromPackagedRegulation(string sPackagedRegulation)
        {
            Regex reg = new Regex(@"^\d+");
            return reg.Replace(sPackagedRegulation, "").ToCharArray()[0].ToString();
        }


        void CreateExcel(DataTable dt,DataTable itemcodeDt)
        {
            string strSampleFileName = "PaperBagReportSample.xls";
            string sSaveFileName = "PaperBagReport.xls";

            string sFullPath = Server.MapPath("/tmpfiles/PaperBag/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];
            int iCount = dt.Rows.Count;
            int iInsertCount = iCount - 2;
            //PaperBagReport

            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            worksheet1.Rows[0].Cells[0].Value = string.Format("Paper Bag Report-Items Summary ({0} - {1})", from, to);
            worksheet1.Rows[2].InsertCopy(iInsertCount, worksheet1.Rows[2]);
            for (int i = 0; i < iCount; i++)
            {
                worksheet1.Rows[i + 2].Cells[0].Value = dt.Rows[i]["ItemCode"];
                worksheet1.Rows[i + 2].Cells[1].Value = dt.Rows[i]["Description"];
                worksheet1.Rows[i + 2].Cells[2].Value = dt.Rows[i]["TotalQuantity"];
                worksheet1.Rows[i + 2].Cells[3].Value = dt.Rows[i]["PackagedUnite"];
                worksheet1.Rows[i + 2].Cells[4].Value = dt.Rows[i]["PackagedRegulation"];
                worksheet1.Rows[i + 2].Cells[5].Value = dt.Rows[i]["Carton"];
                worksheet1.Rows[i + 2].Cells[6].Value = dt.Rows[i]["Unit"];
                worksheet1.Rows[i + 2].Cells[7].Value = dt.Rows[i]["CostCenterName"];
                worksheet1.Rows[i + 2].Cells[8].Value = dt.Rows[i]["CostCenter"];
            }

            #region 纸袋统计数据
            int iItemCount = itemcodeDt.Rows.Count;
            int iItemCodeIndex=8 + iInsertCount;

            worksheet1.Rows[iItemCodeIndex].InsertCopy(iItemCount - 2, worksheet1.Rows[iItemCodeIndex]);

            for (int i = 0; i < iItemCount; i++)
			{
			 worksheet1.Rows[i+iItemCodeIndex].Cells[0].Value=itemcodeDt.Rows[i]["ItemCode"];
             worksheet1.Rows[i + iItemCodeIndex].Cells[1].Value = itemcodeDt.Rows[i]["Description"];
             worksheet1.Rows[i + iItemCodeIndex].Cells[2].Value = itemcodeDt.Rows[i]["TotalQuantity"];
             worksheet1.Rows[i + iItemCodeIndex].Cells[3].Value = itemcodeDt.Rows[i]["PackagedUnite"];
             worksheet1.Rows[i + iItemCodeIndex].Cells[4].Value = itemcodeDt.Rows[i]["PackagedRegulation"];
             worksheet1.Rows[i + iItemCodeIndex].Cells[5].Value = itemcodeDt.Rows[i]["Carton"];
             worksheet1.Rows[i + iItemCodeIndex].Cells[6].Value = itemcodeDt.Rows[i]["Unit"];
            }
            #endregion

            string sSavePath = string.Concat(sFullPath, sSaveFileName);
            objExcelFile.SaveXls(sSavePath);
            SendExcelToClient(sSavePath, sSaveFileName);
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

        string QueryCamle(string sCostCenterID, string sFrom, string sTo, string sBase)
        {

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty( sCostCenterID ))
            {
                sb.AppendFormat(@"<And><And><Or><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>S</Value></BeginsWith><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>H10S</Value></BeginsWith></Or><IsNotNull><FieldRef Name='PackagedRegulation' /></IsNotNull></And><Eq><FieldRef Name='CostCenterID' /><Value Type='Text'>{0}</Value></Eq></And>", sCostCenterID);
            }
            else
            {
                sb.AppendFormat(@"<And><Or><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>S</Value></BeginsWith><BeginsWith><FieldRef Name='CostCenter' /><Value Type='Text'>H10S</Value></BeginsWith></Or><IsNotNull><FieldRef Name='PackagedRegulation' /></IsNotNull></And>");
            }


            List<string> listPRNO = GetPRNO(  sFrom,  sTo,  sBase);
            if (null == listPRNO)
            {
                return null;
            }
            string sQureyformat = string.Empty;
            if ( listPRNO.Count > 1 && listPRNO[0]!="")
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
        /// <summary>
        /// 得到在所选日期内和是否生成了PO的PR号
        /// </summary>
        /// <returns></returns>
        List<string> GetPRNO(string sFrom, string sTo, string sBase)
        {
            List<string> listPRNO = new List<string>();
            string scondition = string.Empty;

            #region  条件
            if (!string.IsNullOrEmpty(sFrom) && string.IsNullOrEmpty(sTo))
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
                                                     </Where>", sFrom);
                }
                else
                {
                    scondition = string.Format(@"<Where>
                                                            <Geq>
                                                                <FieldRef Name='Created' />
                                                                <Value Type='DateTime'>{0}</Value>
                                                            </Geq>
                                                     </Where>", sFrom);
                }
                #endregion
            }
            else if (!string.IsNullOrEmpty(sTo) && string.IsNullOrEmpty(sFrom))
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
                                                     </Where>", sTo);
                }
                else
                {
                    scondition = string.Format(@"<Where>
                                                        <Leq>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                        </Leq>
                                                     </Where>", sTo);
                }
                #endregion

            }
            else if (!string.IsNullOrEmpty(sTo) && !string.IsNullOrEmpty(sFrom))
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
                                                 </Where>", sFrom, sTo);

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
                                                     </Where>", sFrom, sTo);
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
                                                     </Where>", sFrom, sTo);
                }
                else
                {
                    listPRNO.Add("");
                    return listPRNO;
                    //scondition = string.Empty;
                }
                #endregion
            }
            #endregion

            SPQuery query = new SPQuery();
            SPListItemCollection splic = null;
           
            query.Query = scondition;
            splic = SPContext.Current.Web.Lists["Purchase Request Workflow"].GetItems(query);
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

        string GetOrCamle(List<string> listPR)
        {
            string sCamle = string.Empty;
            if (listPR.Count == 1)
            {
                sCamle = string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", listPR[0]);
                return sCamle.ToString();
            }
            for (int i = 0; i < listPR.Count; i++)
            {
                string sPRNO= listPR[i];
                if (sPRNO.Trim().Length == 0)
                {
                    continue;
                }
                string sOrCondition = string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", listPR[i]);
                if (i == 1 || sCamle.ToString().IndexOf("<Or>") == 0)
                {
                    sCamle += sOrCondition;
                    sCamle = string.Format("<Or>{0}</Or>", sCamle);
                }
                else
                {
                    sCamle += (sOrCondition);
                }
            }
            return sCamle.ToString();
        }

        protected void ButtonLog_Click(object sender, EventArgs e)
        {
            SPQuery query = new SPQuery();
            string sCosterCenter = DDLCostCenter.SelectedValue;
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");
            string sBase = DDLBaseOn.SelectedValue;
            query.Query = QueryCamle(sCosterCenter,from,to,sBase);// scondition;

            DataTable dt = new DataTable();
            SPListItemCollection splic = SPContext.Current.Web.Lists["PurchaseRequestItems"].GetItems(query);
            dt = splic.GetDataTable();
            if (null == dt)
            {
                return;
            }
            DataTable dtResult = new DataTable();
            var DataCalculate = from dr in dt.AsEnumerable()
                                orderby dr["CostCenter"] ascending
                                group dr by new { ItemCode = dr["ItemCode"], Description = dr["Description"], Unit = dr["Unit"], CostCenterName = dr["CostCenterName"], CostCenter = dr["CostCenter"], PackagedRegulation = dr["PackagedRegulation"], Title = dr["Title"], PONumber = dr["PONumber"] } into g
                                select new
                                {
                                    ItemCode = g.Key.ItemCode,
                                    Description = g.Key.Description,
                                    Carton = g.Sum(row => Convert.ToDouble(row["TotalQuantity"])),
                                    Unit = g.Key.Unit,
                                    CostCenterName = g.Key.CostCenterName,
                                    CostCenter = g.Key.CostCenter,
                                    PackagedRegulation = g.Key.PackagedRegulation,
                                    PRNO = g.Key.Title,
                                    PONO = g.Key.PONumber
                                };
            StringBuilder sb = new StringBuilder();
            foreach (var item in DataCalculate)
            {
                string sItemCode = item.ItemCode.ToString();
                if (sItemCode.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                string sPackagedRegulation = item.PackagedRegulation.ToString();
                string sCarton = item.Carton.ToString();
                sb.AppendFormat("PRNO: {0}   ", item.PRNO);
                sb.AppendFormat("PONO: {0}    ", item.PONO);
                sb.AppendFormat("TotalQuantity: {0}    ", GetCount(sPackagedRegulation, sCarton));//总数
                sb.AppendFormat("CostCenter: {0}   ", item.CostCenter);
                sb.Append("<br /><br />");
            }
            LiteralLog.Text = sb.ToString();
        }
    }
}