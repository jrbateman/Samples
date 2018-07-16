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
    /// Interaction logic for DataSources.xaml
    /// </summary>
    public partial class DataSources : BaseUserControl
    {
        public DataSources()
        {
            InitializeComponent();
        }
        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientHomeDocument"))
            {
                MessageBox.Show("Please get the Home Document resource firstly");
                return;
            }

            AXRESTClientHomeDocument homeDocClient = Global.clientCaches["AXRESTClientHomeDocument"] as AXRESTClientHomeDocument;

            RegisterClientEvents(homeDocClient);
            AXRESTClientDataSourceList dsListClient = await homeDocClient.GetDataSourceListAsync(Global.MediaType);
            UnregisterClientEvents(homeDocClient);

            Global.clientCaches["AXRESTClientDataSourceList"] = dsListClient;

            PopulateDSListBox(dsListClient);
        }

        private void PopulateDSListBox(AXRESTClientDataSourceList dsListClient)
        {
            this.lbDataSources.ItemsSource = dsListClient.Collection.Keys;

            TreeViewItem item = Global.GetTreeViewItemByName("DataSource Group", "Data Source");
            if (item == null) return;

            DataSource dsUI = Global.UIDic[item] as DataSource;
            List<string> dsList = new List<string>();
            foreach (var ds in this.lbDataSources.ItemsSource)
            {
                dsList.Add(ds.ToString());
            }
            dsUI.PopulateDSList(dsList);
        }

        private void lbDbClicked(object sender, MouseButtonEventArgs e)
        {
            string dsname = this.lbDataSources.SelectedValue.ToString();

            if (string.IsNullOrEmpty(dsname)) return;

            TreeViewItem item = Global.GetTreeViewItemByName("DataSource Group", "Data Source");
            if (item == null) return;

            DataSource dsUI = Global.UIDic[item] as DataSource;
            dsUI.SelectDS(dsname);

            item.IsSelected = true;
        }
    }
}
