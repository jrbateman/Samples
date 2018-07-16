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
    /// Interaction logic for DataType.xaml
    /// </summary>
    public partial class DataType : BaseUserControl
    {
        public DataType()
        {
            InitializeComponent();
        }

        internal void PopulateDTList(List<AXRESTClientDataType> collection)
        {
            this.cbDataTypes.ItemsSource = collection;
        }

        internal void SelectDataType(AXRESTClientDataType dt)
        {
            this.cbDataTypes.SelectedItem = dt;
            
        }

        public override async Task Get()
        {
            AXRESTClientDataType dtClient = this.cbDataTypes.SelectedItem as AXRESTClientDataType;
            if (dtClient == null) return;

            RegisterClientEvents(dtClient);
            dtClient = await dtClient.Refresh(Global.MediaType);
            UnregisterClientEvents(dtClient);

            PopulateDTUI(dtClient);
        }

        private void PopulateDTUI(AXRESTClientDataType dtClient)
        {
            this.lbDataType.Items.Clear();
            this.lbDataType.Items.Add(string.Format("{0}: {1}", "ID", dtClient.ID));
            this.lbDataType.Items.Add(string.Format("{0}: {1}", "Name", dtClient.Name));
            this.lbDataType.Items.Add(string.Format("{0}: {1}", "IsCustom", dtClient.IsCustom));

        }
    }
}
