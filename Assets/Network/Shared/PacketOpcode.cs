
namespace YuriWorkSpace
{
    public class PacketOpcode
    {
        public enum SERVER
        {
            // 伺服器控制代號 1 到 32,767
            PONG = 0,
            RESPONSE_STRING = 1,
            RESPONSE_INT = 2,
            RESPONSE_FLOAT = 3,
            RESPONSE_SCENE = 4,
            RESPONSE_NORMAL = 5,
            RESPONSE_CUBE = 6,

        }
        public enum CLIENT
        {
            // 客戶端控制代號 1 到 32,767
            PING = 0,
            REQUEST_STRING = 1,
            REQUEST_INT = 2,
            REQUEST_FLOAT = 3,
            REQUEST_SCENE = 4,
            REQUEST_NORMAL = 5,
            REQUEST_CUBE = 6,
        }
    }
}