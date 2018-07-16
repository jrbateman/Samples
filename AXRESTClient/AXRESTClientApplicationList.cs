using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientApplicationList : ClientWrapper
    {
        private AXRESTDataModel.AXApps appList;

        public int Count
        {
            get
            {
                if (this.appList != null)
                    return this.appList.Entries.Count;
                else
                    throw new NullReferenceException("The AX application list is not initialized");
            }
        }
        public List<AXRESTClientApplication> Collection 
        {
            get
            {
                if (this.appList != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientApplication>();
                        foreach (var app in this.appList.Entries)
                        {
                            coll.Add(new AXRESTClientApplication(app, ServerOption));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX application list is not initialized");
            }
        }
        private List<AXRESTClientApplication> coll;

        public AXRESTClientApplicationList(AXRESTDataModel.AXApps AppList, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.appList = AppList;
        }
    }
}
