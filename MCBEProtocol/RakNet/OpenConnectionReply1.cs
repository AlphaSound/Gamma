using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet
{
    public class OpenConnectionReply1 : Packet<OpenConnectionReply1>
    {
        public Guid Magic { get; set; }

        public ulong ServerId { get; set; }

        public bool UseSecurity { get; set; }

        public ushort MTUSize { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteGuid(Magic);
            stream.WriteUInt64(ServerId);
            stream.WriteBoolean(UseSecurity);
            stream.WriteUInt16(MTUSize);
        }

        public override void Decode(BinaryStream stream)
        {
            Magic = stream.ReadGuid();
            ServerId = stream.ReadUInt64();
            UseSecurity = stream.ReadBoolean();
            MTUSize = stream.ReadUInt16();
        }
    }
}
