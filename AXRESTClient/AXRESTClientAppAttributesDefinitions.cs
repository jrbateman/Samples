using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientAppAttributesDefinitions
    {
        private AXRESTDataModel.AXAppAttributesDefinitions appAttributesDef;

        public int Count
        {
            get
            {
                if (this.appAttributesDef != null)
                    return this.appAttributesDef.Count;
                else
                    throw new NullReferenceException("The AX application attributes definitions is not initialized");
            }
        }

        public Dictionary<string, string> AllAttributes
        {
            get
            {
                if (this.appAttributesDef != null)
                {
                    return this.appAttributesDef;
                }
                else
                    throw new NullReferenceException("The AX application attributes definitions is not initialized");
            }
        }

        public AXRESTClientAppAttributesDefinitions(AXRESTDataModel.AXAppAttributesDefinitions AppAttributesDef)
        {
            this.appAttributesDef = AppAttributesDef;
        }
    }
}
