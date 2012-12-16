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
using System.Text;
using System.IO;

namespace CA.WorkFlow.UI.EBC
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
                RepeaterDataBind();
            }
        }

        private void RepeaterDataBind()
        {
            ds = new DataSet();
            SPList list = SPContext.Current.Web.Lists["Expatriate Benefit Claim SAP Workflow"];
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
                DataTable wfDetailItemsDT = ExpatriateBenefitClaimCommon.GetCollectionByList("ExpatriateBenefitClaimSAPItems").GetDataTable();
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
            List<string> fieldsList = new List<string>() { "ExpenseType", "CostCenter", "ItemAmount", "GLAccount" };
            foreach (string fieldsName in fieldsList)
            {
                boundField = new SPBoundField();
                string headerText = fieldsName;
                //if (fieldsName == "EECWWorkflowNumber")
                //{
                //    headerText = "WorkflowNumber";
                //}
                if (fieldsName == "ItemAmount")
                {
                    headerText = "Amount";
                }

                boundField.HeaderText = headerText;
                boundField.DataField = fieldsName;
                gridView.Columns.Add(boundField);
            }
            //HiddenField hidWorkflowID = (HiddenField)gridView.Parent.FindControl("hidWorkflowID");
            //gridView.NamingContainer.Controls.Add(hidWorkflowID);
            gridView.DataSource = childView;
            gridView.PagerTemplate = null;
            gridView.DataBind();
        }
        #endregion

        public void btnClaimRelateToSAP_Click(object sender, EventArgs e)
        {
            //SAP Parameter
            List<SapParameter> sapParametersEC = new List<SapParameter>();

            string workFlowID = this.hidWorkflowID.Value.Trim();
            List<string> listWorkFlowNumber = workFlowID.Split(';').ToList<string>();
            listWorkFlowNumber.Remove("");
            string[] listSAPWorkFlowNumber = this.hidSAPWorkflowID.Value.Trim().Split(';');
            //foreach (string workFlowNumber in listWorkFlowNumber)
            //{
            //    SapParameter sapParameters = GetSapParameterByWorkFlowNumber(workFlowNumber);
            //    sapParametersEC.Add(sapParameters);
            //}

            for (int i = 0; i < listWorkFlowNumber.Count; i++)
            {
                SapParameter sapParameters = GetSapParameterByWorkFlowNumber(listWorkFlowNumber[i]);
                sapParametersEC.Add(sapParameters);
                InsertSAPNumberOrErrorMsg(sapParametersEC, listWorkFlowNumber[i], listSAPWorkFlowNumber[i]);
                sapParametersEC.Remove(sapParameters);
            }

            //Insert SAPNumber Or ErrMsg To Claim ListItem

            //InsertSAPNumberOrErrorMsg(sapParametersEC,listWorkFlowNumber, listSAPWorkFlowNumber);
            Response.Write("<script type=\"text/javascript\">window.location = window.location;</script>");
        }

        private SapParameter GetSapParameterByWorkFlowNumber(string workFlowNumber)
        {
            DataTable sapDataTable = ExpatriateBenefitClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("Expatriate Benefit Claim SAP Workflow", workFlowNumber).GetDataTable();
            DataTable sapItemDataTable = ExpatriateBenefitClaimCommon.GetEmployeeExpenseClaimSAPItemsByWorkFlowNumber("ExpatriateBenefitClaimSAPItems", workFlowNumber).GetDataTable();
            if (null == sapDataTable)
            {
                sapDataTable = new DataTable();
            }
            if (null == sapItemDataTable)
            {
                sapItemDataTable = new DataTable();
            }

            string cashAdvanceWorkFlowNumber = "";
            string employeeID = "";
            string employeeName = "";
            string expenseDescription = "";
            StringBuilder logStr = new StringBuilder();

            if (sapDataTable != null && sapDataTable.Rows.Count > 0)
            {
                expenseDescription = sapDataTable.Rows[0]["ExpenseDescription"].ToString();
                cashAdvanceWorkFlowNumber = sapDataTable.Rows[0]["CashAdvanceWorkFlowNumber"].ToString();
                string name = sapDataTable.Rows[0]["Applicant"].ToString();
                Employee employee = UserProfileUtil.GetEmployee(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1));
                employeeID = employee.EmployeeID;
                employeeName = employee.DisplayName;

            }

            CashAdvance[] CashAdvances = new CashAdvance[] { };
            if (cashAdvanceWorkFlowNumber != "")
            {
                List<string> list = cashAdvanceWorkFlowNumber.Split(';').ToList<string>();
                list.Remove("");
                CashAdvances = new CashAdvance[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    string caWorkFlowNumber = list[i].Substring(0, list[i].IndexOf("-"));
                    string caAmount = list[i].Substring(list[i].IndexOf("-") + 1);
                    CashAdvance cashAdvance = new CashAdvance();
                    cashAdvance.ID = caWorkFlowNumber;
                    cashAdvance.CashAmount = Int32.Parse(caAmount);
                    CashAdvances[i] = cashAdvance;
                    logStr.AppendFormat("CashAdvance：{0} cashAdvance.ID：{1} cashAdvance.CashAmount：{2}\r\n", i, caWorkFlowNumber, Int32.Parse(caAmount));
                }
            }

            SapParameter sapParameters = new SapParameter()
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
                UserName = "acnotes",
                CashAdvances = CashAdvances
            };
            logStr.AppendFormat("WorkFlowNumber：{0}\r\nEmployeeID：{1}\r\nmployeeName：{2}\r\n", workFlowNumber, employeeID, employeeName);

            List<ExpenceDetail> expenceDetailsList = new List<ExpenceDetail>();
            if (sapItemDataTable != null && sapItemDataTable.Rows.Count > 0)
            {
                logStr.AppendFormat("====================Expence Details====================\r\n");
                foreach (DataRow dr in sapItemDataTable.Rows)
                {
                    if (dr["ExpenseType"].ToString().IndexOf("OR - employee vendor") == -1 && dr["ExpenseType"].ToString().IndexOf("OR - cash advance") == -1)
                    {
                        ExpenceDetail expenceDetail = new ExpenceDetail();
                        expenceDetail.AccountGL = dr["GLAccount"].ToString();
                        expenceDetail.Amount = decimal.Parse(dr["ItemAmount"].ToString()) < 0 ? 0 - decimal.Parse(dr["ItemAmount"].ToString()) : decimal.Parse(dr["ItemAmount"].ToString());
                        expenceDetail.CostCenter = dr["CostCenter"].ToString();
                        expenceDetail.ItemText = employeeName + " " + dr["ExpenseType"].ToString();
                        expenceDetailsList.Add(expenceDetail);
                        logStr.AppendFormat("AccountGL：{0} Amount：{1} CostCenter：{2} ItemText：{3}\r\n",
                                            expenceDetail.AccountGL,
                                            expenceDetail.Amount,
                                            expenceDetail.CostCenter,
                                            expenceDetail.ItemText);
                    }
                }
                logStr.AppendFormat("====================Expence Details====================\r\n");
            }
            WriteErrorLog(logStr);
            sapParameters.ExpenceDetails = expenceDetailsList;

            return sapParameters;
        }

        private void WriteErrorLog(StringBuilder strLog)
        {
            string sErrorFormate = string.Empty;
            try
            {
                string time = DateTime.Now.ToShortDateString();
                string path = Server.MapPath("~/EBC_Log");
                time = time.Replace("/", "_").Replace(@"\", "_").Replace("-", "_");
                string fileName = path + @"\" + time + ".txt";
                strLog.AppendFormat("\r\nFileName：{0}", fileName);
                sErrorFormate = string.Format("{0}\r\n{1}\r\n-----------------------------------------------------------\r\n", DateTime.Now.ToString(), strLog.ToString());
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
            ISapExchange sapExchange = SapExchangeFactory.GetEmployeeClaim();
            List<object[]> result = sapExchange.ImportDataToSap(sapParametersEC);
            if (null == result)
            {
                Response.Write("<script type=\"text/javascript\">alert('Connection failed !');window.location = window.location;</script>");
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

        private void InsertSAPNumberOrErrorMsg(string sAPNumber, string errorMsg, string workFlowNumber, string sapWorkFlowNumber)
        {
            var delegationList = CA.SharePoint.SharePointUtil.GetList("Expatriate Benefit Claim Workflow");
            var delegationSAPList = CA.SharePoint.SharePointUtil.GetList("Expatriate Benefit Claim SAP Workflow");
            SPQuery query = new SPQuery();
            SPQuery querySAP = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", workFlowNumber);
            querySAP.Query = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", sapWorkFlowNumber);
            SPListItemCollection eecListItem = delegationList.GetItems(query);
            SPListItemCollection eecSAPListItem = delegationSAPList.GetItems(querySAP);
            SPListItem eecli = eecListItem[0];
            SPListItem eecSAPli = eecSAPListItem[0];

            string emsg = eecli["ErrorMsg"].AsString();

            eecli["SAPNo"] = sAPNumber;
            if (errorMsg != "")
            {
                emsg += eecli["Applicant"].ToString() + "-" + DateTime.Now.ToShortDateString() + "：" + errorMsg + " \n ";
            }
            eecli["ErrorMsg"] = emsg;
            eecli.Web.AllowUnsafeUpdates = true;
            eecli.Update();

            eecSAPli["SAPNo"] = sAPNumber;
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

    }
}