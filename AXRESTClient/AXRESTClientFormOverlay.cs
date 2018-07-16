using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientFormOverlays
    {
        private AXFormOverlays formoverlays;
        private List<AXRESTClientFormOverlay> coll;
        public AXRESTClientFormOverlays(AXFormOverlays formoverlays)
        {
            this.formoverlays = formoverlays;
        }

        public int Count
        {
            get
            {
                return this.formoverlays.Entries.Count;
            }
        }

        public List<AXRESTClientFormOverlay> Collection
        {
            get
            {
                if (this.coll == null)
                {
                    this.coll = new List<AXRESTClientFormOverlay>();
                    foreach (var fo in this.formoverlays.Entries)
                    {
                        this.coll.Add(new AXRESTClientFormOverlay(fo));
                    }
                }
                return this.coll;
            }
        }
    }

    public class AXRESTClientFormOverlay
    {
        private AXFormOverlay fo;

        public AXRESTClientFormOverlay(AXFormOverlay fo)
        {
            this.fo = fo;
        }

        public FormTypes FormType
        {
            get
            {
                return this.fo.FormType;
            }
        }

        public float RatioX
        {
            get
            {
                return this.fo.RatioX;
            }
        }

        public float RatioY
        {
            get
            {
                return this.fo.RatioY;
            }
        }

        public float Top
        {
            get
            {
                return this.fo.Top;
            }
        }

        public float Left
        {
            get
            {
                return this.fo.Left;
            }
        }

        public float LPI
        {
            get
            {
                return this.fo.LPI;
            }
        }

        public float CPI
        {
            get
            {
                return this.fo.CPI;
            }
        }

        public FormUnits Unit
        {
            get
            {
                return this.fo.Unit;
            }
        }

        public FormOrientations Orientation
        {
            get
            {
                return this.fo.Orientation;
            }
        }

        public string Name
        {
            get
            {
                return this.fo.Name;
            }
        }

        public uint ID
        {
            get
            {
                return this.fo.ID;
            }
        }

        public string Content
        {
            get
            {
                return this.fo.Content;
            }
        }
    }
}
