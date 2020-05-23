using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YuriWorkSpace;

public class CubeSyncSender : MonoBehaviour
{
    public GameObject obj;

    private void FixedUpdate()
    {
        Transform transform = obj.gameObject.transform;
        if (transform.hasChanged)
        {
            ServerHelper.Instance.Broadcast(ServerCreatePacket.response_cube(transform));
        }
    }
}
