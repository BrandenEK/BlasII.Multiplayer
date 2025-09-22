using Basalt.Framework.Networking.Serializers;
using BlasII.Multiplayer.Core.Packets;
using BlasII.Multiplayer.Core.PacketSerializers;

namespace BlasII.Multiplayer.Core;

public class CoreSerializer : ClassicSerializer
{
    public CoreSerializer()
    {
        byte id = 0;
        AddPacketSerializer<PositionPacket>(id++, new PositionPacketSerializer());
        AddPacketSerializer<AnimationPacket>(id++, new AnimationPacketSerializer());
        AddPacketSerializer<DirectionPacket>(id++, new DirectionPacketSerializer());
        AddPacketSerializer<EquipmentPacket>(id++, new EquipmentPacketSerializer());
    }
}
