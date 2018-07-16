using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientRubberStamps
    {
        private AXRubberStamps rubberstamps;
        private List<AXRESTClientRubberStamp> coll;

        public AXRESTClientRubberStamps(AXRubberStamps rubberstamps)
        {
            this.rubberstamps = rubberstamps;
        }

        public int Count
        {
            get
            {
                return this.rubberstamps.Entries.Count;
            }
        }

        public List<AXRESTClientRubberStamp> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientRubberStamp>();
                    foreach (var rs in this.rubberstamps.Entries)
                    {
                        this.coll.Add(new AXRESTClientRubberStamp(rs));
                    }
                }
                return this.coll;
            }
        }
    }

    public class AXRESTClientRubberStamp
    {
        private AXRubberStamp rs;

        public AXRESTClientRubberStamp(AXRubberStamp rs)
        {
            // TODO: Complete member initialization
            this.rs = rs;
        }

        public RubberStampTypes RubberStampType
        {
            get
            {
                return this.rs.RubberStampType;
            }
        }

        public uint ID
        {
            get
            {
                return this.rs.ID;
            }
        }

        public string Name
        {
            get
            {
                return this.rs.Name;
            }
        }

        public string Description
        {
            get
            {
                return this.rs.Description;
            }
        }

        public string Content
        {
            get
            {
                return this.rs.Content;
            }
        }
    }
}
