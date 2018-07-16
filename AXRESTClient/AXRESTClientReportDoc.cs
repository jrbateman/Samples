using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientReportDoc : ClientWrapper
    {
        private AXRESTDataModel.AXReport report;

        public AXRESTClientReportDoc(AXRESTDataModel.AXReport report, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.report = report;
        }

        public int ID
        {
            get
            {
                if (this.report != null)
                    return this.report.ID;
                else
                    throw new NullReferenceException("The AXReportDoc is not initialized");
            }
        }

        public int PageCount
        {
            get
            {
                if (this.report != null)
                    return this.report.PageCount;
                else
                    throw new NullReferenceException("The AXReportDoc is not initialized");
            }
        }

        public string Description
        {
            get
            {
                if (this.report != null)
                    return this.report.Description;
                else
                    throw new NullReferenceException("The AXReportDoc is not initialized");
            }
        }

        public string ReportType
        {
            get
            {
                if (this.report != null)
                    return this.report.ReportType;
                else
                    throw new NullReferenceException("The AXReportDoc is not initialized");
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                if (this.report != null)
                    return this.report.TimeStamp;
                else
                    throw new NullReferenceException("The AXReportDoc is not initialized");
            }
        }

        public async Task<AXRESTClientReportDocPages> GetAXReportDocPagesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.report.Links[AXRESTLinkRelations.AXReportPages].HRef, UriKind.Relative);

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

        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.report.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.report = null;
                this.Dispose();
            }
            finally
            {
            }
        }
    }
}
