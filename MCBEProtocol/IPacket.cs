using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol
{
    public interface IPacket
    {
        void Encode(BinaryStream stream);

        void Decode(BinaryStream stream);
    }
}
