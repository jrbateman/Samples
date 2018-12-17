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
    /// Interaction logic for AdhocQueryResults.xaml
    /// </summary>
    public partial class 
        AdhocQueryResults : BaseUserControl
    {
        public AdhocQueryResults()
        {
            InitializeComponent();
            this.lbqueryIndexes.ItemsSource = data;
        }

        private ObservableCollection<QueryIndex> data = new ObservableCollection<QueryIndex>();
        private List<AXRESTClientAppField> appfields;
        private List<AXRESTClientCAQField> caqfields;


        private void rbNormalQuery_Click(object sender, RoutedEventArgs e)
        {
            this.normalQueryGrid.Visibility = System.Windows.Visibility.Visible;
            this.reportQueryGrid.Visibility = System.Windows.Visibility.Hidden;

            data.Clear();
            this.cbFields.ItemsSource = this.appfields;
        }

        private void rbReport_Click(object sender, RoutedEventArgs e)
        {
            this.reportQueryGrid.Visibility = System.Windows.Visibility.Visible;
            this.normalQueryGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void rbCAQ_Click(object sender, RoutedEventArgs e)
        {
            this.normalQueryGrid.Visibility = System.Windows.Visibility.Visible;
            this.reportQueryGrid.Visibility = System.Windows.Visibility.Hidden;

            data.Clear();
            this.cbFields.ItemsSource = this.caqfields;

        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            AXRESTClientQueryResults results = null;
            RegisterClientEvents(client);

            if (this.rbNormalQuery.IsChecked.HasValue && this.rbNormalQuery.IsChecked.Value)
            {
                Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
                foreach (var qi in data)
                {
                    queryIndexes[qi.Field] = qi.Value;
                }

                var ftoptions = this.ftOptions.FTOptions;

                bool includePreRevisions = this.IncludePreRevisions.IsChecked.HasValue && this.IncludePreRevisions.IsChecked.Value;

                results = await client.ExecuteAdhocDocumentQueryAsync(queryIndexes, ftoptions, includePreRevisions, Global.MediaType);
            }
            else if (this.rbReportQuery.IsChecked.HasValue && this.rbReportQuery.IsChecked.Value)
            {
                string timestamp = this.tbTIMESTAMP.Text;
                string description = this.tbDESC.Text;
                string reporttype = this.tbRPTTYPE.Text;
                //execute report query.
                results = await client.ExecuteAdhocReportQueryAsync(timestamp, description, reporttype, Global.MediaType);
            }
            else if (this.rbCAQQuery.IsChecked.HasValue && this.rbCAQQuery.IsChecked.Value)
            {
                if (this.CurrentCAQ == null)
                {
                    MessageBox.Show("Please get the CAQ Fields resource firstly");
                    return;
                }

                Dictionary<string, string> queryIndexes = new Dictionary<string, string>();
                foreach (var qi in data)
                {
                    queryIndexes[qi.Field] = qi.Value;
                }
                var ftoptions = this.ftOptions.FTOptions;
                bool includePreRevisions = this.IncludePreRevisions.IsChecked.HasValue && this.IncludePreRevisions.IsChecked.Value;
                results = await client.ExecuteAdhocCAQQueryAsync(this.CurrentCAQ.ID,
                    queryIndexes, ftoptions, includePreRevisions, Global.MediaType);
            }

            UnregisterClientEvents(client);

            PopulateResultsUI(results);
        }

        private void PopulateResultsUI(AXRESTClientQueryResults results)
        {
            if (results.Columns == null || results.Collection == null ||
              results.Columns.Count == 0 || results.Collection.Count == 0) return;

            this.CurrentPage = results;

            this.btnFirst.IsEnabled = results.HasFirstPage;
            this.btnPrev.IsEnabled = results.HasPreviousPage;
            this.btnNext.IsEnabled = results.HasNextPage;
            this.btnLast.IsEnabled = results.HasLastPage;

            ExtendedDataTable table = new ExtendedDataTable();
            foreach (var col in results.Columns)
            {
                table.Columns.Add(col);
            }
            table.Columns.Add("DocID");
            table.Columns.Add("Page Count");

            foreach (var item in results.Collection)
            {
                ExtendedDataRow dr = table.NewRow() as ExtendedDataRow;
                int i = 0;
                for (; i < results.Columns.Count; i++)
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

            if (resultItem.IsReport)
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

        internal void PopulateCAQFields(AXRESTClientCAQConfig caq, List<AXRESTClientCAQField> list)
        {
            this.CurrentCAQ = caq;
            this.caqfields = list;
            this.cbFields.ItemsSource = this.caqfields;
        }

        public AXRESTClientQueryResults CurrentPage { get; set; }
        public AXRESTClientCAQConfig CurrentCAQ { get; set; }
        private class QueryIndex
        {
            public string Field { get; set; }

            public string Value { get; set; }
        }

    }
}
