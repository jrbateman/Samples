using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDataSource : ClientWrapper
    {
        private AXRESTDataModel.AXDataSource dataSource;

        public Guid? ID
        {
            get
            {
                if (this.dataSource != null)
                    return this.dataSource.ID;
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }
        public string Name
        {
            get
            {
                if (this.dataSource != null)
                    return this.dataSource.Name;
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }
        public string Description
        {
            get
            {
                if (this.dataSource != null)
                    return this.dataSource.Description;
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }
        public string DBVersion
        {
            get
            {
                if (this.dataSource != null)
                    return this.dataSource.DBVersion;
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }
        public bool IsDefault
        {
            get
            {
                if (this.dataSource != null)
                    return this.dataSource.IsDefault;
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }
        public string SecurityModel
        {
            get
            {
                if (this.dataSource != null)
                    return this.dataSource.SecurityModel.ToString();
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }

        public AXRESTClientDataSource(AXRESTDataModel.AXDataSource DataSource, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.dataSource = DataSource;
        }

        public async Task<AXRESTClientUser> GetCurrentUserAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.CurrentUser].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXUser user = AXRESTDataModelConvert.DeserializeObject<AXUser>(apiResult, mediatype);
                return new AXRESTClientUser(user);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueryFields> GetODMAQueryFields(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXODMAQueryDef].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXODMAQueryFields odmaQueryFields = AXRESTDataModelConvert.DeserializeObject<AXODMAQueryFields>(apiResult, mediatype);
                return new AXRESTClientQueryFields(odmaQueryFields); ;
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientFormOverlays> GetFormOverlay(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXFormOverlay].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXFormOverlays fos = AXRESTDataModelConvert.DeserializeObject<AXFormOverlays>(apiResult, mediatype);
                return new AXRESTClientFormOverlays(fos); ;
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientApplicationList> GetApplicationsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXApplications].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXApps appList = AXRESTDataModelConvert.DeserializeObject<AXApps>(apiResult, mediatype);
                return new AXRESTClientApplicationList(appList, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDataTypes> GetDataTypesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXDataTypes].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDataTypeCollection datatypes = AXRESTDataModelConvert.DeserializeObject<AXDataTypeCollection>(apiResult, mediatype);
                return new AXRESTClientDataTypes(datatypes, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueries> GetQueriesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXQueries].HRef, UriKind.Relative); 
            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXQueryCollection queries = AXRESTDataModelConvert.DeserializeObject<AXQueryCollection>(apiResult, mediatype);
                return new AXRESTClientQueries(queries, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQuery> CreateQueryAsync(short appid, string queryName, Dictionary<string, string> indexes, FullTextSearchOptions ftOptions = null, bool isPublic = true, bool isIncludingPreviousRevisions = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXQueries].HRef, UriKind.Relative);
            try
            {
                QueryModel qm = new QueryModel();
                qm.Name = queryName;
                qm.QueryType = AXQueryTypes.DocumentQuery;
                qm.IsPublic = isPublic;
                qm.IsIncludingPreviousRevisions = isIncludingPreviousRevisions;
                qm.fullText = ftOptions;

                List<QueryIndex> temp = new List<QueryIndex>();
                foreach (var kvp in indexes)
                {
                    temp.Add(new QueryIndex() { Name = kvp.Key, Value = kvp.Value });
                }
                qm.Indexes = temp.ToArray();

                string newquery = AXRESTDataModelConvert.SerializeObject(
                    qm, mediatype);
                StringContent data = new StringContent(newquery, Encoding.UTF8, mediatype);
                
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("appid", appid.ToString());
                string apiResult = await POST(apiURL, data, mediatype, paras);
                AXQuery query = AXRESTDataModelConvert.DeserializeObject<AXQuery>(apiResult, mediatype);
                return new AXRESTClientQuery(query, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQuery> CreateCAQAsync(string queryName, short[] appids, Dictionary<string, QueryIndexAttribute> fields, bool isPublic = true, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.dataSource.Links[AXRESTLinkRelations.AXQueries].HRef, UriKind.Relative);
            try
            {
                QueryModel qm = new QueryModel();
                qm.Name = queryName;
                qm.QueryType = AXQueryTypes.CrossAppQuery;
                qm.IsPublic = isPublic;
                qm.Apps = appids;

                List<QueryIndex> temp = new List<QueryIndex>();
                foreach (var kvp in fields)
                {
                    bool s = (kvp.Value & QueryIndexAttribute.Searchable) == QueryIndexAttribute.Searchable;
                    bool d = (kvp.Value & QueryIndexAttribute.Displayable) == QueryIndexAttribute.Displayable;
                    temp.Add(new QueryIndex() { Name = kvp.Key, Searchable = s, Displayable = d });
                }
                qm.Indexes = temp.ToArray();

                string newquery = AXRESTDataModelConvert.SerializeObject(
                    qm, mediatype);
                StringContent data = new StringContent(newquery, Encoding.UTF8, mediatype);

                string apiResult = await POST(apiURL, data, mediatype);
                AXQuery query = AXRESTDataModelConvert.DeserializeObject<AXQuery>(apiResult, mediatype);
                return new AXRESTClientQuery(query, ServerOption);
            }
            finally
            {
            }
        }
    }
}
