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
    /// Interaction logic for ODMAProperty.xaml
    /// </summary>
    public partial class ODMAProperty : BaseUserControl
    {
        public ODMAProperty()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the document firstly");
                return;
            }
            AXRESTClientDoc client = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

            RegisterClientEvents(client);
            AXRESTClientDocODMA docODMAClient = await client.GetAXDocODMAAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateDocODMAUI(docODMAClient);
        }

        private void PopulateDocODMAUI(AXRESTClientDocODMA docODMA)
        {
            this.lbDocODMA.Items.Clear();

            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Title", docODMA.Name));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Subject", docODMA.Subject));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Author", docODMA.Author));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Comment", docODMA.Comment));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Created", docODMA.Created));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Creator", docODMA.Creator));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Modified", docODMA.Modified));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Modifier", docODMA.Modifier));
            this.lbDocODMA.Items.Add(string.Format("{0}: {1}", "Keywords", docODMA.Keywords == null ? string.Empty : string.Join(",", docODMA.Keywords)));
        }
    }
}
