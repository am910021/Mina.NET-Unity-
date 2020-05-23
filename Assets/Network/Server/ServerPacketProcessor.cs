using Mina.Core.Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace YuriWorkSpace
{
    public sealed class ServerPacketProcessor : MonoBehaviour
    {
        [SerializeField]
        private AbstractServerPacketHandle[] handlers;

        private AbstractServerPacketHandle[] registered;
        private static readonly object objectLock = new object();

        private static AbstractServerPacketHandle pingHandle;

        private static ServerPacketProcessor s_Instance;
        public static ServerPacketProcessor Instance
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
            s_Instance = FindObjectOfType<ServerPacketProcessor>();

            if (ValidateFail())
            {
                Debug.LogError("ServerPacketProcessor有重復的項目");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private void Start()
        {
            pingHandle = this.gameObject.AddComponent<PingHandle>();
            RegisterHandler(PacketOpcode.CLIENT.PING, pingHandle);
            foreach (AbstractServerPacketHandle handle in handlers)
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
                throw new Exception("ServerPacketProcessor有重復的項目");
            }

        }


        private bool ValidateFail()
        {
            if (handlers.Length == 0)
            {
                return false;
            }

            PacketOpcode.SERVER max = Enum.GetValues(typeof(PacketOpcode.SERVER)).Cast<PacketOpcode.SERVER>().Last();
            bool[] test = new bool[(int)max + 1];
            foreach (AbstractServerPacketHandle handle in handlers)
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


        private ServerPacketProcessor()
        {
            PacketOpcode.SERVER max = Enum.GetValues(typeof(PacketOpcode.SERVER)).Cast<PacketOpcode.SERVER>().Last();
            registered = new AbstractServerPacketHandle[(short)max + 1];
        }


        public void DelegateHandler(ClientObject client, PacketInStream packet)
        {
            lock (objectLock)
            {
                short head = packet.readShort();
                AbstractServerPacketHandle handler = GetHandler(head);
                if (!handler.IsActive(client))
                {
                    return;
                }
                if (handler.IsNeedDebug())
                {
                    packet.printHex("Server");
                }
                handler.DelegatePacket(new InPackageObject(client, packet));
            }
        }

        private AbstractServerPacketHandle GetHandler(short packetId)
        {
            if (packetId > registered.Length)
            {
                return null;
            }

            AbstractServerPacketHandle handler = registered[packetId];
            if (handler != null)
            {
                return handler;
            }
            return null;
        }

        public void RegisterHandler(PacketOpcode.CLIENT code, AbstractServerPacketHandle handler)
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
            registered = new AbstractServerPacketHandle[registered.Length];
            RegisterHandler(PacketOpcode.CLIENT.PING, pingHandle);

        }



    }
}