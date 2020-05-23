using Mina.Core.Session;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using YuriWorkSpace;

public class SenceHandle : AbstractServerPacketHandle
{
    public override PacketOpcode.CLIENT getOpcode()
    {
        return PacketOpcode.CLIENT.REQUEST_SCENE;
    }

    protected override void handlePacket(ClientObject client, PacketInStream packet)
    {
        client.Announce(ServerCreatePacket.response_scene(SceneManager.GetActiveScene().name));
    }
}
