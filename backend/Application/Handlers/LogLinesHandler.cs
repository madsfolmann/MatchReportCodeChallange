using Application.Services;
using Application.Mapping;
using Domain.Match;

namespace Application.Handlers;

public class LogLinesHandler : ILogLinesHandler
{
    public async Task<MatchAggregate> HandleLogsLines(IEnumerable<string> logLines)
    {
        var match = new MatchAggregate();

        foreach (var line in logLines)
        {
            var parsed = LogParserService.Split(line);

            var (timestamp, payload) = parsed;

            if (LogParserService.ParseKill(payload) is { } killEvent)
            {
                match.RegisterKill(
                    killEvent.Killer.SteamId,
                    killEvent.Victim.SteamId,
                    CustomMapper.GameLogWeaponToDomainWeapon(killEvent.Weapon),
                    killEvent.IsHeadshot,
                    timestamp
                );
                continue;
            }

            if (LogParserService.ParsePlayerTeamSwitch(payload) is { } playerSwitchTeamEvent)
            {
                match.RegisterPlayer(playerSwitchTeamEvent.Player.SteamId, playerSwitchTeamEvent.Player.Name);
                match.SwitchPlayerSide(
                    playerSwitchTeamEvent.Player.SteamId,
                    CustomMapper.GameLogSideToDomainSide(playerSwitchTeamEvent.ToTeam)
                );
                continue;
            }

            if (LogParserService.ParseBombEvent(payload) is { } bombEvent)
            {
                match.RegisterBombEvent(
                    bombEvent.Player.SteamId,
                    CustomMapper.GameLogBombActionToDomainBombAction(bombEvent.Action),
                    bombEvent.Bombsite != null ? CustomMapper.GameLogBombSiteToDomainBombSite(bombEvent.Bombsite) : null,
                    timestamp
                );
                continue;
            }

            if (LogParserService.ParseRoundEnd(payload) is not null)
            {
                match.EndRound(timestamp);
                continue;
            }

            if (LogParserService.ParseRoundStart(payload) is not null)
            {
                match.StartRound(timestamp);
                continue;
            }

            if (LogParserService.ParseTeamPlayingSide(payload) is { } teamPlayingSideEvent)
            {
                match.SetTeam(
                    CustomMapper.GameLogSideToDomainSide(teamPlayingSideEvent.Side),
                    teamPlayingSideEvent.TeamName
                );
                continue;
            }

            if (LogParserService.ParseMatchStart(payload) is { } matchStartEvent)
            {
                match.RegisterMatchStart(CustomMapper.GameLogMapnameToDomainMapname(matchStartEvent.MapName), timestamp);
                continue;
            }

            if (LogParserService.ParseGameOver(payload) is not null)
            {
                match.RegisterMatchEnd(timestamp);
                continue;
            }
        }

        return match;
    }
}