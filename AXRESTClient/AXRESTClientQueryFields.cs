using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientQueryFields
    {
        private LinkedResourceCollection<AXQueryField> queryFields;

        public AXRESTClientQueryFields(LinkedResourceCollection<AXQueryField> queryFields)
        {
            this.queryFields = queryFields;
        }


        public int Count
        {
            get
            {
                if (this.queryFields != null)
                    return this.queryFields.Entries.Count;
                else
                    throw new NullReferenceException("The AX query fields list is not initialized");
            }
        }

        private List<AXRESTClientQueryField> coll;
        public List<AXRESTClientQueryField> Collection
        {
            get
            {
                if (this.queryFields != null)
                {
                    if (coll == null)
                    {
                        coll = new List<AXRESTClientQueryField>();
                        foreach (var qf in this.queryFields.Entries)
                        {
                            coll.Add(new AXRESTClientQueryField(qf));
                        }
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX query fields list is not initialized");
            }
        }
    }
}
