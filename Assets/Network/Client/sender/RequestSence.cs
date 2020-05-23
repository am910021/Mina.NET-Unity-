using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuriWorkSpace
{

    public class RequestSence : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ClientHelper.Instance.Send(ClientCreatePacket.request_sence());
        }

    }
}
