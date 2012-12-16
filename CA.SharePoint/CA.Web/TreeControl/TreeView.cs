// --------------------------------------------------------------------
// - ��Ȩ����  beyondbit.com
// - ���ߣ�    �Ž���        Email:jianyi0115@163.com
// - ������    2005.11.18
// - ���ģ�
// - ��ע��    
// --------------------------------------------------------------------

using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.TreeControl
{
	

	/// <summary>
	/// ʵ���첽���ص�TreeView���ڵ�Page����ʵ�ֵĽӿڣ������ӽڵ�������λ�ȡ
	/// </summary>
	/// <example>
	/// �첽����ʾ��:
	/// <code>
	/// public class TreeTest : System.Web.UI.Page , ITreeViewAsyncCallback 
	/// {
	///		public string TreeViewGetChildHtml( TreeNode node ) 
	///		{
	///			return "" ;
	///		}
	///
	///		public TreeNodeCollection TreeViewGetChildNodes( TreeNode parentNode )
	///		{
	///			TreeNodeCollection nodes = new TreeNodeCollection(parentNode);
	///			TreeNode node = new TreeNode();
	///			node.Text = "NodeText" ;
	///			node.Value = "NodeValue" ;
	///			node.ImageUrl = "images/tree/icon-user.gif";
	///			node.Checked = false ;
	///			node.Expand = true ;
	///			nodes.Add( node ) ;
	///			return nodes ;
	///		}
	/// }
	/// </code>
	/// </example>
	public interface ITreeViewAsyncCallback
	{
		/// <summary>
		/// ��ȡ�ӽڵ�
		/// </summary>
		/// <param name="node">���ڵ�;�ṩValue,Text����</param>
		/// <returns>�ӽڵ㼯��</returns>
		TreeNodeCollection TreeViewGetChildNodes( TreeNode node ) ;

		/// <summary>
		/// ��ȡ�ڵ�����(html)
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		string TreeViewGetChildHtml( TreeNode node ) ;

	}

	/// <summary>
	/// �첽���ط�ʽ
	/// </summary>
	public enum AsyncCallbackType
	{
		/// <summary>
		/// �����ӽڵ�
		/// </summary>
		GetChildNodes = 0 ,

		/// <summary>
		/// ���������html
		/// </summary>
		GetChildHtml = 1 
	}

	/// <summary>
	/// �ڵ㴴���¼�����
	/// </summary>
	public class TreeNodeCreatedEventArgs : EventArgs 
	{
		internal TreeNodeCreatedEventArgs(  TreeNode node )
		{
			Node = node;
		}
		/// <summary>
		/// �������Ľڵ�
		/// </summary>
 		public readonly TreeNode Node ;
	}

	/// <summary>
	/// �ڵ㴴���¼�����
	/// </summary>
	public delegate void TreeNodeCreatedEventHandler( object sender , TreeNodeCreatedEventArgs args );

	/// <summary>
	/// 
	///  web���ؼ���1.0
	///  
	///  @author jianyi015@163.com
	///  
	///  2005-12-10 ���
	///  12-13 ���� TreeNodeCreated �¼�
	///  
	///  �������¹���:
	///  1.��ѡ����ѡ���̳�ѡ��Ŀͻ���֧��
	///  2.�첽�����ӽڵ㹦��
	///  3.�����첽����html
	///  
	///  
	///  Ϊ������ܣ��ؼ���ViewState���ܣ����ط�ҳ����Ҫ����TreeView��״̬����Ҫ������ƣ�
	///  �˿ؼ�ʮ���ʺ�ҳ���޻ط���������絯����ѡ�������ڣ���̬���صĵ�����Ŀ¼��
	///  
	///  ע����һ��ҳ��ֻ֧��һ��TreeView 
	///  
	///  ���ǵ����������£�һ��ҳ��ֻ��һ�����ؼ���
	///  Ҫ֧��һ��ҳ�����ؼ�����Ҫ����ӦԪ�������������������������ݴ�������
	///  �����첽������ɸ�����
	///  
	/// </summary>
	[ToolboxData("<{0}:TreeView runat=server></{0}:TreeView>")]
	[Designer( typeof(TreeViewDesigner) )]
	[ParseChildren( true , "ChildNodes" )]
	[DefaultEvent( "TreeNodeCreated" )]
	public class TreeView : System.Web.UI.WebControls.WebControl , System.Web.UI.IPostBackDataHandler
	{
		/// <summary>
		/// 
		/// </summary>
		public TreeView()
		{
			this._ChildNodes = new TreeNodeCollection( this , null ) ;
		}

		/// <summary>
		/// �ڵ㴴���¼�
		/// </summary>
		public event TreeNodeCreatedEventHandler TreeNodeCreated ;

		/// <summary>
		/// ����TreeNodeCreated�¼�
		/// </summary>
		/// <param name="node"></param>
		internal void OnTreeNodeCreated( TreeNode node )
		{
			if( TreeNodeCreated != null )
				RaiseTreeNodeCreatedEvent(  node ) ;
		}

		/// <summary>
		/// ����TreeNodeCreated�¼�
		/// </summary>
		/// <param name="nodes"></param>
		internal void OnTreeNodeCreated( TreeNodeCollection nodes )
		{
			if( TreeNodeCreated != null )
			{
				foreach( TreeNode n in nodes )
					RaiseTreeNodeCreatedEvent(  n ) ;
			}
		}
		
		/// <summary>
		/// ����TreeNodeCreated�¼�,�ڹ������¼�
		/// </summary>
		/// <param name="node"></param>
		private void RaiseTreeNodeCreatedEvent( TreeNode node )
		{
			node.SetOwner( this );
			TreeNodeCreated( this , new TreeNodeCreatedEventArgs( node ) ) ;

			foreach( TreeNode n in node.ChildNodes )
			{
				n.SetOwner( this );
				RaiseTreeNodeCreatedEvent( n ) ;
			}
		}

		private string _ImageFolderUrl = "~/images/tree/" ; 
		/// <summary>
		/// ����ͼƬ�ļ�·��
		/// </summary>
		[Editor( typeof( System.Web.UI.Design.UrlEditor ) , typeof(System.Drawing.Design.UITypeEditor) ),
		Category("Settings"),
		Description("����ͼƬ�ļ�·��")]
		public string ImageFolderUrl
		{
			set
			{ 
				if( value.EndsWith("/") )
					_ImageFolderUrl = value ;
				else
					_ImageFolderUrl = value + "/" ; 
			}
			get
			{ 
				return _ImageFolderUrl ;
			}
		}
		
		//private Hashtable _ValueList = new Hashtable();
		private string _CheckedNodeValueList = "" ; 
		/// <summary>
		/// ��ȡѡ�ֵĽڵ�ֵ�б�
		/// </summary>
		[Category("Data"),
		Description("��ȡѡ�еĽڵ�ֵ�б�")]
		public string CheckedNodeValueList
		{
//			set
//			{ 
//				_CheckedNodeValueList = value ;
//			}
			get
			{ 
				return _CheckedNodeValueList ;  
			}
		}

		private AsyncCallbackType _AsyncCallbackType = AsyncCallbackType.GetChildNodes ;

		/// <summary>
		/// �첽���ط�ʽ
		/// </summary>
		[Category("Behavior"),
		Description("�첽���ط�ʽ")]
		public AsyncCallbackType AsyncCallbackType
		{
			set{ _AsyncCallbackType = value ; }
			get{ return _AsyncCallbackType; }
		}

		private bool _IsCallback = false ;
		/// <summary>
		/// �ؼ��Ƿ����첽�ش�״̬
		/// </summary> 
		[Bindable(false),
		Description("�Ƿ����첽�ش�״̬")]
		public bool IsCallback
		{
			get{ return _IsCallback ;}
		}  

		private bool _IsCallTarget = false ;
		/// <summary>
		/// �ؼ��Ƿ����첽�ش�״̬
		/// </summary> 
		[Bindable(false),
		Description("�ؼ��Ƿ����첽�ش�״̬")]
		public bool IsCallTarget
		{
			get{ return _IsCallTarget ;}
		}  

		private bool _ShowLines = false ; 
		/// <summary>
		/// �Ƿ���ʾ�������½ڵ����
		/// </summary>
		[Category("Appearance"),
		Description("�Ƿ���ʾ�������½ڵ����")]
		public bool ShowLines
		{
			set{ _ShowLines = value ; }
			get{ return _ShowLines ;}
		}  

		private bool _EnableAsyncLoad = false ; 
		/// <summary>
		/// �Ƿ����ö�̬���أ������ã���Page����ʵ��ITreeViewAsyncCallback�ӿ�
		/// </summary>
		[Category("Behavior"),
		Description("�����ö�̬���أ������ã���Page����ʵ��ITreeViewAsyncCallback�ӿ�")]
		public bool EnableAsyncLoad
		{
			set{ _EnableAsyncLoad = value ; }
			get{ return _EnableAsyncLoad ;}
		}  

		private bool _AutoLoadRootNodes = true ;
		[Category("Behavior"),
		Description("�Ƿ��Զ����ظ��ڵ�")]
		public bool AutoLoadRootNodes
		{
			set{ _AutoLoadRootNodes = value ; }
			get{ return _AutoLoadRootNodes ;}
		}  

		private string _CheckboxClickHtml = "" ;
		private string _CheckboxClicked = "" ;
		private bool _MultiSelect = true ; 
		/// <summary>
		/// �Ƿ������ѡ
		/// </summary>
		[Category("Behavior"),
		Description("�Ƿ������ѡ")]
		public bool MultiSelect
		{
			set
			{ 
				_MultiSelect = value ; 
			}
			get{ return _MultiSelect ;}
		}  

		private string _AsyncLoadMessage = " ���ڼ���...";
		/// <summary>
		/// �첽����ʱ��Ϣ
		/// </summary>
		[Category("Settings"),
		Description("�첽����ʱ��Ϣ")]
		public string AsyncLoadMessage
		{
			set{ _AsyncLoadMessage = value ; }
			get{ return _AsyncLoadMessage ;}
		}  

		private string _AsyncLoadErrMessage = "";
		/// <summary>
		/// �첽���ش���ʱ��Ϣ,��δ��������ʾ��ϸ��Ϣ
		/// </summary>
		[Category("Settings"),
		Description("�첽���ش���ʱ��Ϣ,��δ��������ʾ������ϸ��Ϣ")]
		public string AsyncLoadErrMessage
		{
			set{ _AsyncLoadErrMessage = value ; }
			get{ return _AsyncLoadErrMessage ;}
		}  


		private bool _AutoSelectChildNodes = false ; 
		/// <summary>
		/// ��ѡ״̬�£��Ƿ��Զ�ѡ���ӽڵ�
		/// </summary>
		[Category("Behavior"),
		Description("��ѡ״̬�£��Ƿ��Զ�ѡ���ӽڵ�")]
		public bool AutoSelectChildNodes
		{
			set{ _AutoSelectChildNodes = value ; }
			get{  return _AutoSelectChildNodes ; }
		}  

		private bool _AutoSelectParentNodes = false ;
		/// <summary>
		/// �Ƿ��Զ�ѡ�и��ڵ�
		/// </summary>
		[Category("Behavior"),
		Description("��ѡ״̬�£��Ƿ��Զ�ѡ�и��ڵ�")]
		public bool AutoSelectParentNodes
		{
			set{ _AutoSelectParentNodes = value ; }
			get{ return _AutoSelectParentNodes ; }
		}

		private string _OnAsyncLoadComplete;
		/// <summary>
		/// �ڵ��첽�����ӽڵ�󴥷��Ŀͻ���js���� OnAsyncLoadComplete
		/// </summary>
		[Category("Behavior")]
		[Description("�첽�����ӽڵ�����󴥷���js����")]
		public string OnAsyncLoadComplete 
		{
			set{ _OnAsyncLoadComplete = value ; }
			get{ return _OnAsyncLoadComplete ;}
		}

		private string _OnAsyncLoad;
		/// <summary>
		/// �ڵ��첽�����ӽڵ�󴥷��Ŀͻ���js���� OnAsyncLoadComplete
		/// </summary>
		[Category("Behavior")]
		[Description("�첽�����ӽڵ㿪ʼʱ������js����")]
		public string OnAsyncLoad 
		{
			set{ _OnAsyncLoad = value ; }
			get{ return _OnAsyncLoad ;}
		}

		private TreeNodeCollection _ChildNodes   ;
		/// <summary>
		/// ��ȡ�ӽڵ�
		/// </summary>
		[Bindable(false)]
		public TreeNodeCollection ChildNodes
		{
			//set{ _ChildNodes = value ; }
			get{ return _ChildNodes ;}
		}

		private void InitNodeJs()
		{
			_CheckboxClicked = this.ClientID + "_NodeClick(this)";
			_CheckboxClickHtml = "onclick=\""+_CheckboxClicked+"\"";
		}

		/// <summary>
		/// ѡ���Ҫ�Ľű�
		/// </summary>
		private void InitModeJs()
		{			
			string nodeClickJs = "<script language='javascript'>\n";

			nodeClickJs += "function " + this.ClientID + "_NodeClick(obj){\n";

			if( _MultiSelect == false )
			{
				nodeClickJs += "TreeView_SingleSelect('"+this.ClientID+"',obj);\n";
			}
			else
			{
				if( _AutoSelectChildNodes )
				{
					nodeClickJs += "TreeView_SelectChildNodes(obj);\n";
				}

				if( this._AutoSelectParentNodes )
				{
					nodeClickJs += "TreeView_SelectParentNodes('"+this.ClientID+"',obj);\n";
				}
			}

			nodeClickJs += "\n}</script>";

			Page.RegisterClientScriptBlock( this.ClientID , nodeClickJs ) ;


//			if( _MultiSelect == false )
//			{
//				_CheckboxClicked = "TreeView_SingleSelect('"+this.ClientID+"',this)";
//				_CheckboxClickHtml = "onclick=\"TreeView_SingleSelect('"+this.ClientID+"',this);\"";
//			}
//			else
//			{
//				if( _AutoSelectChildNodes )
//				{
//					_CheckboxClicked = "TreeView_SelectChildNodes(this);";
//					_CheckboxClickHtml = "onclick=\"TreeView_SelectChildNodes(this);\"";
//				}
//			}
		}

		/// <summary>
		/// ��ʼ�������ؼ��첽�ش��������Page ITreeViewAsyncCallback �ӿڵ�GetChildNode���������Խڵ�����
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			if( Page.Request.QueryString["TreeView_ClientID"] != null )
			{
				this._IsCallback = true ;
			}

			if( Page.Request.QueryString["TreeView_ClientID"] == this.ClientID )
			{
				this._IsCallTarget = true ;

				this.ImageFolderUrl = base.ResolveUrl( this.ImageFolderUrl );

				this.EnableAsyncLoad = true ;

				if( Page.Request.QueryString["TreeView_ShowLines"] == "true" )
					this._ShowLines = true ;
				else
					this._ShowLines = false ;

				if( Page.Request.QueryString["TreeView_MultiSelect"] == "true" )
					this._MultiSelect= true ;
				else
					this._MultiSelect = false ;

				if( Page.Request.QueryString["TreeView_AutoSelectChildNodes"] == "true" )
					this._AutoSelectChildNodes = true ;
				else
					this._AutoSelectChildNodes = false ;

				ITreeViewAsyncCallback c ;

				if( this is ITreeViewAsyncCallback )
					c = this as ITreeViewAsyncCallback ;
				else if( Page  is ITreeViewAsyncCallback   )
					c = Page as ITreeViewAsyncCallback ;
				else
					throw new Exception( "Page û��ʵ�� ITreeViewAsyncCallback �ӿڣ������첽��������" );

				//TreeNodeCollection nodes = c.GetChildNode( new TreeNode( Page.Request.QueryString["TreeViewLoadValue"] , "" ) ) ;

				TreeNode parentNode = null ;

				if( Page.Request.QueryString["TreeView_LoadRoot"] != "true" )
				{
					parentNode = new TreeNode( Page.Request.QueryString["TreeView_NodeValue"] , Page.Request.QueryString["TreeView_NodeText"] );
				
					parentNode.SetOwner( this ) ;

					if( Page.Request.QueryString["TreeView_NodeChecked"] == "true" )
						parentNode.Checked = true ;

					//parentNode.Id = Page.Request.QueryString["TreeView_NodeID"] ;
				}

				Page.Response.Clear() ;

				//Page.Response.Write( Page.Request.QueryString.ToString() );

				if( _AsyncCallbackType == AsyncCallbackType.GetChildNodes )
				{
					this._ChildNodes = c.TreeViewGetChildNodes( parentNode ) ;

					OnTreeNodeCreated( _ChildNodes ) ;

					HtmlTextWriter writer = new HtmlTextWriter( Page.Response.Output ) ;

					RenderNodes( writer ) ;
				}
				else
				{
					Page.Response.Write( c.TreeViewGetChildHtml( parentNode ) ) ;
				}

				//Page.Response.Flush();
		 
				Page.Response.End();
			}
		}

		private const string ClientJsKey = "TreeView-ClientJs";

		/// <summary>
		/// ��ȡ��������js--ͨ��js
		/// </summary>
		/// <returns></returns>
		private string GetSettingJs()
		{
			string settingJs = "<SCRIPT lang='javascript'>" ;

			settingJs += "\n var TreeView_EnableAsyncLoad = " + this.EnableAsyncLoad.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_MultiSelect = " + this.MultiSelect.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_AutoSelectChildNodes = " + this.AutoSelectChildNodes.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_ShowLines = " + this.ShowLines.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_NodeImgUrl = '"+this.ImageFolderUrl+"node.gif';" ;
			settingJs += "\n var TreeView_EndNodeImgUrl = '"+this.ImageFolderUrl+"end.gif';" ;

			settingJs += "\n var TreeView_AsyncLoadMessage = \"" + this.AsyncLoadMessage.ToString() + "\" ;" ;
			settingJs += "\n var TreeView_AsyncLoadErrMessage = \"" + this.AsyncLoadErrMessage.ToString() + "\" ;" ;

			if( this.ShowLines )
			{
				settingJs += "\n var TreeView_LineNodeClickImgUrl = '"+this.ImageFolderUrl +"centerminus.gif';" +
					"\n var TreeView_LineNodeClickExpandImgUrl = '"+this.ImageFolderUrl+"centerplus.gif';" +								
					"\n var TreeView_LastNodeClickImgUrl = '"+this.ImageFolderUrl+"endminus.gif';" + 
					"\n var TreeView_LastNodeClickExpandImgUrl = '" + this.ImageFolderUrl + "endplus.gif';";
			}
			else
			{
				settingJs += "\n var TreeView_LineNodeClickImgUrl = '"+this.ImageFolderUrl +"minus.gif';" +
					"\n var TreeView_LineNodeClickExpandImgUrl = '"+this.ImageFolderUrl+"plus.gif';" +								
					"\n var TreeView_LastNodeClickImgUrl = '"+this.ImageFolderUrl+"endminus.gif';" + 
					"\n var TreeView_LastNodeClickExpandImgUrl = '" + this.ImageFolderUrl + "endplus.gif';";
			}
								
			settingJs += "\n</SCRIPT>" ;

			return settingJs ;
		}

		//�������js
		//js������ClientID��ʾ����
		private string GetInstanceJs()
		{
			string instanceJs = "<SCRIPT lang='javascript'>" ;

			//_OnAsyncLoad

			instanceJs += "\n function "+this.ClientID+"_OnAsyncLoad(){ \n" ;

			
			if( this.OnAsyncLoad != null )
			{
				instanceJs += "\n try{\n";
				instanceJs += OnAsyncLoad + ";" ;
				instanceJs += "\n }catch(e){alert('OnAsyncLoad��������'+ e )}";
			}
						
			instanceJs += "\n }" ;

			//_OnAsyncLoad end

			// OnAsyncLoadComplete function
			instanceJs += "\n function "+this.ClientID+"_OnAsyncLoadComplete(){ \n" ;
			
			if( this.OnAsyncLoadComplete != null )
			{
				instanceJs += "\n try{\n";
				instanceJs += OnAsyncLoadComplete + ";" ;
				instanceJs += "\n }catch(e){alert('OnAsyncLoadComplete��������'+ e )}";
			}			
			
			instanceJs += "\n }" ;

			//OnAsyncLoadComplete end	

			//_NodeClick function
			instanceJs += "\n function " + this.ClientID + "_NodeClick(obj){\n";

			if( _MultiSelect == false )
			{
				instanceJs += "TreeView_SingleSelect('"+this.ClientID+"',obj);\n"; //��ѡjs
			}
			else
			{
				if( _AutoSelectChildNodes )
				{
					instanceJs += "TreeView_SelectChildNodes(obj);\n"; //�Ӽ̳�ѡ��js
				}

				if( this._AutoSelectParentNodes )
				{
					instanceJs += "TreeView_SelectParentNodes('"+this.ClientID+"',obj);\n"; //��ѡ��js
				}
			}

			instanceJs += "\n }" ;
			////_NodeClick function end
								
			instanceJs += "\n</SCRIPT>" ;

			return instanceJs ;
		}


		/// <summary>
		/// ��ȡ��ʽhtml
		/// </summary>
		/// <returns></returns>
		private string GetStyle()
		{			
			string style = "<style> .Tree_Line { BACKGROUND-POSITION-X: center; BACKGROUND-IMAGE: url("+this.ImageFolderUrl+"back.gif); WIDTH: 10px; BACKGROUND-REPEAT: repeat-y } </style>";
			return style ;
		}

		/// <summary>
		/// ע��ͻ��˽ű�
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			this.ImageFolderUrl = base.ResolveUrl( this.ImageFolderUrl );

			Page.RegisterRequiresPostBack( this ) ; //

//			if( Page.IsClientScriptBlockRegistered( ClientJsKey ) )
//				throw new Exception( "TreeViewֻ֧��ÿ��ҳ��һ���ؼ�" );

			Page.RegisterClientScriptBlock( ClientJsKey , ResourceHelper.GetJsResourceString( typeof(TreeView) ) ) ; //ע��ͨ��js

			Page.RegisterClientScriptBlock( ClientJsKey + "-Setting" , GetSettingJs() ) ; //ע���������js

			Page.RegisterClientScriptBlock( ClientJsKey + "-Style" ,  GetStyle() ) ; //ע��style

			//InitModeJs();

			//�Զ����ص�һ��
			if( this.EnableAsyncLoad && this.ChildNodes.Count== 0 && AutoLoadRootNodes )
			{
				Page.RegisterStartupScript( this.ClientID + "_LoadRoot" , "<SCRIPT lang=\"javascript\">TreeView_AsyncLoadRoot('"+this.ClientID+"');</SCRIPT> " ) ;
			}

			//ע������js����
			Page.RegisterClientScriptBlock( this.ClientID + "_Instance" ,  GetInstanceJs() ) ;

			//ע��checked

			//string js = "<SCRIPT lang='javascript'>" + GetResourceString( t , "js" ) + "</SCRIPT> " ;
			//js += "function TreeView_NodeChecked(this)";

		}

		/// <summary>
		///��ȡ���е���Html�������ű�.
		/// �����ǵ��������ɵ�����html�����棬���Ե��ô˷�����ȡ��ʹ�����е�����html
		/// </summary>
		/// <param name="withJs">�Ƿ�����ű�</param>
		/// <returns></returns>
		public string GetTreeHtml( bool withJs )
		{
			StringWriter tw = new StringWriter(); 
			HtmlTextWriter hw = new HtmlTextWriter(tw);

			if( withJs )
			{
				hw.WriteLine( ResourceHelper.GetJsResourceString( this.GetType() ) ) ;
				hw.WriteLine(  GetSettingJs() ) ;
				hw.WriteLine( GetStyle() ) ;
			}

			this.RenderControl(hw); 
			
			return tw.ToString( )  ;
		}

		/// <summary>
		/// ���html
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderBeginTag( writer ) ;

			RenderNodes( writer ) ;

			this.RenderEndTag( writer ) ;

		}
		/// <summary>
		/// ����ӽڵ�html
		/// </summary>
		/// <param name="writer"></param>
		private void RenderNodes( HtmlTextWriter writer )
		{
			if( ChildNodes == null || ChildNodes.Count == 0 ) return ;

			//InitModeJs() ;

			InitNodeJs();

			writer.WriteLine( "<table border='0' cellspacing=0 cellpadding=0>" );// border='1' bordercolor='#0030b0' cellspacing='0'

			if( _ShowLines )
			{
				for( int i = 0 ; i < this.ChildNodes.Count - 1 ; i ++ )
				{
					RenderLineNode( this.ChildNodes[i] , writer ) ;
				}

				RenderLastNode( this.ChildNodes[this.ChildNodes.Count - 1] , writer ) ;
			}
			else
			{
				for( int i = 0 ; i < this.ChildNodes.Count ; i ++ )
				{
					RenderNode( this.ChildNodes[i] , writer ) ;
				}

			}

			writer.WriteLine( "</table>" );
		}
		
		/// <summary>
		/// ����ڵ�ĸ�ѡ��html
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderCheckbox( TreeNode node , HtmlTextWriter writer )
		{
			if( node.EnableChecked )
			{
				writer.Write( "<input name='" + this.ClientID + "_Node' type='checkbox' value=\"" + node.Value + "\" " );

				if( node.OnCheckBoxClick != null && node.OnCheckBoxClick != "" )
				{
					//if( _CheckboxClickHtml != "" )
					writer.Write( "onclick=\"" );
					writer.Write( _CheckboxClicked );
					writer.Write( ";" );
					writer.Write( node.OnCheckBoxClick );
					writer.Write( ";\"" );
				}
				else
				{
					writer.Write( _CheckboxClickHtml );
				}

				if( node.ToolTip != null )
				{
					writer.Write( " title=\"" );
					writer.Write( node.ToolTip );
					writer.Write( "\"" );
				}

				if( node.Checked )
					writer.Write( " checked " );
				
				if( node.Disabled )
					writer.Write( " disabled " );

				//writer.Write( " ID='"+ node.Id +"'" );

				writer.Write( ">" );
			}
		}

		private void RenderNodeText( TreeNode node , HtmlTextWriter writer )
		{
			if( node.NavigateUrl != null )
			{
				writer.Write( "<a " );
				writer.Write( node.TargetHtml );
				writer.Write( " href=\"" + node.NavigateUrl + "\">" );
				writer.Write( node.Text );
				writer.Write( "</a>");
			}
			else
				writer.Write( node.Text );			 
		}

		
		private void RenderClickImage( TreeNode node , HtmlTextWriter writer )
		{
			if( node.Expand )
				writer.Write( "<image src='" + this.ImageFolderUrl + "minus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
			else
				writer.Write( "<image src='" + this.ImageFolderUrl + "plus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
		}
		
		/// <summary>
		/// �ݹ�����ڵ�html,����ʾ������
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderNode( TreeNode node , HtmlTextWriter writer )
		{
			writer.WriteLine( "<tr>" );
			writer.Write( "<td>" );
			//����Ӻ�
			if( node.ChildNodes.Count > 0 || node.HasChildNodes )
			{
				RenderClickImage( node , writer ) ;

//				if( node.Expand )
//					writer.Write( "<image src='" + this.ImageFolderUrl + "minus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
//				else
//					writer.Write( "<image src='" + this.ImageFolderUrl + "plus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
			}
			else
			{
				writer.Write( " &nbsp; " ) ;
			}
			

			writer.WriteLine( "</td>" );
			writer.Write( "<td nowrap>" );

			RenderCheckbox( node , writer ) ;

			if( node.ImageUrl != null )
				writer.Write( "<image src=" + node.ImageUrl + ">" );
								
//			if( node.NavigateUrl != null )
//			{
//				writer.Write( "<a " );
//				writer.Write( node.TargetHtml );
//				writer.Write( " href=\"" + node.NavigateUrl + "\">" );
//				writer.Write( node.Text );
//				writer.Write( "</a>");
//
//			}
//			else
//				writer.Write( node.Text );

			RenderNodeText( node , writer ) ;

			writer.WriteLine( "</td>" );
			writer.WriteLine( "</tr>" );
				
			//�ӽڵ�
			writer.WriteLine( "<tr " );
			writer.Write( node.ExpandHtml );
			writer.WriteLine( ">" );

			writer.Write( "<td width='5'>&nbsp; " );

			//writer.Write( " --" );

			writer.Write( "</td>" );
			writer.WriteLine( "<td>" );

			if( node.ChildNodes.Count > 0 )
			{
				writer.WriteLine( "<table border='0' cellspacing=0 cellpadding=0 >" );//border='1' bordercolor='#0030b0' cellspacing='0'

				foreach( TreeNode n in node.ChildNodes ) 
					RenderNode( n , writer ) ;

				writer.WriteLine( "</table>" );
			}

			writer.WriteLine( "</td>" );

			writer.WriteLine( "</tr>" );
				
		} 

		/// <summary>
		/// �ݹ�����ڵ�html,��ʾ������
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderLineNode( TreeNode node , HtmlTextWriter writer   )
		{
			writer.WriteLine( "<tr>" );
			writer.Write( "<td>" );
			//����Ӻ�
	 
			if( node.ChildNodes.Count > 0 || node.HasChildNodes )
			{
				if( node.Expand )
					writer.Write( "<image src='" + this.ImageFolderUrl + "centerminus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
				else
					writer.Write( "<image src='" + this.ImageFolderUrl + "centerplus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );

			}
			else
			{
				writer.Write( "<image src='" + this.ImageFolderUrl + "node.gif'>" );
			}
			
			writer.WriteLine( "</td>" );
			writer.Write( "<td nowrap>" );
	
			RenderCheckbox( node , writer ) ;

			if( node.ImageUrl != null )
				writer.Write( "<image src=" + node.ImageUrl + ">" );


			RenderNodeText( node , writer ) ;

			writer.WriteLine( "</td>" );
			writer.WriteLine( "</tr>" );
				
			//�ӽڵ�
			writer.WriteLine( "<tr " );
			writer.Write( node.ExpandHtml );
			writer.WriteLine( ">" );

			writer.Write( "<td class='Tree_Line'> " ); // style='background-repeat:repeat-y;BACKGROUND-POSITION-X: center;'  background='"+ImageFolderUrl+"back.gif'

			//writer.Write( " --" );

			writer.Write( "</td>" );
			writer.WriteLine( "<td>" );

			if( node.ChildNodes.Count > 0 )
			{
				writer.WriteLine( "<table  border=0 cellspacing=0 cellpadding=0>" );//border='1' bordercolor='#0030b0' cellspacing='0'

				for( int i = 0 ; i < node.ChildNodes.Count - 1 ; i ++ )
				{
					RenderLineNode( node.ChildNodes[i] , writer ) ;
				}

				RenderLastNode( node.ChildNodes[node.ChildNodes.Count - 1] , writer ) ;

				writer.WriteLine( "</table>" );
			}

			writer.WriteLine( "</td>" );

			writer.WriteLine( "</tr>" );
				
		} 

		/// <summary>
		/// �ݹ����ͬһ�������һ���ڵ�html,��ʾ������
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderLastNode( TreeNode node , HtmlTextWriter writer   )
		{
			writer.WriteLine( "<tr>" );
			writer.Write( "<td>" );
			//����Ӻ�

			if( node.ChildNodes.Count > 0 || node.HasChildNodes  )
			{
				if( node.Expand )
					writer.Write( "<image src='" + this.ImageFolderUrl + "endminus.gif'  onclick=\"TreeView_LastNodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
				else
					writer.Write( "<image src='" + this.ImageFolderUrl + "endplus.gif'  onclick=\"TreeView_LastNodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
				//writer.Write( "<image src='" + this.ImageFolderUrl + "endplus.gif'   onclick=\"TreeView_LastNodeClick(this,'"+node.Value+"')\">" );
			}
			else
				writer.Write( "<image src='" + this.ImageFolderUrl + "end.gif'>" );
					
			
			writer.WriteLine( "</td>" );
			writer.Write( "<td nowrap>" );
	
			RenderCheckbox( node , writer ) ;

			if( node.ImageUrl != null )
				writer.Write( "<image src=" + node.ImageUrl + ">" );

					
//			if( node.NavigateUrl != null )
//			{
//				writer.Write( "<a " );
//				writer.Write( node.TargetHtml );
//				writer.Write( " href=\"" + node.NavigateUrl + "\">" );
//				writer.Write( node.Text );
//				writer.Write( "</a>");
//			}
//			else
//				writer.Write( node.Text );

			RenderNodeText( node , writer ) ;

			writer.WriteLine( "</td>" );
			writer.WriteLine( "</tr>" );
				
			//�ӽڵ�
			writer.WriteLine( "<tr " );
			writer.Write( node.ExpandHtml );
			writer.WriteLine( ">" );

			writer.Write( "<td width='10'> " );

			writer.Write( "</td>" );
			writer.WriteLine( "<td>" );

			if( node.ChildNodes.Count > 0 )
			{
				writer.WriteLine( "<table  border=0 cellspacing=0 cellpadding=0>" );//border='1' bordercolor='#0030b0' cellspacing='0'

				for( int i = 0 ; i < node.ChildNodes.Count - 1 ; i ++ )
				{
					RenderLineNode( node.ChildNodes[i] , writer ) ;
				}

				RenderLastNode( node.ChildNodes[node.ChildNodes.Count - 1] , writer ) ;

				writer.WriteLine( "</table>" );
			}

			writer.WriteLine( "</td>" );

			writer.WriteLine( "</tr>" );
				
		} 

		
		#region IPostBackDataHandler ��Ա
		/// <summary>
		/// 
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
			// TODO:  ��� TreeView.RaisePostDataChangedEvent ʵ��
		}

		/// <summary>
		/// ����CheckedNodeValueList
		/// </summary>
		/// <param name="postDataKey"></param>
		/// <param name="postCollection"></param>
		/// <returns></returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			this._CheckedNodeValueList = postCollection[ this.ClientID + "_Node" ];
			return false;
		}

		#endregion


		private string _NodeNavigateUrlFormatString = "#";
		[Category("Settings")]
		[Description("�ڵ�����·����ʽ���ַ������ڲ����ɽڵ�ʱʹ�ã���javascript:alert('{0}','{1}')")]
		public string NodeNavigateUrlFormatString 
		{
			get{return _NodeNavigateUrlFormatString ;	}
			set{ _NodeNavigateUrlFormatString = value ; }
		}

		private bool _EnableNodeChecked = false ;
		[Category("Settings")]
		[Description("�Ƿ�����ڵ�ѡ��")]
		public bool EnableNodeChecked 
		{
			get{return _EnableNodeChecked ;	}
			set{ _EnableNodeChecked = value ; }
		}
        

		public string NodeValeField = "Value";
		public string NodeTextField= "Text";

		public object RootId ;
		public string KeyField ;
		public string ParentKeyField;

		private object _DataSource ;
		public object DataSource
		{
			set
			{
				_DataSource = value ;
			}
		}

		public override void DataBind()
		{
			if( this._DataSource != null )
			{
				if( this.RootId != null )
					this.BuildNodes( _DataSource as DataTable , this.ChildNodes , this.RootId  ) ;
				else
				{
					DataTable t = _DataSource as DataTable ;

					ArrayList rootList = new ArrayList();

					foreach( DataRow r in t.Rows ) //��δָ��RootId���Զ��ж�
					{
						DataRow[] rows = t.Select( this.KeyField + "='"+ r[this.ParentKeyField] +"'" );
						
						if( rows.Length == 0 )
						{
							if( ! rootList.Contains( r[this.ParentKeyField].ToString() ) )
								rootList.Add( r[this.ParentKeyField].ToString() );  
						}
					}

					foreach( string rootId in rootList )
					{
						this.BuildNodes( t , this.ChildNodes , rootId  ) ;
					}
				}
			}
		}


		private void BuildNodes( DataTable t , TreeNodeCollection toNodes , object parentId )
		{
			DataRow[] rows = t.Select( this.ParentKeyField + "='"+parentId+"'" );

			//Page.Response.Write( keyColumn + "='"+parentId+"'" + rows.Length.ToString() );

			foreach( DataRow r in rows )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = r ;
			
				n.Text = r[ this.NodeTextField ].ToString() ;
				 
				n.Value = r[ this.NodeValeField ].ToString() ;

				n.EnableChecked = this.EnableNodeChecked ;

				n.NavigateUrl = string.Format( this.NodeNavigateUrlFormatString , n.Value , n.Text );

				toNodes.Add( n ) ;

				BuildNodes( t , n.ChildNodes , r[ this.KeyField ] ) ;
			}

		}


		#region BuildFromDataTable 

		/// <summary>
		/// ��DataTable�д�����,  ֧�������У�
		/// Text,Value,ImageUrl,NavigateUrl,Target,Checked,Expand,HasChildNodes
		/// </summary>
		/// <param name="t">����Դ</param>
		/// <param name="parentId">���ڵ㸸ID</param>
		/// <param name="keyColumn">���� ����</param>
		/// <param name="parentKeyColumn">���ڵ�ID ����</param>
		public void BuildFromDataTable( DataTable t , string parentId , string keyColumn , string parentKeyColumn )
		{
			this._ChildNodes.Clear() ;

			BuildNodesFromDataTable( t , this._ChildNodes , parentId , keyColumn , parentKeyColumn ) ;			
		}

		
		/// <summary>
		/// �ݹ�����ڵ�
		/// </summary>
		/// <param name="t"></param>
		/// <param name="toNodes"></param>
		/// <param name="parentId"></param>
		/// <param name="keyColumn"></param>
		/// <param name="parentKeyColumn"></param>
		private void BuildNodesFromDataTable( DataTable t , TreeNodeCollection toNodes , string parentId , string keyColumn , string parentKeyColumn )
		{
			DataRow[] rows = t.Select( parentKeyColumn + "='"+parentId+"'" );

			//Page.Response.Write( keyColumn + "='"+parentId+"'" + rows.Length.ToString() );

			foreach( DataRow r in rows )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = r ;

				if( t.Columns.Contains( "Text" ) )
					n.Text = r["Text"].ToString() ;

				if( t.Columns.Contains( "Value" ) )
					n.Value = r["Value"].ToString() ;

				if( t.Columns.Contains( "ImageUrl" ) )
					n.ImageUrl = r["ImageUrl"].ToString() ;

				if( t.Columns.Contains( "Target" ) )
					n.Target = r["Target"].ToString() ;

				if( t.Columns.Contains( "NavigateUrl" ) )
					n.NavigateUrl = r["NavigateUrl"].ToString() ;

				if( t.Columns.Contains( "Checked" ) )
					n.Checked = Convert.ToBoolean( r["Checked"].ToString() ) ;

				if( t.Columns.Contains( "Disabled" ) )
					n.Disabled = Convert.ToBoolean( r["Disabled"].ToString() ) ;

				if( t.Columns.Contains( "Expand" ) )
					n.Expand = Convert.ToBoolean( r["Expand"].ToString() ) ; //

				if( t.Columns.Contains( "HasChildNodes" ) )
					n.HasChildNodes = Convert.ToBoolean( r["HasChildNodes"].ToString() ) ;

				toNodes.Add( n ) ;

				BuildNodesFromDataTable( t , n.ChildNodes , r[keyColumn].ToString() , keyColumn , parentKeyColumn ) ;
				
			}

		}

		#endregion

		#region BuildFromXml

		public void BuildFromXmlList( XmlDocument source  , string parentId , string keyAtt , string parentKeyAtt , string nameAtt , string valueAtt )
		{
			BuildNodesFromXmlList( source , this._ChildNodes , parentId , keyAtt , parentKeyAtt , nameAtt ,valueAtt  ) ;			
		}

		private void BuildNodesFromXmlList( XmlDocument source , TreeNodeCollection toNodes , 
			string parentId , string keyAtt , string parentKeyAtt , string nameAtt ,  string valueAtt )
		{

			XmlNodeList nodeList = source.SelectNodes( "*[@"+parentKeyAtt+"='" + parentId + "']" );

			foreach( XmlNode xn in nodeList )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = xn ;
				 
				n.Text = xn.Attributes[nameAtt].Value;
				n.Value = xn.Attributes[valueAtt].Value;

				if( xn.Attributes[ "ImageUrl" ] != null )
					n.ImageUrl = xn.Attributes["ImageUrl"].Value ;

				if( xn.Attributes[ "Target" ] != null )
					n.Target = xn.Attributes["Target"].Value ;

				if( xn.Attributes[ "NavigateUrl" ] != null )
					n.NavigateUrl = xn.Attributes["NavigateUrl"].Value ;

				if( xn.Attributes[ "EnableChecked" ] != null )
					n.EnableChecked = Convert.ToBoolean( xn.Attributes["EnableChecked"].Value );

				if( xn.Attributes[ "Checked" ] != null )
					n.Checked = Convert.ToBoolean( xn.Attributes["Checked"].Value );

				if( xn.Attributes[ "Disabled" ] != null )
					n.Disabled = Convert.ToBoolean( xn.Attributes["Disabled"].Value );

				if( xn.Attributes[ "Expand" ] != null )
					n.Expand = Convert.ToBoolean( xn.Attributes["Expand"].Value ) ;

				if( xn.Attributes[ "HasChildNodes" ] != null )  //
					n.HasChildNodes = Convert.ToBoolean( xn.Attributes["HasChildNodes"].Value );

				BuildNodesFromXmlList( source , n.ChildNodes , xn.Attributes[keyAtt].Value , keyAtt , parentKeyAtt , nameAtt , valueAtt ) ;

				toNodes.Add( n ) ;
			}
		}

		
//		public void BuildFromXmlTree( XmlDocument source , string nameAtt , string valueAtt )
//		{
//			BuildFromXmlTree( source.DocumentElement , this._ChildNodes , nameAtt ,valueAtt  ) ;			
//		}

		public void BuildFromXmlTree( XmlNode source , string nameAtt , string valueAtt )
		{
			BuildFromXmlTree( source , this._ChildNodes , nameAtt ,valueAtt  ) ;			
		}
		
		public void BuildFromXmlTree( XmlNodeList source , string nameAtt , string valueAtt )
		{
			foreach( XmlNode xn in source )
			{
				BuildFromXmlTree( xn , this._ChildNodes , nameAtt ,valueAtt  ) ;		
			}
		}

		public void BuildFromXmlTree( XmlNode source , TreeNodeCollection toNodes , string nameAtt ,  string valueAtt )
		{
			foreach( XmlNode xn in source.ChildNodes )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = xn ;
				 
				n.Text = xn.Attributes[nameAtt].Value;
				n.Value = xn.Attributes[valueAtt].Value;

				if( xn.Attributes[ "ImageUrl" ] != null )
					n.ImageUrl = xn.Attributes["ImageUrl"].Value ;

				if( xn.Attributes[ "Target" ] != null )
					n.Target = xn.Attributes["Target"].Value ;

				if( xn.Attributes[ "NavigateUrl" ] != null )
					n.NavigateUrl = xn.Attributes["NavigateUrl"].Value ;

				if( xn.Attributes[ "EnableChecked" ] != null )
					n.EnableChecked = Convert.ToBoolean( xn.Attributes["EnableChecked"].Value );

				if( xn.Attributes[ "Checked" ] != null )
					n.Checked = Convert.ToBoolean( xn.Attributes["Checked"].Value );

				if( xn.Attributes[ "Disabled" ] != null )
					n.Disabled = Convert.ToBoolean( xn.Attributes["Disabled"].Value );

				if( xn.Attributes[ "Expand" ] != null )
					n.Expand = Convert.ToBoolean( xn.Attributes["Expand"].Value ) ;

				if( xn.Attributes[ "HasChildNodes" ] != null )
					n.HasChildNodes = Convert.ToBoolean( xn.Attributes["HasChildNodes"].Value );

				BuildFromXmlTree( xn , n.ChildNodes , nameAtt , valueAtt ) ;

				toNodes.Add( n ) ;
			}
		}


		 


		#endregion
	}
}
