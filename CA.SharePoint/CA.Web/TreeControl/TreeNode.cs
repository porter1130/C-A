using System;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.TreeControl
{
	/// <summary>
	///���ڵ� 
	/// </summary>
	[ParseChildren( true , "ChildNodes" )]
	public class TreeNode // : System.Web.UI.WebControls.WebControl
	{

		/// <summary>
		/// 
		/// </summary>
		public TreeNode()
		{
			_ChildNodes = new TreeNodeCollection( _Owner , this )   ;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">�ڵ�ֵ</param>
		/// <param name="text">�ڵ���ʾ�ı�</param>
		public TreeNode( string value , string text ) : this()
		{
			this._Value = value ;
			this._Text = text ;		
		}

		private TreeView  _Owner = null ;
		/// <summary>
		/// ��ȡ���ڵ�
		/// </summary>
		public TreeView  Owner
		{
			get{ return _Owner ;}
		}

		/// <summary>
		/// ���ýڵ�������TreeView
		/// </summary>
		/// <param name="owner"></param>
		internal void SetOwner( TreeView  owner )
		{
			_Owner = owner ;
			_ChildNodes.SetOwner( owner ) ;
		}


		private TreeNode  _ParentNode = null ;
		/// <summary>
		/// ��ȡ���ڵ�
		/// </summary>
		public TreeNode  ParentNode
		{
			//set{ _ParentNode = value ; }
			get{ return _ParentNode ;}
		}

		/// <summary>
		/// ���ø��ڵ�
		/// </summary>
		/// <param name="parentNode"></param>
		internal void SetParentNode( TreeNode  parentNode )
		{
			_ParentNode = parentNode ;
		}


		//public string Id ;


 		private string _ImageUrl ; // = "images/tree/top.gif" ;
		/// <summary>
		/// �ڵ�ͼƬ·��
		/// </summary>
		public string ImageUrl
		{
			set{ _ImageUrl = value ; }
			get{ return _ImageUrl ;}
		}

		private string _Text ;
		/// <summary>
		/// �ڵ���ʾ�ı�
		/// </summary>
		public string Text
		{
			set{ _Text = value ; }
			get{ return _Text ;}
		}

		private string _ToolTip ;
		/// <summary>
		/// �ڵ���ʾ�ı�
		/// </summary>
		public string ToolTip
		{
			set{ _ToolTip = value ; }
			get{ return _ToolTip ;}
		}

//		private string _OnAsyncLoaded;
//		/// <summary>
//		/// �ڵ��첽�����ӽڵ�󴥷��Ŀͻ���js���� OnAsyncLoadComplete
//		/// </summary>
//		public string OnAsyncLoadComplete 
//		{
//			set{ _OnAsyncLoaded = value ; }
//			get{ return _OnAsyncLoaded ;}
//		}
//
//		public string OnAsyncLoad ;


		private string _Value = "" ;
		/// <summary>
		/// �ڵ��ֵ
		/// </summary>
		public string Value
		{
			set{ _Value = value ; }
			get{ return _Value ;}
		}

		private bool _EnableChecked = false ;
		/// <summary>
		/// �Ƿ�����ѡ��
		/// </summary>
		public bool EnableChecked
		{
			set{ _EnableChecked = value ; }
			get{ return _EnableChecked ;}
		}

		private bool _Disabled = false ;
		//internal string DisabledHtml = "" ;
		/// <summary>
		/// �Ƿ�����ѡ��
		/// </summary>
		public bool Disabled
		{
			set{ _Disabled = value ; }
			get{ return _Disabled ;}
		}

		//internal string CheckedHtml = "" ;
		private bool _Checked = false ;
		/// <summary>
		/// �Ƿ�ѡ��,�����ô����ԣ����Զ�����EnableChecked
		/// </summary>
		public bool Checked
		{
			set{ 
				
				EnableChecked = true ;
				_Checked = value ;

//				if( value )
//					CheckedHtml = "checked";
//				else
//					CheckedHtml = "";
			
			}
			get{ 
				
				return _Checked ;
			}
		}

		private string _CssClass ;
		/// <summary>
		/// �ڵ���ʽ
		/// </summary>
		public string CssClass
		{
			set{ _CssClass = value ; }
			get{ return _CssClass ;}
		}


		private string _NavigateUrl ;
		/// <summary>
		/// �ڵ�����
		/// </summary>
		public string NavigateUrl
		{
			set{ _NavigateUrl = value ; }
			get{ return _NavigateUrl ;}
		}


		internal string TargetHtml = "" ;
		private string _Target ;
		/// <summary>
		/// �ڵ�����Ŀ����
		/// </summary>
		public string Target
		{
			set
			{ 
				_Target = value ; 
				TargetHtml = "target='"+value+"'";
			}
			get{ return _Target ;}
		}

		internal string ExpandHtml = "style='display:none'" ;
		private bool _Expand = false ;

		/// <summary>
		/// �ڵ��Ƿ�չ��
		/// </summary>
		public bool Expand
		{
			set
			{ 
				_Expand = value ; 
				if( value )
				{
					ExpandHtml = "style=''" ;

				//	if( this._ParentNode != null )    //ȷ���ڵ�һ����չ��
				//		this._ParentNode.Expland = true ;
				}
				else
					ExpandHtml = "style='display:none'" ;		
			}
			get
			{ 
				return _Expand ;
			}
		}

//		private bool _IsLastNode ;
//		public bool IsLastNode
//		{
//			set{ _IsLastNode = value ; }
//			get{ return _IsLastNode ;}
//		}


		private bool _HasChildNodes = false ;  

		/// <summary>
		/// �Ƿ�����Խڵ�,�첽����ʱ�������ô�����Ϊtrue������ڵ��������,
		/// </summary>
		public bool HasChildNodes
		{
			set
			{ 
				_HasChildNodes = value ;
			}
			get
			{ 
				return _HasChildNodes ;
			}
		}

		private string _OnCheckBoxClick ;
		public string OnCheckBoxClick
		{
			set
			{ 
				_OnCheckBoxClick = value ;
			}
			get
			{ 
				return _OnCheckBoxClick ;
			}
		}

		/// <summary>
		/// ���ݰ�ʱ�ڵ��Ӧ�������� DataRow �� XmlNode
		/// </summary>
		public object DataItem ;

		public string Id;
		public string ParentId ;

		private TreeNodeCollection _ChildNodes ;  
		/// <summary>
		/// �ӽڵ㼯��
		/// </summary>
		public TreeNodeCollection ChildNodes
		{
//			set
//			{ 
//				_ChildNodes = value ; 
//			}
			get
			{
				return _ChildNodes ;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ownerDocument"></param>
		/// <returns></returns>
		public XmlNode ToXml( XmlDocument ownerDocument )
		{
			XmlNode node = ownerDocument.CreateElement( "Node" ); 

			XmlAttribute att ;

			att = ownerDocument.CreateAttribute( "Value" );
			att.Value = this._Value ;
			node.Attributes.Append( att ) ;

			att = ownerDocument.CreateAttribute( "Text" );
			att.Value = this._Text ;
			node.Attributes.Append( att ) ;

			att = ownerDocument.CreateAttribute( "NavigateUrl" );
			att.Value = this._NavigateUrl ;
			node.Attributes.Append( att ) ;

			att = ownerDocument.CreateAttribute( "Target" );
			att.Value = this._Target ;
			node.Attributes.Append( att ) ;			

			return node ;
		}

		
	}
}
