using Basalt.Framework.Logging;
using Basalt.Framework.Networking;
using BlasII.Multiplayer.Server.Models;
using System.Collections.Generic;

namespace BlasII.Multiplayer.Server;

public class RoomManager
{
    private readonly Dictionary<string, PlayerInfo> _players = [];

    public void OnClientConnected(string ip)
    {
        if (_players.ContainsKey(ip))
        {
            Logger.Warn($"Failed to add client {ip} because they are already connected");
            return;
        }

        Logger.Info($"Client connected at {ip}");
        _players.Add(ip, new PlayerInfo($"Player {_players.Count + 1}"));
    }

    public void OnClientDisconnected(string ip)
    {
        if (!_players.ContainsKey(ip))
        {
            Logger.Warn($"Failed to remove client {ip} because they are already disconnected");
            return;
        }

        Logger.Info($"Client disconnected at {ip}");
        _players.Remove(ip);
    }

    public void OnPacketReceived(string ip, BasePacket packet)
    {
        Logger.Debug($"Received packet of type {packet.GetType().Name}");
    }
}
