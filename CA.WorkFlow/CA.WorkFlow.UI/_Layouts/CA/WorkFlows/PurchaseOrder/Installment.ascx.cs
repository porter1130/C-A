using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;

namespace CA.WorkFlow.UI.PurchaseOrder
{
    public partial class Installment : System.Web.UI.UserControl
    {

        private string _sPONO = string.Empty;

        /// <summary>
        /// PO号
        /// </summary>
        public string SPONO
        {
            get { return _sPONO; }
            set { _sPONO = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            InitData(SPONO);
        }

        /// <summary>
        /// 绑定分期付款的数据行
        /// </summary>
        /// <param name="iCount"></param>
        void BindReapeaterData(int iCount)
        {
            DataTable dt = CreateInstallData();
            for (int i = 0; i < iCount; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            ReapterInstallment.DataSource = dt;
            ReapterInstallment.DataBind();
        }

        /// <summary>
        /// 创建分期付款的数据表的列 
        /// </summary>
        /// <returns></returns>
        DataTable CreateInstallData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONo");
            dt.Columns.Add("Index"); 
            dt.Columns.Add("Paid");
            dt.Columns.Add("IsNeedGR");
            dt.Columns.Add("Comments");
            dt.Columns.Add("TotalAmount"); 
            return dt;
        }
        
        
        protected void DDLPaymentCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCount = int.Parse(DDLPaymentCount.SelectedValue);
            BindReapeaterData(iCount);
            //ReapterInstallment.
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public int SaveData(string sPONO, string sTotal)
        {
            int i = 0;
            DetePaymentInstallment(sPONO);

            if (RadioListPaymentType.SelectedIndex == 0)
            {
                i = Savinstallment(sPONO, sTotal);
            }
            else
            {
                i = SaveOncePayment(sPONO, sTotal);
            }
            return i;
        }

        /// <summary>
        /// 批删除旧的分期付款数据 。
        /// </summary>
        /// <param name="sPONO"></param>
        void DetePaymentInstallment(string sPONO)
        { 
            SPQuery query = new SPQuery();
            string sQueryFormat=@"
                                   <Where>
                                      <Eq>
                                         <FieldRef Name='PONo' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>";
            query.Query = string.Format(sQueryFormat, sPONO);
            WorkFlowUtil.BatchDeleteItems("PaymentInstallment", query);
        }

        /// <summary>
        /// 保存保分期付款数据
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="sTotal"></param>
        /// <returns></returns>
        int Savinstallment(string sPONO,string sTotal)
        {
            int i = 1;
            DataTable dt = CreateInstallData();

            foreach (RepeaterItem item in ReapterInstallment.Items)
            {
                TextBox TextPercent = item.FindControl("TextBoxPercent") as TextBox;
                TextBox TextComments = item.FindControl("TextBoxCommens") as TextBox;
                CheckBox CheckIsGRSR = item.FindControl("CheckBoxIsGRSR") as CheckBox;

                DataRow dr = dt.NewRow();
                dr["PONo"] = sPONO;
                dr["Index"] = i;
                dr["Paid"] = Convert.ToDouble(TextPercent.Text.Trim());
                dr["IsNeedGR"] = CheckIsGRSR.Checked;
                dr["Comments"] = TextComments.Text.Trim();
                dr["TotalAmount"] = sTotal;
                dt.Rows.Add(dr);
                i++;
            }
            CA.WorkFlow.UI.PurchaseRequest.PurchaseRequestCommon.BatchAddToListByDatatable(dt, "PaymentInstallment");
            return i;
        }

        /// <summary>
        /// 保存一次付款数据
        /// </summary>
        /// <param name="sPONO"></param>
        /// <param name="sTotal"></param>
        /// <returns></returns>
        int SaveOncePayment(string sPONO,string sTotal)
        {
            int i = 0;

            DataTable dt = CreateInstallData();

            DataRow dr = dt.NewRow();
            dr["PONo"] = sPONO;
            dr["Index"] = 1;
            dr["Paid"] = "100";
            dr["IsNeedGR"] = true;
            dr["Comments"] = TextBoxOnceComments.Text.Trim();
            dr["TotalAmount"] = sTotal;
            dt.Rows.Add(dr);
            CA.WorkFlow.UI.PurchaseRequest.PurchaseRequestCommon.BatchAddToListByDatatable(dt, "PaymentInstallment");
            return i;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sPONO"></param>
        void InitData(string sPONO)
        {
            if (string.IsNullOrEmpty(SPONO))
            {
                BindReapeaterData(2);
                return;
            }

            SPSecurity.RunWithElevatedPrivileges(delegate {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                { 
                    using(SPWeb web=site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        CamlExpression ce = null;
                        QueryField qfPONO = new QueryField("PONo");
                        ce = WorkFlowUtil.LinkAnd(ce,qfPONO.Equal(sPONO));
                        DataTable dt = ListQuery.Select().From(web.Lists["PaymentInstallment"]).Where(ce).GetItems().GetDataTable();
                        if (null == dt || dt.Rows.Count == 0)
                        {
                            BindReapeaterData(2);
                        }
                        else
                        {
                            int iCount = dt.Rows.Count;
                            if (iCount > 1)///分期付款
                            {
                                DDLPaymentCount.SelectedIndex = iCount-2;
                                RadioListPaymentType.SelectedIndex = 0;
                                ReapterInstallment.DataSource = dt;
                                ReapterInstallment.DataBind();
                            }
                            else//一次付款
                            {
                                RadioListPaymentType.SelectedIndex = 1;
                                TextBoxOnceComments.Text = dt.Rows[0]["Comments"].ToString();
                                BindReapeaterData(2);
                            }
                        }
                    }
                }
            });
        }
    }
}