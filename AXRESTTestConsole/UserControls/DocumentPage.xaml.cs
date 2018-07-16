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
    /// Interaction logic for DocumentPage.xaml
    /// </summary>
    public partial class DocumentPage : BaseUserControl
    {
        public DocumentPage()
        {
            InitializeComponent();
        }

        internal void PopulateDocPageList(List<AXRESTClientDocPage> list)
        {
            this.cbDocPages.ItemsSource = list;
        }

        internal void SelectDocPage(AXRESTClientDocPage selectedItem)
        {
            this.cbDocPages.SelectedItem = selectedItem;
        }

        private void PopulateDocPageUI(AXRESTClientDocPage docPage)
        {
            this.lbLinks.ItemsSource = new List<string>()
            {
                string.Format("Link: {0}", docPage.Location),
                string.Format("CurrentVersion: {0}", docPage.CurrentVersionLocation),
                string.Format("VersionHistory: {0}", docPage.VersionHistoryLocation),
            };
        }

        public override async Task Get()
        {
            AXRESTClientDocPage client = this.cbDocPages.SelectedItem as AXRESTClientDocPage;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientDocPage"] = client;

            PopulateDocPageUI(client);
        }

        private void cbDocPages_Selected(object sender, RoutedEventArgs e)
        {
            AXRESTClientDocPage selectedItem = this.cbDocPages.SelectedItem as AXRESTClientDocPage;
            if (selectedItem != null)
            {
                SelectDocPage(selectedItem);
            }
        }

        private void lbLinks_Selected(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.lbLinks.SelectedIndex;

            switch (selectedIndex)
            {
                case 1: // current version
                    {
                        TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document PageVersion");
                        if (item != null)
                        {
                            DocumentPageVersion ui = Global.UIDic[item] as DocumentPageVersion;
                            ui.ShowCurrentDocPageVersion(this.cbDocPages.SelectedItem as AXRESTClientDocPage);
                            item.IsSelected = true;
                        }
                    }
                    break;
                case 2: // version history
                    {
                        TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document PageVersions");
                        if (item != null)
                        {
                            DocumentPageVersions ui = Global.UIDic[item] as DocumentPageVersions;
                            item.IsSelected = true;
                        }
                    }
                    break;
            }
        }

        public override async Task Delete()
        {
            AXRESTClientDocPage client = this.cbDocPages.SelectedItem as AXRESTClientDocPage;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);
        }
    }
}
