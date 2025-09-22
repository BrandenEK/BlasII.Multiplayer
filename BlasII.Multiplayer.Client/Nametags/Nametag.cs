using BlasII.Framework.UI;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Nametags;

public class Nametag
{
    private readonly UIPixelTextWithShadow _text;

    public Vector3 Position { get; set; }

    public Nametag(string name)
    {
        _text = UIModder.Create(new RectCreationOptions()
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
    }

    public void UpdateProperties(Camera camera)
    {
        if (_text == null)
            return;

        Vector3 viewPos = camera.WorldToViewportPoint(Position + Vector3.up * NAMETAG_OFFSET);
        _text.shadowText.rectTransform.anchorMin = viewPos;
        _text.shadowText.rectTransform.anchorMax = viewPos;
        _text.shadowText.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void Destroy()
    {
        if (_text == null || _text.gameObject == null)
            return;

        Object.Destroy(_text.gameObject);
    }

    private const float NAMETAG_OFFSET = 2.9f;
}
