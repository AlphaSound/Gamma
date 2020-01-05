using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Internal;

namespace MCBEProtocol.RakNet.Formatter
{
    public class UnconnectedPongFormatter<TTypeResolver> : Formatter<TTypeResolver, UnconnectedPong>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override UnconnectedPong Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            long timestamp = BinaryUtil.ReadInt64(ref bytes, offset);
            ulong pongId = BinaryUtil.ReadUInt64(ref bytes, offset + 8);
            Guid magic = BinaryUtil.ReadGuid(ref bytes, offset + 16);
            ushort nameLength = BinaryUtil.ReadUInt16(ref bytes, offset + 32);
            string serverName = Encoding.UTF8.GetString(BinaryUtil.ReadBytes(ref bytes, offset + 34, nameLength));
            byteSize = 34 + nameLength;
            return new UnconnectedPong(timestamp, pongId, magic, serverName);
        }

        public override int Serialize(ref byte[] bytes, int offset, UnconnectedPong value)
        {
            BinaryUtil.WriteInt64(ref bytes, offset, value.Timestamp);
            BinaryUtil.WriteUInt64(ref bytes, offset + 8, value.PongId);
            BinaryUtil.WriteGuid(ref bytes, offset + 16, value.Magic);
            var bytes_ServerName = Encoding.UTF8.GetBytes(value.ServerName);
            BinaryUtil.WriteUInt16(ref bytes, offset + 32, (ushort)bytes_ServerName.Length);
            BinaryUtil.WriteBytes(ref bytes, offset + 34, bytes_ServerName);
            return 34 + bytes_ServerName.Length;
        }
    }
}
