using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuriWorkSpace
{


    public class PingSender : MonoBehaviour
    {
        // Start is called before the first frame update
        const float pongTime = 2.0f; //second
        float countTime = 0;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            countTime += Time.deltaTime;
            if (countTime >= pongTime)
            {
                countTime = 0.0f;
                PacketOutStream packet = ClientCreatePacket.pong();
                ClientHelper.Instance.Send(packet);
            }
        }
    }
}