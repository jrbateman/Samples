using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientPermissionDefinitions
    {
        private AXRESTDataModel.AXPermissionDescriptions pds;

        public int Count
        {
            get
            {
                if (this.pds != null)
                    return this.pds.Count;
                else
                    throw new NullReferenceException("The AX permissions definitions is not initialized");
            }
        }

        public Dictionary<string, string> AllPermissions
        {
            get
            {
                if (this.pds != null)
                {
                    return this.pds;
                }
                else
                    throw new NullReferenceException("The AX permissions definitions is not initialized");
            }
        }

        public AXRESTClientPermissionDefinitions(AXRESTDataModel.AXPermissionDescriptions perds)
        {
            this.pds = perds;
        }
    }
}
