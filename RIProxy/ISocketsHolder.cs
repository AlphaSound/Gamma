using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Proxy
{
    interface ISocketsHolder
    {
        ProxySocket GetSocket(short port);
    }
}
