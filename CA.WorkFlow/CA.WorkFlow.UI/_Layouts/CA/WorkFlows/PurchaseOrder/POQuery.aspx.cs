using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.SharePoint;
using System.Text;

namespace CA.WorkFlow.UI.PurchaseOrder
{
    public partial class POQuery : CAWorkFlowPage
    {
        public string sDisplayURL = "/WorkFlowCenter/Lists/PurchaseOrderWorkflow/DispForm.aspx?ID=";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string sPO = TextBoxPONO.Text.Trim();
            if (sPO.Length == 0)
            {
                return;
            }
            GetPOBaseInfo(sPO);

            if (sPO.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))//是退货的PO
            {
                GetReturnPOInfo(sPO);//PO号带 R
            }
            /*else//不是退货的PO
            {
                QueryPORefReturnInfo(sPO);
            }*/
        }

        /// <summary>
        /// 显示PO的基本信息
        /// </summary>
        /// <param name="sPONO"></param>
        void GetPOBaseInfo(string sPONO)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        string sPRNO = string.Empty;
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(@"
                                                   <Where>
                                                      <Eq>
                                                         <FieldRef Name='PONumber' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                   </Where>", sPONO);
                        SPListItemCollection splic = web.Lists["Purchase Order Workflow"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            HyperLinkPO.Text = splic[0]["PONumber"] == null ? string.Empty : splic[0]["PONumber"].ToString();
                            HyperLinkPO.NavigateUrl = sDisplayURL + splic[0]["ID"].ToString();

                            if (splic[0]["SapNO"] == null  && splic[0]["SapErrorInfo"] == null )//没有POST
                            {
                                LabelSAPStatus.Text = "UnPost";
                                LabelSAPGRSR.Text = "N/A";
                            }
                            else
                            {
                                if (splic[0]["SapNO"] == null || splic[0]["SapNO"].ToString().Length == 0)// POSt出错
                                {
                                    LabelSAPStatus.Text = CAWorkflowStatus.Completed;
                                    LabelSAPGRSR.Text = "NO";
                                }
                                else
                                {
                                    LabelSAPStatus.Text = CAWorkflowStatus.Completed;//Post成功
                                    LabelSAPGRSR.Text = "YES";
                                }
                            }
                        }
                        else
                        {
                            LabelSAPStatus.Text = "NULL";
                            HyperLinkPO.Text = "NULL";
                            HyperLinkPO.NavigateUrl = string.Empty;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 得到PO关联的退货PO信息
        /// </summary>
        /// <param name="sPONO"></param>
      /*  void QueryPORefReturnInfo(string sPONO)
        {
            string sReturnPO = string.Empty; //退货的目标PO
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        string sPRNO = string.Empty;
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(@"
                                                       <Where>
                                                          <Eq>
                                                             <FieldRef Name='ReturnNumber' />
                                                             <Value Type='Text'>{0}</Value>
                                                          </Eq>
                                                       </Where>", sPONO);
                        SPListItemCollection splic = web.Lists["Purchase Request Workflow"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            sReturnPO = splic[0]["PONumber"].ToString();
                        }
                    }
                }
            });
            StringBuilder sb = new StringBuilder();
            foreach (string sPO in sReturnPO.Split(';'))
            {
                if (sPO.Trim().Length == 0)
                {
                    continue;
                }
                string sPOID = GetPOID(sPO);

                sb.Append(string.Format("<a href='{0}{1}' target='_blank'>", sDisplayURL, sPOID));
                sb.Append(sPO);
                sb.Append("</a><br />");
            }
            LiteralLink.Text = sb.ToString();
        }*/

        /// <summary>
        /// 得到退货PO的信息,PO号带R
        /// </summary>
        /// <param name="sReturnPONO"></param>
        void GetReturnPOInfo(string sReturnPONO)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PO");
            dt.Columns.Add("ID");
            dt.Columns.Add("IsGR");

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        string sPRNO = string.Empty;
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(@"
                                                       <Where>
                                                         <Contains>
                                                             <FieldRef Name='PONumber' />
                                                             <Value Type='Text'>{0}</Value>
                                                          </Contains>
                                                       </Where>", sReturnPONO);
                        SPListItemCollection splic = web.Lists["Purchase Request Workflow"].GetItems(query);
                        foreach (SPListItem item in splic)
                        { 
                            string bSAPGRStatus="Null";
                            string sPO=splic[0]["ReturnNumber"].ToString();
                            string sPOID = GetPOID(sPO, out bSAPGRStatus);
                            DataRow dr = dt.NewRow();
                            dr["PO"]=sPO;
                            dr["ID"]=sPOID;
                            dr["IsGR"] = bSAPGRStatus;
                            dt.Rows.Add(dr);
                        }
                    }
                }
            });
            RepeaterRelatedPO.DataSource = dt;
            RepeaterRelatedPO.DataBind();
        }


        /// <summary>
        /// 得到PO的ID
        /// </summary>
        /// <param name="sPONO"></param>
        /// <returns></returns>
        string GetPOID(string sPONO,out string bSAPGRStatus)
        {
            string isSAPGR = "Null";
            string sID = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        string sPRNO = string.Empty;
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(@"
                                                       <Where>
                                                          <Eq>
                                                             <FieldRef Name='PONumber' />
                                                             <Value Type='Text'>{0}</Value>
                                                          </Eq>
                                                       </Where>", sPONO);
                        SPListItemCollection splic = web.Lists["Purchase Order Workflow"].GetItems(query);
                        if (null != splic && splic.Count > 0)
                        {
                            sID = splic[0]["ID"].ToString();
                            if (null != splic[0]["IsSystemGR"])
                            {
                                isSAPGR = splic[0]["IsSystemGR"].ToString();
                            }
                        }
                    }
                }
            });
            bSAPGRStatus = isSAPGR;
            return sID;
        }
    }
}