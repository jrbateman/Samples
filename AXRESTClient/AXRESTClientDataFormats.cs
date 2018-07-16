using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDataFormats : ClientWrapper
    {
        private AXRESTDataModel.AXDataFormatCollection dataFormats;

        public int Count
        {
            get
            {
                if (this.dataFormats != null)
                    return this.dataFormats.Entries.Count;
                else
                    throw new NullReferenceException("The AX data format list is not initialized");
            }
        }

        public List<AXRESTClientDataFormat> Collection
        {
            get
            {
                if (this.dataFormats != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientDataFormat>();
                        foreach (var df in this.dataFormats.Entries)
                        {
                            coll.Add(new AXRESTClientDataFormat(df, ServerOption));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX data format list is not initialized");
            }
        }

        private List<AXRESTClientDataFormat> coll;

        public AXRESTClientDataFormats(AXRESTDataModel.AXDataFormatCollection Dataformats, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.dataFormats = Dataformats;
        }
    }
}
