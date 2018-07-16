using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientQueryResults : ClientWrapper
    {
        private AXQueryResultCollection queryResults;
        private List<AXRESTClientQueryResultItem> coll;

        public AXRESTClientQueryResults(AXQueryResultCollection queryResults, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.queryResults = queryResults;
        }

        public int Count
        {
            get
            {
                if (this.queryResults != null)
                    return this.queryResults.Entries.Count;
                else
                    throw new NullReferenceException("The AX query result is not initialized");
            }
        }

        public List<AXRESTClientQueryResultItem> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientQueryResultItem>();
                    foreach (var resultitem in this.queryResults.Entries)
                    {
                        this.coll.Add(new AXRESTClientQueryResultItem(resultitem, ServerOption));
                    }
                }
                return this.coll;
            }
        }

        public List<string> Columns
        {
            get
            {
                if (this.queryResults != null)
                    return this.queryResults.Columns;
                else
                    throw new NullReferenceException("The AX query result is not initialized");
            }
        }

        public DateTime LastUpdated
        {
            get
            {
                if (this.queryResults != null)
                    return this.queryResults.LastUpdated;
                else
                    throw new NullReferenceException("The AX query result is not initialized");
            }
        }

        public string Id
        {
            get
            {
                if (this.queryResults != null)
                    return this.queryResults.Id;
                else
                    throw new NullReferenceException("The AX query result is not initialized");
            }
        }

        public bool HasFirstPage
        {
            get
            {
                return this.queryResults.First != null;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return this.queryResults.Next != null;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return this.queryResults.Previous != null;
            }
        }

        public bool HasLastPage
        {
            get
            {
                return this.queryResults.Last != null;
            }
        }

        public async Task<AXRESTClientQueryResults> GetFirstPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasFirstPage) return null;

            var apiURL = new Uri(this.queryResults.First.HRef, UriKind.Relative);

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

        public async Task<AXRESTClientQueryResults> GetNextPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasNextPage) return null;

            var apiURL = new Uri(this.queryResults.Next.HRef, UriKind.Relative);

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

        public async Task<AXRESTClientQueryResults> GetLastPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasLastPage) return null;

            var apiURL = new Uri(this.queryResults.Last.HRef, UriKind.Relative);

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

        public async Task<AXRESTClientQueryResults> GetPreviousPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasPreviousPage) return null;

            var apiURL = new Uri(this.queryResults.Previous.HRef, UriKind.Relative);

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
    }
}
