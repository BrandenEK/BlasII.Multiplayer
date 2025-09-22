
namespace BlasII.Multiplayer.Client.Models;

public class RoomInfo(string room, string player, int team)
{
    public string RoomName { get; } = room;

    public string PlayerName { get; } = player;

    public int Team { get; } = team;
}
