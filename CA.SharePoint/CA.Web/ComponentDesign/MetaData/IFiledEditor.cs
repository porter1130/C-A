//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.

// �ļ�����IFiledEditor.cs
// �ļ�����������������Ա༭���ӿ�
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

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// �༭������
    /// </summary>
    public enum EditorType
    {
        Auto ,

        DateTime ,

        Resource ,

        Radio ,

        List ,

        DropDownList ,

        Checkbox 
    }

    /// <summary>
    /// �ֶα༭��,�пؼ���Ŀʵ��
    /// </summary>
    public interface IFiledEditor
    {
        object FieldValue
        {
            set;
            get;
        }
    }
}
