namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using Microsoft.SharePoint;
    using CodeArt.SharePoint.CamlQuery;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;
    using System.Collections;

    using System.Text;

    public partial class DataGRSR : UserControl
    {
        DataSet rs = null;

        internal DataTable ItemTable
        {
            get
            {
                return (this.ViewState["ItemTable"] as DataTable) ?? CreateSimpleItemTable();
            }
            set
            {
                this.ViewState["ItemTable"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
            if (!this.IsPostBack)
            {
                Bind();
            }
        }

        void CheckAccount()
        {
            // wf_Store可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_Store", "wf_HO", "w_PMGR" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }
        void Bind()
        {
            SPListItemCollection items = GetPOItemCollection(TextBoxPO.Text.Trim(),TextBoxPR.Text.Trim());//根据门店得到门店下的所有的末送货的PO信息
            if (null == items)
            {
                return;
            }
            ItemTable = GetSimpleData(items);//得到精简后的datatable
            DataTable poDT = GetPONOData(ItemTable, "PONumber");//PONumber//作为最外面的repeater使用  。 
            ItemTable.TableName = "ItemTable";
            poDT.TableName = "PODT";
            ItemTable.PrimaryKey =new DataColumn[]{ ItemTable.Columns["ID"]};

            rs = new DataSet("rs");
            rs.Tables.Add(poDT);
            rs.Tables.Add(ItemTable);
            rs.Relations.Add("PONO", poDT.Columns["PONumber"], ItemTable.Columns["PONumber"]);

            this.rptPOs.DataSource = rs.Tables["poDT"];
            this.rptPOs.DataBind();
        }

        /**
         * Bind the PO Datatable to the first level repeater control and bind the sub repeater control.
         */
        protected void rptPOs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater tempRpt = (Repeater)e.Item.FindControl("rptItems");
                HiddenField HFVendorIDName = e.Item.FindControl("HFVendorIDName") as HiddenField;
                
                if (tempRpt != null)
                {
                    System.Data.DataView drv = ((DataRowView)e.Item.DataItem).CreateChildView(rs.Relations["PONO"]);
                    Label LabelCount = e.Item.FindControl("LabelCount") as Label;
                    Label LabelPRBy = e.Item.FindControl("LabelPRBy") as Label;
                    Label LabelCreated = e.Item.FindControl("LabelCreated") as Label;
                    Label LabelVendor = e.Item.FindControl("LabelVendor") as Label;
                    Label LabelPONumber = e.Item.FindControl("LabelPONumber") as Label;

                    tempRpt.DataSource = drv;
                    tempRpt.DataBind();

                    string sDate = string.Empty;
                    string sVendorName=string.Empty;

                   StringBuilder sb = new StringBuilder();
                   DataTable dt = drv.ToTable();
                   int iItemCount = dt.Rows.Count;
                   foreach (DataRow dr in dt.Rows)
                   {
                       if (sb.Length > 0)
                       {
                           sb.Append("|");
                       }
                       sb.Append(dr["CostCenter"]);
                       sb.Append(",");
                       sb.Append(dr["VendorID"]);
                       sb.Append(",");
                       sb.Append(dr["VendorName"]);
                       sb.Append(",");
                       sb.Append(dr["ReturnPONO"]);
                       //iItemCount += int.Parse(dr["TotalQuantity"].ToString());
                       sDate = dr["Created"].ToString();
                       sVendorName = dr["VendorName"].ToString();
                       //
                   }
                  HFVendorIDName.Value = sb.ToString();//CostCenter,VendorID,VendorName|
                  LabelCount.Text = iItemCount.ToString();
                  LabelCreated.Text = DateTime.Parse(sDate).ToString("MM-dd-yyy");
                  LabelVendor.Text = string.Concat("Vendor(供应商): ", sVendorName);
                  LabelPRBy.Text = IsPOCreatedByHO(LabelPONumber.Text.ToString())==true?"HO":"Store";
                  if (!HFIsHO.Value.Equals("True"))
                  {
                      LabelVendor.Visible = false;
                  }
                  
                }
            }
        }

        private void GetRepeaterData()
        {
            List<string> modifiedPO = new List<string>();
            List<string> itemIds = new List<string>();
            foreach (RepeaterItem item in this.rptPOs.Items)
            {
                var hidPONumber = (HiddenField)item.FindControl("hidPONumber");
                var cbIsReceived = (CheckBox)item.FindControl("cbIsReceived");

                if (!cbIsReceived.Checked)
                    continue;

                modifiedPO.Add(hidPONumber.Value);
            }

            if (modifiedPO.Count > 0)
            {
                //根据PO编号查询Record ID，此处ID不会出现重复，因为ID为记录ID
                var ids = from d in ItemTable.AsEnumerable()
                          where modifiedPO.Contains(d.Field<string>("PONumber"))
                          select d["ID"];
                foreach (string id in ids)
                {
                    itemIds.Add(id);
                }
            }

            if (itemIds.Count > 0)
            {
                //Update the po items. Set to received.
                PurchaseRequestCommon.UpdatePOReceivedItems(itemIds);

                //Check whether PO can be paid.
                //In the items, the po must be not completed.
                DataTable items = PurchaseRequestCommon.GetNotRecPOItemsByReqId(modifiedPO.ToArray());
                DataTable nonCompletedPOs = GetPONOData(items, "Title");
                List<string> nonPOs = new List<string>();
                foreach (DataRow dr in nonCompletedPOs.Rows)
                {
                    nonPOs.Add(dr["Title"].ToString());
                }
                //查找modifiedPO中是否有需要付款的PO出现
                var paidPOs = from d in modifiedPO.AsEnumerable<string>()
                              where !nonPOs.Contains(d)
                              select d;

                paidPOs.ToArray<string>();
                
            }

            
        }

        /**
         * 查询当前帐号对应的已产生PO的且为未收货状态的Item
         */
        private SPListItemCollection GetPOItemCollection(string sPONumber,string sPRNumber)
        {
            var qIsReceived = new QueryField("IsReceived", false);
            QueryField qCostCenter = new QueryField("CostCenter", false);
            QueryField qCreated = new QueryField("Created", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qIsReceived.NotEqual(true));

           string sUserName = SPContext.Current.Web.CurrentUser.LoginName;

           if (PurchaseRequestCommon.IsInGroups(sUserName, new string[] { "wf_Store" }))
           {
               List<string> costCenters = GetOwnCostCenters(SPContext.Current.Web.CurrentUser.LoginName);
               if (costCenters.Count == 0)
               {
                   return null;
               }

               CamlExpression ceOR = null;
               foreach (var costCenter in costCenters)
               {
                   ceOR = WorkFlowUtil.LinkOr(ceOR, qCostCenter.Equal(costCenter));
               }
               exp = WorkFlowUtil.LinkAnd(exp, ceOR);

           }
           else if (PurchaseRequestCommon.IsInGroups(sUserName, new string[] { "wf_HO" }))//如果是wf_HO
           {
               QueryField qAuthor = new QueryField("Author", false);
               exp = WorkFlowUtil.LinkAnd(exp, qAuthor.Equal(SPContext.Current.Web.CurrentUser.Name));

               HFIsHO.Value = "True";
           }
           else if (PurchaseRequestCommon.IsInGroups(sUserName, new string[] { "w_PMGR" }))
           {
               QueryField qfTitle = new QueryField("Title", false);
               exp = WorkFlowUtil.LinkAnd(exp, qfTitle.BeginsWith("H"));
           }

           int iPO = sPONumber.Length;
           int iPR = sPRNumber.Length;
           if (iPO > 0)//按PONumber查询
           {
               QueryField qTitle = new QueryField("Title", false);
               exp = WorkFlowUtil.LinkAnd(exp, qTitle.Equal(sPONumber));
           }
           if (iPR > 0)
           {
               QueryField qPRNO = new QueryField("PRNumber", false);
               exp = WorkFlowUtil.LinkAnd(exp, qPRNO.Equal(sPRNumber));
           }
           if (iPO > 0 || iPR > 0)
           {
               QueryField qTitle = new QueryField("Title", false);
               CamlExpression ce = null;
               ce = WorkFlowUtil.LinkAnd(ce,qIsReceived.NotEqual(true));
               ce = WorkFlowUtil.LinkAnd(ce, qTitle.Contains("R"));
               exp = WorkFlowUtil.LinkOr(ce, exp);
           }
          SPListItemCollection  lc = ListQuery.Select()
                   .From(SPContext.Current.Web.Lists["PurchaseOrderItems"])
                    .Where(exp)
                    .GetItems();
             
            return lc;
        }

        /**
         * 为门店查找对应的CostCenters信息，包含过期的
         */
        private List<string> GetOwnCostCenters(string managerAccount)
        {
            var qManagerAccount = new QueryField("ManagerAccount", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qManagerAccount.Equal(managerAccount));
            SPListItemCollection lc = null;
           
            lc = ListQuery.Select()
                    .From(SPContext.Current.Web.Lists["Cost Centers"])
                    .Where(exp)
                    .GetItems();
            

            List<string> centers = new List<string>();
            foreach (SPListItem item in lc)
            {
                centers.Add(item["Title"].ToString());
            }
            return centers;
        }

        /**
         * 收货点击
         */
        protected void btnGRSR_Click(object sender, EventArgs e)
        {     

            DataTable dtFeedBack = CreateTatableFeedback();
            foreach (RepeaterItem item in rptPOs.Items)
            {
                CheckBox cbIsReceived = item.FindControl("cbIsReceived") as CheckBox;
                RadioButtonList RadioListStandhard = item.FindControl("RadioListStandhard") as RadioButtonList;
                RadioButtonList RadioListQuantity = item.FindControl("RadioListQuantity") as RadioButtonList;
                RadioButtonList RadioListDelivery = item.FindControl("RadioListDelivery") as RadioButtonList;
                RadioButtonList RadioListManner = item.FindControl("RadioListManner") as RadioButtonList;
                RadioButtonList RadioListResponse = item.FindControl("RadioListResponse") as RadioButtonList;

                if (cbIsReceived.Checked == false || RadioListStandhard.SelectedIndex == -1 || RadioListQuantity.SelectedIndex == -1 || RadioListDelivery.SelectedIndex == -1 || RadioListManner.SelectedIndex == -1 || RadioListResponse.SelectedIndex == -1)
                {
                    continue;
                }
                Label LabelPOnumber = item.FindControl("LabelPOnumber") as Label;
                HiddenField HFVendorIDName = item.FindControl("HFVendorIDName") as HiddenField;
                //<asp:HiddenField ID="HFVendorIDName" runat="server" />
                DataRow dr = dtFeedBack.NewRow();
                string sPONO= LabelPOnumber.Text;
                dr["PONumber"] =sPONO;
                dr["StandhardAndQuality"] = RadioListStandhard.SelectedValue;
                dr["Quantity"] = RadioListQuantity.SelectedValue;
                dr["DeliveryTime"] = RadioListDelivery.SelectedValue;
                dr["ServiceManner"] = RadioListManner.SelectedValue;
                dr["Response"] = RadioListResponse.SelectedValue;
                string[] venderArray = SetrptItemsReceived(HFVendorIDName.Value.Split('|'), sPONO);//HFVendorIDName.Value:     CostCenter,VendorID,VendorName|
                dr["Title"] = venderArray[0].ToString();
                dr["VendorName"] = venderArray[1].ToString();
                dtFeedBack.Rows.Add(dr);
            }

            if (dtFeedBack.Rows.Count > 0)
            {
                PurchaseRequestCommon.BatchAddToListByDatatable(dtFeedBack, "VendorFeedback");

                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('GR/SR success！');</script>");
            }

            Bind();
        }

        string[] SetrptItemsReceived(string[] strArray,string sPONumber)
        {
            string sVenderID = string.Empty;
            string sVenderName = string.Empty;
            foreach (string str in strArray) //CostCenter,VendorID,VendorName|
            {

                //sb.Append(dr["CostCenter"]);0
                //sb.Append(",");
                //sb.Append(dr["VendorID"]);1
                //sb.Append(",");
                //sb.Append(dr["VendorName"]);2
                //sb.Append(",");
                //sb.Append(dr["ReturnPONO"]);3

                string[] sPOArray = str.Split(',');
                string sCostcenter = sPOArray[0];
                sVenderID = sPOArray[1];
                sVenderName = sPOArray[2];
                string ReturnPONO = sPOArray[3];
                SetPOItemIsReceived("Title", sPONumber, sCostcenter, "PurchaseOrderItems");
                SetPOItemIsReceived("PONumber", sPONumber, sCostcenter, "PurchaseRequestItems");

                //更新退货的
                if (ReturnPONO != "")
                {
                    SetPOItemIsReceived("Title", ReturnPONO, sCostcenter, "PurchaseOrderItems");
                    SetPOItemIsReceived("PONumber", ReturnPONO, sCostcenter, "PurchaseRequestItems");
                }
            }
            return new string[2] { sVenderID, sVenderName };
        }

        void SetReturnPOReceived(string sColumnName,string sValue,string sCostcenter)
        {
            SPQuery query = new SPQuery();
            string sQueryFormate = @"<Where>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='{0}' />
                                                    <Value Type='Text'>{1}</Value>
                                                </Eq>
                                                    <Eq>
                                                    <FieldRef Name='CostCenter' />
                                                    <Value Type='Text'>{2}</Value>
                                                    </Eq>
                                            </And>
                                        </Where>";
            query.Query = string.Format(sQueryFormate, sColumnName, sValue, sCostcenter);
            SPList list = SPContext.Current.Web.Lists["Purchase Request Workflow"];

            SPListItemCollection splic = list.GetItems(query);
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            foreach (SPListItem item in splic)
            {
                item["IsReceived"] = 1;
                item.Update();
            }
               
        }


        void SetPOItemIsReceived(string sPOColumnName,string sPONumber,string sCostcenter,string sListName)
        {
            SPQuery query = new SPQuery();
            string sQueryFormate = @"<Where>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='{0}' />
                                                    <Value Type='Text'>{1}</Value>
                                                </Eq>
                                                    <Eq>
                                                    <FieldRef Name='CostCenter' />
                                                    <Value Type='Text'>{2}</Value>
                                                    </Eq>
                                            </And>
                                        </Where>";
            query.Query = string.Format(sQueryFormate, sPOColumnName, sPONumber, sCostcenter);
            SPList list = SPContext.Current.Web.Lists[sListName];

            SPListItemCollection splic = list.GetItems(query);
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            foreach (SPListItem item in splic)
            {
                item["IsReceived"] = 1;
                item.Update();
            }
              
        }

        DataTable CreateTatableFeedback()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONumber");
            dt.Columns.Add("StandhardAndQuality");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("DeliveryTime");
            dt.Columns.Add("ServiceManner");
            dt.Columns.Add("Response");
            dt.Columns.Add("Title");//VenderID
            dt.Columns.Add("VendorName"); 
            return dt;
        }

        /**
         * Get the distinct po number collection
         */
        protected DataTable GetPONOData(DataTable items, string columnName)
        {
            var pos = from d in items.AsEnumerable()
                      group d by new { PONumber = d["PONumber"] } into g
                      select new
                      {
                          PONumber = g.Key.PONumber
                      };
            DataTable poNODT = new DataTable();
            poNODT.Columns.Add(columnName);
            DataRow dr = null;

            foreach (var poNumber in pos)
            {
                dr = poNODT.Rows.Add();
                dr[columnName] = poNumber.PONumber;
            }
            return poNODT;
        }

        /**
         * Get the neccessary data from splistitemcollection. Only keep some columns.
         */
        protected DataTable GetSimpleData(SPListItemCollection lc)
        {
            DataTable dt = CreateSimpleItemTable();///PRNumber
            DataTable dtR = CreateSimpleItemTable();                                                   
            DataRow dr = null;
            foreach (SPListItem item in lc)
            {
                string sUserName = SPContext.Current.Web.CurrentUser.LoginName;
                //wf_Ho,排除Costcenter中包含S的，且不是以H开头。 
                if (PurchaseRequestCommon.IsInGroups(sUserName, new string[] { "wf_HO" }) && item["CostCenter"].ToString().ToUpper().Contains("S") && !item["CostCenter"].ToString().StartsWith("H10",StringComparison.InvariantCultureIgnoreCase))//如果是wf_HO
                {
                    continue;
                }

                if (item["PRNumber"].ToString().EndsWith("R"))//该条记录是退货
                {
                    DataRow drReturn = dtR.NewRow();
                    drReturn = dtR.Rows.Add();
                    drReturn["ID"] = item["ID"];
                    drReturn["PONumber"] = item["Title"];
                    drReturn["PRNumber"] = item["PRNumber"];
                    drReturn["ItemCode"] = item["ItemCode"];
                    drReturn["Description"] = item["Description"];
                    drReturn["TotalQuantity"] = item["TotalQuantity"];
                    drReturn["Unit"] = item["Unit"];
                }
                else
                {
                    string sPONumber=TextBoxPO.Text.Trim();
                    string sPRumber=TextBoxPR.Text.Trim();

                    if (null != item["Title"])
                    {
                        if (sPONumber != "" && item["Title"].ToString() != sPONumber)
                        {
                            continue;
                        }
                    }
                    if (null != item["PRNumber"])
                    {
                        if (sPRumber != "" && item["PRNumber"].ToString() != sPRumber)
                        {
                            continue;
                        }
                    }

                    dr = dt.Rows.Add();
                    dr["ID"] = item["ID"];
                    dr["PONumber"] = item["Title"];
                    dr["PRNumber"] = item["PRNumber"];
                    dr["ItemCode"] = item["ItemCode"];
                    dr["Description"] = item["Description"];
                    dr["TotalQuantity"] = item["TotalQuantity"];
                    dr["Unit"] = item["Unit"];
                    dr["VendorID"] = item["VendorID"];
                    dr["VendorName"] = item["VendorName"];
                    dr["CostCenter"] = item["CostCenter"];
                    dr["Created"] = item["Created"]; 
                }   
            }


            if (dtR.Rows.Count > 0)
            {
                foreach (DataRow returnRow in dtR.Rows)//遍历退货列表
                {

                    foreach (DataRow item in dt.Rows)
                    {
                        if (returnRow["ItemCode"].ToString() == item["ItemCode"].ToString())
                        {
                            string sReturnPONO = returnRow["PONumber"].ToString();
                            string sReturnPO = PurchaseRequestCommon.GetReturnPO(returnRow["PONumber"].ToString()).Trim();///该退货所对应的PO号。
                            if (sReturnPO == item["PONumber"].ToString())
                            {
                                item["TotalQuantity"] = (double.Parse(item["TotalQuantity"].ToString()) - Math.Abs(double.Parse(returnRow["TotalQuantity"].ToString()))).ToString("0.000");
                                item["ReturnPONO"] = sReturnPONO;
                            }                                                                                   
                        }
                    }
                }
            }

            return dt;
        }

        /**
         * Create the simple datatable that only contain some neccessary columns.
         */
        protected DataTable CreateSimpleItemTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("PRNumber");
            dt.Columns.Add("PONumber");
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("Description");
            dt.Columns.Add("TotalQuantity");
            dt.Columns.Add("Unit");
            dt.Columns.Add("VendorID");
            dt.Columns.Add("VendorName");
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("Created");
            dt.Columns.Add("ReturnPONO");//退货的目的PO号

            return dt;
        }

        protected void ButPOQuery_Click(object sender, EventArgs e)
        {
            Bind();
        }

        bool IsPOCreatedByHO(string sPONO)
        {
            bool isHO = false;

            CamlExpression ce = null;
            QueryField qfPO = new QueryField("PONumber");
            ce = WorkFlowUtil.LinkAnd(ce, qfPO.Equal(sPONO));

            SPListItemCollection splic = null;
            splic = ListQuery.Select()
                    .From(SPContext.Current.Web.Lists["PurchaseRequestItems"])
                    .Where(ce)
                    .GetItems();
            if (null != splic && splic.Count > 0)
            {
                isHO = bool.Parse(splic[0]["IsHO"].ToString());
            }
          
            return isHO;
        }

    }


}