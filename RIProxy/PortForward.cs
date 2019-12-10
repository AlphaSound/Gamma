using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace Gamma.Proxy
{
    class PortForward
    {
        private readonly Queue<short> AvailablePorts;
        private readonly Dictionary<IPEndPoint, short> Clients;

        public PortForward(IEnumerable<short> availablePorts)
        {
            AvailablePorts = new Queue<short>(availablePorts.Distinct());
            Clients = new Dictionary<IPEndPoint, short>();
        }

        public short Add(IPEndPoint address)
        {
            if (AvailablePorts.Count == 0)
                throw new Exception("No available ports");

            short port = AvailablePorts.Dequeue();
            Clients.Add(address, port);
            return port;
        }

        public void Remove(IPEndPoint address)
        {
            if(Clients.Remove(address, out short port))
            {
                AvailablePorts.Enqueue(port);
            }
        }

        public short this[IPEndPoint address] => Clients[address];
        public IPEndPoint this[short port] => Clients.First(c => c.Value == port).Key;
    }
}
