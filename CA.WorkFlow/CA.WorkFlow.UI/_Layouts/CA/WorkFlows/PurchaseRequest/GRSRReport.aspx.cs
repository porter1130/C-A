using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using System.Data;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class GRSRReport : Microsoft.SharePoint.WebControls.LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            CheckAccount();
            BindDDL(SPContext.Current.Web.CurrentUser.LoginName);
            BindCalculateData();
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

        void BindCalculateData()
        {
            DataTable dtQuery = new DataTable();
            dtQuery = GetQuery();
            if (null == dtQuery)
            {
                ClearValue();
                return;
            }

            DataTable dtStandhard = new DataTable();
            int iStandardCount = 0;
            dtStandhard = CalculateData(dtQuery, "StandhardAndQuality", out iStandardCount);
            BindStandhardAndQuality(dtStandhard, iStandardCount);

            DataTable dtProductQty = new DataTable();
            int iProductQty = 0;
            dtProductQty = CalculateData(dtQuery, "Quantity", out iProductQty);
            BindProductQty(dtProductQty, iProductQty);

            DataTable dtDeliveryTime = new DataTable();
            int iDeliveryTime = 0;
            dtDeliveryTime = CalculateData(dtQuery, "DeliveryTime", out iDeliveryTime);
            BindDeliveryTime(dtDeliveryTime, iDeliveryTime);


            DataTable dtServiceManner = new DataTable();
            int iServiceManner = 0;
            dtServiceManner = CalculateData(dtQuery, "ServiceManner", out iServiceManner);
            BindServiceManner(dtServiceManner, iServiceManner);


            DataTable dtRespond = new DataTable();
            int iRespond = 0;
            dtRespond = CalculateData(dtQuery, "Response", out iRespond);
            BindRespond(dtRespond, iRespond);
        }

        void BindStandhardAndQuality(DataTable dt,int iTotalCount)
        {
            LabelStandardTotalQty.Text = iTotalCount.ToString();
            foreach (DataRow dr in dt.Rows)
            {
                int iCount=int.Parse(dr["Count"].ToString());
                string sPercent = ((double)iCount / iTotalCount).ToString("P");

                switch (dr["FeedbackName"].ToString())
                {
                    case "Good":LabelStandardOneCount.Text=iCount.ToString();
                        LabelStandardOneRatio.Text = sPercent;
                        break;
                    case "Acceptable": LabelStandardTwoCount.Text = iCount.ToString();
                        LabelStandardTwoRatio.Text = sPercent;
                        break;
                    case "Poor": LabelStandardThreeCount.Text = iCount.ToString();
                        LabelStandardThreeRatio.Text = sPercent;
                        break;
                }
            }
        }

        void BindProductQty(DataTable dt, int iTotalCount)
        {
            LabelProductQtyTotalQty.Text = iTotalCount.ToString();
            foreach (DataRow dr in dt.Rows)
            {
                int iCount = int.Parse(dr["Count"].ToString());
                string sPercent = ((double)iCount / iTotalCount).ToString("P");

                switch (dr["FeedbackName"].ToString())
                {
                    case "MatchOrdering": LabelProductQtyOneCount.Text = iCount.ToString();
                        LabelProductQtyOneRatio.Text = sPercent;
                        break;
                    case "NotMatchOrdering": LabelProductQtyThreeCount.Text = iCount.ToString();
                        LabelProductQtyThreeRatio.Text = sPercent;
                        break;
                }
            }
        }

        void BindDeliveryTime(DataTable dt, int iTotalCount)
        {
            LabelDeliveryTotal.Text = iTotalCount.ToString();
            foreach (DataRow dr in dt.Rows)
            {
                int iCount = int.Parse(dr["Count"].ToString());
                string sPercent = ((double)iCount / iTotalCount).ToString("P");

                switch (dr["FeedbackName"].ToString())
                {
                    case "Early": LabelDeliveryOneCount.Text = iCount.ToString();
                        LabelDeliveryOneRatio.Text = sPercent;
                        break;
                    case "OnTime": LabelDeliveryTwoCount.Text = iCount.ToString();
                        LabelDeliveryTwoRatio.Text = sPercent;
                        break;
                    case "Delay": LabelDeliveryThreeCount.Text = iCount.ToString();
                        LabelDeliveryThreeRatio.Text = sPercent;
                        break;
                }
            }
        }


        void BindServiceManner(DataTable dt, int iTotalCount)
        {
            LabelServiceTotal.Text = iTotalCount.ToString();
            foreach (DataRow dr in dt.Rows)
            {
                int iCount = int.Parse(dr["Count"].ToString());
                string sPercent = ((double)iCount / iTotalCount).ToString("P");

                switch (dr["FeedbackName"].ToString())
                {
                    case "Satisfied": LabelServiceOneCount.Text = iCount.ToString();
                        LabelServiceOneRatio.Text = sPercent;
                        break;
                    case "Acceptable": LabelServiceTwoCount.Text = iCount.ToString();
                        LabelServiceTwoRatio.Text = sPercent;
                        break;
                    case "Unacceptable": LabelServiceThreeCount.Text = iCount.ToString();
                        LabelServiceThreeRatio.Text = sPercent;
                        break;
                }
            }
        }

        void BindRespond(DataTable dt, int iTotalCount)
        {
            LabelRespondCount.Text = iTotalCount.ToString();
            foreach (DataRow dr in dt.Rows)
            {
                int iCount = int.Parse(dr["Count"].ToString());
                string sPercent = ((double)iCount / iTotalCount).ToString("P");

                switch (dr["FeedbackName"].ToString())
                {
                    case "Fast": LabelRespondOneCount.Text = iCount.ToString();
                        LabelRespondOneRatio.Text = sPercent;
                        break;
                    case "Acceptable": LabelRespondTwoCount.Text = iCount.ToString();
                        LabelRespondTwoRatio.Text = sPercent;
                        break;
                    case "Slow": LabelRespondThreeCount.Text = iCount.ToString();
                        LabelRespondThreeRatio.Text = sPercent;
                        break;
                }
            }
        }



        DataTable CalculateData(DataTable dt,string sColumnName,out int iTotalCount)
        {
            iTotalCount = 0;
            var linqDr = from dr in dt.AsEnumerable()
                       group dr by dr.Field<string>(sColumnName) into g
                       select new { 
                        ColumnName= g.Key,
                        count=g.Count()
                       };
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("FeedbackName");
            dtReturn.Columns.Add("Count");

            foreach (var item in linqDr)
            {
                DataRow dr = dtReturn.NewRow();
                dr["FeedbackName"] = item.ColumnName;
                dr["Count"] = item.count;
                iTotalCount += int.Parse(item.count.ToString());
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        DataTable GetQuery()
        {

            DataTable dt = null;/// new DataTable();

            SPQuery query = new SPQuery();

            string sVendorID = DropdownVendor.SelectedValue;
            string from = this.CADateTimeFrom.IsDateEmpty ? string.Empty : this.CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
            string to = this.CADateTimeTo.IsDateEmpty ? string.Empty : this.CADateTimeTo.SelectedDate.ToString("yyyy-MM-dd");

            string condition = string.Empty;
            if (string.IsNullOrEmpty(sVendorID))
            {
                #region  条件
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
                #endregion
            }
            else
            {
                #region  条件
                if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                {
                    condition = string.Format(@"<Where>
                                            <And>
                                                <Geq>
                                                    <FieldRef Name='Created' />
                                                    <Value Type='DateTime'>{0}</Value>
                                                </Geq>
                                                <Eq>
                                                    <FieldRef Name='Title' />
                                                    <Value Type='Text'>{1}</Value>
                                                </Eq>
                                            </And>
                                        </Where>", from, sVendorID);
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
                                                    <FieldRef Name='Title' />
                                                    <Value Type='Text'>{1}</Value>
                                                </Eq>
                                            </And>
                                        </Where>", to, sVendorID);
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
                                                    <FieldRef Name='Title' />
                                                    <Value Type='Text'>{2}</Value>
                                                </Eq>
                                            </And>
                                        </Where>", from, to, sVendorID);
                }
                else
                {
                    condition = string.Format(@"<Where>
                                                <Eq>
                                                    <FieldRef Name='Title' />
                                                    <Value Type='Text'>{0}</Value>
                                                </Eq>
                                        </Where>", sVendorID);
                }
                #endregion
            }

            query.Query = condition;
            SPList list = SPContext.Current.Web.Lists["VendorFeedback"];
            SPListItemCollection splic = list.GetItems(query);
            dt = splic.GetDataTable();
            return dt;
        }

        void BindDDL(string sAccount)
        {
            SPListItemCollection lc = SPContext.Current.Web.Lists["Vendors"].Items;
           
            DataTable dt = new DataTable();
            dt.Columns.Add("VendorID");
            dt.Columns.Add("Title");
            foreach (SPListItem item in lc)
            {
                DataRow dr = dt.NewRow();
                dr["VendorID"] = item["VendorId"];
                dr["Title"] = item["Title"];
                dt.Rows.Add(dr);
            }
            DropdownVendor.DataSource = dt;
            DropdownVendor.DataValueField = "VendorID";
            DropdownVendor.DataTextField = "Title";
            DropdownVendor.DataBind();
            DropdownVendor.Items.Insert(0,new ListItem("--Please Select Vendor--",""));
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            ClearValue();
            BindCalculateData();
        }

        protected void ButtonExportExcel_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }


        void CreateExcel()
        {
            string strSampleFileName = "GRSRReportSample.xls";
            string sSaveFileName = "GRSRReport.xls";

            string sFullPath = Server.MapPath("/tmpfiles/GRSR/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            worksheet1.Rows[2].Cells[1].Value = LabelStandardTotalQty.Text;
            worksheet1.Rows[2].Cells[3].Value = LabelStandardOneCount.Text;
            worksheet1.Rows[2].Cells[4].Value = LabelStandardOneRatio.Text;
            worksheet1.Rows[2].Cells[6].Value = LabelStandardTwoCount.Text;
            worksheet1.Rows[2].Cells[7].Value = LabelStandardTwoRatio.Text;
            worksheet1.Rows[2].Cells[9].Value = LabelStandardThreeCount.Text;
            worksheet1.Rows[2].Cells[10].Value = LabelStandardThreeRatio.Text;

            worksheet1.Rows[3].Cells[1].Value = LabelProductQtyTotalQty.Text;
            worksheet1.Rows[3].Cells[3].Value = LabelProductQtyOneCount.Text;
            worksheet1.Rows[3].Cells[4].Value = LabelProductQtyOneRatio.Text;
            worksheet1.Rows[3].Cells[9].Value = LabelProductQtyThreeCount.Text;
            worksheet1.Rows[3].Cells[10].Value = LabelProductQtyThreeRatio.Text;

            worksheet1.Rows[4].Cells[1].Value = LabelDeliveryTotal.Text;
            worksheet1.Rows[4].Cells[3].Value = LabelDeliveryOneCount.Text;
            worksheet1.Rows[4].Cells[4].Value = LabelDeliveryOneRatio.Text;
            worksheet1.Rows[4].Cells[6].Value = LabelDeliveryTwoCount.Text;
            worksheet1.Rows[4].Cells[7].Value = LabelDeliveryTwoRatio.Text;
            worksheet1.Rows[4].Cells[9].Value = LabelDeliveryThreeCount.Text;
            worksheet1.Rows[4].Cells[10].Value = LabelDeliveryThreeRatio.Text;

            worksheet1.Rows[5].Cells[1].Value = LabelServiceTotal.Text;
            worksheet1.Rows[5].Cells[3].Value = LabelServiceOneCount.Text;
            worksheet1.Rows[5].Cells[4].Value = LabelServiceOneRatio.Text;
            worksheet1.Rows[5].Cells[6].Value = LabelServiceTwoCount.Text;
            worksheet1.Rows[5].Cells[7].Value = LabelServiceTwoRatio.Text;
            worksheet1.Rows[5].Cells[9].Value = LabelServiceThreeCount.Text;
            worksheet1.Rows[5].Cells[10].Value = LabelServiceThreeRatio.Text;

            worksheet1.Rows[6].Cells[1].Value = LabelRespondCount.Text;
            worksheet1.Rows[6].Cells[3].Value = LabelRespondOneCount.Text;
            worksheet1.Rows[6].Cells[4].Value = LabelRespondOneRatio.Text;
            worksheet1.Rows[6].Cells[6].Value = LabelRespondTwoCount.Text;
            worksheet1.Rows[6].Cells[7].Value = LabelRespondTwoRatio.Text;
            worksheet1.Rows[6].Cells[9].Value = LabelRespondThreeCount.Text;
            worksheet1.Rows[6].Cells[10].Value = LabelRespondThreeRatio.Text;

            string sSavePath = string.Concat(sFullPath, sSaveFileName);
            objExcelFile.SaveXls(sSavePath);
            SendExcelToClient(sSavePath, sSaveFileName);
        }


        void ClearValue()
        {
           LabelStandardTotalQty.Text="0";
           LabelStandardOneCount.Text = "0";
           LabelStandardOneRatio.Text = "0%";
           LabelStandardTwoCount.Text = "0";
           LabelStandardTwoRatio.Text = "0%";
           LabelStandardThreeCount.Text = "0";
           LabelStandardThreeRatio.Text = "0%";

           LabelProductQtyTotalQty.Text = "0";
           LabelProductQtyOneCount.Text = "0";
           LabelProductQtyOneRatio.Text = "0%";
           LabelProductQtyThreeCount.Text = "0";
           LabelProductQtyThreeRatio.Text = "0%";

           LabelDeliveryTotal.Text = "0";
           LabelDeliveryOneCount.Text = "0";
           LabelDeliveryOneRatio.Text = "0%";
           LabelDeliveryTwoCount.Text = "0";
           LabelDeliveryTwoRatio.Text = "0%";
           LabelDeliveryThreeCount.Text = "0";
           LabelDeliveryThreeRatio.Text = "0%";

           LabelServiceTotal.Text = "0";
           LabelServiceOneCount.Text = "0";
           LabelServiceOneRatio.Text = "0%";
           LabelServiceTwoCount.Text = "0";
           LabelServiceTwoRatio.Text = "0%";
           LabelServiceThreeCount.Text = "0";
           LabelServiceThreeRatio.Text = "0%";

           LabelRespondCount.Text = "0";
           LabelRespondOneCount.Text = "0";
           LabelRespondOneRatio.Text = "0%";
           LabelRespondTwoCount.Text = "0";
           LabelRespondTwoRatio.Text = "0%";
           LabelRespondThreeCount.Text = "0";
           LabelRespondThreeRatio.Text = "0%";
        }

        /// <summary>
        /// 向户端发送生成的Excel
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="sFileName"></param>
        void SendExcelToClient(string sFileFullName, string sFileName)
        {
            string sApplicationPath = Request.ApplicationPath;
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/GRSR/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }


    }
}