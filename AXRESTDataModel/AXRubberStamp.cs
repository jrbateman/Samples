using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXRubberStamps : LinkedResourceCollection<AXRubberStamp>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXRubberStamps()
        {
        }

    }


    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXRubberStamp : LinkedResource
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXRubberStamp()
        {
        }


        public string Name { get; set; }

        public uint ID { get; set; }

        public RubberStampTypes RubberStampType { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }
    }

    /// <summary>
    /// Rubber stamp type enumberation
    /// </summary>
    public enum RubberStampTypes
    {
        /// <summary>
        /// Text rubber stamp
        /// </summary>
        Text,
        /// <summary>
        /// Image rubber stamp
        /// </summary>
        Image
    }
}
