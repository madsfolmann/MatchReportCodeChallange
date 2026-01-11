using API.Models;
using API.Models.Enums;
using AutoMapper;

namespace API.Mapping;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Domain.Match.MatchAggregate, Match>()
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src =>
                src.Players
                .Where(player => src.PlayerTeams.ContainsKey(player.SteamId))
                .Select(player => new Player
                {
                    SteamId = player.SteamId,
                    Name = player.Name,
                    TeamName = src.PlayerTeams[player.SteamId],
                    Kills = src.Rounds.Sum(round => round.Kills.Count(k => k.KillerSteamId == player.SteamId)),
                    Deaths = src.Rounds.Sum(round => round.Kills.Count(k => k.VictimSteamId == player.SteamId)),
                    WeaponWithMostKills = src.Rounds
                        .SelectMany(round => round.Kills)
                        .Where(k => k.KillerSteamId == player.SteamId)
                        .GroupBy(k => k.Weapon)
                        .OrderByDescending(killsByWeapon => killsByWeapon.Count())
                        .Select(killsByWeapon => (Weapon)killsByWeapon.Key)
                        .FirstOrDefault()
                })
                .ToList()
            ))
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src =>
                src.Players
                .Where(p => src.PlayerTeams.ContainsKey(p.SteamId))
                .GroupBy(p => src.PlayerTeams[p.SteamId])
                .Select(playersByTeamName => new Team
                {
                    Name = playersByTeamName.Key,
                    PlayerSteamIds = playersByTeamName.Select(p => p.SteamId).ToList(),
                    Kills = src.Rounds.Sum(r => r.Kills.Count(k => playersByTeamName.Select(p => p.SteamId).Contains(k.KillerSteamId))),
                    Deaths = src.Rounds.Sum(r => r.Kills.Count(k => playersByTeamName.Select(p => p.SteamId).Contains(k.VictimSteamId)))
                })
                .ToList()
            ));

        CreateMap<Domain.Round.RoundEntity, Round>()
            .ForMember(dest => dest.PlayerSideBySteamId, opt => opt.MapFrom(src => src.PlayerSides));

        CreateMap<Domain.Round.RoundWin, RoundWin>();

        CreateMap<Domain.Player.PlayerEntity, Player>();

        CreateMap<Domain.Round.KillEvent, KillEvent>();

        CreateMap<Domain.Round.BombEvent, BombEvent>();

        CreateMap<Domain.Round.BombAction, BombAction>();

        CreateMap<Domain.Match.Map, Map>();

        CreateMap<Domain.Round.RoundWinReason, RoundWinReason>();

        CreateMap<Domain.Round.Side, Side>();

    }
}