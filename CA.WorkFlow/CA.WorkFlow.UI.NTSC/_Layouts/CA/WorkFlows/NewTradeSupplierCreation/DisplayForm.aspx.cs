using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.NTSC
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.TaskTrace.Applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();
            }
        }
    }
}