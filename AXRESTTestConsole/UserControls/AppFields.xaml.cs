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
    /// Interaction logic for AppFields.xaml
    /// </summary>
    public partial class AppFields : BaseUserControl
    {
        public AppFields()
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
            AXRESTClientAppFields fieldsClient = await client.GetAppFieldsAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateFieldsUI(fieldsClient);
        }

        private void PopulateFieldsUI(AXRESTClientAppFields fieldsClient)
        {
            this.dgAppFields.ItemsSource = fieldsClient.Collection;

            //populate select index ui
            TreeViewItem item = Global.GetTreeViewItemByName("Application Group", "Select Indexes");
            if (item != null)
            {
                SelectIndex ui = Global.UIDic[item] as SelectIndex;
                ui.PopulateFields(fieldsClient.Collection);
            }

            //populate auto index ui
            item = Global.GetTreeViewItemByName("Application Group", "Auto Indexes");
            if (item != null)
            {
                AutoIndex ui = Global.UIDic[item] as AutoIndex;
                ui.PopulateFields(fieldsClient.Collection);
            }

            //populate query post ui
            item = Global.GetTreeViewItemByName("Query Group", "Queries");
            if (item != null)
            {
                Queries ui = Global.UIDic[item] as Queries;
                ui.PopulateFields(fieldsClient.Collection);
            }

            //populate query post ui
            item = Global.GetTreeViewItemByName("Application Group", "AdhocQuery Results");
            if (item != null)
            {
                AdhocQueryResults ui = Global.UIDic[item] as AdhocQueryResults;
                ui.PopulateFields(fieldsClient.Collection);
            }

            //populate document post ui
            item = Global.GetTreeViewItemByName("Document Group", "Document");
            if (item != null)
            {
                Document ui = Global.UIDic[item] as Document;
                ui.PopulateFields(fieldsClient.Collection);
            }

            //populate document indexes post ui
            item = Global.GetTreeViewItemByName("Document Group", "Document Indexes");
            if (item != null)
            {
                DocumentIndexes ui = Global.UIDic[item] as DocumentIndexes;
                ui.PopulateFields(fieldsClient.Collection);
            }

            //populate document index put ui
            item = Global.GetTreeViewItemByName("Document Group", "Document Index");
            if (item != null)
            {
                DocumentIndex ui = Global.UIDic[item] as DocumentIndex;
                ui.PopulateFields(fieldsClient.Collection);
            }
        }
    }
}
