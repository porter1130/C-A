//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.

// �ļ�����ComponentMetaDataFactctory.cs
// �ļ��������������Ԫ���ݹ��� 
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
    public static class ComponentMetaDataFactctory
    {
        private static readonly IDictionary<string, ComponentSet> _ComponentSets = new Dictionary<string, ComponentSet>();

        static public ComponentSet GetMetaData(Type t)
        {
            return new ComponentSet(t); //��ʱ���û���

            if ( _ComponentSets.ContainsKey(t.FullName) )
            {
                return _ComponentSets[t.FullName];
            }
            else
            {
                ComponentSet set = new ComponentSet( t);

                try
                {
                    _ComponentSets.Add(t.FullName, set);
                }
                catch { } //��ֹ���߳�����

                return set;
            }
        }

    }
}
