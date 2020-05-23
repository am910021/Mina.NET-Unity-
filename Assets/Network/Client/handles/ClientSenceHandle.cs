using Mina.Core.Session;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YuriWorkSpace
{
    public class ClientSenceHandle : AbstractClientPacketHandle
    {
        public override PacketOpcode.SERVER getOpcode()
        {
            return PacketOpcode.SERVER.RESPONSE_SCENE;
        }

        protected override void HandlePacket(PacketInStream packet)
        {
            string sence = packet.readString();
            SceneManager.LoadSceneAsync(sence);
        }
    }
}