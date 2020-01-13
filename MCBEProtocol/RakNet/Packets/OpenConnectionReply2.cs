using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MCBEProtocol.RakNet.StreamExtensions;

namespace MCBEProtocol.RakNet.Packets
{
    public class OpenConnectionReply2 : Packet<OpenConnectionReply2>
    {
        public Guid Magic { get; set; }

        public ulong ServerId { get; set; }

        public IPEndPoint ClientAddress { get; set; }

        public bool UseEncryption { get; set; }

        public ushort MTUSize { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteGuid(Magic);
            stream.WriteUInt64(ServerId);
            stream.WriteAddress(ClientAddress);
            stream.WriteBoolean(UseEncryption);
            stream.WriteUInt16(MTUSize);
        }

        public override void Decode(BinaryStream stream)
        {
            Magic = stream.ReadGuid();
            ServerId = stream.ReadUInt64();
            ClientAddress = stream.ReadAddress();
            UseEncryption = stream.ReadBoolean();
            MTUSize = stream.ReadUInt16();
        }
    }
}
