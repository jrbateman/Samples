using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocPageVersion : ClientWrapper
    {
        private AXRESTDataModel.AXDocPageVersion pageversion;

        public AXRESTClientDocPageVersion(AXRESTDataModel.AXDocPageVersion pageversion, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.pageversion = pageversion;
        }

        public int PageNumber
        {
            get
            {
                if (this.pageversion != null)
                    return this.pageversion.Page;
                else
                    throw new NullReferenceException("The axdocpageversion is not initialized");
            }
        }

        public int Version
        {
            get
            {
                if (this.pageversion != null)
                    return this.pageversion.Version;
                else
                    throw new NullReferenceException("The axdocpageversion is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("DocumentPageVersion{0}", Version);
        }

        public string Location
        {
            get
            {
                if (this.pageversion != null)
                    return this.pageversion.Self;
                else
                    throw new NullReferenceException("The axdocpageversion is not initialized");
            }
        }

        public bool HasAnnotation
        {
            get
            {
                if (this.pageversion != null)
                    return this.pageversion.HasAnnotation;
                else
                    throw new NullReferenceException("The axdocpageversion is not initialized");
            }
        }

        public bool HasTextView
        {
            get
            {
                if (this.pageversion != null)
                    return this.pageversion.HasTextView;
                else
                    throw new NullReferenceException("The axdocpageversion is not initialized");
            }
        }

        public async Task<AXRESTClientDocPageVersion> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.pageversion.Self))
                return null;

            var apiURL = new Uri(this.pageversion.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.pageversion = AXRESTDataModelConvert.DeserializeObject<AXDocPageVersion>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }

        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.pageversion.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.pageversion = null;
                this.Dispose();
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

            var apiURL = new Uri(this.pageversion.Self, UriKind.Relative);

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

        public async Task<AXRESTClientFile> RenderAsync(string filename, string mediatype = AXRESTMediaTypes.JPG, int subpage = 1, int formOverlayOption = 0,
            int annotationRedactionOption = 0, int ClientProfile = 1)
        {
            var apiURL = new Uri(this.pageversion.Links[AXRESTLinkRelations.AXRendition].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras["subpage"] = subpage.ToString();
                paras["formOverlayOption"] = formOverlayOption.ToString();
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
            var apiURL = new Uri(this.pageversion.Links[AXRESTLinkRelations.AXThumbnail].HRef, UriKind.Relative);

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
    }
}
