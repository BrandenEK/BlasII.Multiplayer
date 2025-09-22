using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Utils;
using BlasII.Multiplayer.Core.Packets;
using Il2CppTGK.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Nametags;

public class NametagHandler
{
    private readonly NetworkClient _client;
    private readonly Dictionary<string, Nametag> _nametags;
    private readonly ObjectCache<Camera> _camCache;

    public NametagHandler(NetworkClient client)
    {
        _client = client;
        _nametags = [];
        _camCache = new ObjectCache<Camera>(() => Object.FindObjectsOfType<Camera>().First(x => x.name == "Main Camera"));

        client.OnPacketReceived += OnPacketReceived;
        client.OnClientDisconnected += OnClientDisconnected;
    }

    public void OnEnterScene()
    {
        RemoveAllNametags();
    }

    public void OnLeaveScene()
    {
        RemoveAllNametags();
    }

    public void OnUpdate()
    {
        // TODO: remove nametag when someone leaves room

        if (!_client.IsActive)
            return;

        OnReceivePosition(Multiplayer.PlayerName, CoreCache.PlayerSpawn.PlayerInstance.transform.position);

        foreach (var nametag in _nametags.Values)
        {
            nametag.UpdateProperties(_camCache.Value);
        }
    }

    private void OnClientDisconnected(string ip)
    {
        RemoveAllNametags();
    }

    private void OnPacketReceived(BasePacket packet)
    {
        if (packet is PositionPacket position)
            OnReceivePosition(position.Name, new Vector2(position.X, position.Y));

        // Should I ensure the nametag already exists from the scenepacket or just create them if they dont exist yet ??
    }

    private void OnReceivePosition(string name, Vector2 position)
    {
        if (!_nametags.TryGetValue(name, out Nametag nametag))
        {
            nametag = AddNametag(name);
            //ModLog.Error($"Received position from {name} who does not exist");
            //return;
        }

        nametag.Position = position;
    }

    private Nametag AddNametag(string name)
    {
        if (_nametags.TryGetValue(name, out Nametag nametag))
        {
            ModLog.Warn($"Failed to add nametag {name} because they already exist");
            return nametag;
        }

        ModLog.Info($"Adding nametag {name}");

        nametag = new Nametag(name);
        _nametags.Add(name, nametag);
        return nametag;
    }

    private void RemoveNametag(string name)
    {
        if (!_nametags.TryGetValue(name, out Nametag nametag))
        {
            ModLog.Warn($"Failed to remove nametag {name} because they don't exist");
            return;
        }

        ModLog.Info($"Removing nametag {name}");
        nametag.Destroy();
        _nametags.Remove(name);
    }

    private void RemoveAllNametags()
    {
        foreach (var nametag in _nametags.Values)
            nametag.Destroy();
        _nametags.Clear();
    }
}
