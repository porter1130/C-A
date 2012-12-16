// --------------------------------------------------------------------
// - ��Ȩ����  beyondbit.com
// - ���ߣ�    �Ž���        Email:jianyi0115@163.com
// - ������    2005.11.18
// - ���ģ�
// - ��ע��    
// --------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using CA.Web.TreeControl.TreeGraphHelpers ;

namespace CA.Web.TreeControl
{
	/// <summary>
	/// ��֯����ͼ
	/// </summary>
	[Designer( typeof(TreeGraphDesigner) )]
	[ToolboxData("<{0}:TreeGraph runat=server></{0}:TreeGraph>")]
	[ParseChildren( true , "ChildNodes" )]
	public class TreeGraph : System.Web.UI.WebControls.WebControl 
	{
		public TreeGraph()
		{
		 
		}

		#region ��������

		//internal string LineColorHtml = "black";
		private Color _LineColor = Color.Black ;
		public System.Drawing.Color LineColor
		{
			set
			{
				_LineColor = value ;
				
			}
			get
			{
				return _LineColor ;
			}
		}

		private string _NodeRegionCssClass ;
		public string NodeRegionCssClass
		{
				set
				{
					_NodeRegionCssClass = value ;
				}
				get
				{
					return _NodeRegionCssClass ;
				}
		}	

		private Unit _LineLength = new Unit("20px") ;
		public Unit LineLength
		{
			set
			{
				_LineLength = value ;
			}
			get
			{
				return _LineLength ;
			}
		}	


		private LayoutMode _LayoutMode = LayoutMode.Vertical ;
		public LayoutMode LayoutMode
		{
			set
			{
				_LayoutMode = value ;
			}
			get
			{
				return _LayoutMode ;
			}
		}	
		
		#endregion


		private TreeNodeCollection _ChildNodes = new TreeNodeCollection()   ;
		/// <summary>
		/// ��ȡ�ӽڵ�
		/// </summary>
		[Bindable(false)]
		public TreeNodeCollection ChildNodes
		{
			//set{ _ChildNodes = value ; }
			get{ return _ChildNodes ;}
		}


		/// <summary>
		/// ���html
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			TreeGraphRender  r = null ; 

			if( _LayoutMode == LayoutMode.Vertical )
				r = new VerticalRender( this ) ; 
			else
				r = new HorizontalRender( this ) ; 


			this.RenderBeginTag( writer ) ;

			r.Render( writer ) ;

			this.RenderEndTag( writer ) ;
		}
	}

	/// <summary>
	/// ���з���
	/// </summary>
	public enum LayoutMode
	{
		/// <summary>
		/// ������
		/// </summary>
		Vertical ,

		/// <summary>
		/// ������
		/// </summary>
		Horizontal
	}

}
