using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gamma.Proxy
{
    interface IRouter
    {
        void Send(IPEndPoint source, int port, byte[] payload);
    }
}
