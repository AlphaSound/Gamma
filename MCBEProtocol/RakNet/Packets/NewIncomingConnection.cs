using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MCBEProtocol.RakNet.StreamExtensions;

namespace MCBEProtocol.RakNet.Packets
{
    public class NewIncomingConnection : Packet<NewIncomingConnection>
    {
        public IPEndPoint ServerAddress { get; set; }

        public IEnumerable<IPEndPoint> SystemAddresses { get; set; }

        public long PingTimestamp { get; set; }

        public long PongTimestamp { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteAddress(ServerAddress);
            foreach (var address in SystemAddresses)
            {
                stream.WriteAddress(address);
            }
            stream.WriteInt64(PingTimestamp);
            stream.WriteInt64(PongTimestamp);
        }

        public override void Decode(BinaryStream stream)
        {
            ServerAddress = stream.ReadAddress();
            var list = new List<IPEndPoint>();
            while (stream.Length - stream.Offset - 16 > 0)
            {
                list.Add(stream.ReadAddress());
            }
            PingTimestamp = stream.ReadInt64();
            PongTimestamp = stream.ReadInt64();
        }
    }
}
