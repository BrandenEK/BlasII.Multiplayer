using BlasII.ModdingAPI;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class Companion
{
    private GameObject _obj;

    public CompanionTransform Transform { get; private set; }
    public CompanionRenderer Renderer { get; private set; }

    public Companion(string name)
    {
        ModLog.Info($"Creating new companion: {name}");

        _obj = new GameObject(name);

        Transform = new CompanionTransform(_obj.transform);
        Renderer = new CompanionRenderer(_obj.transform);
    }
}
