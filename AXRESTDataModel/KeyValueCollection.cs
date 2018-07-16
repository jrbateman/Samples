using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    public class KeyValueCollection : Dictionary<string, string>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Clear();
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                string key = reader.LocalName;
                // Decode the element name
                string name = XmlConvert.DecodeName(key);
                reader.ReadStartElement();
                string val = reader.Value;
                Add(name, val);
                if (!reader.Read())
                    break;
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (KeyValuePair<string, string> kv in this)
            {
                // Elelment names must be properly encoded
                string name = XmlConvert.EncodeName(kv.Key);
                writer.WriteStartElement(name);
                writer.WriteValue(kv.Value);
                writer.WriteEndElement();
            }
        }
    }

    public class KeyBooleanCollection : Dictionary<string, bool>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Clear();
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                string key = reader.LocalName;
                reader.ReadStartElement();
                bool val = bool.Parse(reader.Value);
                Add(key, val);
                if (!reader.Read())
                    break;
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (KeyValuePair<string, bool> kv in this)
            {
                writer.WriteStartElement(kv.Key);
                writer.WriteValue(kv.Value);
                writer.WriteEndElement();
            }
        }
    }
}
