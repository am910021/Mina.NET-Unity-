using Mina.Core.Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using YuriWorkSpace;

public class PongHandle : AbstractClientPacketHandle
{
    public float ping = 9999;


    public override PacketOpcode.SERVER getOpcode()
    {
        return PacketOpcode.SERVER.PONG;
    }

    protected override void handlePacket( PacketInStream packet)
    {
        long oldTicks = packet.readLong();
        DateTime dt = DateTime.UtcNow;
        long newTicks = dt.Ticks;
        ping = (newTicks - oldTicks) / 10000;
    }
}
