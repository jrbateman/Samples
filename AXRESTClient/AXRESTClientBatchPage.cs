using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientBatchPage : ClientWrapper
    {
        private AXRESTDataModel.AXBatchPage page;

        public AXRESTClientBatchPage(AXRESTDataModel.AXBatchPage page, AXRESTOptions parentOption)
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
                    throw new NullReferenceException("The AXBatchPage is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("BatchPage{0}", PageNumber);
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

        public async Task<AXRESTClientFile> RenderAsync(string filename, string mediatype = AXRESTMediaTypes.JPG, int subpage = 1, int annotationRedactionOption = 0, int ClientProfile = 1)
        {
            var apiURL = new Uri(this.page.Links[AXRESTLinkRelations.AXRendition].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();

                paras["subpage"] = subpage.ToString();
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

        public async Task<AXRESTClientFile> GetThumbnailAsync(int thumbnailWidth = 0, int thumbnailHeight = 0, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.page.Links[AXRESTLinkRelations.AXThumbnail].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("thumbnailWidth", thumbnailWidth.ToString());
                paras.Add("thumbnailHeight", thumbnailHeight.ToString());

                var mpContents = await GETMultipart(apiURL, mediatype, paras);

                byte[] fileBytes = mpContents.Contents[0].ReadAsByteArrayAsync().Result;
                AXRESTClientFile retFile = AXRESTClientFile.LoadFromMemoryBytes(fileBytes, "", AXRESTClientFile.AXClientFileTypes.Rendition);
                return retFile;
            }
            finally
            {
            }
        }

        public async Task UpdateAnnotationAndTextFileAsync(AXRESTClientFile annoFile = null, AXRESTClientFile textFile = null,
            string mediatype = AXRESTMediaTypes.JSON)
        {
            if (annoFile == null && textFile == null)
                throw new ArgumentException("Please provide either of the annotation and text files");

            var apiURL = new Uri(this.page.Self, UriKind.Relative);

            try
            {
                MultipartFormDataContent apiContent = new MultipartFormDataContent();

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

                string apiResult = await PUT(apiURL, apiContent, mediatype);
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

        public async Task<AXRESTClientBatchPage> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.page.Self))
                return null;

            var apiURL = new Uri(this.page.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.page = AXRESTDataModelConvert.DeserializeObject<AXBatchPage>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
