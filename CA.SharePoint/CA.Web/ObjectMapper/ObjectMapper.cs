using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.Reflection;
using System.Drawing.Design;

[assembly: WebResource(CA.Web.ObjectMapper.JS_RESOURCE_FILE_PATH, "application/x-javascript")]
namespace CA.Web
{
    /// <summary>
    /// UI�ؼ�-����ӳ��ؼ�
    /// ʵ��UI�ؼ���һ�����ݶ����˫��ֵ����
    /// 
    /// ʵ�ֿͻ��˿ؼ�������֤
    /// 
    /// </summary>
    [ToolboxData("<{0}:ObjectMapper runat=server></{0}:ObjectMapper>")]
    [Designer(typeof(ObjectMapperDesigner))]
    [ParseChildren(true, "Parameters")]
	//[DefaultEvent( "TreeNodeCreated" )]
    public class ObjectMapper : WebControl  , IValidator
    {
        internal const string JS_RESOURCE_FILE_PATH = "CA.Web.ObjectMapper.ObjectMapper.js";

        public ObjectMapper()
        {
            _Parameters = new List<Parameter>();
        }
        
        private List<Parameter> _Parameters;
        /// <summary>
        /// ��������
        /// </summary>
        [Editor(typeof(ParameterListTypeEditor), typeof(UITypeEditor))]
        [PersistenceMode( PersistenceMode.InnerDefaultProperty)]
        public List<Parameter> Parameters
        {
            get
            {
                return _Parameters ;
            }            
        }

        /// <summary>
        /// �����ƻ�ȡ��������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Parameter this[string name]
        {
            get
            {                
                foreach (Parameter p in _Parameters)
                {
                    if ( String.Compare( p.Name , name , true ) == 0 )
                        return p;
                }

                return null;
            }
        }

        /// <summary>
        /// ��������ȡ��������
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Parameter this[int index ]
        {
            get
            {
                return _Parameters[index];
            }
        }

        private Type _ObjectType;
        /// <summary>
        /// �ؼ��󶨵Ķ�������
        /// </summary>
        public Type ObjectType
        {
            set
            { 
                _ObjectType = value;
                _ObjectTypeName = value.FullName + "," + value.Assembly.FullName;            
            }
            get { return _ObjectType; }
        }
        
        private string _ObjectTypeName;
        /// <summary>
        /// �󶨶�������ȫ��������������
        /// </summary>
        public string ObjectTypeName
        {
            set
            { 
                _ObjectTypeName = value;

                _ObjectType = Type.GetType( value );

                if (_ObjectType == null)
                    throw new ObjectMapException( "���ܻ�ȡ["+value+"]����" );            
            }
            get { return _ObjectTypeName; }
        }

        /// <summary>
        /// ���UI
        /// </summary>
        /// <param name="data"></param>
        public void UpdateUI(object data)
        {
            if (this.EnableViewState)
            {
                base.ViewState[data.GetType().FullName] = data;
            }

            foreach (Parameter p in _Parameters)
            {
                if (p.ParameterDirection == ParameterDirection.Output || p.ParameterDirection == ParameterDirection.InputOutput )
                    p.UpdateUI(data, this);
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateObject(object obj)
        {
            Type t = obj.GetType();           

            System.Web.HttpContext ctx = System.Web.HttpContext.Current;

            foreach (Parameter p in _Parameters)
            {
                if (p.ParameterDirection == ParameterDirection.Output || p.ParameterDirection == ParameterDirection.None || String.IsNullOrEmpty( p.Name ) )
                    continue;

                object objValue = p.Evaluate(ctx, this);

                if (p.DefaultValue != null && ( objValue == null || objValue.ToString() == "" ) ) //��������Ĭ��ֵ�������루""��null����Ϊ�û�δ���룩
                    objValue = p.DefaultValue;

                System.Reflection.PropertyInfo prop;

                try
                {
                    prop = t.GetProperty(p.Name);
                }
                catch //(Exception ex)
                {
                    throw new ObjectMapException( t.FullName + "������[ "+p.Name+" ]����" , p  );
                }

                if( prop == null )
                    throw new ObjectMapException(t.FullName + "������[ " + p.Name + " ]����", p);

                try
                {
                    object typeValue = convertValue(objValue, prop);

                    prop.SetValue(obj, typeValue, null);
                }
                catch (Exception ex)
                {
                    throw new ObjectMapException(p, objValue, prop.PropertyType, ex);
                }
            }
        }

        private object convertValue(object initValue, PropertyInfo p )
        {
            object typeValue = null;

            if (p.PropertyType.IsEnum)
            {
                typeValue = Enum.Parse(p.PropertyType, initValue.ToString(), true);
            }
            //else if (p.PropertyType == typeof(DateTime))
            //{
            //    m = new DateTimeProperty(p);
            //}
            //else if (p.PropertyType.IsValueType || p.PropertyType == typeof(System.Byte[]))
            //{
            //    m = new SorPropertyMember(p);
            //}
            else if (p.PropertyType == typeof(string))
            {
                typeValue = initValue;
            }
            else
            {
                typeValue = Convert.ChangeType(initValue, p.PropertyType);
            }

            return typeValue ;
        }

        /// <summary>
        /// ��ȡ����Ķ���,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Evaluate<T>( )  where T : class , new() 
        {
            if ( ObjectType != null && typeof( T ) != ObjectType )
            {
                throw new Exception( "��ֵ�����������["+_ObjectTypeName+"]��һ�£�" );
            }

            T obj = null ;

            if (this.EnableViewState)
            {
                obj = ViewState[ typeof(T).FullName ] as T ;
            }

            if (obj == null)
            {
                obj = new T();
            }
            
            UpdateObject(obj);

            return obj ;
        }

        //protected override bool EvaluateIsValid()
        //{
        //    //string text1 = base.GetControlValidationValue(base.ControlToValidate);
        //    //if ((text1 == null) || (text1.Trim().Length == 0))
        //    //{
        //    //    return true;
        //    //}
        //    //try
        //    //{
        //    //    Match match1 = Regex.Match(text1, this.ValidationExpression);
        //    //    return ((match1.Success && (match1.Index == 0)) && (match1.Length == text1.Length));
        //    //}
        //    //catch
        //    //{
        //    //    return true;
        //    //}

        //    return true;
        //}


        #region ��֤

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Page.Validators.Add(this);         
        }

        protected override void OnUnload(EventArgs e)
        {
            if (this.Page != null &&  EnableClientValidation )
            {
                this.Page.Validators.Remove(this);
            }
            base.OnUnload(e);
        }

        /// <summary>
        /// ע����֤�ű�
        /// </summary>
        protected virtual void RegisterValidatorDeclaration()
        {
            string text1 = "document.getElementById(\"" + this.ClientID + "\")";
            this.Page.ClientScript.RegisterArrayDeclaration("Page_Validators", text1);

            //control

            string temp = @"{{Control:{0}, Required:{1} , MaxLength:{2}, 
                            ErrorMessage : '{3}' , ErrorTypeMessage : '{4}' , ErrorMaxLengthMessage:'{5}',
                            ValidationExpression:{6},ClientValidationFunction:""{7}"" ,
                            MessagePanel : new ObjectMapper_MessagePanelWrapper('{8}') , AutoValid:{9} }}";

            foreach (Parameter p in this._Parameters)
            {
                if (p is ControlParameter)
                {
                    ControlParameter cp = p as ControlParameter;

                    string cpClientId = cp.GetMapControl(this).ClientID ;

                    string jsObj = "document.getElementById(\"" + cpClientId + "\")";

                    string json = string.Format(temp, jsObj, cp.Required ? "true" : "false" , cp.MaxLength,
                         GetErrMessage( cp ) ,
                         GetErrTypeMessage( cp ), 
                         GetErrorMaxLengthMessage( cp ) ,
                         GetValidationExpression( cp ) ,
                         cp.ClientValidationFunction ,
                         cpClientId,
                         cp.AutoValid ? "true" : "false"
                         );

                     this.Page.ClientScript.RegisterArrayDeclaration("ObjectMapper_Validators", json );                    
                }
            }

        }


        /// <summary>
        /// ��ȡ��֤������ʽ
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        private string GetValidationExpression( ControlParameter cp )
        {
            if (String.IsNullOrEmpty(cp.ValidationExpression))
            {
                if (cp.ValidationType == ValidationType.None && _ObjectType != null && !String.IsNullOrEmpty(cp.Name) ) //���󶨵Ķ����������ͻ�ȡ����֤����
                {
                    System.Reflection.PropertyInfo prop = _ObjectType.GetProperty(cp.Name);

                    if (prop == null) throw new ObjectMapException( this.ObjectTypeName + "�в���������["+ cp.Name +"]" , cp);

                    if (prop.PropertyType == typeof(String)) 
                        return "''";
                    else if (prop.PropertyType == typeof(Int16) || prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Int64))
                        return cp.GetValidationExpression(ValidationType.Integer);
                    else if (prop.PropertyType == typeof(Double) || prop.PropertyType == typeof(Single) || prop.PropertyType == typeof(System.Decimal))
                        return cp.GetValidationExpression(ValidationType.Double);
                    else
                        return "''";
                }

                return "''";
            }
            else
                return cp.ValidationExpression;
        }

        private string GetErrMessage(ControlParameter cp)
        {
            if (string.IsNullOrEmpty(cp.ErrorMessage))
                return this.ErrorMessage;
            else
                return cp.ErrorMessage;
        }

        private string GetErrTypeMessage(ControlParameter cp)
        {
            if (string.IsNullOrEmpty(cp.ErrorTypeMessage))
                return this.ErrorTypeMessage;
            else
                return cp.ErrorTypeMessage;
        }

        private string GetErrorMaxLengthMessage(ControlParameter cp)
        {
            if (string.IsNullOrEmpty(cp.ErrorMaxLengthMessage))
                return string.Format( this.ErrorMaxLengthMessage , cp.MaxLength ) ;
            else
                return string.Format(cp.ErrorMaxLengthMessage , cp.MaxLength);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (EnableClientValidation)
            {
                RegisterValidatorCommonScript();

                RegisterValidatorDeclaration();
            }
        }

        //protected bool EnableLegacyRendering
        //{
        //    get
        //    {
        //        Page page1 = this.Page;
        //        if (page1 != null)
        //        {
        //            return (page1.XhtmlConformanceMode == XhtmlConformanceMode.Legacy);
        //        }
        //        if (!this.DesignMode && (this.Adapter == null))
        //        {
        //            return (this.GetXhtmlConformanceSection().Mode == XhtmlConformanceMode.Legacy);
        //        }
        //        return false;
        //    }
        //} 

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="controlId"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="encode"></param>
        protected void AddExpandoAttribute( HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
        {
            if (writer != null)
            {
                writer.AddAttribute(attributeName, attributeValue, encode);
            }
            else
            {
                Page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
            }
        }

        /// <summary>
        /// ���ϵͳ��֤������Ҫ����չ����
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            //writer.AddAttribute("evaluationfunction", "ObjectMapper_Valid");
            //writer.AddAttribute("validationGroup", ValidationGroup );

            //HtmlTextWriter writer1 = base.EnableLegacyRendering ? writer : null;

            AddExpandoAttribute( null, this.ClientID, "evaluationfunction", _ValidFunction, false );

            if (!String.IsNullOrEmpty(ValidationGroup))
                AddExpandoAttribute( null, this.ClientID, "validationGroup", ValidationGroup, false);        

        }               

        /// <summary>
        /// �����֤ͨ��js
        /// </summary>
        protected void RegisterValidatorCommonScript()
        {
            Type baseValidatorType = typeof(System.Web.UI.WebControls.BaseValidator);

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(baseValidatorType, "ValidatorIncludeScript"))
            {
                this.Page.ClientScript.RegisterClientScriptResource(baseValidatorType, "WebUIValidation.js");
                this.Page.ClientScript.RegisterStartupScript(baseValidatorType, "ValidatorIncludeScript", "\r\n<script type=\"text/javascript\">\r\n<!--\r\nvar Page_ValidationActive = false;\r\nif (typeof(ValidatorOnLoad) == \"function\") {\r\n    ValidatorOnLoad();\r\n}\r\n\r\nfunction ValidatorOnSubmit() {\r\n    if (Page_ValidationActive) {\r\n        return ValidatorCommonOnSubmit();\r\n    }\r\n    else {\r\n        return true;\r\n    }\r\n}\r\n// -->\r\n</script>\r\n        ");
                this.Page.ClientScript.RegisterOnSubmitStatement(baseValidatorType, "ValidatorOnSubmit", "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;");
            }

            //ResourceHelper.RegisterClientScript(typeof(ObjectMapper), this);

            Page.ClientScript.RegisterClientScriptResource(typeof(ObjectMapper), JS_RESOURCE_FILE_PATH);

            string jsKey = "ObjectMapper-Var";

            if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(ObjectMapper), jsKey))
            {
                //Page.ClientScript.RegisterClientScriptBlock(typeof(ObjectMapper), jsKey, ResourceHelper.GetJsResourceString(typeof(ObjectMapper)));
                string varJs = @"
                                <SCRIPT lang=""javascript"">
                                ObjectMapper_MessagePanelCssClass='{0}';
                                ObjectMapper_MessagePanelWarningImageUrl='{1}';    
                                ObjectMapper_MessagePanelCloseImageUrl='{2}';    
                                </SCRIPT>" ;

                string imgPath = base.ResolveUrl( "~/Images/ObjectMapper/" );

                Page.ClientScript.RegisterClientScriptBlock(typeof(ObjectMapper), jsKey ,
                    String.Format(varJs, MessagePanelCssClass, imgPath + "alert-small.gif", imgPath + "close.gif") 
                    );
            }
        }

        private String _ValidFunction = "ObjectMapper_Valid";
        [Themeable(true)]
        public String ValidFunction
        {
            get { return _ValidFunction; }
            set { _ValidFunction = value; }
        }

        private string _MessagePanelCssClass = "ObjectMapper-MessagePanel";
        /// <summary>
        /// ��Ϣ����ʽ
        /// </summary>
        /// 
        [Themeable(true)]
        public virtual string MessagePanelCssClass
        {
            get
            {
                return _MessagePanelCssClass ;
            }
            set
            {
                _MessagePanelCssClass = value;
            }
        }

      //  [WebSysDescription("BaseValidator_ValidationGroup"), WebCategory("Behavior"), Themeable(false), DefaultValue("")]
        protected string _ValidationGroup = "" ;
        /// <summary>
        /// ��֤��
        /// </summary>
        public virtual string ValidationGroup
        {
            get
            {
                //object obj1 = this.ViewState["ValidationGroup"];
                //if (obj1 != null)
                //{
                //    return (string)obj1;
                //}
                return _ValidationGroup ;
            }
            set
            {
                _ValidationGroup = value;
            }
        }
 
        #region IValidator ��Ա     

        public bool IsValid
        {
            get
            {
                return true;
            }
            set
            {

            }
        }

        public void Validate()
        {

        }

        private string _ErrorMessage = "�������룡";
        /// <summary>
        /// Ϊ��ʱ��Ϣ
        /// </summary>
        [Themeable(true)]
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        private string _ErrorTypeMessage = "�������ʹ���";
        /// <summary>
        /// ���ݸ�ʽ����ʱ��Ϣ
        /// </summary>
        [Themeable(true)]
        public string ErrorTypeMessage
        {
            get
            {
                return _ErrorTypeMessage;
            }
            set
            {
                _ErrorTypeMessage = value;
            }
        }

        private string _ErrorMaxLengthMessage = "���Ȳ��ܳ���{0}";
        /// <summary>
        /// ���ݸ�ʽ����ʱ��Ϣ
        /// </summary> 
        [Themeable(true)]
        public string ErrorMaxLengthMessage
        {
            get
            {
                return _ErrorMaxLengthMessage;
            }
            set
            {
                _ErrorMaxLengthMessage = value;
            }
        }

        private bool _EnableClientValidation = true ;
        /// <summary>
        /// �Ƿ����ÿͻ�����֤
        /// </summary>
        [Themeable(true)]
        public bool EnableClientValidation
        {
            get
            {
                return _EnableClientValidation;
            }
            set
            {
                _EnableClientValidation = value;
            }
        }

        #endregion

        #endregion

       
        
    }

    
}
