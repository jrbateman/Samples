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
    /// Interaction logic for QueryFields.xaml
    /// </summary>
    public partial class QueryFields : BaseUserControl
    {
        //ODMADef
        //QueryFields
        //ODMAQueryFields
        private string original;
        public QueryFields(string original)
        {
            InitializeComponent();

            this.original = original;
        }

        public override async Task Get()
        {
            AXRESTClientQueryFields queryFields;
            if (this.original == "ODMADef")
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientDataSource"))
                {
                    MessageBox.Show("Please get the DataSource resource firstly");
                    return;
                }
                AXRESTClientDataSource dsClient = Global.clientCaches["AXRESTClientDataSource"] as AXRESTClientDataSource;

                RegisterClientEvents(dsClient);
                queryFields = await dsClient.GetODMAQueryFields(Global.MediaType);
                UnregisterClientEvents(dsClient);

            }
            else if (this.original == "QueryFields")
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientQuery"))
                {
                    MessageBox.Show("Please get the query resource firstly");
                    return;
                }
                AXRESTClientQuery queryClient = Global.clientCaches["AXRESTClientQuery"] as AXRESTClientQuery;

                RegisterClientEvents(queryClient);
                queryFields = await queryClient.GetQueryFieldsAsync(Global.MediaType);
                UnregisterClientEvents(queryClient);

                //populate query PUT ui
                var item = Global.GetTreeViewItemByName("Query Group", "Query");
                if (item != null)
                {
                    Query ui = Global.UIDic[item] as Query;
                    ui.PopulateFields(queryFields.Collection);
                }
            }
            else if (this.original == "ODMAQueryFields")
            {
                if (!Global.clientCaches.ContainsKey("AXRESTClientQuery"))
                {
                    MessageBox.Show("Please get the query resource firstly");
                    return;
                }
                AXRESTClientQuery queryClient = Global.clientCaches["AXRESTClientQuery"] as AXRESTClientQuery;

                RegisterClientEvents(queryClient);
                queryFields = await queryClient.GetODMAQueryFieldsAsync(Global.MediaType);
                UnregisterClientEvents(queryClient);
            }
            else
            {
                return;
            }

            PopulateQueryFieldsUI(queryFields);
        }

        private void PopulateQueryFieldsUI(AXRESTClientQueryFields queryFields)
        {
            this.dgQueryFields.ItemsSource = queryFields.Collection;
        }

    }
}
