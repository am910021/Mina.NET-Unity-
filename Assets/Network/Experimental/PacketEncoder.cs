using Mina.Core.Buffer;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.Demux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YuriWorkSpace;

public class PacketEncoder : IMessageEncoder
{
	public void Encode(IoSession session, PacketOutStream message, IProtocolEncoderOutput output)
	{
		int size = message.GetSize();
		IoBuffer buf = IoBuffer.Allocate(size + 4);
		buf.PutInt32(size);
		buf.Put(message.getPackets2());
		buf.Flip();
		output.Write(buf);
	}

	public void Encode(IoSession session, object message, IProtocolEncoderOutput output)
	{
		Encode(session, (PacketOutStream)message, output);
	}
}
