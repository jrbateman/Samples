using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientReportDocPages : ClientWrapper
    {
        private AXRESTDataModel.AXReportDocPageCollection pages;

        public AXRESTClientReportDocPages(AXRESTDataModel.AXReportDocPageCollection pages, AXRESTOptions parentOption)
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
                    throw new NullReferenceException("The AXReportDocPages collection is not initialized");
            }
        }

        public List<AXRESTClientReportDocPage> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientReportDocPage>();
                    foreach (var bp in this.pages.Entries)
                    {
                        this.coll.Add(new AXRESTClientReportDocPage(bp, ServerOption));
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

        public async Task<AXRESTClientReportDocPages> GetFirstPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasFirstPage) return null;

            var apiURL = new Uri(this.pages.First.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXReportDocPageCollection pages = AXRESTDataModelConvert.DeserializeObject<AXReportDocPageCollection>(apiResult, mediatype);
                return new AXRESTClientReportDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientReportDocPages> GetNextPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasNextPage) return null;

            var apiURL = new Uri(this.pages.Next.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXReportDocPageCollection pages = AXRESTDataModelConvert.DeserializeObject<AXReportDocPageCollection>(apiResult, mediatype);
                return new AXRESTClientReportDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientReportDocPages> GetLastPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasLastPage) return null;

            var apiURL = new Uri(this.pages.Last.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXReportDocPageCollection pages = AXRESTDataModelConvert.DeserializeObject<AXReportDocPageCollection>(apiResult, mediatype);
                return new AXRESTClientReportDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientReportDocPages> GetPreviousPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasPreviousPage) return null;

            var apiURL = new Uri(this.pages.Previous.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXReportDocPageCollection pages = AXRESTDataModelConvert.DeserializeObject<AXReportDocPageCollection>(apiResult, mediatype);
                return new AXRESTClientReportDocPages(pages, ServerOption);
            }
            finally
            {
            }
        }

        public List<AXRESTClientReportDocPage> coll { get; set; }
    }
}
