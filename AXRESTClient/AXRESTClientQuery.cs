using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientQuery : ClientWrapper
    {
        private AXQuery query;

        public AXRESTClientQuery(AXQuery q, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.query = q;
        }

        public int ID
        {
            get
            {
                if (this.query != null)
                    return this.query.ID;
                else
                    throw new NullReferenceException("The AX query is not initialized");
            }
        }

        public string Name
        {
            get
            {
                if (this.query != null)
                    return this.query.Name;
                else
                    throw new NullReferenceException("The AX query is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", ID, Name);
        }

        public bool IsPublic
        {
            get
            {
                if (this.query != null)
                    return this.query.IsPublic;
                else
                    throw new NullReferenceException("The AX query is not initialized");
            }
        }

        public string QueryType
        {
            get
            {
                if (this.query != null)
                    return this.query.QueryType.ToString();
                else
                    throw new NullReferenceException("The AX query is not initialized");
            }
        }

        public async Task<AXRESTClientQuery> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.query.Self))
                return null;

            var apiURL = new Uri(this.query.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.query = AXRESTDataModelConvert.DeserializeObject<AXQuery>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientUser> GetCreatorAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.query.Links.ContainsKey(AXRESTLinkRelations.Creator))
                return null;

            var apiURL = new Uri(this.query.Links[AXRESTLinkRelations.Creator].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXUser creator = AXRESTDataModelConvert.DeserializeObject<AXUser>(apiResult, mediatype);
                return new AXRESTClientUser(creator);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueryFields> GetQueryFieldsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.query.Links.ContainsKey(AXRESTLinkRelations.AXQueryDef))
                return null;

            var apiURL = new Uri(this.query.Links[AXRESTLinkRelations.AXQueryDef].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXQueryFields queryFields = AXRESTDataModelConvert.DeserializeObject<AXQueryFields>(apiResult, mediatype);
                return new AXRESTClientQueryFields(queryFields);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueryFields> GetODMAQueryFieldsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.query.Links.ContainsKey(AXRESTLinkRelations.AXODMAQueryDef))
                return null;

            var apiURL = new Uri(this.query.Links[AXRESTLinkRelations.AXODMAQueryDef].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXODMAQueryFields odmaQueryFields = AXRESTDataModelConvert.DeserializeObject<AXODMAQueryFields>(apiResult, mediatype);
                return new AXRESTClientQueryFields(odmaQueryFields);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientFullTextQuery> GetFullTextQueryDefinitionAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.query.Links.ContainsKey(AXRESTLinkRelations.AXFullTextQueryDef))
                return null;

            var apiURL = new Uri(this.query.Links[AXRESTLinkRelations.AXFullTextQueryDef].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXFulltextQuery fulltextQuery = AXRESTDataModelConvert.DeserializeObject<AXFulltextQuery>(apiResult, mediatype);
                return new AXRESTClientFullTextQuery(fulltextQuery);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientCAQConfig> GetCAQConfigAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.query.Links.ContainsKey(AXRESTLinkRelations.AXCAQQueryDef))
                return null;

            var apiURL = new Uri(this.query.Links[AXRESTLinkRelations.AXCAQQueryDef].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXCAQConfig caq = AXRESTDataModelConvert.DeserializeObject<AXCAQConfig>(apiResult, mediatype);
                return new AXRESTClientCAQConfig(caq, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueryResults> ExecuteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.query.Links.ContainsKey(AXRESTLinkRelations.AXSavedQueryResults))
                return null;

            var apiURL = new Uri(this.query.Links[AXRESTLinkRelations.AXSavedQueryResults].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXQueryResultCollection queryResults = AXRESTDataModelConvert.DeserializeObject<AXQueryResultCollection>(apiResult, mediatype);
                return new AXRESTClientQueryResults(queryResults, ServerOption);
            }
            finally
            {
            }
        }

        //modify query
        public async Task UpdateAsync(Dictionary<string, string> indexes, FullTextSearchOptions ftOptions = null,
            bool isPublic = true, bool isIncludingPreviousRevisions = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.query.Self, UriKind.Relative);
            try
            {
                QueryModel qm = new QueryModel();
                qm.Name = this.query.Name;
                qm.ID = this.query.ID;
                qm.IsPublic = isPublic;
                qm.IsIncludingPreviousRevisions = isIncludingPreviousRevisions;
                qm.fullText = ftOptions;

                List<QueryIndex> temp = new List<QueryIndex>();
                foreach (var kvp in indexes)
                {
                    temp.Add(new QueryIndex() { Name = kvp.Key, Value = kvp.Value });
                }
                qm.Indexes = temp.ToArray();

                string updatedQuery = AXRESTDataModelConvert.SerializeObject(
                    qm, mediatype);
                StringContent apiContent = new StringContent(updatedQuery, Encoding.UTF8, mediatype);

                string apiResult = await PUT(apiURL, apiContent, mediatype);
            }
            finally
            {
            }
        }

        //delete query
        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.query.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.query = null;
                this.Dispose();
            }
            finally
            {
            }
        }
    }
}
