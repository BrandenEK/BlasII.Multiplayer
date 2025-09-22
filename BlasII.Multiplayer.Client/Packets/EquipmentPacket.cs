using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Client.Packets;

public class EquipmentPacket : BasePacket
{
    public string Name { get; }

    public byte Type { get; }
    public string Equipment { get; }

    public EquipmentPacket(string name, byte type, string equipment)
    {
        Name = name;
        Type = type;
        Equipment = equipment;
    }
}
