using System;
using System.Collections.Generic;
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
    /// Interaction logic for ReportPages.xaml
    /// </summary>
    public partial class ReportPages : BaseUserControl
    {
        public ReportPages()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientReportDoc"))
            {
                MessageBox.Show("Please get the report doc resource firstly");
                return;
            }
            AXRESTClientReportDoc client = Global.clientCaches["AXRESTClientReportDoc"] as AXRESTClientReportDoc;

            RegisterClientEvents(client);
            AXRESTClientReportDocPages pagesClient = await client.GetAXReportDocPagesAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulatePagesUI(pagesClient);
        }

        private void dgReportPages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientReportDocPage selectedItem = this.dgReportPages.SelectedItem as AXRESTClientReportDocPage;

            TreeViewItem item = Global.GetTreeViewItemByName("Report Group", "Report Page");
            if (item != null)
            {
                ReportPage ui = Global.UIDic[item] as ReportPage;
                ui.SetReportPage(selectedItem);
                item.IsSelected = true;
            }

        }

        private void PopulatePagesUI(AXRESTClientReportDocPages pagesClient)
        {
            this.CurrentPage = pagesClient;

            this.btnFirst.IsEnabled = pagesClient.HasFirstPage;
            this.btnPrev.IsEnabled = pagesClient.HasPreviousPage;
            this.btnNext.IsEnabled = pagesClient.HasNextPage;
            this.btnLast.IsEnabled = pagesClient.HasLastPage;

            this.dgReportPages.ItemsSource = pagesClient.Collection;
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientReportDocPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientReportDocPages pagesClient = await client.GetFirstPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulatePagesUI(pagesClient);
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

            AXRESTClientReportDocPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientReportDocPages pagesClient = await client.GetPreviousPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulatePagesUI(pagesClient);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientReportDocPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientReportDocPages pagesClient = await client.GetNextPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulatePagesUI(pagesClient);
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentPage == null) return;

            AXRESTClientReportDocPages client = this.CurrentPage;

            RegisterClientEvents(client);
            AXRESTClientReportDocPages pagesClient = await client.GetLastPageAsync(Global.MediaType);
            UnregisterClientEvents(client);
            UpdateMainWindow(this.TimeStart, this.TimeCost, this.Request, this.Response);

            PopulatePagesUI(pagesClient);
        }

        public AXRESTClientReportDocPages CurrentPage { get; set; }
    }
}
