using API.Models.Enums;

namespace API.Models;

public class KillEvent
{
    public string KillerSteamId { get; set; }
    public string VictimSteamId { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsHeadshot { get; set; }
    public Weapon Weapon { get; set; }
}