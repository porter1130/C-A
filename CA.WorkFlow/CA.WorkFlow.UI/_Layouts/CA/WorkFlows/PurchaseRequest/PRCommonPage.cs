namespace CA.WorkFlow.UI.PurchaseRequest
{

    class PRCommonPage : CAWorkFlowPage
    {
        protected void CheckSecurity()
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }
        }

        //protected void CheckAccount()
        //{
        //    //门店和HO可以打开页面
        //    var current = SPContext.Current.Web.CurrentUser.LoginName;
        //    if (PurchaseRequestCommon.IsStore(current))
        //    {
        //        DataForm1.DisplayMode = string.Empty;
        //    }
        //    else if (PurchaseRequestCommon.IsHO(current))
        //    {
        //        DataForm1.DisplayMode = "Display";
        //    }
        //    else
        //    {
        //        RedirectToTask();
        //    }
        //}
    }
}