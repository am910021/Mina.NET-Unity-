using Mina.Filter.Codec.Demux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YuriWorkSpace;

public class StreamCodecFactory : DemuxingProtocolCodecFactory
{
    public StreamCodecFactory(Boolean server)
    {
        AddMessageDecoder<PacketDecoder>();
        AddMessageEncoder<PacketOutStream, PacketEncoder>();
    }
}