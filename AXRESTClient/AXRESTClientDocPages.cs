using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocPages : ClientWrapper
    {
        private AXRESTDataModel.AXDocPagesCollection pages;

        public AXRESTClientDocPages(AXRESTDataModel.AXDocPagesCollection pages, AXRESTOptions parentOption)
            :base(parentOption)
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
                    throw new NullReferenceException("The AXDocPages collection is not initialized");
            }
        }

        public List<AXRESTClientDocPage> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientDocPage>();
                    foreach (var page in this.pages.Entries)
                    {
                        this.coll.Add(new AXRESTClientDocPage(page, ServerOption));
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


        public async Task<AXRESTClientDocPages> GetFirstPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasFirstPage) return null;

            var apiURL = new Uri(this.pages.First.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXDocPagesCollection>(apiResult, mediatype);
                return new AXRESTClientDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPages> GetNextPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasNextPage) return null;

            var apiURL = new Uri(this.pages.Next.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXDocPagesCollection>(apiResult, mediatype);
                return new AXRESTClientDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPages> GetLastPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasLastPage) return null;

            var apiURL = new Uri(this.pages.Last.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXDocPagesCollection>(apiResult, mediatype);
                return new AXRESTClientDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPages> GetPreviousPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasPreviousPage) return null;

            var apiURL = new Uri(this.pages.Previous.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPagesCollection pages = AXRESTDataModelConvert.DeserializeObject<AXDocPagesCollection>(apiResult, mediatype);
                return new AXRESTClientDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public List<AXRESTClientDocPage> coll { get; set; }
    }
}
