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
    /// Interaction logic for DocumentIndex.xaml
    /// </summary>
    public partial class DocumentIndex : BaseUserControl
    {
        public DocumentIndex()
        {
            InitializeComponent();

            appFields = new List<AXRESTClientAppField>();
            this.lbqueryIndexes.ItemsSource = data;
        }

        public override async Task Get()
        {
            AXRESTClientDocIndex client = this.cbIndexes.SelectedItem as AXRESTClientDocIndex;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientDocIndex"] = client;

            PopulateIndexesUI(client);
        }

        public override async Task Put()
        {
            AXRESTClientDocIndex client = this.cbIndexes.SelectedItem as AXRESTClientDocIndex;
            if (client == null) return;

            bool failIfMatchIndex = this.chkbFailIfMatchIndex.IsChecked.HasValue ? this.chkbFailIfMatchIndex.IsChecked.Value : false;
            bool failIfDLSViolation = this.chkbFailIfDLSViolation.IsChecked.HasValue ? this.chkbFailIfDLSViolation.IsChecked.Value : false;
            Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
            foreach (var qi in data)
            {
                queryIndexes[qi.FieldID] = qi.Value;
            }

            RegisterClientEvents(client);
            await client.UpdateAsync(queryIndexes, failIfMatchIndex, failIfDLSViolation, Global.MediaType);
            UnregisterClientEvents(client);
        }

        public override async Task Delete()
        {
            AXRESTClientDocIndex client = this.cbIndexes.SelectedItem as AXRESTClientDocIndex;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);

            data.Clear();

            Global.clientCaches.Remove("AXRESTClientDocIndex");
        }

        private void PopulateIndexesUI(AXRESTClientDocIndex indexClient)
        {
            this.dgIndexFields.ItemsSource = indexClient.IndexValues;
        }

        internal void PopulateIndexList(List<AXRESTClientDocIndex> list)
        {
            this.cbIndexes.ItemsSource = list;
        }

        internal void SelectIndex(AXRESTClientDocIndex selectedItem)
        {
            this.cbIndexes.SelectedItem = selectedItem;
        }

        private void cbIndexes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AXRESTClientDocIndex client = this.cbIndexes.SelectedItem as AXRESTClientDocIndex;
            if (client != null)
            {
                PopulateIndexesUI(client);
            }
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

        internal void PopulateFields(List<AXRESTClientAppField> list)
        {
            this.cbFields.ItemsSource = list;
            appFields = list;
        }

        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();

        public List<AXRESTClientAppField> appFields { get; set; }

        private class QueryIndex
        {
            public string FieldID { get; set; }

            public string Field { get; set; }

            public string Value { get; set; }
        }

    }
}
