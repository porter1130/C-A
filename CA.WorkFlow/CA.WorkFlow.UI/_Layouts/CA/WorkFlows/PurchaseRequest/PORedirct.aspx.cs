using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class PORedirct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //PONO

            if (null != Request.QueryString["PONO"])
            {
                string sID = GetPOID(Request.QueryString["PONO"].ToString()).Trim();
                if (sID.Length > 0)
                {

                    string sDisplayURL = "/WorkFlowCenter/Lists/PurchaseOrderWorkflow/DispForm.aspx?ID=" + sID;
                    Response.Redirect(sDisplayURL);
                }
                else
                {
                    Response.Redirect("GenerateReport.aspx?TYPE=PRPOREPORT");
                }
            }
            else
            {
                Response.Redirect("GenerateReport.aspx?TYPE=PRPOREPORT");
            }
        }

        /// <summary>
        /// 根据PO号得到ID
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        string GetPOID(string sPONO)
        {
            string sID = string.Empty;
            SPQuery queryCamle = new SPQuery();
            queryCamle.Query = string.Format(@"
                                                <Where>
                                                    <Eq>
                                                        <FieldRef Name='Title' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </Where>", sPONO);
            SPListItemCollection splic = SPContext.Current.Web.Lists["Purchase Order Workflow"].GetItems(queryCamle);
            if (null != splic)
            {
                foreach (SPListItem item in splic)
                {
                    sID = item["ID"] == null ? string.Empty : item["ID"].ToString();
                    break;
                }
            }
            return sID;
        }
    }
}