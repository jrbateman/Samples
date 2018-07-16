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
    /// Interaction logic for AppAttributes.xaml
    /// </summary>
    public partial class AppAttributes : BaseUserControl
    {
        public AppAttributes()
        {
            InitializeComponent();
        }

        public override async Task Get()
        {

            if (!Global.clientCaches.ContainsKey("AXRESTClientHomeDocument"))
            {
                MessageBox.Show("Please get the Home Document resource firstly");
                return;
            }

            AXRESTClientHomeDocument homeDocClient = Global.clientCaches["AXRESTClientHomeDocument"] as AXRESTClientHomeDocument;

            RegisterClientEvents(homeDocClient);
            AXRESTClientAppAttributesDefinitions appattrDefClient = await homeDocClient.GetAXAppAttributesDefinitionsAsync(Global.MediaType);
            UnregisterClientEvents(homeDocClient);

            PopulateAppAttrDefListBox(appattrDefClient);

        }

        private void PopulateAppAttrDefListBox(AXRESTClientAppAttributesDefinitions appattrDefClient)
        {
            this.lbAppAttributes.Items.Clear();
            foreach (var kvp in appattrDefClient.AllAttributes)
            {
                this.lbAppAttributes.Items.Add(string.Format("{0}: {1}", kvp.Key, kvp.Value));
            }
        }
    }
}
