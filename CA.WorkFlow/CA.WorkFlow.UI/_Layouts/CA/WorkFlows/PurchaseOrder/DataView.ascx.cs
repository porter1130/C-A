namespace CA.WorkFlow.UI.PurchaseOrder
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using System.Data;
    
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;

    public partial class DataView : BaseWorkflowUserControl
    {
        private string msg;
        public string MSG { get { return msg; } }

        private string requestId;
        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        private bool isMaintenance;
        /// <summary>
        /// 是否是维修单
        /// </summary>
        public bool IsMaintenance
        {
            get { return isMaintenance; }
            set { isMaintenance = value; }
        }

        private bool isHideFinanceNum;
        public bool IsHideFinanceNum { set { isHideFinanceNum = value; } }

        private bool isCreateSapStep = false;//

        public bool IsCreateSapStep
        {
            get { return isCreateSapStep; }
            set { isCreateSapStep = value; }
        }

       public bool bIsCompex = false;//是否是CaPex
       public double dServeice = 0;//包含了X开头的产品的服务费
       public double dTotalNetPrice = 0;//非包含了服务费的Item的总净价
       public bool IsNotReturn = false;//是否是非退货

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (isHideFinanceNum)
                {
                    this.ffPONumFinance.Visible = false;
                    ffPONumFinanceDisp.Visible = true;
                }
                else
                {
                    this.ffPONumFinance.Visible = true;
                    ffPONumFinanceDisp.Visible = false;
                }

                if (IsCreateSapStep)
                {
                    ShowFinanceData();
                }
                DataTable dt = new DataTable();
                dt = PurchaseOrderCommon.GetDataTable(requestId);
                if (IsNotReturn)
                {
                    GetServeiceFee(dt);
                }
                this.rptItem.DataSource = dt;
                this.rptItem.DataBind();
            }

        }

        /// <summary>
        /// 得到总服务费及总净价
        /// </summary>
        /// <param name="dt"></param>
        void GetServeiceFee(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string sItemCode=dr["ItemCode"]==null?"":dr["ItemCode"].ToString();
                if (sItemCode.IndexOf("X", StringComparison.InvariantCultureIgnoreCase) == 0)//有包含服务费用的
                {
                    double dPrice = 0;
                    double dTaxValue = 0;
                    double dCurrentNetPrice = 0;
                    if (double.TryParse(dr["TotalPrice"] == null ? "" : dr["TotalPrice"].ToString(), out dPrice))//总价
                    {
                        if (double.TryParse(dr["TaxValue"] == null ? "" : dr["TaxValue"].ToString(), out dTaxValue))//税额
                        {
                             dCurrentNetPrice = dPrice - dTaxValue;

                             string sdiscount = WorkflowItemCode.DISCOUNT;
                             string sDescription = dr["Description"] == null ? "" : dr["Description"].ToString();
                             if (sDescription.ToLower().Contains(sdiscount))//包含打折的就要减掉
                             {
                                 dServeice = dServeice - dCurrentNetPrice;
                             }
                             else
                             {
                                 dServeice = dServeice + dCurrentNetPrice;
                             }
                        }
                    }
                }
                else//非包含服务费用的
                {
                    double dPrice = 0;
                    double dTaxValue = 0;
                    if (double.TryParse(dr["TotalPrice"] == null ? "" : dr["TotalPrice"].ToString(), out dPrice))
                    {
                        if (double.TryParse(dr["TaxValue"] == null ? "" : dr["TaxValue"].ToString(), out dTaxValue))
                        {
                            double dNetPrice = dPrice - dTaxValue;
                            dTotalNetPrice += dNetPrice;
                        }
                    }

                }
            }
        }
        public double GetValue(double dNetPrice,string sItemCode)
        {
            if (!IsNotReturn)
            {
                return dNetPrice;
            }
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sPONo = fields["PONumber"].ToString();
            if (sItemCode.IndexOf("X", StringComparison.InvariantCultureIgnoreCase)==0)
            {
                return 0;
            }
            if (dServeice != 0)
            {
                if (sPONo.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
                {
                    return -Math.Round(dServeice * (dNetPrice / dTotalNetPrice) +Math.Abs(dNetPrice), 2);
                }
                else
                {
                    return Math.Round(dServeice * (dNetPrice / dTotalNetPrice) + dNetPrice, 2);
                }
            }
            else
            {
                return dNetPrice;
            }
        }

        public override bool Validate(string action)
        {
            bool isValid = false;
            if (action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                isValid = WorkflowContext.Current.TaskFields["Body"].AsString().IsNotNullOrWhitespace();
                if (!isValid)
                {
                    msg = "Please fill in the Reject Comments.";
                    return isValid;
                }
            }
            return true;
        }

        /// <summary>
        /// 为PO单添加ACNumber 数据
        /// </summary>
        public void AddFinanceComments()
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            foreach (RepeaterItem item in rptItem.Items)
            {
                HiddenField HFID = item.FindControl("HFID") as HiddenField;
                TextBox TextBoxFinanceComments = item.FindControl("TextBoxACNumber") as TextBox;
                TextBox TextBoxAllocatedValue = item.FindControl("TextBoxAllocatedValue") as TextBox;
                UpdateItem(SPContext.Current.Web, HFID.Value, TextBoxFinanceComments.Text.Trim(), TextBoxAllocatedValue.Text.Trim());
            }
        }

        /// <summary>
        /// 通过ID更新ACNumber字段。
        /// </summary>
        /// <param name="web"></param>
        /// <param name="sID"></param>
        /// <param name="sComments"></param>
        /// <param name="AllocatedValue"></param>
        void UpdateItem(SPWeb web, string sID, string sComments, string AllocatedValue)
        {

            SPQuery query = new SPQuery();
            string sQueryFormat = @"
                                       <Where>
                                          <Eq>
                                             <FieldRef Name='ID' />
                                             <Value Type='Counter'>{0}</Value>
                                          </Eq>
                                       </Where>";
            query.Query = string.Format(sQueryFormat,sID);
            SPListItemCollection splic= web.Lists["PurchaseOrderItems"].GetItems(query);
            if (null == splic || splic.Count == 0)
            {
                return;
            }
            SPListItem spli = splic[0];
            spli["ACNumber"] = sComments;
            spli["AllocatedValue"] = AllocatedValue;
            spli.Update();
        }

        /// <summary>
        /// Capex里财务看的和填 写的
        /// </summary>
        void ShowFinanceData()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sPONumber= fields["PONumber"].ToString();

            bIsCompex =PurchaseOrderCommon.IsComPex(sPONumber);
            if (bIsCompex)
            {
                this.ACCode.Visible = true;
                this.ACNumber.Visible = true;
                //ACCodeEmpty.Visible = true;
               // ACNumberEmpty.Visible = true;

            }
            if (sPONumber.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
            {
                IsNotReturn = false;
            }
            else
            {
                IsNotReturn = true;
                Allocated.Visible = true;
               // AllocatedEmpty.Visible = true;
            }
        }

        /// <summary>
        /// 设置PO单标题
        /// </summary>
       public void SetTitle(string sPOType)
        {
            if (sPOType.Equals("Maintenance"))
            {
                LabelCNtitle.Text = "维护维修服务";
                LabelEnTitle.Text = "Maintenance";
                isMaintenance = true;
                ContractEN.InnerText = "MAINTENANCE";
                ContractCN.InnerText = "维护维修服务合同";
            }
            else
            {
                LabelCNtitle.Text = "对外采购";
                LabelEnTitle.Text = "External";
                ContractEN.InnerText = "SUPPLY";
                ContractCN.InnerText = "供货合同";
            }
        }


    }
}