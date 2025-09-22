using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Client.Packets;

public class AnimationPacket : BasePacket
{
    public int State { get; set; }
    public float Time { get; set; }
    public float Length { get; set; }
}
