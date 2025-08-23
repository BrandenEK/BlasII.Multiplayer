using BlasII.ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class CompanionHandler
{
    private readonly Dictionary<string, Companion> _companions = [];

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

    private void AddCompanion(string name)
    {
        if (_companions.ContainsKey(name))
        {
            ModLog.Warn($"Failed to add companion {name} because they already exist");
            return;
        }

        ModLog.Info($"Adding companion {name}");
        _companions.Add(name, new Companion(name));
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

    private Companion GetCompanionByName(string name) // Remove this and just check for companion with TryGetValue
    {
        // TODO: check if they exist or not

        if (!_companions.ContainsKey(name))
            AddCompanion(name);

        return _companions[name];
    }


    public void TempGetPosition(Vector2 position)
    {
        Companion c = GetCompanionByName("Test");
        c.Transform.UpdatePosition(position + Vector2.right * 2);
    }

    public void TempGetAnimation(int state, float time, float length)
    {
        Companion c = GetCompanionByName("Test");
        c.Renderer.UpdateAnim(state, time, length);
    }

    public void TempGetDirection(bool direction)
    {
        Companion c = GetCompanionByName("Test");
        c.Transform.UpdateDirection(direction);
    }

    public void TempGetEquipment(int type, string name)
    {
        Companion c = GetCompanionByName("Test");
        c.Renderer.UpdateEquipment(type, name);
    }
}
