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
using XtenderSolutions.AXRESTDataModel;

namespace AXRESTTestConsole.UserControls
{
    /// <summary>
    /// Interaction logic for Document.xaml
    /// </summary>
    public partial class Document : BaseUserControl
    {
        public Document()
        {
            InitializeComponent();

            appFields = new List<AXRESTClientAppField>();
            files = new ObservableCollection<AXRESTClientFile>();
            this.lbFiles.ItemsSource = files;
            this.lbqueryIndexes.ItemsSource = data;
        }
        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();

        public override async Task Get()
        {
            AXRESTClientQueryResultItem client = this.CurrentItem as AXRESTClientQueryResultItem;

            RegisterClientEvents(client);
            AXRESTClientDoc doc = await client.GetAXDocAsync(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientDoc"] = doc;

            PopulateDocUI(doc);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
           
            bool ignoreDuplicateIndex = this.chkbIgnoreDuplicateIndex.IsChecked.HasValue ? this.chkbIgnoreDuplicateIndex.IsChecked.Value : false;
            bool ignoreDlsViolation = this.chkbIgnoreDlsViolation.IsChecked.HasValue ? this.chkbIgnoreDlsViolation.IsChecked.Value : false;
            Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
            foreach (var qi in data)
            {
                queryIndexes[qi.FieldID] = qi.Value;
            }

            if (this.rbCreateDocument.IsChecked.HasValue && this.rbCreateDocument.IsChecked.Value)
            {
                AXRESTClientApplication appClient = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

                RegisterClientEvents(appClient);
                AXRESTClientBatch batchClient = await appClient.CreateDocumentAsync(queryIndexes, files.ToList(), ignoreDuplicateIndex, ignoreDlsViolation, Global.MediaType);
                UnregisterClientEvents(appClient);
            }
            else if (this.rbBatchIndex.IsChecked.HasValue && this.rbBatchIndex.IsChecked.Value)
            {
                AXRESTClientBatch batchClient = this.cbBatches.SelectedItem as AXRESTClientBatch;
                if (batchClient == null)
                {
                    MessageBox.Show("Please select a batch first.");
                    return;
                }

                string batchPageNumber = this.tbBatchPageNumber.Text;
                string targetDocID = this.tbTargetDocId.Text;

                RegisterClientEvents(batchClient);
                AXRESTClientDoc docClient = await batchClient.IndexAsync(queryIndexes, Convert.ToInt32(batchPageNumber), Convert.ToUInt32(targetDocID), ignoreDuplicateIndex, ignoreDlsViolation, Global.MediaType);
                UnregisterClientEvents(batchClient);
            }

            btnClearFiles_Click(null, null);
        }

        public override async Task Put()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the document first.");
                return;
            }

            AXRESTClientDoc client = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.SubmitJobsAsync(rbSubmitFTJob.IsChecked.Value, rbSubmitOCRJob.IsChecked.Value, Global.MediaType);
            UnregisterClientEvents(client);
        }

        public override async Task Delete()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the document first.");
                return;
            }

            AXRESTClientDoc client = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches.Remove("AXRESTClientDoc");
        }

        private void PopulateDocUI(AXRESTClientDoc doc)
        {
            this.CurrentDoc = doc;

            this.lbDoc.Items.Clear();

            this.lbDoc.Items.Add(string.Format("{0}: {1}", "ID", doc.ID));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "PageCount", doc.PageCount));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "IsCOLD", doc.IsCOLD));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "FulltextIndexed", doc.FulltextIndexed));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "HasVersions", doc.HasVersions));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "FinalRevision", doc.FinalRevision));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "PreviousRevision", doc.PreviousRevision));
            this.lbDoc.Items.Add(string.Format("{0}: {1}", "Checkedout", doc.Checkedout));

            MainWindow mainWin = App.Current.MainWindow as MainWindow;
            mainWin.SetStatusBar(currentdoc: doc.ID);
        }

        private void btnAddQI_Click(object sender, RoutedEventArgs e)
        {
            AddQueryFieldItem();
        }

        private void btnDeleteQI_Click(object sender, RoutedEventArgs e)
        {
            data.Remove((QueryIndex)this.lbqueryIndexes.SelectedItem);
        }

        private void tbValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddQueryFieldItem();
            }
        }

        private void AddQueryFieldItem()
        {
            if (this.cbFields.SelectedValue == null || string.IsNullOrEmpty(this.cbFields.SelectedValue.ToString()))
                return;

            foreach (QueryIndex qi in data)
            {
                if (qi.Field == this.cbFields.SelectedValue.ToString())
                {
                    qi.Value = this.tbValue.Text;
                    return;
                }
            }

            data.Add(new QueryIndex() { FieldID = appFields.Where(field => field.Name == this.cbFields.SelectedValue.ToString()).FirstOrDefault().ID, Field = this.cbFields.SelectedValue.ToString(), Value = this.tbValue.Text });
        }

        private void btnClearQI_Click(object sender, RoutedEventArgs e)
        {
            data.Clear();
        }

        internal void PopulateFields(List<AXRESTClientAppField> list)
        {
            this.cbFields.ItemsSource = list;
            appFields = list;
        }

        internal void SetResultItem(AXRESTClientQueryResultItem resultItem)
        {
            this.CurrentItem = resultItem;
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

        public AXRESTClientQueryResultItem CurrentItem { get; set; }

        public ObservableCollection<AXRESTClientFile> files { get; set; }

        public List<AXRESTClientAppField> appFields { get; set; }

        private class QueryIndex
        {
            public string FieldID { get; set; }

            public string Field { get; set; }

            public string Value { get; set; }
        }

        private void btnThumbnail_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentDoc == null)
            {
                MessageBox.Show("Please get the doc resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Thumbnails");
            if (item != null)
            {
                Thumbnails ui = Global.UIDic[item] as Thumbnails;
                ui.SetSource(0, this.CurrentDoc);
                item.IsSelected = true;
            }
        }


        public AXRESTClientDoc CurrentDoc { get; set; }

        #region batch

        internal void PopulateBatchList(List<AXRESTClientBatch> list)
        {
            this.cbBatches.ItemsSource = list;
        }

        private void rbBatchIndex_Click(object sender, RoutedEventArgs e)
        {
            postGrid.RowDefinitions[1].Height = new GridLength(35);
            postGrid.RowDefinitions[3].Height = new GridLength(35);
            postGrid.RowDefinitions[4].Height = new GridLength(35);
            postGrid.RowDefinitions[5].Height = new GridLength(0);
            postGrid.RowDefinitions[6].Height = new GridLength(0);
        }

        private void rbCreateDocument_Click(object sender, RoutedEventArgs e)
        {
            postGrid.RowDefinitions[1].Height = new GridLength(0);
            postGrid.RowDefinitions[3].Height = new GridLength(0);
            postGrid.RowDefinitions[4].Height = new GridLength(0);
            postGrid.RowDefinitions[5].Height = new GridLength(35);
            postGrid.RowDefinitions[6].Height = new GridLength(100);
        }

        #endregion
    }
}
