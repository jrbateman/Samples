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
    /// Interaction logic for Batch.xaml
    /// </summary>
    public partial class Batch : BaseUserControl
    {
        public Batch()
        {
            InitializeComponent();
        }

        internal void PopulateBatchList(List<AXRESTClientBatch> list)
        {
            this.cbBatches.ItemsSource = list;
        }

        internal void SelectBatch(AXRESTClientBatch selectedItem)
        {
            this.cbBatches.SelectedItem = selectedItem;
        }

        public override async Task Get()
        {
            AXRESTClientBatch client = this.cbBatches.SelectedItem as AXRESTClientBatch;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            Global.clientCaches["AXRESTClientBatch"] = client;

            PopulateBatchUI(client);
        }

        public override async Task Put()
        {
            AXRESTClientBatch client = this.cbBatches.SelectedItem as AXRESTClientBatch;
            if (client == null) return;

            string name = this.tbBatchUpdateName.Text;
            string description = this.tbBatchUpdateDescription.Text;

            if (string.IsNullOrEmpty(name)) return;

            RegisterClientEvents(client);
            await client.UpdateAsync(name, description, Global.MediaType);
            UnregisterClientEvents(client);

        }

        public override async Task Delete()
        {
            AXRESTClientBatch client = this.cbBatches.SelectedItem as AXRESTClientBatch;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);

            List<AXRESTClientBatch> list = this.cbBatches.ItemsSource as List<AXRESTClientBatch>;
            list.Remove(client);
            this.cbBatches.ItemsSource = list;

            if (list.Count > 0)
                this.cbBatches.SelectedItem = list.First();
            else
                this.cbBatches.SelectedItem = null;
        }

        private void PopulateBatchUI(AXRESTClientBatch client)
        {
            this.CurrentBatch = client;

            this.lbBatch.Items.Clear();

            this.lbBatch.Items.Add(string.Format("{0}:{1}", "ID", client.ID.ToString()));
            this.lbBatch.Items.Add(string.Format("{0}:{1}", "Name", client.Name));
            this.lbBatch.Items.Add(string.Format("{0}:{1}", "Description", client.Description));
            this.lbBatch.Items.Add(string.Format("{0}:{1}", "PageCount", client.PageCount.ToString()));
            this.lbBatch.Items.Add(string.Format("{0}:{1}", "Private", client.Private.ToString()));
            this.lbBatch.Items.Add(string.Format("{0}:{1}", "State", client.State));
        }

        private void btnThumbnail_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentBatch == null)
            {
                MessageBox.Show("Please get the batch resource firstly");
                return;
            }

            TreeViewItem item = Global.GetTreeViewItemByName("Render Group", "Thumbnails");
            if (item != null)
            {
                Thumbnails ui = Global.UIDic[item] as Thumbnails;
                ui.SetSource(1, this.CurrentBatch);
                item.IsSelected = true;
            }
        }

        public AXRESTClientBatch CurrentBatch { get; set; }
    }
}
