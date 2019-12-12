using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class CustomPrincipal : IPrincipal
    {
        IIdentity identity;

        public CustomPrincipal(IIdentity identity)
        {
            this.identity = identity;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string permission)
        {
            //Debugger.Launch();
            // CN=userAdmin, OU=Admins; 0B3E48F7BFA77954809DF79E5D3E295D96CF888C
            string groupName = identity.Name.Split(',', ';')[1].Split('=')[1];

            string permissions = RBACConfig.ResourceManager.GetString(groupName);

            if (permissions.Contains(permission))
            {
                return true;
            }

            return false;
        }
    }
}
