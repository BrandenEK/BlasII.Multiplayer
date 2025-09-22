using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Utils;
using BlasII.Multiplayer.Core.Packets;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Nametags;

public class NametagHandler
{
    private readonly Dictionary<string, UIPixelTextWithShadow> _nametags;
    private readonly ObjectCache<Camera> _camCache;

    public NametagHandler(NetworkClient client)
    {
        _nametags = [];
        _camCache = new ObjectCache<Camera>(() => Object.FindObjectsOfType<Camera>().First(x => x.name == "Main Camera"));

        client.OnPacketReceived += OnPacketReceived;
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
        // TODO: only if connected
        // On disconnect, destroy all

        OnReceivePosition(Multiplayer.PlayerName, CoreCache.PlayerSpawn.PlayerInstance.transform.position);
    }

    private void OnPacketReceived(BasePacket packet)
    {
        if (packet is PositionPacket position)
            OnReceivePosition(position.Name, new Vector2(position.X, position.Y));

        // Should I ensure the nametag already exists from the scenepacket or just create them if they dont exist yet ??
    }

    private void OnReceivePosition(string name, Vector2 position)
    {
        if (!_nametags.TryGetValue(name, out UIPixelTextWithShadow nametag))
        {
            nametag = AddNametag(name);
            //ModLog.Error($"Received position from {name} who does not exist");
            //return;
        }

        Vector3 viewPos = _camCache.Value.WorldToViewportPoint(position + Vector2.up * NAMETAG_OFFSET);
        nametag.shadowText.rectTransform.anchorMin = viewPos;
        nametag.shadowText.rectTransform.anchorMax = viewPos;
        nametag.shadowText.rectTransform.anchoredPosition = Vector2.zero;

        ModLog.Warn("Updating positon to " + viewPos);
    }

    private UIPixelTextWithShadow AddNametag(string name)
    {
        if (_nametags.TryGetValue(name, out UIPixelTextWithShadow nametag))
        {
            ModLog.Warn($"Failed to add nametag {name} because they already exist");
            return nametag;
        }

        ModLog.Info($"Adding nametag {name}");

        nametag = UIModder.Create(new RectCreationOptions()
        {
            Name = name + "_tag",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(100, 50),
        }).AddText(new TextCreationOptions()
        {
            Alignment = Il2CppTMPro.TextAlignmentOptions.Bottom,
            Color = new Color32(0xAB, 0x9A, 0x3F, 0xFF),
            Contents = name,
            FontSize = 48,
        }).AddShadow();

        _nametags.Add(name, nametag);
        return nametag;
    }

    private void RemoveNametag(string name)
    {
        if (!_nametags.TryGetValue(name, out UIPixelTextWithShadow nametag))
        {
            ModLog.Warn($"Failed to remove nametag {name} because they don't exist");
            return;
        }

        ModLog.Info($"Removing nametag {name}");
        Object.Destroy(nametag.gameObject);
        _nametags.Remove(name);
    }

    private void RemoveAllNametags()
    {
        foreach (var nametag in _nametags.Values)
            Object.Destroy(nametag.gameObject);
        _nametags.Clear();
    }

    private const float NAMETAG_OFFSET = 2.9f;
}
