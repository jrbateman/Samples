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
    /// Interaction logic for DataFormat.xaml
    /// </summary>
    public partial class DataFormat : BaseUserControl
    {
        public DataFormat()
        {
            InitializeComponent();
        }

        internal void SelectDataFormat(AXRESTClientDataFormat df)
        {
            this.cbDataFormats.SelectedItem = df;
        }

        internal void PopulateDFList(List<AXRESTClientDataFormat> list)
        {
            this.cbDataFormats.ItemsSource = list;
        }

        public override async Task Get()
        {
            AXRESTClientDataFormat dfClient = this.cbDataFormats.SelectedItem as AXRESTClientDataFormat;
            if (dfClient == null) return;

            RegisterClientEvents(dfClient);
            dfClient = await dfClient.Refresh(Global.MediaType);
            UnregisterClientEvents(dfClient);

            PopulateDFUI(dfClient);
        }

        private void PopulateDFUI(AXRESTClientDataFormat dfClient)
        {
            this.lbDataFormat.Items.Clear();
            this.lbDataFormat.Items.Add(string.Format("{0}: {1}", "ID", dfClient.ID));
            this.lbDataFormat.Items.Add(string.Format("{0}: {1}", "Name", dfClient.Name));
            this.lbDataFormat.Items.Add(string.Format("{0}: {1}", "FormatWidth", dfClient.FormatWidth));
            this.lbDataFormat.Items.Add(string.Format("{0}: {1}", "IsCustom", dfClient.IsCustom));

        }
    }
}
