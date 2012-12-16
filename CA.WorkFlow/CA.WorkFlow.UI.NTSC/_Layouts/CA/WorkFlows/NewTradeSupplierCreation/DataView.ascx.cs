﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using QuickFlow.Core;
using CA.SharePoint;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.NTSC
{
    public partial class DataView : BaseWorkflowUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.lbDepartment.Text = fields["Department"].AsString();
                this.lbApplicant.Text = fields["Applicant"].AsString();
                this.lblSupplierName.Text = fields["SupplierName"].AsString();
                //this.lblSubDivision.Text = fields["SubDivision"].AsString();
                this.lblReason.Text = fields["Reason"].AsString();
                this.lblIsMondial.Text = fields["IsMondial"].AsString() == "True" ? "YES" : "NO";
            }
        }
    }
}