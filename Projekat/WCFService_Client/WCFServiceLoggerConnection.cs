using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFService_Client
{
    public class WCFServiceLoggerConnection : ChannelFactory<IWCFLogger>
    {
        static private IWCFLogger factory;

        private WCFServiceLoggerConnection(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public static IWCFLogger Factory { get => factory; }

        public static IWCFLogger InitializeService(NetTcpBinding binding, EndpointAddress address)
        {
            if (Factory == null)
            {
                var service = new WCFServiceLoggerConnection(binding, address);
            }

            return Factory;
        }

    }
}
