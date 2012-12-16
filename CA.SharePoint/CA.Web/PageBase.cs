using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CA.Web
{
    /// <summary>
    /// ҳ����� , ʵ�ַ��ʿ���, �ṩͨ�õ���֤����, �ͻ���js����, ���ݿ�Ự����
    /// 
    /// �����޸Ĳ���////
    /// 
    /// </summary>
   public class PageBase : Page
   {

       private Script _Script = null ;
       /// <summary>
       /// �ű�����
       /// </summary>
       protected virtual Script Script
       {
           get
           {
               if( _Script == null )
                   _Script = new Script(this);

               return _Script;
           }
       }

       private IDictionary _clientJsVar = new Hashtable(); 
       /// <summary>
       /// ע��js����
       /// </summary>
       /// <param name="varName"></param>
       /// <param name="value"></param>
       protected void RegisterJsVar(string varName, string value)
       {
           if (_clientJsVar[varName] != null)
               _clientJsVar[varName] = value;
           else
               _clientJsVar.Add(varName, value);
       }

       protected override void OnPreRender(EventArgs e)
       {
           base.OnPreRender(e);

           //js����
           StringBuilder sb = new StringBuilder();
           sb.Append("\n<script type=\"text/javascript\">\n");

           foreach (DictionaryEntry d in _clientJsVar)
           {
               sb.Append("var ");
               sb.Append(d.Key);
               sb.Append(" = ");
               sb.Append("\"");
               sb.Append(d.Value);
               sb.Append("\";\n");
           }

           sb.Append("\n</script>");         

           Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "__clientJsVar", sb.ToString());

           AddProgressBar();
       }

       /// <summary>
       /// ��ӽ�����
       /// </summary>
       protected virtual void AddProgressBar()
       {
           if (EnableProgressBar)
           {
               if (this.Form == null) return;

               this.Form.Attributes.Add("onsubmit", "ShowProgressInfo1();"); //��ӽ����� 
           }
       }

       //private Security.User _CurrentUser = null;
       ///// <summary>
       ///// ��ȡ��ǰ��½�û�����
       ///// </summary>
       //public virtual Security.User CurrentUser
       //{
       //    get
       //    {
       //        if (_CurrentUser == null)
       //             _CurrentUser = Security.SecurityContext.Current.User;

       //        return _CurrentUser;
       //    }
       //}

       private bool _showProgressBar = true ;
       /// <summary>
       /// ҳ���ύʱ�Ƿ���ʾ������
       /// </summary>
       public bool EnableProgressBar
       {
           get
           {
               return _showProgressBar;
           }
           set
           {
               _showProgressBar = value;
           }
       }

       protected void OnRowCreated(object o , GridViewRowEventArgs e)
       {
           if (e.Row.RowType == DataControlRowType.DataRow)
           {
               //�����������ʱ���ı��е���ɫ
               e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#e7e7e7';this.style.cursor = 'hand';");
               e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
           }
           else if (e.Row.RowType == DataControlRowType.Header)
           {
               //����GridView�������ʽ
               for (int i = 0; i < e.Row.Cells.Count; i++)
               {
                   e.Row.Cells[i].CssClass = "TdHeaderStyle1";
               }
               //��������״̬ͼƬ
               //DisplaySortOrderImages(SortExpression, e.Row);
               //this.CreateRow(0, 0, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
           }
          // base.OnRowCreated(e);
       }

       //protected override void OnInit(EventArgs e)
       //{
       //    base.OnInit(e);

       //    if ( Request.QueryString["ldap"] != null)
       //    {
       //        string ldap = Request.QueryString["ldap"];
 
       //        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(ldap, false, 300);
       //        string encTicket = FormsAuthentication.Encrypt(ticket);
       //        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

       //    }
       //}


       #region ��ѯ�ַ�����������

       /// <summary>
       /// ��ѯ�ַ���ת����int���������ڣ����׳��쳣
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
       protected virtual int GetIntFromQueryString(string name)
       {
           if (Request.QueryString[name] == null) throw new ArgumentNullException("name", "��ѯ�ַ���[ " + name + " ]������");

           return Convert.ToInt32(Request.QueryString[name].Trim());
       }

       /// <summary>
       /// ��ѯ�ַ���ת����int���������ڣ��򷵻�Ĭ��ֵ
       /// </summary>
       /// <param name="name"></param>
       /// <param name="nullValue"></param>
       /// <returns></returns>
       protected virtual int GetIntFromQueryString(string name, int defaultValue)
       {
           if (Request.QueryString[name] == null) return defaultValue;

           return Convert.ToInt32(Request.QueryString[name].Trim());
       }


       /// <summary>
       /// ��ȡ��ѯ�ַ���ֵ���������ڣ����׳��쳣
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
       protected virtual string GetFromQueryString(string name)
       {
           if (Request.QueryString[name] == null) throw new ArgumentNullException("name", "��ѯ�ַ���[ " + name + " ]������");

           return Request.QueryString[name].Trim() ;
       }

       /// <summary>
       /// ��ȡ��ѯ�ַ���ֵ���������ڣ��򷵻�Ĭ��ֵ
       /// </summary>
       /// <param name="name"></param>
       /// <param name="defaultValue"></param>
       /// <returns></returns>
       protected virtual string GetFromQueryString(string name, string defaultValue)
       {
           if (Request.QueryString[name] == null) return defaultValue;

           return Request.QueryString[name].Trim();
       }

       /// <summary>
       /// ��ȡ��ѯ�ַ���ֵ���������ڣ����׳��쳣
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="name"></param>
       /// <returns></returns>
       protected virtual T GetFromQueryString<T>(string name)
       {
           if (Request.QueryString[name] == null) throw new ArgumentNullException("name", "��ѯ�ַ���[ " + name + " ]������");

           string s = Request.QueryString[name].Trim();

           object obj;

           try
           {
               obj = Convert.ChangeType(s, typeof(T));
               return (T)obj;
           }
           catch (Exception ex)
           {
               throw new ArgumentException("���ܽ���ѯ�ַ�������ֵ[ " + s + " ]ת��Ϊ" + typeof(T).FullName , name, ex);
           }
       }

       /// <summary>
       /// ��ȡ��ѯ�ַ���ֵ���������ڣ��򷵻�Ĭ��ֵ
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="name"></param>
       /// <param name="defaultValue"></param>
       /// <returns></returns>
       protected virtual T GetFromQueryString<T>(string name, T defaultValue)
       {
           if (Request.QueryString[name] == null) return defaultValue;

           return GetFromQueryString<T>(name);
       }

       //protected string GetFromQueryString(string name, params string[] otherValues)
       //{
       //    if (Request.QueryString[name] != null) return Request.QueryString[name].Trim() ;

       //    foreach (string v in otherValues)
       //    {
       //        if (v != null && v != "")
       //            return v.Trim();
       //    }

       //   return  null ;
       //}

      
       #endregion

       #region ����ת��

       protected void RenderErr(string errMessage)
       {
            string js = "<script language='javascript'>alert('" + errMessage + "');history.back();</script>";
            Response.Clear();
            Response.Write(js);
            Response.End();
        }

        protected string ToString(object obj)
        {
            string strTemp = "";
            if (obj == null || obj == DBNull.Value)
            {
                strTemp = "";
            }
            else
            {
                strTemp = obj.ToString();
            }
            return strTemp;
        }

        /// <summary>
        /// �����ֶ���֤
        /// </summary>
        /// <param name="v">�ֶ�ֵ</param>
        /// <param name="errMessage">�ֶ�Ϊ��ʱ��ʾ����Ϣ</param>
        /// <returns></returns>
        protected string ToString(string v, string errMessage)
        {

            if (v == null)
            {
                RenderErr(errMessage);
                return null;
            }
            else
            {
                string strTemp = v.Trim();
                if (strTemp == "")
                {
                    RenderErr(errMessage);
                    return null;
                }
                else
                {
                    return strTemp;
                }
            }
        }

       protected string ToString(string v, string errMessage , int maxLength , string errMessage2 )
       {
           if (v == null)
           {
               RenderErr(errMessage);
               return null;
           }
           else
           {
               string strTemp = v.Trim();
               if (strTemp == "")
               {
                   RenderErr(errMessage);
                   return null;
               }
               else if (strTemp.Length > maxLength)
               {
                   RenderErr(errMessage2);
                   return null;
               }
               else
               {
                   return strTemp;
               }
           }
       }

        protected int ToInt32(string v)
        {

            if (v == null || v.Trim() == "") return 0;

            try
            {
                return Convert.ToInt32(v);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ���ַ���ת��������
        /// </summary>
        /// <param name="v"></param>
        /// <param name="errMessage">ת������ʱ��ʾ����Ϣ</param>
        /// <returns></returns>
        protected int ToInt32(string v, string errMessage)
        {
            if (v == null || v.Trim() == "") return 0;

            try
            {
                return Convert.ToInt32(v);
            }
            catch
            {
                RenderErr(errMessage);
            }
            finally
            {

            }

            return Int32.MinValue;
        }

        protected int ToInt32(string v, string errMessage, bool canEmpty)
        {
            if (v == null || v.Trim() == "") return 0;

            try
            {
                return Convert.ToInt32(v);
            }
            catch
            {
                RenderErr(errMessage);
            }
            finally
            {

            }

            return Int32.MinValue;
        }

        /// <summary>
        /// ���ַ���ת��Ϊʱ������
        /// </summary>
        /// <param name="v"></param>
        /// <param name="errMessage">ת������ʱ��ʾ����Ϣ</param>
        /// <returns></returns>
        protected DateTime ToDateTime(string v, string errMessage)
        {
            try
            {
                return Convert.ToDateTime(v);
            }
            catch
            {
                RenderErr(errMessage);
            }
            finally
            {

            }

            return DateTime.Now;
        }

        /// <summary>
        /// ���ַ���ת��Ϊ��������
        /// </summary>
        /// <param name="v"></param>
        /// <param name="errMessage">ת������ʱ��ʾ����Ϣ</param>
        /// <returns></returns>
        protected Single ToSingle(string v, string errMessage)
        {
            try
            {
                return Convert.ToSingle(v);
            }
            catch
            {
                RenderErr(errMessage);
            }
            finally
            {

            }

            return 1;
        }

        /// <summary>
        /// ���ַ���ת��ΪС������
        /// </summary>
        /// <param name="v"></param>
        /// <param name="errMessage">ת������ʱ��ʾ����Ϣ</param>
        /// <returns></returns>
        protected decimal ToDecimal(string v, string errMessage)
        {
            try
            {
                return Convert.ToDecimal(v);
            }
            catch
            {
                RenderErr(errMessage);
            }
            finally
            {

            }

            return -0.1M;
        }

        #endregion

       #region �ͻ��˺���

        /// <summary>
        ///�ڿͻ�����ʾһ����Ϣ
        /// </summary>
        /// <param name="message">��ʾ����Ϣ</param>
       protected virtual void AlertMessage(string message)
        {
            Script.Alert(message);

           // Page.RegisterStartupScript("AlertMessage", "<script language='javascript'>alert(\"" + message + "\");</script>");

           // ClientScript.RegisterStartupScript( 
        }

       protected virtual void AlertMessage()
        {
            AlertMessage("����ɹ���");
        }

        /// <summary>
        /// �ڿͻ�����ʾһ����Ϣ������ʾ���������� 
        /// </summary>
        /// <param name="field">����������</param>
        /// <param name="message">��Ϣ</param>
        protected virtual void AlertMessage(string field, string message)
        {
            Page.RegisterStartupScript("AlertMessage_" + field, "<script language='javascript'>alert(\"" + message + "\");displayErrField('" + field + "');</script>");
        }

        //		protected void AlertErrDataTypeField( string field , string message )
        //		{
        //			Page.RegisterStartupScript( "AlertMessage","<script language='javascript'>alert(\"" + message + "\");displayErrField('" + field + "');</script>" );
        //		}

        /// <summary>
        /// �ٿͻ�����ʾһ����Ϣ��Ȼ��ת��һ��ҳ��
        /// </summary>
        /// <param name="message">��ʾ����Ϣ</param>
        /// <param name="url">ת��ҳ��·��</param>
        protected virtual void AlertRedirect(string message, string url)
        {
            Page.RegisterStartupScript("AlertRedirect", "<script language='javascript'>alert(\"" + JsEncoder.Encode( message ) + "\");window.navigate('" + url + "');</script>");
        }
        /// <summary>
        /// �ٿͻ�����ʾһ����Ϣ��Ȼ��ر�ҳ��
        /// </summary>
        /// <param name="message"></param>
       protected virtual void AlertClose(string message)
        {
            Script.Alert(message);
            Script.Close();

            //Page.RegisterStartupScript("AlertClose", "<script language='javascript'>alert(\"" + message + "\");window.close();</script>");
        }

        /// <summary>
        /// �رմ���
        /// </summary>
       protected virtual void CloseWindow()
        {
            Script.Close();
            //Page.RegisterStartupScript("CloseWindow", "<script language='javascript'>window.close();</script>");
        }


        /// <summary>
        /// �ٿͻ�����ʾһ����Ϣ��Ȼ��ط�����һ��ҳ��
        /// </summary>
        /// <param name="message"></param>
       protected virtual void AlertReflashFrame(string message, string frame)
        {
            Page.RegisterStartupScript("AlertReflashFrame_" + frame, "<script language='javascript'>alert('"
                + JsEncoder.Encode( message ) + "');window.parent.frames['" + frame + "'].location.reload();</script>");
        }
        /// <summary>
        /// ˢ�´����ҳ���ҳ��
        /// </summary>
        /// <param name="message"></param>
       protected virtual void AlertReflashOpener(string message)
        {
            Page.RegisterStartupScript("AlertReflashOpener", "<script language='javascript'>alert('"
                + JsEncoder.Encode( message ) + "');window.opener.location.reload();;window.close();</script>");
        }

       protected virtual void AlertBack(string message)
        {
            if( string.IsNullOrEmpty( message ) )
                Page.RegisterStartupScript("AlertBack", "<script language='javascript'>;history.back();</script>");
            else
            Page.RegisterStartupScript("AlertBack", "<script language='javascript'>alert('" + JsEncoder.Encode( message ) + "');history.back();</script>");
        }

       protected virtual void DisableAllElement()
       {
           Page.RegisterStartupScript("DisableAllElement", "<script language='javascript'>DisableAllElement();</script>");
       }

       protected virtual void DisableAllInputElement()
       {
           Page.RegisterStartupScript("DisableAllInputElement", "<script language='javascript'>DisableAllInputElement();</script>");
       }

       protected virtual void RegisterWrappedStartupJs(string js)
       {
           Page.RegisterStartupScript("StartupJs", "<script language='javascript'>\n"+js+"\n</script>");
       }

        #endregion

       /// <summary>
       /// �ͷ����ݿ�Ự
       /// </summary>
       //protected  virtual void DisposeDbSession()
       //{
       //    Sor.Container.SessionContainerFactory.GetSessionContainer().Dispose();
       //}

       //protected  override void OnUnload(EventArgs e)
       //{
       //    DisposeDbSession();           
 
       //    base.OnUnload(e);
       //}

       protected virtual string SiteRoot
        {
            get
            {
                string root = Page.Request.ApplicationPath;
                if (root == "/") return root;
                else return root + "/";
            }
        }

        protected virtual void RightControl()
        {

        }


       protected override void CreateChildControls()
       {
           base.CreateChildControls();

           //string screenInfo = Framework.Controls.ClientScreenAdapter.ClientScreenInfo;

           //Literal style = new Literal();
           //style.Text = "\n<link href=\"" + this.SiteRoot + "App_MultiStyle/" + this.Theme + "/" + screenInfo + ".css\" type=\"text/css\" rel=\"stylesheet\" />\n";

           //this.Header.Controls.Add( style );           
           
          // Page.RegisterClientScriptBlock("validate-js", "<script language='javascript' src='" + root + "Common/Js/Validate.js'></script>");

           Page.RegisterClientScriptBlock("common-js", "<script language='javascript' src='" + SiteRoot + "Common/Js/Common2.js'></script>");

           if (EnableProgressBar)
             Page.RegisterClientScriptBlock("common-Progress", "<script language='javascript' src='" + SiteRoot + "Common/Js/Progress.js'></script>");
       }

       
       //protected override void OnError(EventArgs e)
       //{
       //    Exception ex = Server.GetLastError();

       //    if (ex is Exceptions.LogicException)
       //    {
       //        Response.Write(ex.Message);
       //        Response.End();

       //    }
       //    //else if (ex is Exceptions.NotAuthException)
       //    //{
       //    //    Response.Redirect(System.Configuration.ConfigurationSettings.AppSettings["VerifyUrl"] + "?Currentpage=" + Request.Url );

       //    //    Response.End();
       //    //}
       //    else
       //    {
       //        base.OnError(e);
       //    }
       //}

    }
}
