using System;
using System.Collections.Generic;
using System.Text;


namespace MCBEProtocol.RakNet
{
    public class OpenConnectionRequest1 : Packet<OpenConnectionRequest1>
    {
        public Guid Magic { get; set; }

        public byte Protocol { get; set; }

        public byte[] NullPadding { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteGuid(Magic);
            stream.WriteByte(Protocol);
            stream.WriteBytes(NullPadding);
        }

        public override void Decode(BinaryStream stream)
        {
            Magic = stream.ReadGuid();
            Protocol = stream.ReadByte();
            NullPadding = stream.ReadBytes(stream.Length - stream.Offset);
        }
    }
}
