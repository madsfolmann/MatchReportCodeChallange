namespace Domain.Round;

public class KillEvent(string killerSteamId, string victimSteamId, Weapon weapon, bool isHeadshot, DateTime timestamp)
{
    public string KillerSteamId { get; private set; } = killerSteamId;
    public string VictimSteamId { get; private set; } = victimSteamId;
    public Weapon Weapon { get; private set; } = weapon;
    public bool IsHeadshot { get; private set; } = isHeadshot;
    public DateTime Timestamp { get; private set; } = timestamp;
}