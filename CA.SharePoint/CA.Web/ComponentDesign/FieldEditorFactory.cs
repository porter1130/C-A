//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.

// �ļ�����FieldEditorFactory.cs
// �ļ������������༭���������������Ƶı༭��ʵ�� 
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
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    internal static class FieldEditorFactory
    {
        static public System.Web.UI.WebControls.WebControl GetFieldEditor(FieldSet f)
        {
            //if( f.CheckValues != null && f.CheckValues.Length > 0 )
            //    return 

            if (f.EditorType == EditorType.Auto)
            {
                if (f.Type == typeof(DateTime))
                    return new DateTimeFiledEditor();

                if (f.Type == typeof(Boolean))
                    return new BooleanFiledEditor();

                if (f.Type.IsEnum)
                    return new EnumFiledEditor(f);

                return new StringFiledEditor();
            }
            else
            {
                if (f.EditorType == EditorType.DateTime)
                    return new DateTimeFiledEditor();

                if (f.EditorType == EditorType.Radio)
                {
                    if (f.Type.IsEnum)
                    {
                        return new RadioEnumEditor(f);
                    }
                    else if (f.CheckValues != null && f.CheckValues.Length > 0)
                    {
                        return new RadioCheckValuesEditor(f);
                    }
                    else
                    {
                        throw new NotSupportedException("[" + f.UniqueName + "]��ֻ����ö�����ͻ�ָ����Լ���ſ�����Radio�༭��");
                    }
                }

                if (f.EditorType == EditorType.Checkbox)
                {
                    //if (f.CheckValues == null || f.CheckValues.Length == 0)
                    //{
                    //    throw new NotSupportedException("[" + f.UniqueName + "]��ֻ��ָ����Լ���ſ�����Checkbox�༭��");
                    //}

                    return new CheckboxEditor(f);
                }

                if (f.EditorType == EditorType.DropDownList)
                {
                    if (f.Type.IsEnum)
                    {
                        return new EnumFiledEditor(f);
                    }
                    else if (f.CheckValues != null && f.CheckValues.Length > 0)
                    {
                        return new DropDownCheckValuesEditor(f);
                    }
                    else
                    {
                        throw new NotSupportedException("[" + f.UniqueName + "]��ֻ����ö�����ͻ�ָ����Լ���ſ�����DropDownList�༭��");
                    }
                }

                //if (f.EditorType == EditorType.Resource)
                //{
                //    if (f.EditorArgs == null)
                //    {
                //        throw new NotSupportedException("[" + f.UniqueName + "]��Resource�༭����Ҫ��Դ��ʾ��Ϣ");
                //    }

                //    return new ResourceEditor(f);
                //}


                return new StringFiledEditor();
            }
        }
    }

}
