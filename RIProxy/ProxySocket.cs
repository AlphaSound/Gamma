using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gamma.Proxy
{
    class ProxySocket : IDisposable
    {
        public short Port { get; }

        protected UdpClient Socket { get; private set; }
        protected BlockingCollection<(IPEndPoint, byte[])> SendQueue { get; }
        protected IRouter Router { get; }

        protected Task Receiver;
        protected Task Sender;
        protected CancellationTokenSource CTS;

        public DateTime StartTime { get; }
        private ReaderWriterLockSlim RWLock { get; }
        private DateTime _UpdateTime;
        public DateTime UpdateTime
        {
            get
            {
                try
                {
                    RWLock.EnterReadLock();
                    return _UpdateTime;
                }
                finally
                {
                    RWLock.ExitReadLock();
                }
            }
            protected set
            {
                try
                {
                    RWLock.EnterWriteLock();
                    _UpdateTime = value;
                }
                finally
                {
                    RWLock.ExitWriteLock();
                }
            }
        }

        public ProxySocket(IPEndPoint bindAddress, IRouter router)
        {
            StartTime = DateTime.Now;
            RWLock = new ReaderWriterLockSlim();
            UpdateTime = DateTime.Now;

            Port = (short)bindAddress.Port;
            Socket = new UdpClient(bindAddress);
            Socket.DontFragment = true;
            Socket.Client.ReceiveBufferSize = 5 * 1024 * 1024;
            Socket.Client.SendBufferSize = 5 * 1024 * 1024;
            SendQueue = new BlockingCollection<(IPEndPoint, byte[])>(new ConcurrentQueue<(IPEndPoint, byte[])>());
            Router = router;

            CTS = new CancellationTokenSource();
            Receiver = StartReceiver();
            Sender = Task.Run(StartSender);
        }

        private async Task StartReceiver()
        {
            try
            {
                while (true)
                {
                    var receive = await Socket.ReceiveAsync();
                    var address = receive.RemoteEndPoint;
                    var buff = receive.Buffer;
                    UpdateTime = DateTime.Now;
                    Router.Send(address, Port, buff);
                }
            }
            catch (ObjectDisposedException) { /* Socket closed */ }
        }

        private void StartSender()
        {
            try
            {
                foreach (var (address, payload) in SendQueue.GetConsumingEnumerable(CTS.Token))
                {
                    Socket.Send(payload, payload.Length, address);
                }
            }
            catch (OperationCanceledException) { /* Canceled */ }
        }

        public void Send(IPEndPoint destination, byte[] payload) =>
            SendQueue.Add((destination, payload));

        public void Dispose()
        {
            CTS.Cancel();
            try { Sender.Wait(); }
            catch (ObjectDisposedException) { /* Already completed */ }
            Socket.Close();
            try { Receiver.Wait(); }
            catch (ObjectDisposedException) { /* Already completed */ }
            SendQueue.Dispose();
            CTS.Dispose();
        }
    }
}
