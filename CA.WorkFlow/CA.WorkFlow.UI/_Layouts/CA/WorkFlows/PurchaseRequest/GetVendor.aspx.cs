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
    public partial class GetVendor : System.Web.UI.Page
    {
        //internal TimeSpan tsCache = TimeSpan.FromMinutes(PurchaseRequestCommon.GetPRCacheMinutes());
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sVendorID = Context.Request.QueryString["VendorID"].ToString();
                string sJson = GetVendorID(sVendorID);

                Response.Clear();
                Response.Write(sJson);
            }
            catch (Exception ex)
            {
                CommonUtil.logError("CA.WorkFlow.UI.PurchaseRequest.GetVendor has an error:" + ex.ToString());
            }
            Response.End();
        }

        string GetVendorID(string sVendorID)
        {
            string sJson = string.Empty;
            DataTable dt = new DataTable();
            dt = PurchaseRequestCommon.GetVendorFromCache();///得到所有的可用的ItemCode数据

            EnumerableRowCollection<DataRow> drColle = from dr in dt.AsEnumerable()
                                                       where AsString(dr["VendorID"]).Trim().Equals(sVendorID, StringComparison.CurrentCultureIgnoreCase)
                                                       select dr;
            int iCount = drColle.Count();
            if (iCount > 0)
            {
                DataTable dtResult = drColle.CopyToDataTable();
                sJson = PurchaseRequestCommon.DataTableToJson("Vendor", dtResult, 1);
            }
            return sJson;
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