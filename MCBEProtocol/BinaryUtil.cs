using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCBEProtocol
{
    // Based ZeroFormatter.Internal.BinaryUtil
    // https://github.com/neuecc/ZeroFormatter/blob/master/src/ZeroFormatter/Internal/BinaryUtil.cs
    public static class BinaryUtil
    {
        public static void EnsureCapacity(ref byte[] bytes, int offset, int appendLength)
        {
            var newLength = offset + appendLength;

            // If null(most case fisrt time) fill byte.
            if (bytes == null)
            {
                bytes = new byte[newLength];
                return;
            }

            // like MemoryStream.EnsureCapacity
            var current = bytes.Length;
            if (newLength <= current) return;
            if (newLength < 256)
            {
                newLength = 256;
            }
            else if (newLength < current * 2)
            {
                newLength = current * 2;
            }

            FastResize(ref bytes, newLength);
        }

        // Buffer.BlockCopy version of Array.Resize
        public static void FastResize(ref byte[] array, int newSize)
        {
            if (newSize < 0) throw new ArgumentOutOfRangeException(nameof(newSize));

            byte[] array2 = array;
            if (array2 == null)
            {
                array = new byte[newSize];
                return;
            }

            if (array2.Length != newSize)
            {
                byte[] array3 = new byte[newSize];
                Buffer.BlockCopy(array2, 0, array3, 0, (array2.Length > newSize) ? newSize : array2.Length);
                array = array3;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBytesUnsafe(ref byte[] bytes, int offset, byte[] value)
        {
            Buffer.BlockCopy(value, 0, bytes, offset, value.Length);
            return value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBytes(ref byte[] bytes, int offset, byte[] value)
        {
            EnsureCapacity(ref bytes, offset, value.Length);
            Buffer.BlockCopy(value, 0, bytes, offset, value.Length);
            return value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadBytes(ref byte[] bytes, int offset, int length)
        {
            if (bytes.Length < offset + length) throw new OverflowException();
            var dest = new byte[length];
            Buffer.BlockCopy(bytes, offset, dest, 0, length);
            return dest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteByteUnsafe(ref byte[] bytes, int offset, byte value)
        {
            bytes[offset] = value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteByte(ref byte[] bytes, int offset, byte value)
        {
            EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadByte(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 1) throw new OverflowException();
            return bytes[offset];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteSByteUnsafe(ref byte[] bytes, int offset, sbyte value)
        {
            bytes[offset] = (byte)value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteSByte(ref byte[] bytes, int offset, sbyte value)
        {
            EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = (byte)value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSByte(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 1) throw new OverflowException();
            return (sbyte)bytes[offset];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBooleanUnsafe(ref byte[] bytes, int offset, bool value)
        {
            bytes[offset] = (byte)(value ? 1 : 0);
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBoolean(ref byte[] bytes, int offset, bool value)
        {
            EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = (byte)(value ? 1 : 0);
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolean(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 1) throw new OverflowException();
            return bytes[offset] != 0x00;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt16Unsafe(ref byte[] bytes, int offset, short value)
        {
            fixed (byte* ptr = bytes)
            {
                *(short*)(ptr + offset) = value;
            }

            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt16(ref byte[] bytes, int offset, short value)
        {
            EnsureCapacity(ref bytes, offset, 2);

            fixed(byte* ptr = bytes)
            {
                *(short*)(ptr + offset) = value;
            }

            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe short ReadInt16(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 2) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(short*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt16Unsafe(ref byte[] bytes, int offset, ushort value)
        {
            fixed (byte* ptr = bytes)
            {
                *(ushort*)(ptr + offset) = value;
            }

            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt16(ref byte[] bytes, int offset, ushort value)
        {
            EnsureCapacity(ref bytes, offset, 2);

            fixed(byte* ptr = bytes)
            {
                *(ushort*)(ptr + offset) = value;
            }

            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ushort ReadUInt16(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 2) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(ushort*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt24Unsafe(ref byte[] bytes, int offset, int value)
        {
            bytes[offset] = (byte)value;
            bytes[offset + 1] = (byte)(value >> 8);
            bytes[offset + 2] = (byte)(value >> 16);

            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt24(ref byte[] bytes, int offset, int value)
        {
            EnsureCapacity(ref bytes, offset, 3);

            bytes[offset] = (byte)value;
            bytes[offset + 1] = (byte)(value >> 8);
            bytes[offset + 2] = (byte)(value >> 16);

            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt24(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 3) throw new OverflowException();
            return ((int)bytes[offset])
                | (((int)bytes[offset + 1]) << 8)
                | (((int)bytes[offset + 2]) << 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt32Unsafe(ref byte[] bytes, int offset, int value)
        {
            fixed (byte* ptr = bytes)
            {
                *(int*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt32(ref byte[] bytes, int offset, int value)
        {
            EnsureCapacity(ref bytes, offset, 4);

            fixed(byte* ptr = bytes)
            {
                *(int*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int ReadInt32(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 4) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(int*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt32Unsafe(ref byte[] bytes, int offset, uint value)
        {
            fixed (byte* ptr = bytes)
            {
                *(uint*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt32(ref byte[] bytes, int offset, uint value)
        {
            EnsureCapacity(ref bytes, offset, 4);

            fixed(byte* ptr = bytes)
            {
                *(uint*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ReadUInt32(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 4) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(uint*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteFloatUnsafe(ref byte[] bytes, int offset, float value)
        {
            fixed (byte* ptr = bytes)
            {
                *(float*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteFloat(ref byte[] bytes, int offset, float value)
        {
            EnsureCapacity(ref bytes, offset, 4);

            fixed(byte* ptr = bytes)
            {
                *(float*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float ReadFloat(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 4) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(float*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt64Unsafe(ref byte[] bytes, int offset, long value)
        {
            fixed (byte* ptr = bytes)
            {
                *(long*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt64(ref byte[] bytes, int offset, long value)
        {
            EnsureCapacity(ref bytes, offset, 8);

            fixed(byte* ptr = bytes)
            {
                *(long*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe long ReadInt64(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 8) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(long*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt64Unsafe(ref byte[] bytes, int offset, ulong value)
        {
            fixed (byte* ptr = bytes)
            {
                *(ulong*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt64(ref byte[] bytes, int offset, ulong value)
        {
            EnsureCapacity(ref bytes, offset, 8);

            fixed(byte* ptr = bytes)
            {
                *(ulong*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ulong ReadUInt64(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 8) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(ulong*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteDoubleUnsafe(ref byte[] bytes, int offset, double value)
        {
            fixed (byte* ptr = bytes)
            {
                *(double*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteDouble(ref byte[] bytes, int offset, double value)
        {
            EnsureCapacity(ref bytes, offset, 8);

            fixed(byte* ptr = bytes)
            {
                *(double*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double ReadDouble(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 8) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(double*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint EncodeZigZag32(int value) => (uint)((value << 1) ^ (value >> 31));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DecodeZigZag32(uint value) => (int)(value >> 1) ^ -(int)(value & 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong EncodeZigZag64(long value) => (ulong)((value << 1) ^ (value >> 63));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long DecodeZigZag64(ulong value) => (long)(value >> 1) ^ -(long)(value & 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUVarInt(ref byte[] bytes, int offset, uint value)
        {
            EnsureCapacity(ref bytes, offset, 5);

            int i = 0;
            while((value & -128) != 0)
            {
                bytes[offset + i++] = (byte)((value & 0x7f) | 0x80);
                value >>= 7;
            }
            bytes[offset + i] = (byte)value;
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUVarInt(ref byte[] bytes, int offset, out int length)
        {
            uint value = 0;
            for (int i = 0; i < 5; i++)
            {
                if (bytes.Length < offset + i + 1) throw new OverflowException();
                value |= (bytes[offset + i] & 127u) << (i * 7);
                if((bytes[offset + i] & 0x7f) != 0)
                {
                    length = i + 1;
                    return value;
                }
            }
            throw new OverflowException("VarInt too big");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteVarInt(ref byte[] bytes, int offset, int value)
        {
            return WriteUVarInt(ref bytes, offset, EncodeZigZag32(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadVarInt(ref byte[] bytes, int offset, out int length)
        {
            return DecodeZigZag32(ReadUVarInt(ref bytes, offset, out length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUVarLong(ref byte[] bytes, int offset, ulong value)
        {
            EnsureCapacity(ref bytes, offset, 10);

            int i = 0;
            while ((value & 0xffffffffffffff80u) != 0)
            {
                bytes[offset + i++] = (byte)((value & 0x7f) | 0x80);
                value >>= 7;
            }
            bytes[offset + i] = (byte)value;
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUVarLong(ref byte[] bytes, int offset, out int length)
        {
            ulong value = 0;
            for (int i = 0; i < 10; i++)
            {
                if (bytes.Length < offset + i + 1) throw new OverflowException();
                value |= (bytes[offset + i] & 127u) << (i * 7);
                if ((bytes[offset + i] & 0x7f) != 0)
                {
                    length = i + 1;
                    return value;
                }
            }
            throw new OverflowException("VarInt too big");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteVarLong(ref byte[] bytes, int offset, long value)
        {
            return WriteUVarLong(ref bytes, offset, EncodeZigZag64(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadVarLong(ref byte[] bytes, int offset, out int length)
        {
            return DecodeZigZag64(ReadUVarLong(ref bytes, offset, out length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString(ref byte[] bytes, int offset, string value)
        {
            return WriteString(ref bytes, offset, value, Encoding.UTF8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteString(ref byte[] bytes, int offset, string value, Encoding encoding)
        {
            var str_bytes = encoding.GetBytes(value);
            EnsureCapacity(ref bytes, offset, str_bytes.Length);

            fixed (void* src = &str_bytes[0])
            fixed (void* dest = &bytes[offset])
            {
                Buffer.MemoryCopy(src, dest, str_bytes.Length, str_bytes.Length);
            }

            return str_bytes.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(ref byte[] bytes, int offset, int length)
        {
            return ReadString(ref bytes, offset, length, Encoding.UTF8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(ref byte[] bytes, int offset, int length, Encoding encoding)
        {
            if (bytes.Length < offset + length) throw new OverflowException();
            return encoding.GetString(bytes, offset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteStringUInt16(ref byte[] bytes, int offset, string value)
        {
            return WriteStringUInt16(ref bytes, offset, value, Encoding.UTF8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteStringUInt16(ref byte[] bytes, int offset, string value, Encoding encoding)
        {
            var str_bytes = encoding.GetBytes(value);
            if (str_bytes.Length > ushort.MaxValue) throw new ArgumentOutOfRangeException("length > USHORT_MAX");
            EnsureCapacity(ref bytes, offset, 2 + str_bytes.Length);

            fixed(byte* ptr = bytes)
            {
                *(ushort*)(ptr + offset) = (ushort)str_bytes.Length;
            }

            fixed (void* src = &str_bytes[0])
            fixed (void* dest = &bytes[offset + 2])
            {
                Buffer.MemoryCopy(src, dest, str_bytes.Length, str_bytes.Length);
            }

            return 2 + str_bytes.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadStringUInt16(ref byte[] bytes, int offset, out int length)
        {
            return ReadStringUInt16(ref bytes, offset, out length, Encoding.UTF8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string ReadStringUInt16(ref byte[] bytes, int offset, out int length, Encoding encoding)
        {
            if (bytes.Length < offset + 2) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                var len = *(ushort*)(ptr + offset);
                length = 2 + len;
                if (bytes.Length < offset + length) throw new OverflowException();
                return encoding.GetString(bytes, offset + 2, len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteStringUVarInt(ref byte[] bytes, int offset, string value)
        {
            return WriteStringUVarInt(ref bytes, offset, value, Encoding.UTF8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteStringUVarInt(ref byte[] bytes, int offset, string value, Encoding encoding)
        {
            var str_bytes = encoding.GetBytes(value);
            int headerLen = WriteUVarInt(ref bytes, offset, (uint)str_bytes.Length);
            EnsureCapacity(ref bytes, offset + headerLen, str_bytes.Length);

            fixed (void* src = &str_bytes[0])
            fixed (void* dest = &bytes[offset + headerLen])
            {
                Buffer.MemoryCopy(src, dest, str_bytes.Length, str_bytes.Length);
            }

            return headerLen + str_bytes.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadStringUVarInt(ref byte[] bytes, int offset, out int length)
        {
            return ReadStringUVarInt(ref bytes, offset, out length, Encoding.UTF8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadStringUVarInt(ref byte[] bytes, int offset, out int length, Encoding encoding)
        {
            var len = checked((int)ReadUVarInt(ref bytes, offset, out length));
            if (bytes.Length < offset + len + length) throw new OverflowException();
            string str = encoding.GetString(bytes, offset + length, len);
            length += len;
            return str;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteGuid(ref byte[] bytes, int offset, Guid value)
        {
            EnsureCapacity(ref bytes, offset, 16);

            fixed (byte* ptr = bytes)
            {
                *(Guid*)(ptr + offset) = value;
            }

            return 16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Guid ReadGuid(ref byte[] bytes, int offset)
        {
            if (bytes.Length < offset + 16) throw new OverflowException();
            fixed (byte* ptr = bytes)
            {
                return *(Guid*)(ptr + offset);
            }
        }
    }
}
