using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gamma.Proxy
{
    class Router : IRouter
    {
        protected ConcurrentDictionary<(IPEndPoint, int), (IPEndPoint Dest, int To)> Routes { get; }
        protected IPacketHandler Handler { get; }
        protected ISocketsHolder SocketsHolder { get; }

        public Router(ISocketsHolder socketsHolder, IPacketHandler packetHandler)
        {
            Routes = new ConcurrentDictionary<(IPEndPoint, int), (IPEndPoint, int)>();
            Handler = packetHandler;
            SocketsHolder = socketsHolder;
        }

        public void Send(IPEndPoint source, int from, byte[] payload)
        {
            if(Handler.HandleBeforeRouting(source, from, payload))
            {
                if (Routes.TryGetValue((source, from), out (IPEndPoint Dest, int To) route))
                {
                    SocketsHolder.GetSocket(route.To).Send(route.Dest, payload);
                }
                Handler.HandleAfterRouting(source, from, payload);
            }
        }

        public bool AddRoute(IPEndPoint source, int from, IPEndPoint dest, int to) =>
            Routes.TryAdd((source, from), (dest, to));

        public bool RemoveRoute(IPEndPoint source, int port, out (IPEndPoint, int) target) =>
            Routes.TryRemove((source, port), out target);

        public void ClearRoute() =>
            Routes.Clear();
    }
}
