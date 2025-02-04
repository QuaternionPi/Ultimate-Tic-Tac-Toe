using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Tile : ICell<Tile>
{
    [JsonInclude]
    public Player.Token? Owner { get; }
    public bool Placeable { get; }
    public Tile(Player.Token? player)
    {
        Owner = player;
        Placeable = player == null;
    }
    public Tile Place(Player.Token? player)
    {
        return new Tile(player);
    }
}