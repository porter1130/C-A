using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class PRPOQuery :LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
        }
        


    }
}