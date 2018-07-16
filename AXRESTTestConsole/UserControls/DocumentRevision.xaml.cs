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
    /// Interaction logic for DocumentRevision.xaml
    /// </summary>
    public partial class DocumentRevision : BaseUserControl
    {
        public DocumentRevision()
        {
            InitializeComponent();
        }

        internal void PopulateDocRevisionList(List<AXRESTClientDocRevision> list)
        {
            this.cbDocRevisions.ItemsSource = list;
        }

        internal void SelectDocRevision(AXRESTClientDocRevision selectedItem)
        {
            this.cbDocRevisions.SelectedItem = selectedItem;
        }

        public override async Task Get()
        {
            AXRESTClientDocRevision client = this.cbDocRevisions.SelectedItem as AXRESTClientDocRevision;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.Refresh(Global.MediaType);
            UnregisterClientEvents(client);

            PopulateDocRevisionUI(client);
        }

        private void PopulateDocRevisionUI(AXRESTClientDocRevision client)
        {
            this.lbLinks.ItemsSource = new List<string>()
            {
                string.Format("RevisionNumber: {0}", client.RevisionNumber),
                string.Format("CheckinBy: {0}", client.CheckinBy),
                string.Format("CheckinDate: {0}", client.CheckinDate),
                string.Format("CheckinComment: {0}", client.CheckinComment),
                string.Format("RevisionDocument: {0}", client.RevisionDocumentLocation),
            };
        }

        public override async Task Delete()
        {
            AXRESTClientDocRevision client = this.cbDocRevisions.SelectedItem as AXRESTClientDocRevision;
            if (client == null) return;

            RegisterClientEvents(client);
            await client.DeleteAsync(Global.MediaType);
            UnregisterClientEvents(client);
        }
    }
}
