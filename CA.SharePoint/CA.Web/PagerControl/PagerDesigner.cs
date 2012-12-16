// --------------------------------------------------------------------
// - ��Ȩ����  beyondbit.com
// - ���ߣ�    �Ž���        Email:jianyi0115@163.com
// - ������    2005.11.18
// - ���ģ�
// - ��ע��    
// --------------------------------------------------------------------

using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.IO;

namespace CA.Web
{
	/// <summary>
	/// TPagerDesigner ��ժҪ˵����
	/// </summary>
	public class TPagerDesigner : System.Web.UI.Design.ControlDesigner
	{
		public TPagerDesigner()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


		private Pager _pager ;

		/// <summary>
		/// ��ʼ��
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component) 
		{
			_pager = (Pager)component;
			base.Initialize(component);
		}


		/// <summary>
		/// ��ȡ���ʱhtml
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml()
		{
			StringWriter sw = new StringWriter();

			HtmlTextWriter htw = new HtmlTextWriter(sw);

			_pager.DisplayMode = DisplayMode.Always ; //ȷ�����ģʽ�¿ؼ�ʼ����ʾ

			_pager.RenderControl( htw );
			return sw.ToString() ;
			 
		}
	}
}
