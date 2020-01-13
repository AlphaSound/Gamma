using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class Message
    {
        public const int CONTENT_MAX_SIZE = 0x1fff;

        public MessageReliability Reliability { get; set; }
        public bool HasSplit { get; set; }
        public int MessageIndex { get; set; }

        public int SequenceIndex { get; set; }
        public int OrderIndex { get; set; }
        public byte OrderChannel { get; set; }

        public ushort SplitId { get; set; }
        public int SplitCount { get; set; }
        public int SplitIndex { get; set; }

        public byte[] Content { get; set; }

        public void Encode(BinaryStream stream)
        {
            stream.WriteByte((byte)(((byte)Reliability << 5) | (HasSplit ? 0x10 : 0x00)));

            if (Content.Length > CONTENT_MAX_SIZE) throw new OverflowException("Content.Length > CONTENT_MAX_SIZE");
            stream.WriteInt16((short)(Content.Length << 3));

            if (ReliabilityHelper.IsReliable(Reliability))
            {
                stream.WriteInt24(MessageIndex);
            }

            if (ReliabilityHelper.IsSequenced(Reliability))
            {
                stream.WriteInt24(SequenceIndex);
                stream.WriteInt24(OrderIndex);
                stream.WriteByte(OrderChannel);
            }
            else if (ReliabilityHelper.IsOrdered(Reliability))
            {
                stream.WriteInt24(OrderIndex);
                stream.WriteByte(OrderChannel);
            }

            if (HasSplit)
            {
                stream.WriteInt32(SplitCount);
                stream.WriteUInt16(SplitId);
                stream.WriteInt32(SplitIndex);
            }

            stream.WriteBytes(Content);
        }

        public void Decode(BinaryStream stream)
        {
            byte flags = stream.ReadByte();
            Reliability = (MessageReliability)(flags >> 5);
            HasSplit = (flags & 0x10) != 0;
            
            var contentLength = stream.ReadInt16() >> 3;

            if (ReliabilityHelper.IsReliable(Reliability))
            {
                MessageIndex = stream.ReadInt24();
            }

            if (ReliabilityHelper.IsSequenced(Reliability))
            {
                SequenceIndex = stream.ReadInt24();
                OrderIndex = stream.ReadInt24();
                OrderChannel = stream.ReadByte();
            }
            else if (ReliabilityHelper.IsOrdered(Reliability))
            {
                OrderIndex = stream.ReadInt24();
                OrderChannel = stream.ReadByte();
            }

            if (HasSplit)
            {
                SplitCount = stream.ReadInt32();
                SplitId = stream.ReadUInt16();
                SplitIndex = stream.ReadInt32();
            }

            Content = stream.ReadBytes(contentLength);
        }
    }

    public enum MessageReliability : byte
    {
        UNRELIABLE,
        UNRELIABLE_SEQUENCED,
        RELIABLE,
        RELIABLE_ORDERED,
        RELIABLE_SEQUENCED,
        UNRELIABLE_WITH_ACK_RECEIPT,
        RELIABLE_WITH_ACK_RECEIPT,
        RELIABLE_ORDERED_WITH_ACK_RECEIPT
    }

    public static class ReliabilityHelper
    {
        public static bool IsReliable(MessageReliability reliability)
        {
            return reliability == MessageReliability.RELIABLE
                || reliability == MessageReliability.RELIABLE_ORDERED
                || reliability == MessageReliability.RELIABLE_SEQUENCED
                || reliability == MessageReliability.RELIABLE_WITH_ACK_RECEIPT
                || reliability == MessageReliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT;
        }

        public static bool IsSequenced(MessageReliability reliability)
        {
            return reliability == MessageReliability.UNRELIABLE_SEQUENCED
                || reliability == MessageReliability.RELIABLE_SEQUENCED;
        }

        public static bool IsOrdered(MessageReliability reliability)
        {
            return reliability == MessageReliability.RELIABLE_ORDERED
                || reliability == MessageReliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT;
        }

        public static bool NeedACK(MessageReliability reliability)
        {
            return reliability == MessageReliability.UNRELIABLE_WITH_ACK_RECEIPT
                || reliability == MessageReliability.RELIABLE_WITH_ACK_RECEIPT
                || reliability == MessageReliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT;
        }
    }
}
