using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.IO;

namespace CA.Web.TreeControl
{
	/// <summary>
	/// TreeGraph���֧����
	/// </summary>
	public class TreeGraphDesigner : System.Web.UI.Design.ControlDesigner
	{

		private TreeGraph _Tree = null ;

		 

		/// <summary>
		/// ��ʼ��
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component) 
		{
			_Tree = (TreeGraph)component;
			base.Initialize(component);
		}


		/// <summary>
		/// ��ȡ���ʱhtml
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml()
		{
			if( _Tree.ChildNodes.Count > 0 )
				return base.GetDesignTimeHtml();

			StringWriter sw = new StringWriter();

			HtmlTextWriter htw = new HtmlTextWriter(sw);

			//Tree.RenderControl( htw );

			_Tree.RenderBeginTag( htw );

			htw.Write( "<b>"+_Tree.ID+"</b>" );

			_Tree.RenderEndTag( htw );

			return sw.ToString() ;
		}


		
	}


}
