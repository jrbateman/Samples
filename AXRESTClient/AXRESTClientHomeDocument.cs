using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientHomeDocument : ClientWrapper
    {
        private AXRESTDataModel.HomeDocument homeDoc;

        public AXRESTClientHomeDocument(AXRESTDataModel.HomeDocumentJson HomeDoc, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.homeDoc = HomeDoc;
        }

        public AXRESTClientHomeDocument(AXRESTDataModel.HomeDocumentXML HomeDoc, AXRESTOptions parentOption)
            : base(parentOption)
        {
            this.homeDoc = HomeDoc;
        }

        public async Task<AXRESTClientDataSourceList> GetDataSourceListAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.homeDoc.ResourcesLinks[AXRESTLinkRelations.AXDataSources], UriKind.RelativeOrAbsolute);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXDataSources dataSources = AXRESTDataModelConvert.DeserializeObject<AXDataSources>(apiResult, mediatype);
                return new AXRESTClientDataSourceList(dataSources, ServerOption);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientPermissionDefinitions> GetAXPermissionDefinitionsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.homeDoc.ResourcesLinks[AXRESTLinkRelations.AXPermissionsDef], UriKind.RelativeOrAbsolute);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXPermissionDescriptions pds = AXRESTDataModelConvert.DeserializeObject<AXPermissionDescriptions>(apiResult, mediatype);
                return new AXRESTClientPermissionDefinitions(pds);
            }
            finally
            {
            }
        }

        public async Task<AXRESTClientAppAttributesDefinitions> GetAXAppAttributesDefinitionsAsync(string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(this.homeDoc.ResourcesLinks[AXRESTLinkRelations.AXAppAttributesDef], UriKind.RelativeOrAbsolute);

            try
            {
                string apiResult = await GET(apiURL, mediatype);
                AXAppAttributesDefinitions appAttributesDef = AXRESTDataModelConvert.DeserializeObject<AXAppAttributesDefinitions>(apiResult, mediatype);
                return new AXRESTClientAppAttributesDefinitions(appAttributesDef);
            }
            finally
            {
            }
        }

        //In case of single data source configured. The client cannot access the data source list
        public async Task<AXRESTClientDataSource> LoginDataSourceAsync(string datasource, AXRESTOptions.AuthModes mode, string username = "", string password = "", bool requestFullText = false, string clientCode = "", string mediatype = AXRESTMediaTypes.JSON)
        {
            var apiURL = new Uri(string.Format("/api/AXDataSources/{0}", datasource), UriKind.Relative);
            try
            {
                ServerOption.AuthenticationMode = mode;
                ServerOption.Username = username;
                ServerOption.Password = password;
                ServerOption.RequestFullText = requestFullText;
                ServerOption.ClientCode = clientCode;
                InvalidateClient();
                string apiResult = await GET(apiURL, mediatype);
                AXDataSource dataSource = AXRESTDataModelConvert.DeserializeObject<AXDataSource>(apiResult, mediatype);
                return new AXRESTClientDataSource(dataSource, ServerOption);
            }
            finally
            {
            }
        }
    }
}
