using BlasII.ModdingAPI;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class Companion
{
    public GameObject GameObject { get; private set; }
    public CompanionTransform Transform { get; private set; }
    public CompanionRenderer Renderer { get; private set; }

    public Companion(string name)
    {
        ModLog.Info($"Creating new companion: {name}");

        GameObject = new GameObject(name);

        Transform = new CompanionTransform(GameObject.transform);
        Renderer = new CompanionRenderer(GameObject.transform);
    }

    public void Destroy()
    {
        if (GameObject == null)
            return;

        Object.Destroy(GameObject);
    }
}
