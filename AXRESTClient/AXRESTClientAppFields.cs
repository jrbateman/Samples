using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientAppFields : ClientWrapper
    {
        private AXRESTDataModel.AXAppFields appfields;

        public AXRESTClientAppFields(AXRESTDataModel.AXAppFields appfields, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.appfields = appfields;
        }

        public int Count
        {
            get
            {
                if (this.appfields != null)
                    return this.appfields.Entries.Count;
                else
                    throw new NullReferenceException("The AX application fields list is not initialized");
            }
        }

        public List<AXRESTClientAppField> Collection
        {
            get
            {
                if (this.appfields != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientAppField>();
                        foreach (var f in this.appfields.Entries)
                        {
                            coll.Add(new AXRESTClientAppField(f, ServerOption));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX application fields list is not initialized");
            }
        }
        private List<AXRESTClientAppField> coll;
    }
}
