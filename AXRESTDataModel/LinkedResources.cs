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
    /// Link class for supporting linked resources
    /// </summary>
    [XmlRoot("link", Namespace = LinkedResource.DeaufaultNameSpace)]
    public class Link
    {
        /// <summary>
        /// Relationship
        /// </summary>
        [XmlAttribute("rel")]
        [JsonProperty("rel")]
        public string Rel { get; set; }
        /// <summary>
        /// Hyperlink
        /// </summary>
        [XmlAttribute("href")]
        [JsonProperty("href")]
        public string HRef { get; set; }

        /// <summary>
        /// Link title
        /// </summary>
        [XmlAttribute("title")]
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public string Title { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Link()
        {
        }

        /// <summary>
        /// Construct a link with provided name and url
        /// </summary>
        /// <param name="relationship">Relationship name</param>
        /// <param name="url">Hyperlink</param>
        public Link(string relationship, string url)
        {
            Rel = relationship;
            HRef = url;
        }
    }

    /// <summary>
    /// Base interface for base link URL
    /// </summary>
    public interface IBaseLink
    {
        /// <summary>
        /// Base URL for all the links
        /// </summary>
        string BaseURL { get; set; }
    }

    /// <summary>
    /// Base class for all resource model classes. It should contain at least a link to self.
    /// This base class is only serialized via special DctmXmlMediaTypeFormatter or DctmJsonMediaTypeFormatter.
    /// All derived classes should use DataContract/DataMember for normal serialization processing. 
    /// When normal XML and JSON formatter is used, this base class will then be ignored.
    /// </summary>
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public abstract class LinkedResource : IBaseLink
    {
        /// <summary>
        /// Default namespace
        /// </summary>
        public const string DeaufaultNameSpace = "http://www.emc.com/ax";
        /// <summary>
        /// Link relation name for self
        /// </summary>
        public const string SelfRelName = "self";

        private Dictionary<string, Link> _links = new Dictionary<string, Link>();

        /// <summary>
        ///  Hyper links to other resources (exclude self). Not serialized
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Dictionary<string, Link> Links
        {
            get
            {
                lock (this)
                {
                    if (_links == null)
                        _links = new Dictionary<string, Link>();    // this is needed when the object is deserialized
                }
                return _links;
            }
        }

        /// <summary>
        /// Add a link
        /// </summary>
        /// <param name="lnk"></param>
        public void AddLink(Link lnk)
        {
            Links[lnk.Rel] = lnk;
        }

        /// <summary>
        /// Add a link
        /// </summary>
        /// <param name="sRel"></param>
        /// <param name="sHRef"></param>
        public void AddLink(string sRel, string sHRef)
        {
            AddLink(new Link(sRel, sHRef));
        }

        /// <summary>
        /// Link URL to self. 
        /// </summary>
        [XmlAttribute("href")]
        [JsonIgnore]
        public string Self { get; set; }

        /// <summary>
        /// Optional Base URL for all the links
        /// </summary>
        [JsonProperty(PropertyName = "@base", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("baseurl")]
        public string BaseURL { get; set; }

        /// <summary>
        /// This property is for use with XML serialization only. All links excluding Self is included
        /// </summary>
        [XmlElement(ElementName = "link")]
        [JsonIgnore]
        public Link[] XmlLinks
        {
            get
            {
                List<Link> lnks = new List<Link>();
                lock (this)
                {
                    if (_links != null)
                    {
                        foreach (Link link in _links.Values)
                            lnks.Add(link);
                    }
                }
                return lnks.ToArray();
            }
            set
            {
                lock (this)
                {
                    if (_links != null)
                        _links.Clear();
                    if (value == null || value.Length == 0)
                        return;
                    foreach (Link lnk in value)
                        AddLink(lnk);
                }
            }
        }

        /// <summary>
        /// This property is for use with JSON serialization only. All links including Self is included
        /// </summary>
        [JsonProperty(PropertyName = "links")]
        [XmlIgnore]
        public Link[] JsonLinks
        {
            get
            {
                List<Link> lnks = new List<Link>();
                lock (this)
                {
                    if (Self != null)
                    {
                        lnks.Add(new Link(SelfRelName, Self));
                    }
                    if (_links != null)
                    {
                        foreach (Link link in _links.Values)
                            lnks.Add(link);
                    }
                }
                return lnks.ToArray();
            }
            set
            {
                lock (this)
                {
                    Self = null;
                    if (_links != null)
                        _links.Clear();
                    if (value == null || value.Length == 0)
                        return;
                    foreach (Link lnk in value)
                    {
                        if (string.Compare(lnk.Rel, SelfRelName, true) == 0)
                            Self = lnk.HRef;
                        else
                            AddLink(lnk);
                    }
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LinkedResource()
        {
        }
    }

    /// <summary>
    /// Base class for simple resource collection classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LinkedResourceCollection<T> : IBaseLink where T : LinkedResource
    {
        /// <summary>
        /// Base URL for all the links
        /// </summary>
        [JsonProperty(PropertyName = "@base", NullValueHandling = NullValueHandling.Ignore)]
        [XmlAttribute("baseurl")]
        public string BaseURL { get; set; }

        /// <summary>
        /// Collection entries
        /// </summary>
        [JsonProperty("entries")]
        [XmlElement("entries")]
        public List<T> Entries { get; set; }

        /// <summary>
        /// Collection item count. Only set during initization if needed
        /// </summary>
        [XmlElement("count", IsNullable = true)]
        [JsonProperty("count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Count { get; set; }

    }

    /// <summary>
    /// Paginated collection supporting AtomPub and EDAAJSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedCollection<T> : LinkedResource where T : LinkedResource
    {
        /// <summary>
        /// FeedId
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Feed updated timestamp
        /// </summary>
        [JsonProperty("updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Collection entries
        /// </summary>
        [JsonProperty("entries")]
        public List<T> Entries { get; set; }

        /// <summary>
        /// Collection item count. Only set during initization if needed
        /// </summary>
        [XmlElement(IsNullable = true)]
        [JsonProperty("count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Count { get; set; }

        /// <summary>
        /// Link to first page
        /// </summary>
        [JsonIgnore]
        public Link First
        {
            get
            {
                lock (this)
                {
                    if (Links.ContainsKey("first"))
                        return Links["first"];
                    else
                        return null;
                }
            }
            set
            {
                lock (this)
                {
                    if (value != null)
                    {
                        value.Rel = "first";
                        Links["first"] = value;
                    }
                    else
                        Links.Remove("first");
                }
            }
        }

        /// <summary>
        /// Link to next page
        /// </summary>
        [JsonIgnore]
        public Link Next
        {
            get
            {
                lock (this)
                {
                    if (Links.ContainsKey("next"))
                        return Links["next"];
                    else
                        return null;
                }
            }
            set
            {
                lock (this)
                {
                    if (value != null)
                    {
                        value.Rel = "next";
                        Links["next"] = value;
                    }
                    else
                        Links.Remove("next");
                }
            }
        }

        /// <summary>
        /// Link to previous page
        /// </summary>
        [JsonIgnore]
        public Link Previous
        {
            get
            {
                lock (this)
                {
                    if (Links.ContainsKey("prev"))
                        return Links["prev"];
                    else
                        return null;
                }
            }
            set
            {
                lock (this)
                {
                    if (value != null)
                    {
                        value.Rel = "prev";
                        Links["prev"] = value;
                    }
                    else
                        Links.Remove("prev");
                }
            }
        }

        /// <summary>
        /// Link to last page
        /// </summary>
        [JsonIgnore]
        public Link Last
        {
            get
            {
                lock (this)
                {
                    if (Links.ContainsKey("last"))
                        return Links["last"];
                    else
                        return null;
                }
            }
            set
            {
                lock (this)
                {
                    if (value != null)
                    {
                        value.Rel = "last";
                        Links["last"] = value;
                    }
                    else
                        Links.Remove("last");
                }
            }
        }
        // use OData syntax for offet ($skip) and count ($top)
        const string LinkFormatter = "$skip={0}&$top={1}";    // format a pagenated link

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public PaginatedCollection()
            : base()
        {
            Id = Guid.NewGuid().ToString();
            LastUpdated = DateTime.UtcNow;
            Entries = new List<T>();
        }

        #endregion
    }
}
