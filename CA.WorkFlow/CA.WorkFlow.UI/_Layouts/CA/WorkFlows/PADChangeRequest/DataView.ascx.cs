using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest
{
    public partial class DataView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.Trace1.GridLines = GridLines.Horizontal;
                this.Trace1.BorderStyle = BorderStyle.Solid;
            }
        }
    }
}