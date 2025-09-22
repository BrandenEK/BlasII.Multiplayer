using Basalt.Framework.Logging;
using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Server;
using BlasII.Multiplayer.Core.Packets;
using BlasII.Multiplayer.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Multiplayer.Server;

public class RoomManager
{
    private readonly NetworkServer _server;
    private readonly Dictionary<string, PlayerInfo> _players;

    public RoomManager(NetworkServer server)
    {
        _server = server;
        _players = [];

        server.OnClientConnected += OnClientConnected;
        server.OnClientDisconnected += OnClientDisconnected;
        server.OnPacketReceived += OnPacketReceived;
    }

    private void OnClientConnected(string ip)
    {
        if (_players.ContainsKey(ip))
        {
            Logger.Warn($"Failed to add client {ip} because they are already connected");
            return;
        }

        Logger.Info($"Client connected at {ip}");
        _players.Add(ip, new PlayerInfo($"Player {_players.Count + 1}"));
    }

    private void OnClientDisconnected(string ip)
    {
        if (!_players.ContainsKey(ip))
        {
            Logger.Warn($"Failed to remove client {ip} because they are already disconnected");
            return;
        }

        Logger.Info($"Client disconnected at {ip}");
        _players.Remove(ip);
    }

    private void OnPacketReceived(string ip, BasePacket packet)
    {
        Logger.Debug($"Received packet of type {packet.GetType().Name} from {ip}");

        if (!_players.TryGetValue(ip, out PlayerInfo player))
        {
            Logger.Error($"Received {packet.GetType().Name} from non-registered player {ip}");
            return;
        }

        // TODO: when entering a new scene, also send all status packets

        if (packet is PositionPacket position)
        {
            player.PositionX = position.X;
            player.PositionY = position.Y;

            _server.Send(_players.Keys.Where(x => x != ip), position);
        }

        if (packet is AnimationPacket animation)
        {
            player.AnimationState = animation.State;
            player.AnimationLength = animation.Length;

            _server.Send(_players.Keys.Where(x => x != ip), animation);
        }

        if (packet is DirectionPacket direction)
        {
            player.FacingDirection = direction.FacingDirection;

            _server.Send(_players.Keys.Where(x => x != ip), direction);
        }

        if (packet is EquipmentPacket equipment)
        {
            if (equipment.Type == 0)
                player.ArmorName = equipment.Equipment;
            else if (equipment.Type == 1)
                player.WeaponName = equipment.Equipment;
            else if (equipment.Type == 2)
                player.WeaponfxName = equipment.Equipment;

            _server.Send(_players.Keys.Where(x => x != ip), equipment);
        }
    }
}
