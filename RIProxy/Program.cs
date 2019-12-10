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
            var availablePorts = Enumerable.Range(55000, 10);
            using (var riproxy = new RIProxy(serverAddress, proxyAddress, availablePorts))
            {
                Console.WriteLine("Proxy started...");
                Console.ReadLine();
            }
        }
    }
}
