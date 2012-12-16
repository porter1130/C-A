using System;

namespace CA.Web.UploadControl
{
	/// <summary>
	/// PostedFileException ��ժҪ˵����
	/// </summary>
	public class PostedFileException : Exception 
	{
		public PostedFileException( string message ) : base ( message ) 
		{
 
		}


	}


	public enum	PostedFileExceptionType
	{
		ContentType ,
		Size ,
	}
}
