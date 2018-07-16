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
    /// AX query resultset items collection with pagenation.
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXQueryResultCollection : PaginatedCollection<AXQueryResultItem>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXQueryResultCollection()
            : base()
        {
        }

        [XmlElement("column")]
        [JsonProperty("columns")]
        public List<string> Columns { get; set; }
    }

    /// <summary>
    /// Represents a query resultset item
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXQueryResultItem : LinkedResource
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXQueryResultItem()
        { }

        /// <summary>
        /// Resultset item column values
        /// </summary>
        [XmlElement("indexvalue")]
        [JsonProperty("indexvalues")]
        public List<string> IndexValues { get; set; }

        /// <summary>
        /// Primary ID of the resultset item (could be doc or report ID)
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Page count of the document or report
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Attributes
        /// </summary>
        public KeyBooleanCollection Attributes { get; set; }

        [XmlElement(IsNullable = true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        /// <summary>
        /// If full text search if performed, this contains number of full text hits on this item.
        /// Otherwise, it will be null.
        /// </summary>
        public uint? FulltextHits { get; set; }
    }
}
