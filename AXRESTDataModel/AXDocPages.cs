using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AX Doc page collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocPagesCollection : PaginatedCollection<AXDocPage>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDocPagesCollection()
            : base()
        {
        }
    }

    /// <summary>
    /// AX Doc page resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocPage : LinkedResource
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public AXDocPage()
        { }

    }

    /// <summary>
    /// AX Doc page version collection
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocPageVersionCollection : PaginatedCollection<AXDocPageVersion>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDocPageVersionCollection()
            : base()
        {
        }
    }

    /// <summary>
    /// AX doc page version resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDocPageVersion : LinkedResource
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Version number
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Has annotation file
        /// </summary>
        public bool HasAnnotation { get; set; }

        /// <summary>
        /// Has text view file
        /// </summary>
        public bool HasTextView { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDocPageVersion()
        {
        }

    }
}
