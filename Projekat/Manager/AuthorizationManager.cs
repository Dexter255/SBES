using System;
using System.Collections.Generic;
using System.Linq;
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
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            return base.CheckAccessCore(operationContext);
        }
    }
}
