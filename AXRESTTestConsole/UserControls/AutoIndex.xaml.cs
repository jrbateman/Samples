using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for AutoIndex.xaml
    /// </summary>
    public partial class AutoIndex : BaseUserControl
    {
        public AutoIndex()
        {
            InitializeComponent();
            this.lbqueryIndexes.ItemsSource = data;
        }

        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();


        public override async Task Post()
        {
            Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
            foreach (var qi in data)
            {
                queryIndexes[qi.Field] = qi.Value;
            }


            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            RegisterClientEvents(client);
            AXRESTClientQueryResults resultsClient = await client.GetAutoIndexLookupResultsAsync(queryIndexes, Global.MediaType);
            UnregisterClientEvents(client);

            PopulateResultsUI(resultsClient);
        }

        private void PopulateResultsUI(AXRESTClientQueryResults resultsClient)
        {
            if (resultsClient.Columns == null || resultsClient.Collection == null ||
                resultsClient.Columns.Count == 0 || resultsClient.Collection.Count == 0) return;

            DataTable table = new DataTable();
            foreach (var col in resultsClient.Columns)
            {
                table.Columns.Add(col);
            }

            foreach (var item in resultsClient.Collection)
            {
                table.Rows.Add(item.IndexValues.ToArray());
            }

            this.dgAutoIndexResults.DataContext = table.DefaultView;
        }

        internal void PopulateFields(List<AXRESTClientAppField> list)
        {
            this.cbFields.ItemsSource = list;
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

        private class QueryIndex
        {
            public string Field { get; set; }

            public string Value { get; set; }
        }



    }
}
