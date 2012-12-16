using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using System.Data;
using System.Text;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class GetItemCode :  System.Web.UI.Page//CAWorkFlowPage//
    {
        /// <summary>
        /// Itemcode缓存的Key
        /// </summary>
        internal string sItemCacheKey = "ItemCode";
        internal TimeSpan tsCache = TimeSpan.FromMinutes(PurchaseRequestCommon.GetPRCacheMinutes());

        int iPageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sItemCode = Context.Request.QueryString["ItemCode"].ToString();
                string sDecription = Context.Request.QueryString["Desc"].ToString();
                string sItemTyp = Context.Request.QueryString["ItemType"].ToString();// QO,PB......
                string sItemStart = Context.Request.QueryString["sItemStart"].ToString();//E,X 
                string sIsStore = Context.Request.QueryString["isStore"].ToString();// 
                int iPageIndex = int.Parse(Context.Request.QueryString["pageIndex"].ToString());
                string sJson = SearchItemCode(sItemCode, sDecription, sItemTyp, sItemStart, iPageIndex, sIsStore == "1" ? true : false);
                
                Response.Clear();
                Response.Write(sJson);
            }
            catch (Exception ex)
            {
                CommonUtil.logError("CA.WorkFlow.UI.PurchaseRequest.GetItemCode has an error:" + ex.ToString());
            }
            Response.End();
        }

        string SearchItemCode(string sItemCode, string sDecription, string sItemTyp, string sItemStart, int iPageIndex, bool isStore)
        {
            string sJson = string.Empty;
            DataTable dtResult = new DataTable();
            DataTable dt = new DataTable();
            dt = PurchaseRequestCommon.GetActiveItemCode();/////得到所有的可用的ItemCode数据
            EnumerableRowCollection<DataRow> drColle = from dr in dt.AsEnumerable()
                                                       where (string.IsNullOrEmpty(sItemCode) || AsString(dr["Title"]).StartsWith(sItemCode, StringComparison.CurrentCultureIgnoreCase))
                                                       && (string.IsNullOrEmpty(sDecription) || AsString(dr["Description"]).Contains(sDecription))
                                                       && (isStore ? AsString(dr["ItemScope"]).Trim().Equals(sItemTyp, StringComparison.CurrentCultureIgnoreCase) : true) //QO,PB......
                                                       && (AsString(dr["Title"]).StartsWith(sItemStart, StringComparison.CurrentCultureIgnoreCase) || AsString(dr["Title"]).StartsWith("X", StringComparison.CurrentCultureIgnoreCase))//E,C
                                                       select dr;
            DataTable dt10 = drColle.CopyToDataTable();
            int iCount = dt10.Rows.Count;
            if (iCount > 0)
            {
                int iCurrentRow = (iPageIndex - 1) * iPageSize;

                List<DataRow> listDr = new List<DataRow>();

                int iMaxRowIndex = iCurrentRow + iPageSize;
                if (iCount < iPageSize)///当结果行数少于每页显示的行数
                {
                    iMaxRowIndex = iCount;
                }
                if (iMaxRowIndex > iCount)//请求的最后一页所要求的行数大于所在行数。
                {
                    iMaxRowIndex = iCount;
                }


                for (int i = iCurrentRow; i < iMaxRowIndex; i++)
                {
                    listDr.Add(dt10.Rows[i]);
                }
                dtResult = listDr.CopyToDataTable();
                sJson = PurchaseRequestCommon.DataTableToJson("ItemCode", dtResult, iCount);
            }
            return sJson;
        }


        /// <summary>
        /// 得到可用的ItemCode的数据（从缓存中读取。）
        /// </summary>
        /// <returns></returns>
        public DataTable GetActiveItemCode()
        {
            DataTable dt = new DataTable();
            dt = HttpContext.Current.Cache[sItemCacheKey] as DataTable;
            if (dt == null)///缓存过期或缓存中没有
            {
                SPQuery query = new SPQuery();
                query.Query = @"
                            <Where>
                                <Eq>
                                    <FieldRef Name='IsActive' />
                                    <Value Type='Boolean'>1</Value>
                                </Eq>
                            </Where>";
                ///query.RowLimit = 10;
                query.ViewFields = GetQueryFiled();
                dt = SPContext.Current.Web.Lists["Item Codes"].GetItems(query).GetDataTable();
                HttpContext.Current.Cache.Insert(sItemCacheKey, dt, null, System.Web.Caching.Cache.NoAbsoluteExpiration, tsCache);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetQueryFiled()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldRef Name='Title'/>");
            sb.Append("<FieldRef Name='ItemType'/>");
            sb.Append("<FieldRef Name='Description'/>");
            sb.Append("<FieldRef Name='Unit'/>");
            sb.Append("<FieldRef Name='AssetClass'/>");
            sb.Append("<FieldRef Name='VendorID'/>");
            sb.Append("<FieldRef Name='DeliveryPeriod'/>");
            sb.Append("<FieldRef Name='UnitPrice'/>");
            sb.Append("<FieldRef Name='TaxValue'/>");
            sb.Append("<FieldRef Name='IsAccpetDecimal'/>");
            sb.Append("<FieldRef Name='Currency'/>");
            sb.Append("<FieldRef Name='ItemScope'/>");
            sb.Append("<FieldRef Name='PackagedRegulation'/>");
            return sb.ToString();
        }


        string AsString(object o)
        {
            if (o == null)
            {
                return string.Empty;
            }
            else
            {
                return o.ToString();
            }
        }
    }
}