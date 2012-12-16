namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using Microsoft.Office.Core;
    using System.Web.UI.WebControls;

    public partial class DisplayForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.hfNoticeStatus.Value = "FromIMG";
            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"])
            {
                this.hfNoticeStatus.Value = "FromPO";
            }

            this.DataView1.Step = "DisplayStep";

            this.DataView1.Wfstep = "DisplayStep";

            this.DataView1.RequestId = WorkflowContext.Current.DataFields["SubPRNo"].ToString();
            Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();

            this.ApplicantLabel.Text = this.Applicant;
            if (!this.Page.IsPostBack) 
            {
                this.Trace1.GridLines = System.Web.UI.WebControls.GridLines.Horizontal;
                this.Trace1.BorderStyle = BorderStyle.Solid;
            }
        }

        public string Applicant
        {
            get { return this.ViewState["Applicant"].AsString(); }
            set { this.ViewState["Applicant"] = value;  }
        }
    }
}