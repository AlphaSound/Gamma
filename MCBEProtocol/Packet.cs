using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol
{
    public abstract class Packet<T> where T : new()
    {
        public abstract void Encode(BinaryStream stream);

        public abstract void Decode(BinaryStream stream);
    }
}
