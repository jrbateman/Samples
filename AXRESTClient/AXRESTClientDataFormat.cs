using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDataFormat : ClientWrapper
    {
        private AXRESTDataModel.AXDataFormat dataFormat;

        public int ID
        {
            get
            {
                if (this.dataFormat != null)
                    return this.dataFormat.ID;
                else
                    throw new NullReferenceException("The AX data format is not initialized");
            }
        }
        public string Name
        {
            get
            {
                if (this.dataFormat != null)
                    return this.dataFormat.Name;
                else
                    throw new NullReferenceException("The AX data format is not initialized");
            }
        }
        public int FormatWidth
        {
            get
            {
                if (this.dataFormat != null)
                    return this.dataFormat.FormatWidth;
                else
                    throw new NullReferenceException("The AX data format is not initialized");
            }
        }
        public bool IsCustom
        {
            get
            {
                if (this.dataFormat != null)
                    return this.dataFormat.IsCustom;
                else
                    throw new NullReferenceException("The AX data format is not initialized");
            }
        }
        public override string ToString()
        {
            return string.Format("{0}-{1}", ID, Name);
        }
        public AXRESTClientDataFormat(AXRESTDataModel.AXDataFormat DataFormat, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.dataFormat = DataFormat;
        }


        public async Task<AXRESTClientDataFormat> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.dataFormat.Self))
                return null;

            var apiURL = new Uri(this.dataFormat.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.dataFormat = AXRESTDataModelConvert.DeserializeObject<AXDataFormat>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
