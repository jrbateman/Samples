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
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : BaseUserControl
    {
        public ReportPage()
        {
            InitializeComponent();
        }

        internal void SetReportPage(AXRESTClientReportDocPage selectedItem)
        {
            this.CurrentPage = selectedItem;
            PopulatePageUI(selectedItem);
        }


        public override async Task Get()
        {

            AXRESTClientReportDocPage client = this.CurrentPage as AXRESTClientReportDocPage;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            PopulatePageUI(client);
        }

        private void PopulatePageUI(AXRESTClientReportDocPage client)
        {
            this.tbPageNumber.Text = client.PageNumber.ToString();
            this.tbLoc.Text = client.Location;

            this.CurrentReportPage = client;
        }


        public AXRESTClientReportDocPage CurrentPage { get; set; }

        private void btnRender_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentReportPage == null)
            {
                MessageBox.Show("Please get the report page resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Rendition");
            if (item != null)
            {
                Rendition ui = Global.UIDic[item] as Rendition;
                ui.SetSource(2, this.CurrentReportPage);
                item.IsSelected = true;
            }
        }

        public AXRESTClientReportDocPage CurrentReportPage { get; set; }
    }
}
