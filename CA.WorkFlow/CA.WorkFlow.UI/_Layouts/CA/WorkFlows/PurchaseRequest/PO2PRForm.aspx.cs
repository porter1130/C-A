namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint;

    public partial class PO2PRForm : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
        }

        private void CheckAccount()
        {
            //HO,Legal可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_HO", "wf_Legal" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }

    }
}