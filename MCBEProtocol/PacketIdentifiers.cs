using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MCBEProtocol
{
    public class PacketIdentifiers
    {
        private ReaderWriterLockSlim rwlock;
        private Type[] Identifiers;

        public PacketIdentifiers()
        {
            rwlock = new ReaderWriterLockSlim();
            Identifiers = new Type[byte.MaxValue];
        }

        public void Set<T>(byte id) where T : Packet<T>, new()
        {
            var type = typeof(T);
            try
            {
                rwlock.EnterWriteLock();
                Identifiers[id] = typeof(T);//
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        public byte Get<T>() where T : Packet<T>, new()
        {
            var type = typeof(T);
            try
            {
                rwlock.EnterReadLock();
                for (int i = 0; i < byte.MaxValue; i++)
                {
                    if (Identifiers[i].Equals(type))
                    {
                        return (byte)i;
                    }
                }
                throw new KeyNotFoundException();
            }
            finally
            {
                rwlock.ExitReadLock();
            }
        }
    }
}
