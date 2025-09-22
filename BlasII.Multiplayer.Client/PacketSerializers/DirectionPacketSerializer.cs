using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Serializers;
using Basalt.Framework.Networking.Streams;
using BlasII.Multiplayer.Client.Packets;

namespace BlasII.Multiplayer.Client.PacketSerializers;

public class DirectionPacketSerializer : IPacketSerializer
{
    public byte[] Serialize(BasePacket packet)
    {
        DirectionPacket p = (DirectionPacket)packet;

        var stream = new OutStream();
        stream.Write_string(p.Name);
        stream.Write_bool(p.FacingDirection);

        return stream;
    }

    public BasePacket Deserialize(byte[] data)
    {
        var stream = new InStream(data);

        string name = stream.Read_string();
        bool direction = stream.Read_bool();

        return new DirectionPacket(name, direction);
    }
}
