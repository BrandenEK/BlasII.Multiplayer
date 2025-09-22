using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.Multiplayer.Client.Models;
using BlasII.Multiplayer.Core.Extensions;
using BlasII.Multiplayer.Core.Packets;

namespace BlasII.Multiplayer.Client;

public class NetworkHandler
{
    private readonly NetworkClient _client;

    private RoomInfo _currentRoom;

    public NetworkHandler(NetworkClient client)
    {
        _client = client;
        _client.OnPacketReceived += OnPacketReceived;
    }

    public void Connect(string ip, int port, RoomInfo room)
    {
        _currentRoom = room;

        try
        {
            _client.Connect(ip, port);
        }
        catch (System.Exception ex)
        {
            ModLog.Error($"Encountered an error when attempting to connect - {ex}");
            return;
        }
    }

    public void Send(BasePacket packet)
    {
        if (!_client.IsActive)
            return;

        if (packet is INamedPacket p)
            p.Name = _currentRoom.PlayerName;

        if (LOG_TRAFFIC)
            ModLog.Debug($"SENDING {packet.Stringify()}");

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

    private void OnPacketReceived(BasePacket packet)
    {
        if (LOG_TRAFFIC)
            ModLog.Debug($"RECEIVING {packet.Stringify()}");
    }

    private const bool LOG_TRAFFIC = true;
}
