using API.Models.Enums;

namespace API.Models;

public class Player
{
    public string SteamId { get; set; }
    public string Name { get; set; }
    public string TeamName { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public Weapon? WeaponWithMostKills { get; set; }
    public double Kd => Deaths == 0 ? Kills : (double)Kills / Deaths;
}