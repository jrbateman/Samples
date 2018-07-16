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
    /// AXDoc resource
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDoc : LinkedResource
    {
        /// <summary>
        /// Whether the document is a working copy
        /// </summary>
        public bool WorkingCopy { get; set; }

        /// <summary>
        /// Doc ID
        /// </summary>
        public uint ID { get; set; }
        /// <summary>
        /// Page Count
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// Doc revision attributes
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AXDocAttributesCollection Attributes { get; set; }

        #region Update parameters that only used during AXDoc update
        /// <summary>
        /// Submit fulltext job. Can not be used during checkout
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool SubmitFulltext { get; set; }
        /// <summary>
        /// Obsolete. Server will use its own logic to determine which queue it will use basing on the Application and User.</param>
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FulltextQueue { get; set; }
        /// <summary>
        /// Submit OCR job. Can not be used during checkout
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool SubmitOCR { get; set; }
        /// <summary>
        /// Obsolete. Server will use its own logic to determine which queue it will use basing on the Application and User.</param>
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OCRQueue { get; set; }
        #endregion
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXDoc()
        {

        }
    }

    /// <summary>
    /// AX Doc Attributes collection
    /// </summary>
    public class AXDocAttributesCollection
    {
        /// <summary>
        /// Doc is checked out by current user
        /// </summary>
        [XmlAttribute()]
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Checkedout { get; set; }
        /// <summary>
        /// Doc is a previous revision
        /// </summary>
        [XmlAttribute()]
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool PreviousRevision { get; set; }
        /// <summary>
        /// Doc has revisions
        /// </summary>
        [XmlAttribute()]
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasVersions { get; set; }
        /// <summary>
        /// Doc is the final revision
        /// </summary>
        [XmlAttribute()]
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FinalRevision { get; set; }
        /// <summary>
        /// Doc is a COLD document
        /// </summary>
        [XmlAttribute()]
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsCOLD { get; set; }

        public void Init(DocRevisionAttributes attr, bool bIsCOLD)
        {
            Checkedout = (attr & DocRevisionAttributes.Checkedout) != 0;
            PreviousRevision = (attr & DocRevisionAttributes.PreviousRevision) != 0;
            HasVersions = (attr & DocRevisionAttributes.HasVersions) != 0;
            FinalRevision = (attr & DocRevisionAttributes.Final) != 0;
            IsCOLD = bIsCOLD;
        }

        public bool GetAttribute(DocRevisionAttributes attr)
        {
            switch (attr)
            {
                case DocRevisionAttributes.Checkedout:
                    return Checkedout;
                case DocRevisionAttributes.PreviousRevision:
                    return PreviousRevision;
                case DocRevisionAttributes.HasVersions:
                    return HasVersions;
                case DocRevisionAttributes.Final:
                    return FinalRevision;
                default:
                    return false;
            }
        }

        public void SetAttribute(DocRevisionAttributes attr, bool bEnable)
        {
            switch (attr)
            {
                case DocRevisionAttributes.Checkedout:
                    Checkedout = bEnable;
                    break;
                case DocRevisionAttributes.PreviousRevision:
                    PreviousRevision = bEnable;
                    break;
                case DocRevisionAttributes.HasVersions:
                    HasVersions = bEnable;
                    break;
                case DocRevisionAttributes.Final:
                    FinalRevision = bEnable;
                    break;
                default:
                    break;
            }
        }
    }

    public enum DocRevisionAttributes
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Doc is checked out by me
        /// </summary>
        Checkedout = 1,
        /// <summary>
        /// Doc is a previous revision of another doc and can't be checked out
        /// </summary>
        PreviousRevision = 2,
        /// <summary>
        /// Doc has revisions
        /// </summary>
        HasVersions = 4,
        /// <summary>
        /// Doc is a a working copy of a checked out doc
        /// </summary>
        WorkingCopy = 8,
        /// <summary>
        /// Final immutable revision of the doc
        /// </summary>
        Final = 16,
        /// <summary>
        /// Doc is marked as COLD document
        /// </summary>
        COLDDoc = 128,
    }

    public class NewDocumentRequest
    {
        public AXDoc TargetDoc { get; set; }
        public AXDocIndex NewIndex { get; set; }
        public AXBatch FromBatch { get; set; }
        public int BatchPageNum { get; set; }
        public bool IgnoreDuplicateIndex { get; set; }
        public bool IgnoreDlsViolation { get; set; }
    }
}
