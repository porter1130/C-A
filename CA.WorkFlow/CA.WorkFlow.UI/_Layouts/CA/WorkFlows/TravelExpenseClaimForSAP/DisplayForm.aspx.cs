using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.TravelExpenseClaimForSAP
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TaskTrace1.Applicant = TravelExpenseClaimForSAPCommon.ReturnApplicant(WorkflowContext.Current.DataFields["TCWorkflowNumber"].AsString());
        }
    }
}