using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using QuickFlow.Core;
using SAP.Middleware.Exchange;
using System.Collections;
using System.Reflection;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    public partial class DataSummary : BaseWorkflowUserControl
    {

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

            SPList list = SPContext.Current.Web.Lists[WorkflowListName.TravelExpenseClaimForSAP];

            SPQuery query = new SPQuery();

            string queryFormat = @" <Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='Status' />
                                            <Value Type='Text'>{0}</Value>
                                         </Eq>
                                         <IsNull>
                                            <FieldRef Name='SAPNo' />
                                         </IsNull>
                                      </And>
                                   </Where>";
            query.Query = string.Format(queryFormat, "Completed");

            SPListItemCollection items = list.GetItems(query);

            if (items.Count > 0)
            {

                DataTable wfItemsDT = items.GetDataTable();
                wfItemsDT.TableName = "ReviewedItems";

                DataTable wfDetailItemsDT = SPContext.Current.Web.Lists[WorkflowListName.TravelExpenseClaimDetailsForSAP].Items.GetDataTable();
                wfDetailItemsDT.TableName = "DetailsItems";

                //GetPostItemsCount(wfItemsDT, wfDetailItemsDT);

                ds.Tables.Add(wfItemsDT);
                ds.Tables.Add(wfDetailItemsDT);
                ds.Relations.Add("relation",
                                 wfItemsDT.Columns["Title"],
                                 wfDetailItemsDT.Columns["Title"], false);
                rptWFItemCollection.DataSource = ds.Tables["ReviewedItems"];
                rptWFItemCollection.DataBind();

            }

        }

        private void GetPostItemsCount(DataTable wfItemsDT, DataTable wfDetailItemsDT)
        {
            wfItemsDT.Columns.Add("PostItemsCount");

            foreach (DataRow dr in wfItemsDT.Rows)
            {
                DataTable dt = TravelExpenseClaimForSAPCommon.GetDataSource(wfDetailItemsDT, "Title='" + dr["Title"].AsString() + "'");
                dr["PostItemsCount"] = dt.Rows.Count;
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

            List<string> fieldsList = new List<string>() {"ExpenseType",
                                                           "GLAccount",
                                                           "CostCenter",
                                                            "ApprovedRmbAmt"
                                                         };

            foreach (string fieldsName in fieldsList)
            {
                boundField = new SPBoundField();
                boundField.HeaderText = fieldsName;
                boundField.DataField = fieldsName;
                gridView.Columns.Add(boundField);
            }


            HiddenField hidWorkflowID = (HiddenField)gridView.Parent.FindControl("hidWorkflowID");
            //gridView.NamingContainer.Controls.Add(hidWorkflowID);

            gridView.DataSource = childView;
            gridView.PagerTemplate = null;
            gridView.DataBind();
        }


        internal void PostToSAP()
        {
            foreach (RepeaterItem item in this.rptWFItemCollection.Items)
            {
                CheckBox cbWorkflowItem = item.FindControl("cbWorkflowItem") as CheckBox;

                if (cbWorkflowItem.Checked)
                {
                    HiddenField hidWorkflowID = item.FindControl("hidWorkflowID") as HiddenField;

                    PostDetailsToSAP(hidWorkflowID.Value, WorkflowListName.TravelExpenseClaimDetailsForSAP);
                }

            }

            this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">window.location = window.location;</script>");
        }

        private void PostDetailsToSAP(string id, string listName)
        {
            DataTable dt = TravelExpenseClaimForSAPCommon.GetDataTable(id, listName);

            string tcWorkflowId = TravelExpenseClaimForSAPCommon.GetDataCollection(id, WorkflowListName.TravelExpenseClaimForSAP)[0]["TCWorkflowNumber"].AsString();


            if (dt != null)
            {
                DataTable expenseTypeDT = ConvertToSAPDataSource("GroupExpenceDetails", TravelExpenseClaimForSAPCommon.GetDataSource(dt, "CostCenter<>''"));
                DataTable originalDetailsDT = ConvertToSAPDataSource("ExpenceDetails", TravelExpenseClaimForSAPCommon.GetDataTable(tcWorkflowId, WorkflowListName.TravelExpenseClaimDetails));
                SPListItem item = TravelExpenseClaimForSAPCommon.GetDataCollection(tcWorkflowId, WorkflowListName.TravelExpenseClaim)[0];
                Employee employee = UserProfileUtil.GetEmployee(item["ApplicantSPUser"].AsString().Split(new string[] { ";#" }, StringSplitOptions.None)[1]);

                Hashtable parametersHash = new Hashtable();
                parametersHash.Add("RefDocNo", tcWorkflowId);
                parametersHash.Add("RefDocNo1", item["TRWorkflowNumber"].AsString());
                parametersHash.Add("EmployeeID", employee.EmployeeID);
                parametersHash.Add("EmployeeName", employee.DisplayName);
                parametersHash.Add("CashAmount", decimal.Parse(item["CashAdvanced"].AsString()));
                parametersHash.Add("Header", item["Purpose"].AsString());


                List<SapParameter> mSapParametersTR = InitializeSapParameter(originalDetailsDT, expenseTypeDT, parametersHash);
                #region return results

                ISapExchange tec1 = SapExchangeFactory.GetTravelClaim();
                List<object[]> result = tec1.ImportDataToSap(mSapParametersTR);
                for (int i = 0; i < result.Count; i++)
                {
                    bool bl = (bool)result[i][2];
                    SapParameter sp = (SapParameter)result[i][0];
                    SPListItem sapWFItem = TravelExpenseClaimForSAPCommon.GetDataCollection(id, WorkflowListName.TravelExpenseClaimForSAP)[0];
                    int postCount = 0;
                    sapWFItem["PostCount"] = (int.TryParse(sapWFItem["PostCount"].AsString(), out postCount) ? postCount : 0) + 1;

                    if (bl)
                    {
                        SapResult sr = (SapResult)result[i][1];
                        sapWFItem["SAPNo"] = sr.OBJ_KEY;

                    }
                    else
                    {

                        string errorFormat = "{0} {1}({2}) Post fails:\n";
                        sapWFItem["ErrorMsg"] += string.Format(errorFormat, DateTime.Now.ToShortTimeString(), sp.EmployeeName, sp.EmployeeID);
                        if (result[i][1] is string)
                        {
                            sapWFItem["ErrorMsg"] += result[i][1].ToString() + "\n";

                        }
                        else
                        {
                            SapResult sr = (SapResult)result[i][1];

                            foreach (SAP.Middleware.Table.RETURN ret in sr.RETURN_LIST)
                            {
                                sapWFItem["ErrorMsg"] += ret.MESSAGE + "\n";
                            }

                        }
                    }

                    //Update WF Item
                    SPContext.Current.Web.AllowUnsafeUpdates = true;
                    sapWFItem.Update();
                    SPContext.Current.Web.AllowUnsafeUpdates = false;
                }
                #endregion

            }
        }

        private DataTable ConvertToSAPDataSource(string type, DataTable dataTable)
        {
            DataTable dt = new DataTable();
            switch (type)
            {

                case "ExpenceDetails":
                    dt = dataTable.DefaultView.ToTable(false, new string[] {"RmbAmt",
                                                                             "IsPaidByCredit",
                                                                             "SpecialApproved",
                                                                             "SpecialApprove",
                                                                             "CompanyStandards"});

                    dt.Columns["RmbAmt"].ColumnName = "Amount";
                    dt.Columns["IsPaidByCredit"].ColumnName = "IsPaidByCC";
                    dt.Columns["SpecialApproved"].ColumnName = "IsApproved";
                    dt.Columns["CompanyStandards"].ColumnName = "CompanyStd";
                    dt.Columns["SpecialApprove"].ColumnName = "IsNeedApproved";
                    break;

                case "GroupExpenceDetails":
                    dt = dataTable.DefaultView.ToTable(false, new string[] {
                                                                             "GLAccount",
                                                                             "ApprovedRmbAmt",
                                                                             "ExpenseType",
                                                                             "CostCenter"});

                    dt.Columns["GLAccount"].ColumnName = "AccountGL";
                    dt.Columns["ApprovedRmbAmt"].ColumnName = "Amount";
                    dt.Columns["ExpenseType"].ColumnName = "ItemText";

                    break;
                default:
                    break;
            }

            return dt;
        }

        private List<SapParameter> InitializeSapParameter(DataTable originalDetailsDT, DataTable expenseTypeDT, Hashtable parametersHash)
        {
            List<SapParameter> mSapParametersTR = new List<SapParameter>();

            SapParameter mSapParameters = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "KR",
                BusArea = "0001",
                Currency = "RMB",
                //EmployeeID = "6000000150",  //
                // EmployeeName = "TEST",        //
                ExchRate = 1,
                //Header = "Travel Expense Claim",
                //RefDocNo = "CA" + DateTime.Now.ToString("yyyyMMddHHmmss"),// 
                UserName = "acnotes"
            };

            foreach (DictionaryEntry entry in parametersHash)
            {
                foreach (PropertyInfo prop in mSapParameters.GetType().GetProperties())
                {
                    if (entry.Key.AsString() == prop.Name)
                    {

                        switch (prop.PropertyType.ToString())
                        {
                            case "System.Decimal":
                                prop.SetValue(mSapParameters, decimal.Parse(entry.Value.AsString()), null);
                                break;
                            default:
                                prop.SetValue(mSapParameters, entry.Value.AsString(), null);
                                break;
                        }

                        break;
                    }
                }
            }

            mSapParameters.GroupExpenceDetails = GetSapParametersList(expenseTypeDT);
            mSapParameters.ExpenceDetails = GetSapParametersList(originalDetailsDT);

            mSapParametersTR.Add(mSapParameters);

            return mSapParametersTR;
        }

        internal static List<ExpenceDetail> GetSapParametersList(DataTable dt)
        {
            List<ExpenceDetail> oList = new List<ExpenceDetail>();
            ExpenceDetail expenceDetail;
            decimal tmp;

            foreach (DataRow dr in dt.Rows)
            {
                expenceDetail = new ExpenceDetail();
                foreach (PropertyInfo prop in expenceDetail.GetType().GetProperties())
                {
                    if (dt.Columns.Contains(prop.Name))
                    {
                        switch (prop.PropertyType.ToString())
                        {
                            case "System.Decimal":
                                if (decimal.TryParse(dr[prop.Name].AsString(), out tmp))
                                {
                                    prop.SetValue(expenceDetail, tmp, null);
                                }
                                break;
                            case "System.Boolean":
                                prop.SetValue(expenceDetail, Convert.ToBoolean(int.Parse(dr[prop.Name].AsString())), null);
                                break;
                            default:
                                prop.SetValue(expenceDetail, dr[prop.Name].AsString(), null);
                                break;
                        }

                    }
                }
                oList.Add(expenceDetail);
            }

            return oList;

        }
    }
}