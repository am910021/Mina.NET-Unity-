using Mina.Core.Session;
using System;
using System.Net;

namespace YuriWorkSpace
{

    public class ClientObject : IDisposable
    {
        private bool _disposed = false;

        ~ClientObject() => Dispose(false);

        readonly IoSession session;

        public ClientObject(IoSession session)
        {
            this.session = session;
        }

        public void Announce(PacketOutStream packet)
        {
            session.Write(packet.getPackets());
            packet.Dispose();
        }


        public string GetAddress()
        {
            IPEndPoint remoreIP = (IPEndPoint)session.RemoteEndPoint;
            return remoreIP.Address.ToString();
        }

        public int Port()
        {
            IPEndPoint remoreIP = (IPEndPoint)session.RemoteEndPoint;
            return remoreIP.Port;
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
