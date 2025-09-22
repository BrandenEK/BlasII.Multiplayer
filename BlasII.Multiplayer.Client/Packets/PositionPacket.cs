using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Client.Packets;

public class PositionPacket : BasePacket
{
    public float X { get; set; }
    public float Y { get; set; }
}
