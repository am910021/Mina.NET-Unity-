using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace YuriWorkSpace
{
    public class PacketDecoderNew : IDisposable
    {
        private bool _disposed = false;

        ~PacketDecoderNew() => Dispose(false);

        byte[] packet;

        public PacketDecoderNew(byte[] packet)
        {
            this.packet = packet;
        }

        public bool Available()
        {
            uint contentSize = BitConverter.ToUInt32(packet.Take(4).ToArray(), 0);
            byte[] content = packet.Skip(4).Take(packet.Length - 4).ToArray();
            return contentSize == content.Length;
        }

        public PacketInStream GetPacket()
        {
            uint contentSize = BitConverter.ToUInt32(packet.Take(4).ToArray(), 0);
            byte[] content = packet.Skip(4).Take(packet.Length - 4).ToArray();
            if (contentSize == content.Length)
            {
                return new PacketInStream(content);
            }
            return new PacketInStream(new byte[0]);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }
    }
}
