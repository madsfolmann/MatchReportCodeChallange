using Application.Handlers;
using Application.Test.Helpers;
using Domain.Match;
using Domain.Round;
using Xunit;

namespace Application.Test;

public class LogLinesHandlerShould
{
    private const string TeamVitality = "TeamVitality";
    private const string NAVI_GGBET = "NAVI GGBET";

    [Fact]
    public async Task HandleFullMatchCorrectly()
    {
        var matchId = Guid.NewGuid();
        var handler = new LogLinesHandler();

        var logLines = LogFile.LoadEmbeddedLogLines("TestData.full_match_log.txt");

        var match = await handler.HandleLogsLines(logLines);

        Assert.NotNull(match);

        Assert.Equal(Map.Nuke, match.Map);

        var naviPlayers = new[] { "STEAM_1:0:92970669", "STEAM_1:0:80477379", "STEAM_1:0:143170874", "STEAM_1:1:41889689", "STEAM_1:1:36968273" };
        var vitalityPlayers = new[] { "STEAM_1:1:14739219", "STEAM_1:1:60631591", "STEAM_1:1:76700232", "STEAM_1:1:22851120", "STEAM_1:1:23327283" };

        foreach (var steamId in naviPlayers)
        {
            Assert.Equal(NAVI_GGBET, match.PlayerTeams[steamId]);
        }

        foreach (var steamId in vitalityPlayers)
        {
            Assert.Equal(TeamVitality, match.PlayerTeams[steamId]);
        }

        Assert.Equal(new DateTime(2021, 11, 28, 20, 41, 11), match.StartedAt);
        Assert.Equal(new DateTime(2021, 11, 28, 21, 30, 17), match.EndedAt);
        Assert.Equal(22, match.Rounds.Count);

        var secondRound = match.Rounds.ElementAt(1);
        Assert.Equal(new DateTime(2021, 11, 28, 20, 43, 36), secondRound.StartedAt);
        Assert.Equal(new DateTime(2021, 11, 28, 20, 45, 36), secondRound.EndedAt);
        Assert.Equal(7, secondRound.Kills.Count);
        Assert.Equal(RoundWinReason.BombDefused, secondRound.Winner?.Reason);

        Assert.Equal(3, match.Rounds.Count(r => r.Winner?.Reason == RoundWinReason.BombDefused));
        Assert.Equal(3, match.Rounds.Count(r => r.Winner?.Reason == RoundWinReason.BombExploded));
        Assert.Equal(16, match.Rounds.Count(r => r.Winner?.Reason == RoundWinReason.EliminatedAllOpponents));

        var secondRoundFirstKill = secondRound.Kills.OrderBy(k => k.Timestamp).First();
        Assert.Equal("STEAM_1:1:60631591", secondRoundFirstKill.KillerSteamId);
        Assert.Equal("STEAM_1:0:80477379", secondRoundFirstKill.VictimSteamId);
        Assert.Equal(Weapon.M4A1Silencer, secondRoundFirstKill.Weapon);
        Assert.Equal(new DateTime(2021, 11, 28, 20, 44, 21), secondRoundFirstKill.Timestamp);
        Assert.False(secondRoundFirstKill.IsHeadshot);

        var lastRound = match.Rounds.Last();
        var lastRoundSecondKill = lastRound.Kills.OrderBy(k => k.Timestamp).ElementAt(1);
        Assert.Equal("STEAM_1:0:143170874", lastRoundSecondKill.KillerSteamId);
        Assert.Equal("STEAM_1:1:22851120", lastRoundSecondKill.VictimSteamId);
        Assert.Equal(Weapon.Famas, lastRoundSecondKill.Weapon);
        Assert.True(lastRoundSecondKill.IsHeadshot);
        Assert.Equal(new DateTime(2021, 11, 28, 21, 30, 0), lastRoundSecondKill.Timestamp);

        var firstRound = match.Rounds.ElementAt(0);
        Assert.Equal(RoundWinReason.EliminatedAllOpponents, firstRound.Winner?.Reason);
        var vitalityPlayerNames = new[] { "misutaaa", "shox ", "apEX", "ZywOo", "Kyojin" };
        var naviPlayerNames = new[] { "s1mple", "electronic", "Boombl4", "Perfecto", "b1t" };

        foreach (var playerName in vitalityPlayerNames)
        {
            var player = match.Players.FirstOrDefault(p => p.Name == playerName);
            Assert.NotNull(player);
            Assert.True(secondRound.PlayerSides.ContainsKey(player!.SteamId));
            Assert.Equal(Side.CounterTerrorist, secondRound.PlayerSides[player.SteamId]);
        }

        foreach (var playerName in naviPlayerNames)
        {
            var player = match.Players.FirstOrDefault(p => p.Name == playerName);
            Assert.NotNull(player);
            Assert.True(secondRound.PlayerSides.ContainsKey(player!.SteamId));
            Assert.Equal(Side.Terrorist, secondRound.PlayerSides[player.SteamId]);
        }

        var secondRoundBombEvents = secondRound.BombEvents;
        var defusedBombEvent = secondRoundBombEvents.FirstOrDefault(e => e.Action == BombAction.Defused);
        Assert.NotNull(defusedBombEvent);
        Assert.Equal("STEAM_1:1:22851120", defusedBombEvent!.PlayerSteamId);

        var beginPlantBombEvent = secondRoundBombEvents.FirstOrDefault(e => e.Action == BombAction.BeginPlant);
        Assert.NotNull(beginPlantBombEvent);
        Assert.Equal("STEAM_1:0:143170874", beginPlantBombEvent!.PlayerSteamId);

        Assert.Equal(RoundWinReason.BombExploded, match.Rounds.ElementAt(6).Winner?.Reason);

        foreach (var playerName in vitalityPlayerNames)
        {
            var player = match.Players.FirstOrDefault(p => p.Name == playerName);
            Assert.NotNull(player);
            Assert.True(lastRound.PlayerSides.ContainsKey(player!.SteamId));
            Assert.Equal(Side.Terrorist, lastRound.PlayerSides[player.SteamId]);
        }

        foreach (var playerName in naviPlayerNames)
        {
            var player = match.Players.FirstOrDefault(p => p.Name == playerName);
            Assert.NotNull(player);
            Assert.True(lastRound.PlayerSides.ContainsKey(player!.SteamId));
            Assert.Equal(Side.CounterTerrorist, lastRound.PlayerSides[player.SteamId]);
        }

        var firstRoundTeams = firstRound.TeamSides;
        Assert.Equal(TeamVitality, firstRoundTeams[Side.CounterTerrorist]);
        Assert.Equal(NAVI_GGBET, firstRoundTeams[Side.Terrorist]);

        var fifteenthRoundTeams = match.Rounds.ElementAt(15).TeamSides;
        Assert.Equal(TeamVitality, fifteenthRoundTeams[Side.Terrorist]);
        Assert.Equal(NAVI_GGBET, fifteenthRoundTeams[Side.CounterTerrorist]);

        var lastRoundTeams = lastRound.TeamSides;
        Assert.Equal(TeamVitality, lastRoundTeams[Side.Terrorist]);
        Assert.Equal(NAVI_GGBET, lastRoundTeams[Side.CounterTerrorist]);

        foreach (var round in match.Rounds)
        {
            Assert.Equal(10, round.PlayerSides.Count);
            Assert.Equal(5, round.PlayerSides.Values.Count(side => side == Side.CounterTerrorist));
            Assert.Equal(5, round.PlayerSides.Values.Count(side => side == Side.Terrorist));
        }

        Assert.Equal(TeamVitality, match.WinnerTeamName);

        var vitalityWonRounds = new[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 12, 13, 17, 18, 19, 20, 21 };
        foreach (var roundIndex in vitalityWonRounds)
        {
            Assert.Equal(TeamVitality, match.Rounds.ElementAt(roundIndex).Winner!.TeamName);
        }

        var naviWonRounds = new[] { 6, 10, 11, 14, 15, 16 };
        foreach (var roundIndex in naviWonRounds)
        {
            Assert.Equal(NAVI_GGBET, match.Rounds.ElementAt(roundIndex).Winner!.TeamName);
        }
    }
}