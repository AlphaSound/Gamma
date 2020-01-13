using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class UnconnectedPing : Packet<UnconnectedPing>
    {
        public long Timestamp { get; set; }

        public Guid Magic { get; set; }

        public ulong ClientId { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteInt64(Timestamp);
            stream.WriteGuid(Magic);
            stream.WriteUInt64(ClientId);
        }

        public override void Decode(BinaryStream stream)
        {
            Timestamp = stream.ReadInt64();
            Magic = stream.ReadGuid();
            ClientId = stream.ReadUInt64();
        }
    }
}
