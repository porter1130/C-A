//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.

// �ļ�����Attributes.cs
// �ļ�����������������Ԫ���� 
// 
// 
// ������ʶ�� �Ž��� 2007-7-3
//
// �޸ı�ʶ��
// �޸�������
//----------------------------------------------------------------

using System;
using System.Collections;
using System.Text;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// �������ʱ����
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EditorAttribute : System.Attribute
    {         
        /// <summary>
        /// ��ʾ��
        /// </summary>
        public string DisplayName ;

        /// <summary>
        /// ��󳤶�
        /// </summary>
        public int MaxLength = -1;

        //public bool IsNullable = true ;

        /// <summary>
        /// У������
        /// </summary>
        public ValidationType ValidationType = ValidationType.Auto ;

        /// <summary>
        /// У��������ʽ
        /// </summary>
        public string ValidationExpression = "" ;

        /// <summary>
        /// Լ��ֵ
        /// </summary>
        public object[] CheckValues ;         

        /// <summary>
        /// Լ��ֵ�ṩ����
        /// </summary>
        public Type CheckValuesProvider;

        /// <summary>
        /// �ֶα༭������
        /// </summary>
        public EditorType EditorType = EditorType.Auto ;

        /// <summary>
        /// �ֶα༭������
        /// </summary>
        public object[] EditorArgs = null ;   
     
        /// <summary>
        /// ���ʱ����
        /// </summary>
        public bool Ignore = false ;

        public int Sequence = 0;

    }

    /// <summary>
    /// Լ��ֵ�ṩ����ӿ�
    /// </summary> 
    public interface ICheckValuesProvider
    {
        System.Collections.IDictionary GetCheckValues();         
    }

    internal class SimpleCheckValuesProvider : ICheckValuesProvider
    {
        private object[] _values;

        public SimpleCheckValuesProvider(object[] values)
        {
            _values = values;
        }

        #region ICheckValuesProvider ��Ա

        public System.Collections.IDictionary GetCheckValues()
        {
            IDictionary dic = new System.Collections.Specialized.ListDictionary();

            foreach (object o in _values)
            {
                dic.Add( o , o );
            }

            return dic;                 
        }

        #endregion
    }

    /// <summary>
    /// ��֤����
    /// </summary>
    //public enum ValidationType
    //{
    //    Auto = 20 ,

    //    Integer = 2,

    //    Double = 3,

    //    Currency= 4,

    //    //Date ,

    //    //Time ,

    //    DateTime= 5,

    //    Email= 6,

    //    Phone= 7,

    //    Mobile= 8,

    //    //Url,

    //    IdCard= 9,

    //    Number= 10,

    //    //Zip,

    //    //QQ,

    //    English= 11,

    //    Chinese= 12,

    //    // UnSafe ,

    //}

}
