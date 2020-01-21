using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MCBEProtocol.RakNet.StreamExtensions;

namespace MCBEProtocol.RakNet.Packets
{
    public class ConnectionRequestAccepted : Packet<ConnectionRequestAccepted>
    {
        public IPEndPoint ClientAddress { get; set; }

        public ushort AddressIndex { get; set; }

        public IEnumerable<IPEndPoint> SystemAddresses { get; set; }

        public long PingTimestamp { get; set; }

        public long PongTimestamp { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteAddress(ClientAddress);
            stream.WriteUInt16(AddressIndex);
            foreach (var address in SystemAddresses)
            {
                stream.WriteAddress(address);
            }
            stream.WriteInt64(PingTimestamp);
            stream.WriteInt64(PongTimestamp);
        }

        public override void Decode(BinaryStream stream)
        {
            ClientAddress = stream.ReadAddress();
            AddressIndex = stream.ReadUInt16();
            var list = new List<IPEndPoint>();
            while (stream.Length - stream.Offset - 16 > 0)
            {
                list.Add(stream.ReadAddress());
            }
            SystemAddresses = list;
            PingTimestamp = stream.ReadInt64();
            PongTimestamp = stream.ReadInt64();
        }
    }
}
