using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCBEProtocol.RakNet.Packets
{
    public class Acknowledge<T> : Packet<T> where T : new()
    {
        public IEnumerable<int> Numbers { get; set; }

        public override void Encode(BinaryStream stream)
        {
            var nums = Numbers.ToList();
            nums.Sort();
            if (nums.Count == 0)
            {
                stream.WriteUInt16(0);
                return;
            }
            var list = new List<(int start, int end)>();
            {
                int start = nums[0];
                for (int i = 1; i < nums.Count; i++)
                {
                    if (nums[i] - start <= 1) continue;
                    list.Add((start, nums[i - 1]));
                    start = nums[i];
                }
                list.Add((start, nums[nums.Count - 1]));

                if (list.Count > ushort.MaxValue)
                {
                    list.RemoveRange(0, list.Count - ushort.MaxValue);
                }
            }

            stream.WriteUInt16((ushort)list.Count);
            foreach (var (start, end) in list)
            {
                if (end - start > 0)
                {
                    stream.WriteBoolean(false);//is single
                    stream.WriteInt24(start);
                    stream.WriteInt24(end);
                }
                else
                {
                    stream.WriteBoolean(true);
                    stream.WriteInt24(start);
                }
            }
        }

        public override void Decode(BinaryStream stream)
        {
            var count = stream.ReadUInt16();
            var list = new SortedSet<int>();
            for (int i = 0; i < count; i++)
            {
                if (stream.ReadBoolean())//is single
                {
                    list.Add(stream.ReadInt24());
                }
                else
                {
                    var start = stream.ReadInt24();
                    var end = stream.ReadInt24();
                    foreach (var n in Enumerable.Range(start, end - start))//
                    {
                        list.Add(n);
                    }
                }
            }
        }
    }

    public class ACK : Acknowledge<ACK> { }

    public class NACK : Acknowledge<NACK> { }
}
