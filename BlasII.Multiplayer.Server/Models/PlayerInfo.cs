
namespace BlasII.Multiplayer.Server.Models;

public class PlayerInfo(string name)
{
    public string Name { get; } = name;

    public float PositionX { get; set; } = 0;
    public float PositionY { get; set; } = 0;

    public int AnimationState { get; set; } = 0;
    public float AnimationLength { get; set; } = 0;

    public bool FacingDirection { get; set; } = true;

    public string ArmorName { get; set; } = string.Empty;
    public string WeaponName { get; set; } = string.Empty;
    public string WeaponfxName { get; set; } = string.Empty;
}
