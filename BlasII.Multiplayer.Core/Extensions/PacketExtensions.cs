using Basalt.Framework.Networking;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlasII.Multiplayer.Core.Extensions;

public static class PacketExtensions
{
    public static string Stringify(this BasePacket packet)
    {
        if (packet == null)
            return string.Empty;

        var sb = new StringBuilder($"{packet.GetType().Name}: ");

        var properties = packet.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(x => $" {x.Name}={x.GetValue(packet)}");

        sb.Append(string.Join(',', properties));
        return sb.ToString();
    }
}
