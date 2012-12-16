namespace CA.WorkFlow.UI.PurchaseOrder
{
    using System;
    using QuickFlow.Core;

    public partial class DataEdit : BaseWorkflowUserControl
    {
        private string requestId;
        public string RequestId
        {
            set
            {
                this.requestId = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.rptItem.DataSource = PurchaseOrderCommon.GetDataTable(requestId);
                this.rptItem.DataBind();
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                Installment1.SPONO = fields["PONumber"].ToString();
            }
        }

        public void SavePaymentData()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            Installment1.SaveData(fields["PONumber"].ToString(), fields["GrandTotal"].ToString());
        }
        
    }
}