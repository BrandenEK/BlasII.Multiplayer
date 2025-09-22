using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.Multiplayer.Client.Models;

namespace BlasII.Multiplayer.Client;

public class NetworkHandler
{
    private readonly NetworkClient _client;

    private RoomInfo _currentRoom;

    public NetworkHandler(NetworkClient client)
    {
        _client = client;
    }

    public void Connect(string ip, int port, RoomInfo room)
    {
        try
        {
            _client.Connect(ip, port);
        }
        catch (System.Exception ex)
        {
            ModLog.Error($"Encountered an error when attempting to connect - {ex}");
            return;
        }

        _currentRoom = room;
    }

    public void Send(BasePacket packet)
    {
        // TODO: add name to packet

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
