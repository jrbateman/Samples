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
    /// Interaction logic for Queries.xaml
    /// </summary>
    public partial class Queries : BaseUserControl
    {
        public Queries()
        {
            InitializeComponent();
            this.lbqueryIndexes.ItemsSource = data;
        }
        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();
        private List<AXRESTClientAppField> appfields;

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            RegisterClientEvents(client);
            AXRESTClientQueries queriesClient = await client.GetQueriesAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateQueriesUI(queriesClient);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            string queryname = this.tbQueryName.Text;
            if (string.IsNullOrEmpty(queryname)) return;

            Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
            foreach (var qi in data)
            {
                queryIndexes[qi.Field] = qi.Value;
            }

            var ftoptions = this.ftOptions.FTOptions;

            bool ispublic = this.IsPublic.IsChecked.HasValue && this.IsPublic.IsChecked.Value;
            bool includePreRevisions = this.IncludePreRevisions.IsChecked.HasValue && this.IncludePreRevisions.IsChecked.Value;

            RegisterClientEvents(client);
            AXRESTClientQuery query = await client.CreateQueryAsync(queryname, queryIndexes, ftoptions, ispublic, includePreRevisions, Global.MediaType);
            UnregisterClientEvents(client);
        }

        private void PopulateQueriesUI(AXRESTClientQueries queriesClient)
        {
            this.dgQueries.ItemsSource = queriesClient.Collection;

            //Populate Queries list in Query UI
            TreeViewItem item = Global.GetTreeViewItemByName("Query Group", "Query");
            if (item != null)
            {
                Query ui = Global.UIDic[item] as Query;
                ui.PopulateQueryList(queriesClient.Collection);
            }
        }

        private void dgQueries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientQuery selectedItem = this.dgQueries.SelectedItem as AXRESTClientQuery;

            TreeViewItem item = Global.GetTreeViewItemByName("Query Group", "Query");
            if (item != null)
            {
                Query ui = Global.UIDic[item] as Query;
                ui.SelectQuery(selectedItem);
                item.IsSelected = true;
            }
        }

        private void btnAddQI_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbFields.SelectedValue == null || string.IsNullOrEmpty(this.cbFields.SelectedValue.ToString()))
                return;

            foreach (QueryIndex qi in data)
            {
                if (qi.Field == this.cbFields.SelectedValue.ToString())
                {
                    return;
                }
            }

            data.Add(new QueryIndex() { Field = this.cbFields.SelectedValue.ToString(), Value = this.tbValue.Text });
        }

        private void btnDeleteQI_Click(object sender, RoutedEventArgs e)
        {
            data.Remove((QueryIndex)this.lbqueryIndexes.SelectedItem);
        }

        private void btnClearQI_Click(object sender, RoutedEventArgs e)
        {
            data.Clear();
        }

        private void tbValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (this.cbFields.SelectedValue == null || string.IsNullOrEmpty(this.cbFields.SelectedValue.ToString()))
                    return;

                foreach (QueryIndex qi in data)
                {
                    if (qi.Field == this.cbFields.SelectedValue.ToString())
                    {
                        return;
                    }
                }

                data.Add(new QueryIndex() { Field = this.cbFields.SelectedValue.ToString(), Value = this.tbValue.Text });
            }
        }

        internal void PopulateFields(List<AXRESTClientAppField> list)
        {
            this.appfields = list;
            this.cbFields.ItemsSource = this.appfields;
        }

        private class QueryIndex
        {
            public string Field { get; set; }

            public string Value { get; set; }
        }

    }
}
