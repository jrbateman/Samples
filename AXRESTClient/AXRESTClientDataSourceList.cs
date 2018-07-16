using XtenderSolutions.AXRESTDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDataSourceList : ClientWrapper
    {
        private AXRESTDataModel.AXDataSources dataSources;

        public int Count
        {
            get
            {
                if (this.dataSources != null)
                    return this.dataSources.Entries.Count;
                else
                    throw new NullReferenceException("The AX data source list is not initialized");
            }
        }

        public Dictionary<string, bool> Collection
        {
            get
            {
                if (this.dataSources != null)
                {
                    Dictionary<string, bool> coll = new Dictionary<string, bool>();
                    foreach (var ds in this.dataSources.Entries)
                    {
                        coll[ds.Name] = ds.IsDefault;
                    }
                    return coll;
                }
                else
                    throw new NullReferenceException("The AX data source list is not initialized");
            }
        }

        public AXRESTClientDataSourceList(AXRESTDataModel.AXDataSources DataSources, AXRESTOptions parentOption)
            :base(parentOption)
        {
            this.dataSources = DataSources;
        }

        public async Task<AXRESTClientDataSource> LoginDataSourceAsync(string datasource, AXRESTOptions.AuthModes mode, string username = "", string password = "", bool requestFullText = false, string clientCode = "", string mediatype = AXRESTMediaTypes.JSON)
        {
            bool found = false;
            Uri apiURL = null;
            foreach(var ds in this.dataSources.Entries)
            {
                if(ds.Name == datasource)
                {
                    found = true;
                    apiURL = new Uri(ds.Self, UriKind.Relative);
                    break;
                }
            }

            if (!found) throw new ArgumentException("The specified data source cannot be found");

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
