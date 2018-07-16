using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XtenderSolutions.AXRESTClient;

namespace AXRESTTestConsole
{
    internal class Global
    {
        public static Dictionary<TreeViewItem, UserControl> UIDic = new Dictionary<TreeViewItem, UserControl>();

        public static Dictionary<string, ClientWrapper> clientCaches = new Dictionary<string, ClientWrapper>();

        public static bool XMLMediaType
        {
            get
            {
                return !string.IsNullOrEmpty(Properties.Settings.Default.mediatype) &&
                        string.Compare(Properties.Settings.Default.mediatype, "xml", true) == 0;
            }
        }

        //default is json, unless xml is set in app config
        public static string HomeMediaType
        {
            get
            {
                if (XMLMediaType)
                {
                    return "application/home+xml";
                }

                return "application/home+json";

            }
        }

        public static string MediaType
        {
            get
            {
                if (XMLMediaType)
                {
                    return "application/vnd.emc.ax+xml";
                }

                return "application/vnd.emc.ax+json";

            }
        }

        public static TreeViewItem GetTreeViewItemByName(string group, string name)
        {
            MainWindow mainWin = App.Current.MainWindow as MainWindow;

            foreach(TreeViewItem item in mainWin.tvNavigation.Items)
            {
                if (item.Header == group)
                {
                    foreach(TreeViewItem sub in item.Items)
                    {
                        if (sub.Header == name)
                        {
                            return sub;
                        }
                    }
                }
            }

            return null;
        }
    }
}
