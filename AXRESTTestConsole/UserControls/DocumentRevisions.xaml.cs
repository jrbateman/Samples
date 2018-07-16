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
    /// Interaction logic for DocumentRevisions.xaml
    /// </summary>
    public partial class DocumentRevisions : BaseUserControl
    {
        public DocumentRevisions()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the Document resource firstly");
                return;
            }
            AXRESTClientDoc docClient = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

            RegisterClientEvents(docClient);
            AXRESTClientDocRevisions revsClient = await docClient.GetDocRevisionsAsync(Global.MediaType);
            UnregisterClientEvents(docClient);

            PopulateDocumentRevisionsUI(revsClient);
        }

        private void PopulateDocumentRevisionsUI(AXRESTClientDocRevisions revsClient)
        {
            if (revsClient == null) return;

            this.txtCurrentRevision.Text = revsClient.CurrentDocRevision;
            this.dgDocRevisions.ItemsSource = revsClient.Revisions;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document Revision");
            if (item != null)
            {
                DocumentRevision docRevUI = Global.UIDic[item] as DocumentRevision;
                docRevUI.PopulateDocRevisionList(revsClient.Revisions);
            }
        }

        private void dgDocRevisions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AXRESTClientDocRevision selectedItem = this.dgDocRevisions.SelectedItem as AXRESTClientDocRevision;

            TreeViewItem item = Global.GetTreeViewItemByName("Document Group", "Document Revision");
            if (item != null)
            {
                DocumentRevision ui = Global.UIDic[item] as DocumentRevision;
                ui.SelectDocRevision(selectedItem);
                item.IsSelected = true;
            }
        }

        public override async Task Post()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the Document resource firstly");
                return;
            }
            AXRESTClientDoc docClient = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

            RegisterClientEvents(docClient);
            AXRESTClientDocRevisions revsClient = await docClient.CheckInAsync(this.cbAction.SelectedIndex + 1, 
                this.chIsFinal.IsChecked.Value, this.txtComment.Text, this.chFulltext.IsChecked.Value, Global.MediaType);
            UnregisterClientEvents(docClient);

            PopulateDocumentRevisionsUI(revsClient);
        }
    }
}
