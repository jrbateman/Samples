using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientBatch : ClientWrapper
    {
        private AXRESTDataModel.AXBatch batch;

        public int ID
        {
            get
            {
                if (this.batch != null)
                    return this.batch.ID;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }
        public string Location
        {
            get
            {
                if (this.batch != null)
                    return this.batch.Self;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }

        public string Name
        {
            get
            {
                if (this.batch != null)
                    return this.batch.Name;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }
        public string Description
        {
            get
            {
                if (this.batch != null)
                    return this.batch.Description;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }
        public DateTime CreationTime
        {
            get
            {
                if (this.batch != null)
                    return this.batch.CreationTime;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }
        public int PageCount
        {
            get
            {
                if (this.batch != null)
                    return this.batch.PageCount;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }
        public bool Private
        {
            get
            {
                if (this.batch != null)
                    return this.batch.Private == true;
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }
        public string State
        {
            get
            {
                if (this.batch != null)
                    return this.batch.State.ToString();
                else
                    throw new NullReferenceException("The AX batch document is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", ID, Name);
        }

        public AXRESTClientBatch(AXRESTDataModel.AXBatch Batch, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.batch = Batch; 
        }

        public async Task UpdateAsync(string batchName, string batchDescription, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.batch.Self, UriKind.Relative); 
            try
            {
                string updatedbatch = AXRESTDataModelConvert.SerializeObject(
                    new AXBatch() { Name = batchName, Description = batchDescription }, mediatype);
                StringContent apiContent = new StringContent(updatedbatch, Encoding.UTF8, mediatype);

                string apiResult = await PUT(apiURL, apiContent, mediatype);
            }
            finally
            {
            }
        }

        public async Task AppendAsync(List<AXRESTClientFile> clientFiles, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.batch.Self, UriKind.Relative); 
            try
            {
                MultipartFormDataContent apiContent = new MultipartFormDataContent();
                string thisbatch = AXRESTDataModelConvert.SerializeObject(
                    new AXBatch() { Name = this.batch.Name}, mediatype);
                StringContent data = new StringContent(thisbatch, Encoding.UTF8, mediatype);
                apiContent.Add(data, "data");

                foreach (var clientFile in clientFiles)
                {
                    var streamContent = new StreamContent(clientFile.Stream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/bin");

                    apiContent.Add(streamContent, clientFile.TypeName, clientFile.FileName);
                }

                string apiResult = await POST(apiURL, apiContent, mediatype);
                this.batch = AXRESTDataModelConvert.DeserializeObject<AXBatch>(apiResult, mediatype); 
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDoc> IndexAsync(Dictionary<string, string> indexvalues, int batchPageNumber = 0, uint targetDocId = 0,  bool ignoreDuplicateIndex = true, bool ignoreDlsViolation = true, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.batch.Links[AXRESTLinkRelations.AXDoc].HRef, UriKind.Relative);
            try
            {
                AXDocIndex newIndex = new AXDocIndex();
                List<AXDocIndexValue> temp = new List<AXDocIndexValue>();
                foreach (var kvp in indexvalues)
                {
                    AXDocIndexValue value = new AXDocIndexValue();
                    value.FieldID = kvp.Key;
                    value.FieldValue = kvp.Value;
                    temp.Add(value);
                }
                newIndex.IndexValues = temp.ToArray();

                string newDoc = AXRESTDataModelConvert.SerializeObject(
                    new NewDocumentRequest()
                    {
                        NewIndex = newIndex,
                        FromBatch = new AXBatch(){ ID = this.batch.ID},
                        TargetDoc = new AXDoc() { ID = targetDocId },
                        BatchPageNum = batchPageNumber,
                        IgnoreDlsViolation = ignoreDlsViolation,
                        IgnoreDuplicateIndex = ignoreDuplicateIndex
                    }, mediatype);

                StringContent apiContent = new StringContent(newDoc, Encoding.UTF8, mediatype);

                string apiResult = await POST(apiURL, apiContent, mediatype);
                var doc = AXRESTDataModelConvert.DeserializeObject<AXDoc>(apiResult, mediatype);
                return new AXRESTClientDoc(doc, ServerOption);
            }
            finally
            {
            }
        }

        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.batch.Self, UriKind.Relative); 

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.batch = null;
                this.Dispose();
            }
            finally
            {
            }
        }

        //get batch pages
        public async Task<AXRESTClientBatchPages> GetAXBatchPagesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.batch.Links[AXRESTLinkRelations.AXBatchPages].HRef, UriKind.Relative);

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


        public async Task<AXRESTClientUser> GetCreatorAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.batch.Links.ContainsKey(AXRESTLinkRelations.Creator))
                return null;

            var apiURL = new Uri(this.batch.Links[AXRESTLinkRelations.Creator].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXUser creator = AXRESTDataModelConvert.DeserializeObject<AXUser>(apiResult, mediatype);
                return new AXRESTClientUser(creator);
            }
            finally
            {
            }
        }

        //get thumbnails

        public async Task<List<AXRESTClientFile>> GetThumbnailsAsync(int pageStart, int pageEnd, int thumbnailWidth = 0, int thumbnailHeight = 0, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.batch.Links[AXRESTLinkRelations.AXThumbnail].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("pageStart", pageStart.ToString());
                paras.Add("pageEnd", pageEnd.ToString());
                paras.Add("thumbnailWidth", thumbnailWidth.ToString());
                paras.Add("thumbnailHeight", thumbnailHeight.ToString());

                var mpContents = await GETMultipart(apiURL, mediatype, paras);

                List<AXRESTClientFile> thumbnails = new List<AXRESTClientFile>();
                foreach (var content in mpContents.Contents)
                {
                    byte[] bCont = content.ReadAsByteArrayAsync().Result;
                    var thumb = AXRESTClientFile.LoadFromMemoryBytes(bCont,
                        "", AXRESTClientFile.AXClientFileTypes.Rendition);
                    thumbnails.Add(thumb);
                }

                return thumbnails;
            }
            finally
            {
            }
        }


        public async Task<AXRESTClientBatch> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.batch.Self))
                return null;

            var apiURL = new Uri(this.batch.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.batch = AXRESTDataModelConvert.DeserializeObject<AXBatch>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
