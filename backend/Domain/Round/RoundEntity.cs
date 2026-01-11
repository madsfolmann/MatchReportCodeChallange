namespace Domain.Round;

public class RoundEntity(int number, DateTime startedAt)
{
    public int Number { get; private set; } = number;

    public DateTime StartedAt { get; private set; } = startedAt;
    public DateTime? EndedAt { get; private set; }

    public RoundWin? Winner { get; private set; }

    private readonly List<KillEvent> _kills = [];
    public IReadOnlyCollection<KillEvent> Kills => _kills;

    private readonly List<BombEvent> _bombEvents = [];
    public IReadOnlyCollection<BombEvent> BombEvents => _bombEvents;

    private readonly Dictionary<string, Side> _sideByPlayer = [];
    public IReadOnlyDictionary<string, Side> PlayerSides => _sideByPlayer;

    private readonly Dictionary<Side, string> _teamsBySide = [];
    public IReadOnlyDictionary<Side, string> TeamSides => _teamsBySide;

    public void SetTeamSide(string teamName, Side side)
    {
        _teamsBySide[side] = teamName;
    }

    public void EndRound(DateTime timestamp)
    {
        EndedAt = timestamp;
    }

    public void RegisterKill(KillEvent kill)
    {
        _kills.Add(kill);
    }

    public void SetPlayerSide(string steamId, Side side)
    {
        _sideByPlayer[steamId] = side;
    }

    public void RegisterBombEvent(BombEvent bombEvent)
    {
        _bombEvents.Add(bombEvent);
    }

    public void CalculateWinner()
    {
        var bombPlanted = _bombEvents.Any(b => b.Action == BombAction.Planted);
        var bombDefused = _bombEvents.Any(b => b.Action == BombAction.Defused);

        Side? winningSide = null;
        RoundWinReason reason = RoundWinReason.Timeout;

        var aliveT = CountAlivePlayers(Side.Terrorist);
        var aliveCT = CountAlivePlayers(Side.CounterTerrorist);


        if (bombDefused)
        {
            winningSide = Side.CounterTerrorist;
            reason = RoundWinReason.BombDefused;
        }
        else if (aliveT == 0 || aliveCT == 0)
        {
            if (bombDefused) System.Diagnostics.Debugger.Break();

            winningSide = aliveT == 0 ? Side.CounterTerrorist : Side.Terrorist;
            reason = RoundWinReason.EliminatedAllOpponents;
        }
        else if (bombPlanted && !bombDefused)
        {
            winningSide = Side.Terrorist;
            reason = RoundWinReason.BombExploded;
        }
        else
        {
            winningSide = Side.CounterTerrorist;
            reason = RoundWinReason.Timeout;
        }

        if (winningSide.HasValue)
        {
            Winner = new RoundWin(TeamSides[winningSide.Value], reason, winningSide.Value);
        }
    }

    private int CountAlivePlayers(Side side)
    {
        var startCount = _sideByPlayer.Count(kvp => kvp.Value == side);
        var deadCount = _kills.Count(k => _sideByPlayer[k.VictimSteamId] == side);
        return startCount - deadCount;
    }
}