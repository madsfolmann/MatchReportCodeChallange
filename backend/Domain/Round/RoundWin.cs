namespace Domain.Round;

public class RoundWin(string teamName, RoundWinReason reason, Side side)
{
    public string TeamName { get; private set; } = teamName;
    public RoundWinReason Reason { get; private set; } = reason;
    public Side Side { get; private set; } = side;
}