using System.Text.RegularExpressions;
using Application.Models;

namespace Application.Services;

public static class LogParserService
{
    public static (DateTime Timestamp, string Payload) Split(string line)
    {
        var match = Regex.Match(
            line,
            @"^(?<date>\d+/\d+/\d+) - (?<time>\d+:\d+:\d+): (?<payload>.+)$"
        );

        if (!match.Success) throw new ArgumentException("Log line is not in the correct format", nameof(line));

        var timestamp = DateTime.ParseExact(
            $"{match.Groups["date"].Value} {match.Groups["time"].Value}",
            "MM/dd/yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture
        );

        return (timestamp, match.Groups["payload"].Value);
    }

    public static MatchStart ParseMatchStart(string payload)
    {
        return Parse(
            payload,
            @"World triggered \""Match_Start\"" on \""(?<map>.+)\""",
            m => new MatchStart
            {
                MapName = m.Groups["map"].Value
            }
        );
    }

    public static object ParseGameOver(string payload)
    {
        return Parse(
            payload,
            @"Game Over: (?<type>\w+) \d+ (?<map>\w+) score (?<scoreA>\d+):(?<scoreB>\d+) after (?<duration>\d+) min$",
            m => new { }
        );
    }

    public static object ParseRoundStart(string payload)
    {
        return Parse(
            payload,
            @"World triggered \""Round_Start\""",
            m => new { }
        );
    }

    public static object ParseRoundEnd(string payload)
    {
        return Parse(
            payload,
            @"World triggered \""Round_End\""",
            m => new { }
        );
    }

    public static Kill ParseKill(string payload)
    {
        return Parse(
            payload,
            "^(?<killer>\".+?\") \\[.*?\\] killed (?<victim>\".+?\") \\[.*?\\] with \"(?<weapon>.+?)\"(?: \\(headshot\\))?",
            m => new Kill
            {
                Killer = ParsePlayerBlock(m.Groups["killer"].Value),
                Victim = ParsePlayerBlock(m.Groups["victim"].Value),
                Weapon = m.Groups["weapon"].Value,
                IsHeadshot = payload.Contains("(headshot)")
            }
        );
    }

    public static PlayerTeamSwitch ParsePlayerTeamSwitch(string payload)
    {
        return Parse(
            payload,
            "^(?<player>\".+?\") switched from team <(?<fromTeam>[^>]+)> to <(?<toTeam>[^>]+)>$",
            m => new PlayerTeamSwitch
            {
                Player = ParsePlayerBlock(m.Groups["player"].Value),
                FromTeam = m.Groups["fromTeam"].Value,
                ToTeam = m.Groups["toTeam"].Value
            }
        );
    }

    public static BombEvent ParseBombEvent(string payload)
    {
        return Parse(
            payload,
            "^(?<player>\".+?\") triggered \"(?<action>.+?)\"(?: at bombsite (?<bombsite>\\w+))?$",
            m => new BombEvent
            {
                Player = ParsePlayerBlock(m.Groups["player"].Value),
                Action = m.Groups["action"].Value,
                Bombsite = m.Groups["bombsite"].Success ? m.Groups["bombsite"].Value : null
            }
        );
    }

    public static TeamPlaying ParseTeamPlayingSide(string payload)
    {
        return Parse(
            payload,
            @"^Team playing \""(?<side>.+)\"": (?<team>.+)$",
            m => new TeamPlaying
            {
                TeamName = m.Groups["team"].Value,
                Side = m.Groups["side"].Value
            }
        );
    }

    private static Player ParsePlayerBlock(string payload)
    {
        var m = Regex.Match(
            payload,
            "^\"(?<name>.+?)<(?<slot>\\d+)><(?<steam>[^>]+)>(?:<(?<team>[^>]+)>)?\"$"
        );

        if (!m.Success) throw new ArgumentException("Player block is not in the correct format", nameof(payload));

        return new Player
        {
            Name = m.Groups["name"].Value,
            SteamId = m.Groups["steam"].Value,
            Team = m.Groups["team"].Success ? m.Groups["team"].Value : null,
        };
    }

    private static T Parse<T>(
        string payload,
        string pattern,
        Func<Match, T> factory
    ) where T : class
    {
        var match = Regex.Match(payload, pattern);
        if (!match.Success) return null;

        return factory(match);
    }
}