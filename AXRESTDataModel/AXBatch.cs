using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AXBatch resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXBatch : LinkedResource
    {
        /// <summary>
        /// Batch ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Batch name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Batch description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Batch state
        /// </summary>
        public BATCH_STATE State { get; set; }
        /// <summary>
        /// Batch creation time
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// Batch page count
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// Whether batch is private
        /// </summary>
        public bool? Private { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXBatch()
        {
        }
    }

    /// <summary>
    /// AXBatch collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXBatchCollection : LinkedResourceCollection<AXBatch>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXBatchCollection()
        {

        }
    }

    public enum BATCH_STATE
    {
        /// <summary>
        /// Batch is idle
        /// </summary>
        Idle,
        /// <summary>
        /// Batch is in scan/import mode
        /// </summary>
        Scan,
        /// <summary>
        /// Batch is in index mode
        /// </summary>
        Index,
    }
}
