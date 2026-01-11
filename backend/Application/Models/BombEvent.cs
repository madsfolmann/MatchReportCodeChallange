namespace Application.Models;

public class BombEvent
{
    public Player Player { get; set; }
    public string Action { get; set; }
    public string? Bombsite { get; set; }
}