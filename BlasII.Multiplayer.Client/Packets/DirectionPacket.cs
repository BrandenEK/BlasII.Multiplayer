using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Client.Packets;

public class DirectionPacket : BasePacket
{
    public string Name { get; }

    public bool FacingDirection { get; }

    public DirectionPacket(string name, bool facingDirection)
    {
        Name = name;
        FacingDirection = facingDirection;
    }
}
