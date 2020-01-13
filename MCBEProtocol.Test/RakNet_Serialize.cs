using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MCBEProtocol.RakNet;

namespace MCBEProtocol.Test
{
    public class RakNet_Serialize
    {
        public RakNet_Serialize()
        {
            //Protocol.Initialize();
        }

        [Fact]
        public void Test()
        {
            var bs = new BinaryStream();

            new UnconnectedPong
            {
                Timestamp = 0x0123456789ff,
                PongId = 1,
                Magic = Constants.Magic,
                ServerName = "MCBEProtocol Test Server"
            }.Encode(bs);
            Assert.Equal(bs.GetBuffer(), new byte[]
            {
                // Timestamp
                0xff, 0x89, 0x67, 0x45, 0x23, 0x01, 0x00, 0x00,
                // PongId
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                // Magic
                0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78,
                // ServerName Length
                0x18, 0x00,
                // ServerName
                0x4d, 0x43, 0x42, 0x45, 0x50, 0x72, 0x6f, 0x74, 0x6f, 0x63, 0x6f, 0x6c, 0x20, 0x54, 0x65, 0x73, 0x74, 0x20, 0x53, 0x65, 0x72, 0x76, 0x65, 0x72
            });
            bs.Clear();

            new UnconnectedPing
            {
                Timestamp = 0x0123456789ff,
                Magic = Constants.Magic,
                ClientId = 0xfedcba98765432
            }.Encode(bs);
            Assert.Equal(bs.GetBuffer(), new byte[]
            {
                // Timestamp
                0xff, 0x89, 0x67, 0x45, 0x23, 0x01, 0x00, 0x00,
                // Magic
                0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78, 
                // ClientId
                0x32, 0x54, 0x76, 0x98, 0xba, 0xdc, 0xfe, 0x00
            });
            bs.Clear();
        }
    }
}
