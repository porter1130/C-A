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
using System.IO;
using System.Collections;

namespace CA.WorkFlow.UI.PaymentRequestSAP
{
    public partial class DataListSAPView : BaseWorkflowUserControl
    {
        #region Bind SAP Data

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
                LoadExpenseType();
                RepeaterDataBind();
            }
        }

        private void RepeaterDataBind()
        {
            ds = new DataSet();
            SPList list = SPContext.Current.Web.Lists["Payment Request SAP WorkFlow"];
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
                DataTable wfDetailItemsDT = PaymentRequestSAPCommon.GetCollectionByList("Payment Request SAP Items WorkFlow").GetDataTable();
                wfDetailItemsDT.TableName = "DetailsItems";
                ds.Tables.Add(wfItemsDT);
                ds.Tables.Add(wfDetailItemsDT);
                ds.Relations.Add("relation", wfItemsDT.Columns["Title"], wfDetailItemsDT.Columns["Title"], false);
                rptWFItemCollection.DataSource = ds.Tables["ReviewedItems"];
                rptWFItemCollection.DataBind();
            }
        }

        protected void rptWFItemCollection_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SPGridView gridView = (SPGridView)e.Item.FindControl("spgvWFItem");
                BindToSPGridView(gridView, ((DataRowView)e.Item.DataItem).CreateChildView("relation"));
            }
        }

        private void BindToSPGridView(SPGridView gridView, System.Data.DataView childView)
        {
            SPBoundField boundField;
            List<string> fieldsList = new List<string>() { "AssetNo", "ExpenseType", "CostCenter", "ItemAmount", "GLAccount" };
            foreach (string fieldsName in fieldsList)
            {
                boundField = new SPBoundField();
                boundField.HeaderText = fieldsName;
                boundField.DataField = fieldsName;
                gridView.Columns.Add(boundField);
            }
            gridView.DataSource = childView;
            gridView.PagerTemplate = null;
            gridView.DataBind();
        }

        internal Hashtable OriginalExpenseType
        {
            get
            {
                return this.ViewState["OriginalExpenseType"] as Hashtable;
            }
            set
            {
                this.ViewState["OriginalExpenseType"] = value;
            }
        }

        private void LoadExpenseType()
        {
            DataTable table = WorkFlowUtil.GetCollectionByList("Payment Request Expense Type").GetDataTable();
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in table.Rows)
            {
                if (!ht.Contains(dr["ExpenseType"].ToString()))
                {
                    ht.Add(dr["ExpenseType"].ToString(), dr["NewExpenseType"].ToString());
                }
            }
            OriginalExpenseType = ht;
        }

        protected void spgvWFItem_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var row = e.Row.DataItem as DataRowView;
                e.Row.Cells[1].Text = row["ExpenseType"].AsString();
                if (OriginalExpenseType[e.Row.Cells[1].Text] != null)
                {
                    e.Row.Cells[1].Text = OriginalExpenseType[row["ExpenseType"].AsString()].AsString();
                }
            }
        }

        #endregion

        #region Post To SAP

        public void btnClaimRelateToSAP_Click(object sender, EventArgs e)
        {
            //SAP Parameter
            List<SapParameter> sapParametersEC = new List<SapParameter>();

            string workFlowID = this.hidWorkflowID.Value.Trim();
            List<string> listWorkFlowNumber = workFlowID.Split(';').ToList<string>();
            listWorkFlowNumber.Remove("");
            string[] listSAPWorkFlowNumber = this.hidSAPWorkflowID.Value.Trim().Split(';');
            List<string> list = new List<string>();

            //foreach (string workFlowNumber in listWorkFlowNumber)
            //{
            //    string workFlowNumberAndSAPNO = "";

            //    SapParameter sapParameters = GetSapParameterByWorkFlowNumber(workFlowNumber, out workFlowNumberAndSAPNO);
            //    if (sapParameters != null)
            //    {
            //        sapParametersEC.Add(sapParameters);
            //    }
            //    else
            //    {
            //        list.Add(workFlowNumberAndSAPNO);
            //    }


               
            //}

            for (int i = 0; i < listWorkFlowNumber.Count; i++) 
            {
                string workFlowNumberAndSAPNO = "";

                SapParameter sapParameters = GetSapParameterByWorkFlowNumber(listWorkFlowNumber[i], out workFlowNumberAndSAPNO);
                if (sapParameters != null)
                {
                    sapParametersEC.Add(sapParameters);

                    InsertSAPNumberOrErrorMsg(sapParametersEC, listWorkFlowNumber[i], listSAPWorkFlowNumber[i]);

                    sapParametersEC.Remove(sapParameters);
                }
                else
                {
                    list.Add(workFlowNumberAndSAPNO);
                }


                
            }

            //Insert SAPNumber Or ErrMsg To Claim ListItem
            
            
          

            if (list.Count > 0) 
            {
                foreach (string workFlowNumberAndSAPNO in list) 
                {
                    List<string> listWorkFlowNumberAndSAPNO = workFlowNumberAndSAPNO.Split(';').ToList<string>();
                    listWorkFlowNumberAndSAPNO.Remove("");
                    InsertSAPNumber(listWorkFlowNumberAndSAPNO[0], listWorkFlowNumberAndSAPNO[1]);
                }
            }

            this.Page.ClientScript.RegisterStartupScript(typeof(DataListSAPView), "alert", "<script type=\"text/javascript\">window.location = window.location;</script>");
        }

        private SapParameter GetSapParameterByWorkFlowNumber(string workFlowNumber, out string workFlowNumberAndSAPNO)
        {
            DataTable sapDataTable = PaymentRequestSAPCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Payment Request SAP WorkFlow", workFlowNumber).GetDataTable();
            DataTable sapItemDataTable = PaymentRequestSAPCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Payment Request SAP Items WorkFlow", workFlowNumber).GetDataTable();

            DataTable sapDataTable1 = PaymentRequestSAPCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber2("PaymentRequestItems", workFlowNumber).GetDataTable();

            SapParameter mSapParameters = null;
            
            if (null == sapDataTable)
            {
                sapDataTable = new DataTable();
            }
            if (null == sapItemDataTable)
            {
                sapItemDataTable = new DataTable();
            }
            workFlowNumberAndSAPNO = "";
            
            string employeeID = "";
            string employeeName = "";
            bool isFromPO = false;
            string systemPONo = "";
            string paidThisTime = "";
            string vendorNo = "";
            string vendorName = "";
            string pONo = "";

            string vendorCity = "";
            string vendorCountry = "";
            string bankCity = "";
            string swiftCode = "";
            string bankAccount = "";
            string taxPrice = "";
            string bankName = "";
            string paymentDesc = "";

            string currency = "";
            string exchRate = "";

            if (sapDataTable != null && sapDataTable.Rows.Count > 0)
            {
                string name = sapDataTable.Rows[0]["Applicant"].ToString();
                //systemPONo = sapDataTable.Rows[0]["SystemPONo"].ToString();

                systemPONo = sapDataTable1.Rows[0]["SystemPONo"].AsString();

                paidThisTime = sapDataTable.Rows[0]["PaidThisTime"].ToString();
                vendorNo = sapDataTable.Rows[0]["VendorNo"].AsString();
                pONo = sapDataTable.Rows[0]["PONo"].ToString();
                vendorName = sapDataTable.Rows[0]["VendorName"].ToString();
                isFromPO = sapDataTable.Rows[0]["FromPOStatus"].ToString() == "1" ? true : false;
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                employeeID = employee.EmployeeID;
                employeeName = employee.DisplayName;

                vendorCity = sapDataTable.Rows[0]["VendorCity"].AsString();
                vendorCountry = sapDataTable.Rows[0]["VendorCountry"].AsString();
                bankCity = sapDataTable.Rows[0]["BankCity"].AsString();
                swiftCode = sapDataTable.Rows[0]["SwiftCode"].AsString();
                bankAccount = sapDataTable.Rows[0]["BankAccount"].AsString();
                taxPrice = sapDataTable.Rows[0]["TaxPrice"].AsString();
                bankName = sapDataTable.Rows[0]["BankName"].AsString();

                paymentDesc = sapDataTable.Rows[0]["PaymentDesc"].AsString();

                currency = sapDataTable.Rows[0]["Currency"].AsString();
                exchRate = sapDataTable.Rows[0]["ExchRate"].AsString();
                exchRate = exchRate == "" ? "1" : exchRate;
            }
            bool isSystemGR = false;
            System.Text.StringBuilder strLog = new System.Text.StringBuilder();

            if (isFromPO && systemPONo != "")
            {
                DataTable dt = PaymentRequestSAPCommon.GetEmployeeExpenseClaimSAPItemsByPONumber("Purchase Order Workflow", pONo).GetDataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string systemGR = dt.Rows[0]["IsSystemGR"].ToString();
                    string sapNO = dt.Rows[0]["SapNO"].AsString();
                    if (systemGR == "1")
                    {
                        isSystemGR = true;
                        workFlowNumberAndSAPNO += workFlowNumber + ";" + sapNO;
                    }
                }
                //CommonUtil.logError(string.Format("dt.Rows[0][IsSystemGR].ToString(): {0} \n. dt.Rows.Count: {1}\n", dt.Rows[0]["IsSystemGR"].ToString(), dt.Rows.Count));
                if (!isSystemGR)
                {
                    mSapParameters = new SapParameter()
                        {
                            DocDate = DateTime.Now.ToString("yyyyMMdd"),
                            RefDocNo = workFlowNumber, //PO单 POST到SAP时生成的SAP号   
                            Header = workFlowNumber //工作流ID,只要是唯一值就行           
                        };
                    List<StoresReceiveItem> paymentRequest = new List<StoresReceiveItem>();


                    DataTable sapItemDataTable1 = PaymentRequestSAPCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber1("PurchaseOrderItems", pONo)
                                                                          .GetDataTable()
                                                                          .AsEnumerable()
                                                                          .Where(dr => dr.Field<string>("ItemCode").AsString().ToLower().IndexOf("x") != 0)
                                                                          .CopyToDataTable();
                    foreach (DataRow dr in sapItemDataTable1.Rows)
                    {
                        //decimal returnQuantityForSAP = 0;
                        ////(decimal.Parse(dr["TotalQuantity"].ToString()) * Int32.Parse(paidThisTime)) / 100
                        //if (dr["ReturnQuantityForSAP"] == null)
                        //{
                        //    returnQuantityForSAP = 0;
                        //}
                        //else
                        //{
                        //    returnQuantityForSAP = decimal.Parse(dr["ReturnQuantityForSAP"].ToString() == "" ? "0" : dr["ReturnQuantityForSAP"].ToString());
                        //}
                        //if (returnQuantityForSAP < 0)
                        //{
                        //    returnQuantityForSAP = 0 - returnQuantityForSAP;
                        //}
                        //decimal quantity = ((decimal.Parse(dr["TotalQuantity"].ToString()) - returnQuantityForSAP) * decimal.Parse(paidThisTime)) / 100;
                        decimal quantity = (decimal.Parse(dr["TotalQuantity"].ToString()) * decimal.Parse(paidThisTime)) / 100;
                        decimal totalQuantity = Math.Round(quantity, 3);
                        paymentRequest.Add(new StoresReceiveItem()
                        {
                            Quantity = totalQuantity,
                            SapNumber = dr["SapNO"].ToString(),
                            ItemNo = Int32.Parse(dr["ItemNO"].ToString()),
                            ItemText = paymentDesc
                        });
                        strLog.AppendFormat("\r\nworkFlowNumber:{0} \r\n ", workFlowNumber);
                        strLog.AppendFormat("TotalQuantity:{0} \r\n ", dr["TotalQuantity"].ToString());
                        strLog.AppendFormat("ReturnQuantityForSAP:{0} \r\n ", dr["ReturnQuantityForSAP"].ToString());
                        strLog.AppendFormat("totalQuantity:{0} \r\n ", totalQuantity.ToString());
                        strLog.AppendFormat("SapNO:{0} \r\n ", dr["SapNO"].ToString());
                        strLog.AppendFormat("ItemNO:{0} \r\n ", dr["ItemNO"].ToString());
                        strLog.AppendFormat("paidThisTime:{0} \r\n ", paidThisTime);
                        
                    }

                    List<ExpenceDetail> expenceDetailsList1 = new List<ExpenceDetail>();
                    mSapParameters.ExpenceDetails = expenceDetailsList1;

                    mSapParameters.StoresReceiveItems = paymentRequest;
                    WriteErrorLog(strLog);
                    return mSapParameters;
                }
            }
            if (!isFromPO)
            {
                decimal exchrate = 0;
                if (currency == "RMB")
                {
                    exchrate = 1;
                }
                mSapParameters = new SapParameter()
                {
                    BusAct = "RFBU",
                    CompCode = "CA10",
                    DocType = "KR",
                    BusArea = "0001",
                    Currency = currency,
                    EmployeeID = vendorNo == "" ? "0099999999" : vendorNo,
                    EmployeeName = vendorName,
                    ExchRate = exchrate,
                    Header = paymentDesc,
                    RefDocNo = workFlowNumber,
                    UserName = "acnotes",
                    Vendor = vendorNo
                };

                if (vendorNo == "" || vendorNo == "99999999")
                {
                    mSapParameters.VendorInfo = new Vendor()
                    {
                        BankAcct = bankAccount, //"234567893456789345678",
                        BankCity = bankCity,    //"CN",
                        BankNo = swiftCode,     //"104290003033",
                        City = vendorCity,      //"上海",
                        Country = vendorCountry, // "CN",
                        Name = vendorName       //"上海文思创新科技有限公司-上海文思创新科技有限公司"
                    };
                }
                System.Text.StringBuilder detailsList = new System.Text.StringBuilder();
                List<ExpenceDetail> expenceDetailsList = new List<ExpenceDetail>();
                if (sapItemDataTable != null && sapItemDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in sapItemDataTable.Rows)
                    {
                        if (dr["ExpenseType"].ToString().IndexOf("OP-Non-trade vendor") == -1
                            && dr["ExpenseType"].ToString().IndexOf("GRIR vendor code") == -1)
                        {
                            ExpenceDetail expenceDetail = new ExpenceDetail();
                            expenceDetail.AccountGL = dr["GLAccount"].ToString();
                            
                            expenceDetail.Amount = decimal.Parse(dr["ItemAmount"].ToString()) < 0 ? 0 - decimal.Parse(dr["ItemAmount"].ToString()) : decimal.Parse(dr["ItemAmount"].ToString());

                            //if (dr["ExpenseType"].ToString().IndexOf("Accrual") != -1
                            //    || dr["ExpenseType"].ToString().IndexOf("Accrued") != -1
                            //    || dr["ExpenseType"].ToString().IndexOf("Prepaid") != -1)
                            //{
                            //    expenceDetail.CostCenter = "0000000000";
                            //    expenceDetail.BusArea = dr["BusinessArea"].AsString();
                            //}
                            //else 
                            //{
                            //    expenceDetail.CostCenter = dr["CostCenter"].AsString() == "" ? "0000000000" : dr["CostCenter"].AsString();
                            //    expenceDetail.BusArea = "";
                            //}

                            if (dr["ExpenseType"].ToString().ToLower() == ("Tax payable - VAT input").ToLower())
                            {
                                expenceDetail.CostCenter = "0000000000";
                            }
                            else 
                            {
                                if (dr["ExpenseType"].ToString().IndexOf("Accrual") != -1
                                   || dr["ExpenseType"].ToString().IndexOf("Accrued") != -1
                                   || dr["ExpenseType"].ToString().IndexOf("Prepaid") != -1)
                                {
                                    expenceDetail.CostCenter = "";
                                }
                                else 
                                {
                                    expenceDetail.CostCenter = dr["CostCenter"].AsString();
                                }
                            }
                            if (dr["BusinessArea"].AsString() != "")
                            {
                                 expenceDetail.BusArea = dr["BusinessArea"].AsString();
                            }

                            //expenceDetail.ItemText = employeeName + " " + dr["ExpenseType"].ToString();
                            expenceDetail.ItemText = paymentDesc;
                            
                            expenceDetailsList.Add(expenceDetail);

                            detailsList.AppendFormat("\r\nAccountGL:{0} \r\n ", dr["GLAccount"].AsString());
                            detailsList.AppendFormat("\r\nAmount:{0} \r\n ", dr["ItemAmount"].AsString());
                            detailsList.AppendFormat("\r\nExpenseType:{0} \r\n ", OriginalExpenseType[dr["ExpenseType"].AsString()].AsString());
                            detailsList.AppendFormat("\r\nCostCenter:{0} \r\n ", expenceDetail.CostCenter);
                            detailsList.AppendFormat("\r\nBusinessArea:{0} \r\n ", dr["BusinessArea"].AsString());
                        }
                    }
                }
                List<StoresReceiveItem> paymentRequest1 = new List<StoresReceiveItem>();
                mSapParameters.StoresReceiveItems = paymentRequest1;

                mSapParameters.ExpenceDetails = expenceDetailsList;

                strLog.AppendFormat("\r\nvendorNo:{0} \r\n ", vendorNo);
                strLog.AppendFormat("vendorName:{0} \r\n ", vendorName);
                strLog.AppendFormat("workFlowNumber:{0} \r\n ", workFlowNumber);
                strLog.AppendFormat("BankAcct:{0} \r\n ", bankAccount);
                strLog.AppendFormat("BankCity:{0} \r\n ", bankCity);
                strLog.AppendFormat("BankNo:{0} \r\n ", swiftCode);
                strLog.AppendFormat("City:{0} \r\n ", vendorCity);
                strLog.AppendFormat("Country:{0} \r\n ", vendorCountry);
                strLog.AppendFormat("Name:{0} \r\n ", vendorName);
                strLog.Append("\r\n============= Currency、Exchange Rate Info =============\r\n");
                strLog.AppendFormat("Currency:{0} \r\n ", currency);
                strLog.AppendFormat("Exchange Rate:{0} \r\n ", exchrate);
                strLog.Append("\r\n============= Currency、Exchange Rate Info =============\r\n");
                strLog.Append("\r\n============= Expence Details List =============\r\n");
                strLog.Append(detailsList.ToString());
                strLog.Append("\r\n============= Expence Details List =============\r\n");
                WriteErrorLog(strLog);
                return mSapParameters;
            }
            WriteErrorLog("null", "null");
            return mSapParameters;
        }
        private void WriteErrorLog(System.Text.StringBuilder strLog)
        {
            string sErrorFormate = string.Empty;
            try
            {
                string time = DateTime.Now.ToShortDateString();
                string path = Server.MapPath("~/PR_Log");
                time = time.Replace("/", "_").Replace(@"\", "_").Replace("-", "_");
                string fileName = path + @"\" + time + ".txt";
                strLog.AppendFormat("\r\nFileName：{0}", fileName);
                sErrorFormate = string.Format("{0}: {1}\r\n-----------------------------------------------------------\r\n", DateTime.Now.ToString(), strLog.ToString());
                if (!File.Exists(fileName))
                {
                    FileStream fs = File.Create(fileName);
                    fs.Flush();
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(fileName);

                sw.WriteLine(sErrorFormate);
                sw.Flush();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                CommonUtil.logError(sErrorFormate + "\r\n" + ex.Message);
            }
        }

        private void WriteErrorLog(string paidThisTime, string workFlowNumber)
        {
            string sErrorFormate = string.Empty;
            try
            {
                string time = DateTime.Now.ToShortDateString();
                string path = Server.MapPath("~/PR_Log");
                time = time.Replace("/", "_").Replace(@"\", "_").Replace("-", "_");
                string fileName = path + @"\" + time + ".txt";
                sErrorFormate += fileName + "\r\n";
                string log = string.Format("\r\nWorkFlowNumber:{0}\r\nPaidThisTime:{1}", workFlowNumber, paidThisTime);
                sErrorFormate += string.Format("{0}: {1}\r\n-----------------------------------------------------------\r\n", DateTime.Now.ToString(), log);
                if (!File.Exists(fileName))
                {
                    FileStream fs = File.Create(fileName);
                    fs.Flush();
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(fileName);

                sw.WriteLine(sErrorFormate);
                sw.Flush();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                CommonUtil.logError(sErrorFormate + "\r\n" + ex.Message);
            }
        }


        private void InsertSAPNumberOrErrorMsg(List<SapParameter> sapParametersEC, string listWorkFlowNumber, string listSAPWorkFlowNumber)
        {
            List<SapParameter> listStoresReceiveItems = new List<SapParameter>();
            List<SapParameter> listExpenceDetails = new List<SapParameter>();
            foreach (SapParameter sp in sapParametersEC)
            {
              
                if (sp.StoresReceiveItems.Count > 0) 
                {
                    listStoresReceiveItems.Add(sp);
                }
                if (sp.ExpenceDetails.Count > 0)
                {
                    listExpenceDetails.Add(sp);
                }
            }

            if (listStoresReceiveItems.Count > 0)
            {

                //Payment Request          
                ISapExchange sapExchangeStoresReceiveItems = SapExchangeFactory.GetStoresReceive();
                List<object[]> resultStoresReceiveItems = sapExchangeStoresReceiveItems.ImportDataToSap(listStoresReceiveItems);

                if (null == resultStoresReceiveItems)
                {
                    this.Page.ClientScript.RegisterStartupScript(typeof(DataListSAPView), "alert", "<script type=\"text/javascript\">alert('Connection failed !'); window.location = window.location;</script>");
                    return;
                }
                for (int i = 0; i < resultStoresReceiveItems.Count; i++)
                {
                    SapParameter sp = (SapParameter)resultStoresReceiveItems[i][0];
                    bool bl = (bool)resultStoresReceiveItems[i][2];
                    string sAPNumber = "";
                    string errorMsg = "";
                    if (bl)
                    {
                        SapResult sr = (SapResult)resultStoresReceiveItems[i][1];
                        sAPNumber = sr.OBJ_KEY;
                    }
                    else
                    {
                        if (resultStoresReceiveItems[i][1] is string)
                        {
                            errorMsg = resultStoresReceiveItems[i][1].ToString();
                        }
                        else
                        {
                            string wfID = sp.RefDocNo;
                            SapResult sr = (SapResult)resultStoresReceiveItems[i][1];
                            foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
                            {
                                errorMsg += ret.MESSAGE;
                            }
                        }
                    }
                    InsertSAPNumberOrErrorMsg(sAPNumber, errorMsg, listWorkFlowNumber, listSAPWorkFlowNumber);
                }

            }

            if (listExpenceDetails.Count > 0)
            {

                ISapExchange sapExchange = SapExchangeFactory.GetEmployeeClaim();
                List<object[]> result = sapExchange.ImportDataToSap(listExpenceDetails);

                if (null == result)
                {
                    this.Page.ClientScript.RegisterStartupScript(typeof(DataListSAPView), "alert", "<script type=\"text/javascript\">alert('Connection failed !'); window.location = window.location;</script>");
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
                    InsertSAPNumberOrErrorMsg(sAPNumber, errorMsg, listWorkFlowNumber, listSAPWorkFlowNumber);
                }
            }
        }

        private void InsertSAPNumberOrErrorMsg(string sAPNumber, string errorMsg, string workFlowNumber, string sapWorkFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("PaymentRequestItems");
            var delegationSAPList = CA.SharePoint.SharePointUtil.GetList("Payment Request SAP WorkFlow");
            SPQuery query = new SPQuery();
            SPQuery querySAP = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='SubPRNo' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            querySAP.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", sapWorkFlowNumber);
            SPListItemCollection eecListItem = delegationList.GetItems(query);
            SPListItemCollection eecSAPListItem = delegationSAPList.GetItems(querySAP);
            SPListItem eecli = eecListItem[0];
            SPListItem eecSAPli = eecSAPListItem[0];

            string emsg = eecli["ErrorMsg"].AsString();

            eecli["SAPNumber"] = sAPNumber;
            if (errorMsg != "")
            {
                emsg += eecli["Applicant"].ToString() + "-" + DateTime.Now.ToShortDateString() + "：" + errorMsg + " \n ";
            }
            eecli["ErrorMsg"] = emsg;

            bool isFromPO = bool.Parse(eecli["IsFromPO"].ToString());
            if (isFromPO && sAPNumber != "")
            {
                eecli["IsSystemGR"] = true;
            }

            eecli.Web.AllowUnsafeUpdates = true;
            eecli.Update();

            eecSAPli["SAPNumber"] = sAPNumber;
            eecSAPli["ErrorMsg"] = emsg;
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
                eecSAPli["PostSAPStatus"] = "1";
            }
            eecSAPli.Web.AllowUnsafeUpdates = true;
            eecSAPli.Update();
        }

        private void InsertSAPNumber(string workFlowNumber, string sAPNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("PaymentRequestItems");
            var delegationSAPList = CA.SharePoint.SharePointUtil.GetList("Payment Request SAP WorkFlow");
            SPQuery query = new SPQuery();
            SPQuery querySAP = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='SubPRNo' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            querySAP.Query = string.Format("<Where><Eq><FieldRef Name='PRWorkflowNumber' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            SPListItemCollection eecListItem = delegationList.GetItems(query);
            SPListItemCollection eecSAPListItem = delegationSAPList.GetItems(querySAP);
            SPListItem eecli = eecListItem[0];
            SPListItem eecSAPli = eecSAPListItem[0];

            eecli["SAPNumber"] = sAPNumber;

            eecli.Web.AllowUnsafeUpdates = true;
            eecli.Update();

            eecSAPli["SAPNumber"] = sAPNumber;

            if (eecSAPli["PostCount"] == null)
            {
                eecSAPli["PostCount"] = "1";
            }
            else
            {
                eecSAPli["PostCount"] = (Int32.Parse(eecSAPli["PostCount"].ToString()) + 1).ToString();
            }

            eecSAPli["PostSAPStatus"] = "1";

            eecSAPli.Web.AllowUnsafeUpdates = true;
            eecSAPli.Update();
        }

        #endregion


    }
}