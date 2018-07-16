using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientQueries : ClientWrapper
    {
        private AXRESTDataModel.AXQueryCollection queries;

        public int Count
        {
            get
            {
                if (this.queries != null)
                    return this.queries.Entries.Count;
                else
                    throw new NullReferenceException("The AX query list is not initialized");
            }
        }

        public List<AXRESTClientQuery> Collection
        {
            get
            {
                if (this.queries != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientQuery>();
                        foreach (var q in this.queries.Entries)
                        {
                            coll.Add(new AXRESTClientQuery(q, ServerOption));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX query list is not initialized");
            }
        }
        private List<AXRESTClientQuery> coll;

        public AXRESTClientQueries(AXRESTDataModel.AXQueryCollection Queries, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.queries = Queries;
        }
    }
}
