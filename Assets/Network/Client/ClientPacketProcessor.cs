using Mina.Core.Session;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Unity.Collections;
using UnityEngine;

namespace YuriWorkSpace
{
    public sealed class ClientPacketProcessor : MonoBehaviour
    {
        [SerializeField]
        private AbstractClientPacketHandle[] handlers;

        private AbstractClientPacketHandle[] registered;
        private static readonly object objectLock = new object();
        private static AbstractClientPacketHandle pongHandler;

        private static ClientPacketProcessor s_Instance;
        public static ClientPacketProcessor Instance
        {
            get
            {
                if (s_Instance != null)
                {
                    return s_Instance;      // 已經註冊的Singleton物件
                }

                //尋找已經在Scene的Singleton物件:
                if (s_Instance != null)
                {
                    return s_Instance;
                }

                return s_Instance;
            }
        }

        private void OnEnable()
        {
            s_Instance = FindObjectOfType<ClientPacketProcessor>();

            if (ValidateFail())
            {
                Debug.LogError("ClientPacketProcessor有重復的項目");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private void Start()
        {
            pongHandler = this.gameObject.AddComponent<PongHandle>();
            RegisterHandler(PacketOpcode.SERVER.PONG, pongHandler);
            foreach (AbstractClientPacketHandle handle in handlers)
            {
                if (handle == null)
                {
                    continue;
                }
                RegisterHandler(handle.getOpcode(), handle);
            }
        }
        void OnValidate()
        {
            if (ValidateFail())
            {
                throw new Exception("ClientPacketProcessor有重復的項目");
            }

        }


        private bool ValidateFail()
        {
            if(handlers.Length == 0)
            {
                return false;
            }

            PacketOpcode.SERVER max = Enum.GetValues(typeof(PacketOpcode.SERVER)).Cast<PacketOpcode.SERVER>().Last();
            bool[] test = new bool[(int)max + 1];
            foreach (AbstractClientPacketHandle handle in handlers)
            {
                if (handle == null)
                {
                    continue;
                }

                if (test[(int)handle.getOpcode()])
                {
                    return true;
                }
                else
                {
                    test[(int)handle.getOpcode()] = true;
                }
            }
            return false;
        }


        private ClientPacketProcessor()
        {
            PacketOpcode.SERVER max = Enum.GetValues(typeof(PacketOpcode.SERVER)).Cast<PacketOpcode.SERVER>().Last();
            registered = new AbstractClientPacketHandle[(short)max + 1];
        }


        public void DelegateHandler(ClientObject client, PacketInStream packet)
        {
            lock (objectLock)
            {
                short head = packet.readShort();
                AbstractClientPacketHandle handler = GetHandler(head);

                if (!handler.IsActive(client))
                    return;

                if (handler.IsNeedDebug())
                {
                    packet.printHex("Client");
                }
                handler.DelegatePacket(new InPackageObject(client, packet));
            }
        }

        private AbstractClientPacketHandle GetHandler(short packetId)
        {
            if (packetId > registered.Length)
            {
                return null;
            }

            AbstractClientPacketHandle handler = registered[packetId];
            if (handler != null)
            {
                return handler;
            }
            return null;
        }

        public void RegisterHandler(PacketOpcode.SERVER code, AbstractClientPacketHandle handler)
        {
            try
            {
                registered[(short)code] = handler;
            }
            catch (Exception e)
            {
                Debug.LogError("Error registering handler - " + code);
                Debug.LogError(e.Message);
            }
        }

        public void reset()
        {
            registered = new AbstractClientPacketHandle[registered.Length];
            RegisterHandler(PacketOpcode.SERVER.PONG, pongHandler);

        }



    }
}