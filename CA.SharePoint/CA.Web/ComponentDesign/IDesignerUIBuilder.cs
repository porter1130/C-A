//----------------------------------------------------------------
// Copyright (C) 2005 �Ϻ�������������޹�˾
// ��Ȩ���С� 
// All rights reserved.
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
    /// <summary>
    /// �����UI�����߼�
    /// </summary>
    interface IDesignerUIBuilder
    {
        /// <summary>
        /// �����༭UI
        /// </summary>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, IFiledEditor> BuildEditUI(ComponentDesignControl designer, ComponentSet set, object obj);

        /// <summary>
        /// �����ط���ԭUI,ֻ��Ҫ�����ؼ�������Ҫ���ؼ�ֵ
        /// </summary>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, IFiledEditor> BuildRetrieveUI(ComponentDesignControl designer, ComponentSet set);

        /// <summary>
        /// ����ֻ���鿴UI
        /// </summary>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        void BuildViewUI(ComponentDesignControl designer, ComponentSet set, object obj);
    }
}
