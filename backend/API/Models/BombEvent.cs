using API.Models.Enums;

namespace API.Models;

public class BombEvent
{
    public string PlayerSteamId { get; set; }
    public BombAction Action { get; set; }
    public Bombsite? Bombsite { get; set; }
    public DateTime Timestamp { get; set; }
}