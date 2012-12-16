using System;
using System.Web.UI.WebControls;

namespace CA.Web
{
	/// <summary>
	/// �ͻ�����Ļ�ֱ����жϿؼ�
	/// 
	/// �˿ؼ�Ӧ�ñ��������û����ܵ�һ�η��ʵ�ҳ�棨һ��Ϊ��½ҳ�棩
	/// ��ҳ��������ʹ��
	/// 
	/// 2006-6-9 
	/// </summary>
	public class ClientScreenAdapter : System.Web.UI.Control
	{
		public ClientScreenAdapter()
		{
		}

		private const string  ClientScreenInfoSessionKey = "__ClientScreenInfo" ;

        private const string Js = @"<script language=""javascript"">
			if( window.location.href.indexOf(""?"") == -1 )
				window.location.href = window.location.href + ""?__ClientScreenInfo="" +  window.screen.width + ""-"" + window.screen.height ;
			else
				window.location.href = window.location.href + ""&__ClientScreenInfo="" +  window.screen.width + ""-"" + window.screen.height ;
		</script>";

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			if( Page.Session[ ClientScreenInfoSessionKey ] == null )
			{
				if( Page.Request.QueryString["__ClientScreenInfo"] != null )
				{
					Page.Session[ ClientScreenInfoSessionKey ] = Page.Request.QueryString["__ClientScreenInfo"] ;
				}
				else
				{
//                    string js = @"<script language=""javascript"">
//			if( window.location.href.indexOf(""?"") == -1 )
//				window.location.href = window.location.href + ""?__ClientScreenInfo="" +  window.screen.width + ""-"" + window.screen.height ;
//			else
//				window.location.href = window.location.href + ""&__ClientScreenInfo="" +  window.screen.width + ""-"" + window.screen.height ;
//		</script>";
				
					Page.Response.Clear();
					Page.Response.Write( Js ) ;
					Page.Response.End() ;			
				}
			}
		}

		/// <summary>
		/// ��ȡ�ͻ�����Ļ�ֱ���:800-600 1024-768 1280-1024 ...
		/// </summary>
		public static string ClientScreenInfo
		{
			get
			{
				object info = System.Web.HttpContext.Current.Session[ ClientScreenInfoSessionKey ] ;

                if (info == null) //���ֱ�����Ϣ�����ڣ���ǿ���ض���
                {
                    if (System.Web.HttpContext.Current.Request.QueryString["__ClientScreenInfo"] != null)
                    {
                        System.Web.HttpContext.Current.Session[ClientScreenInfoSessionKey] =
                            System.Web.HttpContext.Current.Request.QueryString["__ClientScreenInfo"];

                        return System.Web.HttpContext.Current.Request.QueryString["__ClientScreenInfo"];
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.Write(Js);
                        System.Web.HttpContext.Current.Response.End();
                    }

                    return "1024-768";
                }
                else
                    return info.ToString();
			}
		}


	}
}
