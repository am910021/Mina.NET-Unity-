using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuriWorkSpace
{
    class ClientCreatePacket
    {
        
        public static PacketOutStream pong()
        {
            DateTime dt = DateTime.UtcNow;
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.CLIENT.PING);
            packet.writeLong(dt.Ticks);
            return packet;
        }


        public static PacketOutStream request_sence()
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.CLIENT.REQUEST_SCENE);
            return packet;
        }

        public static PacketOutStream request_cube()
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.CLIENT.REQUEST_CUBE);
            return packet;
        }


        public static PacketOutStream request_test()
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)99);
            return packet;
        }

    }
}