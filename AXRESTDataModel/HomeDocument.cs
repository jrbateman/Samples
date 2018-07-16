using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    public class HomeDocument
    {
        public HomeDocument()
        {

        }

        [XmlIgnore]
        public virtual Dictionary<string, string> ResourcesLinks { get { return new Dictionary<string, string>(); } }
    }

    public class HomeDocumentJson : HomeDocument
    {
        public HomeDocumentJson()
        {

        }

        [JsonProperty(PropertyName = "resources")]
        public Dictionary<string, EmbeddedResource> Links { get; set; }

        [JsonIgnore]
        public override Dictionary<string, string> ResourcesLinks
        {
            get
            {
                var links = new Dictionary<string, string>();
                foreach(var kvp in Links)
                {
                    links[kvp.Key] = kvp.Value.HRef;
                }
                return links;
            }
        }
    }

    public class EmbeddedResource
    {
        public EmbeddedResource()
        {

        }

        [JsonProperty(PropertyName = "href")]
        public string HRef { get; set; }

        [JsonProperty(PropertyName = "hints")]
        public Hints Hints { get; set; }
    }

    public class Hints
    {
        public Hints()
        {

        }

        [JsonProperty(PropertyName = "allow")]
        public string[] Allow { get; set; }

        [JsonProperty(PropertyName = "representations")]
        public string[] Representations { get; set; }
    }

    [XmlRoot("resources", Namespace = "")]
    public class HomeDocumentXML : HomeDocument
    {
        [XmlElement("resource")]
        public EmbeddedResourceXML[] Resources;

        [XmlIgnore]
        public override Dictionary<string, string> ResourcesLinks
        {
            get
            {
                var links = new Dictionary<string, string>();
                foreach (var r in Resources)
                {
                    links[r.relation] = r.link.href;
                }
                return links;
            }
        }
    }


    public class EmbeddedResourceXML
    {
        [XmlAttribute("rel")]
        public string relation { get; set; }
        [XmlElement("link")]
        public LinkXML link { get; set; }
        [XmlElement("hints")]
        public HintsXML hints { get; set; }
    }

    public class LinkXML
    {
        [XmlAttribute("href")]
        public string href { get; set; }
    }

    public class HintsXML
    {
        [XmlElement("allow")]
        public AllowActionsXML allow { get; set; }
        [XmlElement("representations")]
        public RepresentationsXML representations { get; set; }
    }

    public class AllowActionsXML
    {
        [XmlElement("i")]
        public string[] items;

    }

    public class RepresentationsXML
    {
        [XmlElement("i")]
        public string[] items;
    }
}
