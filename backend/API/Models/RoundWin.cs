using API.Models.Enums;

namespace API.Models;

public class RoundWin
{
    public string TeamName { get; set; }
    public Side Side { get; set; }
    public RoundWinReason Reason { get; set; }
}