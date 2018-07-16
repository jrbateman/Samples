using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientCAQConfig : ClientWrapper
    {
        private AXCAQConfig caq;
        private List<AXRESTClientCAQField> coll;

        public AXRESTClientCAQConfig(AXCAQConfig caq, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.caq = caq;
        }

        public int ID
        {
            get
            {
                if (this.caq != null)
                    return this.caq.ID;
                else
                    throw new NullReferenceException("The AX CAQ query is not initialized");
            }
        }

        public string Name
        {
            get
            {
                if (this.caq != null)
                    return this.caq.Name;
                else
                    throw new NullReferenceException("The AX CAQ query is not initialized");
            }
        }

        public bool IsPublic
        {
            get
            {
                if (this.caq != null)
                    return this.caq.IsPublic;
                else
                    throw new NullReferenceException("The AX CAQ query is not initialized");
            }
        }

        public short[] CAQApps
        {
            get
            {
                if (this.caq != null)
                    return this.caq.CAQApps;
                else
                    throw new NullReferenceException("The AX CAQ query is not initialized");
            }
        }

        public List<AXRESTClientCAQField> Fields
        {
            get
            {
                if( this.caq != null && this.caq.Fields != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientCAQField>();
                        foreach (var caqField in this.caq.Fields)
                        {
                            coll.Add(new AXRESTClientCAQField(caqField));
                        }
                    }
                    return coll;
                }
                
                else
                    throw new NullReferenceException("The AX CAQ query is not initialized");
            }
        }

        public async Task<AXRESTClientQuery> GetQueryAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.caq.Links.ContainsKey(AXRESTLinkRelations.AXQuery))
                return null;

            var apiURL = new Uri(this.caq.Links[AXRESTLinkRelations.AXQuery].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXQuery query = AXRESTDataModelConvert.DeserializeObject<AXQuery>(apiResult, mediatype);
                return new AXRESTClientQuery(query, ServerOption);
            }
            finally
            {
            }
        }

        //config caq

        public async Task UpdateAsync(short[] appids, Dictionary<string, QueryIndexAttribute> fields, bool? isPublic = null,
            string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.caq.Links[AXRESTLinkRelations.AXQuery].HRef, UriKind.Relative);
            try
            {
                QueryModel qm = new QueryModel();
                qm.QueryType = AXQueryTypes.CrossAppQuery;
                qm.IsPublic = isPublic.HasValue ? isPublic.Value : this.caq.IsPublic;
                qm.Apps = appids;

                List<QueryIndex> temp = new List<QueryIndex>();
                foreach (var kvp in fields)
                {
                    bool s = (kvp.Value & QueryIndexAttribute.Searchable) == QueryIndexAttribute.Searchable;
                    bool d = (kvp.Value & QueryIndexAttribute.Displayable) == QueryIndexAttribute.Displayable;
                    temp.Add(new QueryIndex() { Name = kvp.Key, Searchable = s, Displayable = d });
                }
                qm.Indexes = temp.ToArray();

                string updatedCAQ = AXRESTDataModelConvert.SerializeObject(
                    qm, mediatype);
                StringContent apiContent = new StringContent(updatedCAQ, Encoding.UTF8, mediatype);

                string apiResult = await PUT(apiURL, apiContent, mediatype);
            }
            finally
            {
            }
        }
    }
}
