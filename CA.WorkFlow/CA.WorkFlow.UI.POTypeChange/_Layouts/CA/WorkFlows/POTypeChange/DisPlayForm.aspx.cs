using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.WorkFlow.UI.POTypeChange
{
    public partial class DisPlayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataView1.isDisplayStep = true;
        }
    }
}