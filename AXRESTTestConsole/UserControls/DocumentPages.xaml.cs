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
    /// Interaction logic for DocumentPages.xaml
    /// </summary>
    public partial class DocumentPages : BaseUserControl
    {
        public DocumentPages()
        {
            InitializeComponent();
        }

        public AXRESTClientDocPages CurrentPageCollection { get; set; }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the Document resource firstly");
                return;
            }
            AXRESTClientDoc docClient = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

            RegisterClientEvents(docClient);
            AXRESTClientDocPages dpsClient = await docClient.GetAXDocPagesAsync(Global.MediaType);
            UnregisterClientEvents(docClient);

            PopulateDocumentPagesUI(dpsClient);
        }

        private void PopulateDocumentPagesUI(AXRESTClientDocPages dpsClient)
        {
            this.CurrentPageCollection = dpsClient;

            this.btnFirst.IsEnabled = dpsClient.HasFirstPage;
            this.btnPrev.IsEnabled = dpsClient.HasPreviousPage;
            this.btnNext.IsEnabled = dpsClient.HasNextPage;
            this.btnLast.IsEnabled = dpsClient.HasLastPage;

            this.dgDocPages.ItemsSource = dpsClient.Collection;
            this.cbLocation.ItemsSource = dpsClient.Collection;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document Page");
            if (item != null)
            {
                DocumentPage docPageUI = Global.UIDic[item] as DocumentPage;
                docPageUI.PopulateDocPageList(dpsClient.Collection);
            }
        }

        private void dgDocPages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientDocPage selectedItem = this.dgDocPages.SelectedItem as AXRESTClientDocPage;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document Page");
            if (item != null)
            {
                DocumentPage ui = Global.UIDic[item] as DocumentPage;
                ui.SelectDocPage(selectedItem);
                item.IsSelected = true;
            }
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPageCollection == null) return;

            AXRESTClientDocPages client = this.CurrentPageCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPages docPagesClient = await client.GetFirstPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulateDocumentPagesUI(docPagesClient);
        }

        private async void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPageCollection == null) return;

            AXRESTClientDocPages client = this.CurrentPageCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPages docPagesClient = await client.GetPreviousPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulateDocumentPagesUI(docPagesClient);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPageCollection == null) return;

            AXRESTClientDocPages client = this.CurrentPageCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPages docPagesClient = await client.GetNextPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulateDocumentPagesUI(docPagesClient);
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPageCollection == null) return;

            AXRESTClientDocPages client = this.CurrentPageCollection;

            RegisterClientEvents(client);
            AXRESTClientDocPages docPagesClient = await client.GetLastPageAsync(Global.MediaType);
            UnregisterClientEvents(client);

            UpdateRequestInfo();
            PopulateDocumentPagesUI(docPagesClient);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the Document resource firstly");
                return;
            }
            AXRESTClientDoc docClient = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

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

            RegisterClientEvents(docClient);
            await docClient.NewDocPageAsync(binFile, annoFile, txtFile,
                cbLocation.SelectedIndex, cbOperation.SelectedIndex, Global.MediaType);
            UnregisterClientEvents(docClient);
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
