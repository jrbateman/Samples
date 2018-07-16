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
    /// Interaction logic for RubberStamps.xaml
    /// </summary>
    public partial class RubberStamps : BaseUserControl
    {
        public RubberStamps()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            RegisterClientEvents(client);
            AXRESTClientRubberStamps rssClient = await client.GetRubberStamps(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateRubberStampsUI(rssClient);

        }

        private void PopulateRubberStampsUI(AXRESTClientRubberStamps rssClient)
        {
            this.dgRubberStamps.ItemsSource = rssClient.Collection;
        }
    }
}
