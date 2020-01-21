using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class ConnectedPong : Packet<ConnectedPong>
    {
        public long PingTimestamp { get; set; }

        public long PongTimestamp { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteInt64(PingTimestamp);
            stream.WriteInt64(PongTimestamp);
        }

        public override void Decode(BinaryStream stream)
        {
            PingTimestamp = stream.ReadInt64();
            PongTimestamp = stream.ReadInt64();
        }
    }
}
