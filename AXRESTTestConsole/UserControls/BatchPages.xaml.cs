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
    /// Interaction logic for BatchPages.xaml
    /// </summary>
    public partial class BatchPages : BaseUserControl
    {
        public BatchPages()
        {
            InitializeComponent();

            files = new ObservableCollection<AXRESTClientFile>();

            this.lbFiles.ItemsSource = files;
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientBatch"))
            {
                MessageBox.Show("Please get the batch resource firstly");
                return;
            }
            AXRESTClientBatch client = Global.clientCaches["AXRESTClientBatch"] as AXRESTClientBatch;

            RegisterClientEvents(client);
            AXRESTClientBatchPages  bpagesClient = await client.GetAXBatchPagesAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateBatchPagesUI(bpagesClient);
        }

        private void PopulateBatchPagesUI(AXRESTClientBatchPages bpagesClient)
        {
            this.CurrentPage = bpagesClient;

            this.btnFirst.IsEnabled = bpagesClient.HasFirstPage;
            this.btnPrev.IsEnabled = bpagesClient.HasPreviousPage;
            this.btnNext.IsEnabled = bpagesClient.HasNextPage;
            this.btnLast.IsEnabled = bpagesClient.HasLastPage;

            this.dgBatchPages.ItemsSource = bpagesClient.Collection;

            //populate batch pages list in batch page UI
            TreeViewItem item = Global.GetTreeViewItemByName("Batch Group", "Batch Page");
            if (item != null)
            {
                BatchPage ui = Global.UIDic[item] as BatchPage;
                ui.PopulateBatchPageList(bpagesClient.Collection);
            }
        }

        public override async Task Post()
        {
            if (this.CurrentPage == null) return;

            AXRESTClientBatchPages client = this.CurrentPage;

            RegisterClientEvents(client);
            await client.CreateBatchPageAsync(files.ToList(), Global.MediaType);
            UnregisterClientEvents(client);

            btnClearFiles_Click(null, null);
        }


        private void dgBatchPages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientBatchPage selectedItem = this.dgBatchPages.SelectedItem as AXRESTClientBatchPage;

            TreeViewItem item = Global.GetTreeViewItemByName("Batch Group", "Batch Page");
            if (item != null)
            {
                BatchPage ui = Global.UIDic[item] as BatchPage;
                ui.SelectBatchPage(selectedItem);
                item.IsSelected = true;
            }

        }
        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientBatchPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientBatchPages bpagesClient = await client.GetFirstPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateBatchPagesUI(bpagesClient);
        }

        private void UpdateMainWindow(string timestart, string timecost, string request, string response)
        {
            MainWindow win = App.Current.MainWindow as MainWindow;
            win.tbRequestStart.Text = timestart;
            win.tbRequestTime.Text = timecost;

            win.tbRequestContent.Text = request;
            win.tbResponseContent.Text = response;
        }

        private async void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientBatchPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientBatchPages bpagesClient = await client.GetPreviousPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateBatchPagesUI(bpagesClient);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientBatchPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientBatchPages bpagesClient = await client.GetNextPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateBatchPagesUI(bpagesClient);
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientBatchPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientBatchPages bpagesClient = await client.GetLastPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateBatchPagesUI(bpagesClient);
        }

        public AXRESTClientBatchPages CurrentPage { get; set; }

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
