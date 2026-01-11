namespace Application.Models;

public class Kill
{
    public Player Killer { get; set; }
    public Player Victim { get; set; }
    public string Weapon { get; set; }
    public bool IsHeadshot { get; set; }
}