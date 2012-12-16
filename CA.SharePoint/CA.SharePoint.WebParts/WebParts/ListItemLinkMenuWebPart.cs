using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace CA.SharePoint
{
    /// <summary>
    /// ���б�����Ӳ˵�
    /// </summary>
    public class ListItemLinkMenuWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {        

        private string _NavigationUrl = "/_layouts/CA/soundmail.aspx";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        public string NavigationUrl
        {
            get { return _NavigationUrl; }
            set { _NavigationUrl = value; }
        }

        

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //base.Render(writer);
            writer.Write("\n<script language=\"javascript\">\n");
            writer.Write("function Custom_AddDocLibMenuItems(m, ctx){\n");
            writer.Write("var strDisplayText = '"+ this.Title +"';    \n");     // �˵������ʾ����

            writer.Write("var strAction=\"window.location='" + this.NavigationUrl + "?ListId='+ ctx.listName +'&ItemId='+currentItemID;\" ; \n");        // �˵����ʵ�ʹ���

            writer.Write("var strImagePath = '';\n");        // �˵������ʾͼƬ 

            writer.Write("CAMOpt(m, strDisplayText, strAction, strImagePath);\n");

            // ���һ���ָ���
            writer.Write("CAMSep(m);\n");


            // ���Ϊtrue������ʾϵͳĬ�ϵĲ˵���
            // ���Ϊfasle,��ʾϵͳĬ�ϵĲ˵���

            writer.Write("return false;}\n");

            writer.Write("</script>\n");
        }


    }

   
}
