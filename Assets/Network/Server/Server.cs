using UnityEngine;

namespace YuriWorkSpace
{
    public class Server : MonoBehaviour
    {

        // Use this for initialization
        ServerHelper helper = ServerHelper.Instance;

        public string address = "127.0.0.1";
        public int port = 8007;
        public int maxConnection = 10;

        void OnEnable()
        {
            helper.Bind(address, port); //設定ip 和 port
            helper.SetMaxConnect(maxConnection); //最大連線量
            helper.Start(); //啟動伺服器
        }

        void Start()
        {
            helper.Bind(address, port); //設定ip 和 port
            helper.SetMaxConnect(maxConnection); //最大連線量
            helper.Start(); //啟動伺服器
        }

        // Update is called once per frame
        void Update()
        {

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