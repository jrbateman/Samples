using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AX Doc ODMA resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocODMA : LinkedResource
    {
        /// <summary>
        /// Document name/title
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }
        /// <summary>
        /// Document author
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Author { get; set; }
        /// <summary>
        /// Document subject
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Subject { get; set; }
        /// <summary>
        /// Document comment
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Comment { get; set; }
        /// <summary>
        /// Document creation time
        /// </summary>
        [XmlElement]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? Created { get; set; }
        /// <summary>
        /// Document creator
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Creator { get; set; }
        /// <summary>
        /// Document modification time
        /// </summary>
        [XmlElement]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? Modified { get; set; }
        /// <summary>
        /// Document modifier
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Modifier { get; set; }
        /// <summary>
        /// Document keywords
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Keywords { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDocODMA()
        {

        }
    }
}
