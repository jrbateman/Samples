using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// AX saved query object
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXQuery : LinkedResource
    {
        /// <summary>
        /// Saved query name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Saved query ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Saved query type
        /// </summary>
        public AXQueryTypes QueryType { get; set; }
        /// <summary>
        /// Public or private query
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXQuery()
        {

        }
    }

    /// <summary>
    /// AX saved query collection
    /// </summary>
    public class AXQueryCollection : LinkedResourceCollection<AXQuery>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXQueryCollection()
        {

        }
    }

    public enum AXQueryTypes
    {
        /// <summary>
        /// Normal single app document query
        /// </summary>
        DocumentQuery,
        /// <summary>
        /// Cross app document query
        /// </summary>
        CrossAppQuery,
        /// <summary>
        /// ERMX report query
        /// </summary>
        ReportQuery,
        /// <summary>
        /// Document retention query
        /// </summary>
        RetentionQuery,
    }


    /// <summary>
    /// Query field collection for an AX query
    /// </summary>
    public class AXQueryFields : LinkedResourceCollection<AXQueryField>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXQueryFields()
        {

        }
    }


    /// <summary>
    /// AX query field
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXQueryField : LinkedResource
    {
        /// <summary>
        /// Query Field Name for display
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Query Field Name for search (used in OData query)
        /// </summary>
        public string SearchName { get; set; }
        /// <summary>
        /// Query field attributes
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public KeyBooleanCollection FieldAttributes { get; set; }
        /// <summary>
        /// Optional field length
        /// </summary>
        public int FieldLength { get; set; }
        /// <summary>
        /// Query criteria value
        /// </summary>
        [XmlElement(IsNullable = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string QueryValue { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXQueryField()
        {

        }
    }

    /// <summary>
    /// Query field collection for an AX query
    /// </summary>
    public class AXODMAQueryFields : LinkedResourceCollection<AXQueryField>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AXODMAQueryFields()
        {

        }
    }

    /// <summary>
    /// AX Fulltext query
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXFulltextQuery : LinkedResource
    {
        /// <summary>
        /// Fulltext query operator (relationship between DB query and full text query)
        /// </summary>
        public AXFulltextQueryOperator FTQueryOperator { get; set; }
        /// <summary>
        /// Fulltext query expression
        /// </summary>
        public AXFulltextQueryExpression FTQueryExpression { get; set; }
        /// <summary>
        /// Fulltext query criteria value
        /// </summary>
        //[XmlElement(IsNullable = false)]
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FTQueryValue { get; set; }
        /// <summary>
        /// Fulltext query options
        /// </summary>
        //[XmlElement(IsNullable = false)]
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public KeyBooleanCollection FTQueryOptions { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AXFulltextQuery()
        {

        }
    }

    /// <summary>
    /// Full text query operator
    /// </summary>
    public enum AXFulltextQueryOperator
    {
        /// <summary>
        /// AND operator
        /// </summary>
        And = 0,
        /// <summary>
        /// OR operator
        /// </summary>
        Or = 1,
    }
    /// <summary>
    /// Full text search expressions
    /// </summary>
    public enum AXFulltextQueryExpression
    {
        /// <summary>
        /// All words
        /// </summary>
        And = 0,
        /// <summary>
        /// Any words
        /// </summary>
        OR = 1,
        /// <summary>
        /// Exact phrase
        /// </summary>
        Exact = 2,
        /// <summary>
        /// Expression
        /// </summary>
        Expression = 3,
    }

    public class AXCAQConfig : LinkedResource
    {
        public AXCAQConfig() { }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public short[] CAQApps { get; set; }
        public AXCAQField[] Fields { get; set; }
    }

    public class AXCAQField
    {
        public AXCAQField() { }

        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Displayable { get; set; }
    }

    public class QueryModel
    {
        public QueryModel()
        {

        }

        public QueryIndex[] Indexes { get; set; }

        public FullTextSearchOptions fullText { get; set; }

        public RetentionQueryOptions RetentionOptions { get; set; }

        public string Name { get; set; }

        public int ID { get; set; }

        public bool IsPublic { get; set; }

        public bool IsIncludingPreviousRevisions { get; set; }

        public string Owner { get; set; }

        public AXQueryTypes QueryType { get; set; }

        public short[] Apps { get; set; }
    }

    public class QueryIndex
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsODMAField { get; set; }
        public bool Searchable { get; set; }
        public bool Displayable { get; set; }
    }

    public enum QueryIndexAttribute
    {
        Searchable = 0x01,
        Displayable = 0x02
    }

    public class FullTextSearchOptions
    {
        public AXFulltextQueryOperator QueryOperator { get; set; }

        public AXFulltextQueryExpression SearchType { get; set; }

        public bool Thesaurus { get; set; }
        public string Value { get; set; }
    }

    public class RetentionQueryOptions
    {
        public AXRetentionQueryType Type { get; set; }
    }

    /// <summary>
    /// Retention Query Types
    /// </summary>
    /// <remarks>This enumeration provides retention flags for search </remarks>
    public enum AXRetentionQueryType
    {
        /// <summary>
        /// Include docs under retention in search
        /// </summary>
        AllIncludingRetention = 0,
        /// <summary>
        /// Exclude docs under retention in search
        /// </summary>
        AllExcludingRetention = 1,
        /// <summary>
        /// Only search docs under retention 
        /// </summary>
        OnlyUnderRetention = 2,
        /// <summary>
        /// Only search docs on retention hold
        /// </summary>
        OnlyOnRetentionHold = 3,
        /// <summary>
        /// Only search docs under retention exclude docs on hold
        /// </summary>
        OnlyUnderRetentionNotOnHold = 4,
    }
}
