using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace YuriWorkSpace
{
    public class PacketInStream
    {
        private MemoryStream stream;
        private BinaryReader packet;

        public PacketInStream(byte[] inPacket)
        {
            stream = new MemoryStream(inPacket);
            packet = new BinaryReader(stream);
        }


        public void printText(string side = "Client")
        {
            Debug.Log(side + "收到: " + Encoding.UTF8.GetString(stream.ToArray()));
        }

        public void printHex(string side = "Client")
        {
            Debug.Log(String.Format("{0} 收到: {1}", side, BitConverter.ToString(stream.ToArray())));
        }


        public byte readByte()
        {
            return packet.ReadByte();
        }

        public short readShort()
        {
            return packet.ReadInt16();
        }

        public int readInt()
        {
            return packet.ReadInt32();
        }

        public long readLong()
        {
            return packet.ReadInt64();
        }

        public float readFloat()
        {
            return packet.ReadSingle();
        }

        public string readString()
        {
            short len = this.readShort();
            byte[] code = packet.ReadBytes(len);
            return Encoding.UTF8.GetString(code);
        }

        public byte[] Decompress(byte[] input)
        {
            byte[] decompressedData;

            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(input))
                {
                    using (var zip = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        this.CopyTo(zip, outputStream);
                    }
                }

                decompressedData = outputStream.ToArray();
            }

            return decompressedData;
        }

        public long CopyTo(Stream source, Stream destination)
        {
            byte[] buffer = new byte[2048];
            int bytesRead;
            long totalBytes = 0;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                destination.Write(buffer, 0, bytesRead);
                totalBytes += bytesRead;
            }
            return totalBytes;
        }


    }
}
