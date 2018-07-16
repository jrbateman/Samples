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
    /// Interaction logic for KeyRef.xaml
    /// </summary>
    public partial class KeyRef : BaseUserControl
    {
        public KeyRef()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (string.IsNullOrEmpty(this.tbKeyFieldValue.Text)) return;

            if (!Global.clientCaches.ContainsKey("AXRESTClientApplication"))
            {
                MessageBox.Show("Please get the Application resource firstly");
                return;
            }
            AXRESTClientApplication client = Global.clientCaches["AXRESTClientApplication"] as AXRESTClientApplication;

            RegisterClientEvents(client);
            AXRESTClientKeyReferenceLookupResults krClient = await client.GetKeyReferenceLookupResultsAsync(this.tbKeyFieldValue.Text, Global.MediaType);
            UnregisterClientEvents(client);

            PopulateKeyRefUI(krClient);
        }

        private void PopulateKeyRefUI(AXRESTClientKeyReferenceLookupResults krClient)
        {
            this.lbKeyRef.Items.Clear();
            foreach (var kvp in krClient.IndexValues)
            {
                this.lbKeyRef.Items.Add(string.Format("{0}: {1}", kvp.Key, kvp.Value));
            }
        }
    }
}
