using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Storages;

public class IconStorage
{
    private readonly Sprite[] _statusSprites;

    /// <summary>
    /// Loads all required icons
    /// </summary>
    public IconStorage(FileHandler file)
    {
        file.LoadDataAsFixedSpritesheet("mpstatus.png", new Vector2(22, 22), out _statusSprites);
    }

    /// <summary>
    /// Gets the connected or disconnected icon
    /// </summary>
    public Sprite GetStatusIcon(bool status)
    {
        return _statusSprites[status ? 0 : 1];
    }
}
