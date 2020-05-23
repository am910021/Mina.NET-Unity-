
using Mina.Core.Future;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.Serialization;
using Mina.Filter.Codec.TextLine;
using Mina.Filter.Logging;
using Mina.Transport.Socket;
using System;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YuriWorkSpace
{

    public class ClientHelper
    {

        // Use this for initialization
        private readonly int PORT = 8007;
        private readonly long CONNECT_TIMEOUT = 30 * 1000L; // 30 seconds

        // Set this to false to use object serialization instead of custom codec.
        IoSession session;
        private AsyncSocketConnector connector;
        Thread thread;
        bool isConnected = false;
        bool isConnecting = false;

        IPEndPoint endPoint;
        IPAddress address;
        int port;


        private static volatile ClientHelper instance;
        private static object syncRoot = new System.Object();

        public static ClientHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ClientHelper();
                        }
                    }
                }

                return instance;
            }
        }

        private ClientHelper() { }


        public void Connector(string host, int port)
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
            if (isConnected || isConnecting)
            {
                return;
            }

            isConnecting = true;
            thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        public void Stop()
        {
            if (!isConnected)
                return;

            Debug.Log("client Stop");

            session.CloseNow();
            connector.Dispose();
            thread.Abort();
            isConnected = false;
        }


        public bool IsConnected()
        {
            return isConnected;
        }

        public void Send(PacketOutStream packet)
        {
            if (!isConnected)
            {
                return;
            }
            this.session.Write(packet.getPackets()); ;
        }

        void Run()
        {
            connector = new AsyncSocketConnector();

            // Configure the service.
            connector.ConnectTimeoutInMillis = CONNECT_TIMEOUT;

            connector.FilterChain.AddLast("logger", new LoggingFilter());
            //connector.FilterChain.AddLast("codec", new ProtocolCodecFilter(new TextLineCodecFactory(Encoding.UTF8)));
            connector.FilterChain.AddLast("codec", new ProtocolCodecFilter(new ObjectSerializationCodecFactory()));
            connector.MessageReceived += MessageReceived;
            connector.MessageSent += MessageSent;
            connector.SessionIdle += SessionIdle;
            connector.SessionOpened += SessionOpened;
            connector.SessionClosed += SessionClosed;
            connector.ExceptionCaught += ExceptionCaught;

            while (true)
            {
                try
                {
                    IConnectFuture future = connector.Connect(endPoint);
                    future.Await();
                    session = future.Session;
                    isConnected = true;
                    isConnecting = false;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Thread.Sleep(3000);
                }
            }

            // wait until the summation is done
            session.CloseFuture.Await();
        }


        private void SessionOpened(object sender, IoSessionEventArgs e)
        {
            IPEndPoint remoreIP = (IPEndPoint)e.Session.RemoteEndPoint;
            Debug.Log(String.Format("connect to server {0} {1}", remoreIP.Address, remoreIP.Port));
        }

        private void SessionClosed(object sender, IoSessionEventArgs e)
        {
            IPEndPoint remoreIP = (IPEndPoint)e.Session.RemoteEndPoint;
            Debug.Log(String.Format("disconnect from server {0} {1}", remoreIP.Address, remoreIP.Port));
            connector.Dispose();
            thread.Abort();
            isConnected = false;
        }

        private void SessionIdle(object sender, IoSessionIdleEventArgs e)
        {
            e.Session.Close(true);
            Debug.Log("client SessionIdle");

        }

        private void ExceptionCaught(object sender, IoSessionExceptionEventArgs e)
        {
            Debug.Log(e.Exception);
            e.Session.Close(true);
            connector.Dispose();
            thread.Abort();
            isConnected = false;
            Debug.Log("client ExceptionCaught");
        }

        private void MessageSent(object sender, IoSessionMessageEventArgs e)
        {
            Debug.Log("client MessageSent");
        }

        private void MessageReceived(object sender, IoSessionMessageEventArgs e)
        {
            byte[] data = (byte[])e.Message;
            PacketDecoderNew decoder = new PacketDecoderNew(data);
            if (!decoder.Available())
                return;
            ServerPacketProcessor.Instance.DelegateHandler(client, decoder.GetPacket());
            decoder.Dispose();
        }
    }
}
