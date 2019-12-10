using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gamma.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverAddress = new IPEndPoint(IPAddress.Parse(Config.ServerAddress), Config.ServerPort);
            var proxyAddress = new IPEndPoint(IPAddress.Parse(Config.ProxyAddress), Config.ProxyPort);
            var availablePorts = new short[] { Config.ProxyPort }.Concat(Enumerable.Range(55000, 55010).Select(d => (short)d));
            using (var riproxy = new RIProxy(serverAddress, proxyAddress, availablePorts))
            {
                Console.WriteLine("Proxy started...");
                Console.ReadLine();
            }
        }
    }
}
