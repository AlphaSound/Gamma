using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class IncompatibleProtocolVersion : Packet<IncompatibleProtocolVersion>
    {
        public byte Protocol { get; set; }

        public Guid Magic { get; set; }

        public ulong ServerId { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteByte(Protocol);
            stream.WriteGuid(Magic);
            stream.WriteUInt64(ServerId);
        }

        public override void Decode(BinaryStream stream)
        {
            Protocol = stream.ReadByte();
            Magic = stream.ReadGuid();
            ServerId = stream.ReadUInt64();
        }
    }
}
