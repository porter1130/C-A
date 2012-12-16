using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Collections.Generic;
using QuickFlow.Core;
using Microsoft.SharePoint.WebControls;
using SAP.Middleware.Exchange;

namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class DataListSAPView : QFUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.AddHeader("pragma", "no-cache");
            Response.CacheControl = "no-cache";
            if (!IsPostBack)
            {
                BindSAPData();
            }
        }

        private void BindSAPData()
        {
            DataTable dt = GetSAPDataTable();
           
            rpSAPData.DataSource = dt;
           
            rpSAPData.DataBind();
        }

        private DataTable GetSAPDataTable()
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequestSAP");
            SPQuery query = new SPQuery();

            query.Query = "<Where><And><Eq><FieldRef Name='Status' /><Value Type='Text'>Completed</Value></Eq><Eq><FieldRef Name='SAPStatus' /><Value Type='Text'>0</Value></Eq></And></Where><OrderBy><FieldRef Name='Title' Ascending='False' /></OrderBy>";
            SPListItemCollection listItems = delegationList.GetItems(query);
            query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;

            DataTable dt = listItems.GetDataTable();
            return dt;
        }

        
        public void btnCashAdvanceRelateToSAP_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.rpSAPData.Items)
            {
                CheckBox chkitem = (CheckBox)item.FindControl("ckAllItems");
                if (chkitem.Checked)
                {
                    HiddenField hfwfID = (HiddenField)item.FindControl("hfwfID");
                    HiddenField hfEmployeeID = (HiddenField)item.FindControl("hfEmployeeID");
                    HiddenField hfEmployeeName = (HiddenField)item.FindControl("hfEmployeeName");
                    HiddenField hfCAWorkflowNumber = (HiddenField)item.FindControl("hfCAWorkflowNumber");
                    HiddenField hfAmount = (HiddenField)item.FindControl("hfAmount");

                    //借款类型 cash transfer
                    HiddenField hfAdvanceType = (HiddenField)item.FindControl("hfAdvanceType");
                    //借款用途或备注，格式用分号隔开，分别为：“借款用途+;+紧急借款备注+;+备注”
                    HiddenField hfAdvanceRemark = (HiddenField)item.FindControl("hfAdvanceRemark");
                    //List<string> arList = hfAdvanceRemark.Value.Split(';').ToList<string>();
                    
                    //Post SAP
                    List<SapParameter> mSapParametersCD = new List<SapParameter>();
                    SapParameter mSapParameters = new SapParameter()
                    {
                        BusAct = "RFBU",
                        CompCode = "CA10",
                        DocType = "KR",
                        BusArea = "0001",
                        Currency = "RMB",
                        EmployeeID = hfEmployeeID.Value,
                        EmployeeName = hfEmployeeName.Value,
                        ExchRate = 1,
                        Header = hfAdvanceRemark.Value,
                        RefDocNo = hfCAWorkflowNumber.Value,
                        UserName = "acnotes",
                        CashAmount = decimal.Parse(hfAmount.Value),
                        PaidByCC = 100,
                        PymtMeth = hfAdvanceType.Value == "Cash" ? "E" : ""


                    };
                    mSapParametersCD.Add(mSapParameters);
                    string sAPNumber = "";
                    string errorMsg = "";
                    bool postResult = false;
                    ISapExchange sapExchange = SapExchangeFactory.GetCashAdvance();
                    List<object[]> result = sapExchange.ImportDataToSap(mSapParametersCD);
                    //if (null == result)
                    //{
                    //    errorMsg += hfEmployeeName + "-" + DateTime.Now.ToShortDateString() + "：" + "Connection failed.";
                    //}
                    if (null == result)
                    {
                        this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Connection failed !'); window.location = window.location;</script>");
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            SapParameter sp = (SapParameter)result[i][0];
                            bool bl = (bool)result[i][2];
                            if (bl)
                            {
                                SapResult sr = (SapResult)result[i][1];
                                sAPNumber = sr.OBJ_KEY;
                                postResult = true;
                            }
                            else
                            {
                                if (result[i][1] is string)
                                {
                                    errorMsg += hfEmployeeName + "-" + DateTime.Now.ToShortDateString() + "：" + result[i][1].ToString() + " \n ";
                                }
                                else
                                {
                                    string wfID = sp.RefDocNo;
                                    SapResult sr = (SapResult)result[i][1];
                                    foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
                                    {
                                        errorMsg += hfEmployeeName + "-" + DateTime.Now.ToShortDateString() + "：" + ret.MESSAGE + " \n ";
                                    }
                                }
                            }
                        }
                    }
                    var delegationList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequest");
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", hfCAWorkflowNumber.Value);
                    SPListItemCollection eecListItem = delegationList.GetItems(query);
                    SPListItem eecli = eecListItem[0];

                    var delegationSAPList = CA.SharePoint.SharePointUtil.GetList("CashAdvanceRequestSAP");
                    SPQuery sapquery = new SPQuery();
                    sapquery.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", hfwfID.Value);
                    SPListItemCollection eecsapListItem = delegationSAPList.GetItems(sapquery);
                    SPListItem eecsapli = eecsapListItem[0];

                    string emsg = eecli["ErrorMsg"].AsString();
                    emsg += errorMsg;
                    if (postResult)
                    {
                        eecli["SAPNumber"] = sAPNumber;
                        eecsapli["SAPNumber"] = sAPNumber;
                        eecsapli["SAPStatus"] = "1";
                    }
                    else 
                    {
                        eecli["ErrorMsg"] = emsg;
                        eecsapli["ErrorMsg"] = emsg;
                    }
                    if (eecsapli["PostCount"] == null)
                    {
                        eecsapli["PostCount"] = "1";
                    }
                    else
                    {
                        eecsapli["PostCount"] = (Int32.Parse(eecsapli["PostCount"].ToString()) + 1).ToString();
                    }

                    eecli.Web.AllowUnsafeUpdates = true;
                    eecli.Update();

                    eecsapli.Web.AllowUnsafeUpdates = true;
                    eecsapli.Update();
                }
            }

            this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">window.location = window.location;</script>");
        }
    }
}