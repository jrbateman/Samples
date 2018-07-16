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
    /// Interaction logic for CAQ.xaml
    /// </summary>
    public partial class CAQ : BaseUserControl
    {
        public CAQ()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientQuery"))
            {
                MessageBox.Show("Please get the Query resource firstly");
                return;
            }
            AXRESTClientQuery client = Global.clientCaches["AXRESTClientQuery"] as AXRESTClientQuery;

            if (client.QueryType != "CrossAppQuery") return;

            RegisterClientEvents(client);
            AXRESTClientCAQConfig caqClient = await client.GetCAQConfigAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateCAQUI(caqClient);
        }

        private void PopulateCAQUI(AXRESTClientCAQConfig caqClient)
        {
            this.lbCAQ.Items.Clear();
            this.lbCAQ.Items.Add(string.Format("{0}: {1}", "ID", caqClient.ID));
            this.lbCAQ.Items.Add(string.Format("{0}: {1}", "Name", caqClient.Name));
            this.lbCAQ.Items.Add(string.Format("{0}: {1}", "IsPublic", caqClient.IsPublic));
            this.lbCAQ.Items.Add(string.Format("{0}: {1}", "App", string.Join(" ", caqClient.CAQApps)));

            this.dgCAQFields.ItemsSource = caqClient.Fields;


            TreeViewItem item = Global.GetTreeViewItemByName("Application Group", "AdhocQuery Results");
            if (item != null)
            {
                AdhocQueryResults ui = Global.UIDic[item] as AdhocQueryResults;
                ui.PopulateCAQFields(caqClient, caqClient.Fields.Where(f => f.Searchable).ToList());
            }
        }
    }
}
