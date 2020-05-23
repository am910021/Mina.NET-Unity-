using Mina.Core.Session;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace YuriWorkSpace
{
    public class CubeHandle : AbstractClientPacketHandle
    {
        public GameObject obj;

        public override PacketOpcode.SERVER getOpcode()
        {
            return PacketOpcode.SERVER.RESPONSE_CUBE;
        }

        protected override void handlePacket( PacketInStream packet)
        {
            Vector3 pos = new Vector3(packet.readFloat(), packet.readFloat(), packet.readFloat());
            Vector3 rot = new Vector3(packet.readFloat(), packet.readFloat(), packet.readFloat());
            Vector3 scl = new Vector3(packet.readFloat(), packet.readFloat(), packet.readFloat());
            obj.transform.position = pos;
            obj.transform.eulerAngles = rot;
            obj.transform.localScale = scl;
        }
        public override bool IsNeedDebug()
        {
            return true;
        }
    }
}
