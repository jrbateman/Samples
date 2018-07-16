using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XtenderSolutions.AXRESTClient;

namespace AXRESTTestConsole.UserControls
{
    /// <summary>
    /// Interaction logic for Rendition.xaml
    /// </summary>
    public partial class Rendition : BaseUserControl
    {
        public Rendition()
        {
            InitializeComponent();
        }

        public int SourceType { get; set; }

        public AXRESTClientBatchPage CurrentBatchPage { get; set; }

        public AXRESTClientDocPageVersion CurrentDocPageVersion { get; set; }

        public AXRESTClientReportDocPage CurrentReportDocPage { get; set; }

        /// <summary>
        ///     Set the current source to be rendered
        /// </summary>
        /// <param name="sourcetype">0 doc page version, 1 batch page 2 report doc page</param>
        /// <param name="source"></param>
        public void SetSource(int sourcetype, object source)
        {
            this.SourceType = sourcetype;

            PopulateUI(sourcetype);

            if (sourcetype == 0)
            {
                CurrentDocPageVersion = source as AXRESTClientDocPageVersion;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentDocPageVersion.Location);
                return;
            }

            if (sourcetype == 1)
            {
                CurrentBatchPage = source as AXRESTClientBatchPage;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentBatchPage.Location);
                return;
            }

            if (sourcetype == 2)
            {
                CurrentReportDocPage = source as AXRESTClientReportDocPage;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentReportDocPage.Location);
                return;
            }
        }

        //image/jpg
        //image/gif
        //application/pdf
        //application/xps
        //text/html
        //image/raw
        //image/annobinary
        //image/anno
        //mage/ocr
        private void PopulateUI(int sourcetype)
        {
            if (sourcetype == 0)
            {
                this.tbSubPage.IsEnabled = true;
                this.cbFormOverlayOptions.IsEnabled = true;
                this.cbAnnoRedactionOptions.IsEnabled = true;
                this.cbClientProfile.IsEnabled = true;
                this.cbMediaTypes.ItemsSource = new string[] {"image/jpg",
                                                              "image/gif",
                                                              "application/pdf",
                                                              "application/xps",
                                                              "text/html",
                                                              "image/raw",
                                                              "image/annobinary",
                                                              "image/anno",
                                                              "image/ocr" };
                return;
            }

            if (sourcetype == 1)
            {
                this.tbSubPage.IsEnabled = true;
                this.cbFormOverlayOptions.IsEnabled = false;
                this.cbAnnoRedactionOptions.IsEnabled = true;
                this.cbClientProfile.IsEnabled = true;
                this.cbMediaTypes.ItemsSource = new string[] {"image/jpg",
                                                              "image/gif",
                                                              "application/pdf",
                                                              "application/xps",
                                                              "text/html",
                                                              "image/raw",
                                                              "image/annobinary",
                                                              "image/anno",
                                                              "image/ocr" };
                return;
            }

            if (sourcetype == 2)
            {
                this.tbSubPage.IsEnabled = false;
                this.cbFormOverlayOptions.IsEnabled = false;
                this.cbAnnoRedactionOptions.IsEnabled = true;
                this.cbClientProfile.IsEnabled = true;
                this.cbMediaTypes.ItemsSource = new string[] {"image/jpg",
                                                              "image/gif",
                                                              "application/pdf",
                                                              "application/xps",
                                                              "text/html",
                                                              "image/raw"};
                return;
            }
        }

        private string GetExtension()
        {
            switch(this.cbMediaTypes.SelectionBoxItem.ToString())
            {
                case "image/jpg":
                    return ".jpg";
                case "image/gif":
                    return ".gif";
                case "application/pdf":
                    return ".pdf";
                case "application/xps":
                    return ".xps";
                case "text/html":
                    return ".html";

                default:
                    return ".bin";
            }
        }

        public override async Task Get()
        {
            AXRESTClientFile result = null;
            string fileName = Guid.NewGuid().ToString() + GetExtension();

            if (SourceType == 0)
            {
                AXRESTClientDocPageVersion client = this.CurrentDocPageVersion;

                RegisterClientEvents(client);
                result = await client.RenderAsync(fileName, this.cbMediaTypes.SelectionBoxItem.ToString(),
                    Convert.ToInt32(this.tbSubPage.Text), this.cbFormOverlayOptions.SelectedIndex, 
                    this.cbAnnoRedactionOptions.SelectedIndex, this.cbClientProfile.SelectedIndex);
                UnregisterClientEvents(client);

            }

            if (SourceType == 1)
            {
                AXRESTClientBatchPage client = this.CurrentBatchPage;

                RegisterClientEvents(client);
                result = await client.RenderAsync(fileName, this.cbMediaTypes.SelectionBoxItem.ToString(), 
                    Convert.ToInt32(this.tbSubPage.Text), this.cbAnnoRedactionOptions.SelectedIndex, 
                    this.cbClientProfile.SelectedIndex);
                UnregisterClientEvents(client);

            }

            if (SourceType == 2)
            {
                AXRESTClientReportDocPage client = this.CurrentReportDocPage;

                RegisterClientEvents(client);
                result = await client.RenderAsync(fileName, this.cbMediaTypes.SelectionBoxItem.ToString(), 
                    this.cbAnnoRedactionOptions.SelectedIndex, this.cbClientProfile.SelectedIndex);
                UnregisterClientEvents(client);

            }

            PopulateResultUI(result);
        }


        private void PopulateResultUI(AXRESTClientFile result)
        {
            this.lbRenderResults.Items.Clear();

            string fullname = System.IO.Path.GetTempPath() + result.FileName;
            result.SaveToLocal(fullname);
            this.lbRenderResults.Items.Add(fullname);
        }

        private void lbRenderResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string fullname = this.lbRenderResults.SelectedValue.ToString();

            System.Diagnostics.Process.Start(fullname);
        }
    }
}
