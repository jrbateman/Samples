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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : BaseUserControl
    {
        //Current User
        //Batch Creator
        //Query Creator
        private string original;
        public User(string orginal)
        {
            InitializeComponent();
            this.original = orginal;
        }

        public override async Task Get()
        {
            AXRESTClientUser userClient;
            if (original == "Current User")
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientDataSource"))
                {
                    MessageBox.Show("Please get the DataSource resource firstly");
                    return;
                }
                AXRESTClientDataSource dsClient = Global.clientCaches["AXRESTClientDataSource"] as AXRESTClientDataSource;

                RegisterClientEvents(dsClient);
                userClient = await dsClient.GetCurrentUserAsync(Global.MediaType);
                UnregisterClientEvents(dsClient);

            }
            else if (original == "Batch Creator")
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientBatch"))
                {
                    MessageBox.Show("Please get the batch resource firstly");
                    return;
                }
                AXRESTClientBatch batchClient = Global.clientCaches["AXRESTClientBatch"] as AXRESTClientBatch;

                RegisterClientEvents(batchClient);
                userClient = await batchClient.GetCreatorAsync(Global.MediaType);
                UnregisterClientEvents(batchClient);
            }
            else if (original == "Query Creator")
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientQuery"))
                {
                    MessageBox.Show("Please get the query resource firstly");
                    return;
                }
                AXRESTClientQuery queryClient = Global.clientCaches["AXRESTClientQuery"] as AXRESTClientQuery;

                RegisterClientEvents(queryClient);
                userClient = await queryClient.GetCreatorAsync(Global.MediaType);
                UnregisterClientEvents(queryClient);
            }
            else
            {
                return;
            }

            PopulateUserUI(userClient);
        }

        private void PopulateUserUI(AXRESTClientUser userClient)
        {
            this.tbUserID.Text = userClient.ID.ToString();
            this.tbUserName.Text = userClient.Name;
            this.tbUserFullName.Text = userClient.FullName;

            this.lbPerms.Items.Clear();

            foreach (string p in userClient.Permissions)
            {
                this.lbPerms.Items.Add(p);
            }
        }
    }
}
