using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web
{
    /// <summary>
    /// UI�ؼ�-����ӳ���쳣
    /// </summary>
    public class ObjectMapException : Exception
    {
        public ObjectMapException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">�쳣��Ϣ</param>
        /// <param name="p">�����쳣�Ĳ���</param>
        public ObjectMapException(string message, Parameter p)
            : base(message)
        {
            _Parameter = p;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">�����쳣�Ĳ���</param>
        /// <param name="initValue">UIֵ</param>
        /// <param name="toType">������������</param>
        /// <param name="innerException"></param>
        public ObjectMapException(Parameter p, object initValue, Type toType,
            Exception innerException)

            : base("���ܽ�[ " + initValue + " ]ת��Ϊ����[ " + toType + " ] ", innerException)
        {
            _Parameter = p;
        }

        private Parameter _Parameter;
        /// <summary>
        /// �����쳣�Ĳ��� 
        /// </summary>
        public Parameter Parameter
        {
            get
            {
                return _Parameter;
            }
        }
    }
    
}
