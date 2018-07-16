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
    /// Interaction logic for DataSource.xaml
    /// </summary>
    public partial class DataSource : BaseUserControl
    {
        public DataSource()
        {
            InitializeComponent();
        }

        public void PopulateDSList(List<string> dslist)
        {
            this.cbDataSources.ItemsSource = dslist;
        }

        public void SelectDS(string dsname)
        {
            this.cbDataSources.SelectedValue = dsname;
        }

        public override async Task Get()
        {
            
            if (!Global.clientCaches.ContainsKey("AXRESTClientDataSourceList"))
            {
                MessageBox.Show("Please get the DataSources resource firstly");
                return;
            }
            string dsname = this.cbDataSources.SelectedValue.ToString();
            string username = this.tbUser.Text;
            string password = this.tbPwd.Password;
            XtenderSolutions.AXRESTClient.AXRESTOptions.AuthModes authmode = AXRESTOptions.AuthModes.CM;
            if (this.chkbWinAuth.IsChecked.HasValue && this.chkbWinAuth.IsChecked.Value)
            {
                authmode = AXRESTOptions.AuthModes.WindowsIntegrated;
                username = string.Empty;
                password = string.Empty;
            }
            bool reqft = this.chkbReqFT.IsChecked.HasValue && this.chkbReqFT.IsChecked.Value;


            AXRESTClientDataSourceList dslClient = Global.clientCaches["AXRESTClientDataSourceList"] as AXRESTClientDataSourceList;

            RegisterClientEvents(dslClient);
            AXRESTClientDataSource dsClient = await dslClient.LoginDataSourceAsync(
                dsname, authmode, username, password, reqft, "", Global.MediaType);
            UnregisterClientEvents(dslClient);

            Global.clientCaches["AXRESTClientDataSource"] = dsClient;

            PopulateDS(dsClient);
        }

        private void PopulateDS(AXRESTClientDataSource dsClient)
        {
            this.lbDataSource.Items.Clear();

            this.lbDataSource.Items.Add(string.Format("{0}: {1}", "Name", dsClient.Name));
            this.lbDataSource.Items.Add(string.Format("{0}: {1}", "ID", dsClient.ID));
            this.lbDataSource.Items.Add(string.Format("{0}: {1}", "Description", dsClient.Description));
            this.lbDataSource.Items.Add(string.Format("{0}: {1}", "DBVersion", dsClient.DBVersion));
            this.lbDataSource.Items.Add(string.Format("{0}: {1}", "Default", dsClient.IsDefault));

            MainWindow mainWin = App.Current.MainWindow as MainWindow;
            mainWin.SetStatusBar(currentds: dsClient.Name);
        }
    }
}
