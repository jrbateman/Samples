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
    /// Interaction logic for BatchPage.xaml
    /// </summary>
    public partial class BatchPage : BaseUserControl
    {
        public BatchPage()
        {
            InitializeComponent();

            files = new ObservableCollection<AXRESTClientFile>();
            this.lbFiles.ItemsSource = files;
        }

        internal void PopulateBatchPageList(List<AXRESTClientBatchPage> list)
        {
            this.cbPages.ItemsSource = list;
        }

        internal void SelectBatchPage(AXRESTClientBatchPage selectedItem)
        {
            this.cbPages.SelectedItem = selectedItem;
        }

        public override async Task Get()
        {
            AXRESTClientBatchPage client = this.cbPages.SelectedItem as AXRESTClientBatchPage;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            this.CurrentBatchPage = client;
        }

        public override async Task Put()
        {
            AXRESTClientBatchPage client = this.cbPages.SelectedItem as AXRESTClientBatchPage;

            AXRESTClientFile anno = null;
            AXRESTClientFile ocr = null;
            foreach (var f in files)
            {
                if (f.Type == AXRESTClientFile.AXClientFileTypes.Annotation)
                {
                    anno = f;
                    break;
                }
            }
            foreach (var f in files)
            {
                if (f.Type == AXRESTClientFile.AXClientFileTypes.Text)
                {
                    ocr = f;
                    break;
                }
            }

            RegisterClientEvents(client);
            await client.UpdateAnnotationAndTextFileAsync(anno, ocr, Global.MediaType);
            UnregisterClientEvents(client);

            btnClearFiles_Click(null, null);
        }

        public override async Task Delete()
        {
            AXRESTClientBatchPage client = this.cbPages.SelectedItem as AXRESTClientBatchPage;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);

            List<AXRESTClientBatchPage> list = this.cbPages.ItemsSource as List<AXRESTClientBatchPage>;
            list.Remove(client);
            this.cbPages.ItemsSource = null;
            this.cbPages.ItemsSource = list;

            if (list.Count > 0)
                this.cbPages.SelectedItem = list.First();
            else
                this.cbPages.SelectedItem = null;
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

        private void btnRender_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentBatchPage == null)
            {
                MessageBox.Show("Please get the batch page resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Rendition");
            if (item != null)
            {
                Rendition ui = Global.UIDic[item] as Rendition;
                ui.SetSource(1, this.CurrentBatchPage);
                item.IsSelected = true;
            }
        }

        private void btnThumbnail_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentBatchPage == null)
            {
                MessageBox.Show("Please get the batch page resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Thumbnails");
            if (item != null)
            {
                Thumbnails ui = Global.UIDic[item] as Thumbnails;
                ui.SetSource(3, this.CurrentBatchPage);
                item.IsSelected = true;
            }
        }

        public AXRESTClientBatchPage CurrentBatchPage { get; set; }

    }
}
