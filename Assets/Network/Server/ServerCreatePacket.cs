using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuriWorkSpace
{
    class ServerCreatePacket
    {
        public static PacketOutStream ping(long ticks)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.PONG);
            packet.writeLong(ticks);
            return packet;
        }

        //SYN SCENE
        public static PacketOutStream response_scene(string sceneName)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.RESPONSE_SCENE);
            packet.writeString(sceneName);
            return packet;
        }


        public static PacketOutStream normal(string msgTest)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.RESPONSE_NORMAL);
            packet.writeString(msgTest);
            return packet;
        }


        public static PacketOutStream response_string(string[] arrs)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.RESPONSE_STRING);
            packet.writeInt(arrs.Length);
            foreach(string s in arrs)
            {
                packet.writeString(s);
            }
            return packet;
        }

        public static PacketOutStream response_int(int[] arrs)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.RESPONSE_INT);
            packet.writeInt(arrs.Length);
            foreach (int s in arrs)
            {
                packet.writeInt(s);
            }
            return packet;
        }

        public static PacketOutStream response_float(float[] arrs)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.RESPONSE_FLOAT);
            packet.writeInt(arrs.Length);
            foreach (float s in arrs)
            {
                packet.writeFloat(s);
            }
            return packet;
        }

        public static PacketOutStream response_cube(Transform arrs)
        {
            PacketOutStream packet = new PacketOutStream();
            packet.writeShort((short)PacketOpcode.SERVER.RESPONSE_CUBE);
            
            packet.writeFloat(arrs.localPosition.x);
            packet.writeFloat(arrs.localPosition.y);
            packet.writeFloat(arrs.localPosition.z);

            packet.writeFloat(arrs.localEulerAngles.x);
            packet.writeFloat(arrs.localEulerAngles.y);
            packet.writeFloat(arrs.localEulerAngles.z);

            packet.writeFloat(arrs.localScale.x);
            packet.writeFloat(arrs.localScale.y);
            packet.writeFloat(arrs.localScale.z);

            return packet;
        }
    }
}