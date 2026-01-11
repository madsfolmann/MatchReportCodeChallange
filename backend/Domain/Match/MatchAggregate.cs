using Domain.Player;
using Domain.Round;

namespace Domain.Match;

public sealed class MatchAggregate
{
    public MatchAggregate() { }

    public Map Map { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public string? WinnerTeamName { get; private set; }

    private RoundEntity? _currentRound;

    private readonly List<RoundEntity> _rounds = [];
    public IReadOnlyCollection<RoundEntity> Rounds => _rounds.AsReadOnly();

    private readonly Dictionary<Side, string> _teamsSideAtMatchStart = [];

    private readonly Dictionary<string, string> _playerTeams = [];
    public IReadOnlyDictionary<string, string> PlayerTeams => _playerTeams;

    private readonly Dictionary<string, PlayerEntity> _playersBySteamId = [];
    public IReadOnlyCollection<PlayerEntity> Players => _playersBySteamId.Values;


    public void RegisterMatchStart(Map map, DateTime timestamp)
    {
        Map = map;
        StartedAt = timestamp;
    }

    public void RegisterMatchEnd(DateTime timestamp)
    {
        if (StartedAt == null) throw new InvalidOperationException("Cannot end a match that hasn't started.");
        EndedAt = timestamp;
        CalculateMatchWinner();
    }

    public void SetTeam(Side side, string teamName)
    {
        _teamsSideAtMatchStart[side] = teamName;
    }

    public void RegisterPlayer(string steamId, string name)
    {
        if (_playersBySteamId.ContainsKey(steamId)) return;
        _playersBySteamId[steamId] = new PlayerEntity(name, steamId);
    }

    public void SwitchPlayerSide(string steamId, Side side)
    {
        if (_playersBySteamId.TryGetValue(steamId, out var player))
        {
            player.SwitchSide(side);
            return;
        }

        throw new InvalidOperationException($"Player with SteamID {steamId} not found.");
    }

    public void StartRound(DateTime timestamp)
    {
        _currentRound = new RoundEntity(_rounds.Count + 1, timestamp);

        foreach (var player in _playersBySteamId.Values)
        {
            if (player.Side is not Side.Terrorist and not Side.CounterTerrorist) continue;

            _currentRound.SetPlayerSide(player.SteamId, player.Side);

            if (!_playerTeams.TryGetValue(player.SteamId, out var teamName))
            {
                _playerTeams[player.SteamId] = _teamsSideAtMatchStart[player.Side];
            }

            _currentRound.SetTeamSide(_playerTeams[player.SteamId], player.Side);
        }
    }

    public void EndRound(DateTime timestamp)
    {
        if (_currentRound == null) throw new InvalidOperationException("No round is currently active.");

        _currentRound.EndRound(timestamp);
        _currentRound.CalculateWinner();
        _rounds.Add(_currentRound);
        _currentRound = null;
    }

    public void RegisterBombEvent(string playerSteamId, BombAction action, BombSite? bombsite, DateTime timestamp)
    {
        if (_currentRound == null) return;

        var player = _playersBySteamId[playerSteamId];

        var isTerroristAction = action is BombAction.Planted or BombAction.BeginPlant or BombAction.Dropped or BombAction.Recieved;
        var isValidAction = (player.Side == Side.Terrorist && isTerroristAction) || (player.Side == Side.CounterTerrorist && !isTerroristAction);

        if (!isValidAction) throw new InvalidOperationException($"Player {player.Name} ({player.Side}) cannot perform {action} action.");

        _currentRound.RegisterBombEvent(new BombEvent(playerSteamId, action, bombsite, timestamp));
    }

    public void RegisterKill(string killerSteamId, string victimSteamId, Weapon weapon, bool isHeadshot, DateTime timestamp)
    {
        if (_currentRound == null) return;

        _currentRound.RegisterKill(new KillEvent(killerSteamId, victimSteamId, weapon, isHeadshot, timestamp));
    }

    private void CalculateMatchWinner()
    {
        WinnerTeamName = _rounds
            .Where(r => r.Winner != null)
            .GroupBy(r => r.Winner!.TeamName)
            .MaxBy(winsByTeamName => winsByTeamName.Count())
            ?.Key;
    }
}
