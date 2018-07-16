using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocIndexes : ClientWrapper
    {
        private AXRESTDataModel.AXDocIndexes indexes;

        public AXRESTClientDocIndexes(AXRESTDataModel.AXDocIndexes indexes, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.indexes = indexes;
        }

        public int Count
        {
            get
            {
                if (this.indexes != null)
                    return this.indexes.Entries.Count;
                else
                    throw new NullReferenceException("The AXDocIndexes collection is not initialized");
            }
        }


        public List<AXRESTClientDocIndex> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientDocIndex>();
                    foreach (var index in this.indexes.Entries)
                    {
                        this.coll.Add(new AXRESTClientDocIndex(index, ServerOption));
                    }
                }
                return this.coll;
            }
        }

        public bool HasFirstPage
        {
            get
            {
                return this.indexes.First != null;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return this.indexes.Next != null;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return this.indexes.Previous != null;
            }
        }

        public bool HasLastPage
        {
            get
            {
                return this.indexes.Last != null;
            }
        }


        public async Task<AXRESTClientDocIndexes> GetFirstPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasFirstPage) return null;

            var apiURL = new Uri(this.indexes.First.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocIndexes docindexes = AXRESTDataModelConvert.DeserializeObject<AXDocIndexes>(apiResult, mediatype);
                return new AXRESTClientDocIndexes(docindexes, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocIndexes> GetNextPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasNextPage) return null;

            var apiURL = new Uri(this.indexes.Next.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocIndexes docindexes = AXRESTDataModelConvert.DeserializeObject<AXDocIndexes>(apiResult, mediatype);
                return new AXRESTClientDocIndexes(docindexes, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocIndexes> GetLastPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasLastPage) return null;

            var apiURL = new Uri(this.indexes.Last.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocIndexes docindexes = AXRESTDataModelConvert.DeserializeObject<AXDocIndexes>(apiResult, mediatype);
                return new AXRESTClientDocIndexes(docindexes, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocIndexes> GetPreviousPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasPreviousPage) return null;

            var apiURL = new Uri(this.indexes.Previous.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocIndexes docindexes = AXRESTDataModelConvert.DeserializeObject<AXDocIndexes>(apiResult, mediatype);
                return new AXRESTClientDocIndexes(docindexes, ServerOption);
            }
            finally
            {
            }
        }
       
        public List<AXRESTClientDocIndex> coll { get; set; }
    }
}
