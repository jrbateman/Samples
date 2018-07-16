using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    public class AXRESTDataModelConvert
    {
        public static T DeserializeObject<T>(string objString, string mediatype)
        {
            if (mediatype.Contains("json"))
            {
                return JsonConvert.DeserializeObject<T>(objString);
            }
            else if (mediatype.Contains("xml"))
            {
                var serializer = new XmlSerializer(typeof(T));
                StringReader rdr = new StringReader(objString);
                return (T)serializer.Deserialize(rdr);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string SerializeObject(object target, string mediatype)
        {
            if (mediatype.Contains("json"))
            {
                return JsonConvert.SerializeObject(target);
            }
            else if (mediatype.Contains("xml"))
            {
                var serializer = new XmlSerializer(target.GetType());
                using (StringWriter sww = new StringWriter())
                using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings() { OmitXmlDeclaration = true}))
                {
                    serializer.Serialize(writer, target);
                    return sww.ToString(); 
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
