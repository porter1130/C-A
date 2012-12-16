using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.Common.CustomAction.DownLoadDocumentFiles
{
    public class CACheckBoxControl : BaseFieldControl
    {
        protected override void CreateChildControls()
        {
            Label lb = new Label();
            this.Controls.Add(lb);
        }
    }
}