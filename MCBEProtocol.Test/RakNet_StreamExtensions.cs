using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Xunit;
using MCBEProtocol.RakNet.StreamExtensions;

namespace MCBEProtocol.Test
{
    public class RakNet_StreamExtensions
    {
        [Fact]
        public void Test()
        {
            var bs = new BinaryStream();
            bs.WriteAddress(new IPEndPoint(IPAddress.Parse("192.168.1.10"), 19132));
            Assert.Equal(bs.GetBuffer(), new byte[7] { 0x04, 0x3f, 0x57, 0xfe, 0xf5, 0xbc, 0x4a });
            bs.Clear();

            bs.WriteAddress(new IPEndPoint(IPAddress.Parse("2404:6800:4004:0810:0000:0000:0000:2004"), 19132));
            Assert.Equal(bs.GetBuffer(), new byte[21]
            {
                0x06,
                0x17, 0x00,
                0xbc, 0x4a,
                0x24, 0x04, 0x68, 0x00, 0x40, 0x04, 0x08, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x04
            });
        }
    }
}
