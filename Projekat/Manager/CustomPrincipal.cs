using System;
using System.Collections.Generic;
using System.Linq;
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
            return true;
            /*object obj;

            foreach (var group in (identity as WindowsIdentity).Groups)
            {
                string groupName = Formatter.ParseName(group.Translate(typeof(NTAccount)).ToString());
                obj = ConfigFile.ResourceManager.GetObject(groupName);

                if (obj != null)
                {
                    string[] permissions = (obj as string).Split(',');

                    foreach (var permission1 in permissions)
                    {
                        if (permission1 == permission)
                            return true;
                    }
                }
            }
            return false;*/
        }
    }
}
