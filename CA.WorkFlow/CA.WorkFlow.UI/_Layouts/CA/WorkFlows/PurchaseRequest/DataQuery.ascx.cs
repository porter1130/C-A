namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Web.UI;
    using System.Data;
    using System.Collections.Generic;

    public partial class DataQuery : UserControl
    {
        DataTable poItems = null;
        DataTable prItems = null;
        protected string prListId = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnQuery_Click(object sendor, EventArgs e)
        {
            var poNumber = this.hidPONum.Value.Trim();
            poItems = PurchaseRequestCommon.GetPOItemsByRequestId(poNumber);
            if (poItems == null || poItems.Rows.Count == 0)
            {
                return;
            }

            prListId = WorkflowListID.GetListId("Purchase Request Workflow");

            List<string> ids = GetIds();
            prItems = PurchaseRequestCommon.GetPRItemsById(ids.ToArray());

            this.rptPOItem.DataSource = poItems;
            this.rptPOItem.DataBind();

            this.rptPRItem.DataSource = prItems;
            this.rptPRItem.DataBind();

            this.lbPOTotal.Text = GetTotal(poItems);
            this.lbPRTotal.Text = GetTotal(prItems);
        }

        protected void btnOpenDetail_Click(object sendor, EventArgs e)
        {
            var detailStr = this.hidDetail.Value; //Purchase Request Workflow;PR0001;PurchaseRequest'
            char[] split = { ';' };
            var details = detailStr.Split(split);
            if (details == null || details.Length == 0)
            {
                return;
            }
            string listId = WorkflowListID.GetListId(details[0]);
            string id = GetId(details[0], details[1]);

            string link = string.Format("/WorkFlowCenter/_Layouts/CA/WorkFlows/{0}/DisplayForm.aspx?List={1}&ID={2}", details[2], listId, id);

            if (id != null)
            {
                this.hidNewWindowLink.Value = link;
                //this.Response.Redirect(link);
                //this.Response.Write(string.Format("<script type='text/javascript'>window.open('{0}', '_blank');</script>", link));
            }
        }

        private string GetId(string listName, params string[] values)
        {
            var result = PurchaseRequestCommon.GetRecordId(listName, "WorkflowNumber", "Title", values);
            return result.Count > 0 ? result[0] : null;
        }

        private List<string> GetIds()
        {
            List<string> ids = new List<string>();
            
            foreach (DataRow dr in poItems.Rows)
            {
                ids.Add(dr["PRItemID"].ToString());
            }
            return ids;
        }

        private string GetTotal(DataTable dt)
        {
            double total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                total += Convert.ToDouble(dr["TotalPrice"].ToString());
            }

            return Convert.ToString(Math.Round(total, 2));
        }

    }
}