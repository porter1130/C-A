using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.PurchaseRequestGeneral
{
    public partial class DataView : BaseWorkflowUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (fields["IsIncurred"] != null)
            {
                if (fields["IsIncurred"].ToString() == "True")
                {
                    PanelIncurred.Visible = true;
                }
                else
                {
                    PanelIncurred.Visible = false;
                }
            }
        }

        public void ShowInernalNO()
        {
            InernalNO.Visible = true;
        }
        public void SetInerNaNODisplay()
        {
            InernalNO.Visible = true;
            FormFieldInernalNO.ControlMode = Microsoft.SharePoint.WebControls.SPControlMode.Display;
        }
    }
}