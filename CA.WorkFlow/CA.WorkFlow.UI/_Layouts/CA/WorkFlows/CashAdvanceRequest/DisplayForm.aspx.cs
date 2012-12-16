using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.CashAdvanceRequest
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.TaskTrace1.Applicant = fields["Applicant"].ToString();
        }
    }
}