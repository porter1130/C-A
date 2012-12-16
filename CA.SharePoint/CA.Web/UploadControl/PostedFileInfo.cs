using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.UploadControl
{
	/// <summary>
	/// �ϴ�������Ϣ
	/// </summary>
	public class PostedFileInfo
	{
		public PostedFileInfo()
		{
 
		}

		private System.Web.HttpPostedFile _PostedFile ;
		/// <summary>
		/// �ϴ�����
		/// </summary>
		public System.Web.HttpPostedFile PostedFile
		{
			get{
				return _PostedFile ;
			}
		}

		private string _CustomName ;
		/// <summary>
		/// �����Զ�����
		/// </summary>
		public string CustomName
		{
			get
			{
				return _CustomName ;
			}
		}
		
		/// <summary>
		/// ���ø�����
		/// </summary>
		/// <param name="customName"></param>
		internal void SetCustomName( string customName )
		{
			_CustomName = customName ;		
		}
		
		/// <summary>
		/// ���ø���
		/// </summary>
		/// <param name="postedFile"></param>
		internal void SetPostedFile( System.Web.HttpPostedFile postedFile )
		{
			_PostedFile = postedFile ;

			if( _PostedFile != null )
			{
				_FileName = _PostedFile.FileName ;
				_ContentLength = _PostedFile.ContentLength ;
				_ContentType = _PostedFile.ContentType ;
			}
		}


		private string _FileName ;
		/// <summary>
		/// ����ȫ��
		/// </summary>
		public string FileName
		{
			get
			{
				return _FileName ;
			}
		}

		private string _ContentType ;
		/// <summary>
		/// ��������
		/// </summary>
		public string ContentType
		{
			get
			{
				return _ContentType ;
			}
		}

		private int _ContentLength ;
		/// <summary>
		/// ������С���ֽڣ�
		/// </summary>
		public int ContentLength
		{
			get
			{
				return _ContentLength ;
			}
		}

 

	}
}
