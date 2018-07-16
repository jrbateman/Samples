using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClient : ClientWrapper
    {
        public AXRESTClient(string server, string appname) :
            base(new AXRESTOptions(server, appname))
        {
        }

        public async Task<AXRESTClientHomeDocument> GetHomeDocumentAsync(string mediatype = AXRESTMediaTypes.HOMEJSON)
        {
            try
            {
                string apiResult = await GET(new Uri("api/service", UriKind.Relative), mediatype);
                if (mediatype.Contains("json"))
                {
                    HomeDocumentJson homedocjson = AXRESTDataModelConvert.DeserializeObject<HomeDocumentJson>(apiResult, mediatype);
                    return new AXRESTClientHomeDocument(homedocjson, ServerOption);
                }
                else 
                {
                    HomeDocumentXML homedocxml = AXRESTDataModelConvert.DeserializeObject<HomeDocumentXML>(apiResult, mediatype);
                    return new AXRESTClientHomeDocument(homedocxml, ServerOption);
                }

            }
            finally
            {
            }
        }
    }

}
