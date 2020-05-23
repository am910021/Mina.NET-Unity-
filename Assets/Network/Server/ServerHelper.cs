using Mina.Core.Service;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.Serialization;
using Mina.Filter.Codec.TextLine;
using Mina.Filter.Logging;
using Mina.Transport.Socket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YuriWorkSpace
{

    public sealed class ServerHelper
    {

        // Use this for initialization

        IoAcceptor acceptor;
        Thread thread;
        bool isStarted = false;

        IPEndPoint endPoint;
        IPAddress address;
        int port;
        int maxConnect = 10;

        List<ClientObject> clients;

        private static string SERVER_KEY = "qDOWOf0rmudy79I2CNP7R1IrIS4UrSVr";

        private static volatile ServerHelper instance;
        private static object syncRoot = new System.Object();

        public static ServerHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ServerHelper();
                        }
                    }
                }

                return instance;
            }
        }

        private ServerHelper() { }



        public void Bind(string host, int port)
        {
            if (!IPAddress.TryParse(host, out address))
            {
                address = Dns.GetHostEntry(host).AddressList[0];
            }
            endPoint = new IPEndPoint(address, port);
            this.port = port;
        }


        public void Start()
        {
            if (isStarted)
            {
                return;
            }

            thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        public void Stop()
        {
            if (!isStarted)
                return;

            acceptor.Unbind();
            CloseAllClient();
            acceptor.Dispose();
            thread.Abort();
            isStarted = false;
        }




        void Run()
        {
            clients = new List<ClientObject>();

            acceptor = new AsyncSocketAcceptor();

            acceptor.FilterChain.AddLast("logger", new LoggingFilter());
            // acceptor.FilterChain.AddLast("codec", new ProtocolCodecFilter(new TextLineCodecFactory(Encoding.UTF8)));
            acceptor.FilterChain.AddLast("codec", new ProtocolCodecFilter(new ObjectSerializationCodecFactory()));
            acceptor.MessageReceived += MessageReceived;
            acceptor.MessageSent += MessageSent;
            acceptor.SessionIdle += SessionIdle;
            acceptor.SessionOpened += SessionOpened;
            acceptor.SessionClosed += SessionClosed;
            acceptor.ExceptionCaught += ExceptionCaught;
            acceptor.Bind(endPoint);
            isStarted = true;

            Debug.Log(String.Format("Server listen on {0} {1}", endPoint.Address, endPoint.Port));
        }

        private void SessionOpened(object sender, IoSessionEventArgs e)
        {
            IPEndPoint remoreIP = (IPEndPoint)e.Session.RemoteEndPoint;
            if (clients.Count >= maxConnect)
            {
                Debug.Log(String.Format("Reject client connecte from {0} {1}, reason Reason: connection to reach the upper limit.", remoreIP.Address, remoreIP.Port));
                e.Session.Write(System.Text.Encoding.UTF8.GetBytes("Reject connect, Reason: connection to reach the upper limit."));
                e.Session.CloseNow();
                return;
            }
            Debug.Log(String.Format("client connected  from {0} {1}", remoreIP.Address, remoreIP.Port));
            ClientObject client = new ClientObject(e.Session);
            e.Session.SetAttribute(SERVER_KEY, client);
            
            clients.Add(client);
        }

        private void SessionClosed(object sender, IoSessionEventArgs e)
        {
            IPEndPoint remoreIP = (IPEndPoint)e.Session.RemoteEndPoint;
            Debug.Log(String.Format("client disconnect from {0} {1}", remoreIP.Address, remoreIP.Port));

            ClientObject client = (ClientObject)e.Session.GetAttribute(SERVER_KEY);
            clients.Remove(client);
            e.Session.RemoveAttribute(SERVER_KEY);
            client.Dispose();
        }

        private void SessionIdle(object sender, IoSessionIdleEventArgs e)
        {
            e.Session.Close(true);
            Debug.Log("server SessionIdle");
        }

        private void ExceptionCaught(object sender, IoSessionExceptionEventArgs e)
        {
            Debug.Log(e.Exception);
            e.Session.Close(true);
            Debug.Log("server ExceptionCaught");

        }

        private void MessageSent(object sender, IoSessionMessageEventArgs e)
        {

        }

        private void MessageReceived(object sender, IoSessionMessageEventArgs e)
        {
            byte[] data = (byte[])e.Message;
            PacketDecoderNew decoder = new PacketDecoderNew(data);
            if (!decoder.Available())
                return;
            ClientObject client = (ClientObject)e.Session.GetAttribute(SERVER_KEY);
            ServerPacketProcessor.Instance.DelegateHandler(client, decoder.GetPacket());
            decoder.Dispose();
        }


        public void Broadcast(PacketOutStream packet)
        {
            acceptor.Broadcast(packet.getPackets()); ;
            packet.Dispose();
        }

        private void CloseAllClient()
        {
            foreach (IoSession session in clients)
            {
                ClientObject client = (ClientObject)session.GetAttribute(SERVER_KEY);
                session.CloseNow();
                clients.Remove(client);
                client.Dispose();
            }
        }
    }
}
