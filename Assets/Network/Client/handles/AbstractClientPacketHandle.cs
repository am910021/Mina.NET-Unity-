using Mina.Core.Session;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;


namespace YuriWorkSpace
{

    public abstract class AbstractClientPacketHandle : MonoBehaviour
    {
        public Queue _stack = new Queue();

        [SerializeField]
        private bool isDebug = false;

        public void DelegatePacket(InPackageObject packet)
        {
            _stack.Enqueue(packet);
        }
        protected void FixedUpdate()
        {
            if (_stack.Count > 0)
            {
                InPackageObject package = (InPackageObject)_stack.Dequeue();
                this.handlePacket(package.client, package.packet);
                package = null;
            }
        }

        public abstract PacketOpcode.SERVER getOpcode();

        protected abstract void handlePacket(ClientObject client, PacketInStream packet);

        public virtual bool IsActive(ClientObject client)
        {
            return true;
        }

        public virtual bool IsNeedDebug()
        {
            return this.isDebug;
        }

    }

}