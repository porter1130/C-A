namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using System.Web.UI;
    using System.Data;

    public partial class HistoryView : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPaymentRequestHistoryInfo(Request.QueryString["PONO"].ToString());
            }
        }

        private void BindPaymentRequestHistoryInfo(string poNO)
        {
            DataTable dTable1 = PaymentRequestComm.GetPaymentRequestItemsInfoByPONO(poNO).GetDataTable();
            DataTable dTable2 = PaymentRequestComm.GetPaymentInstallmentInfo(poNO).GetDataTable();
            if (dTable1 != null && dTable2 != null)
            {
                string formPO = dTable1.Rows[0]["IsFromPO"].ToString();
                System.Data.DataView dView2 = dTable2.DefaultView;
                dView2.Sort = "Index";
                foreach (DataRow dRow2 in dView2.ToTable().Rows)
                {
                    bool isHaveThisLine = false;
                    decimal paidBefore = 0;

                    foreach (DataRow dRow1 in dTable1.Rows)
                    {
                        paidBefore += decimal.Parse(dRow1["PaidThisTime"].ToString());
                        if (dRow1["PaidInd"].ToString() == dRow2["Index"].ToString())
                        {
                            isHaveThisLine = true;
                        }
                    }

                    if (isHaveThisLine == false)
                    {
                        DataRow dRow = dTable1.NewRow();
                        dRow["PaidInd"] = dRow2["Index"].ToString();
                        dRow["TotalAmount"] = dRow2["TotalAmount"].ToString();
                        dRow["PaidThisTime"] = dRow2["Paid"].ToString();
                        dRow["Balance"] = 100 - paidBefore - decimal.Parse(dRow2["Paid"].ToString());
                        dRow["PaidBefore"] = paidBefore;
                        dRow["Status"] = "NotStarted";
                        dTable1.Rows.Add(dRow);
                    }
                }

                dTable1.Columns.Add("PaidThisTimeAmount");
                dTable1.Columns.Add("PaidBeforeAmount");
                dTable1.Columns.Add("BalanceAmount");

                for (int i = 0; i < dTable1.Rows.Count; i++)
                {
                    if (dTable1.Rows[i]["TotalAmount"].ToString().IsNullOrWhitespace() == false)
                    {
                        decimal amount = decimal.Parse(dTable1.Rows[i]["TotalAmount"].ToString());
                       
                        object obj = Request.QueryString["IsFromPO"];
                        
                        if (formPO == "1" || obj != null)
                        {
                            dTable1.Rows[i]["PaidThisTimeAmount"] = Math.Round(amount * decimal.Parse(dTable1.Rows[i]["PaidThisTime"].ToString()) / 100, 2);
                            dTable1.Rows[i]["PaidBeforeAmount"] = Math.Round(amount * decimal.Parse(dTable1.Rows[i]["PaidBefore"].ToString()) / 100, 2);
                            dTable1.Rows[i]["BalanceAmount"] = Math.Round(amount * decimal.Parse(dTable1.Rows[i]["Balance"].ToString()) / 100, 2);
                        }
                        else
                        {
                            if (i != 0)
                            {
                                decimal paidBeforeAmount = 0;
                                int go = i;
                                --go;
                                while (go>= 0)
                                {
                                    if (dTable2.Rows[go]["IsPaid"].AsString() == "1")
                                    {
                                        paidBeforeAmount += decimal.Parse(dTable2.Rows[go]["PaidThisTimeAmount"].AsString());
                                    }
                                    --go;
                                };
                                dTable1.Rows[i]["PaidBeforeAmount"] = paidBeforeAmount;
                                dTable1.Rows[i]["BalanceAmount"] = amount - paidBeforeAmount - decimal.Parse(dTable2.Rows[i]["PaidThisTimeAmount"].AsString());
                            }
                            if (i == 0)
                            {
                                dTable1.Rows[i]["PaidBeforeAmount"] = 0;
                                dTable1.Rows[i]["BalanceAmount"] = amount - decimal.Parse(dTable2.Rows[i]["PaidThisTimeAmount"].AsString());
                            }
                            dTable1.Rows[i]["PaidThisTimeAmount"] = decimal.Parse(dTable2.Rows[i]["PaidThisTimeAmount"].AsString());
                        }
                    }
                }
                System.Data.DataView dView1 = dTable1.DefaultView;
                dView1.Sort = "PaidInd";

                rptPRInfo.DataSource = dView1;
                rptPRInfo.DataBind();
            }
        }
    }
}