using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YuriWorkSpace;

public class TestSender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PacketOutStream packet = ClientCreatePacket.request_test();
        ClientHelper.Instance.Send(packet);
    }
}
