using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientQueryResultItem : ClientWrapper
    {
        private AXQueryResultItem item;

        public AXRESTClientQueryResultItem(AXQueryResultItem item, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.item = item;
        }

        public uint ID
        {
            get
            {
                if (this.item != null)
                    return this.item.ID;
                else
                    throw new NullReferenceException("The result item is not initialized");
            }
        }

        public int PageCount
        {
            get
            {
                if (this.item != null)
                    return this.item.PageCount;
                else
                    throw new NullReferenceException("The result item is not initialized");
            }
        }

        public Dictionary<string, bool> Attributes
        {
            get
            {
                if (this.item != null)
                    return this.item.Attributes;
                else
                    throw new NullReferenceException("The result item is not initialized");
            }
        }

        public List<string> IndexValues
        {
            get
            {
                if (this.item != null)
                    return this.item.IndexValues;
                else
                    throw new NullReferenceException("The result item is not initialized");
            }
        }

        public uint? FulltextHits
        {
            get
            {
                if (this.item != null)
                    return this.item.FulltextHits;
                else
                    throw new NullReferenceException("The result item is not initialized");
            }
        }

        public bool IsReport
        {
            get
            {
                return this.item.Links.ContainsKey(AXRESTLinkRelations.AXReport);
            }
        }

        public async Task<AXRESTClientDoc> GetAXDocAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.item.Links.ContainsKey(AXRESTLinkRelations.AXDoc))
                return null;

            var apiURL = new Uri(this.item.Links[AXRESTLinkRelations.AXDoc].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDoc doc = AXRESTDataModelConvert.DeserializeObject<AXDoc>(apiResult, mediatype);
                return new AXRESTClientDoc(doc, ServerOption);
            }
            finally
            {
            }
        }

        //get report document
        public async Task<AXRESTClientReportDoc> GetAXReportDocAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.item.Links.ContainsKey(AXRESTLinkRelations.AXReport))
                return null;

            var apiURL = new Uri(this.item.Links[AXRESTLinkRelations.AXReport].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXReport report = AXRESTDataModelConvert.DeserializeObject<AXReport>(apiResult, mediatype);
                return new AXRESTClientReportDoc(report, ServerOption);
            }
            finally
            {
            }
        }
    }
}
