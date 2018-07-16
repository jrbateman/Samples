using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AX Batch page collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXBatchPagesCollection : PaginatedCollection<AXBatchPage>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXBatchPagesCollection()
            : base()
        {
        }
    }

    /// <summary>
    /// AX Batch page resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXBatchPage : LinkedResource
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public AXBatchPage()
        { }
    }
}
