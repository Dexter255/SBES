using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class AuthorizationManager : ServiceAuthorizationManager
    {
        // poziva se pre poziva metode u servisu kako bi se obezbedila provera prava pirstupa korisnika koji je pozvao metodu
        // posto se poziva pre metode u servisu, nit jos njije kreirana, tako da iz niti nije moguce izvaditi user-a koji
        // je pozvao metodu

        // nudi centralizovanu kontrolu pristupa u slučajevima kada postoji zajednički
        // preduslov za pristup svim metodama servisa
        // u ovom slucaju ce biti View
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            //Debugger.Launch();
            IPrincipal customPrincipal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as CustomPrincipal;

            return customPrincipal.IsInRole("View");
        }
    }
}
