using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Core.Packets;

public class AnimationPacket : BasePacket
{
    public string Name { get; }

    public int State { get; }

    public float Time { get; }

    public float Length { get; }

    public AnimationPacket(string name, int state, float time, float length)
    {
        Name = name;
        State = state;
        Time = time;
        Length = length;
    }
}
