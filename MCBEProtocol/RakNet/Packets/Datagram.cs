using System;
using System.Collections.Generic;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    class Datagram : Packet<Datagram>
    {
        public int SequenceNumber { get; set; }

        public IEnumerable<Message> Messages { get; set; }

        public override void Encode(BinaryStream stream)
        {
            stream.WriteInt24(SequenceNumber);
            foreach (var message in Messages)
            {
                message.Encode(stream);
            }
        }

        public override void Decode(BinaryStream stream)
        {
            SequenceNumber = stream.ReadInt24();
            var messages = new List<Message>();
            while(stream.Offset < stream.Length)
            {
                var message = new Message();
                message.Decode(stream);
                messages.Add(message);
            }
            Messages = messages;
        }
    }
}
