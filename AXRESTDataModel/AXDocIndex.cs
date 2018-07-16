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
    /// AX Doc Index collection with pagenation
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocIndexes : PaginatedCollection<AXDocIndex>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXDocIndexes()
            : base()
        {
        }
    }

    /// <summary>
    /// AX Doc Index 
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocIndex : LinkedResource
    {
        /// <summary>
        /// Index ID
        /// </summary>
        [XmlElement("indexid")]
        [JsonProperty("indexid")]
        public int IndexID { get; set; }
        /// <summary>
        /// Index values for each field
        /// </summary>
        [XmlElement("values")]
        [JsonProperty("values")]
        public AXDocIndexValue[] IndexValues { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXDocIndex()
        {

        }
    }


    /// <summary>
    /// Document index field value
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocIndexValue
    {
        /// <summary>
        /// App field ID
        /// </summary>
        public string FieldID { get; set; }
        /// <summary>
        /// Index value
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FieldValue { get; set; }
    }

    /// <summary>
    /// AX Doc Index 
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXKeyRefIndex : LinkedResource
    {
        /// <summary>
        /// Index values for each field
        /// </summary>
        [XmlElement("values")]
        [JsonProperty("values")]
        public AXDocIndexValue[] IndexValues { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXKeyRefIndex()
        {

        }
    }
}
