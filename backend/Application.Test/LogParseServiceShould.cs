using Application.Models;
using Application.Services;
using Xunit;

namespace Application.Test;

public class LogParseServiceShould
{
    [Fact]
    public void SplitLogLineIntoTimestampAndPayload()
    {
        // Given
        string logLine = """
        12/31/2023 - 23:59:59: World triggered "Match_Start" on "de_nuke"
        """;

        // When
        var (timestamp, payload) = LogParserService.Split(logLine);

        // Then
        Assert.Equal(new DateTime(2023, 12, 31, 23, 59, 59), timestamp);
        Assert.Equal("""
        World triggered "Match_Start" on "de_nuke"
        """, payload);
    }

    [Fact]
    public void ParseMatchStart()
    {
        // Given
        string logLine = """
        World triggered "Match_Start" on "de_nuke"
        """;

        // When
        var result = LogParserService.ParseMatchStart(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equal("de_nuke", result.MapName);
    }

    [Fact]
    public void ParseGameOver()
    {
        // Given
        string logLine = """
        Game Over: competitive 1092904694 de_nuke score 6:16 after 50 min
        """;

        // When
        var result = LogParserService.ParseGameOver(logLine);

        // Then
        Assert.NotNull(result);
    }

    [Fact]
    public void ParseRoundStart()
    {
        // Given
        string logLine = """
        World triggered "Round_Start"
        """;

        // When
        var result = LogParserService.ParseRoundStart(logLine);

        // Then
        Assert.NotNull(result);
    }

    [Fact]
    public void ParseRoundEnd()
    {
        // Given
        string logLine = """
        World triggered "Round_End"
        """;

        // When
        var result = LogParserService.ParseRoundEnd(logLine);

        // Then
        Assert.NotNull(result);
    }

    [Fact]
    public void ParseKillWithHeadshot()
    {
        // Given
        string logLine = """
        "b1t<35><STEAM_1:0:143170874><TERRORIST>" [-455 -1040 -416] killed "ZywOo<26><STEAM_1:1:76700232><CT>" [220 -1250 -352] with "ak47" (headshot)
        """;

        // When
        var result = LogParserService.ParseKill(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "b1t", SteamId = "STEAM_1:0:143170874", Team = "TERRORIST" }, result.Killer);
        Assert.Equivalent(new Player { Name = "ZywOo", SteamId = "STEAM_1:1:76700232", Team = "CT" }, result.Victim);
        Assert.Equal("ak47", result.Weapon);
        Assert.True(result.IsHeadshot);
    }

    [Fact]
    public void ParseKillWithoutHeadshot()
    {
        // Given
        string logLine = """
        "b1t<35><STEAM_1:0:143170874><TERRORIST>" [-455 -1040 -416] killed "ZywOo<26><STEAM_1:1:76700232><CT>" [220 -1250 -352] with "ak47"
        """;

        // When
        var result = LogParserService.ParseKill(logLine);

        // Then
        Assert.NotNull(result);
        Assert.False(result.IsHeadshot);
    }

    [Fact]
    public void ParsePlayerTeamSwitch()
    {
        // Given
        string logLine = """
        "misutaaa<24><STEAM_1:1:60631591>" switched from team <Unassigned> to <CT>
        """;

        // When
        var result = LogParserService.ParsePlayerTeamSwitch(logLine);
        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "misutaaa", SteamId = "STEAM_1:1:60631591", Team = null }, result.Player);
        Assert.Equal("Unassigned", result.FromTeam);
        Assert.Equal("CT", result.ToTeam);
    }

    [Fact]
    public void ParseTeamPlaying()
    {
        // Given
        string logLine = """
        Team playing "CT": TeamVitality
        """;

        // When
        var result = LogParserService.ParseTeamPlayingSide(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equal("TeamVitality", result.TeamName);
        Assert.Equal("CT", result.Side);
    }

    [Fact]
    public void ParseNullTeamPlaying()
    {
        // Given
        string logLine = """
        MatchStatus: Team playing "CT": NAVI GGBET
        """;

        // When
        var result = LogParserService.ParseTeamPlayingSide(logLine);

        // Then
        Assert.Null(result);
    }

    [Fact]
    public void ParseBombEvent_Bomb_Begin_Plant()
    {
        // Given
        string logLine = """
        "ZywOo<26><STEAM_1:1:76700232><TERRORIST>" triggered "Bomb_Begin_Plant" at bombsite B
        """;

        // When
        var result = LogParserService.ParseBombEvent(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "ZywOo", SteamId = "STEAM_1:1:76700232", Team = "TERRORIST" }, result.Player);
        Assert.Equal("Bomb_Begin_Plant", result.Action);
        Assert.Equal("B", result.Bombsite);
    }

    [Fact]
    public void ParseBombEvent_Begin_Bomb_Defuse_With_Kit()
    {
        // Given
        string logLine = """
        "Kyojin<34><STEAM_1:1:22851120><CT>" triggered "Begin_Bomb_Defuse_With_Kit"
        """;

        // When
        var result = LogParserService.ParseBombEvent(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "Kyojin", SteamId = "STEAM_1:1:22851120", Team = "CT" }, result.Player);
        Assert.Equal("Begin_Bomb_Defuse_With_Kit", result.Action);
        Assert.Null(result.Bombsite);
    }

    [Fact]
    public void ParseBombEvent_Defused_The_Bomb()
    {
        // Given
        string logLine = """
        "Kyojin<34><STEAM_1:1:22851120><CT>" triggered "Defused_The_Bomb"
        """;

        // When
        var result = LogParserService.ParseBombEvent(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "Kyojin", SteamId = "STEAM_1:1:22851120", Team = "CT" }, result.Player);
        Assert.Equal("Defused_The_Bomb", result.Action);
        Assert.Null(result.Bombsite);
    }

    [Fact]
    public void ParseBombEvent_Planted_The_Bomb()
    {
        // Given
        string logLine = """
        "ZywOo<26><STEAM_1:1:76700232><TERRORIST>" triggered "Planted_The_Bomb" at bombsite B
        """;

        // When
        var result = LogParserService.ParseBombEvent(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "ZywOo", SteamId = "STEAM_1:1:76700232", Team = "TERRORIST" }, result.Player);
        Assert.Equal("Planted_The_Bomb", result.Action);
        Assert.Equal("B", result.Bombsite);
    }

    [Fact]
    public void ParseBombEvent_Dropped_The_Bomb()
    {
        // Given
        string logLine = """
        "electronic<31><STEAM_1:1:41889689><TERRORIST>" triggered "Dropped_The_Bomb"
        """;

        // When
        var result = LogParserService.ParseBombEvent(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "electronic", SteamId = "STEAM_1:1:41889689", Team = "TERRORIST" }, result.Player);
        Assert.Equal("Dropped_The_Bomb", result.Action);
        Assert.Null(result.Bombsite);
    }

    [Fact]
    public void ParseBombEvent_Got_The_Bomb()
    {
        // Given
        string logLine = """
        "electronic<31><STEAM_1:1:41889689><TERRORIST>" triggered "Got_The_Bomb"
        """;

        // When
        var result = LogParserService.ParseBombEvent(logLine);

        // Then
        Assert.NotNull(result);
        Assert.Equivalent(new Player { Name = "electronic", SteamId = "STEAM_1:1:41889689", Team = "TERRORIST" }, result.Player);
        Assert.Equal("Got_The_Bomb", result.Action);
        Assert.Null(result.Bombsite);
    }
}