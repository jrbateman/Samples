using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XtenderSolutions.AXRESTDataModel
{
    [XmlRoot(Namespace = LinkedResource.DeaufaultNameSpace)]
    public class AXUser : LinkedResource
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// User ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// User permissions
        /// </summary>
        public AXPermissionCollection Permissions { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AXUser()
        {
        }
    }
}
