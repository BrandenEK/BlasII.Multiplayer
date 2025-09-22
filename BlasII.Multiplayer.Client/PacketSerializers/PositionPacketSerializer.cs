using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Serializers;
using Basalt.Framework.Networking.Streams;
using BlasII.Multiplayer.Client.Packets;

namespace BlasII.Multiplayer.Client.PacketSerializers;

internal class PositionPacketSerializer : IPacketSerializer
{
    public byte[] Serialize(BasePacket packet)
    {
        PositionPacket p = (PositionPacket)packet;

        var stream = new OutStream();
        stream.Write_string(p.Name);
        stream.Write_float(p.X);
        stream.Write_float(p.Y);

        return stream;
    }

    public BasePacket Deserialize(byte[] data)
    {
        var stream = new InStream(data);

        string name = stream.Read_string();
        float x = stream.Read_float();
        float y = stream.Read_float();

        return new PositionPacket(name, x, y);
    }
}
