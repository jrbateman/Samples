using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Batches.xaml
    /// </summary>
    public partial class Batches : BaseUserControl
    {
        public Batches()
        {
            InitializeComponent();

            files = new ObservableCollection<AXRESTClientFile>();
            this.lbFiles.ItemsSource = files;
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            RegisterClientEvents(client);
            AXRESTClientBatches batchesClient = await client.GetBatchesAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateBatchesUI(batchesClient);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            string name = this.tbName.Text;
            string description = this.tbDescription.Text;
            bool isprivate = this.chkbPrivate.IsChecked.HasValue ? this.chkbPrivate.IsChecked.Value : false;


            RegisterClientEvents(client);
            AXRESTClientBatch batchClient = await client.CreateBatchDocumentAsync(name, description, files.ToList(), isprivate, Global.MediaType);
            UnregisterClientEvents(client);

            this.tbName.Text = this.tbDescription.Text = string.Empty;
            this.chkbPrivate.IsChecked = false;
            btnClearFiles_Click(null, null);

        }

        private void PopulateBatchesUI(AXRESTClientBatches batchesClient)
        {
            this.dgBatches.ItemsSource = batchesClient.Collection;

            //populate batches combox in Batch UI
            TreeViewItem item = Global.GetTreeViewItemByName("Batch Group", "Batch");
            if (item != null)
            {
                Batch ui = Global.UIDic[item] as Batch;
                ui.PopulateBatchList(batchesClient.Collection);
            }

            //populate batches combox in Batch Index UI
            item = Global.GetTreeViewItemByName("Document Group", "Document");
            if (item != null)
            {
                Document ui = Global.UIDic[item] as Document;
                ui.PopulateBatchList(batchesClient.Collection);
            }
        }

        private void dgBatches_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientBatch selectedItem = this.dgBatches.SelectedItem as AXRESTClientBatch;

            TreeViewItem item = Global.GetTreeViewItemByName("Batch Group", "Batch");
            if (item != null)
            {
                Batch ui = Global.UIDic[item] as Batch;
                ui.SelectBatch(selectedItem);
                item.IsSelected = true;
            }

        }

        private void btnAddBinFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fullpath = dlg.FileName;
                var file = AXRESTClientFile.LoadFromFile(
                    fullpath, AXRESTClientFile.AXClientFileTypes.Bin);
                files.Add(file);
            }

        }
        private void btnAddAnnoFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fullpath = dlg.FileName;
                var file = AXRESTClientFile.LoadFromFile(
                    fullpath, AXRESTClientFile.AXClientFileTypes.Annotation);
                files.Add(file);
            }

        }
        private void btnAddTextViewFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fullpath = dlg.FileName;
                var file = AXRESTClientFile.LoadFromFile(
                    fullpath, AXRESTClientFile.AXClientFileTypes.Text);
                files.Add(file);
            }

        }

        private void btnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            var file = this.lbFiles.SelectedItem as AXRESTClientFile;
            if (file == null) return;

            file.Dispose();
            files.Remove(file);
        }

        private void btnClearFiles_Click(object sender, RoutedEventArgs e)
        {
            foreach (var file in files)
            {
                file.Dispose();
            }
            files.Clear();
        }



        public ObservableCollection<AXRESTClientFile> files { get; set; }
    }
}
