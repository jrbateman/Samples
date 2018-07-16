using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocIndex : ClientWrapper
    {
        private AXRESTDataModel.AXDocIndex index;

        public AXRESTClientDocIndex(AXRESTDataModel.AXDocIndex i, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.index = i;
        }

        public int IndexId
        {
            get
            {
                if (this.index != null)
                    return this.index.IndexID;
                else
                    throw new NullReferenceException("The AXDocIndex is not initialized");
            }
        }

        public Dictionary<string, string> IndexValues
        {
            get
            {
                if (this.index != null)
                {
                    Dictionary<string, string> ret = new Dictionary<string,string>();
                    foreach(var indexvalue in this.index.IndexValues)
                    {
                        ret.Add(indexvalue.FieldID, indexvalue.FieldValue);
                    }
                    return ret;
                }
                else
                    throw new NullReferenceException("The AXDocIndex is not initialized");
            }
        }

        public async Task<AXDocIndex> Refresh(string mediatype = AXRESTMediaTypes.JSON)
        {
            if (string.IsNullOrEmpty(this.index.Self))
                return null;

            var apiURL = new Uri(this.index.Self, UriKind.Relative);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                this.index = AXRESTDataModelConvert.DeserializeObject<AXDocIndex>(apiResult, mediatype);
                return index;
            }
            finally
            {
            }
        }

        public async Task DeleteAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.index.Self, UriKind.Relative);

            try
            {
                string apiResult = await DELETE(apiURL, mediatype);
                this.index = null;
                this.Dispose();
            }
            finally
            {
            }
        }

        public async Task UpdateAsync(Dictionary<string, string> indexvalues, bool failIfMatchIndex = false, bool failIfDLSViolation = false, string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.index.Self, UriKind.Relative);
            try
            {
                AXDocIndex newIndex = new AXDocIndex();
                List<AXDocIndexValue> temp = new List<AXDocIndexValue>();
                foreach(var kvp in indexvalues)
                {
                    AXDocIndexValue value = new AXDocIndexValue();
                    value.FieldID = kvp.Key;
                    value.FieldValue = kvp.Value;
                    temp.Add(value);
                }
                newIndex.IndexValues = temp.ToArray();


                string tempContent = AXRESTDataModelConvert.SerializeObject(
                    newIndex, mediatype);
                StringContent apiContent = new StringContent(tempContent, Encoding.UTF8, mediatype);
                
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("FailIfMatchIndex", failIfMatchIndex.ToString());
                paras.Add("FailIfDLSViolation", failIfDLSViolation.ToString());
                string apiResult = await PUT(apiURL, apiContent, mediatype, paras);
                this.index = AXRESTDataModelConvert.DeserializeObject<AXDocIndex>(apiResult, mediatype);
            }
            finally
            {
            }
        }
    }
}
