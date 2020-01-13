using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MCBEProtocol.RakNet.StreamExtensions
{
    public static class Extensions
    {
        public static unsafe void WriteAddress(this BinaryStream stream, IPEndPoint address)
        {
            if(address.AddressFamily == AddressFamily.InterNetwork)
            {
                stream.WriteByte(0x04);
                var bytes = address.Address.GetAddressBytes();
                fixed(byte* ptr = bytes)
                {
                    *(uint*)ptr = ~*(uint*)ptr;
                }
                stream.WriteBytes(bytes);
                stream.WriteInt16((short)address.Port);
            }
            else if(address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                stream.WriteByte(0x06);
                stream.WriteUInt16(23);//AddressFamily.InterNetworkV6
                stream.WriteInt16((short)address.Port);
                stream.WriteBytes(address.Address.GetAddressBytes());
            }
            else
            {
                throw new NotSupportedException("Currently only supports IPv4 and IPv6");
            }
        }

        public static unsafe IPEndPoint ReadAddress(this BinaryStream stream)
        {
            IPAddress ip = IPAddress.Any;
            int port = 0;

            byte version = stream.ReadByte();
            if(version == 0x04)
            {
                var bytes = stream.ReadBytes(4);
                fixed(byte* ptr = bytes)
                {
                    *(uint*)ptr = ~*(uint*)ptr;
                }
                ip = new IPAddress(bytes);
                port = stream.ReadInt16();
            }
            else if(version == 0x06)
            {
                if(stream.ReadUInt16() != 23)
                {
                    //TODO: Logging
                }
                port = stream.ReadInt16();
                ip = new IPAddress(stream.ReadBytes(16));
            }
            else
            {
                //TODO: Logging
            }

            return new IPEndPoint(ip, port);
        }
    }
}
