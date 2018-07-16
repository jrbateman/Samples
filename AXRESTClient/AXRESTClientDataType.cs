using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDataType : ClientWrapper
    {
        private AXRESTDataModel.AXDataType dataType;

        public int ID
        {
            get
            {
                if (this.dataType != null)
                    return this.dataType.ID;
                else
                    throw new NullReferenceException("The AX data type is not initialized");
            }
        }
        public string Name
        {
            get
            {
                if (this.dataType != null)
                    return this.dataType.Name;
                else
                    throw new NullReferenceException("The AX data type is not initialized");
            }
        }
        public bool IsCustom
        {
            get
            {
                if (this.dataType != null)
                    return this.dataType.IsCustom;
                else
                    throw new NullReferenceException("The AX data type is not initialized");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", ID, Name);
        }

        public AXRESTClientDataType(AXRESTDataModel.AXDataType DataType, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.dataType = DataType;
        }

        public async Task<AXRESTClientDataFormats> GetDataFormatsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            //there are data types do not have data formats, like "Text"
            if (!this.dataType.Links.ContainsKey(AXRESTLinkRelations.AXDataFormats))
                return null;

            var apiURL = new Uri(this.dataType.Links[AXRESTLinkRelations.AXDataFormats].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDataFormatCollection dataformats = AXRESTDataModelConvert.DeserializeObject<AXDataFormatCollection>(apiResult, mediatype);
                return new AXRESTClientDataFormats(dataformats, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDataType> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.dataType.Self))
                return null;

            var apiURL = new Uri(this.dataType.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.dataType = AXRESTDataModelConvert.DeserializeObject<AXDataType>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
