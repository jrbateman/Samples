using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXDataSources : LinkedResourceCollection<AXDataSource>
    {
        public AXDataSources()
        {
        }

        public static bool SingleDataSource { get; set; }
    }
}
