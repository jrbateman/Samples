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
    /// Interaction logic for DocumentPageVersions.xaml
    /// </summary>
    public partial class DocumentPageVersions : BaseUserControl
    {
        public DocumentPageVersions()
        {
            InitializeComponent();
        }

        public AXRESTClientDocPageVersions CurrentVersionCollection { get; set; }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDocPage"))
            {
                MessageBox.Show("Please get the Document Page resource firstly");
                return;
            }
            AXRESTClientDocPage pgClient = Global.clientCaches["AXRESTClientDocPage"] as AXRESTClientDocPage;

            RegisterClientEvents(pgClient);
            AXRESTClientDocPageVersions pvsClient = await pgClient.GetPageVersionsAsync(Global.MediaType);
            UnregisterClientEvents(pgClient);

            PopulatePageVersionsUI(pvsClient);
        }

        private void PopulatePageVersionsUI(AXRESTClientDocPageVersions pvsClient)
        {
            this.CurrentVersionCollection = pvsClient;

            this.btnFirst.IsEnabled = pvsClient.HasFirstPage;
            this.btnPrev.IsEnabled = pvsClient.HasPreviousPage;
            this.btnNext.IsEnabled = pvsClient.HasNextPage;
            this.btnLast.IsEnabled = pvsClient.HasLastPage;

            this.dgPageVersions.ItemsSource = pvsClient.Collection;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document PageVersion");
            if (item != null)
            {
                DocumentPageVersion docPageVerUI = Global.UIDic[item] as DocumentPageVersion;
                docPageVerUI.PopulateDocPageVersionList(pvsClient.Collection);
            }
        }

        private void dgPageVersions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientDocPageVersion selectedItem = this.dgPageVersions.SelectedItem as AXRESTClientDocPageVersion;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document PageVersion");
            if (item != null)
            {
                DocumentPageVersion ui = Global.UIDic[item] as DocumentPageVersion;
                ui.SelectDocPageVersion(selectedItem);
                item.IsSelected = true;
            }
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentVersionCollection == null) return;

            AXRESTClientDocPageVersions client = this.CurrentVersionCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPageVersions versionsClient = await client.GetFirstPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulatePageVersionsUI(versionsClient);
        }

        private async void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentVersionCollection == null) return;

            AXRESTClientDocPageVersions client = this.CurrentVersionCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPageVersions versionsClient = await client.GetPreviousPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulatePageVersionsUI(versionsClient);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentVersionCollection == null) return;

            AXRESTClientDocPageVersions client = this.CurrentVersionCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPageVersions versionsClient = await client.GetNextPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulatePageVersionsUI(versionsClient);
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentVersionCollection == null) return;

            AXRESTClientDocPageVersions client = this.CurrentVersionCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPageVersions versionsClient = await client.GetLastPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulatePageVersionsUI(versionsClient);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDocPage"))
            {
                MessageBox.Show("Please get the Document Page resource firstly");
                return;
            }
            AXRESTClientDocPage pageClient = Global.clientCaches["AXRESTClientDocPage"] as AXRESTClientDocPage;

            if (string.IsNullOrEmpty(this.txtBinFile.Text))
            {
                MessageBox.Show("Please speficy the bin file firstly");
                return;
            }

            AXRESTClientFile binFile = AXRESTClientFile.LoadFromFile(this.txtBinFile.Text, AXRESTClientFile.AXClientFileTypes.Bin);
            AXRESTClientFile annoFile = null;
            if (!string.IsNullOrEmpty(this.txtAnnoFile.Text))
                annoFile = AXRESTClientFile.LoadFromFile(this.txtAnnoFile.Text, AXRESTClientFile.AXClientFileTypes.Annotation);
            AXRESTClientFile txtFile = null;
            if (!string.IsNullOrEmpty(this.txtTxtFile.Text))
                txtFile = AXRESTClientFile.LoadFromFile(this.txtTxtFile.Text, AXRESTClientFile.AXClientFileTypes.Text);

            RegisterClientEvents(pageClient);
            await pageClient.NewPageVersionAsync(binFile, annoFile, txtFile, Global.MediaType);
            UnregisterClientEvents(pageClient);
        }

        private void btnBinFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? result = dlg.ShowDialog();

            if (result == true)
                this.txtBinFile.Text = dlg.FileName;
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
    }
}
