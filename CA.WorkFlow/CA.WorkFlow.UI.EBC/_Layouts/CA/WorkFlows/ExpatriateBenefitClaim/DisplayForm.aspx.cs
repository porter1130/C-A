using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.EBC
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e) 
        {
            if (!this.IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                DataForm.SummaryExpenseType = fields["SummaryExpenseType"].AsString();
                DataForm.ExpatriateBenefitForm = fields["ExpatriateBenefitForm"].AsString();
                this.TaskTrace.Applicant = fields["Applicant"].ToString();
                this.DataForm.Step = "DisplayStep";
            }
        }
    }
}