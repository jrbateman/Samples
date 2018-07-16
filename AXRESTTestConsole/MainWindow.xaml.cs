using AXRESTTestConsole.UserControls;
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

namespace AXRESTTestConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            var dsGroup = InitializeNavTreeNode(null, "DataSource Group");
            InitUI(InitializeNavTreeNode(dsGroup, "Home Document"), new HomeDocument());
            InitUI(InitializeNavTreeNode(dsGroup, "Data Sources"), new DataSources());
            InitUI(InitializeNavTreeNode(dsGroup, "Data Source"), new DataSource());
            InitUI(InitializeNavTreeNode(dsGroup, "Current User"), new User("Current User"));
            InitUI(InitializeNavTreeNode(dsGroup, "FormOverlay"), new FormOverlays());
            InitUI(InitializeNavTreeNode(dsGroup, "ODMAQuery Fields"), new QueryFields("ODMADef"));
            InitUI(InitializeNavTreeNode(dsGroup, "Data Source Queries"), new DSQueries());
            InitUI(InitializeNavTreeNode(dsGroup, "Data Types"), new DataTypes());
            InitUI(InitializeNavTreeNode(dsGroup, "Data Type"), new DataType());
            InitUI(InitializeNavTreeNode(dsGroup, "Data Formats"), new AXRESTTestConsole.UserControls.DataFormats());
            InitUI(InitializeNavTreeNode(dsGroup, "Data Format"), new AXRESTTestConsole.UserControls.DataFormat());
            InitUI(InitializeNavTreeNode(dsGroup, "Permission"), new Permissions());
            InitUI(InitializeNavTreeNode(dsGroup, "Application Attributes"), new AppAttributes());

            var appGroup = InitializeNavTreeNode(null, "Application Group");
            InitUI(InitializeNavTreeNode(appGroup, "Applications"), new Applications());
            InitUI(InitializeNavTreeNode(appGroup, "Application"), new AXRESTTestConsole.UserControls.Application());
            InitUI(InitializeNavTreeNode(appGroup, "Application Fields"), new AppFields());
            InitUI(InitializeNavTreeNode(appGroup, "AdhocQuery Results"), new AdhocQueryResults());
            InitUI(InitializeNavTreeNode(appGroup, "RubberStamp"), new RubberStamps());
            InitUI(InitializeNavTreeNode(appGroup, "Key Reference Indexes"), new KeyRef());
            InitUI(InitializeNavTreeNode(appGroup, "Auto Indexes"), new AutoIndex());
            InitUI(InitializeNavTreeNode(appGroup, "Select Indexes"), new SelectIndex());

            var qryGroup = InitializeNavTreeNode(null, "Query Group");
            InitUI(InitializeNavTreeNode(qryGroup, "Queries"), new Queries());
            InitUI(InitializeNavTreeNode(qryGroup, "Query"), new Query());
            InitUI(InitializeNavTreeNode(qryGroup, "Query Fields"), new QueryFields("QueryFields"));
            InitUI(InitializeNavTreeNode(qryGroup, "ODMAQuery Fields"), new QueryFields("ODMAQueryFields"));
            InitUI(InitializeNavTreeNode(qryGroup, "FullText"), new FullTextQuery());
            InitUI(InitializeNavTreeNode(qryGroup, "CAQ"), new CAQ());
            InitUI(InitializeNavTreeNode(qryGroup, "Query Creator"), new User("Query Creator"));
            InitUI(InitializeNavTreeNode(qryGroup, "Query Results"), new QueryResults());

            var batchGroup = InitializeNavTreeNode(null, "Batch Group");
            InitUI(InitializeNavTreeNode(batchGroup, "Batches"), new Batches());
            InitUI(InitializeNavTreeNode(batchGroup, "Batch"), new Batch());
            InitUI(InitializeNavTreeNode(batchGroup, "Batch Creator"), new User("Batch Creator"));
            InitUI(InitializeNavTreeNode(batchGroup, "Batch Pages"), new BatchPages());
            InitUI(InitializeNavTreeNode(batchGroup, "Batch Page"), new BatchPage());

            var docGroup = InitializeNavTreeNode(null, "Document Group");
            InitUI(InitializeNavTreeNode(docGroup, "Document"), new Document());
            InitUI(InitializeNavTreeNode(docGroup, "Document Indexes"), new DocumentIndexes());
            InitUI(InitializeNavTreeNode(docGroup, "Document Index"), new DocumentIndex());
            InitUI(InitializeNavTreeNode(docGroup, "ODMA Properties"), new ODMAProperty());
            InitUI(InitializeNavTreeNode(docGroup, "Document Pages"), new DocumentPages());
            InitUI(InitializeNavTreeNode(docGroup, "Document Page"), new AXRESTTestConsole.UserControls.DocumentPage());
            InitUI(InitializeNavTreeNode(docGroup, "Document PageVersions"), new DocumentPageVersions());
            InitUI(InitializeNavTreeNode(docGroup, "Document PageVersion"), new DocumentPageVersion());
            InitUI(InitializeNavTreeNode(docGroup, "Document Revisions"), new DocumentRevisions());
            InitUI(InitializeNavTreeNode(docGroup, "Document Revision"), new DocumentRevision());
            InitUI(InitializeNavTreeNode(docGroup, "Document Working Copy"), new DocumentWorkingCopy());

            var reportGroup = InitializeNavTreeNode(null, "Report Group");
            InitUI(InitializeNavTreeNode(reportGroup, "Report"), new Report());
            InitUI(InitializeNavTreeNode(reportGroup, "Report Pages"), new ReportPages());
            InitUI(InitializeNavTreeNode(reportGroup, "Report Page"), new ReportPage());

            var renderGroup = InitializeNavTreeNode(null, "Render Group");
            InitUI(InitializeNavTreeNode(renderGroup, "Thumbnails"), new Thumbnails());
            InitUI(InitializeNavTreeNode(renderGroup, "Rendition"), new Rendition());
        }

        private void InitUI(TreeViewItem treeViewItem, UserControl userControl)
        {
            Global.UIDic[treeViewItem] = userControl;
        }

        private TreeViewItem InitializeNavTreeNode(TreeViewItem parent, string name)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = name;
            item.IsExpanded = true;
            item.Tag = parent;
            if (parent == null)
            {
                //group 
                this.tvNavigation.Items.Add(item);
            }
            else
            {
                parent.Items.Add(item);
            }
            return item;
        }

        private void tvNavigation_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //there is a request in processing
            if (!this.btnInvoke.IsEnabled) return;

            TreeViewItem selectedItem = this.tvNavigation.SelectedItem as TreeViewItem;
            if (selectedItem == null)
            {
                return;
            }
            if (selectedItem.Items.Count > 0)
            {
                TreeViewItem firstChild = selectedItem.Items[0] as TreeViewItem;
                firstChild.IsSelected = true;
            }
            else
            {
                DisplayUserControl(selectedItem);
            }
        }

        private void DisplayUserControl(TreeViewItem selectedItem)
        {
            this.panelUCContainer.Content = Global.UIDic[selectedItem];

            this.tbRequestStart.Text = string.Empty;
            this.tbRequestTime.Text = string.Empty;
            this.tbRequestContent.Text = string.Empty;
            this.tbResponseContent.Text = string.Empty;
        }

        private async void btnInvoke_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btnInvoke.IsEnabled = false;

                BaseUserControl current = this.panelUCContainer.Content as BaseUserControl;

                if (current == null)
                    return;

                switch (current.HttpAction)
                {
                    case HttpActions.GET:
                        await current.Get();
                        break;
                    case HttpActions.POST:
                        await current.Post();
                        break;
                    case HttpActions.PUT:
                        await current.Put();
                        break;
                    case HttpActions.DELETE:
                        await current.Delete();
                        break;
                }

                this.tbRequestStart.Text = current.TimeStart;
                this.tbRequestTime.Text = current.TimeCost;

                this.tbRequestContent.Text = current.Request;
                this.tbResponseContent.Text = current.Response;
            }
            finally
            {
                this.btnInvoke.IsEnabled = true;
            }
        }

        public void SetStatusBar(string currentds = null, string currentapp = null, uint? currentdoc = null, string currentquery = null)
        {
            if (!string.IsNullOrEmpty(currentds))
            {
                this.tbDS.Text = string.Format("Data Source: {0}", currentds);
            }

            if (!string.IsNullOrEmpty(currentapp))
            {
                this.tbApp.Text = string.Format("Application: {0}", currentapp);
            }

            if (currentdoc != null)
            {
                this.tbDoc.Text = string.Format("Document: {0}", currentdoc.Value);
            }

            if (currentquery != null)
            {
                this.tbQuery.Text = string.Format("Query: {0}", currentquery);
            }
        }
    }
}
