using Mina.Core.Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace YuriWorkSpace
{
    public class InPackageObject
    {
        public readonly ClientObject client;
        public readonly  PacketInStream packet;
        public InPackageObject(ClientObject client, PacketInStream packet)
        {
            this.client = client;
            this.packet = packet;
        }

        private InPackageObject()
        {

        }
    }

    public class OutPackageObject
    {
        public readonly ClientObject client;
        public readonly PacketOutStream packet;
        public OutPackageObject(ClientObject client, PacketOutStream packet)
        {
            this.client = client;
            this.packet = packet;
        }

        private OutPackageObject()
        {

        }
    }


}
