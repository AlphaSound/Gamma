using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol
{
    public class BinaryStream
    {
        protected int Offset;
        protected int Length;
        protected byte[] Buffer;

        public void Clear()
        {
            Offset = 0;
            Length = 0;
            Buffer = new byte[0];
        }

        public void SetBuffer(byte[] buffer)
        {
            Buffer = new byte[buffer.Length];
            System.Buffer.BlockCopy(buffer, 0, Buffer, 0, buffer.Length);
        }

        public byte[] GetBuffer()
        {
            var buff = new byte[Length];
            System.Buffer.BlockCopy(Buffer, 0, buff, 0, Length);
            return buff;
        }

        public BinaryStream()
        {
            Clear();
        }
        
        public void WriteBytes(byte[] value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteBytes(ref Buffer, Offset, value));
            Offset += value.Length;
        }

        public void WriteByte(byte value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteByte(ref Buffer, Offset, value));
            Offset += 1;
        }

        public void WriteBoolean(bool value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteBoolean(ref Buffer, Offset, value));
            Offset += 1;
        }

        public void WriteInt16(short value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteInt16(ref Buffer, Offset, value));
            Offset += 2;
        }

        public void WriteUInt16(ushort value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteUInt16(ref Buffer, Offset, value));
            Offset += 2;
        }

        public void WriteInt24(int value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteInt24(ref Buffer, Offset, value));
            Offset += 3;
        }

        public void WriteInt32(int value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteInt32(ref Buffer, Offset, value));
            Offset += 4;
        }

        public void WriteUInt32(uint value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteUInt32(ref Buffer, Offset, value));
            Offset += 4;
        }

        public void WriteInt64(long value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteInt64(ref Buffer, Offset, value));
            Offset += 8;
        }

        public void WriteUInt64(ulong value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteUInt64(ref Buffer, Offset, value));
            Offset += 8;
        }

        public void WriteFloat(float value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteFloat(ref Buffer, Offset, value));
            Offset += 4;
        }

        public void WriteDouble(double value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteDouble(ref Buffer, Offset, value));
            Offset += 8;
        }

        public void WriteVarInt(int value)
        {
            int len = BinaryUtil.WriteVarInt(ref Buffer, Offset, value);
            Length = Math.Max(Length, Offset + len);
            Offset += len;
        }

        public void WriteUVarInt(uint value)
        {
            int len = BinaryUtil.WriteUVarInt(ref Buffer, Offset, value);
            Length = Math.Max(Length, Offset + len);
            Offset += len;
        }

        public void WriteVarLong(long value)
        {
            int len = BinaryUtil.WriteVarLong(ref Buffer, Offset, value);
            Length = Math.Max(Length, Offset + len);
            Offset += len;
        }

        public void WriteUVarLong(ulong value)
        {
            int len = BinaryUtil.WriteUVarLong(ref Buffer, Offset, value);
            Length = Math.Max(Length, Offset + len);
            Offset += len;
        }

        public void WriteStringUInt16(string value, Encoding encoding)
        {
            int len = BinaryUtil.WriteStringUInt16(ref Buffer, Offset, value, encoding);
            Length = Math.Max(Length, Offset + len);
            Offset += len;
        }

        public void WriteStringUVarInt(string value, Encoding encoding)
        {
            int len = BinaryUtil.WriteStringUVarInt(ref Buffer, Offset, value, encoding);
            Length = Math.Max(Length, Offset + len);
            Offset += len;
        }

        public void WriteGuid(Guid value)
        {
            Length = Math.Max(Length, Offset + BinaryUtil.WriteGuid(ref Buffer, Offset, value));
            Offset += 16;
        }

        public byte[] ReadBytes(int length)
        {
            var value = BinaryUtil.ReadBytes(ref Buffer, Offset, length);
            Offset += length;
            return value;
        }

        public byte ReadByte()
        {
            var value = BinaryUtil.ReadByte(ref Buffer, Offset);
            Offset += 1;
            return value;
        }

        public bool ReadBoolean()
        {
            var value = BinaryUtil.ReadBoolean(ref Buffer, Offset);
            Offset += 1;
            return value;
        }

        public short ReadInt16()
        {
            var value = BinaryUtil.ReadInt16(ref Buffer, Offset);
            Offset += 2;
            return value;
        }

        public ushort ReadUInt16()
        {
            var value = BinaryUtil.ReadUInt16(ref Buffer, Offset);
            Offset += 2;
            return value;
        }

        public int ReadInt24()
        {
            var value = BinaryUtil.ReadInt24(ref Buffer, Offset);
            Offset += 3;
            return value;
        }

        public int ReadInt32()
        {
            var value = BinaryUtil.ReadInt32(ref Buffer, Offset);
            Offset += 4;
            return value;
        }

        public uint ReadUInt32()
        {
            var value = BinaryUtil.ReadUInt32(ref Buffer, Offset);
            Offset += 4;
            return value;
        }

        public long ReadInt64()
        {
            var value = BinaryUtil.ReadInt64(ref Buffer, Offset);
            Offset += 8;
            return value;
        }

        public ulong ReadUInt64()
        {
            var value = BinaryUtil.ReadUInt64(ref Buffer, Offset);
            Offset += 8;
            return value;
        }

        public float ReadFloat()
        {
            var value = BinaryUtil.ReadFloat(ref Buffer, Offset);
            Offset += 4;
            return value;
        }

        public double ReadDouble()
        {
            var value = BinaryUtil.ReadDouble(ref Buffer, Offset);
            Offset += 8;
            return value;
        }

        public int ReadVarInt()
        {
            var value = BinaryUtil.ReadVarInt(ref Buffer, Offset, out int len);
            Offset += len;
            return value;
        }

        public uint ReadUVarInt()
        {
            var value = BinaryUtil.ReadUVarInt(ref Buffer, Offset, out int len);
            Offset += len;
            return value;
        }

        public long ReadVarLong()
        {
            var value = BinaryUtil.ReadVarLong(ref Buffer, Offset, out int len);
            Offset += len;
            return value;
        }

        public ulong ReadUVarLong()
        {
            var value = BinaryUtil.ReadUVarLong(ref Buffer, Offset, out int len);
            Offset += len;
            return value;
        }

        public string ReadStringUInt16(Encoding encoding)
        {
            var value = BinaryUtil.ReadStringUInt16(ref Buffer, Offset, out int len);
            Offset += len;
            return value;
        }

        public string ReadStringUVarInt(Encoding encoding)
        {
            var value = BinaryUtil.ReadStringUVarInt(ref Buffer, Offset, out int len);
            Offset += len;
            return value;
        }

        public Guid ReadGuid()
        {
            var value = BinaryUtil.ReadGuid(ref Buffer, Offset);
            Offset += 16;
            return value;
        }
    }
}
