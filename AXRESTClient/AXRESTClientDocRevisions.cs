using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocRevisions : ClientWrapper
    {
        private AXRESTDataModel.AXDocRevisionHistory docrevisions;

        public AXRESTClientDocRevisions(AXRESTDataModel.AXDocRevisionHistory docrevisions, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.docrevisions = docrevisions;
        }

        public string CurrentDocRevision
        {
            get
            {
                if (this.docrevisions != null)
                    return this.docrevisions.CurrentDocRevision;
                else
                    throw new NullReferenceException("The AXDocRevisionHistory is not initialized");
            }
        }

        public int Count
        {
            get
            {
                if (this.docrevisions != null)
                    return this.docrevisions.Entries.Count;
                else
                    throw new NullReferenceException("The AXDocRevisionHistory is not initialized");
            }
        }

        public List<AXRESTClientDocRevision> Revisions
        {
            get
            {
                if (this.docrevisions != null)
                {
                    if (revs == null)
                    {
                        revs = new List<AXRESTClientDocRevision>();
                        foreach (var r in this.docrevisions.Entries)
                        {
                            revs.Add(new AXRESTClientDocRevision(r, ServerOption));
                        }
                    }
                    return revs;
                }
                else
                    throw new NullReferenceException("The AXDocRevisionHistory is not initialized");
            }
        }
        private List<AXRESTClientDocRevision> revs;

        public AXRESTClientDocRevision GetRevisionAsync(string revisionNumber, string mediatype = AXRESTMediaTypes.JSON)
        {
            if (this.docrevisions == null)
                throw new NullReferenceException("The AXDocRevisionHistory collection is not initialized");
            foreach(var r in this.docrevisions.Entries)
            {
                if (string.Compare(r.RevisionNumber, revisionNumber, true) == 0)
                {
                    return new AXRESTClientDocRevision(r, ServerOption);
                }
            }

            throw new KeyNotFoundException("The specified revision is not found");
        }
    }
}
