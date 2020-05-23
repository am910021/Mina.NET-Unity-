using Mina.Core.Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Collections;
using UnityEngine;

namespace YuriWorkSpace
{
    public class PingHandle : AbstractServerPacketHandle
    {

        public override PacketOpcode.CLIENT getOpcode()
        {
            return PacketOpcode.CLIENT.PING;
        }

        protected override void handlePacket(ClientObject client, PacketInStream packet)
        {
            long ticks = packet.readLong();
            client.Announce(ServerCreatePacket.ping(ticks));
        }

    }

}
