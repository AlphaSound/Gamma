using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class ConnectedPing : Packet<ConnectedPing>
    {
        public long PingTimestamp { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteInt64(PingTimestamp);
        }

        public override void Decode(BinaryStream stream)
        {
            PingTimestamp = stream.ReadInt64();
        }
    }
}
