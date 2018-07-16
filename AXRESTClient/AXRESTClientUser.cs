using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientUser
    {
        private AXRESTDataModel.AXUser user;

        public int ID
        {
            get
            {
                if (this.user != null)
                    return this.user.ID;
                else
                    throw new NullReferenceException("The AX user account is not initialized");
            }
        }

        public string Name
        {
            get
            {
                if (this.user != null)
                    return this.user.Name;
                else
                    throw new NullReferenceException("The AX user account is not initialized");
            }
        }

        public string FullName
        {
            get
            {
                if (this.user != null)
                    return this.user.FullName;
                else
                    throw new NullReferenceException("The AX user account is not initialized");
            }
        }

        public List<string> Permissions
        {
            get
            {
                if (this.user != null)
                {
                    List<string> perms = new List<string>();
                    foreach(var kvp in this.user.Permissions)
                    {
                        if (kvp.Value) perms.Add(kvp.Key.ToString());
                    }
                    return perms;
                }
                else
                    throw new NullReferenceException("The AX user account is not initialized");
            }
        }

        public AXRESTClientUser(AXRESTDataModel.AXUser User)
        {
            this.user = User;
        }
    }
}
