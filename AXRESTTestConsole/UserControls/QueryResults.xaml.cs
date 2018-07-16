using System;
using System.Collections.Generic;
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
    /// Interaction logic for QueryResults.xaml
    /// </summary>
    public partial class QueryResults : BaseUserControl
    {
        public QueryResults()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientQuery"))
            {
                MessageBox.Show("Please get the Query resource firstly");
                return;
            }
            AXRESTClientQuery client = Global.clientCaches["AXRESTClientQuery"] as AXRESTClientQuery;

            RegisterClientEvents(client);
            AXRESTClientQueryResults resultsClient = await client.ExecuteAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateResultsUI(resultsClient);
        }

        private void PopulateResultsUI(AXRESTClientQueryResults resultsClient)
        {
            if (resultsClient.Columns == null || resultsClient.Collection == null ||
               resultsClient.Columns.Count == 0 || resultsClient.Collection.Count == 0) return;

            this.CurrentPage = resultsClient;

            this.btnFirst.IsEnabled = resultsClient.HasFirstPage;
            this.btnPrev.IsEnabled = resultsClient.HasPreviousPage;
            this.btnNext.IsEnabled = resultsClient.HasNextPage;
            this.btnLast.IsEnabled = resultsClient.HasLastPage;

            ExtendedDataTable table = new ExtendedDataTable();
            foreach (var col in resultsClient.Columns)
            {
                table.Columns.Add(col);
            }
            table.Columns.Add("DocID");
            table.Columns.Add("Page Count");
            
            foreach (var item in resultsClient.Collection)
            {
                ExtendedDataRow dr = table.NewRow() as ExtendedDataRow;
                int i = 0;
                for (; i < resultsClient.Columns.Count; i++)
                {
                    dr[i] = item.IndexValues[i];
                }
                dr[i++] = item.ID;
                dr[i++] = item.PageCount;
                dr.Tag = item;
                table.Rows.Add(dr);
            }

            this.dgResults.DataContext = table.DefaultView;
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientQueryResults client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientQueryResults pageClient = await client.GetFirstPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateResultsUI(pageClient);
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

            AXRESTClientQueryResults client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientQueryResults pageClient = await client.GetPreviousPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateResultsUI(pageClient);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientQueryResults client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientQueryResults pageClient = await client.GetNextPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateResultsUI(pageClient);
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientQueryResults client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientQueryResults pageClient = await client.GetLastPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulateResultsUI(pageClient);
        }

        private void dgResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExtendedDataRow selectedItem = ((DataRowView)this.dgResults.SelectedItem).Row as ExtendedDataRow;
            AXRESTClientQueryResultItem resultItem = selectedItem.Tag as AXRESTClientQueryResultItem;

            if(resultItem.IsReport)
            {
                TreeViewItem item = Global.GetTreeViewItemByName("Report Group", "Report");
                if (item != null)
                {
                    Report ui = Global.UIDic[item] as Report;
                    ui.SetResultItem(resultItem);
                    item.IsSelected = true;
                }
            }
            else
            {
                TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document");
                if (item != null)
                {
                    Document ui = Global.UIDic[item] as Document;
                    ui.SetResultItem(resultItem);
                    item.IsSelected = true;
                }
            }
        }


        public AXRESTClientQueryResults CurrentPage { get; set; }
    }


}
