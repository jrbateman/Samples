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
    /// Interaction logic for DataFormats.xaml
    /// </summary>
    public partial class DataFormats : BaseUserControl
    {
        public DataFormats()
        {
            InitializeComponent();
        }

        internal void PopulateDTList(List<AXRESTClientDataType> collection)
        {
            this.cbDataTypes.ItemsSource = collection;
        }

        public override async Task Get()
        {
            AXRESTClientDataType dtClient = this.cbDataTypes.SelectedItem as AXRESTClientDataType;
            if (dtClient == null) return;

            RegisterClientEvents(dtClient);
            AXRESTClientDataFormats dfsClient = await dtClient.GetDataFormatsAsync(Global.MediaType);
            UnregisterClientEvents(dtClient);

            if(dfsClient == null)
            {
                MessageBox.Show("This data type does not have any format!");
                return;
            }
            PopulateDFSUI(dfsClient);
        }

        private void PopulateDFSUI(AXRESTClientDataFormats dfsClient)
        {
            this.dgDataFormats.ItemsSource = dfsClient.Collection;

            TreeViewItem item = Global.GetTreeViewItemByName("DataSource Group", "Data Format");
            if (item != null)
            {
                DataFormat dfUI = Global.UIDic[item] as DataFormat;
                dfUI.PopulateDFList(dfsClient.Collection);
            }

        }


        private void dgDataFormats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientDataFormat df = this.dgDataFormats.SelectedItem as AXRESTClientDataFormat;

            TreeViewItem item = Global.GetTreeViewItemByName("DataSource Group", "Data Format");
            if (item != null)
            {
                DataFormat dfUI = Global.UIDic[item] as DataFormat;
                dfUI.SelectDataFormat(df);
                item.IsSelected = true;
            }

        }
    }
}
