namespace API.Models;

public class Team
{
    public string Name { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public List<string> PlayerSteamIds { get; set; } = [];
    public double Kd => Deaths == 0 ? Kills : (double)Kills / Deaths;
}