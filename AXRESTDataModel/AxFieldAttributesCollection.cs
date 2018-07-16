using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTDataModel
{
    public class AXFieldAttributesCollection : KeyBooleanCollection
    {

    }

    public enum AXAppFieldAttributes
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Searchable
        /// </summary>
        Searchable = 0x0001,
        /// <summary>
        /// Doc Level Security
        /// </summary>
        DLSEnabled = 0x0002,
        /// <summary>
        /// Auto Index
        /// </summary>
        AutoIndex = 0x0004,
        /// <summary>
        /// Part of Unique Key
        /// </summary>
        UniqueKey = 0x0008,
        /// <summary>
        /// Required
        /// </summary>
        Required = 0x0010,
        /// <summary>
        /// Read only
        /// </summary>
        Readonly = 0x0020,
        /// <summary>
        /// Reference Key
        /// </summary>
        ReferenceKey = 0x0040,
        /// <summary>
        /// Reference Data
        /// </summary>
        ReferenceData = 0x0080,
        /// <summary>
        /// Eithe a ref key or ref data
        /// </summary>
        ReferenceField = ReferenceKey | ReferenceData,
        /// <summary>
        /// Dual Data Entry
        /// </summary>
        DualDataEntry = 0x0100,
        /// <summary>
        /// Value Mask
        /// </summary>
        ValueMask = 0x0200,
        /// <summary>
        /// Reserved
        /// </summary>
        LeadingZero = 0x0400,
        /// <summary>
        /// Reserved
        /// </summary>
        IndexedBy = 0x0800,
        /// <summary>
        /// Time Stamp
        /// </summary>
        TimeStamp = 0x1000,
        /// <summary>
        /// Hidden field
        /// </summary>
        Hidden = 0x2000,
    }
}
