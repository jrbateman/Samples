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
    /// Interaction logic for DocumentIndexes.xaml
    /// </summary>
    public partial class DocumentIndexes : BaseUserControl
    {
        public DocumentIndexes()
        {
            InitializeComponent();

            appFields = new List<AXRESTClientAppField>();
            this.lbqueryIndexes.ItemsSource = data;
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the document firstly");
                return;
            }
            AXRESTClientDoc client = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

            RegisterClientEvents(client);
            AXRESTClientDocIndexes indexesClient = await client.GetDocIndexesAsync(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientDocIndexes"] = indexesClient;

            PopulateIndexesUI(indexesClient);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the document first.");
                return;
            }

            AXRESTClientDoc client = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;
            if (client == null) return;

            bool failIfMatchIndex = this.chkbFailIfMatchIndex.IsChecked.HasValue ? this.chkbFailIfMatchIndex.IsChecked.Value : false;
            bool failIfDLSViolation = this.chkbFailIfDLSViolation.IsChecked.HasValue ? this.chkbFailIfDLSViolation.IsChecked.Value : false;
            Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
            foreach (var qi in data)
            {
                queryIndexes[qi.FieldID] = qi.Value;
            }

            RegisterClientEvents(client);
            await client.NewDocIndexAsync(queryIndexes, failIfMatchIndex, failIfDLSViolation, Global.MediaType);
            UnregisterClientEvents(client);
        }

        private void PopulateIndexesUI(AXRESTClientDocIndexes indexesClient)
        {
            CurrentPage = indexesClient;

            this.dgIndexes.ItemsSource = indexesClient.Collection;

            this.btnFirst.IsEnabled = indexesClient.HasFirstPage;
            this.btnPrev.IsEnabled = indexesClient.HasPreviousPage;
            this.btnNext.IsEnabled = indexesClient.HasNextPage;
            this.btnLast.IsEnabled = indexesClient.HasLastPage;

            //populate indexes combox in Index UI
            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document Index");
            if (item != null)
            {
                DocumentIndex ui = Global.UIDic[item] as DocumentIndex;
                ui.PopulateIndexList(CurrentPage.Collection);
            }
        }

        internal void PopulateFields(List<AXRESTClientAppField> list)
        {
            this.cbFields.ItemsSource = list;
            appFields = list;
        }

        private void dgIndexes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientDocIndex selectedItem = this.dgIndexes.SelectedItem as AXRESTClientDocIndex;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document Index");
            if (item != null)
            {
                DocumentIndex ui = Global.UIDic[item] as DocumentIndex;
                ui.SelectIndex(selectedItem);
                item.IsSelected = true;
            }
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientDocIndexes client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientDocIndexes indexesClient = await client.GetFirstPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateIndexesUI(indexesClient);
        }

        private async void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientDocIndexes client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientDocIndexes indexesClient = await client.GetPreviousPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateIndexesUI(indexesClient);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientDocIndexes client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientDocIndexes indexesClient = await client.GetNextPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateIndexesUI(indexesClient);
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientDocIndexes client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientDocIndexes indexesClient = await client.GetLastPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateIndexesUI(indexesClient);
        }

        private void UpdateMainWindow(string timestart, string timecost, string request, string response)
        {
            MainWindow win = App.Current.MainWindow as MainWindow;
            win.tbRequestStart.Text = timestart;
            win.tbRequestTime.Text = timecost;

            win.tbRequestContent.Text = request;
            win.tbResponseContent.Text = response;
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

        private void btnClearQI_Click(object sender, RoutedEventArgs e)
        {
            data.Clear();
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

        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();

        public AXRESTClientDocIndexes CurrentPage { get; set; }

        public List<AXRESTClientAppField> appFields { get; set; }

        private class QueryIndex
        {
            public string FieldID { get; set; }

            public string Field { get; set; }

            public string Value { get; set; }
        }
    }
}
