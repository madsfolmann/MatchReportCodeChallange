using Domain.Match;
using Domain.Round;

namespace Application.Mapping;

public static class CustomMapper
{
    public static Side GameLogSideToDomainSide(string side)
    {
        return side switch
        {
            "CT" => Side.CounterTerrorist,
            "TERRORIST" => Side.Terrorist,
            "Spectator" => Side.Spectator,
            "Unassigned" => Side.Unassigned,
        };
    }

    public static Weapon GameLogWeaponToDomainWeapon(string weapon)
    {
        var pascal = string.Concat(weapon.Split('_').Select(s => char.ToUpperInvariant(s[0]) + s[1..]));
        return Enum.Parse<Weapon>(pascal, ignoreCase: true);
    }

    public static Map GameLogMapnameToDomainMapname(string map)
    {
        return map switch
        {
            "de_nuke" => Map.Nuke,
        };
    }

    public static BombAction GameLogBombActionToDomainBombAction(string action)
    {
        return action switch
        {
            "Bomb_Begin_Plant" => BombAction.BeginPlant,
            "Planted_The_Bomb" => BombAction.Planted,
            "Begin_Bomb_Defuse_With_Kit" => BombAction.BeginDefuseWithKit,
            "Defused_The_Bomb" => BombAction.Defused,
            "Got_The_Bomb" => BombAction.Recieved,
            "Dropped_The_Bomb" => BombAction.Dropped
        };
    }

    public static BombSite GameLogBombSiteToDomainBombSite(string site)
    {
        return site switch
        {
            "A" => BombSite.A,
            "B" => BombSite.B,
        };
    }
}