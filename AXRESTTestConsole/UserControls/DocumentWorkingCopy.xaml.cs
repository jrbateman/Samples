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
    /// Interaction logic for DocumentWorkingCopy.xaml
    /// </summary>
    public partial class DocumentWorkingCopy : BaseUserControl
    {
        public DocumentWorkingCopy()
        {
            InitializeComponent();
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
            AXRESTClientDoc wpClient = await docClient.CheckOutAsync(this.txtComment.Text, Global.MediaType);
            UnregisterClientEvents(docClient);

            Global.clientCaches["AXRESTClientDoc"] = wpClient;

            PopulateWorkingCopyUI(wpClient);
        }

        private void PopulateWorkingCopyUI(AXRESTClientDoc wpClient)
        {
            this.lbLinks.ItemsSource = new List<string>()
            {
                string.Format("Link: {0}", wpClient.Location),
                string.Format("WorkingCopyOf: {0}", wpClient.WorkingCopyOfLocation),
            };

            MainWindow mainWin = App.Current.MainWindow as MainWindow;
            mainWin.SetStatusBar(currentdoc: wpClient.ID);
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
            AXRESTClientDoc wpClient = await docClient.ResumeCheckOutAsync(Global.MediaType);
            UnregisterClientEvents(docClient);

            Global.clientCaches["AXRESTClientDoc"] = wpClient;

            PopulateWorkingCopyUI2(wpClient);
        }

        private void PopulateWorkingCopyUI2(AXRESTClientDoc wpClient)
        {
            this.lbLinks2.ItemsSource = new List<string>()
            {
                string.Format("Link: {0}", wpClient.Location),
                string.Format("WorkingCopyOf: {0}", wpClient.WorkingCopyOfLocation),
            };

            MainWindow mainWin = App.Current.MainWindow as MainWindow;
            mainWin.SetStatusBar(currentdoc: wpClient.ID);
        }

        public override async Task Delete()
        {
            if (!Global.clientCaches.ContainsKey("AXRESTClientDoc"))
            {
                MessageBox.Show("Please get the Document resource firstly");
                return;
            }
            AXRESTClientDoc docClient = Global.clientCaches["AXRESTClientDoc"] as AXRESTClientDoc;

            RegisterClientEvents(docClient);
            await docClient.CancelCheckOutAsync(Global.MediaType);
            UnregisterClientEvents(docClient);
        }
    }
}
