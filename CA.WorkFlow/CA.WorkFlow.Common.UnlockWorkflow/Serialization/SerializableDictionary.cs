using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;

namespace CA.WorkFlow.Common.UnlockWorkflow.Serialization
{
    public class SerializableDictionary<TKey, TValue> : Dictionary<string, string>, IXmlSerializable
    {

        public SerializableDictionary()
        {
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();

            bool isEmptyElement = reader.IsEmptyElement;

            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    //reader.ReadStartElement();
                    base.Add(reader.Name, reader.ReadElementString());
                    //reader.ReadEndElement();
                }

                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (string local in base.Keys)
            {
                writer.WriteElementString(local, base[local]);
            }
        }

    }
}