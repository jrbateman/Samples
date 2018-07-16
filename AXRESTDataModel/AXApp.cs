using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AXApp resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXApp : LinkedResource
    {
        /// <summary>
        /// App name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// App description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// App ID
        /// </summary>
        public short ID { get; set; }
        /// <summary>
        /// App attributes
        /// </summary>
        [XmlElement]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public AXAppAttributesCollection Attributes { get; set; }
        /// <summary>
        /// App specific user permissions
        /// </summary>
        [XmlElement]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public AXPermissionCollection Permissions { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXApp()
        {

        }
    }

    /// <summary>
    /// AXApp collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXApps : LinkedResourceCollection<AXApp>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXApps()
        {

        }
    }

    /// <summary>
    /// AXAppField resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXAppField : LinkedResource
    {
        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Field ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Field edit mask (optional)
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EditMask { get; set; }

        /// <summary>
        /// Validation regex for index field
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ValidationRegex { get; set; }
        /// <summary>
        /// Validation regex for query field
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string QueryValidationRegex { get; set; }

        /// <summary>
        /// Field length (optional)
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FieldLength { get; set; }

        /// <summary>
        /// Field attributes
        /// </summary>
        public AXFieldAttributesCollection Attributes { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXAppField()
        {

        }
    }

    /// <summary>
    /// AXAppField collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXAppFields : LinkedResourceCollection<AXAppField>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXAppFields()
        {

        }
    }
}
