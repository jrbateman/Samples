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
using XtenderSolutions.AXRESTDataModel;

namespace AXRESTTestConsole.UserControls
{
    /// <summary>
    /// Interaction logic for DSQueries.xaml
    /// </summary>
    public partial class DSQueries : BaseUserControl
    {
        public DSQueries()
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
            AXRESTClientDataSource client = Global.clientCaches["AXRESTClientDataSource"] as AXRESTClientDataSource;

            RegisterClientEvents(client);
            AXRESTClientQueries queriesClient = await client.GetQueriesAsync(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateQueriesUI(queriesClient);
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDataSource"))
            {
                MessageBox.Show("Please get the DataSource resource firstly");
                return;
            }
            AXRESTClientDataSource client = Global.clientCaches["AXRESTClientDataSource"] as AXRESTClientDataSource;

            string queryname = this.tbCAQName.Text;
            if (string.IsNullOrEmpty(queryname)) return;

            List<short> appids = new List<short>();
            foreach (TreeViewItem n in this.tvCAQList.Items)
            {
                appids.Add((short)(int)n.Tag);
            }
            if (appids.Count <= 1) return;

            bool ispublic = this.chIsPublic.IsChecked == true;

            Dictionary<string, QueryIndexAttribute> fields = new Dictionary<string, QueryIndexAttribute>();
            foreach (TreeViewItem node in this.tvCAQList.Items)
            {
                bool sfExisted = false;
                foreach (TreeViewItem subnode in node.Items)
                {
                    string header = subnode.Header.ToString();
                    if (header[0] == (char)int.Parse("00D7", System.Globalization.NumberStyles.HexNumber))
                    {
                        //invisible 00D7
                        continue;
                    }
                    else if (header[0] == (char)int.Parse("221A", System.Globalization.NumberStyles.HexNumber))
                    {
                        //displayble 221A
                        fields[subnode.Tag.ToString()] = QueryIndexAttribute.Displayable;
                    }
                    else
                    {
                        //searchable 25CB
                        fields[subnode.Tag.ToString()] = QueryIndexAttribute.Displayable | QueryIndexAttribute.Searchable;
                        sfExisted = true;
                    }
                }

                if (!sfExisted)
                {
                    MessageBox.Show(string.Format("Application {0} does not contain a searchable field", node.Header.ToString()));
                    return;
                }
            }

            RegisterClientEvents(client);
            await client.CreateCAQAsync(queryname, appids.ToArray(), fields, ispublic, Global.MediaType);
            UnregisterClientEvents(client);
        }

        private void PopulateQueriesUI(AXRESTClientQueries queriesClient)
        {
            this.dgQueries.ItemsSource = queriesClient.Collection;

            //Populate Queries list in Query UI
            TreeViewItem item = Global.GetTreeViewItemByName("Query Group", "Query");
            if (item != null)
            {
                Query ui = Global.UIDic[item] as Query;
                ui.PopulateQueryList(queriesClient.Collection);
            }
        }

        private void dgQueries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientQuery selectedItem = this.dgQueries.SelectedItem as AXRESTClientQuery;

            TreeViewItem item = Global.GetTreeViewItemByName("Query Group", "Query");
            if (item != null)
            {
                Query ui = Global.UIDic[item] as Query;
                ui.SelectQuery(selectedItem);
                item.IsSelected = true;
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AXRESTClientApplication app = this.lbAppList.SelectedItem as AXRESTClientApplication;
            if (app == null) return;

            foreach(var node in this.tvCAQList.Items)
            {
                if (((TreeViewItem)node).Header.ToString() == app.Name)
                    return;
            }

            await app.Refresh(Global.MediaType);
            AXRESTClientAppFields fields = await app.GetAppFieldsAsync(Global.MediaType);
            
            TreeViewItem appNode = new TreeViewItem();
            appNode.Header = app.Name;
            appNode.Tag = app.ID;
            appNode.IsExpanded = true;
            this.tvCAQList.Items.Add(appNode);

            foreach (var f in fields.Collection)
            {
                TreeViewItem fNode = new TreeViewItem();
                fNode.Tag = f.Name;
                fNode.Header = string.Format("{0} {1}", (char)int.Parse("00D7", System.Globalization.NumberStyles.HexNumber), f.Name);
                appNode.Items.Add(fNode);
            }
        }

        private void btnremove_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = this.tvCAQList.SelectedItem as TreeViewItem;
            if (selected == null || selected.Items.Count == 0) return;

            this.tvCAQList.Items.Remove(selected);
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tabPost.IsSelected)
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientApplicationList"))
                {
                    MessageBox.Show("Please get the Application list resource firstly");
                    return;
                }

                AXRESTClientApplicationList client = Global.clientCaches["AXRESTClientApplicationList"] as AXRESTClientApplicationList;


                this.lbAppList.ItemsSource = client.Collection;
            }
        }

        private void tvCAQList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selected = this.tvCAQList.SelectedItem as TreeViewItem;

            if (selected == null || selected.Items.Count > 0) return;

            string header = selected.Header.ToString();
            if (header[0] == (char)int.Parse("00D7", System.Globalization.NumberStyles.HexNumber))
            {
                //invisible 00D7
                selected.Header = string.Format("{0} {1}", (char)int.Parse("221A", System.Globalization.NumberStyles.HexNumber), selected.Tag);
            }
            else if (header[0] == (char)int.Parse("221A", System.Globalization.NumberStyles.HexNumber))
            {
                //displayble 221A
                selected.Header = string.Format("{0} {1}", (char)int.Parse("25CB", System.Globalization.NumberStyles.HexNumber), selected.Tag);
            }
            else
            {
                //searchable 25CB
                selected.Header = string.Format("{0} {1}", (char)int.Parse("00D7", System.Globalization.NumberStyles.HexNumber), selected.Tag);
            }

            foreach(TreeViewItem node in this.tvCAQList.Items)
            {
                foreach (TreeViewItem subnode in node.Items)
                {
                    if (subnode != selected && string.Compare(subnode.Tag.ToString(), selected.Tag.ToString(), false) == 0)
                    {
                        subnode.Header = selected.Header;
                    }
                }
            }

        }

    }
}
