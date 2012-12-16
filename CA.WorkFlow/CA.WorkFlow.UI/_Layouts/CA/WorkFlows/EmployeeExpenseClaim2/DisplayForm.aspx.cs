using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;
namespace CA.WorkFlow.UI.EmployeeExpenseClaim2
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].ToString();
            this.DataForm1.DisplayMode = "Display";
            this.TaskTrace1.Applicant = fields["RequestedBy"].AsString()==""?fields["Applicant"].AsString():fields["RequestedBy"].AsString();
            this.DataForm1.Step = "DisplayStep";
            this.DataForm1.SummaryExpenseType = fields["SummaryExpenseType"] == null ? "" : fields["SummaryExpenseType"].ToString();
        }
    }
}