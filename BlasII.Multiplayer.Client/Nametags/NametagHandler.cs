using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.Multiplayer.Client.Components;
using Il2CppTGK.Game.Components.UI;
using System.Collections.Generic;

namespace BlasII.Multiplayer.Client.Nametags;

public class NametagHandler
{
    private readonly Dictionary<string, UIPixelTextWithShadow> _nametags = [];

    public NametagHandler(NetworkClient client)
    {
        client.OnPacketReceived += OnPacketReceived;
    }

    public void OnEnterScene()
    {
        RemoveAllCompanions();
    }

    public void OnLeaveScene()
    {
        RemoveAllCompanions();
    }

    public void OnUpdate()
    {
        foreach (var companion in _companions.Values)
            companion.Renderer.OnUpdate();
    }

    private void OnPacketReceived(BasePacket packet)
    {
        ModLog.Info($"Received packet of type {packet.GetType().Name}");

        if (packet is PositionPacket position)
            OnReceivePosition(position.Name, new Vector2(position.X, position.Y));

        if (packet is AnimationPacket animation)
            OnReceiveAnimation(animation.Name, animation.State, animation.Time, animation.Length);

        if (packet is DirectionPacket direction)
            OnReceiveDirection(direction.Name, direction.FacingDirection);

        if (packet is EquipmentPacket equipment)
            OnReceiveEquipment(equipment.Name, equipment.Type, equipment.Equipment);

        // Should I ensure the companion already exists from the scenepacket or just create them if they dont exist yet ??
    }

    private void OnReceivePosition(string name, Vector2 position)
    {
        if (!_companions.TryGetValue(name, out Companion companion))
        {
            companion = AddCompanion(name);
            //ModLog.Error($"Received position from {name} who does not exist");
            //return;
        }

        companion.Transform.UpdatePosition(position);
    }

    private void OnReceiveAnimation(string name, int state, float time, float length)
    {
        if (!_companions.TryGetValue(name, out Companion companion))
        {
            companion = AddCompanion(name);
            //ModLog.Error($"Received position from {name} who does not exist");
            //return;
        }

        companion.Renderer.UpdateAnim(state, time, length);
    }

    private void OnReceiveDirection(string name, bool direction)
    {
        if (!_companions.TryGetValue(name, out Companion companion))
        {
            companion = AddCompanion(name);
            //ModLog.Error($"Received position from {name} who does not exist");
            //return;
        }

        companion.Transform.UpdateDirection(direction);
    }

    private void OnReceiveEquipment(string name, byte type, string equipment)
    {
        if (!_companions.TryGetValue(name, out Companion companion))
        {
            companion = AddCompanion(name);
            //ModLog.Error($"Received position from {name} who does not exist");
            //return;
        }

        companion.Renderer.UpdateEquipment(type, equipment);
    }

    private Companion AddCompanion(string name)
    {
        if (_companions.TryGetValue(name, out Companion companion))
        {
            ModLog.Warn($"Failed to add companion {name} because they already exist");
            return companion;
        }

        ModLog.Info($"Adding companion {name}");
        companion = new Companion(name);

        _companions.Add(name, companion);
        return companion;
    }

    private void RemoveCompanion(string name)
    {
        if (!_companions.TryGetValue(name, out Companion companion))
        {
            ModLog.Warn($"Failed to remove companion {name} because they don't exist");
            return;
        }

        ModLog.Info($"Removing companion {name}");
        companion.Destroy();
        _companions.Remove(name);
    }

    private void RemoveAllCompanions()
    {
        foreach (var companion in _companions.Values)
            companion.Destroy();
        _companions.Clear();
    }
}
