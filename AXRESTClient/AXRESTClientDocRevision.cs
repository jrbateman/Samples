using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocRevision : ClientWrapper
    {
        private AXRESTDataModel.AXDocRevision revision;

        public AXRESTClientDocRevision(AXRESTDataModel.AXDocRevision revision, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.revision = revision;
        }

        public string RevisionNumber
        {
            get
            {
                if (this.revision != null)
                    return this.revision.RevisionNumber;
                else
                    throw new NullReferenceException("The AXDocRevision is not initialized");
            }
        }

        public override string ToString()
        {
            if (this.revision != null)
                return string.Format("DocumentRevision{0}", RevisionNumber);
            else
                throw new NullReferenceException("The AXDocRevision is not initialized");
        }

        public string CheckinBy
        {
            get
            {
                if (this.revision != null)
                    return this.revision.CheckinBy;
                else
                    throw new NullReferenceException("The AXDocRevision is not initialized");
            }
        }

        public DateTime CheckinDate
        {
            get
            {
                if (this.revision != null)
                    return this.revision.CheckinDate;
                else
                    throw new NullReferenceException("The AXDocRevision is not initialized");
            }
        }

        public string CheckinComment
        {
            get
            {
                if (this.revision != null)
                    return this.revision.CheckinComment;
                else
                    throw new NullReferenceException("The AXDocRevision is not initialized");
            }
        }

        public string RevisionDocumentLocation
        {
            get
            {
                if (this.revision != null)
                    return this.revision.Links[AXRESTLinkRelations.AXDoc].HRef;
                else
                    throw new NullReferenceException("The AXDocRevision is not initialized");
            }
        }

        public async Task<AXRESTClientDocRevision> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.revision.Self))
                return null;

            var apiURL = new Uri(this.revision.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.revision = AXRESTDataModelConvert.DeserializeObject<AXDocRevision>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }

        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.revision.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.revision = null;
                this.Dispose();
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDoc> GetRevisionDocAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.revision.Links.ContainsKey(AXRESTLinkRelations.AXDoc))
                return null;

            var apiURL = new Uri(this.revision.Links[AXRESTLinkRelations.AXDoc].HRef, UriKind.Relative);

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
    }
}
