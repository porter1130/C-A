namespace CA.WorkFlow.UI.PurchaseOrder
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using System.Data;

    public partial class DisplayForm : CAWorkFlowPage
    {        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            CheckAccount();

            this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
            this.DataForm1.IsHideFinanceNum = true;
            
        }

        private void CheckAccount()
        {
            //Legal,HO他legal可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (WorkFlowUtil.IsInGroups(current, new string[] { "wf_HO", "wf_Legal", "wf_Finance_PO" }) || PurchaseOrderCommon.isAdmin())
            {
                //empty
            }
            else if (WorkflowContext.Current.DataFields["Approvers"].AsString().Contains(current))
            {
                //empty
            }
            else
            {
                RedirectToTask();
            }
        }




        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }
        void CreateExcel()
        {
            string sRequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();

            //string sPONOSAP = WorkflowContext.Current.DataFields["SapNO"].AsString();
            string sVendor = WorkflowContext.Current.DataFields["Vendor"].AsString();
            string sAddress = WorkflowContext.Current.DataFields["VendorAddress"].AsString();
            string sCode = WorkflowContext.Current.DataFields["VendorCode"].AsString();
            string sCity = WorkflowContext.Current.DataFields["VendorCity"].AsString();
            string sPhone = WorkflowContext.Current.DataFields["Phone"].AsString();
            string sFax = WorkflowContext.Current.DataFields["Fax"].AsString();
            string sVendorPhone = WorkflowContext.Current.DataFields["VendorPhone"].AsString();
            string sVendorFax = WorkflowContext.Current.DataFields["VendorFax"].AsString();
            string sEmail = WorkflowContext.Current.DataFields["Email"].AsString();
            string sVendorMail = WorkflowContext.Current.DataFields["VendorMail"].AsString();
            string sBuyer = WorkflowContext.Current.DataFields["Buyer"].AsString();
            string sDepartment = WorkflowContext.Current.DataFields["Department"].AsString();
            string sVendorNo = WorkflowContext.Current.DataFields["VendorNo"].AsString();
            string sVendorContact = WorkflowContext.Current.DataFields["VendorContact"].AsString();
            string sPONumber = WorkflowContext.Current.DataFields["PONumber"].AsString();
            string sPONumFinance = WorkflowContext.Current.DataFields["PONumFinance"].AsString();
            string sIssuedDate = WorkflowContext.Current.DataFields["IssuedDate"].AsString();
            string sTotal = WorkflowContext.Current.DataFields["Total"].AsString();
            string sSiteInstallFee = WorkflowContext.Current.DataFields["SiteInstallFee"].AsString();
            string sPackageCharge = WorkflowContext.Current.DataFields["PackageCharge"].AsString();
            string sFreightCost = WorkflowContext.Current.DataFields["FreightCost"].AsString();
            string sDiscount = WorkflowContext.Current.DataFields["Discount"].AsString();
            string sTaxValue = WorkflowContext.Current.DataFields["TaxValue"].AsString();
            string sGrandTotal = WorkflowContext.Current.DataFields["GrandTotal"].AsString();
            string sOrderComment1 = WorkflowContext.Current.DataFields["OrderComment1"].AsString();
            string sPaymentCondition = WorkflowContext.Current.DataFields["PaymentCondition"].AsString();
            string sDeliveryDirections = WorkflowContext.Current.DataFields["DeliveryDirections"].AsString();
            string sGuarantee = WorkflowContext.Current.DataFields["Guarantee"].AsString();
            string sDeliveryDate = WorkflowContext.Current.DataFields["DeliveryDate"].AsString();
            string sOrderComment2 = WorkflowContext.Current.DataFields["OrderComment2"].AsString();


            string strSampleFileName = GetSampleExcel(sPONumber); //"PurchaseOrdeFormSample.xls";
            string sSaveFileName ="PurchaseOrdeForm.xls";

            string sFullPath = Server.MapPath("/tmpfiles/PurchaseOrder/");
            string sFullPathSampleName = string.Concat(sFullPath, strSampleFileName);

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new GemBox.Spreadsheet.ExcelFile(); //new ExcelFile();
            objExcelFile.LoadXls(sFullPathSampleName);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];

            //worksheet1.Rows[0].Cells[12].Value = sPONOSAP;
            worksheet1.Rows[1].Cells[7].Value = sVendor;
            worksheet1.Rows[2].Cells[7].Value = sAddress;

            worksheet1.Rows[3].Cells[7].Value = sCode;
            worksheet1.Rows[3].Cells[11].Value = sCity;

            worksheet1.Rows[4].Cells[1].Value = sPhone;
            worksheet1.Rows[4].Cells[4].Value = sFax;
            worksheet1.Rows[4].Cells[7].Value = sVendorPhone;
            worksheet1.Rows[4].Cells[11].Value = sVendorFax;

            worksheet1.Rows[5].Cells[1].Value = sEmail;
            worksheet1.Rows[5].Cells[7].Value = sVendorMail;

            worksheet1.Rows[6].Cells[1].Value = sBuyer;
            worksheet1.Rows[6].Cells[4].Value = sDepartment;
            worksheet1.Rows[6].Cells[7].Value = sVendorNo;
            worksheet1.Rows[6].Cells[11].Value = sVendorContact;

            worksheet1.Rows[8].Cells[0].Value = sPONumber;
            worksheet1.Rows[8].Cells[4].Value = sPONumFinance;
            worksheet1.Rows[8].Cells[11].Value = sIssuedDate;

            worksheet1.Rows[12].Cells[0].Value = sTotal;
            worksheet1.Rows[12].Cells[2].Value = sSiteInstallFee;
            worksheet1.Rows[12].Cells[4].Value = sPackageCharge;
            worksheet1.Rows[12].Cells[6].Value = sFreightCost;
            worksheet1.Rows[12].Cells[8].Value = sDiscount;
            worksheet1.Rows[12].Cells[9].Value = sTaxValue;
            worksheet1.Rows[12].Cells[10].Value = sGrandTotal;

            worksheet1.Rows[14].Cells[0].Value = sPaymentCondition;
            worksheet1.Rows[14].Cells[3].Value = sDeliveryDirections;
            worksheet1.Rows[14].Cells[5].Value = sGuarantee;
            worksheet1.Rows[14].Cells[7].Value = sDeliveryDate;


            //string sComments = worksheet1.Rows[13].Cells[10].Value.ToString();
            //worksheet1.Rows[13].Cells[10].Value = sComments + sOrderComment1;
            //string sComments2 = worksheet1.Rows[15].Cells[10].Value.ToString();
            //worksheet1.Rows[15].Cells[10].Value = sComments + sOrderComment2;

            DataTable dt = PurchaseOrderCommon.GetDataTable(sRequestId);
            if (null != dt && dt.Rows.Count > 0)
            {
                int iRowsCount = dt.Rows.Count;
               // worksheet1.Rows[11].InsertEmpty(iRowsCount);
                worksheet1.Rows[11].InsertCopy(iRowsCount-1, worksheet1.Rows[10]);
                for (int i = 0; i < iRowsCount; i++)
                {
                    if (i == 0 || i == (iRowsCount-1))
                    {
                        System.Drawing.Color color = System.Drawing.Color.Black;
                        GemBox.Spreadsheet.LineStyle LineSyleThick = GemBox.Spreadsheet.LineStyle.Medium;//粗实线
                        GemBox.Spreadsheet.MultipleBorders borderTop = GemBox.Spreadsheet.MultipleBorders.Top;
                        GemBox.Spreadsheet.MultipleBorders borderbuttom = GemBox.Spreadsheet.MultipleBorders.Bottom;
                        GemBox.Spreadsheet.MultipleBorders border;
                        if (i == 0)
                        {
                            border = borderTop;
                        }
                        else
                        {

                            border = borderbuttom;
                        }
                        for (int j = 0; j < 14; j++)
                        {
                            worksheet1.Rows[10+i].Cells[j].SetBorders(border, color, LineSyleThick);
                        }
                    }
                    worksheet1.Rows[10 + i].Cells[0].Value = i + 1;
                    worksheet1.Rows[10 + i].Cells[1].Value = dt.Rows[i]["ItemCode"];
                    worksheet1.Rows[10 + i].Cells[2].Value = dt.Rows[i]["CostCenter"];
                    worksheet1.Rows[10 + i].Cells[3].Value = dt.Rows[i]["Description"];
                    worksheet1.Rows[10 + i].Cells[4].Value = dt.Rows[i]["TotalQuantity"];
                    worksheet1.Rows[10 + i].Cells[5].Value = dt.Rows[i]["TransQuantity"];
                    worksheet1.Rows[10 + i].Cells[6].Value = dt.Rows[i]["RequestQuantity"];
                    worksheet1.Rows[10 + i].Cells[7].Value = dt.Rows[i]["Unit"];
                    worksheet1.Rows[10 + i].Cells[8].Value = dt.Rows[i]["Currency"];
                    worksheet1.Rows[10 + i].Cells[9].Value = dt.Rows[i]["UnitPrice"];
                    worksheet1.Rows[10 + i].Cells[10].Value = dt.Rows[i]["TotalPrice"];
                    worksheet1.Rows[10 + i].Cells[11].Value = dt.Rows[i]["TaxRate"];
                    worksheet1.Rows[10 + i].Cells[12].Value = dt.Rows[i]["TaxValue"];
                    worksheet1.Rows[10 + i].Cells[12].Value = dt.Rows[i]["TaxValue"];
                    decimal dTotalPrice = 0;
                    decimal dTaxValue = 0;
                    decimal dPrice = 0;
                    if (decimal.TryParse(dt.Rows[i]["TotalPrice"].ToString(), out dTotalPrice) && decimal.TryParse(dt.Rows[i]["TaxValue"].ToString(), out dTaxValue))
                    {
                        dPrice = dTotalPrice - dTaxValue;
                        worksheet1.Rows[10 + i].Cells[13].Value = dPrice;
                    }
                    // worksheet1.Rows[11 + i].Cells[15].Value = dt.Rows[i]["AssetClass"];
                }
            }
           
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
            string sFilePath = string.Concat(sApplicationPath, "tmpfiles/PurchaseOrder/", sFileName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>popexcel('{0}');</script>", sFilePath));
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/PurchaseOrderWorkflow/MyApply.aspx");
        }

        string GetSampleExcel(string sPONumber)
        {
            string sExcelName = "PurchaseOrdeFormSample.xls";
            bool IsMaintenance= DataForm1.IsMaintenance;
            if (DDLPoType.SelectedIndex != 0)
            {
                sExcelName= "PurchaseOrdeFormSampleMaintenance.xls";
            }
            if (sPONumber.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
            {
                sExcelName = "PurchaseOrdeFormSampleReturn.xls";
            }
            return sExcelName;
        }

        protected void DDLPoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLPoType.SelectedIndex != 0)
            {
                DataForm1.SetTitle("Maintenance");
            }
            else
            {
                DataForm1.SetTitle("");
            }
        }
    }
}