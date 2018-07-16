using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientBatchPages : ClientWrapper
    {
        private AXRESTDataModel.AXBatchPagesCollection pages;
        private List<AXRESTClientBatchPage> coll;

        public AXRESTClientBatchPages(AXRESTDataModel.AXBatchPagesCollection pages, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.pages = pages;
        }

        public int Count
        {
            get
            {
                if (this.pages != null)
                    return this.pages.Entries.Count;
                else
                    throw new NullReferenceException("The AXBatchPages collection is not initialized");
            }
        }

        public List<AXRESTClientBatchPage> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientBatchPage>();
                    foreach (var bp in this.pages.Entries)
                    {
                        this.coll.Add(new AXRESTClientBatchPage(bp, ServerOption));
                    }
                }
                return this.coll;
            }
        }

        public bool HasFirstPage
        {
            get
            {
                return this.pages.First != null;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return this.pages.Next != null;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return this.pages.Previous != null;
            }
        }

        public bool HasLastPage
        {
            get
            {
                return this.pages.Last != null;
            }
        }

        public async Task<AXRESTClientBatchPage> CreateBatchPageAsync(List<AXRESTClientFile> clientFiles, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.pages.Self, UriKind.Relative);
            try
            {
                MultipartFormDataContent apiContent = new MultipartFormDataContent();

                foreach (var clientfile in clientFiles)
                {
                    var streamContent = new StreamContent(clientfile.Stream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                    apiContent.Add(streamContent, clientfile.TypeName, clientfile.FileName);
                }

                string apiResult = await POST(apiURL, apiContent, mediatype);
                AXBatchPage page = AXRESTDataModelConvert.DeserializeObject<AXBatchPage>(apiResult, mediatype);
                return new AXRESTClientBatchPage(page, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientBatchPages> GetFirstPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasFirstPage) return null;

            var apiURL = new Uri(this.pages.First.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXBatchPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXBatchPagesCollection>(apiResult, mediatype);
                return new AXRESTClientBatchPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientBatchPages> GetNextPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasNextPage) return null;

            var apiURL = new Uri(this.pages.Next.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXBatchPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXBatchPagesCollection>(apiResult, mediatype);
                return new AXRESTClientBatchPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientBatchPages> GetLastPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasLastPage) return null;

            var apiURL = new Uri(this.pages.Last.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXBatchPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXBatchPagesCollection>(apiResult, mediatype);
                return new AXRESTClientBatchPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientBatchPages> GetPreviousPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasPreviousPage) return null;

            var apiURL = new Uri(this.pages.Previous.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXBatchPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXBatchPagesCollection>(apiResult, mediatype);
                return new AXRESTClientBatchPages(pages, ServerOption);
            }
            finally
            {
            }
        }


    }
}
