using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Tile : ICell<Tile>
{
    [JsonInclude]
    public Player.Token? Owner { get; }
    [JsonIgnore]
    public bool Placeable { get => Owner == null; }
    [JsonConstructor]
    public Tile(Player.Token? owner)
    {
        Owner = owner;
    }
    public Tile Place(Player.Token? owner)
    {
        return new Tile(owner);
    }
}