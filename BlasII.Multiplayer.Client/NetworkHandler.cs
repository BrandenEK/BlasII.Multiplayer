using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;

namespace BlasII.Multiplayer.Client;

public class NetworkHandler
{
    private readonly NetworkClient _client;

    public NetworkHandler(NetworkClient client)
    {
        _client = client;
    }

    public void Connect(string ip, int port)
    {
        try
        {
            _client.Connect(ip, port);
            ModLog.Info($"Client connected to {ip}:{port}");
        }
        catch (System.Exception ex)
        {
            ModLog.Error($"Encountered an error when attempting to connect - {ex}");
            return;
        }
    }

    public void Send(BasePacket packet)
    {
        _client.Send(packet);
    }

    public void OnUpdate()
    {
        if (!_client.IsActive)
            return;

        try
        {
            _client.Receive();
        }
        catch (NetworkException ex)
        {
            ModLog.Error($"Encountered an error when receiving data - {ex}");
        }

        _client.Update();
    }
}
