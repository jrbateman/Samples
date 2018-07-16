using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientAppField : ClientWrapper
    {
        private AXRESTDataModel.AXAppField field;

        public AXRESTClientAppField(AXRESTDataModel.AXAppField fd, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.field = fd;
        }

        public string ID
        {
            get
            {
                if (this.field != null)
                    return this.field.ID;
                else
                    throw new NullReferenceException("The AX application field is not initialized");
            }
        }

        public string Name
        {
            get
            {
                if (this.field != null)
                    return this.field.Name;
                else
                    throw new NullReferenceException("The AX application field is not initialized");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string EditMask
        {
            get
            {
                if (this.field != null)
                    return this.field.EditMask;
                else
                    throw new NullReferenceException("The AX application field is not initialized");
            }
        }

        public string ValidationRegex
        {
            get
            {
                if (this.field != null)
                    return this.field.ValidationRegex;
                else
                    throw new NullReferenceException("The AX application field is not initialized");
            }
        }


        public string QueryValidationRegex
        {
            get
            {
                if (this.field != null)
                    return this.field.QueryValidationRegex;
                else
                    throw new NullReferenceException("The AX application field is not initialized");
            }
        }

        public int FieldLength
        {
            get
            {
                if (this.field != null)
                    return this.field.FieldLength;
                else
                    throw new NullReferenceException("The AX application field is not initialized");
            }
        }

        public List<string> Attributes
        {
            get
            {
                if (this.field != null)
                {
                    List<string> attributes = new List<string>();
                    foreach (var attr in this.field.Attributes)
                    {
                        if (attr.Value) attributes.Add(attr.Key.ToString());
                    }
                    return attributes;
                }
                else
                    throw new NullReferenceException("The AX application attributes definitions is not initialized");
            }
        }

        public async Task<AXRESTClientDataType> GetDataTypeAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.field.Links[AXRESTLinkRelations.AXDataType].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDataType dt = AXRESTDataModelConvert.DeserializeObject<AXDataType>(apiResult, mediatype);
                return new AXRESTClientDataType(dt, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientDataFormat> GetDataFormatAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (!this.field.Links.ContainsKey(AXRESTLinkRelations.AXDataFormat))
                return null;

            var apiURL = new Uri(this.field.Links[AXRESTLinkRelations.AXDataFormat].HRef, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDataFormat dformat = AXRESTDataModelConvert.DeserializeObject<AXDataFormat>(apiResult, mediatype);
                return new AXRESTClientDataFormat(dformat, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientAppField> Refresh(string mediatype)
        {
            if (string.IsNullOrEmpty(this.field.Self))
                return null;

            var apiURL = new Uri(this.field.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.field = AXRESTDataModelConvert.DeserializeObject<AXAppField>(apiResult, mediatype);
                return this;
            }
            finally
            {
            }
        }
    }
}
