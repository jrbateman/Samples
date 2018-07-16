using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AXDataType resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDataType : LinkedResource
    {
        /// <summary>
        /// Data type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data type ID
        /// </summary>
        public short ID { get; set; }

        /// <summary>
        /// Is this a custom data type
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDataType()
        {
        }
    }

    /// <summary>
    /// AX Data type collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDataTypeCollection : LinkedResourceCollection<AXDataType>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDataTypeCollection()
            : base()
        {
        }
    }

    /// <summary>
    /// AXDataFormat resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDataFormat : LinkedResource
    {
        /// <summary>
        /// Data format name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data formay ID
        /// </summary>
        public short ID { get; set; }

        /// <summary>
        /// Is this a custom data format
        /// </summary>
        public bool IsCustom { get; set; }
        /// <summary>
        /// Format width
        /// </summary>
        public short FormatWidth { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDataFormat()
        {
        }
    }

    /// <summary>
    /// AX data format collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDataFormatCollection : LinkedResourceCollection<AXDataFormat>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDataFormatCollection()
            : base()
        {
        }
    }
}
