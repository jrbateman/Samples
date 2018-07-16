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
    /// Interaction logic for FormOverlays.xaml
    /// </summary>
    public partial class FormOverlays : BaseUserControl
    {
        public FormOverlays()
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
            AXRESTClientFormOverlays fosClient = await dsClient.GetFormOverlay(Global.MediaType);
            UnregisterClientEvents(dsClient);

            PopulateFOSUI(fosClient);

        }

        private void PopulateFOSUI(AXRESTClientFormOverlays fosClient)
        {
            this.dgFormOverlays.ItemsSource = fosClient.Collection;
        }
    }
}
