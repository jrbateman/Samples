using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AXApp atrributes collection
    /// </summary>
    public class AXAppAttributesCollection : KeyBooleanCollection
    {
    }

    /// <summary>
    /// A list of all AXApp attributes
    /// </summary>
    public class AXAppAttributesDefinitions : KeyValueCollection
    {
    }

    public enum AXAppAttributes
    {
        /// <summary>
        /// Multiple Index
        /// </summary>
        MultiIndex,
        /// <summary>
        /// Auto Index
        /// </summary>
        AutoIndex,
        /// <summary>
        /// DLS
        /// </summary>
        DocLevelSecurity,
        /// <summary>
        /// Key Reference
        /// </summary>
        KeyReference,
        /// <summary>
        /// Unique Key
        /// </summary>
        UniqueKey,
        /// <summary>
        /// Doc signing
        /// </summary>
        DocSigning,
        /// <summary>
        /// EDB
        /// </summary>
        EDBEnabled,
        /// <summary>
        /// Prompt checkout
        /// </summary>
        PromptCheckout,
        /// <summary>
        /// Check out comment
        /// </summary>
        CheckoutComment,
        /// <summary>
        /// Check in comment
        /// </summary>
        CheckinComment,
        /// <summary>
        /// HIPPA comment
        /// </summary>
        HIPPAComment,
        /// <summary>
        /// Centera Storage
        /// </summary>
        [Obsolete]
        Centera,
        /// <summary>
        /// RM or SRM Retention
        /// </summary>
        Retention,
        /// <summary>
        /// Supports Retention Hold
        /// </summary>
        RetentionHold,
        /// <summary>
        /// IRM
        /// </summary>
        IRM,
        /// <summary>
        /// Fulltext
        /// </summary>
        Fulltext,
    }
}
