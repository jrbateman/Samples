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
    public class AXRESTClientDoc : ClientWrapper
    {
        private AXRESTDataModel.AXDoc doc;

        public AXRESTClientDoc(AXRESTDataModel.AXDoc doc, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.doc = doc;
        }

        public uint ID
        {
            get
            {
                if (this.doc != null)
                    return this.doc.ID;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public string Location
        {
            get
            {
                if (this.doc != null)
                    return this.doc.Self;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public int PageCount
        {
            get
            {
                if (this.doc != null)
                    return this.doc.PageCount;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public bool FulltextIndexed
        {
            get
            {
                if (this.doc != null)
                    return this.doc.SubmitFulltext;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public bool Checkedout
        {
            get
            {
                if (this.doc != null)
                    return this.doc.Attributes.Checkedout;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public bool PreviousRevision
        {
            get
            {
                if (this.doc != null)
                    return this.doc.Attributes.PreviousRevision;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public bool HasVersions
        {
            get
            {
                if (this.doc != null)
                    return this.doc.Attributes.HasVersions;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public bool FinalRevision
        {
            get
            {
                if (this.doc != null)
                    return this.doc.Attributes.FinalRevision;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public bool IsCOLD
        {
            get
            {
                if (this.doc != null)
                    return this.doc.Attributes.IsCOLD;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        public string WorkingCopyOfLocation
        {
            get
            {
                if (this.doc != null)
                    return this.doc.WorkingCopy ? this.doc.Links[AXRESTLinkRelations.WorkingCopyOf].HRef : null;
                else
                    throw new NullReferenceException("The AXDoc is not initialized");
            }
        }

        //Get thumbnails
        public async Task<List<AXRESTClientFile>> GetThumbnailsAsync(int pageStart, int pageEnd, int thumbnailWidth = 0, int thumbnailHeight = 0, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.AXThumbnail].HRef, UriKind.Relative);

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

        //Get Indexes
        public async Task<AXRESTClientDocIndexes> GetDocIndexesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.AXIndexes].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocIndexes indexes = AXRESTDataModelConvert.DeserializeObject<AXDocIndexes>(apiResult, mediatype);
                return new AXRESTClientDocIndexes(indexes, ServerOption);
            }
            finally
            {
            }
        }

        //New Doc Index

        public async Task<AXRESTClientDocIndex> NewDocIndexAsync(Dictionary<string, string> indexvalues, bool failIfMatchIndex = false, bool failIfDLSViolation = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.AXIndexes].HRef, UriKind.Relative);

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


            string tempContent = AXRESTDataModelConvert.SerializeObject(
                newIndex, mediatype);
            StringContent apiContent = new StringContent(tempContent, Encoding.UTF8, mediatype);

            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("FailIfMatchIndex", failIfMatchIndex.ToString());
            paras.Add("FailIfDLSViolation", failIfDLSViolation.ToString());
            string apiResult = await POST(apiURL, apiContent, mediatype, paras);
            AXDocIndex index = AXRESTDataModelConvert.DeserializeObject<AXDocIndex>(apiResult, mediatype);
            return new AXRESTClientDocIndex(index, ServerOption);
        }

        //Get ODMA 
        public async Task<AXRESTClientDocODMA> GetAXDocODMAAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.AXDocODMA].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocODMA odma = AXRESTDataModelConvert.DeserializeObject<AXDocODMA>(apiResult, mediatype);
                return new AXRESTClientDocODMA(odma);
            }
            finally
            {
            }
        }

        //Get DocPages
        public async Task<AXRESTClientDocPages> GetAXDocPagesAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.AXDocPages].HRef, UriKind.Relative);

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

        //New Doc Page
        public async Task NewDocPageAsync(AXRESTClientFile binFile, AXRESTClientFile annoFile = null, AXRESTClientFile textFile = null,
            int insertLocation = 0, int insertOperation = 2, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.AXDocPages].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("idx", insertLocation.ToString());
                paras.Add("pageop", insertOperation.ToString());

                MultipartFormDataContent apiContent = new MultipartFormDataContent();

                var bin = new StreamContent(binFile.Stream);
                bin.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                apiContent.Add(bin, binFile.TypeName, binFile.FileName);

                if (annoFile != null)
                {
                    var anno = new StreamContent(annoFile.Stream);
                    anno.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                    apiContent.Add(anno, annoFile.TypeName, annoFile.FileName);
                }
                if (textFile != null)
                {
                    var text = new StreamContent(textFile.Stream);
                    text.Headers.ContentType = new MediaTypeHeaderValue("application/bin");
                    apiContent.Add(text, textFile.TypeName, textFile.FileName);
                }

                string apiResult = await POST(apiURL, apiContent, mediatype, paras);
            }
            finally
            {
            }
        }

        //Get working copy of
        public async Task<AXRESTClientDoc> GetWorkingCopyOfAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.doc.WorkingCopy)
                return null;

            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.WorkingCopyOf].HRef, UriKind.Relative);

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
        //Cancel check out
        public async Task CancelCheckOutAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.doc.WorkingCopy && !this.doc.Attributes.Checkedout)
                throw new InvalidOperationException("The document is not checked out nor a working copy. Cannot perform cancel check out operation");

            if (this.doc.WorkingCopy)
            {
                var apiURL = new Uri(this.doc.Self, UriKind.Relative);

                try
                {
                    string apiResult = await DELETE(apiURL, mediatype);
                }
                finally
                {
                }
            }
            else
            {
                var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.WorkingCopy].HRef, UriKind.Relative);

                try
                {
                    string apiResult = await DELETE(apiURL, mediatype);
                }
                finally
                {
                }
            }
        }
        //Check in
        public async Task<AXRESTClientDocRevisions> CheckInAsync(int checkInAction, bool finalRevision, string comments, bool fulltext, string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.doc.WorkingCopy)
                throw new InvalidOperationException("This is not a working copy. Cannot perform check in operation");

            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.VersionHistory].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("chkinAction", checkInAction.ToString());
                paras.Add("finalrev", finalRevision.ToString());
                paras.Add("comments", comments.ToString());
                paras.Add("fulltext", fulltext.ToString());

                StringContent content = new StringContent("", Encoding.UTF8, "multipart/form-data");
                string apiResult = await POST(apiURL, content, mediatype, paras);
                AXDocRevisionHistory docrevisions = AXRESTDataModelConvert.DeserializeObject<AXDocRevisionHistory>(apiResult, mediatype);
                return new AXRESTClientDocRevisions(docrevisions, ServerOption);
            }
            finally
            {
            }
        }
        //check out
        public async Task<AXRESTClientDoc> CheckOutAsync(string comment, string mediatype = AXRESTMediaTypes.JSON)
        {
            if (this.doc.Attributes.Checkedout)
                throw new InvalidOperationException("The document is already checked out. Cannot perform Check out operation");

            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.WorkingCopy].HRef, UriKind.Relative);

            try
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("comment", comment);
                StringContent content = new StringContent("", Encoding.UTF8, "multipart/form-data");
                string apiResult = await POST(apiURL, content, mediatype, paras);
                AXDoc doc = AXRESTDataModelConvert.DeserializeObject<AXDoc>(apiResult, mediatype);
                return new AXRESTClientDoc(doc, ServerOption);
            }
            finally
            {
            }
        }
        //resume check out and return working copy
        public async Task<AXRESTClientDoc> ResumeCheckOutAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.doc.Attributes.Checkedout)
                throw new InvalidOperationException("The document is not checked out. Cannot perform resume check out operation");

            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.WorkingCopy].HRef, UriKind.Relative);

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

        //get version history

        public async Task<AXRESTClientDocRevisions> GetDocRevisionsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.doc.Attributes.HasVersions)
                return null;

            var apiURL = new Uri(this.doc.Links[AXRESTLinkRelations.VersionHistory].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDocRevisionHistory docrevisions = AXRESTDataModelConvert.DeserializeObject<AXDocRevisionHistory>(apiResult, mediatype);
                return new AXRESTClientDocRevisions(docrevisions, ServerOption);
            }
            finally
            {
            }
        }

        //Delete
        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.doc = null;
                this.Dispose();
            }
            finally
            {
            }
        }

        //Put - Submit OCR or FullTextJobs
        public async Task SubmitJobsAsync(bool submitFTJob, bool submitOcrJob, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.doc.Self, UriKind.Relative);
            try
            {
                string updatedDoc = AXRESTDataModelConvert.SerializeObject(
                    new AXDoc() { SubmitFulltext = submitFTJob, SubmitOCR = submitOcrJob },
                    mediatype);
                StringContent apiContent = new StringContent(updatedDoc, Encoding.UTF8, mediatype);

                string apiResult = await PUT(apiURL, apiContent, mediatype);
            }
            finally
            {
            }
        }
    }
}
