namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using System.Web.UI;
    using System.Data;
    using System.Collections.Generic;
    using System.Collections;
    using System.Text;

    using Microsoft.SharePoint;

    public partial class DataList : UserControl
    {
        DataTable dt = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                init();
            }
        }

        private void init()
        {
            string sAccount = SPContext.Current.Web.CurrentUser.LoginName;

            dt = PurchaseRequestCommon.GetPRTableByPOStatus("Pending",sAccount);
            this.rptItem.DataSource = dt;
            this.rptItem.DataBind();
        }

        public void Reload()
        {
            init();
        }


        protected void btnCreatePO_Click(object sender, EventArgs e)
        {
            CreatePOForMultiPR();
            Reload();
        }

        private void CreatePOForMultiPR()
        {
            var poNums = this.hidSelectNums.Value;
            char[] split = { ';' };
            var nums = poNums.Split(split);
            if (nums.Length == 0)
            {
                return;
            }
            Hashtable hashPOs = PurchaseRequestCommon.CreatePOByReqestIds(nums);

            UpdatePRTable(nums, hashPOs); //保存生成的PO Number到PR

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var po in hashPOs.Values)
            {
                sb.Append(po);
                if (++i == 5)
                {
                    sb.Append("\n");
                    i = 0;
                }
            }
            //sb.Remove(sb.Length - 1, 1);
            this.hidCreatedPONumber.Value = sb.ToString();
        }

        private void UpdatePRTable(string[] requestIds, Hashtable hashPOs)
        {
            var ids = this.hidSelectIds.Value;
            char[] split = { ';' };
            var nums = ids.Split(split);
            if (nums.Length == 0 || hashPOs == null || hashPOs.Count == 0)
            {
                return;
            }
            PurchaseRequestCommon.UpdatePRTable(nums, requestIds, hashPOs);
        }

    }
}