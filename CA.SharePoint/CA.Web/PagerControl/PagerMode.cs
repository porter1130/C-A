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
	/// ��ҳ�ؼ�ģʽ
	/// </summary>
	public enum TPagerMode
	{
		 
		/// <summary>
		/// Ĭ��
		/// </summary>
		Default = 0,
 
		/// <summary>
		/// ��С
		/// </summary>
		NextPrev = 1,

		/// <summary>
		/// ��������ģʽ
		/// </summary>
		NumericPages = 2 ,

	    //�������й���
		//Advanced = 3 ,
		
		/// <summary>
		/// ��׼�ؼ�
		/// </summary>
		Standard = 3 ,

	}

	public enum DisplayMode
	{
		Always = 0 ,

		AutoHidden = 1 ,

		AutoHiddenBeforePost = 2




	}
}
