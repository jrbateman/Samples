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
    /// Interaction logic for DataTypes.xaml
    /// </summary>
    public partial class DataTypes : BaseUserControl
    {
        public DataTypes()
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
            AXRESTClientDataTypes dtsClient = await dsClient.GetDataTypesAsync(Global.MediaType);
            UnregisterClientEvents(dsClient);

            PopulateDataTypesUI(dtsClient);
        }

        private void PopulateDataTypesUI(AXRESTClientDataTypes dtsClient)
        {
            this.dgDataTypes.ItemsSource = dtsClient.Collection;

            TreeViewItem item = Global.GetTreeViewItemByName("DataSource Group", "Data Type");
            if (item != null)
            {
                DataType dtUI = Global.UIDic[item] as DataType;
                dtUI.PopulateDTList(dtsClient.Collection);
            }

            TreeViewItem item2 = Global.GetTreeViewItemByName("DataSource Group", "Data Formats");
            if (item2 != null)
            {
                DataFormats dfsUI = Global.UIDic[item2] as DataFormats;
                dfsUI.PopulateDTList(dtsClient.Collection);
            }
        }

        private void dgDataTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientDataType dt = this.dgDataTypes.SelectedItem as AXRESTClientDataType;

            TreeViewItem item = Global.GetTreeViewItemByName("DataSource Group", "Data Type");
            if (item != null)
            {
                DataType dtUI = Global.UIDic[item] as DataType;
                dtUI.SelectDataType(dt);
                item.IsSelected = true;
            } 
            
        }
    }
}
