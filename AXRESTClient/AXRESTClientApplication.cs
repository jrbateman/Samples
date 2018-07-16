using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientApplication : ClientWrapper
    {
        private AXRESTDataModel.AXApp application;

        public int ID
        {
            get
            {
                if (this.application != null)
                    return this.application.ID;
                else
                    throw new NullReferenceException("The AX application is not initialized");
            }
        }

        public string Name
        {
            get
            {
                if (this.application != null)
                    return this.application.Name;
                else
                    throw new NullReferenceException("The AX application is not initialized");
            }
        }

        public string Description
        {
            get
            {
                if (this.application != null)
                    return this.application.Description;
                else
                    throw new NullReferenceException("The AX application is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", ID, Name);
        }

        public List<string> Attributes
        {
            get
            {
                if (this.application != null)
                {
                    List<string> attributes = new List<string>();
                    foreach (var attr in this.application.Attributes)
                    {
                        if (attr.Value) attributes.Add(attr.Key.ToString());
                    }
                    return attributes;
                }
                else
                    throw new NullReferenceException("The AX application is not initialized");
            }
        }

        public List<string> Permissions
        {
            get
            {
                if (this.application != null)
                {
                    List<string> perms = new List<string>();
                    foreach (var perm in this.application.Permissions)
                    {
                        if (perm.Value) perms.Add(perm.Key.ToString());
                    }
                    return perms;
                }
                else
                    throw new NullReferenceException("The AX application is not initialized");
            }
        }

        public AXRESTClientApplication(AXRESTDataModel.AXApp Application, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.application = Application;
        }

        public async Task<AXRESTClientRubberStamps> GetRubberStamps(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXRubberStamp].HRef, UriKind.Relative);
            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXRubberStamps rubberstamps = AXRESTDataModelConvert.DeserializeObject<AXRubberStamps>(apiResult, mediatype);
                return new AXRESTClientRubberStamps(rubberstamps);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueries> GetQueriesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXQueries].HRef, UriKind.Relative);
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

        public async Task<AXRESTClientBatches> GetBatchesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXBatches].HRef, UriKind.Relative);
            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXBatchCollection batches = AXRESTDataModelConvert.DeserializeObject<AXBatchCollection>(apiResult, mediatype);
                return new AXRESTClientBatches(batches, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientBatch> CreateBatchDocumentAsync(string batchName, string batchDescription, List<AXRESTClientFile> clientFiles, bool privat = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXBatches].HRef, UriKind.Relative);
            try
            {
                MultipartFormDataContent apiContent = new MultipartFormDataContent();
                string newbatch = AXRESTDataModelConvert.SerializeObject(
                    new AXBatch() { Name = batchName, Description = batchDescription, Private = privat }, mediatype);
                StringContent data = new StringContent(newbatch, Encoding.UTF8, mediatype);
                apiContent.Add(data, "data");

                foreach (var clientfile in clientFiles)
                {
                    var streamContent = new StreamContent(clientfile.Stream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                    apiContent.Add(streamContent, clientfile.TypeName, clientfile.FileName);
                }

                string apiResult = await POST(apiURL, apiContent, mediatype);
                AXBatch batch = AXRESTDataModelConvert.DeserializeObject<AXBatch>(apiResult, mediatype);
                return new AXRESTClientBatch(batch, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientBatch> CreateDocumentAsync(Dictionary<string, string> indexvalues, List<AXRESTClientFile> clientFiles,
            bool ignoreDuplicateIndex = true, bool ignoreDlsViolation = true, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXDoc].HRef, UriKind.Relative);
            try
            {
                AXDocIndex newIndex = new AXDocIndex();
                List<AXDocIndexValue> temp = new List<AXDocIndexValue>();
                foreach (var kvp in indexvalues)
                {
                    AXDocIndexValue value = new AXDocIndexValue();
                    value.FieldID = kvp.Key;
                    value.FieldValue = kvp.Value;
                    temp.Add(value);
                }
                newIndex.IndexValues = temp.ToArray();

                MultipartFormDataContent apiContent = new MultipartFormDataContent();
                string newDoc = AXRESTDataModelConvert.SerializeObject(
                    new NewDocumentRequest()
                    {
                        NewIndex = newIndex,
                        IgnoreDlsViolation = ignoreDlsViolation,
                        IgnoreDuplicateIndex = ignoreDuplicateIndex
                    }, mediatype);
                StringContent data = new StringContent(newDoc, Encoding.UTF8, mediatype);
                apiContent.Add(data, "data");

                foreach (var clientFile in clientFiles)
                {
                    var streamContent = new StreamContent(clientFile.Stream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                    apiContent.Add(streamContent, clientFile.TypeName, clientFile.FileName);
                }

                string apiResult = await POST(apiURL, apiContent, mediatype);
                AXBatch batch = AXRESTDataModelConvert.DeserializeObject<AXBatch>(apiResult, mediatype);
                return new AXRESTClientBatch(batch, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQuery> CreateQueryAsync(string queryName, Dictionary<string, string> indexes, FullTextSearchOptions ftOptions = null, bool isPublic = true, bool isIncludingPreviousRevisions = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXQueries].HRef, UriKind.Relative);
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

                string apiResult = await POST(apiURL, data, mediatype);
                AXQuery query = AXRESTDataModelConvert.DeserializeObject<AXQuery>(apiResult, mediatype);
                return new AXRESTClientQuery(query, ServerOption);
            }
            finally
            {
            }
        }

        //execute adhoc query
        public async Task<AXRESTClientQueryResults> ExecuteAdhocDocumentQueryAsync(Dictionary<string, string> indexes, FullTextSearchOptions ftOptions = null,
            bool isIncludingPreviousRevisions = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXAdhocQueryResults].HRef, UriKind.Relative);

            QueryModel qm = new QueryModel();
            qm.QueryType = AXQueryTypes.DocumentQuery;
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

            string apiResult = await POST(apiURL, data, mediatype);

            AXQueryResultCollection results = AXRESTDataModelConvert.DeserializeObject<AXQueryResultCollection>(apiResult, mediatype);
            return new AXRESTClientQueryResults(results, ServerOption);
        }
        public async Task<AXRESTClientQueryResults> ExecuteAdhocCAQQueryAsync(int caqID, Dictionary<string, string> indexes, FullTextSearchOptions ftOptions = null,
            bool isIncludingPreviousRevisions = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXAdhocQueryResults].HRef, UriKind.Relative);

            QueryModel qm = new QueryModel();
            qm.ID = caqID;
            qm.QueryType = AXQueryTypes.CrossAppQuery;
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

            string apiResult = await POST(apiURL, data, mediatype);

            AXQueryResultCollection results = AXRESTDataModelConvert.DeserializeObject<AXQueryResultCollection>(apiResult, mediatype);
            return new AXRESTClientQueryResults(results, ServerOption);
        }
        public async Task<AXRESTClientQueryResults> ExecuteAdhocReportQueryAsync(string TIMESTAMP = "", string DESC = "", string RPTTYPE = "",
            string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXAdhocQueryResults].HRef, UriKind.Relative);

            QueryModel qm = new QueryModel();
            qm.QueryType = AXQueryTypes.ReportQuery;

            List<QueryIndex> temp = new List<QueryIndex>();
            temp.Add(new QueryIndex() { Name = "TIMESTAMP", Value = TIMESTAMP });
            temp.Add(new QueryIndex() { Name = "DESC", Value = DESC });
            temp.Add(new QueryIndex() { Name = "RPTTYPE", Value = RPTTYPE });
            qm.Indexes = temp.ToArray();

            string newquery = AXRESTDataModelConvert.SerializeObject(
                qm, mediatype);
            StringContent data = new StringContent(newquery, Encoding.UTF8, mediatype);

            string apiResult = await POST(apiURL, data, mediatype);

            AXQueryResultCollection results = AXRESTDataModelConvert.DeserializeObject<AXQueryResultCollection>(apiResult, mediatype);
            return new AXRESTClientQueryResults(results, ServerOption);
        }

        //get app fields
        public async Task<AXRESTClientAppFields> GetAppFieldsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXFields].HRef, UriKind.Relative);
            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXAppFields appfields = AXRESTDataModelConvert.DeserializeObject<AXAppFields>(apiResult, mediatype);
                return new AXRESTClientAppFields(appfields, ServerOption);
            }
            finally
            {
            }
        }

        //Look up key reference

        public async Task<AXRESTClientKeyReferenceLookupResults> GetKeyReferenceLookupResultsAsync(string keyFieldValue, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXKeyReferenceIndex].HRef, UriKind.Relative);
            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("fieldvalue", keyFieldValue);

                string apiResult = await GET(apiURL, mediatype, paras);
                AXKeyRefIndex keyRefIndex = AXRESTDataModelConvert.DeserializeObject<AXKeyRefIndex>(apiResult, mediatype);
                return new AXRESTClientKeyReferenceLookupResults(keyRefIndex);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueryResults> GetSelectIndexLookupResultsAsync(Dictionary<string, string> indexes, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXSelectIndex].HRef, UriKind.Relative);
            try
            {
                List<QueryIndex> temp = new List<QueryIndex>();
                foreach (var kvp in indexes)
                {
                    temp.Add(new QueryIndex() { Name = kvp.Key, Value = kvp.Value });

                }
                QueryIndex[] qis = temp.ToArray();
                string selectindexQuery = AXRESTDataModelConvert.SerializeObject(
                    qis, mediatype);
                StringContent data = new StringContent(selectindexQuery, Encoding.UTF8, mediatype);
                string apiResult = await POST(apiURL, data, mediatype);
                AXQueryResultCollection selectIndexResult = AXRESTDataModelConvert.DeserializeObject<AXQueryResultCollection>(apiResult, mediatype);
                return new AXRESTClientQueryResults(selectIndexResult, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientQueryResults> GetAutoIndexLookupResultsAsync(Dictionary<string, string> indexes, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.application.Links[AXRESTLinkRelations.AXAutoIndex].HRef, UriKind.Relative);
            try
            {
                List<QueryIndex> temp = new List<QueryIndex>();
                foreach (var kvp in indexes)
                {
                    temp.Add(new QueryIndex() { Name = kvp.Key, Value = kvp.Value });

                }
                QueryIndex[] qis = temp.ToArray();
                string selectindexQuery = AXRESTDataModelConvert.SerializeObject(
                    qis, mediatype);
                StringContent data = new StringContent(selectindexQuery, Encoding.UTF8, mediatype);
                string apiResult = await POST(apiURL, data, mediatype);
                AXQueryResultCollection autoIndexResult = AXRESTDataModelConvert.DeserializeObject<AXQueryResultCollection>(apiResult, mediatype);
                return new AXRESTClientQueryResults(autoIndexResult, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientApplication> Refresh(string mediatype)
        {
            if (string.IsNullOrEmpty(this.application.Self))
                return null;

            var apiURL = new Uri(this.application.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.application = AXRESTDataModelConvert.DeserializeObject<AXApp>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
