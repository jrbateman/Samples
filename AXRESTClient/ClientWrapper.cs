using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Web;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTOptions
    {
        public enum AuthModes
        {
            None,
            CM,
            WindowsIntegrated,
            ADFS
        }

        public AXRESTOptions(string server, string iisappname,
            AuthModes authmode = AuthModes.None, string username = "", string password = "", string adfsToken = "",
            bool reqfulltext = false, string clientcode = "")
        {
            this.Server = new Uri(new Uri(server), string.Format("{0}/", iisappname));
            this.AuthenticationMode = authmode;
            if (this.AuthenticationMode == AuthModes.CM)
            {
                this.Username = username;
                this.Password = password;
            }
            if (this.AuthenticationMode == AuthModes.ADFS)
            {
                this.ADFSToken = adfsToken;
            }
            this.RequestFullText = reqfulltext;
            this.ClientCode = clientcode;
        }
        public Uri Server { get; set; }
        public AuthModes AuthenticationMode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RequestFullText { get; set; }
        public string ClientCode { get; set; }
        public string ADFSToken { get; set; }
    }

    public class ClientWrapper : IDisposable
    {
        public delegate void HttpRequestSentMessageHandler(string data, DateTime timestamp);
        public delegate void HttpResponseReceivedMessageHandler(string rawdata, DateTime timestamp);

        public event HttpRequestSentMessageHandler OnHttpRequestSent;
        public event HttpResponseReceivedMessageHandler OnHttpResponseReceived;

        private AXRESTOptions serverOption;
        protected AXRESTOptions ServerOption
        {
            get
            {
                return serverOption;
            }
        }
        public ClientWrapper(AXRESTOptions option)
        {
            serverOption = option;
        }

        private HttpClient client;
        protected HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = InitHttpClient();
                }
                return client;
            }
        }
        protected void InvalidateClient()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
        private HttpClient InitHttpClient()
        {
            HttpClientHandler hander = new HttpClientHandler();
            hander.UseDefaultCredentials = true;
            HttpClient clt = new HttpClient(hander);
            clt.BaseAddress = new UriBuilder(this.serverOption.Server.Scheme, this.serverOption.Server.Host).Uri;
            if (this.serverOption.AuthenticationMode == AXRESTOptions.AuthModes.CM)
            {
                var auth = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", this.serverOption.Username, this.serverOption.Password))));
                clt.DefaultRequestHeaders.Authorization = auth;
            }
            else if (this.serverOption.AuthenticationMode == AXRESTOptions.AuthModes.WindowsIntegrated)
            {
            }
            else if (this.serverOption.AuthenticationMode == AXRESTOptions.AuthModes.ADFS)
            {
                throw new NotImplementedException("to be supported");
            }

            return clt;
        }

        private Uri PreProcessURL(Uri apiURL, Dictionary<string, string> parameters)
        {
            Uri url;
            if (apiURL.IsAbsoluteUri)
            {
                //whole link ref
                url = apiURL;
            }
            else
            {
                url = new Uri(this.serverOption.Server, apiURL);
            }
            parameters = InheritFromGlobal(parameters);
            if (parameters != null && parameters.Count > 0)
            {
                NameValueCollection query = HttpUtility.ParseQueryString(url.Query);
                foreach (var kvp in parameters)
                {
                    query[kvp.Key] = kvp.Value;

                }
                string queryString = query.ToString();

                UriBuilder builder = new UriBuilder(url);
                builder.Query = queryString;
                url = builder.Uri;
            }

            return url;
        }

        private Dictionary<string, string> InheritFromGlobal(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            if (this.serverOption.RequestFullText)
            {
                parameters["requestft"] = "true";
            }
            if (!string.IsNullOrEmpty(this.serverOption.ClientCode))
            {
                parameters["productcode"] = this.serverOption.ClientCode;
            }
            return parameters;
        }

        protected async Task<string> GET(Uri apiURL, string mediatype, Dictionary<string, string> parameters = null)
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));

                Uri url = PreProcessURL(apiURL, parameters);

                Task<HttpResponseMessage> responseTask = Client.GetAsync(url);

                if (OnHttpRequestSent != null)
                {
                    string data = string.Format("{0}: {1} \r\n {2}", "GET", url.ToString(), string.Empty);
                    OnHttpRequestSent(data, DateTime.Now);
                }

                var response = await responseTask;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(result, DateTime.Now);

                    return result;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(error, DateTime.Now);
                    throw new RestException(response.RequestMessage.RequestUri.ToString(), response.StatusCode, error);
                }
            }
            catch (RestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RestException(apiURL.ToString(), ex);
            }
        }

        protected async Task<MultipartMemoryStreamProvider> GETMultipart(Uri apiURL, string mediatype, Dictionary<string, string> parameters = null)
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));

                Uri url = PreProcessURL(apiURL, parameters);

                Task<HttpResponseMessage> responseTask = Client.GetAsync(url);

                if (OnHttpRequestSent != null)
                {
                    string data = string.Format("{0}: {1} \r\n {2}", "GET", url.ToString(), string.Empty);
                    OnHttpRequestSent(data, DateTime.Now);
                }

                var response = await responseTask;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsMultipartAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived("Multipart Files Stream", DateTime.Now);

                    return result;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(error, DateTime.Now);
                    throw new RestException(response.RequestMessage.RequestUri.ToString(), response.StatusCode, error);
                }
            }
            catch (RestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RestException(apiURL.ToString(), ex);
            }
        }

        protected async Task<byte[]> GETBinary(Uri apiURL, string mediatype, Dictionary<string, string> parameters = null)
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));

                Uri url = PreProcessURL(apiURL, parameters);

                Task<HttpResponseMessage> responseTask = Client.GetAsync(url);

                if (OnHttpRequestSent != null)
                {
                    string data = string.Format("{0}: {1} \r\n {2}", "GET", url.ToString(), string.Empty);
                    OnHttpRequestSent(data, DateTime.Now);
                }

                var response = await responseTask;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsByteArrayAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived("Binary File Stream", DateTime.Now);

                    return result;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(error, DateTime.Now);
                    throw new RestException(response.RequestMessage.RequestUri.ToString(), response.StatusCode, error);
                }
            }
            catch (RestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RestException(apiURL.ToString(), ex);
            }
        }

        protected async Task<string> POST(Uri apiURL, HttpContent apiContent, string mediatype, Dictionary<string, string> parameters = null)
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));

                Uri url = PreProcessURL(apiURL, parameters);
                string contentstring = await apiContent.ReadAsStringAsync();

                Task<HttpResponseMessage> responseTask = Client.PostAsync(url, apiContent);

                if (OnHttpRequestSent != null)
                {
                    string data = string.Format("{0}: {1} \r\n {2}", "POST", url.ToString(), contentstring);
                    OnHttpRequestSent(data, DateTime.Now);
                }

                var response = await responseTask;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(result, DateTime.Now);

                    return result;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(error, DateTime.Now);
                    throw new RestException(response.RequestMessage.RequestUri.ToString(), response.StatusCode, error);
                }
            }
            catch (RestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RestException(apiURL.ToString(), ex);
            }
        }

        protected async Task<string> PUT(Uri apiURL, HttpContent apiContent, string mediatype, Dictionary<string, string> parameters = null)
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));

                Uri url = PreProcessURL(apiURL, parameters);
                string contentstring = await apiContent.ReadAsStringAsync();

                Task<HttpResponseMessage> responseTask = Client.PutAsync(url, apiContent);

                if (OnHttpRequestSent != null)
                {
                    string data = string.Format("{0}: {1} \r\n {2}", "PUT", url.ToString(), contentstring);
                    OnHttpRequestSent(data, DateTime.Now);
                }

                var response = await responseTask;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(result, DateTime.Now);

                    return result;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(error, DateTime.Now);
                    throw new RestException(response.RequestMessage.RequestUri.ToString(), response.StatusCode, error);
                }
            }
            catch (RestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RestException(apiURL.ToString(), ex);
            }
        }

        protected async Task<string> DELETE(Uri apiURL, string mediatype, Dictionary<string, string> parameters = null)
        {
            try
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));

                Uri url = PreProcessURL(apiURL, parameters);

                Task<HttpResponseMessage> responseTask = Client.DeleteAsync(url);

                if (OnHttpRequestSent != null)
                {
                    string data = string.Format("{0}: {1} \r\n {2}", "DELETE", url.ToString(), string.Empty);
                    OnHttpRequestSent(data, DateTime.Now);
                }

                var response = await responseTask;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(result, DateTime.Now);

                    return result;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();

                    if (OnHttpResponseReceived != null)
                        OnHttpResponseReceived(error, DateTime.Now);
                    throw new RestException(response.RequestMessage.RequestUri.ToString(), response.StatusCode, error);
                }
            }
            catch (RestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RestException(apiURL.ToString(), ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Client != null)
                    Client.Dispose();
            }
        }
        ~ClientWrapper()
        {
            Dispose(false);
        }
    }
}
