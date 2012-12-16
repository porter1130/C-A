namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using QuickFlow.UI.ListForm;
    using SharePoint.Utilities.Common;
    using System.Web.UI;

    public partial class SelectItem : UserControl
    {
        private readonly ObjectDataSource dataSource = new ObjectDataSource();

        private bool isHO = false;
        internal bool IsHO { set { isHO = value; } }

        protected override void OnLoad(EventArgs e)
        {
            this.dataSource.ID = "ItemCodeDS";
            this.dataSource.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSource.SelectMethod = "GetItemCodeDS";
            this.Controls.Add(this.dataSource);
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.SPGridView1.BorderStyle = BorderStyle.Solid;
                this.SPGridView1.GridLines = GridLines.Horizontal;

                this.SPGridView1.DataSource = null;
                this.SPGridView1.DataBind();
            }
        }

        protected void btnQuery_Click(object sendor, EventArgs e)
        {
            string startItemCode = this.txtItemCode.Text.Trim();
            string partDesc = this.txtDesc.Text.Trim();
            string startWiths = string.Empty;

            var dataCtl = (this.Parent.FindControl("ListFormControl1") as ListFormControl).FindControl("DataForm1") as DataEdit;
            var requestType = dataCtl.GetRequestType();
            var formType = dataCtl.GetFormType();
            var storePurpose = dataCtl.GetStorePurposeType();
            var itemScope = string.Empty;
            if (formType == "Store") // by xu 
            {
                if (storePurpose.Equals("QuarterlyOrder", StringComparison.CurrentCultureIgnoreCase))
                {
                    itemScope = "QO";
                }
                else if (storePurpose.Equals("PaperBag", StringComparison.CurrentCultureIgnoreCase))
                {
                    itemScope = "PB";
                }
            }

            switch (requestType) //根据Request Type初始化Item Code，默认Capex被选中
            {
                case "Capex":
                    startWiths = "C,X"; //Capex
                    break;
                case "Opex":
                    startWiths = "E,X"; //Expense
                    break;
                case "Service":
                    startWiths = "S"; //Service
                    break;
                default:
                    startWiths = "E,X"; //Opex属于常用项目
                    break;
            }

            this.hidSelectedItemCode.Value = string.Empty; //Clear old hidden value once clicking query button

            this.dataSource.SelectParameters.Clear();
            this.dataSource.SelectParameters.Add("startWiths", DbType.String, startWiths);
            this.dataSource.SelectParameters.Add("startItemCode", DbType.String, startItemCode);
            this.dataSource.SelectParameters.Add("partDesc", DbType.String, partDesc);
            this.dataSource.SelectParameters.Add("itemScope", DbType.String, itemScope);
            this.dataSource.SelectParameters.Add("needFilter", DbType.String, isHO ? "N" : "Y");

            this.SPGridView1.DataSourceID = "ItemCodeDS";
            this.SPGridView1.DataBind();
        }

        protected void SPGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SPGridView1.PageIndex = e.NewPageIndex;
            this.SPGridView1.DataBind();
        }

        protected void btnCopyItem_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        internal void Reset()
        {
            this.hidSelectedItemCode.Value = string.Empty;

            this.txtItemCode.Text = this.txtDesc.Text = string.Empty;

            this.SPGridView1.DataSource = null;
            this.SPGridView1.DataSourceID = null;
            this.SPGridView1.DataBind();
        }

        public DataTable GetItemCodeDS(string startWiths, string startItemCode, string partDesc, string itemScope, string needFilter)
        {
            return PurchaseRequestCommon.GetItemCodeForDS(startWiths, startItemCode, partDesc, itemScope, needFilter.Equals("Y"));
        }
    }
}