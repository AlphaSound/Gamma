using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet
{
    public class Constants
    {
        public static readonly Guid Magic = 
            new Guid(new byte[16] { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 });
    }
}
