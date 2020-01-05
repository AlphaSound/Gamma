using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace MCBEProtocol.RakNet
{
    public class UnconnectedPing
    {
        public long Timestamp { get; set; }

        public Guid Magic { get; set; }

        public ulong ClientId { get; set; }

        public UnconnectedPing() { }

        public UnconnectedPing(long timestamp, Guid magic, ulong clientId)
        {
            Timestamp = timestamp;
            Magic = magic;
            ClientId = clientId;
        }
    }
}
