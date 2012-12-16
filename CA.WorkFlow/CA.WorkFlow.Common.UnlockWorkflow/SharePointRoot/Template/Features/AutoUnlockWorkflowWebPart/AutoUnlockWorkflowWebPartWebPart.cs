using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.Common.UnlockWorkflow.UI.WebControls.WebParts
{
    [ToolboxItemAttribute(false)]
    [Guid("c6a5126f-2978-4935-9fd8-26a09629ff3c")]
    public class AutoUnlockWorkflowWebPartWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private const string ASCX_PATH = @"~/_CONTROLTEMPLATES/CA.WorkFlow.Common.UnlockWorkflow/AutoUnlockWorkflowWebPartUserControl.ascx";

        private bool _error = false;


        public AutoUnlockWorkflowWebPartWebPart()
        {
        }

        /// <summary>
        /// Create all your controls here for rendering.
        /// Try to avoid using the RenderWebPart() method.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!_error)
            {
                try
                {

                    base.CreateChildControls();

                    Control control = this.Page.LoadControl(ASCX_PATH);
                    Controls.Add(control);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Ensures that the CreateChildControls() is called before events.
        /// Use CreateChildControls() to create your controls.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    this.EnsureChildControls();

                    // Your code here...
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Clear all child controls and add an error message for display.
        /// </summary>
        /// <param name="ex"></param>
        private void HandleException(Exception ex)
        {
            this._error = true;
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }
    }
}
