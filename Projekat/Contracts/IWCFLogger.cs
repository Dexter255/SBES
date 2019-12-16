using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFLogger
    {
        [OperationContract]
        void AutentificationSuccess(string userName);
        [OperationContract]
        void AutentificationFailed(string userName);
        [OperationContract]
        void AuthorizationSuccess(string userName);
        [OperationContract]
        void AuthorizationFailed(string userName, string reason);
        [OperationContract]
        void UserOperationSuccess(string userName, string operation);
        [OperationContract]
        void UserOperationFailed(string userName, string operation, string reason);
    }
}
