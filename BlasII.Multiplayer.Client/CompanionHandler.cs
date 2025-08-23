using BlasII.ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class CompanionHandler
{
    private readonly Dictionary<string, Companion> _companions = [];

    public void OnEnterScene()
    {
        _companions.Clear();

        AddCompanion("Test");
    }

    public void OnLeaveScene()
    {
        _companions.Clear();
    }

    public void TempGetPosition(Vector2 position)
    {
        Companion c = GetCompanionByName("Test");
        c.Transform.UpdatePosition(position + Vector2.right * 5);
    }

    public void TempGetAnimation(int state, float time)
    {
        Companion c = GetCompanionByName("Test");
        c.Renderer.UpdateAnim(state, time);
    }

    public void TempGetDirection(bool direction)
    {
        Companion c = GetCompanionByName("Test");
        c.Transform.UpdateDirection(direction);
    }

    private void AddCompanion(string name)
    {
        // TODO: check for name collision

        Companion companion = new Companion(name);
        _companions.Add(name, companion);

        ModLog.Info($"Adding companion {name}");
    }

    private void RemoveCompanion(string name)
    {
        // TODO: actually get the object and destroy it, or log warning

        if (_companions.ContainsKey(name))
            _companions.Remove(name);

        ModLog.Info($"Removing companion {name}");
    }

    private Companion GetCompanionByName(string name)
    {
        // TODO: check if they exist or not

        return _companions[name];
    }
}
