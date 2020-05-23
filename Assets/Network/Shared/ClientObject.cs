using Mina.Core.Session;
using System.Net;

namespace YuriWorkSpace
{

    public class ClientObject
    {
        readonly IoSession session;

        public ClientObject(IoSession session)
        {
            this.session = session;
        }

        public void Announce(PacketOutStream packet)
        {
            session.Write(packet.getPackets());
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
    }
}
