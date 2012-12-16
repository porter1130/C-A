using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    public partial class Summary : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPostToSAP_Click(object sender, EventArgs e)
        {
            this.DataForm1.PostToSAP();
        }
    }
}