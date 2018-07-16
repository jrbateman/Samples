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
    public class AXRESTClientDocPage : ClientWrapper
    {
        private AXRESTDataModel.AXDocPage page;

        public AXRESTClientDocPage(AXRESTDataModel.AXDocPage page, AXRESTOptions parentOption)
            :base(parentOption)
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
                    throw new NullReferenceException("The AXDocPage is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("DocumentPage{0}", PageNumber);
        }

        public string Location
        {
            get
            {
                if (this.page != null)
                    return this.page.Self;
                else
                    throw new NullReferenceException("The AXDocPage is not initialized");
            }
        }

        public string CurrentVersionLocation
        {
            get
            {
                if (this.page != null)
                    return this.page.Links[AXRESTLinkRelations.CurrentVersion].HRef;
                else
                    throw new NullReferenceException("The AXDocPage is not initialized");
            }
        }

        public string VersionHistoryLocation
        {
            get
            {
                if (this.page != null)
                    return this.page.Links[AXRESTLinkRelations.VersionHistory].HRef;
                else
                    throw new NullReferenceException("The AXDocPage is not initialized");
            }
        }

        public async Task<AXRESTClientDocPage> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.page.Self))
                return null;

            var apiURL = new Uri(this.page.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.page = AXRESTDataModelConvert.DeserializeObject<AXDocPage>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPageVersions> GetPageVersionsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.page.Links[AXRESTLinkRelations.VersionHistory].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPageVersionCollection pageversions = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersionCollection>(apiResult, mediatype);
                return new AXRESTClientDocPageVersions(pageversions, ServerOption);
            }
            finally
            {
            }
        }

        public async Task NewPageVersionAsync(AXRESTClientFile binFile, AXRESTClientFile annoFile = null, AXRESTClientFile textFile = null,
            string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.page.Links[AXRESTLinkRelations.VersionHistory].HRef, UriKind.Relative);

            try
            {
                MultipartFormDataContent apiContent = new MultipartFormDataContent();

                var bin = new StreamContent(binFile.Stream);
                bin.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                apiContent.Add(bin, binFile.TypeName, binFile.FileName);

                if (annoFile != null)
                {
                    var anno = new StreamContent(annoFile.Stream);
                    apiContent.Add(anno, annoFile.TypeName, annoFile.FileName);
                }
                if (textFile != null)
                {
                    var text = new StreamContent(textFile.Stream);
                    apiContent.Add(text, textFile.TypeName, textFile.FileName);
                }

                string apiResult = await POST(apiURL, apiContent, mediatype);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDocPageVersion> GetCurrentPageVersionAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.page.Links[AXRESTLinkRelations.CurrentVersion].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocPageVersion pageversion = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersion>(apiResult, mediatype);
                return new AXRESTClientDocPageVersion(pageversion, ServerOption);
            }
            finally
            {
            }
        }

        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.page.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.page = null;
                this.Dispose();
            }
            finally
            {
            }
        }
    }
}
