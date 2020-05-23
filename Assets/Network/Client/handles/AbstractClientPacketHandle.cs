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

        public void DelegatePacket(PacketInStream packet)
        {
            _stack.Enqueue(packet);
        }
        protected void FixedUpdate()
        {
            if (_stack.Count > 0)
            {
                PacketInStream packet = (PacketInStream)_stack.Dequeue();
                this.HandlePacket(packet);
                packet.Dispose();
            }
        }

        public abstract PacketOpcode.SERVER getOpcode();

        protected abstract void HandlePacket(PacketInStream packet);

        public virtual bool IsActive()
        {
            return true;
        }

        public virtual bool IsNeedDebug()
        {
            return this.isDebug;
        }

    }

}