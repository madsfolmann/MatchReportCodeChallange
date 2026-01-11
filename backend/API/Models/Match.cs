using API.Models.Enums;

namespace API.Models;

public class Match
{
    public Map Map { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public List<Round> Rounds { get; set; } = [];
    public List<Team> Teams { get; set; } = [];
    public List<Player> Players { get; set; } = [];
    public string? WinnerTeamName { get; set; }
    public int TotalRounds => Rounds.Count;
    public TimeSpan Duration => EndedAt - StartedAt;
    public TimeSpan AverageRoundDuration =>
        Rounds.Count == 0 ? TimeSpan.Zero : TimeSpan.FromTicks(Rounds.Sum(r => r.Duration.Ticks) / Rounds.Count);
    public List<WeaponStats> WeaponStats => Rounds
        .SelectMany(r => r.Kills)
        .GroupBy(k => k.Weapon)
        .Select(g => new WeaponStats
        {
            WeaponName = g.Key,
            Kills = g.Count(),
            Headshots = g.Sum(k => k.IsHeadshot ? 1 : 0)
        })
        .ToList();
}