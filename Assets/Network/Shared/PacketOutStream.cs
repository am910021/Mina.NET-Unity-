using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace YuriWorkSpace 
{
    public class PacketOutStream : IDisposable
    {
        private bool _disposed = false;
        ~PacketOutStream() => Dispose(false);

        private MemoryStream stream;
        private BinaryWriter outPacket;
        byte[] outData = null;

        public PacketOutStream()
        {
            stream = new MemoryStream();
            outPacket = new BinaryWriter(stream);
        }

        public void writeByte(byte b)
        {
            outPacket.Write(b);
        }

        public void writeShort(short s)
        {
            outPacket.Write(s);
        }

        public void writeInt(int i)
        {
            outPacket.Write(i);
        }

        public void writeLong(long l)
        {
            outPacket.Write(l);
        }

        public void writeFloat(float f)
        {
            outPacket.Write(f);
        }

        public void writeString(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            this.writeShort((short)bytes.Length);
            outPacket.Write(bytes);
        }

        public void writeSerializable(ISerializable s, string className)
        {
            this.writeString(className);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, s);
                byte[] bytes = memoryStream.ToArray();
                this.writeShort((short)bytes.Length);
                outPacket.Write(bytes);
            }
        }

        public byte[] getPackets2()
        {
            return stream.ToArray();
        }

        public int GetSize()
        {
           return (int)stream.Length;
        }


        public byte[] getPackets()
        {
            if (outData == null)
            {
                int size = (int)stream.Length;
                byte[] header = BitConverter.GetBytes(size);
                outData = header.Concat(stream.ToArray()).ToArray();
            }
            outPacket.Close();
            stream.Close();
            return outData;
        }


        private static byte[] Compress(byte[] input)
        {
            byte[] compressesData;

            using (var outputStream = new MemoryStream())
            {
                using (var zip = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zip.Write(input, 0, input.Length);
                }

                compressesData = outputStream.ToArray();
            }

            return compressesData;
        }

        public void printHex(string side = "Server")
        {
            Debug.Log(side + "發送: " + BitConverter.ToString(stream.ToArray()));
        }

        public void printText(string side = "Server")
        {
            Debug.Log(side + "發送: " + Encoding.UTF8.GetString(getPackets()));
        }

        public static string toHex(byte[] data)
        {
            string str = "";
            foreach (byte b in data)
            {
                str += String.Format("{0:x} ", b);
            }
            return str;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }


    }
}
