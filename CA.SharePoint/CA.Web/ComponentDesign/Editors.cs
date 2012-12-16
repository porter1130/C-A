//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.

// �ļ�����Editors.cs
// �ļ������������༭��ʵ�� 
// 
// 
// ������ʶ�� �Ž��� 2007-7-3
//
// �޸ı�ʶ��
// �޸�������
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CA.Web;
using System.Web.UI.WebControls;
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// �ַ����༭��
    /// </summary>
    internal class StringFiledEditor : TextBox, IFiledEditor
    {
        public StringFiledEditor()
        {
            this.Width = new Unit( "100%" );
        }    

        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value == null)
                    this.Text = "";
                else
                    this.Text = value.ToString();
            }
        }

        #endregion
    }

    internal class BooleanFiledEditor : CheckBox, IFiledEditor
    {
        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                return this.Checked;
            }
            set
            {
                if (value == null)
                    this.Checked = false;
                else
                    this.Checked = Convert.ToBoolean(value);
            }
        }

        #endregion
    }
    /// <summary>
    /// ʱ���ֶα༭��
    /// </summary>
    internal class DateTimeFiledEditor : DatePicker, IFiledEditor
    {
        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                if( value != null )
                    this.Value = Convert.ToDateTime(value);
            }
        }

        #endregion
    }

    /// <summary>
    /// ö���ֶα༭�������������б�ʵ��
    /// </summary>
    internal class EnumFiledEditor : DropDownList, IFiledEditor
    {
        private FieldSet _EditedField;
        public EnumFiledEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                if (String.IsNullOrEmpty(this.SelectedValue))
                    return null;
                else
                    return Enum.Parse(_EditedField.Type, this.SelectedValue);
            }
            set
            {
                if( value != null )
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //����ö���ֶ�
            FieldInfo[] fields = _EditedField.Type.GetFields();

            for (int i = 1; i < fields.Length; i++)//��Ҫ�ӵ�2����ʼ����һ��Ϊ������Ϣ
            {
                ListItem item = new ListItem();
                item.Value = fields[i].Name;
                item.Text = ComponentSet.GetDisplayName(fields[i]);
                this.Items.Add(item);
            }
        }

        #endregion
    }

    /// <summary>
    /// ö���ֶα༭�������õ�ѡ��ʵ��
    /// </summary>
    internal class RadioEnumEditor : RadioButtonList, IFiledEditor
    {
        private FieldSet _EditedField;
        public RadioEnumEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                return Enum.Parse(_EditedField.Type, this.SelectedValue);
            }
            set
            {
                if( value != null )
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            FieldInfo[] fields = _EditedField.Type.GetFields();

            for (int i = 1; i < fields.Length; i++)
            {
                ListItem item = new ListItem();
                item.Value = fields[i].Name;
                item.Text = ComponentSet.GetDisplayName(fields[i]);
                this.Items.Add(item);
            }
        }

        #endregion
    }

    /// <summary>
    /// Լ���ֶα༭�������õ�ѡ��ʵ��
    /// </summary>
    internal class RadioCheckValuesEditor : RadioButtonList, IFiledEditor
    {
        private FieldSet _EditedField;
        public RadioCheckValuesEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                if (value != null)
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EditorHelper.BuildListItem(_EditedField, this);
        }

        #endregion
    }

    internal class CheckboxEditor : CheckBoxList, IFiledEditor
    {
        private FieldSet _EditedField;
        public CheckboxEditor(FieldSet f)
        {
            _EditedField = f;
        }

        public object FieldValue
        {
            get
            {
                return EditorHelper.GetSelectedValue(this);
                 
            }
            set
            {
                EditorHelper.SetSelectedValue(this,value);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.RepeatDirection = RepeatDirection.Horizontal;

            EditorHelper.BuildListItem(_EditedField, this);
            

            //for (int i = 0; i < _EditedField.CheckValues.Length; i++)
            //{
            //    ListItem item = new ListItem();
            //    item.Text = _EditedField.CheckValues[i].ToString();
            //    this.Items.Add(item);
            //}
        }
    }

    //internal class ResourceEditor : DropDownList, IFiledEditor
    //{
    //    private FieldSet _EditedField;
    //    public ResourceEditor(FieldSet f)
    //    {
    //        _EditedField = f;
    //    }

    //    public object FieldValue
    //    {
    //        get
    //        {
    //            return this.SelectedValue;
    //        }
    //        set
    //        {
    //            if (value != null)
    //                this.SelectedValue = value.ToString();
    //        }
    //    }

    //    protected override void OnInit(EventArgs e)
    //    {
    //        base.OnInit(e);

    //        object[] args = (object[])_EditedField.EditorArgs;

    //        if (args.Length == 1) //����������ָֻ������Դ��ʶ
    //        {
    //            this.ResourceType = args[0].ToString();

    //        }
    //        else if (args.Length == 2)
    //        {
    //            this.ResourceName = args[0].ToString();
    //            this.ResourceType = args[1].ToString();
    //        }
    //    }
    //}

    internal class DropDownCheckValuesEditor : DropDownList, IFiledEditor
    {
        private FieldSet _EditedField;
        public DropDownCheckValuesEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor ��Ա

        public object FieldValue
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                if (value != null)
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EditorHelper.BuildListItem(_EditedField, this);

            //for (int i = 1; i < _EditedField.CheckValues.Length; i++)
            //{
            //    ListItem item = new ListItem();
            //    item.Text = _EditedField.CheckValues[i].ToString();
            //    this.Items.Add(item);
            //}
        }


        #endregion
    }

    /// <summary>
    /// �༭��������
    /// </summary>
    internal static class EditorHelper
    {
        /// <summary>
        /// �����б���
        /// 
        /// ������Լ��ֵ�����Լ��ֵ������
        /// ������Լ��ֵ�ṩ���������֮������
        /// �����׳��쳣
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static void BuildListItem( FieldSet f , ListControl ctl )
        {
            if (f.CheckValues != null && f.CheckValues.Length > 0)
            {
                for (int i = 0; i < f.CheckValues.Length; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = f.CheckValues[i].ToString();
                    ctl.Items.Add( item );
                }
                 
            }
            else if (f.CheckValuesProvider != null)
            {
                IDictionary dic = f.CheckValuesProvider.GetCheckValues();

                foreach( DictionaryEntry d in dic )
                {
                    ListItem item = new ListItem() ;

                    item.Value = d.Key.ToString();
                    item.Text = d.Value.ToString();

                    ctl.Items.Add(item);
                }               

           }
            else
            {
                throw new NotSupportedException("[" + f.UniqueName + "]û��ָ��Լ��ֵ��Լ��ֵ�ṩ�����޷������б���");
            }
        }

        public static string GetSelectedValue(ListControl ctl )
        {
            string v = "";

            foreach (ListItem i in ctl.Items )
            {
                if (i.Selected)
                {
                    if (v != "") v += ",";

                    v += i.Value;
                }
            }

            return v;
        }

        public static void SetSelectedValue(ListControl ctl,object value )
        {
            if (value == null)
                return;

            string v = "," + value + ",";

            if (v == ",,") return;

            foreach (ListItem i in ctl.Items)
            {
                if (v.IndexOf("," + i.Value + ",") != -1)
                    i.Selected = true;                
            }
        }
    }


}
