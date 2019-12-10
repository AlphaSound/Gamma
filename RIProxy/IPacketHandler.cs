using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gamma.Proxy
{
    interface IPacketHandler
    {
        bool Handle(IPEndPoint address, short socketPort, byte[] payload);
    }
}
