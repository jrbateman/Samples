using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// Document revision history
    /// </summary>
    public class AXDocRevisionHistory : LinkedResourceCollection<AXDocRevision>
    {
        public string CurrentDocRevision { get; set; }

        public Link DocLink { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDocRevisionHistory()
        {

        }
    }

    /// <summary>
    /// Document revision
    /// </summary>
    public class AXDocRevision : LinkedResource
    {
        public string RevisionNumber { get; set; }
        public string CheckinBy { get; set; }
        public DateTime CheckinDate { get; set; }
        public string CheckinComment { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDocRevision()
        { }

    }
}
