using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    //IAuthorizationComponent -> je komponenta koja pomaze kod autorizacije korisnika
    //  klasa koja implementira ovaj interfejs ne autorizuje korisnike nego omogucava ServiceAuthorizationManager klasi da to uradi
    //ServiceAuthorizationManager zove evaluate metodu za svaki AuthorizationPolicy
    public class CustomPolicy : IAuthorizationPolicy
    {
        readonly string id;

        public CustomPolicy()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return id; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            Debugger.Launch();
            // podesava se custom principal i prosledjuje se WindowsIdentity

            object obj;
            if (!evaluationContext.Properties.TryGetValue("Identities", out obj))
                return false;

            List<IIdentity> identities = obj as List<IIdentity>;
            if (obj == null || identities.Count <= 0)
                return false;

            evaluationContext.Properties["Principal"] = new CustomPrincipal(identities[0]);
            return true;
        }
    }
}
