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
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : BaseUserControl
    {
        public Report()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            AXRESTClientQueryResultItem client = this.CurrentItem as AXRESTClientQueryResultItem;

            if (client == null) return;

            RegisterClientEvents(client);
            AXRESTClientReportDoc report = await client.GetAXReportDocAsync(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientReportDoc"] = report;

            PopulateReportUI(report);
        }

        public override async Task Delete()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientReportDoc"))
            {
                MessageBox.Show("Please get the report doc resource firstly");
                return;
            }
            AXRESTClientReportDoc client = Global.clientCaches["AXRESTClientReportDoc"] as AXRESTClientReportDoc;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);
        }

        private void PopulateReportUI(AXRESTClientReportDoc report)
        {
            this.lbReportDoc.Items.Clear();

            this.lbReportDoc.Items.Add(string.Format("{0}: {1}", "ID", report.ID));
            this.lbReportDoc.Items.Add(string.Format("{0}: {1}", "Description", report.Description));
            this.lbReportDoc.Items.Add(string.Format("{0}: {1}", "ReportType", report.ReportType));
            this.lbReportDoc.Items.Add(string.Format("{0}: {1}", "TimeStamp", report.TimeStamp));
            this.lbReportDoc.Items.Add(string.Format("{0}: {1}", "PageCount", report.PageCount));
        }

        internal void SetResultItem(AXRESTClientQueryResultItem resultItem)
        {
            this.CurrentItem = resultItem;
        }

        public AXRESTClientQueryResultItem CurrentItem { get; set; }
    }
}
