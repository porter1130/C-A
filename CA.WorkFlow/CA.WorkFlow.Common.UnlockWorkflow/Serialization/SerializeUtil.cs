using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace CA.WorkFlow.Common.UnlockWorkflow.Serialization
{
    public class SerializeUtil
    {
        public static string Serialize(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream w = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(w, Encoding.UTF8);
            serializer.Serialize((XmlWriter)writer, obj);
            return Encoding.UTF8.GetString(w.ToArray()).Trim();
        }

        public static object Deserialize(Type t, string xml)
        {
            XmlSerializer serializer = new XmlSerializer(t);
            StringReader textReader = new StringReader(xml);

            return serializer.Deserialize(textReader);
        }
    }
}