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
    /// Interaction logic for Permissions.xaml
    /// </summary>
    public partial class Permissions : BaseUserControl
    {
        public Permissions()
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
            AXRESTClientPermissionDefinitions permDefClient = await homeDocClient.GetAXPermissionDefinitionsAsync(Global.MediaType);
            UnregisterClientEvents(homeDocClient);

            PopulatePermDefListBox(permDefClient);

        }

        private void PopulatePermDefListBox(AXRESTClientPermissionDefinitions permDefClient)
        {
            this.lbPermDef.Items.Clear();
            foreach (var kvp in permDefClient.AllPermissions)
            {
                this.lbPermDef.Items.Add(string.Format("{0}: {1}", kvp.Key, kvp.Value));
            }
        }
    }
}
