namespace Domain.Round;

public class BombEvent(string playerSteamId, BombAction action, BombSite? bombsite, DateTime timestamp)
{
    public string PlayerSteamId { get; private set; } = playerSteamId;
    public BombAction Action { get; private set; } = action;
    public BombSite? Bombsite { get; private set; } = bombsite;
    public DateTime Timestamp { get; private set; } = timestamp;
}