using System;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.SharePoint.Utilities.Common;
using QuickFlow;
using System.Collections.Generic;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;

namespace CA.WorkFlow.UI.PaymentRequestSAP
{
    public partial class SAPDisplay : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
            if (fields["Status"].ToString() == "Completed")
            {
                this.btnClaimToSAPForm.Visible = true;
            }
            else
            {
                this.btnClaimToSAPForm.Visible = false;
            }
            this.TaskTrace1.Applicant = fields["Applicant"].ToString();
        }

    }
}