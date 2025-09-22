using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Client.Packets;

public class DirectionPacket : BasePacket
{
    public bool FacingDirection { get; set; }
}
