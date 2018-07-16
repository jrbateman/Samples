using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientBatches : ClientWrapper
    {
        private AXRESTDataModel.AXBatchCollection batches;

        public int Count
        {
            get
            {
                if (this.batches != null)
                    return this.batches.Entries.Count;
                else
                    throw new NullReferenceException("The AX batch list is not initialized");
            }
        }
        public List<AXRESTClientBatch> Collection
        {
            get
            {
                if (this.batches != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientBatch>();
                        foreach (var batch in this.batches.Entries)
                        {
                            coll.Add(new AXRESTClientBatch(batch, ServerOption));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX batch list is not initialized");
            }
        }
        private List<AXRESTClientBatch> coll;

        public AXRESTClientBatches(AXRESTDataModel.AXBatchCollection Batches, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.batches = Batches;
        }
    }
}
