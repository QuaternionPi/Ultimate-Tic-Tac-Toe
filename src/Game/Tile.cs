using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Tile : ICell<Tile>
{
    [JsonInclude]
    public Player? Player { get; }
    public bool Placeable { get; }
    public Tile(Player? player)
    {
        Player = player;
        Placeable = player == null;
    }
    public Tile Place(Player? player)
    {
        return new Tile(player);
    }
}