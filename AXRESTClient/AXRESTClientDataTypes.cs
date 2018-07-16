using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDataTypes : ClientWrapper
    {
        private AXRESTDataModel.AXDataTypeCollection datatypes;

        public int Count
        {
            get
            {
                if (this.datatypes != null)
                    return this.datatypes.Entries.Count;
                else
                    throw new NullReferenceException("The AX data type list is not initialized");
            }
        }

        public List<AXRESTClientDataType> Collection
        {
            get
            {
                if (this.datatypes != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientDataType>();

                        foreach (var dt in this.datatypes.Entries)
                        {
                            coll.Add(new AXRESTClientDataType(dt, ServerOption));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX data type list is not initialized");
            }
        }

        private List<AXRESTClientDataType> coll;

        public AXRESTClientDataTypes(AXRESTDataModel.AXDataTypeCollection datatypes, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.datatypes = datatypes;
        }
    }
}
