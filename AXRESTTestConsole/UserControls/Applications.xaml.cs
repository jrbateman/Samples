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
    /// Interaction logic for Applications.xaml
    /// </summary>
    public partial class Applications : BaseUserControl
    {
        public Applications()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDataSource"))
            {
                MessageBox.Show("Please get the DataSource resource firstly");
                return;
            }
            AXRESTClientDataSource dsClient = Global.clientCaches["AXRESTClientDataSource"] as AXRESTClientDataSource;

            RegisterClientEvents(dsClient);
            AXRESTClientApplicationList appsClient = await dsClient.GetApplicationsAsync(Global.MediaType);
            UnregisterClientEvents(dsClient);

            Global.clientCaches["AXRESTClientApplicationList"] = appsClient;

            PopulateAppsUI(appsClient);
        }

        private void PopulateAppsUI(AXRESTClientApplicationList appsClient)
        {
            this.dgApps.ItemsSource = appsClient.Collection;

            //to be done, populate app list on other UIS
            TreeViewItem item = Global.GetTreeViewItemByName("Application Group", "Application");
            if (item != null)
            {
                Application ui = Global.UIDic[item] as Application;
                ui.PopulateAppList(appsClient.Collection);
            }
        }

        private void dgApps_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientApplication selectedItem = this.dgApps.SelectedItem as AXRESTClientApplication;

            TreeViewItem item = Global.GetTreeViewItemByName("Application Group", "Application");
            if (item != null)
            {
                Application ui = Global.UIDic[item] as Application;
                ui.SelectApp(selectedItem);
                item.IsSelected = true;
            }

        }
    }
}
