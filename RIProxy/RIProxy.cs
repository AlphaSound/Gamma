using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gamma.Proxy
{
    class RIProxy : ISocketsHolder, IPacketHandler, IDisposable
    {
        protected readonly IPEndPoint ServerAddress;
        protected readonly IPEndPoint ProxyAddress;

        protected Router Router { get; }
        protected ConcurrentQueue<int> AvailablePorts { get; }
        protected ConcurrentDictionary<int, ProxySocket> Sockets { get; }
        protected ConcurrentDictionary<IPEndPoint, int> Clients { get; }

        public RIProxy(IPEndPoint server, IPEndPoint proxy, IEnumerable<int> availablePorts)
        {
            ServerAddress = server;
            ProxyAddress = proxy;
            Router = new Router(this, this);
            AvailablePorts = new ConcurrentQueue<int>();
            AvailablePorts.Enqueue(proxy.Port);
            foreach (var port in availablePorts)
                AvailablePorts.Enqueue(port);
            Clients = new ConcurrentDictionary<IPEndPoint, int>();
            Sockets = new ConcurrentDictionary<int, ProxySocket>();
            AllocPort();
        }

        public ProxySocket GetSocket(int port) => Sockets[port];

        protected ProxySocket AllocPort()
        {
            if (AvailablePorts.Count == 0)
                throw new Exception("No available ports");
            if (AvailablePorts.TryDequeue(out int port))
            {
                var bind = new IPEndPoint(ProxyAddress.Address, port);
                var socket = new ProxySocket(bind, Router);
                if (!Sockets.TryAdd(port, socket))
                    throw new Exception("WTF AvailablePorts MANAGING MISS");
                return socket;
            }
            throw new Exception("Failed port allocating");
        }

        protected async Task ReleasePort(int port)
        {
            if(Sockets.TryGetValue(port, out ProxySocket socket))
            {
                await Task.Run(socket.Dispose);
                AvailablePorts.Enqueue(port);
            }
        }

        public bool Handle(IPEndPoint address, int socketPort, byte[] payload)
        {
            if (address.Equals(ServerAddress))//from Server
            {
                if(payload[0] == 0x1c)
                {
                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(50);
                        Router.RemoveRoute(address, socketPort, out (IPEndPoint, int) _);
                        Router.RemoveRoute(Clients.First(d => d.Value == socketPort).Key, socketPort, out (IPEndPoint, int) _);
                    });
                }
                return true;
            }
            else//from Client
            {
                if (!Clients.ContainsKey(address))
                {
                    if (AvailablePorts.Count == 0)
                        return false;
                    var port = AllocPort().Port;
                    Clients.TryAdd(address, port);
                    Router.AddRoute(address, socketPort, ServerAddress, port);
                    Router.AddRoute(ServerAddress, port, address, socketPort);
                }
                //
                return true;
            }
        }

        public void Dispose()
        {
            Router.ClearRoute();
            Task.WaitAll(Sockets.Keys.Select(port => ReleasePort(port)).ToArray());
            Clients.Clear();
        }
    }
}
