using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Serializers;
using Basalt.Framework.Networking.Streams;
using BlasII.Multiplayer.Core.Packets;

namespace BlasII.Multiplayer.Core.PacketSerializers;

public class EquipmentPacketSerializer : IPacketSerializer
{
    public byte[] Serialize(BasePacket packet)
    {
        EquipmentPacket p = (EquipmentPacket)packet;

        var stream = new OutStream();
        stream.Write_string(p.Name);
        stream.Write_byte(p.Type);
        stream.Write_string(p.Equipment);

        return stream;
    }

    public BasePacket Deserialize(byte[] data)
    {
        var stream = new InStream(data);

        string name = stream.Read_string();
        byte type = stream.Read_byte();
        string equipment = stream.Read_string();

        return new EquipmentPacket(name, type, equipment);
    }
}
