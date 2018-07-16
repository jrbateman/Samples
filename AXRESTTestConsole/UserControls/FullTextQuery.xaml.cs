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
    /// Interaction logic for FullTextQuery.xaml
    /// </summary>
    public partial class FullTextQuery : BaseUserControl
    {
        public FullTextQuery()
        {
            InitializeComponent();
     
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientQuery"))
            {
                MessageBox.Show("Please get the Quert resource firstly");
                return;
            }
            AXRESTClientQuery client = Global.clientCaches["AXRESTClientQuery"] as AXRESTClientQuery;

            RegisterClientEvents(client);
            AXRESTClientFullTextQuery ftClient = await client.GetFullTextQueryDefinitionAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateFTUI(ftClient);
        }

        private void PopulateFTUI(AXRESTClientFullTextQuery ftClient)
        {
            this.lbFullText.Items.Clear();

            this.lbFullText.Items.Add(string.Format("{0}: {1}", "Query Operator", ftClient.FTQueryOperator));
            this.lbFullText.Items.Add(string.Format("{0}: {1}", "Query Expression", ftClient.FTQueryExpression));
            this.lbFullText.Items.Add(string.Format("{0}: {1}", "Query Value", ftClient.FTQueryValue));
            this.lbFullText.Items.Add(string.Format("{0}: {1}", "Thesaurus", ftClient.FTQueryOptions.ContainsKey("Thesaurus")));
        }
    }

}
