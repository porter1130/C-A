using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Specialized; 
using System.Xml;
using CA.SharePoint.WebPartSkin;

namespace CA.SharePoint.EditParts 
{
    /// <summary>
    ///  ��ѯ�ֶα༭�ؼ�
    /// </summary>
    public class ListQueryFieldEditorPart : EditorPart
    {
        public ListQueryFieldEditorPart()
        {

        }
 

        private FieldSelector _FieldSelector = new FieldSelector();

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Title = WPResource.GetString("ListFieldEditorPart_QueryFieldEdit");
           
            //ListQueryConditionWebPart wp = (ListQueryConditionWebPart)this.WebPartToEdit;

            //_FieldSelector.SetSelectedFields(wp.QueryFields, wp.List.Fields);
            this.Controls.Add(_FieldSelector);

            this.ChildControlsCreated = true;
        }
              

        public override bool ApplyChanges()
        {
            this.EnsureChildControls();              

            ListQueryConditionWebPart wp = (ListQueryConditionWebPart)WebPartToEdit;

            wp.QueryFields = _FieldSelector.GetSelectedFields();

            wp.ReBuildQueryPanel();

            return true;
        }

        /// <summary>  
        /// ��һ�μ��ؿؼ�ʱ�������ύ�޸�ʱ���ᴥ���ķ�����
        /// </summary>
        public override void SyncChanges()
        {
            this.EnsureChildControls();    

            ListQueryConditionWebPart wp = ((ListQueryConditionWebPart)WebPartToEdit) ;

            if( wp.List != null )
                _FieldSelector.SetSelectedFields(wp.QueryFields, wp.List.Fields);

            //this.Controls.Clear();
            //BuildBySelectedFields(fields);
        }

       
         

    }
}
