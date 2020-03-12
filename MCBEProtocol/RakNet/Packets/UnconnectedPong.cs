using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class UnconnectedPong : Packet<UnconnectedPong>
    {
        public long Timestamp { get; set; }

        public ulong PongId { get; set; }

        public Guid Magic { get; set; }

        public string ServerInfo { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteInt64(Timestamp);
            stream.WriteUInt64(PongId);
            stream.WriteGuid(Magic);
            stream.WriteStringUInt16(ServerInfo);
        }

        public override void Decode(BinaryStream stream)
        {
            Timestamp = stream.ReadInt64();
            PongId = stream.ReadUInt64();
            Magic = stream.ReadGuid();
            ServerInfo = stream.ReadStringUInt16();
        }
    }
}
