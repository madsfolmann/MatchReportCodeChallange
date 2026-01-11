using Domain.Round;

namespace Domain.Player;

public class PlayerEntity(string name, string steamId)
{
    public string Name { get; private set; } = name;
    public string SteamId { get; private set; } = steamId;
    public Side Side { get; private set; } = Side.Unassigned;

    public void SwitchSide(Side side)
    {
        Side = side;
    }
}