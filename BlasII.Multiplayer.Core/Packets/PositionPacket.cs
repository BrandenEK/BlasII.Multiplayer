using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Core.Packets;

public class PositionPacket : BasePacket
{
    public string Name { get; set; }

    public float X { get; }

    public float Y { get; }

    //public static PositionPacket FromClient(float x, float y)
    //{
    //    return new PositionPacket(null, x, y);
    //}

    //public static PositionPacket FromServer(string name, float x, float y)
    //{
    //    return new PositionPacket(name, x, y);
    //}

    public PositionPacket(string name, float x, float y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}
