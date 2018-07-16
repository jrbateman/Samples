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
    public class AXRESTClientDocPageVersions : ClientWrapper
    {
        private AXRESTDataModel.AXDocPageVersionCollection pageversions;

        public AXRESTClientDocPageVersions(AXRESTDataModel.AXDocPageVersionCollection pageversions, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.pageversions = pageversions;
        }

        public int Count
        {
            get
            {
                if (this.pageversions != null)
                    return this.pageversions.Entries.Count;
                else
                    throw new NullReferenceException("The AXDocPageVersions collection is not initialized");
            }
        }

        public List<AXRESTClientDocPageVersion> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientDocPageVersion>();
                    foreach (var pv in this.pageversions.Entries)
                    {
                        this.coll.Add(new AXRESTClientDocPageVersion(pv, ServerOption));
                    }
                }
                return this.coll;
            }
        }

        public bool HasFirstPage
        {
            get
            {
                return this.pageversions.First != null;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return this.pageversions.Next != null;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return this.pageversions.Previous != null;
            }
        }

        public bool HasLastPage
        {
            get
            {
                return this.pageversions.Last != null;
            }
        }


        public async Task<AXRESTClientDocPageVersions> GetFirstPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasFirstPage) return null;

            var apiURL = new Uri(this.pageversions.First.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPageVersionCollection pvs = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersionCollection>(apiResult, mediatype);
                return new AXRESTClientDocPageVersions(pvs, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPageVersions> GetNextPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasNextPage) return null;

            var apiURL = new Uri(this.pageversions.Next.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPageVersionCollection pvs = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersionCollection>(apiResult, mediatype);
                return new AXRESTClientDocPageVersions(pvs, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPageVersions> GetLastPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasLastPage) return null;

            var apiURL = new Uri(this.pageversions.Last.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPageVersionCollection pvs = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersionCollection>(apiResult, mediatype);
                return new AXRESTClientDocPageVersions(pvs, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPageVersions> GetPreviousPageAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!HasPreviousPage) return null;

            var apiURL = new Uri(this.pageversions.Previous.HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPageVersionCollection pvs = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersionCollection>(apiResult, mediatype);
                return new AXRESTClientDocPageVersions(pvs, ServerOption);
            }
            finally
            {
            }
        }

        public List<AXRESTClientDocPageVersion> coll { get; set; }
    }
}
