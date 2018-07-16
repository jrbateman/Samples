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
    /// Interaction logic for Query.xaml
    /// </summary>
    public partial class Query : BaseUserControl
    {
        public Query()
        {
            InitializeComponent();
            this.lbqueryIndexes.ItemsSource = data;
        }
        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();
        internal void SelectQuery(AXRESTClientQuery selectedItem)
        {
            this.cbQueries.SelectedItem = selectedItem;
        }

        internal void PopulateQueryList(List<AXRESTClientQuery> list)
        {
            this.cbQueries.ItemsSource = list;
        }

        public override async Task Get()
        {
            AXRESTClientQuery client = this.cbQueries.SelectedItem as AXRESTClientQuery;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientQuery"] = client;

            PolupateQueryUI(client);
        }

        public override async Task Put()
        {
            AXRESTClientQuery client = this.cbQueries.SelectedItem as AXRESTClientQuery;
            if (client == null) return;

            Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
            foreach (var qi in data)
            {
                queryIndexes[qi.Field] = qi.Value;
            }

            var ftoptions = this.ftOptions.FTOptions;

            bool ispublic = this.IsPublic.IsChecked.HasValue && this.IsPublic.IsChecked.Value;
            bool includePreRevisions = this.IncludePreRevisions.IsChecked.HasValue && this.IncludePreRevisions.IsChecked.Value;

            RegisterClientEvents(client);
            await client.UpdateAsync(queryIndexes, ftoptions, ispublic, includePreRevisions, Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientQuery"] = client;
        }

        public override async Task Delete()
        {
            AXRESTClientQuery client = this.cbQueries.SelectedItem as AXRESTClientQuery;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);

            List<AXRESTClientQuery> list = this.cbQueries.ItemsSource as List<AXRESTClientQuery>;
            list.Remove(client);
            this.cbQueries.ItemsSource = list;
            if (list.Count > 0)
                this.cbQueries.SelectedItem = list.First();
            else
                this.cbQueries.SelectedItem = null;
        }

        private void PolupateQueryUI(AXRESTClientQuery client)
        {
            this.lbQuery.Items.Clear();

            this.lbQuery.Items.Add(string.Format("{0}: {1}", "ID", client.ID));
            this.lbQuery.Items.Add(string.Format("{0}: {1}", "Name", client.Name));
            this.lbQuery.Items.Add(string.Format("{0}: {1}", "IsPublic", client.IsPublic));
            this.lbQuery.Items.Add(string.Format("{0}: {1}", "QueryType", client.QueryType));

            MainWindow mainWin = App.Current.MainWindow as MainWindow;
            mainWin.SetStatusBar(currentquery: client.ToString());
        }


        private void btnDeleteQI_Click(object sender, RoutedEventArgs e)
        {
            data.Remove((QueryIndex)this.lbqueryIndexes.SelectedItem);
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

        internal void PopulateFields(List<AXRESTClientQueryField> list)
        {
            this.cbFields.ItemsSource = list;
        }

        private class QueryIndex
        {
            public string Field { get; set; }

            public string Value { get; set; }
        }


    }
}
