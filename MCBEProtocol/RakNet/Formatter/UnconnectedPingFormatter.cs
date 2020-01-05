using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Internal;

namespace MCBEProtocol.RakNet.Formatter
{
    public class UnconnectedPingFormatter<TTypeResolver> : Formatter<TTypeResolver, UnconnectedPing>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 32;
        }

        public override UnconnectedPing Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
        {
            byteSize = 32;
            long timestamp = BinaryUtil.ReadInt64(ref bytes, offset);
            Guid magic = BinaryUtil.ReadGuid(ref bytes, offset + 8);
            ulong clientId = BinaryUtil.ReadUInt64(ref bytes, offset + 24);
            return new UnconnectedPing(timestamp, magic, clientId);
        }

        public override int Serialize(ref byte[] bytes, int offset, UnconnectedPing value)
        {
            BinaryUtil.WriteInt64(ref bytes, offset, value.Timestamp);
            BinaryUtil.WriteGuid(ref bytes, offset + 8, value.Magic);
            BinaryUtil.WriteUInt64(ref bytes, offset + 24, value.ClientId);
            return 32;
        }
    }
}
