using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gamma.Proxy
{
    class Router : IRouter
    {
        protected ConcurrentDictionary<(IPEndPoint, short), (IPEndPoint Dest, short To)> Routes { get; }
        protected IPacketHandler Handler { get; }
        protected ISocketsHolder SocketsHolder { get; }

        public Router(ISocketsHolder socketsHolder, IPacketHandler packetHandler)
        {
            Routes = new ConcurrentDictionary<(IPEndPoint, short), (IPEndPoint, short)>();
            Handler = packetHandler;
            SocketsHolder = socketsHolder;
        }

        public void Send(IPEndPoint source, short from, byte[] payload)
        {
            if(Handler.Handle(source, from, payload))
            {
                if (Routes.TryGetValue((source, from), out (IPEndPoint Dest, short To) route))
                {
                    SocketsHolder.GetSocket(route.To).Send(route.Dest, payload);
                }
            }
        }

        public bool AddRoute(IPEndPoint source, short from, IPEndPoint dest, short to) =>
            Routes.TryAdd((source, from), (dest, to));

        public bool RemoveRoute(IPEndPoint source, short port, out (IPEndPoint, short) target) =>
            Routes.TryRemove((source, port), out target);

        public void ClearRoute() =>
            Routes.Clear();
    }
}
