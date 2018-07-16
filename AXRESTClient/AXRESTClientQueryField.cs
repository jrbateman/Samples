using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientQueryField
    {
        private AXRESTDataModel.AXQueryField queryField;

        public AXRESTClientQueryField(AXRESTDataModel.AXQueryField qf)
        {
            this.queryField = qf;
        }

        public string Name
        {
            get
            {
                if (this.queryField != null)
                    return this.queryField.Name;
                else
                    throw new NullReferenceException("The AX query field is not initialized");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string SearchName
        {
            get
            {
                if (this.queryField != null)
                    return this.queryField.SearchName;
                else
                    throw new NullReferenceException("The AX query field is not initialized");
            }
        }

        public string QueryValue
        {
            get
            {
                if (this.queryField != null)
                    return this.queryField.QueryValue;
                else
                    throw new NullReferenceException("The AX query field is not initialized");
            }
        }
        public Dictionary<string, bool> FieldAttributes
        {
            get
            {
                if (this.queryField != null)
                    return this.queryField.FieldAttributes;
                else
                    throw new NullReferenceException("The AX query field is not initialized");
            }
        }

        public int FieldLength
        {
            get
            {
                if (this.queryField != null)
                    return this.queryField.FieldLength;
                else
                    throw new NullReferenceException("The AX query field is not initialized");
            }
        }
    }
}
