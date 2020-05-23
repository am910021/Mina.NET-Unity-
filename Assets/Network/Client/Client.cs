using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuriWorkSpace
{

    public class Client : MonoBehaviour
    {

        // Use this for initialization
        // Use this for initialization
        ClientHelper helper = ClientHelper.Instance;

        public bool autoReConnect = false; //斷線自動重線
        public string address = "127.0.0.1";
        public int port = 8007;



        void OnEnable()
        {
            helper.Connector(address, port); //設定ip 和 port
            helper.Start(); //啟動連線
        }

        void Start()
        {
            helper.Connector(address, port); //設定ip 和 port
            helper.Start(); //啟動連線
        }

        // Update is called once per frame
        void Update()
        {
            if (autoReConnect && !helper.IsConnected())
            {
                helper.Connector(address, port); //設定ip 和 port
                helper.Start(); //啟動連線
            }
        }

        void OnDisable()
        {
            helper.Stop();
        }

        void OnApplicationPause()
        {
            helper.Stop();
        }

        void OnDestroy()
        {
            helper.Stop();
        }
    }
}
