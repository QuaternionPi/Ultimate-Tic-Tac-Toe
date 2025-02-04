using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Tile : ICell<Tile>
{
    public Player.Token? Owner { get; }
    public bool Placeable { get => Owner == null; }
    public Tile(Player.Token? player)
    {
        Owner = player;
    }
    public Tile Place(Player.Token? player)
    {
        return new Tile(player);
    }
}