// --------------------------------------------------------------------
// - ��Ȩ����  beyondbit.com
// - ���ߣ�    �Ž���        Email:jianyi0115@163.com
// - ������    2005.10.18
// - ���ģ�
// - ��ע��    
// --------------------------------------------------------------------

using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
 
namespace CA.Web
{
	
	///@ zhang jianyi 2005-10-18
	/// <summary>
	/// ��ҳ�ؼ�----ֻ�ṩ��ʾ�߼������漰ʵ�ʵ����ݷ�ҳ����
	/// ҳ���һ�μ���ʱ���ü�¼�������õ�һҳ��ʾ��
	/// oninit�а�PagerChanged�¼����¼�����ʱ���¼�����������Ӧҳ��
	/// �ṩ����ʽ��ģ�����ù���
	///
	/// </summary>
	/// <example>
	/// ���Բ���������ƿؼ�����ۼ�ѡ����Ӧ��ʾ���ܣ�ʾ�����£�
	/// <code>
	/// <![CDATA[
	/// 
	/// <bbit:tpager id="TPager1" runat="server" NumericButtonCount="3" RecordCount="100" EnableCookie="True"
	/// PageSizeOptions="10,20,30,40,50" BackColor="YellowGreen">
	/// <Template>
	///  ��¼������<%# Container.RecordCount %> 
	/// ��ҳ����<%# Container.PageCount %> 
	/// ÿҳ��¼����<%# Container.PageSize %> 
	/// ��ǰ��<%# Container.CurrentPageNumber %>ҳ |			
	/// <%# Container.PrePageLink %>
	/// <%# Container.NextPageLink %>  |
	/// <%# Container.NumericLinks %> |
	/// ת����<%# Container.PageNumSelect %>ҳ | 
	/// ��<%# Container.PageNumInput %> | 
	/// ÿҳ��¼����<%# Container.PageSizeChange %> | 
	/// </Template>
	/// </bbit:tpager>
	/// ]]>
	/// </code>
	/// </example>
	[DefaultProperty("PageSize"),
	DefaultEvent("PageIndexChanged"),
		ToolboxData("<{0}:TPager runat=server></{0}:TPager>")]
	[ParseChildren(true)]
	[Designer( typeof(TPagerDesigner) )]
	public class Pager : System.Web.UI.WebControls.WebControl , System.Web.UI.IPostBackEventHandler , System.Web.UI.IPostBackDataHandler
		, System.Web.UI.INamingContainer
	{

		#region ��������

		private bool enableCookie = true ;
		/// <summary>
		/// �Ƿ�����cookie,�����ã���������վ�㱣��PageSizeͬ��
		/// </summary>
		[Description("�Ƿ�����cookie,�����ã���������վ�㱣��PageSizeͬ��"),
		Category("Behavior")]
		public bool EnableCookie
		{
			get {return enableCookie ; }
			set {enableCookie= value;}
		}

		private DisplayMode displayMode = DisplayMode.Always ;

		/// <summary>
		/// ��ʾģʽ:Always������ʾ;AutoHidden����¼��Ϊ0ʱ�Զ�����;AutoHiddenBeforePost����¼��Ϊ0�ҵ�һ�μ���ʱ�Զ����� 
		/// </summary>
		[Description("��ʾģʽ:Always������ʾ;AutoHidden����¼��Ϊ0ʱ�Զ�����;AutoHiddenBeforePost����¼��Ϊ0�ҵ�һ�μ���ʱ�Զ����� "),
		Category("Behavior")]
		public DisplayMode DisplayMode
		{
			get {return displayMode ; }
			set {displayMode= value;}
		} 

		 
		private string cookieName = "Pager-PageSize";
		/// <summary>
		/// ����ÿҳ��¼����cookie��
		/// </summary>
		[Description("����ÿҳ��¼����cookie��"),
		Category("Behavior")]
		public string CookieName
		{
			get {return cookieName ; }
			set {cookieName= value;}
		}

		private HorizontalAlign horizontalAlign = HorizontalAlign.Center;
		/// <summary>
		/// ����ˮƽ���뷽ʽ
		/// </summary>
		[Description("����ˮƽ���뷽ʽ"),
			Category("Appearance"),
			DefaultValue(HorizontalAlign.Center)]
		public HorizontalAlign HorizontalAlign 
		{
			set
			{
				horizontalAlign = value ;
			}
			get
			{
				return horizontalAlign ;
			}
		}

		private string pageSizeOptions = "10,15,20,30";

		/// <summary>
		/// ��ѡÿҳ��¼��
		/// </summary>
		[Description("��ѡÿҳ��¼��"),Category("Behavior")]
		public string PageSizeOptions
		{
			get {return pageSizeOptions ; }
			set {pageSizeOptions= value;}
		}

		private int pageSize = 10 ;
		/// <summary>
		/// ���û��ȡÿҳ��¼��
		/// </summary>
		[Description("���û��ȡÿҳ��¼��"),Category("Behavior")]
		public int PageSize
		{
			set
			{ 
				this.pageSize = value;

				setPageCount() ;

				if( false == this.EnableCookie ) return ;

				HttpCookie cookie = new HttpCookie( CookieName );
				cookie.Name = CookieName ;
				cookie.Value = value.ToString() ;

				cookie.Expires = DateTime.Now.AddYears(1);

				Page.Response.Cookies.Add( cookie);
			}
			get
			{ 
				return this.pageSize; 
			}
		}

		private int pageCount = 0 ;
		/// <summary>
		/// ��ȡҳ��
		/// </summary>
		[Description("��ȡҳ��"),Category("Behavior")]
		public int PageCount
		{
			get
			{ 
				return this.pageCount; 
			}
		}
		
		private int currentPageIndex = 0 ;
		/// <summary>
		/// ���û��ȡ��ǰҳ������0��ʼ��
		/// </summary>
		[Description("���û��ȡ��ǰҳ������0��ʼ��"),Category("Behavior")]
		public int CurrentPageIndex
		{
			get {return currentPageIndex ; }
			set {currentPageIndex= value;}
		}

		private int recordCount = 0;
		/// <summary>
		/// ���û��ȡ��¼��
		/// </summary>
		[Description("���û��ȡ��¼��"),Category("Behavior")]
		public int RecordCount
		{
			get { return recordCount ;}
			set 
			{ 
				recordCount = value ;
				setPageCount() ;
			}
		}
		/// <summary>
		/// ���¼���ҳ��
		/// </summary>
		private void setPageCount()
		{
			this.pageCount = this.recordCount / this.pageSize ;
			if( this.pageCount * this.pageSize < this.recordCount ) this.pageCount ++ ;
		}

		private TPagerMode mode = TPagerMode.Default ;
		/// <summary>
		/// �û��ȡ��ҳ�ؼ�����ʽ�������ڲ�ģ�棬�������ʧЧ
		/// </summary>
		[Description("���û��ȡ��ҳ�ؼ�����ʽ�������ڲ�ģ�棬�������ʧЧ"),Category("Behavior")]
		public TPagerMode Mode
		{
			get { return mode ;}
			set 
			{ 
				mode = value ;
			}
		}

		private string nextPageText = "��һҳ" ;
		/// <summary>
		/// Ҫ����"��һҳ"��ť�ϵ�����
		/// </summary>
		[Description( @"Ҫ����""��һҳ""��ť�ϵ�����"),Category("Behavior")]
		public string NextPageText
		{
			get {return nextPageText ; }
			set {nextPageText= value;}
		}

		private string prePageText = "��һҳ" ;
		/// <summary>
		/// Ҫ����"��һҳ"��ť�ϵ�����
		/// </summary>
		[Description( @"Ҫ����""��һҳ""��ť�ϵ�����"),Category("Behavior")]
		public string PrePageText
		{
			get {return prePageText ; }
			set {prePageText= value;}
		}


		private string nextNumericText = "..." ;
		/// <summary>
		/// ����ҳ����һ������
		/// </summary>
		[Description( @"����ҳ����һ������"),Category("Behavior")]
		public string NextNumericText
		{
			get {return nextNumericText ; }
			set {nextNumericText= value;}
		}

		private string preNumericText = "..." ;
		/// <summary>
		/// ����ҳ����һ������
		/// </summary>
		[Description( @"����ҳ����һ������"),Category("Behavior")]
		public string PreNumericText
		{
			get {return preNumericText ; }
			set {preNumericText= value;}
		}

		private string firstPageText = "..." ;
		/// <summary>
		/// ��ҳ��������
		/// </summary>
		[Description( "��ҳ"),Category("Behavior")]
		public string FirstPageText
		{
			get { return firstPageText ; }
			set { firstPageText= value;  } 
		}

		private string lastPageText = "..." ;
		/// <summary>
		/// βҳ��������
		/// </summary>
		[Description( "βҳ"),Category("Behavior")]
		public string LastPageText
		{
			get { return lastPageText ; }
			set { lastPageText= value;}
		}

		private int pageButtonCount = 3 ;
		/// <summary>
		/// ��ҳ�û�������Ҫ��ʾ������ҳ�����
		/// </summary>
		[Description( @"��ҳ�û�������Ҫ��ʾ������ҳ�����"),Category("Behavior")]
		public int NumericButtonCount
		{
			get {return pageButtonCount ; }
			set {pageButtonCount= value;}
		}

		private string numericButtonFormat = "[{0}]" ;
		/// <summary>
		/// ����ҳ���ʽ
		/// </summary>
		[Description( @"����ҳ���ʽ"),Category("Behavior")]
		public string  NumericButtonFormat
		{
			get {return numericButtonFormat ; }
			set {numericButtonFormat= value;}
		}

//		private string numericButtonFormat = "[{0}]" ;
//		/// <summary>
//		/// ����ҳ���ʽ
//		/// </summary>
//		[Description( @"����ҳ���ʽ"),Category("Behavior")]
//		public string  PrePageLinkCssClass
//		{
//			get {return numericButtonFormat ; }
//			set {numericButtonFormat= value;}
//		}

		

		 
		#endregion 

		/// <summary>
		/// ��ҳ�¼�����
		/// </summary>
		public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

		/// <summary>
		/// ��ҳ�¼���ҳ��ı���¼�����������¼�
		/// </summary>
		public event PageChangedEventHandler PagerChanged;


		/// <summary>
		/// �����ҳ�¼�
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPagerChanged(PageChangedEventArgs e)
		{
			if (PagerChanged != null)
				PagerChanged(this, e);
		}


		/// <summary>
		/// �ؼ���ʼ��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			//base.OnInit (e);
			Page.RegisterRequiresPostBack( this );

			if( this.EnableCookie && Page.Request.Cookies[ CookieName ] != null )
			{
				//Page.Response.Write( Page.Request.Cookies[CookieName].Value );

				this.pageSize = Convert.ToInt32( Page.Request.Cookies[CookieName].Value );

				this.setPageCount();
			}

		}

		private  ITemplate _template ;
		/// <summary>
		/// �ؼ�ģ��
		/// </summary>
		[PersistenceMode( PersistenceMode.InnerProperty ),TemplateContainer( typeof(PagerTemplate) )]
		public ITemplate Template
		{
			set{ this._template = value ; }
			get{ return this._template ; }
		}
		
		/// <summary>
		/// ģ�洦��
		/// </summary>
		protected override void CreateChildControls()
		{
			//base.CreateChildControls ();

			if( this._template != null )
			{
				PagerTemplate temp = new PagerTemplate( this );

				this._template.InstantiateIn( temp );

				this.Controls.Add( temp ) ;
			}

			base.ChildControlsCreated = true ;

		}

		private bool _isBinded = false ;
		/// <summary>
		/// ��
		/// </summary>
		public override void DataBind()
		{
			base.EnsureChildControls() ;
			base.DataBind ();
			_isBinded = true ;
		}
	
		/// <summary>
		/// �ؼ�����֮ǰע��js
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			if( _isBinded == false )
				DataBind();

			base.OnPreRender (e);	

			string jsKey = "Pager-js";

			if( Page.IsClientScriptBlockRegistered( jsKey ) ) return ;

			string js = "<script language='javascript'>function Event(args){ __doPostBack('"+ this.ID +"' , args ) ; }</script>";

			Page.RegisterClientScriptBlock( jsKey , js ) ;
		}


		/// <summary>
		/// ���˿ؼ����ָ�ָ�������������
		/// </summary>
		/// <param name="output"> Ҫд������ HTML ��д�� </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.WriteLine( "<input type='hidden'name='" + this.UniqueID + "_PageArgs' value='" + 
				this.recordCount + "," + this.pageCount+ "," + this.currentPageIndex + ","+this.pageSize+"'>" );

			if( this.recordCount <= 0 ) //�Զ�����
			{
				if( displayMode == DisplayMode.AutoHidden )
					return ;
				else if( displayMode == DisplayMode.AutoHiddenBeforePost && false == Page.IsPostBack )
					return ;
			}

			if( this.Controls.Count != 0 ) //������ģ��
				base.Render( output );
			else
			{
				PagerTemplate temp = new PagerTemplate( this );

				base.RenderBeginTag( output ) ;
				output.Write( "<table align='"+this.horizontalAlign.ToString()+"'>" );
				output.Write( "<tr>" );
				if( this.Mode == TPagerMode.NextPrev )
				{
					output.Write( "<td valign=middle>" );
					output.Write( temp.PrePageLink + "&nbsp;" + temp.NextPageLink );
					output.Write( "</td>" );
				}
				else if( this.Mode == TPagerMode.NumericPages )
				{
					output.Write( temp.NumericLinks );
				}
				else if( this.Mode == TPagerMode.Standard )
				{
					output.Write( "<td valign=center align='left' width='40%' valign=middle nowrap >" );

					output.Write( string.Format( "��ǰҳ�룺{0} ��ҳ����{1} ÿҳ��{2} ������{3} " , this.CurrentPageIndex + 1 , this.pageCount ,this.pageSize ,this.recordCount  ) );

					output.WriteLine( " &nbsp; "  );

					output.WriteLine( "</td><td valign=center align='center' nowrap valign=middle>"  );
					output.Write( "��" + temp.PrePageLink + "��&nbsp;��" + temp.NextPageLink + "��" );

					output.WriteLine( "</td><td valign=center align='right' width='30%' nowrap valign=middle'>"  );
					output.WriteLine( temp.PageNumInput  );
					output.WriteLine( "</td>"  );
				}
				else if( this.Mode == TPagerMode.Default )
				{
					
					output.Write( "<td valign=center align='center' width='40%' valign=middle nowrap >" );

					output.Write( string.Format( "��ǰҳ�룺{0} ��ҳ����{1} ÿҳ��{2} ������{3} " , this.CurrentPageIndex + 1 , this.pageCount ,this.pageSize ,this.recordCount  ) );

					output.Write( "��" + temp.PrePageLink + "��&nbsp;��" + temp.NextPageLink + "��" );
					output.Write( "ת��" );			
					output.Write( temp.PageNumSelect  );

					output.WriteLine( " &nbsp; "  );

					output.Write( "ÿҳ" );	
					output.Write( temp.PageSizeChange  );
				}

				output.Write( "</tr>" );
				output.Write( "</table>" );

				base.RenderEndTag( output ) ;

			}

		}

		#region IPostBackEventHandler ��Ա

		/// <summary>
		/// ����ط��¼�
		/// </summary>
		/// <param name="eventArgument"></param>
		public void RaisePostBackEvent(string eventArgument)
		{
			// TODO:  ��� XPager.RaisePostBackEvent ʵ��

			//Page.Response.Write( eventArgument + currentPageIndex ) ;

			PageChangedEventArgs args = new PageChangedEventArgs();
			args.OldPageIndex = this.currentPageIndex ;

			args.RecordCount = this.recordCount;
			args.PageSize = this.PageSize ;

			if( eventArgument == "pre" )
			{
				args.NewPageIndex = args.OldPageIndex -1 ;
				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "next" )
			{
				args.NewPageIndex = args.OldPageIndex + 1 ;
				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "go-input" )
			{
				try
				{
					//args.NewPageIndex = Convert.ToInt32( this._sPageIndexFromInput ) - 1  ;

                    if (Int32.TryParse(this._sPageIndexFromInput, out args.NewPageIndex))
                        args.NewPageIndex = args.NewPageIndex - 1;
					
					if( args.NewPageIndex >= this.pageCount  )
						args.NewPageIndex = this.pageCount - 1;

					if( args.NewPageIndex < 0 ) args.NewPageIndex = 0 ;
				}
				catch
				{
					args.NewPageIndex = 0 ;
				}

				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "go-select" )
			{
				args.NewPageIndex = Convert.ToInt32( this._sPageIndexFromSelect ) - 1 ;
				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "ps" ) //ҳ���ñ�
			{
				args.NewPageIndex = 0  ;
				//args.PageSize =  Convert.ToInt32( this._sPageSize ) ;
				//this.currentPageSize = args.PageSize ;
				args.EventType = PagerEventType.PageSizeChanged ;




			}
			else if( eventArgument == "pregroup" )
			{
				int start = ( this.CurrentPageIndex / this.NumericButtonCount ) * this.NumericButtonCount  ;
				args.NewPageIndex = start - this.NumericButtonCount  ;
 
				args.EventType = PagerEventType.PageSizeChanged ;
			}
			else if( eventArgument == "nextgroup" )
			{
				int start = ( this.CurrentPageIndex / this.NumericButtonCount ) * this.NumericButtonCount  ;
				args.NewPageIndex = start + this.NumericButtonCount  ;
 
				args.EventType = PagerEventType.PageSizeChanged ;
			}
			else
			{
				args.NewPageIndex =  Convert.ToInt32( eventArgument ) ;  ;
				args.EventType = PagerEventType.PageIndexChanged;
			}


			currentPageIndex = args.NewPageIndex ;

			//Page.Response.Write( eventArgument );

			this.OnPagerChanged( args ) ;
		}

		#endregion

		#region IPostBackDataHandler ��Ա
		
		/// <summary>
		/// �������ݸı��¼����շ���
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
		}

		private string _sPageSize ;
		private string _sPageIndexFromInput ;
		private string _sPageIndexFromSelect ;
 	
		/// <summary>
		/// ����ط�����
		/// </summary>
		/// <param name="postDataKey"></param>
		/// <param name="postCollection"></param>
		/// <returns></returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			string args = postCollection[ this.UniqueID + "_PageArgs" ];
			if( args == null || args == "" ) return false ;

			string[] arr = args.Split( ',' );
			this.recordCount = int.Parse(arr[0]);
			this.pageCount = int.Parse(arr[1]);
			this.currentPageIndex = int.Parse(arr[2]);

            

			//this.pageSize = int.Parse(arr[3]);
			//Page.Response.Write( " LoadPostData" + currentPageIndex ) ;

			_sPageSize = postCollection[ this.ClientID + "_PageSize" ];
			_sPageIndexFromInput = postCollection[ this.ClientID + "_PageIndexFromInput" ];
            _sPageIndexFromSelect = postCollection[this.UniqueID + "_PageIndexFromSelect"];

			if( _sPageSize != null )   
				this.PageSize = Convert.ToInt32( _sPageSize );

			return false;
		}

		#endregion

		#region FilterByPager ��������
		
		/// <summary>
		/// ȡ����ǰҳ����
		/// </summary>
		/// <param name="oldData"></param>
		/// <returns></returns>
		public DataTable FilterByPager( DataTable oldData )
		{
			return FilterByPager( oldData, this.pageSize , this.currentPageIndex ) ;
		}

		/// <summary>
		/// ȡ����ǰҳ����
		/// </summary>
		/// <param name="oldData">����Դ</param>
		/// <param name="pSize">ÿҳ��¼��</param>
		/// <param name="pIndex">��ʼ����</param>
		/// <returns></returns>
		public DataTable FilterByPager( DataTable oldData , int pSize , int pIndex )
		{
			if( oldData.Rows.Count <= pSize ) return oldData ;

			DataTable newData = new DataTable();
			foreach( DataColumn col in oldData.Columns )
			{
				newData.Columns.Add( new DataColumn( col.ColumnName , col.DataType ) );
			}

			int startIndex = 0 ;
			startIndex = pSize * pIndex ;
			if( startIndex > oldData.Rows.Count - 1 ) return newData ;

			for( ; startIndex < oldData.Rows.Count ; startIndex ++  )
			{
				if( newData.Rows.Count == pSize ) break;
				DataRow newRow = newData.NewRow();
				newData.Rows.Add( newRow );
				for( int i = 0 ; i < newData.Columns.Count ; i ++ )
				{
					newRow[i] = oldData.Rows[startIndex][i] ;
				}
			}
			return newData;
		}

		#endregion


	}
}
