using Basalt.Framework.Networking.Client;
using BlasII.Framework.UI;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Multiplayer.Client.Displays;

public class StatusDisplay
{
    private readonly NetworkClient _client;

    private Image _image;

    /// <summary>
    /// Initializes the status connection
    /// </summary>
    public StatusDisplay(NetworkClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Updates the connection icon
    /// </summary>
    public void OnUpdate()
    {
        if (_image == null)
            _image = CreateImage();

        if (_image == null)
            return;

        _image.sprite = Main.Multiplayer.IconStorage.GetStatusIcon(_client.IsActive);
    }

    private Image CreateImage()
    {
        Transform parent = Object.FindObjectOfType<UITearsControl>()?.transform;

        if (parent == null)
            return null;

        RectTransform connect = UIModder.Create(new RectCreationOptions()
        {
            Name = "mpstatus",
            Parent = parent,
            Pivot = Vector2.one,
            XRange = Vector2.one,
            YRange = Vector2.zero,
            Position = new Vector2(0, 0),
            Size = new Vector2(66, 66),
        });

        return connect.AddImage();
    }
}
