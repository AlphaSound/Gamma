using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MCBEProtocol.RakNet.StreamExtensions;

namespace MCBEProtocol.RakNet
{
    public class OpenConnectionRequest2 : Packet<OpenConnectionRequest2>
    {
        public Guid Magic { get; set; }

        public IPEndPoint ServerAddress { get; set; }

        public ushort MTUSize { get; set; }

        public ulong ClientId { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteGuid(Magic);
            stream.WriteAddress(ServerAddress);
            stream.WriteUInt16(MTUSize);
            stream.WriteUInt64(ClientId);
        }

        public override void Decode(BinaryStream stream)
        {
            Magic = stream.ReadGuid();
            ServerAddress = stream.ReadAddress();
            MTUSize = stream.ReadUInt16();
            ClientId = stream.ReadUInt64();
        }
    }
}
