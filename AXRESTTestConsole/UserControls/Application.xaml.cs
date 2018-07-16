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
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class Application : BaseUserControl
    {
        public Application()
        {
            InitializeComponent();
        }

        internal void PopulateAppList(List<AXRESTClientApplication> list)
        {
            this.cbApps.ItemsSource = list;
        }

        internal void SelectApp(AXRESTClientApplication selectedItem)
        {
            this.cbApps.SelectedItem = selectedItem;
        }

        public override async Task Get()
        {
            AXRESTClientApplication appClient = this.cbApps.SelectedItem as AXRESTClientApplication;
            if (appClient == null) return;

            RegisterClientEvents(appClient);
            await appClient.Refresh(Global.MediaType);
            UnregisterClientEvents(appClient);

            Global.clientCaches["AXRESTClientApplication"] = appClient;

            PopulateAppUI(appClient);
        }

        private void PopulateAppUI(AXRESTClientApplication appClient)
        {
            this.tbID.Text = appClient.ID.ToString();
            this.tbName.Text = appClient.Name;
            this.tbDescription.Text = appClient.Description;

            this.lbPerms.ItemsSource = appClient.Permissions;
            this.lbAttrs.ItemsSource = appClient.Attributes;

            MainWindow mainWin = App.Current.MainWindow as MainWindow;
            mainWin.SetStatusBar(currentapp: appClient.ToString());
        }
    }
}
