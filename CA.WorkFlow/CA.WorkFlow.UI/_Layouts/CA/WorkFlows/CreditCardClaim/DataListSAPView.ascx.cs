using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using SAP.Middleware.Exchange;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class DataListSAPView : QFUserControl
    {
        #region  Bind SAP Data

        protected DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.AddHeader("pragma", "no-cache");
            Response.CacheControl = "no-cache";
            if (!IsPostBack)
            {
                RepeaterDataBind();
            }
        }

        private void RepeaterDataBind()
        {
            ds = new DataSet();
            SPList list = SPContext.Current.Web.Lists["Credit Card Claim SAP Workflow"];
            SPQuery query = new SPQuery();
            query.Query = @"<Where>
                                <And>
                                      <Eq>
                                         <FieldRef Name='Status' />
                                         <Value Type='Text'>Completed</Value>
                                      </Eq>
                                      <Eq>
                                        <FieldRef Name='PostSAPStatus' />
                                        <Value Type='Text'>0</Value>
                                      </Eq>
                                </And>
                           </Where>";
            SPListItemCollection items = list.GetItems(query);
            if (items.Count > 0)
            {
                DataTable wfItemsDT = items.GetDataTable();
                wfItemsDT.TableName = "ReviewedItems";
                DataTable wfDetailItemsDT = CreditCardClaimCommon.GetCollectionByList("Credit Card Claim SAP Detail").GetDataTable();
                wfDetailItemsDT.TableName = "DetailsItems";
                ds.Tables.Add(wfItemsDT);
                ds.Tables.Add(wfDetailItemsDT);
                ds.Relations.Add("relation",
                                 wfItemsDT.Columns["Title"],
                                 wfDetailItemsDT.Columns["Title"], false);
                rptWFItemCollection.DataSource = ds.Tables["ReviewedItems"];
                rptWFItemCollection.DataBind();
            }
        }

        protected void rptWFItemCollection_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SPGridView gridView = (SPGridView)e.Item.FindControl("spgvWFItem");
                BindToSPGridView(gridView, ((DataRowView)e.Item.DataItem).CreateChildView("relation"));
            }
        }

        private void BindToSPGridView(SPGridView gridView, System.Data.DataView childView)
        {
            SPBoundField boundField;
            List<string> fieldsList = new List<string>() {
                                                            "ExpenseType",
                                                           "CostCenter",
                                                           "DealAmount",
                                                           "AmountType",
                                                           "GLAccount"};

            foreach (string fieldsName in fieldsList)
            {
                boundField = new SPBoundField();
                string headerText = fieldsName;
                //if (fieldsName == "CCCWWorkflowNumber")
                //{
                //    headerText = "WorkflowNumber";
                //}
                if (fieldsName == "DealAmount")
                {
                    headerText = "Amount";
                }
                if (fieldsName == "AmountType")
                {
                    headerText = "Currency";
                }
                boundField.HeaderText = headerText;
                boundField.DataField = fieldsName;
                gridView.Columns.Add(boundField);
            }
            gridView.DataSource = childView;
            gridView.PagerTemplate = null;
            gridView.DataBind();
        }

        #endregion

        public void btnClaimRelateToSAP_Click(object sender, EventArgs e)
        {
            //SAP Parameter
            List<SapParameter> sapRMBParametersEC = new List<SapParameter>();
            List<SapParameter> sapUSDParametersEC = new List<SapParameter>();

            string workFlowID = this.hidWorkflowID.Value.Trim();
            List<string> listWorkFlowNumber = workFlowID.Split(';').ToList<string>();
            listWorkFlowNumber.Remove("");

            string[] listSAPWorkFlowNumber = this.hidSAPWorkflowID.Value.Trim().Split(';');

            //foreach (string workFlowNumber in listWorkFlowNumber)
            //{
            //    SapParameter sapRMBParameters = GetRMBSapParameterByWorkFlowNumber(workFlowNumber);
            //    SapParameter sapUSDParameters = GetUSDSapParameterByWorkFlowNumber(workFlowNumber);
            //    if (null != sapRMBParameters)
            //    {
            //        sapRMBParametersEC.Add(sapRMBParameters);
            //    }
            //    if (null != sapUSDParameters)
            //    {
            //        sapUSDParametersEC.Add(sapUSDParameters);
            //    }
            //}

            for (int i = 0; i < listWorkFlowNumber.Count; i++) 
            {
                SapParameter sapRMBParameters = GetRMBSapParameterByWorkFlowNumber(listWorkFlowNumber[i]);
                SapParameter sapUSDParameters = GetUSDSapParameterByWorkFlowNumber(listWorkFlowNumber[i]);
                if (null != sapRMBParameters)
                {
                    sapRMBParametersEC.Add(sapRMBParameters);
                    InsertSAPNumberOrErrorMsg(sapRMBParametersEC, listWorkFlowNumber[i], listSAPWorkFlowNumber[i], "RMB");
                    sapRMBParametersEC.Remove(sapRMBParameters);
                }
                if (null != sapUSDParameters)
                {
                    sapUSDParametersEC.Add(sapUSDParameters);
                    InsertSAPNumberOrErrorMsg(sapUSDParametersEC, listWorkFlowNumber[i], listSAPWorkFlowNumber[i], "USD");
                    sapUSDParametersEC.Remove(sapUSDParameters);
                }
                
                
            }

            //Insert SAPNumber Or ErrMsg To Claim ListItem
          
            
           
            this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">window.location = window.location;</script>");
        }

        private SapParameter GetRMBSapParameterByWorkFlowNumber(string workFlowNumber)
        {
            SapParameter sapParameters = null;

            DataTable sapDataTable = CreditCardClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Credit Card Claim SAP Workflow", workFlowNumber).GetDataTable();
            DataTable sapItemDataTable = CreditCardClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Credit Card Claim SAP Detail", workFlowNumber, "RMB").GetDataTable();
            DataTable sapUSDItemDataTable = CreditCardClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Credit Card Claim SAP Detail", workFlowNumber, "USD").GetDataTable();
            if (null == sapDataTable)
            {
                sapDataTable = new DataTable();
            }
            if (null == sapItemDataTable)
            {
                sapItemDataTable = new DataTable();
            }
            if (null == sapUSDItemDataTable)
            {
                sapUSDItemDataTable = new DataTable();
            }

            string employeeID = "";
            string employeeName = "";
            string expenseDescription = "";

            if (sapDataTable != null && sapDataTable.Rows.Count > 0)
            {
                expenseDescription = sapDataTable.Rows[0]["ExpenseDescription"].ToString();
                string name = sapDataTable.Rows[0]["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                employeeID = employee.EmployeeID;
                employeeName = employee.DisplayName;
                if (sapItemDataTable != null && sapItemDataTable.Rows.Count > 0)
                {
                    if (sapItemDataTable.Rows[0]["Status"].ToString() == "success")
                    {
                        return null;
                    }
                }
            }
           
            if (sapUSDItemDataTable != null && sapUSDItemDataTable.Rows.Count > 0
               && sapItemDataTable != null && sapItemDataTable.Rows.Count > 0)
            {
                workFlowNumber = workFlowNumber + "_1";
            }

            //RMB sapParameters
            if (sapItemDataTable != null && sapItemDataTable.Rows.Count > 0)
            {
                sapParameters = new SapParameter()
                {
                    BusAct = "RFBU",
                    CompCode = "CA10",
                    DocType = "KR",
                    BusArea = "0001",
                    Currency = "RMB",
                    EmployeeID = employeeID,
                    EmployeeName = employeeName,
                    ExchRate = 1,
                    Header = expenseDescription,
                    RefDocNo = workFlowNumber,
                    UserName = "acnotes"
                };

                List<ExpenceDetail> expenceDetailsList = new List<ExpenceDetail>();
                foreach (DataRow dr in sapItemDataTable.Rows)
                {
                    if (dr["ExpenseType"].ToString().IndexOf("OR - employee vendor") == -1)
                    {
                        ExpenceDetail expenceDetail = new ExpenceDetail();
                        expenceDetail.AccountGL = dr["GLAccount"].ToString();
                        expenceDetail.Amount = decimal.Parse(dr["DealAmount"].ToString());
                        expenceDetail.CostCenter = dr["CostCenter"].ToString();
                        expenceDetail.ItemText = employeeName + " " + dr["ExpenseType"].ToString();
                        expenceDetail.RefKey = dr["CreditCardBillID"].AsString();
                        expenceDetail.Currency = "RMB";
                        expenceDetail.ExchRate = 1;
                        expenceDetailsList.Add(expenceDetail);
                    }
                }

                sapParameters.ExpenceDetails = expenceDetailsList;
            }

            return sapParameters;
        }

        private SapParameter GetUSDSapParameterByWorkFlowNumber(string workFlowNumber)
        {
            SapParameter sapUSDParameters = null;

            DataTable sapDataTable = CreditCardClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Credit Card Claim SAP Workflow", workFlowNumber).GetDataTable();
            DataTable sapItemDataTable = CreditCardClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Credit Card Claim SAP Detail", workFlowNumber, "RMB").GetDataTable();
            DataTable sapUSDItemDataTable = CreditCardClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Credit Card Claim SAP Detail", workFlowNumber, "USD").GetDataTable();
            if (null == sapDataTable)
            {
                sapDataTable = new DataTable();
            }
            if (null == sapItemDataTable)
            {
                sapItemDataTable = new DataTable();
            }
            if (null == sapUSDItemDataTable)
            {
                sapUSDItemDataTable = new DataTable();
            }

            string employeeID = "";
            string employeeName = "";
            string expenseDescription = "";

            if (sapDataTable != null && sapDataTable.Rows.Count > 0)
            {
                expenseDescription = sapDataTable.Rows[0]["ExpenseDescription"].ToString();
                string name = sapDataTable.Rows[0]["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                employeeID = employee.EmployeeID;
                employeeName = employee.DisplayName;
                if (sapUSDItemDataTable != null && sapUSDItemDataTable.Rows.Count > 0)
                {
                    if (sapUSDItemDataTable.Rows[0]["Status"].ToString() == "success")
                    {
                        return null;
                    }
                }
            }

            if (sapUSDItemDataTable != null && sapUSDItemDataTable.Rows.Count > 0
                && sapItemDataTable != null && sapItemDataTable.Rows.Count > 0)
            {
                workFlowNumber = workFlowNumber + "_2";
            }

            //USD sapParameters
            if (sapUSDItemDataTable != null && sapUSDItemDataTable.Rows.Count > 0)
            {
                sapUSDParameters = new SapParameter()
                {
                    BusAct = "RFBU",
                    CompCode = "CA10",
                    DocType = "KR",
                    BusArea = "0001",
                    Currency = "USD",
                    EmployeeID = employeeID,
                    EmployeeName = employeeName,
                    ExchRate = 0,
                    Header = expenseDescription,
                    RefDocNo = workFlowNumber,
                    UserName = "acnotes"
                };

                List<ExpenceDetail> expenceUSDDetailsList = new List<ExpenceDetail>();
                foreach (DataRow dr in sapUSDItemDataTable.Rows)
                {
                    if (dr["ExpenseType"].ToString().IndexOf("OR - employee vendor") == -1)
                    {
                        ExpenceDetail expenceDetail = new ExpenceDetail();
                        expenceDetail.AccountGL = dr["GLAccount"].ToString();
                        expenceDetail.Amount = decimal.Parse(dr["DealAmount"].ToString());
                        expenceDetail.CostCenter = dr["CostCenter"].ToString();
                        expenceDetail.ItemText = employeeName + " " + dr["ExpenseType"].ToString();
                        expenceDetail.RefKey = dr["CreditCardBillID"].AsString();
                        expenceDetail.Currency = "USD";
                        expenceDetail.ExchRate = 0;
                        expenceUSDDetailsList.Add(expenceDetail);
                    }
                }

                sapUSDParameters.ExpenceDetails = expenceUSDDetailsList;
            }

            return sapUSDParameters;
        }

        private void InsertSAPNumberOrErrorMsg(List<SapParameter> sapParametersEC, string listWorkFlowNumber, string listSAPWorkFlowNumber, string type)
        {
            ISapExchange sapExchange = SapExchangeFactory.GetEmployeeCCClaim();
            List<object[]> result = sapExchange.ImportDataToSap(sapParametersEC);
            if (null == result)
            {
                this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Connection failed !'); window.location = window.location;</script>");
                return;
            }
            for (int i = 0; i < result.Count; i++)
            {
                SapParameter sp = (SapParameter)result[i][0];
                bool bl = (bool)result[i][2];
                string sAPNumber = "";
                string errorMsg = "";
                if (bl)
                {
                    SapResult sr = (SapResult)result[i][1];
                    sAPNumber = sr.OBJ_KEY;
                }
                else
                {
                    if (result[i][1] is string)
                    {
                        errorMsg = result[i][1].ToString();
                    }
                    else
                    {
                        string wfID = sp.RefDocNo;
                        SapResult sr = (SapResult)result[i][1];
                        foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
                        {
                            errorMsg += ret.MESSAGE;
                        }
                    }
                }
                InsertSAPNumberOrErrorMsg(sAPNumber, errorMsg, listWorkFlowNumber, listSAPWorkFlowNumber, type);
            }
        }

        private void InsertSAPNumberOrErrorMsg(string sAPNumber, string errorMsg, string workFlowNumber, string sapWorkFlowNumber, string type)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim Workflow");
            var delegationSAPList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim SAP Workflow");
            var delegationSAPDetailList = CA.SharePoint.SharePointUtil.GetList("Credit Card Claim SAP Detail");
            SPQuery query = new SPQuery();
            SPQuery querySAP = new SPQuery();
            SPQuery querySAPDetail = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            querySAP.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", sapWorkFlowNumber);
            querySAPDetail.Query = string.Format("<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='AmountType' /><Value Type='Text'>{1}</Value></Eq></And></Where>", sapWorkFlowNumber, type);
            SPListItemCollection eecListItem = delegationList.GetItems(query);
            SPListItemCollection eecSAPListItem = delegationSAPList.GetItems(querySAP);
            SPListItemCollection eecSAPDetailListItem = delegationSAPDetailList.GetItems(querySAPDetail);
            SPListItem eecli = eecListItem[0];
            SPListItem eecSAPli = eecSAPListItem[0];

            string emsg = eecli["ErrorMsg"].AsString();
            string eusdmsg = eecli["ErrorUSDMsg"].AsString();

            if (type == "RMB")
            {
                eecli["SAPNo"] = sAPNumber;
                if (errorMsg != "")
                {
                    emsg += eecli["Applicant"].ToString() + "-" + DateTime.Now.ToShortDateString() + "：" + errorMsg + " \n ";
                }
            }
            if (type == "USD")
            {
                eecli["SAPUSDNo"] = sAPNumber;
                if (errorMsg != "")
                {
                    eusdmsg += eecli["Applicant"].ToString() + "-" + DateTime.Now.ToShortDateString() + "：" + errorMsg + " \n ";
                }
            }
            eecli["ErrorMsg"] = emsg;
            eecli["ErrorUSDMsg"] = eusdmsg;

            eecli.Web.AllowUnsafeUpdates = true;
            eecli.Update();
                        
            if (type == "RMB")
            {
                eecSAPli["SAPNo"] = sAPNumber;
            }
            if (type == "USD")
            {
                eecSAPli["SAPUSDNo"] = sAPNumber;
            }
            eecSAPli["ErrorMsg"] = emsg;
            eecSAPli["ErrorUSDMsg"] = eusdmsg;

            if (eecSAPli["PostCount"] == null)
            {
                eecSAPli["PostCount"] = "1";
            }
            else
            {
                eecSAPli["PostCount"] = (Int32.Parse(eecSAPli["PostCount"].ToString()) + 1).ToString();
            }

            if (errorMsg == "" && sAPNumber != "") 
            {
                if (eecSAPDetailListItem.Count>0)
                {
                    foreach (SPListItem spi in eecSAPDetailListItem) 
                    {
                        SPListItem spli = spi;
                        spli["Status"] = "success";
                        spli.Web.AllowUnsafeUpdates = true;
                        spli.Update();
                    }
                }
            }

            if (errorMsg == "" && sAPNumber != "")
            {
                if (eecSAPli["PostSAPType"].ToString() == "RMB;USD;") 
                {
                    if (eecSAPli["SAPNo"].AsString() != "" && eecSAPli["SAPUSDNo"].AsString() != "")
                    {
                        eecSAPli["PostSAPStatus"] = "1";
                    }
                }

                if (eecSAPli["PostSAPType"].ToString() == "RMB;")
                {
                    if (eecSAPli["SAPNo"].AsString() != "")
                    {
                        eecSAPli["PostSAPStatus"] = "1";
                    }
                }

                if (eecSAPli["PostSAPType"].ToString() == "USD;")
                {
                    if (eecSAPli["SAPUSDNo"].AsString() != "")
                    {
                        eecSAPli["PostSAPStatus"] = "1";
                    }
                }
                
            }

            eecSAPli.Web.AllowUnsafeUpdates = true;
            eecSAPli.Update();
        }

    }
}