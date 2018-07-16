using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using XtenderSolutions.AXRESTClient;

namespace AXRESTTestConsole.UserControls
{
    public class BaseUserControl : UserControl
    {
        public BaseUserControl()
        {
        }

        public HttpActions HttpAction
        {
            get
            {
                TabControl tab = (TabControl)this.FindName("Tabs");
                TabItem tabItem = (TabItem)tab.SelectedItem;
                HttpActions action;
                Enum.TryParse<HttpActions>(tabItem.Header.ToString(), true, out action);
                return action;
            }
        }

        public virtual async Task Get()
        {
            await Task.Run(() => { });
        }
        public virtual async Task Post()
        {
            await Task.Run(() => { });
        }
        public virtual async Task Put()
        {
            await Task.Run(() => { });
        }
        public virtual async Task Delete()
        {
            await Task.Run(() => { });
        }

        private DateTime start;
        private DateTime end;

        public string TimeStart { get; set; }
        public string TimeCost { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

        public void OnHttpRequestSent(string data, DateTime timestamp)
        {
            Request = data;
            start = timestamp;
            TimeStart = start.ToString();
        }

        public void OnHttpResponseReceived(string data, DateTime timestamp)
        {
            if (Global.XMLMediaType)
            {
                data = FormatXML(data);
            }
            else
            {
                try
                {
                    JObject json = JObject.Parse(data);
                    data = json.ToString();
                }
                catch (Exception ex)
                {
                    //ignore
                }
            }

            Response = data;
            end = timestamp;
            TimeCost = (end - start).Milliseconds.ToString();
        }

        internal void RegisterClientEvents(ClientWrapper client)
        {
            client.OnHttpRequestSent += OnHttpRequestSent;
            client.OnHttpResponseReceived += OnHttpResponseReceived;
        }

        internal void UnregisterClientEvents(ClientWrapper client)
        {
            client.OnHttpRequestSent -= OnHttpRequestSent;
            client.OnHttpResponseReceived -= OnHttpResponseReceived;
        }

        internal void UpdateRequestInfo()
        {
            MainWindow win = App.Current.MainWindow as MainWindow;
            win.tbRequestStart.Text = TimeStart;
            win.tbRequestTime.Text = TimeCost;

            win.tbRequestContent.Text = Request;
            win.tbResponseContent.Text = Response;
        }

        public string FormatXML(String XML)
        {
            string result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(XML);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                string FormattedXML = sReader.ReadToEnd();

                result = FormattedXML;
            }
            catch (XmlException)
            {
            }
            return result;
        }
    }

    public enum HttpActions
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
