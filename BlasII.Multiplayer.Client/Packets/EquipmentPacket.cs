using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Client.Packets;

public class EquipmentPacket : BasePacket
{
    public byte Type { get; set; }
    public string Name { get; set; }
}
