using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// ���ֶ���
    /// </summary>
    public class LayoutField
    {
        private string _FieldKey;
        /// <summary>
        /// �ֶιؼ���
        /// </summary>
        public string FieldKey
        {
            get
            {
                return _FieldKey;
            }
            set
            {
                _FieldKey = value;
            }
        }
        private int _OrderPosition;
        /// <summary>
        /// �ֶ�������λ��
        /// </summary>
        public int OrderPosition
        {
            get
            {
                return _OrderPosition;
            }
            set
            {
                _OrderPosition = value;
            }
        }
        private string _ChsDisplayName;
        /// <summary>
        /// ������ʾ��
        /// </summary>
        public string ChsDisplayName
        {
            get
            {
                return _ChsDisplayName;
            }
            set
            {
                _ChsDisplayName = value;
            }
        }
    }
}
