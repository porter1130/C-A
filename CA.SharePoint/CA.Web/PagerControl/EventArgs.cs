// --------------------------------------------------------------------
// - ��Ȩ����  beyondbit.com
// - ���ߣ�    �Ž���        Email:jianyi0115@163.com
// - ������    2005.11.18
// - ���ģ�
// - ��ע��    
// --------------------------------------------------------------------
using System;

namespace CA.Web
{

	/// <summary>
	/// ��ҳ�¼�����
	/// </summary>
	public class PageChangedEventArgs : EventArgs 
	{
		/// <summary>
		/// ��һҳ����
		/// </summary>
		public int OldPageIndex;

		/// <summary>
		/// ��ҳ����
		/// </summary>
		public int NewPageIndex;
		
		/// <summary>
		/// ��¼��
		/// </summary>
		public int RecordCount ;

		/// <summary>
		/// ÿҳ��¼��
		/// </summary>
		public int PageSize ;
		
		/// <summary>
		/// ��ҳ�¼�����
		/// </summary>
		public PagerEventType EventType = PagerEventType.PageIndexChanged ;
	}


	/// <summary>
	/// ��ҳ�¼�����
	/// </summary>
	public enum PagerEventType
	{
		/// <summary>
		/// ҳ��ı��¼�
		/// </summary>
		PageIndexChanged = 0 ,

		/// <summary>
		/// ÿҳ��¼���ı��¼�
		/// </summary>
		PageSizeChanged = 1 
	}
}
