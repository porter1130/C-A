//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.

// �ļ�����FieldEditorFactory.cs
// �ļ�����������������������һ����������Խ��б༭ ,֧�ֶ���������ݲ�֧���б�����
// 
// 
// ������ʶ�� �Ž��� 2007-7-3
//
// �޸ı�ʶ��
// �޸�������
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using CA.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using CA.Web.ComponentDesign;

namespace CA.Web
{
    /// <summary>
    /// ��̬�����ؼ��¼�����
    /// </summary>
    public class ControlCreatedEventArgs : EventArgs
    {
        internal ControlCreatedEventArgs(System.Web.UI.WebControls.WebControl ctl, FieldSet f)
        {
            Control = ctl;
            FieldSet = f;
        }
        /// <summary>
        /// �������Ŀؼ� 
        /// </summary>
        public readonly System.Web.UI.WebControls.WebControl Control;

        /// <summary>
        /// �ؼ���Ӧ���ֶ���Ϣ 
        /// </summary>
        public readonly FieldSet FieldSet;
    }

    /// <summary>
    /// ��̬�����ؼ��¼�����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ControlCreatedEventHandler(object sender, ControlCreatedEventArgs e);
    
    /// <summary>
    /// �����UI�������
    /// </summary>
    public enum BuilderType
    {
        /// <summary>
        /// ��񲼾�
        /// </summary>
        Layoutable,

        /// <summary>
        /// ��β���
        /// </summary>
        Hierarchial
    }

    /// <summary>
    /// ��������
    /// </summary>
    [ParseChildren(true)]
    public class ComponentDesignControl : Table, System.Web.UI.INamingContainer, IComponentDesigner
    {
        const string DESIENED_OBJECT = "DESIENED_OBJECT";

        private bool _ReadOnly = false;
        /// <summary>
        /// �Ƿ�ֻ��
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
            }
        }

        private HierarchicalUIBuilder _HierarchicalUIBuilder = new HierarchicalUIBuilder ();
        /// <summary>
        /// 
        /// </summary>
        public HierarchicalUIBuilder HierarchicalUIBuilder
        {
            get { return _HierarchicalUIBuilder; }
        }

        private LayoutableUIBuilder _LayoutableUIBuilder = new LayoutableUIBuilder();
        public LayoutableUIBuilder LayoutableUIBuilder
        {
            get
            {
                return _LayoutableUIBuilder;
            }
        }

        private string _NameCellCssClass;
        /// <summary>
        /// �ֶ�����Ԫ����ʽ
        /// </summary>
        public string NameCellCssClass
        {
            get { return _NameCellCssClass; }
            set { _NameCellCssClass = value; }
        }

        private string _ValueCellCssClass;
        /// <summary>
        /// �ֶ�ֵ��Ԫ����ʽ
        /// </summary>
        public string ValueCellCssClass
        {
            get { return _ValueCellCssClass; }
            set { _ValueCellCssClass = value; }
        }

        private string _FieldNameFormatString;
        /// <summary>
        /// �ֶ�����ʽ���ַ���
        /// </summary>
        public string FieldNameFormatString
        {
            get { return _FieldNameFormatString; }
            set { _FieldNameFormatString = value; }
        }

        private ComponentSet _ComponentSet;
        private object _DesignedObject;

        private IDesignerUIBuilder _UIBuilder;
        /// <summary>
        /// ���ص�ǰUIBuilder
        /// </summary>
        internal IDesignerUIBuilder UIBuilder 
        {
            get
            {
                if (_UIBuilder == null)
                {
                    if (_BuilderType == BuilderType.Hierarchial)
                        _UIBuilder = _HierarchicalUIBuilder;
                    else
                        _UIBuilder = _LayoutableUIBuilder;
                }

                return _UIBuilder;
            }
        }

        private BuilderType _BuilderType=BuilderType.Hierarchial;
        /// <summary>
        /// ����Ĳ�������
        /// </summary>
        [Browsable(true)]
        public BuilderType BuilderType
        {
            get
            {
                return _BuilderType;
            }
            set 
            {
                _BuilderType = value;
                if (_BuilderType == BuilderType.Hierarchial)
                    _UIBuilder = _HierarchicalUIBuilder;
                else 
                    _UIBuilder = _LayoutableUIBuilder;
            }
        }

        //��¼���б༭�ؼ�
        private IDictionary<string, IFiledEditor> FieldSetEditors = new Dictionary<string, IFiledEditor>();

        //private string _LayoutString;
        ///// <summary>
        ///// �����ַ���,����','����ֶΣ�|���� Name,Sex,Birthday|UserType 
        ///// </summary>
        //public string LayoutString
        //{
        //    get { return _LayoutString; }
        //    set { _LayoutString = value; }
        //}

        //private List<LayoutField> _LayoutFields=new List<LayoutField>();
        ///// <summary>
        ///// ������ֶεĲ��ֶ���ļ���
        ///// </summary>        
        //public List<LayoutField> LayoutFields
        //{
        //    get
        //    {
        //        return _LayoutFields;
        //    }
        //    set
        //    {
        //        _LayoutFields = value;
        //    }
        //}

        private int _RepeatColumns = 1;
        /// <summary>
        /// ���ظ�����2�У�4�У�6�У�����Ϊż��
        /// </summary>
        public int RepeatColumns
        { 
            get { return _RepeatColumns; }
            set 
            {
                 _RepeatColumns = value; 
            }
        }

        /// <summary>
        /// ��̬�����ؼ��¼�
        /// </summary>
        public event ControlCreatedEventHandler ControlCreated;

        internal void RaiseControlCreatedEvent(System.Web.UI.WebControls.WebControl ctl, FieldSet f)
        {
            if (ControlCreated != null)
                ControlCreated(this, new ControlCreatedEventArgs(ctl, f));
        }

        public void ShowComponent(object obj)
        {
            _ComponentSet = ComponentMetaDataFactctory.GetMetaData(obj.GetType());
            //BuildReadonlyUI(_ComponentSet, obj);
            UIBuilder.BuildViewUI(this, _ComponentSet, obj);
        }

        /// <summary>
        /// ��ʼ��������UI
        /// </summary>
        /// <param name="obj"></param>
        public void EditComponent(object obj)
        {
            this.Rows.Clear();

            Type t = obj.GetType();
            _DesignedObject = obj;

            _ComponentSet = ComponentMetaDataFactctory.GetMetaData(t);

            this.ViewState[DESIENED_OBJECT] = obj;

            if( ReadOnly )
                UIBuilder.BuildViewUI(this, _ComponentSet, _DesignedObject);
            else
                this.FieldSetEditors = UIBuilder.BuildEditUI(this, _ComponentSet, _DesignedObject);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page.IsPostBack)
            {
                InitControls();
            }
        }

        private void InitControls()
        {
            if (this.ViewState[DESIENED_OBJECT] != null)
            {
                this.Rows.Clear();

                _DesignedObject = this.ViewState[DESIENED_OBJECT];

                _ComponentSet = ComponentMetaDataFactctory.GetMetaData(_DesignedObject.GetType());

                if (ReadOnly)
                    UIBuilder.BuildViewUI(this, _ComponentSet, _DesignedObject);
                else
                    this.FieldSetEditors = UIBuilder.BuildRetrieveUI(this, _ComponentSet);
            }
        }

        /// <summary>
        /// ����������ֵ
        /// </summary>
        /// <param name="obj"></param>
        public object GetComponent()
        {
            if (_ComponentSet == null)
                throw new Exception("not define edit control.");

            IDictionary<string, object> dic = new Dictionary<string, object>();

            foreach (KeyValuePair<string, IFiledEditor> kv in this.FieldSetEditors)
            {
                dic.Add(kv.Key, kv.Value.FieldValue);
            }

            _ComponentSet.UpdateValue(dic, _DesignedObject);

            return _DesignedObject;
        }

    }

   
}