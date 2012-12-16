using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using Microsoft.SharePoint;
namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.TaskTrace1.Applicant = fields["Applicant"].ToString();
            this.DataForm1.RequestId = fields["WorkflowNumber"].ToString();
            this.DataForm1.DataDataFields(fields);
            this.DataForm1.Step = "DisplayStep";
            //this.DataForm1.SummaryExpenseType = fields["SummaryExpenseType"].ToString();
        }
    }
}