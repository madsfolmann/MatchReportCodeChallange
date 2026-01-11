using API.Models.Enums;

namespace API.Models;

public class Round
{
    public int Number { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public TimeSpan Duration => EndedAt - StartedAt;
    public RoundWin Winner { get; set; }
    public Dictionary<string, Side> PlayerSideBySteamId { get; set; }
    public List<KillEvent> Kills { get; set; } = [];
    public List<BombEvent> BombEvents { get; set; } = [];
}