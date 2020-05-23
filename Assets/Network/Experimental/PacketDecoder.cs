using Mina.Core.Buffer;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.Demux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketDecoder : IMessageDecoder
{
	public MessageDecoderResult Decodable(IoSession session, IoBuffer input)
	{
		int size = input.GetInt16();
		int cap = input.Capacity;
		Debug.Log(string.Format("{0} {1}", size, cap));
		return MessageDecoderResult.OK;
	}

	public MessageDecoderResult Decode(IoSession session, IoBuffer input, IProtocolDecoderOutput output)
	{



		output.Write(input);
		return MessageDecoderResult.OK;
	}

	public void FinishDecode(IoSession session, IProtocolDecoderOutput output)
	{
		throw new System.NotImplementedException();
	}

}
