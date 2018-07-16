using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXFormOverlays : LinkedResourceCollection<AXFormOverlay>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXFormOverlays()
        {
        }
    }

    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXFormOverlay : LinkedResource
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXFormOverlay()
        {
        }

        public FormTypes FormType { get; set; }

        public float RatioY { get; set; }

        public float RatioX { get; set; }

        public float Top { get; set; }

        public float Left { get; set; }

        public FormUnits Unit { get; set; }

        public FormOrientations Orientation { get; set; }

        public float LPI { get; set; }

        public float CPI { get; set; }

        public string Name { get; set; }

        public uint ID { get; set; }

        public string Content { get; set; }
    }

    /// <summary>
    /// Form types
    /// </summary>
    public enum FormTypes
    {
        /// <summary>
        /// ASCII form
        /// </summary>
        ASCII,
        /// <summary>
        /// Image form
        /// </summary>
        IMAGE,
    }

    /// <summary>
    /// Form orientations
    /// </summary>
    public enum FormOrientations
    {
        /// <summary>
        /// Protrait orientation
        /// </summary>
        PORTRAIT,
        /// <summary>
        /// Landscape orientation
        /// </summary>
        LANDSCAPE
    }

    /// <summary>
    /// Form units
    /// </summary>
    public enum FormUnits
    {
        /// <summary>
        /// Inch
        /// </summary>
        IN,
        /// <summary>
        /// Millimeter
        /// </summary>
        MM,
        /// <summary>
        /// Line
        /// </summary>
        LINE
    }
}
