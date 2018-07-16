using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXReport : LinkedResource
    {
        /// <summary>
        /// Doc ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Page Count
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Report Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Report Type
        /// </summary>
        public string ReportType { get; set; }
        /// <summary>
        /// TimeStamp 
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public AXReport()
        {

        }
    }


    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXReportDocPageCollection : PaginatedCollection<AXReportDocPage>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXReportDocPageCollection()
            : base()
        {
        }
    }

    /// <summary>
    /// AX doc page version resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXReportDocPage : LinkedResource
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXReportDocPage()
        {
        }
    }
}
