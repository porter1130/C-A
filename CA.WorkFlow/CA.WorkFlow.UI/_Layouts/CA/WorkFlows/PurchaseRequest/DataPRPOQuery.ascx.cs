using System;
using System.Web.UI;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;

using Microsoft.SharePoint;



namespace CA.WorkFlow.UI.PurchaseRequest
{

    public partial class DataPRPOQuery : UserControl
    {
        private readonly ObjectDataSource dataSource1 = new ObjectDataSource();
        private readonly ObjectDataSource dataSource2 = new ObjectDataSource();
        private readonly ObjectDataSource dataSourcePRPO = new ObjectDataSource();

        //private bool isHO;
        //internal bool IsHO { set { isHO = value; } }

        private string currentAccount;
        internal string CurrentAccount { set { currentAccount = value; } }

        protected override void OnLoad(EventArgs e)
        {
            this.dataSource1.ID = "PRDS";
            this.dataSource1.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSource1.SelectMethod = "GetDataTablefromPR";
            this.Controls.Add(this.dataSource1);
            
            this.dataSource2.ID = "PODS";
            this.dataSource2.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSource2.SelectMethod = "GetDataTableFromPO";
            this.Controls.Add(this.dataSource2);

            this.dataSourcePRPO.ID = "PRPODS";
            this.dataSourcePRPO.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSourcePRPO.SelectMethod = "GetDataTableByPRPO";
            this.Controls.Add(this.dataSourcePRPO);



            
            base.OnLoad(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.SPGridView1.BorderStyle = BorderStyle.Solid;
                this.SPGridView1.GridLines = GridLines.Horizontal;
                this.SPGridView1.DataSource = null;
                this.SPGridView1.DataBind();

                this.SPGridView2.BorderStyle = BorderStyle.Solid;
                this.SPGridView2.GridLines = GridLines.Horizontal;
                this.SPGridView2.DataSource = null;
                this.SPGridView2.DataBind();
            }
            if (!IsHO())
            {
                PO.Visible = false;
            }
        }

        bool IsHO()
        {
            bool isHO = false;
            string sName = SPContext.Current.Web.CurrentUser.LoginName;
            if (PurchaseRequestCommon.IsInGroup(sName, "wf_HO"))
            {
                isHO = true;
            }
            return isHO;
        }

        protected void btnQuery_Click(object sendor, EventArgs e)
        {
            string prNumber = this.txtPRNumber.Text.Trim();
            string poNumber = this.txtPONumber.Text.Trim();
           // DataTable prItems = null;
            DataTable poItems = null;
            string prListName = "Purchase Request Workflow";
            if (IsHO())
            {

                if (string.IsNullOrEmpty(prNumber) && string.IsNullOrEmpty(poNumber))
                {
                    ClearData();
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(prNumber))
                {
                    ClearData();
                    return;
                } 
                //Adjust the pr is created by current user
                bool isLegal = IsWorkflowCreator(prListName, prNumber);
                if (!isLegal)
                {
                    ClearData();
                    return;
                }
            }

            if (!string.IsNullOrEmpty(prNumber) && string.IsNullOrEmpty(poNumber))
            {
                this.dataSource1.SelectParameters.Clear();
                this.dataSource1.SelectParameters.Add("column", DbType.String, "Title");
                this.dataSource1.SelectParameters.Add("val", DbType.String, prNumber);
                this.dataSource1.SelectParameters.Add("listName", DbType.String, "PurchaseRequestItems");

                this.SPGridView1.DataSourceID = "PRDS";
                this.SPGridView1.DataBind();
                decimal dPrTotalPrice = GetTotalPrice("Title", prNumber, "PurchaseRequestItems");
                LabelPRPrice.Text = dPrTotalPrice.ToString();

                this.dataSource2.SelectParameters.Clear();
                this.dataSource2.SelectParameters.Add("column", DbType.String, "PRNumber");
                this.dataSource2.SelectParameters.Add("val", DbType.String, prNumber);
                this.dataSource2.SelectParameters.Add("listName", DbType.String, "PurchaseOrderItems");

                this.SPGridView2.DataSourceID = "PODS";
                this.SPGridView2.DataBind();
                decimal dPoTotalPrice = GetTotalPrice("PRNumber", prNumber, "PurchaseOrderItems");
                LabelPOPrice.Text = dPoTotalPrice.ToString();
            }
            else if (!string.IsNullOrEmpty(poNumber) && string.IsNullOrEmpty(prNumber))
            {
                poItems = GetDataForDS("Title", poNumber, "PurchaseOrderItems");
                if (poItems == null || poItems.Rows.Count == 0)
                {
                    ClearData();
                    return;
                }

                this.dataSource2.SelectParameters.Clear();
                this.dataSource2.SelectParameters.Add("column", DbType.String, "Title");
                this.dataSource2.SelectParameters.Add("val", DbType.String, poNumber);
                this.dataSource2.SelectParameters.Add("listName", DbType.String, "PurchaseOrderItems");
                this.SPGridView2.DataSourceID = "PODS";
                this.SPGridView2.DataBind();
                decimal dPoTotalPrice = GetTotalPrice("Title", poNumber, "PurchaseOrderItems");
                LabelPOPrice.Text = dPoTotalPrice.ToString();

                this.dataSource1.SelectParameters.Clear();
                this.dataSource1.SelectParameters.Add("column", DbType.String, "PONumber");
                this.dataSource1.SelectParameters.Add("val", DbType.String, poNumber);
                this.dataSource1.SelectParameters.Add("listName", DbType.String, "PurchaseRequestItems");

                this.SPGridView1.DataSourceID = "PRDS";
                this.SPGridView1.DataBind();
                decimal dPrTotalPrice = GetTotalPrice("PONumber", poNumber, "PurchaseRequestItems");
                LabelPRPrice.Text = dPrTotalPrice.ToString();

            }
            else if (!string.IsNullOrEmpty(poNumber) &&!string.IsNullOrEmpty(prNumber))
            {
                //public DataTable GetDataTableByPRPO(string sListName,string sPRColumn,string sPOColumn,string sPRVal, string sPOVal)    PurchaseRequestItems    PurchaseOrderItems


                this.dataSourcePRPO.SelectParameters.Clear();
                this.dataSourcePRPO.SelectParameters.Add("sListName", DbType.String, "PurchaseRequestItems");
                this.dataSourcePRPO.SelectParameters.Add("sPRColumn", DbType.String, "Title");
                this.dataSourcePRPO.SelectParameters.Add("sPOColumn", DbType.String, "PONumber");
                this.dataSourcePRPO.SelectParameters.Add("sPRVal", DbType.String, prNumber);
                this.dataSourcePRPO.SelectParameters.Add("sPOVal", DbType.String, poNumber);
                
                this.SPGridView1.DataSourceID = "PRPODS";
                this.SPGridView1.DataBind();
                decimal dPrTotalPrice = GetPRPOTotalPrice("PurchaseRequestItems", "Title", "PONumber", prNumber, poNumber);
                LabelPRPrice.Text = dPrTotalPrice.ToString();

                this.dataSourcePRPO.SelectParameters.Clear();
                this.dataSourcePRPO.SelectParameters.Add("sListName", DbType.String, "PurchaseOrderItems");
                this.dataSourcePRPO.SelectParameters.Add("sPRColumn", DbType.String, "PRNumber");
                this.dataSourcePRPO.SelectParameters.Add("sPOColumn", DbType.String, "Title");
                this.dataSourcePRPO.SelectParameters.Add("sPRVal", DbType.String, prNumber);
                this.dataSourcePRPO.SelectParameters.Add("sPOVal", DbType.String, poNumber);

                this.SPGridView2.DataSourceID = "PRPODS";
                this.SPGridView2.DataBind();
                decimal dPototalPrice = GetPRPOTotalPrice("PurchaseOrderItems", "PRNumber", "Title", prNumber,poNumber);
                LabelPOPrice.Text = dPototalPrice.ToString();
            }
        }

        /**
         * Adjust the workflow item is created by current user
         **/
        private bool IsWorkflowCreator(string prListName, string prNumber)
        {
            string currentAccount = SPContext.Current.Web.CurrentUser.LoginName;
            return PurchaseRequestCommon.IsItemCreateByUser(prListName,prNumber,currentAccount);
        }

        DataTable GetDataForDS(string column, string val, string listName)
        {
            return WorkFlowUtil.GetCollectionByColumn(column, val, listName).GetDataTable();
        }
        /// <summary>
        /// 清空页面上的数据
        /// </summary>
        void ClearData()
        {
            SPGridView1.Columns.Clear();
            LabelPRPrice.Text = string.Empty;

            SPGridView2.Columns.Clear();
            LabelPOPrice.Text = string.Empty;
        }

        /// <summary>
        /// 得到 PurchaseRequestItems  的数据源
        /// </summary>
        /// <param name="column"></param>
        /// <param name="val"></param>
        /// <param name="listName"></param>
        /// <param name="sPRIDS"></param>
        /// <returns></returns>
        public DataTable GetDataTablefromPR(string column, string val, string listName)//,string sPRIDS)
        {
            DataTable dt = new DataTable();
            dt = WorkFlowUtil.GetCollectionByColumn(column, val, listName).GetDataTable();
            return ConvertPRSource(dt);
        }

        /// <summary>
        /// 得与column=val的listName下的totalPrice总和
        /// </summary>
        /// <param name="column"></param>
        /// <param name="val"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        decimal GetTotalPrice(string column, string val, string listName)
        {
            DataTable dt = new DataTable();
            dt = WorkFlowUtil.GetCollectionByColumn(column, val, listName).GetDataTable();
            if (null == dt)
            {
                return 0;
            }
            decimal dTotalPrice = 0;
            var linq = from dr in dt.AsEnumerable()
                       select dr["TotalPrice"];
            foreach (var item in linq)
            {
               if(null!=item&&item.ToString().Length>0)
               {
                   dTotalPrice += decimal.Parse(item.ToString());
               }
            }
            return dTotalPrice;
        }


        /// <summary>
        ///  得到 PurchaseOrderItems 的数据源
        /// </summary>
        /// <param name="column"></param>
        /// <param name="val"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromPO(string column, string val, string listName)
        {
            DataTable dt = new DataTable();
            dt = WorkFlowUtil.GetCollectionByColumn(column, val, listName).GetDataTable();
            return ConvertPOSource(dt);
        }

        /// <summary>
        /// 由PRNO,PONO得到数据
        /// </summary>
        /// <param name="sListName"></param>
        /// <param name="sPRColumn"></param>
        /// <param name="sPOColumn"></param>
        /// <param name="sPRVal"></param>
        /// <param name="sPOVal"></param>
        /// <returns></returns>
        public DataTable GetDataTableByPRPO(string sListName,string sPRColumn,string sPOColumn,string sPRVal, string sPOVal)
        {
            DataTable dt = new DataTable();
            dt = PurchaseRequestCommon.GetDataByPRPO(sListName,sPRColumn,sPOColumn,sPRVal,sPOVal);
            if (sListName == "PurchaseRequestItems")
            {
                return ConvertPRSource(dt);
            }
            else
            {
                return ConvertPOSource(dt);
            }
        }

        /// <summary>
        /// 得到sListName下，sPRColumn=sPRVal，sPOColumn=sPOVal的totalPrice总和 
        /// </summary>
        /// <param name="sListName"></param>
        /// <param name="sPRColumn"></param>
        /// <param name="sPOColumn"></param>
        /// <param name="sPRVal"></param>
        /// <param name="sPOVal"></param>
        /// <returns></returns>
        decimal GetPRPOTotalPrice(string sListName, string sPRColumn, string sPOColumn, string sPRVal, string sPOVal)
        {
            decimal dTotalPrice=0;

            DataTable dt = new DataTable();
            dt = PurchaseRequestCommon.GetDataByPRPO(sListName, sPRColumn, sPOColumn, sPRVal, sPOVal);
            if (null == dt)
            {
                return 0;
            }
            var linq = from dr in dt.AsEnumerable()
                       select dr["TotalPrice"];
            foreach (var item in linq)
            {
                if (null != item && item.ToString().Length > 0)
                {
                    dTotalPrice += decimal.Parse(item.ToString());
                }
            }
            return dTotalPrice;
        }

        /// <summary>
        /// 构造PurchaseRequestItems  的数据源 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable ConvertPRSource(DataTable dt)
        {
            if (null == dt)
            {
                return null;
            }
            DataTable dtResult = GetSampleTable();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtResult.NewRow();
                drNew["Title"] = dr["Title"];    //PRNumber
                drNew["PONumber"] = dr["PONumber"];  //Title
                drNew["VendorName"] = dr["VendorName"];  //VendorName
                drNew["TotalPrice"] = dr["TotalPrice"];//TotalPrice
                dtResult.Rows.Add(drNew);
            }
            return dtResult;
        }

        /// <summary>
        ///  构造 PurchaseOrderItems  的数据源 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable ConvertPOSource(DataTable dt)
        {
            if (null == dt)
            {
                return null;
            }
            DataTable dtResult = GetSampleTable();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtResult.NewRow();
                drNew["Title"] = dr["PRNumber"];
                drNew["PONumber"] = dr["Title"];
                drNew["VendorName"] = dr["VendorName"];
                drNew["TotalPrice"] = dr["TotalPrice"];
                dtResult.Rows.Add(drNew);
            }
            return dtResult;
        }

        /// <summary>
        /// 得到数据源模板
        /// </summary>
        /// <returns></returns>
        DataTable GetSampleTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("PONumber");
            dt.Columns.Add("VendorName");
            dt.Columns.Add("TotalPrice");
            return dt;
        }

        protected void SPGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SPGridView1.PageIndex = e.NewPageIndex;
            SPGridView1.DataBind();
        }

        protected void SPGridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SPGridView2.PageIndex = e.NewPageIndex;
            SPGridView2.DataBind();
        }
        //protected void btnOpenDetail_Click(object sendor, EventArgs e)
        //{
        //    var detailStr = this.hidDetail.Value; //Purchase Request Workflow;PR0001;PurchaseRequest'
        //    char[] split = { ';' };
        //    var details = detailStr.Split(split);
        //    if (details == null || details.Length == 0)
        //    {
        //        return;
        //    }
        //    string listId = WorkflowListID.GetListId(details[0]);
        //    string id = GetId(details[0], details[1]);

        //    string link = string.Format("/WorkFlowCenter/_Layouts/CA/WorkFlows/{0}/DisplayForm.aspx?List={1}&ID={2}", details[2], listId, id);

        //    if (id != null)
        //    {
        //        this.hidNewWindowLink.Value = link;
        //        //this.Response.Redirect(link);
        //        //this.Response.Write(string.Format("<script type='text/javascript'>window.open('{0}', '_blank');</script>", link));
        //    }
        //}

        //private string GetId(string listName, params string[] values)
        //{
        //    var result = PurchaseRequestCommon.GetRecordId(listName, "WorkflowNumber", "Title", values);
        //    return result.Count > 0 ? result[0] : null;
        //}

        //private List<string> GetIds()
        //{
        //    List<string> ids = new List<string>();
            
        //    foreach (DataRow dr in poItems.Rows)
        //    {
        //        ids.Add(dr["PRItemID"].ToString());
        //    }
        //    return ids;
        //}

        //private string GetTotal(DataTable dt)
        //{
        //    double total = 0;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        total += Convert.ToDouble(dr["TotalPrice"].ToString());
        //    }

        //    return Convert.ToString(Math.Round(total, 2));
        //}

    }
}