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
    /// Interaction logic for Thumbnails.xaml
    /// </summary>
    public partial class Thumbnails : BaseUserControl
    {
        public Thumbnails()
        {
            InitializeComponent();
        }
        public int SourceType { get; set; }

        public AXRESTClientBatch CurrentBatch { get; set; }

        public AXRESTClientDoc CurrentDoc { get; set; }

        public AXRESTClientBatchPage CurrentBatchPage { get; set; }

        public AXRESTClientDocPageVersion CurrentDocPageVersion { get; set; }


        /// <summary>
        ///     Set the current source to be rendered
        /// </summary>
        /// <param name="sourcetype">0 doc , 1 batch, 2 docpageversion, 3 batchpage</param>
        /// <param name="source"></param>
        public void SetSource(int sourcetype, object source)
        {
            this.SourceType = sourcetype;

            if (sourcetype == 0)
            {
                CurrentDoc = source as AXRESTClientDoc;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentDoc.Location);
                return;
            }

            if (sourcetype == 1)
            {
                CurrentBatch = source as AXRESTClientBatch;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentBatch.Location);
                return;
            }

            if (sourcetype == 2)
            {
                CurrentDocPageVersion = source as AXRESTClientDocPageVersion;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentDocPageVersion.Location);
                return;
            }

            if (sourcetype == 3)
            {
                CurrentBatchPage = source as AXRESTClientBatchPage;

                this.tbCurrent.Text = string.Format("Current: {0}", CurrentBatchPage.Location);
                return;
            }

            
        }

        public override async Task Get()
        {
            List<AXRESTClientFile> results = null;
            
            if (SourceType == 0)
            {
                AXRESTClientDoc client = this.CurrentDoc;
                int pagestart;
                int pageend;
                int width;
                int height ;
                try
                {
                    pagestart = Convert.ToInt32(this.tbPageStart.Text);
                    pageend = Convert.ToInt32(this.tbPageEnd.Text);
                    width = Convert.ToInt32(this.tbThumbnailWidth.Text);
                    height = Convert.ToInt32(this.tbThumbnailHeight.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                pagestart = pagestart < 1 ? 1 : pagestart;
                pageend = pageend > client.PageCount ? client.PageCount : pageend;

                RegisterClientEvents(client);
                results = await client.GetThumbnailsAsync(pagestart, pageend, width, height);
                UnregisterClientEvents(client);

            }

            if (SourceType == 1)
            {
                AXRESTClientBatch client = this.CurrentBatch;
                int pagestart;
                int pageend;
                int width;
                int height;
                try
                {
                    pagestart = Convert.ToInt32(this.tbPageStart.Text);
                    pageend = Convert.ToInt32(this.tbPageEnd.Text);
                    width = Convert.ToInt32(this.tbThumbnailWidth.Text);
                    height = Convert.ToInt32(this.tbThumbnailHeight.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                pagestart = pagestart < 1 ? 1 : pagestart;
                pageend = pageend > client.PageCount ? client.PageCount : pageend;

                RegisterClientEvents(client);
                results = await client.GetThumbnailsAsync(pagestart, pageend, width, height);
                UnregisterClientEvents(client);

            }


            if (SourceType == 2)
            {
                //ingore page start and end parameter
                AXRESTClientDocPageVersion client = this.CurrentDocPageVersion;
                int width;
                int height;
                try
                {
                    width = Convert.ToInt32(this.tbThumbnailWidth.Text);
                    height = Convert.ToInt32(this.tbThumbnailHeight.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                RegisterClientEvents(client);
                AXRESTClientFile result = await client.GetThumbnailAsync(width, height);
                results = new List<AXRESTClientFile>() { result };
                UnregisterClientEvents(client);

            }

            if (SourceType == 3)
            {
                //ingore page start and end parameter
                AXRESTClientBatchPage client = this.CurrentBatchPage;
                int width;
                int height;
                try
                {
                    width = Convert.ToInt32(this.tbThumbnailWidth.Text);
                    height = Convert.ToInt32(this.tbThumbnailHeight.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                RegisterClientEvents(client);
                AXRESTClientFile result = await client.GetThumbnailAsync(width, height);
                results = new List<AXRESTClientFile>() { result };
                UnregisterClientEvents(client);

            }

            PopulateResultUI(results);
        }

        private void PopulateResultUI(List<AXRESTClientFile> results)
        {
            this.lbResults.Items.Clear();

            foreach(var file in results)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string fullname = System.IO.Path.GetTempPath() + fileName;
                file.SaveToLocal(fullname);
                this.lbResults.Items.Add(fullname);

                file.Dispose();
            }
        }

        private void lbResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string fullname = this.lbResults.SelectedValue.ToString();

            System.Diagnostics.Process.Start(fullname);
        }


    }
}
