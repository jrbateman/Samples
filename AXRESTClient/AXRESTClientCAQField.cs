using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientCAQField
    {
        private AXRESTDataModel.AXCAQField caqField;

        public AXRESTClientCAQField(AXRESTDataModel.AXCAQField caqField)
        {
            this.caqField = caqField;
        }

        public string Name
        {
            get
            {
                if (this.caqField != null)
                    return this.caqField.Name;
                else
                    throw new NullReferenceException("The AX CAQ field is not initialized");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Displayable
        {
            get
            {
                if (this.caqField != null)
                    return this.caqField.Displayable;
                else
                    throw new NullReferenceException("The AX CAQ field is not initialized");
            }
        }

        public bool Searchable
        {
            get
            {
                if (this.caqField != null)
                    return this.caqField.Searchable;
                else
                    throw new NullReferenceException("The AX CAQ field is not initialized");
            }
        }
    }
}
