namespace Application.Models;

public class PlayerTeamSwitch
{
    public Player Player { get; set; }
    public string FromTeam { get; set; }
    public string ToTeam { get; set; }
}