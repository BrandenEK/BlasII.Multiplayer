using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Core.Packets;

public class DirectionPacket : BasePacket, INamedPacket
{
    public string Name { get; set; }

    public bool FacingDirection { get; }

    public DirectionPacket(string name, bool facingDirection)
    {
        Name = name;
        FacingDirection = facingDirection;
    }
}
