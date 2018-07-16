using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientKeyReferenceLookupResults
    {
        private AXRESTDataModel.AXKeyRefIndex keyRefIndex;

        public AXRESTClientKeyReferenceLookupResults(AXRESTDataModel.AXKeyRefIndex keyRefIndex)
        {
            this.keyRefIndex = keyRefIndex;
        }

        public Dictionary<string, string> IndexValues
        {
            get 
            {
                if (this.keyRefIndex != null)
                {
                    Dictionary<string, string> ret = new Dictionary<string, string>();
                    foreach(var f in this.keyRefIndex.IndexValues)
                    {
                        ret[f.FieldID] = f.FieldValue;
                    }
                    return ret;
                }
                else
                    throw new NullReferenceException("The AX data source is not initialized");
            }
        }
    }
}
