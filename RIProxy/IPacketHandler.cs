using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gamma.Proxy
{
    interface IPacketHandler
    {
        bool HandleBeforeRouting(IPEndPoint address, int socketPort, byte[] payload);
        void HandleAfterRouting(IPEndPoint address, int socketPort, byte[] payload);
    }
}
