using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet
{
    public class UnconnectedPong
    {
        public long Timestamp { get; set; }

        public ulong PongId { get; set; }

        public Guid Magic { get; set; }

        public string ServerName { get; set; }

        public UnconnectedPong() { }

        public UnconnectedPong(long timestamp, ulong pongId, Guid magic, string serverName)
        {
            Timestamp = timestamp;
            PongId = pongId;
            Magic = magic;
            ServerName = serverName;
        }
    }
}
