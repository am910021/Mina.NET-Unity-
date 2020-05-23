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
                this.handlePacket((PacketInStream)_stack.Dequeue());
            }
        }

        public abstract PacketOpcode.SERVER getOpcode();

        protected abstract void handlePacket(PacketInStream packet);

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