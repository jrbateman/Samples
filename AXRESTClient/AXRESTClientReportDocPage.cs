using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientReportDocPage : ClientWrapper
    {
        private AXRESTDataModel.AXReportDocPage page;

        public AXRESTClientReportDocPage(AXRESTDataModel.AXReportDocPage page, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.page = page;
        }

        public int PageNumber
        {
            get
            {
                if (this.page != null)
                    return this.page.Page;
                else
                    throw new NullReferenceException("The AXReportDocPage is not initialized");
            }
        }

        public string Location
        {
            get
            {
                if (this.page != null)
                    return this.page.Self;
                else
                    throw new NullReferenceException("The AXBatchPage is not initialized");
            }
        }

        public async Task<AXRESTClientFile> RenderAsync(string filename, string mediatype = AXRESTMediaTypes.JPG, int annotationRedactionOption = 0, int ClientProfile = 1)
        {
            var apiURL = new Uri(this.page.Links[AXRESTLinkRelations.AXRendition].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();

                paras["annotationRedactionOption"] = annotationRedactionOption.ToString();
                paras["ClientProfile"] = ClientProfile.ToString();

                byte[] fileBytes = await GETBinary(apiURL, mediatype, paras);
                AXRESTClientFile retFile = AXRESTClientFile.LoadFromMemoryBytes(fileBytes, filename, AXRESTClientFile.AXClientFileTypes.Rendition);
                return retFile;
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientReportDocPage> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.page.Self))
                return null;

            var apiURL = new Uri(this.page.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.page = AXRESTDataModelConvert.DeserializeObject<AXReportDocPage>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
