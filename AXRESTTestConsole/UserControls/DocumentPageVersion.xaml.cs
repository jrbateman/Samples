using Microsoft.Win32;
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
    /// Interaction logic for DocumentPageVersion.xaml
    /// </summary>
    public partial class DocumentPageVersion : BaseUserControl
    {
        public DocumentPageVersion()
        {
            InitializeComponent();
        }

        internal void PopulateDocPageVersionList(List<AXRESTClientDocPageVersion> list)
        {
            this.cbDocPageVers.IsEnabled = true;
            this.cbDocPageVers.ItemsSource = list;
        }

        internal void SelectDocPageVersion(AXRESTClientDocPageVersion selectedItem)
        {
            this.cbDocPageVers.SelectedItem = selectedItem;
        }

        internal async void ShowCurrentDocPageVersion(AXRESTClientDocPage selectedItem)
        {
            RegisterClientEvents(selectedItem);
            AXRESTClientDocPageVersion pgVer = await selectedItem.GetCurrentPageVersionAsync(Global.MediaType);
            UnregisterClientEvents(selectedItem);

            Global.clientCaches["AXRESTClientDocPageVersion"] = pgVer;

            this.cbDocPageVers.ItemsSource = new List<AXRESTClientDocPageVersion>() { pgVer };
            this.cbDocPageVers.IsEnabled = false;
            this.cbDocPageVers.SelectedItem = pgVer;

            UpdateRequestInfo();
            PopulatePageVersionUI(pgVer);
        }

        public override async Task Get()
        {
            AXRESTClientDocPageVersion client = this.cbDocPageVers.SelectedItem as AXRESTClientDocPageVersion;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientDocPageVersion"] = client;
            this.CurrentPageVersion = client;
            PopulatePageVersionUI(client);
        }

        private void PopulatePageVersionUI(AXRESTClientDocPageVersion client)
        {
            this.lbInfo.ItemsSource = new List<string>()
            {
                string.Format("Version: {0}", client.Version),
                string.Format("PageNumber: {0}", client.PageNumber),
                string.Format("Link: {0}", client.Location),
                string.Format("HasAnnotation: {0}", client.HasAnnotation),
                string.Format("HasTextView: {0}", client.HasTextView),
            };
        }

        private void lbInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public override async Task Put()
        {
            AXRESTClientDocPageVersion client = this.cbDocPageVers.SelectedItem as AXRESTClientDocPageVersion;
            if (client == null) return;

            AXRESTClientFile annoFile = null;
            if (!string.IsNullOrEmpty(this.txtAnnoFile.Text))
                annoFile = AXRESTClientFile.LoadFromFile(this.txtAnnoFile.Text, AXRESTClientFile.AXClientFileTypes.Annotation);
            AXRESTClientFile txtFile = null;
            if (!string.IsNullOrEmpty(this.txtTxtFile.Text))
                txtFile = AXRESTClientFile.LoadFromFile(this.txtTxtFile.Text, AXRESTClientFile.AXClientFileTypes.Text);

            RegisterClientEvents(client);
            await client.UpdateAnnotationAndTextFileAsync(annoFile, txtFile, Global.MediaType);
            UnregisterClientEvents(client);
        }

        private void btnAnnoFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? result = dlg.ShowDialog();

            if (result == true)
                this.txtAnnoFile.Text = dlg.FileName;
        }

        private void btnTxtFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? result = dlg.ShowDialog();

            if (result == true)
                this.txtTxtFile.Text = dlg.FileName;
        }

        public override async Task Delete()
        {
            AXRESTClientDocPageVersion client = this.cbDocPageVers.SelectedItem as AXRESTClientDocPageVersion;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);
        }

        private void btnRender_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPageVersion == null)
            {
                MessageBox.Show("Please get the doc page version resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Rendition");
            if (item != null)
            {
                Rendition ui = Global.UIDic[item] as Rendition;
                ui.SetSource(0, this.CurrentPageVersion);
                item.IsSelected = true;
            }
        }

        private void btnThumbnail_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPageVersion == null)
            {
                MessageBox.Show("Please get the doc page version resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Thumbnails");
            if (item != null)
            {
                Thumbnails ui = Global.UIDic[item] as Thumbnails;
                ui.SetSource(2, this.CurrentPageVersion);
                item.IsSelected = true;
            }
        }

        public AXRESTClientDocPageVersion CurrentPageVersion { get; set; }

        
    }
}
