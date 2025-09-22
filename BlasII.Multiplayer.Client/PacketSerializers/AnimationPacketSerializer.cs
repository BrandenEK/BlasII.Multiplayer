using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Serializers;
using Basalt.Framework.Networking.Streams;
using BlasII.Multiplayer.Client.Packets;

namespace BlasII.Multiplayer.Client.PacketSerializers;

public class AnimationPacketSerializer : IPacketSerializer
{
    public byte[] Serialize(BasePacket packet)
    {
        AnimationPacket p = (AnimationPacket)packet;

        var stream = new OutStream();
        stream.Write_string(p.Name);
        stream.Write_int(p.State);
        stream.Write_float(p.Time);
        stream.Write_float(p.Length);

        return stream;
    }

    public BasePacket Deserialize(byte[] data)
    {
        var stream = new InStream(data);

        string name = stream.Read_string();
        int state = stream.Read_int();
        float time = stream.Read_float();
        float length = stream.Read_float();

        return new AnimationPacket(name, state, time, length);
    }
}
