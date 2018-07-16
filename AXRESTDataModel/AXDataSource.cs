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
    /// AXDataSource resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDataSource : LinkedResource
    {
        /// <summary>
        /// datasource name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// datasource description
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// default datasource
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDefault { get; set; }

        /// <summary>
        /// datesource GUID string for unique identifier
        /// </summary>
        [XmlElement(IsNullable = true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? ID { get; set; }

        /// <summary>
        /// datasource DB version
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DBVersion { get; set; }

        /// <summary>
        /// datasource DB version
        /// </summary>
        public DataSourceAuthenticationType SecurityModel { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDataSource()
        {
        }
    }

    public enum DataSourceAuthenticationType
    {
        /// <summary>
        /// Unknown type or none
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// CM security login
        /// </summary>
        CM,
        /// <summary>
        /// Windows security
        /// </summary>
        Windows,
        /// <summary>
        /// LDAP security
        /// </summary>
        Directory,
        /// <summary>
        /// EAI security
        /// </summary>
        EAI,
        /// <summary>
        /// Custom security
        /// </summary>
        Custom,
    }
}
