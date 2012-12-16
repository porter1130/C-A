using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.Common.CustomAction.DownLoadDocumentFiles
{
    public class CACheckBoxField:SPFieldText
    {
       public CACheckBoxField(SPFieldCollection fields, string sFieldName)
           : base(fields, sFieldName)
       {
       }
       public CACheckBoxField(SPFieldCollection fields, string typeName, string displayName)
           : base(fields, typeName, displayName)
        {
        }
        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl bfc = new CACheckBoxControl();
                bfc.FieldName = this.InternalName;
                return bfc;

            }
        }

    }
}