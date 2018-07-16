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
    /// Interaction logic for HomeDocument.xaml
    /// </summary>
    public partial class HomeDocument : BaseUserControl
    {
        public HomeDocument()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            string serverAddress = this.tbServer.Text.Trim().Trim('/');
            int lastslash = serverAddress.LastIndexOf('/');
            //http://localhost/
            if (lastslash <= 7)
            {
                MessageBox.Show("The server address is not valid");
                return;
            }
            string host = serverAddress.Substring(0, lastslash);
            string appname = serverAddress.Substring(lastslash + 1);

            AXRESTClient client = new AXRESTClient(serverAddress, appname);

            RegisterClientEvents(client);
            AXRESTClientHomeDocument homeDoc = await client.GetHomeDocumentAsync(Global.HomeMediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientHomeDocument"] = homeDoc;
        }

    }
}
