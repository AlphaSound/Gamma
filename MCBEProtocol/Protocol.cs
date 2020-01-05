using MCBEProtocol.RakNet;
using MCBEProtocol.RakNet.Formatter;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;
using ZeroFormatter.Formatters;

namespace MCBEProtocol
{
    public static class Protocol
    {
        public static void Initialize()
        {
            Formatter<DefaultResolver, UnconnectedPing>.Register(new UnconnectedPingFormatter<DefaultResolver>());
            Formatter<DefaultResolver, UnconnectedPong>.Register(new UnconnectedPongFormatter<DefaultResolver>());
        }
    }
}
