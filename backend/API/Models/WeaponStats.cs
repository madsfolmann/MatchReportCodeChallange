using API.Models.Enums;

namespace API.Models;

public class WeaponStats
{
    public Weapon WeaponName { get; set; }
    public int Kills { get; set; }
    public int Headshots { get; set; }
}